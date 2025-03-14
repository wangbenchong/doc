# 基础

在Unity中，使用Asset Bundles是一种高效管理游戏资源的方法，尤其是在需要动态加载资源的情况下。Asset Bundles允许你将游戏资源打包成独立的文件，并在运行时按需加载它们。下面是如何制作、加载和卸载Asset Bundles的详细步骤和代码示例。

## 制作Bundle文件

首先，你需要使用Unity的Build Pipeline来创建Asset Bundles。这通常是在Editor脚本中完成的。按照惯例bundle的名字不能出现大写字母。

在游戏运行时，不能直接从Assets目录加载AssetBundle，因为Assets目录的内容在打包时会被整合到最终的游戏包中，而不是作为独立的文件存在。
与Assets目录不同，StreamingAssets目录中的内容在打包时会原封不动地被打入最终的游戏包中，不会进行压缩或加密，包括AssetBundle。
StreamingAssets目录是只读的，即游戏运行时不能向其中写入数据。

那么关于生成路径，使用`Application.dataPath`而不是`Application.streamingAssetsPath`有几个原因：

- 代码的目的是在Unity编辑器中构建AssetBundles，而不是在运行时加载它们。因此，它需要将AssetBundles输出到一个编辑器可以访问的目录，这个目录通常是项目的根目录或附近的某个目录。
- 使用`Application.streamingAssetsPath`作为输出目录在编辑器中可能没有太大意义，因为编辑器已经能够访问`Assets`目录中的任何资源。而且，在编辑器中构建的AssetBundles通常是为了测试或分发到另一个Unity项目，而不是直接用于运行时的加载。
- 在实际的游戏发布过程中，你可能会将AssetBundles构建到一个单独的目录（不是`StreamingAssets`），然后将它们打包到游戏的分发包中，并在运行时根据需要下载或加载它们。

因此，在构建AssetBundles的上下文中，使用`Application.dataPath`（或更常见的是，一个相对于项目根目录的自定义路径）作为输出目录是更合适的选择。然而，在运行时加载这些AssetBundles时，你可能会使用`Application.persistentDataPath`（对于用户生成的数据）或`Application.streamingAssetsPath`（对于打包在应用程序中的资源）作为加载路径。

关于打包时把bundle生成在StreamingAssets，可以这么做：

- 打开Unity编辑器的“Build”菜单，选择“Build AssetBundles”或使用自定义的编辑器脚本来启动打包过程。
- 在弹出的“Build AssetBundles”窗口中，你可以设置输出路径、打包选项等。
- 确保将输出路径设置为指向你的项目的StreamingAssets目录，或者任何你希望存放AssetBundle的目录。例如，你可以设置输出路径为`Assets/StreamingAssets`（在编辑器中）或`Application.streamingAssetsPath`（在运行时脚本中，但注意这在编辑器脚本中可能不适用，因为`Application.streamingAssetsPath`在编辑器中指向的是项目的StreamingAssets目录，而在构建后的应用程序中指向的是不同的位置）。

以下是一个简单的编辑器脚本示例，它展示了如何遍历`Assets`文件夹中的资源，将它们打包成AssetBundle，并将这些bundle输出到项目的`StreamingAssets`目录（或你指定的任何其他目录）：

```csharp
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
 
public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        // 设置AssetBundle的输出路径
        string outputPath = Path.Combine(Application.dataPath, "../Assets/StreamingAssets");
 
        // 确保输出目录存在
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
 
        // 获取所有需要打包的资源路径（这里以Assets文件夹下的所有资源为例）
        string[] assetPaths = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories);
        List<AssetBundleBuild> bundleBuildList = new List<AssetBundleBuild>();
 
        // 遍历资源路径，并添加到bundleBuildList中
        foreach (string assetPath in assetPaths)
        {
            // 跳过非Unity资源文件（如meta文件、cs文件等）
            if (!assetPath.EndsWith(".meta") && !assetPath.EndsWith(".cs") && !assetPath.Contains("Library/") && !assetPath.Contains("Temp/") && !assetPath.Contains("obj/") && !assetPath.Contains("bin/"))
            {
                // 获取相对于Assets文件夹的路径
                string relativePath = assetPath.Substring(Application.dataPath.Length - "Assets".Length);
 
                // 假设每个资源都属于一个以其文件夹命名的AssetBundle（这里需要根据你的实际需求来调整）
                string bundleName = Path.GetDirectoryName(relativePath).Replace("\\", "/").TrimStart('/');
                if (string.IsNullOrEmpty(bundleName))
                {
                    bundleName = "default";
                }
 
                // 创建一个新的AssetBundleBuild对象，并添加到列表中
                var assetBundleBuild = new AssetBundleBuild
                {
                    assetBundleName = bundleName,
                    assetNames = new string[] { relativePath.Replace("\\", "/") }
                };
                bundleBuildList.Add(assetBundleBuild);
            }
        }
 
        // 使用BuildPipeline.BuildAssetBundles来打包资源
        BuildPipeline.BuildAssetBundles(outputPath, bundleBuildList.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
 
        // 提示用户打包完成
        Debug.Log("AssetBundles have been built to: " + outputPath);
    }
}
```

**重要说明**：

1. **路径处理**：这个脚本假设所有资源都位于`Assets`文件夹下，并且每个资源都属于一个以其文件夹命名的AssetBundle。你可能需要根据你的项目结构来调整这部分逻辑。
2. **打包选项**：`BuildAssetBundleOptions.None`表示使用默认的打包选项。你可以根据需要添加其他选项，如`BuildAssetBundleOptions.CollectDependencies`、`BuildAssetBundleOptions.CompleteAssets`、`BuildAssetBundleOptions.DeterministicAssetBundle`等。
3. **目标平台**：`BuildTarget.StandaloneWindows64`指定了目标平台为Windows 64位。你需要根据你的目标平台来调整这个值。
4. **菜单项**：`[MenuItem("Assets/Build AssetBundles")]`创建了一个菜单项，允许你在Unity编辑器的“Assets”菜单下找到并运行这个脚本。
5. **性能考虑**：这个脚本在遍历所有资源时可能效率不高，特别是对于大型项目。在实际项目中，你可能需要更高效地处理资源路径和打包逻辑。
6. **运行时加载**：在运行时加载这些AssetBundle时，请确保使用正确的路径。在编辑器中，`Application.streamingAssetsPath`将指向项目的`Assets/StreamingAssets`目录；在构建后的应用程序中，它将指向应用程序安装目录中的一个特定位置。
7. **测试**：在将AssetBundle分发到玩家之前，请确保在目标平台上进行充分的测试。
8. **文档**：始终建议查阅Unity的官方文档以获取最新和最准确的信息，因为Unity的版本更新可能会引入变化。

**压缩选项**：

- 不压缩

- LZ4：基于Block的压缩

- LZMA：基于Chunk的压缩，读Header，找Chunk，再找Block

  

## 加载Bundle

（完整流程：上传、下载（filelist.json包含ab文件名和md5）、解包、加载资源、实例化资源）

接下来，我们需要在运行时加载这些Bundle。在运行时，你可以使用`UnityWebRequestAssetBundle.GetAssetBundle`或`AssetBundle.LoadFromFile`（对于直接从磁盘加载的情况，但注意这通常不推荐用于移动平台）来加载StreamingAssets目录中的AssetBundle。

编辑器下从Assets目录加载，代码如下：

```csharp
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class LoadBundle : MonoBehaviour
{
    private string bundleURL = "file://" + Application.dataPath + "/Bundles/";
 
    void Start()
    {
        StartCoroutine(LoadAssetBundle("myassetbundle"));
    }
 
    IEnumerator LoadAssetBundle(string bundleName)
    {
        string url = bundleURL + bundleName;
 
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            // 等待请求完成
            yield return uwr.SendWebRequest();
 
            if (uwr.result != UnityWebRequest.Result.ConnectionError && uwr.result != UnityWebRequest.Result.ProtocolError)
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
 
                // 从Bundle中加载资源
                GameObject prefab = bundle.LoadAsset<GameObject>("MyPrefab");
                Instantiate(prefab);
 
                // 你可以在这里存储bundle对象以便后续使用
                // ...
 
                // 卸载Bundle（延迟卸载）
                bundle.Unload(false);
            }
            else
            {
                Debug.Log(uwr.error);
            }
        }
    }
}
```

如果是从StreamingAssets目录加载这么写：

```c#
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
 
public class LoadAssetBundleFromStreamingAssets : MonoBehaviour
{
    IEnumerator Start()
    {
        // 获取StreamingAssets目录的路径
        string streamingAssetsPath = Application.streamingAssetsPath;
        
        // 假设AssetBundle文件名为"myassetbundle"，并放置在StreamingAssets目录下
        string assetBundlePath = System.IO.Path.Combine(streamingAssetsPath, "myassetbundle");
 
        // UnityWebRequest用于从本地或网络加载资源
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(assetBundlePath);
 
        // 发送请求并等待完成
        yield return request.SendWebRequest();
 
        // 检查请求是否成功
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }
 
        // 获取加载的AssetBundle对象
        AssetBundle assetBundle = request.downloadHandler.assetBundle;
 
        // 从AssetBundle中加载资源，假设资源名为"myasset"
        GameObject asset = assetBundle.LoadAsset<GameObject>("myasset");
 
        // 实例化加载的资源
        Instantiate(asset);
 
        // 释放AssetBundle对象（可选，通常根据需要在资源加载完成后释放）
        // assetBundle.Unload(false); // false表示不卸载依赖的AssetBundle
 
        // 释放UnityWebRequest对象
        request.Dispose();
    }
}
```



## 卸载Bundle

卸载Bundle可以通过`Unload`方法实现，有两种模式：

- `Unload(false)`：延迟卸载，当没有其他对象引用Bundle中的资源时，资源才会被卸载。
- `Unload(true)`：立即卸载，即使资源仍在使用中也会被卸载，这可能会导致资源丢失。

在上面的代码中，我们已经演示了延迟卸载。如果你需要立即卸载，可以这样做：

```csharp
// 立即卸载Bundle
bundle.Unload(true);
```

## 注意事项

1. **路径问题**：确保你的Bundle文件路径正确。在编辑器中运行时，路径可能与在构建后运行时不同。
2. **资源管理**：确保正确管理资源的加载和卸载，以避免内存泄漏。
3. **依赖关系**：注意Bundle之间的依赖关系，确保先加载依赖的Bundle。

通过这些步骤和代码示例，你应该能够在Unity中有效地使用Asset Bundles来管理你的游戏资源。



# 收集野依赖资源做成bundle

```c#
//TODO 对每个bundle中的文件做引用搜集
string[] arr = AssetDatabase.GetDependencies(bundleFilePath, true);
//TODO 搜集后去重、去掉自身、排除字体、排除shader、排除脚本文件、排除自动打包资源
//如果依赖资源在其他bundle中也搜到了，登记为野依赖资源
```

# 新的Bundle管理方式

## AssetBundle Browser

unity官方推出插件：AssetBundle Browser，通过packagemanager可安装

AssetBundle Browse可以将项目中的资源打包成AB包，发布游戏后，项目将通过加载StreamingAssets中AB包的形式，加载所需资源，而不是像之前那样，从Assets中进行加载。

教程：[Unity 使用AssetBundle-Browser打包助手打包AssetBundle（+复用）_assetbundlebrowser-CSDN博客](https://blog.csdn.net/WenHuiJun_/article/details/113178688)



## [Unity Addressable Assets 系统](https://docs.unity.cn/cn/2020.2/Manual/com.unity.addressables.html)

[Addressable Asset System（进阶版AB）和AssetBundle（以下简称AB）制作的资源管理系统的对比](https://www.freesion.com/article/7825289903/)

官方社区文章：

- [Addressable系统的加载资源API总结 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5de3619dedbc2a6576af9862)
- [使用Addressable更好的管理内存 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5dd79600edbc2a0281c3b4e4)
- [已有项目要不要迁移到Addressable系统？ - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5dd262b2edbc2a0020a72682)
- [【Addressable】发布到服务端的那些事 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5dfb3d33edbc2a00208a0a64)

不是新事物，而是对AB的高级包装，支持资源引用计数、自动资源寻址、远程/本地热更新对开发者透明

# 热更

## 资源热更

包括预制体、材质、贴图、模型、场景、shader、动画控制器、动画等除代码之外的所有资源

打包方式：小项目按Label打包，常规按照资源路径打包，编写版本文件并上传资源服务器，客户端启动时比对。

## 代码热更

- Xlua
- ILRuntime



# 查看Bundle的工具

过去叫UnityStudio，现在叫AssetStudio：

https://github.com/Perfare/AssetStudio/releases

相关文章介绍：[Unity3D研究院之提取游戏资源的三个工具支持Unity5（八十四） | 雨松MOMO程序研究院](https://www.xuanyusong.com/archives/3618)



# Bundle的加密

参考文章：[AssetBundle的几种加密方式 - 知乎](https://zhuanlan.zhihu.com/p/382888420)

AssetBundle是Unity游戏开发中使用的一种资源打包方式，它默认是不加密的，但开发者可以通过多种方式对AssetBundle进行加密，以保护自己的游戏资源不被轻易窃取或篡改。以下是一些常见的AssetBundle加密方式：

1. **Unity中国区提供的加密方式**：

   - 打包前设置秘钥：开发者可以在打包AssetBundle前设置一个16位的任意数字字母组合作为秘钥，通过调用`AssetBundle.SetAssetBundleDecryptKey`方法设置。加载时也需要指定相同的秘钥进行解密。
   - 不过这个加密方式有个弊端就是每次资源变动计算出的MD5是不一致的，即使资源还原，再打AB包，计算的MD5值也不会跟之前的重复，这种加密方式用来做热更会比较麻烦，需要自己去做一下处理。

2. **对AB包的二进制文件进行加密操作**：

   - 开发者可以对AssetBundle的二进制文件进行加密操作，然后在加载时使用`AssetBundle.LoadFromMemoryAsync`或`AssetBundle.LoadFromMemory`方法从内存中加载解密后的数据。这种加密方式需要在先把文件的字节数加载到内存中，再通过API加载，这样会在内存中存在双份内存，会造成较高的内存峰值。

   - ```c#
     AssetBundle abBundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(filePath));
     this._loadRequest =AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(filePath))；
     ```

3. **使用Offset加密（推荐）**：

   - Offset加密的原理是对AssetBundle的二进制文件添加一些字符来达到加密的目的。解密时需要跳过这些添加的字符来正确读取数据。Unity官方提供了根据Offset进行加载的API，如`AssetBundle.LoadFromFile`和`AssetBundle.LoadFromFileAsync`。

   - ```c#
     public static void ABOffsetEncryption(string fileFile)
     {
         byte[] oldData = File.ReadAllBytes(fileFile);
         int newOldLen = 8 + oldData.Length;//这个空字节数可以自己指定,示例指定8个字节
         var newData = new byte[newOldLen];
         for (int tb = 0; tb < oldData.Length; tb++)
         {
         	newData[8+ tb] = oldData[tb];
         }
         FileStream fs = File.OpenWrite(fileFile);//打开写入进去
         fs.Write(newData, 0, newOldLen);
         fs.Close();
     }
     //同步加载
     AssetBundle abBundle = AssetBundle.LoadFromFile(assetBundleMapPath,0,8);
     //异步加载
     this._loadRequest = AssetBundle.LoadFromFileAsync(this.filePath,0,8);
     ```

     

4. **将数据加密为TextAsset**：

   - 开发者可以将数据加密后保存为.bytes文件，Unity会将其视为TextAsset类型。在游戏运行时，可以先下载这个加密的TextAsset，然后解密其内容，并使用`AssetBundle.CreateFromMemory`方法从解密后的数据中创建AssetBundle。同样有双份内存问题。

5. **共享密钥加密（推荐）**：

   - 这是一种更通用的加密方式，如AES、DES等加密算法都可以用于加密AssetBundle。加密时，将AssetBundle的数据读取出来，与密钥进行某种运算（如异或操作）得到加密后的数据。解密时，再进行相反的运算即可恢复原始数据。