# 如何防止Unity运行时修改材质球文件

在Unity中，当你在编辑器运行时动态修改材质参数，这些修改默认会保留到材质球文件中。以下是几种防止这种情况的方法：

## 使用MaterialPropertyBlock

这是最推荐的方法，特别适用于需要频繁修改材质属性的情况：

```c#
private MaterialPropertyBlock propertyBlock;
private Renderer renderer;

void Start()
{
    renderer = GetComponent<Renderer>();
    propertyBlock = new MaterialPropertyBlock();
    
    // 获取当前属性值
    renderer.GetPropertyBlock(propertyBlock);
    
    // 修改属性而不影响原始材质
    propertyBlock.SetFloat("_YourPropertyName", yourValue);
    propertyBlock.SetColor("_Color", Color.red);
    
    // 应用修改
    renderer.SetPropertyBlock(propertyBlock);
}
```

## 运行时创建材质实例

```c#
private Material originalMaterial;
private Material runtimeMaterial;

void Start()
{
    Renderer renderer = GetComponent<Renderer>();
    originalMaterial = renderer.sharedMaterial;
    runtimeMaterial = new Material(originalMaterial);
    renderer.material = runtimeMaterial; // 这会创建并使用实例
}

void OnDisable()
{
    // 可选：恢复原始材质
    Renderer renderer = GetComponent<Renderer>();
    renderer.sharedMaterial = originalMaterial;
    
    // 销毁运行时材质
    if (Application.isEditor && !Application.isPlaying)
    {
        DestroyImmediate(runtimeMaterial);
    }
    else
    {
        Destroy(runtimeMaterial);
    }
}
```

## 使用脚本控制恢复

```c#
private float originalValue;

void Start()
{
    // 保存原始值
    originalValue = GetComponent<Renderer>().sharedMaterial.GetFloat("_YourProperty");
    
    // 修改值
    GetComponent<Renderer>().sharedMaterial.SetFloat("_YourProperty", newValue);
}

void OnDisable()
{
    // 恢复原始值
    GetComponent<Renderer>().sharedMaterial.SetFloat("_YourProperty", originalValue);
}
```

## 使用Editor脚本自动恢复

创建一个编辑器脚本，在退出播放模式时自动恢复所有材质：

```c#
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
public class MaterialResetOnExitPlayMode
{
    static MaterialResetOnExitPlayMode()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            // 这里可以添加恢复材质的逻辑
            // 例如重新加载所有材质或恢复备份
        }
    }
}
#endif
```

## 最佳实践建议

1. **对于频繁修改的属性**：使用MaterialPropertyBlock，这是性能最好的方式
2. **对于复杂材质替换**：使用运行时材质实例
3. **对于简单项目**：使用脚本控制恢复可能足够

MaterialPropertyBlock是最优解，因为它不会创建新的材质实例，也不会修改原始材质资源，同时具有良好的性能表现。



# 为什么Unity不默认使用MaterialPropertyBlock？

> 既然MaterialPropertyBlock这么好，为什么默认不是MaterialPropertyBlock的效果而是修改材质球文件？而且有MaterialPropertyBlock，运行时创建材质岂不是只有弊端没有优点？

这是一个很好的问题，涉及到Unity材质系统的设计哲学和权衡。让我们来分析其中的原因和设计考量：

## 为什么默认行为是修改材质球文件？

1. **直观性和易用性**：
   - 直接修改材质属性是最直观的方式，特别适合初学者
   - 对于不需要频繁修改的材质属性，直接修改是最简单的工作流程
2. **编辑器工作流**：
   - 设计师和美术人员通常希望在编辑器中看到最终效果
   - 直接修改材质可以让这些变化在编辑器中立即可见
3. **序列化需求**：
   - 材质属性的修改需要被序列化保存
   - MaterialPropertyBlock的修改是临时的，不会被保存
4. **历史原因**：
   - MaterialPropertyBlock是后来加入的功能
   - 保持向后兼容性很重要

## 运行时创建材质实例 vs MaterialPropertyBlock

虽然MaterialPropertyBlock在很多方面更优，但运行时创建材质实例仍有其用途：

| 比较维度       | MaterialPropertyBlock | 运行时材质实例            |
| :------------- | :-------------------- | :------------------------ |
| **性能**       | 更高 (不创建新材质)   | 较低 (需实例化)           |
| **内存**       | 更低                  | 更高 (每个实例都是新对象) |
| **功能完整性** | 只能修改部分属性      | 可以完全替换材质          |
| **Shader变体** | 使用原材质变体        | 可创建不同变体            |
| **编辑器支持** | 修改不可见            | 修改在编辑器可见          |

### 运行时创建材质实例的优势场景：

1. **需要完全不同的材质**：
   - 当需要切换到一个完全不同的shader而不仅仅是修改参数时
2. **需要不同的渲染队列**：
   - 修改材质的渲染队列必须通过完整材质实例
3. **需要不同的shader变体**：
   - 如启用/禁用关键字(Shader.EnableKeyword)
4. **编辑器可视化需求**：
   - 在编辑器中需要看到修改后的效果
5. **复杂材质替换**：
   - 当需要替换整个材质而不仅仅是几个属性时

## Unity的设计权衡

Unity选择默认行为是修改材质球文件，是基于以下考虑：

1. **简单性优先**：让最简单的用例最简单实现
2. **可视化优先**：确保编辑时修改立即可见
3. **工作流兼容**：与AssetBundle、资源管理等系统兼容
4. **渐进式复杂度**：从简单到高级的使用路径

MaterialPropertyBlock虽然高效，但也有其局限性：

- 不能修改所有材质属性
- 修改不会显示在材质编辑器中
- 对某些渲染特性支持有限

## 实际开发建议

1. **优先使用MaterialPropertyBlock**：

   - 适用于大多数运行时属性修改
   - 特别是大量对象的属性修改(如大量角色的颜色变化)

2. **必要时使用运行时实例**：

   - 当需要完全不同的材质表现时
   - 当需要编辑器可视化时
   - 当MaterialPropertyBlock无法满足需求时

3. **混合使用**：

   ```c#
   // 创建实例用于复杂修改
   var tempMat = new Material(originalMat);
   tempMat.EnableKeyword("SPECULAR_ON");
   
   // 使用PropertyBlock进行频繁修改
   var propBlock = new MaterialPropertyBlock();
   propBlock.SetColor("_Color", dynamicColor);
   renderer.SetPropertyBlock(propBlock);
   ```

这种设计给了开发者选择的灵活性，可以根据具体需求选择最适合的方法。

# 为什么 MaterialPropertyBlock 不能修改变体？

> 也就是说单纯依靠MaterialPropertyBlock 是无法修改变体的是吧？

是的，**单纯使用 `MaterialPropertyBlock` 确实无法修改 Shader 变体（Shader Variants）**，因为它只能修改材质属性（如 `float`、`Color`、`Texture` 等），而不能影响 Shader 的关键字（Keywords）或变体切换。

1. **变体由 Shader Keywords 控制**
   - Shader 变体是通过 `#pragma multi_compile` 或 `#pragma shader_feature` 定义的，运行时需要用 `Material.EnableKeyword` 或 `Shader.EnableKeyword` 来切换。
   - `MaterialPropertyBlock` 仅修改材质属性（`_MainTex`, `_Color` 等），不涉及 Shader 的编译变体。
2. **MaterialPropertyBlock 是轻量级的属性覆盖**
   - 它的设计目的是高效修改渲染属性，而不影响材质本身的 Shader 配置。
   - 因此，它无法修改 `RenderQueue`、`Shader` 本身或 `Keywords`。

------

## **如何动态修改 Shader 变体？**

如果你需要 **运行时切换 Shader 变体**，必须使用 **`Material` 实例**（而不是 `MaterialPropertyBlock`）。

### 方法 1：直接修改 Material 的关键字（影响所有使用该材质的对象）

```c#
// 启用变体（如 "_SPECULAR_ON"）
GetComponent<Renderer>().sharedMaterial.EnableKeyword("_SPECULAR_ON");

// 禁用变体
GetComponent<Renderer>().sharedMaterial.DisableKeyword("_SPECULAR_ON");
```

⚠️ **问题**：这会修改原始材质（`.sharedMaterial`），导致停止运行后材质被污染。

------

### 方法 2：创建运行时 Material 实例（推荐）

```c#
private Material originalMat;
private Material runtimeMat;

void Start()
{
    var renderer = GetComponent<Renderer>();
    originalMat = renderer.sharedMaterial;
    
    // 创建实例，避免修改原始材质
    runtimeMat = new Material(originalMat);
    renderer.material = runtimeMat;  // 使用实例而非共享材质
    
    // 修改变体
    runtimeMat.EnableKeyword("_SPECULAR_ON");
}

void OnDestroy()
{
    // 销毁临时材质（防止内存泄漏）
    if (runtimeMat != null)
    {
        if (Application.isPlaying)
            Destroy(runtimeMat);
        else
            DestroyImmediate(runtimeMat);
    }
}
```

✅ **优点**：

- 不影响原始材质文件
- 可以自由修改 **Shader 变体** + **MaterialPropertyBlock**

------

### 方法 3：混合使用 MaterialPropertyBlock + 运行时 Material

```c#
Material runtimeMat;
MaterialPropertyBlock propBlock;

void Start()
{
    // 创建实例用于变体切换
    var renderer = GetComponent<Renderer>();
    runtimeMat = new Material(renderer.sharedMaterial);
    renderer.material = runtimeMat;
    
    // 启用变体
    runtimeMat.EnableKeyword("_EMISSION_ON");
    
    // 使用 PropertyBlock 修改动态属性（如颜色）
    propBlock = new MaterialPropertyBlock();
    propBlock.SetColor("_Color", Color.red);
    renderer.SetPropertyBlock(propBlock);
}
```

👉 **适用场景**：

- 需要 **变体切换**（如 `_SPECULAR_ON`）
- 同时需要 **高效修改属性**（如动态颜色 `_Color`）

------

## 总结

| 需求                                   | 解决方案                                       |
| :------------------------------------- | :--------------------------------------------- |
| **仅修改属性（`_Color`, `_MainTex`）** | `MaterialPropertyBlock`（最佳性能）            |
| **需要修改 Shader 变体（Keywords）**   | **运行时 `Material` 实例**（`new Material()`） |
| **同时需要变体 + 动态属性**            | **`Material` 实例 + `MaterialPropertyBlock`**  |

如果你的需求只是修改 `float`、`Color` 等属性，**优先用 `MaterialPropertyBlock`**。
但如果涉及 **Shader 变体切换**，则必须使用 **运行时 `Material` 实例**。



# 运行时创建材质对DrawCall的影响

> 创建运行时材质，会不会导致drawcall数量增加？

创建运行时材质实例是否会导致DrawCall增加，取决于具体的使用方式。让我们详细分析：

## 基本影响原则

1. **相同材质的对象可以批量渲染**：
   - Unity会对使用完全相同材质的对象进行动态合批(Dynamic Batching)
   - 每个独特的材质实例通常会产生独立的DrawCall
2. **运行时材质的影响**：
   - 如果你为每个对象创建独特的材质实例，确实会增加DrawCall
   - 如果多个对象共享同一个运行时材质实例，则不会额外增加DrawCall

## 具体场景分析

### 情况1：每个对象有自己的材质实例

```c#
// 为每个对象创建独立材质实例（最坏情况）
void Start() {
    var renderer = GetComponent<Renderer>();
    var newMat = new Material(renderer.sharedMaterial);
    renderer.material = newMat; // 每个对象都有独特实例
}
```

✅ **结果**：每个对象都会有独立的DrawCall，无法合批

### 情况2：多个对象共享同一个运行时材质实例

```c#
// 共享同一个运行时材质实例
public class MaterialManager : MonoBehaviour {
    public static Material SharedRuntimeMaterial;
    
    void Start() {
        if(SharedRuntimeMaterial == null) {
            var renderer = GetComponent<Renderer>();
            SharedRuntimeMaterial = new Material(renderer.sharedMaterial);
        }
        GetComponent<Renderer>().sharedMaterial = SharedRuntimeMaterial;
    }
}
```

✅ **结果**：所有使用该材质的对象可以合批，DrawCall不会增加

### 情况3：使用MaterialPropertyBlock

```c#
// 使用MaterialPropertyBlock修改属性
MaterialPropertyBlock block;
void Start() {
    block = new MaterialPropertyBlock();
    GetComponent<Renderer>().GetPropertyBlock(block);
    block.SetColor("_Color", Random.ColorHSV());
    GetComponent<Renderer>().SetPropertyBlock(block);
}
```

✅ **结果**：DrawCall不会增加，可以保持合批

## 优化建议

1. **尽量共享材质实例**：
   - 让需要相同变体的对象共享同一个运行时材质实例
2. **分层使用**：
   - 对需要变体切换的使用共享的运行时材质
   - 对需要个性化属性的使用MaterialPropertyBlock
3. **使用GPU Instancing**：
   - 如果Shader支持GPU Instancing，即使材质属性不同也能合批
   - 需要启用材质的"Enable GPU Instancing"选项
4. **考虑SRP Batcher**：
   - 在URP/HDRP中，SRP Batcher可以减少类似材质的DrawCall
   - 需要Shader兼容SRP Batcher

## 性能对比表

| 方法                  | DrawCall影响 | 内存影响 | 适用场景               |
| :-------------------- | :----------- | :------- | :--------------------- |
| 原始共享材质          | 最优(可合批) | 最优     | 静态不变材质           |
| 共享运行时材质        | 可合批       | 中等     | 需要变体切换的相同对象 |
| 独立运行时材质        | 增加DrawCall | 较高     | 每个对象需要不同变体   |
| MaterialPropertyBlock | 不影响合批   | 最低     | 只需修改属性           |

## 结论

**运行时创建材质不一定会增加DrawCall**，关键看你如何使用：

- 如果**共享同一个运行时材质实例**，DrawCall不会增加
- 如果**每个对象都有独立实例**，确实会增加DrawCall
- **MaterialPropertyBlock**是最轻量级的解决方案，但无法处理变体切换

最佳实践是根据需求混合使用这些技术，在功能需求和性能之间取得平衡。



# UGUI 能否使用 MaterialPropertyBlock ？

在 Unity 的 UGUI 系统中，`Graphic` 类（如 `Image`、`Text`、`RawImage` 等）确实有一个 `material` 属性，但它的使用方式与常规 3D 渲染有所不同。

## 结论: 不能

**UGUI 的 Graphic 组件不能直接使用 MaterialPropertyBlock**，原因如下：

1. **UGUI 渲染流程特殊**：
   - UGUI 使用 Canvas 渲染系统，最终由 CanvasRenderer 处理
   - 材质属性修改通过 `CanvasRenderer` 的特定方法实现，而非 `MaterialPropertyBlock`
2. **Graphic 类的工作方式**：
   - 首次调用 `Graphic.material` 的方法 (如SetColor) 修改变量会先判断，如果是默认材质会先创建材质实例之后再修改

## 替代方案

### 使用 Graphic 的默认方式（会创建材质实例）

```c#
// 获取/创建材质实例
var mat = graphic.material;
mat.SetColor("_Color", Color.red);//如果mat是默认材质，会隐式创建材质实例
```

⚠️ **问题**：这会创建材质实例，增加内存开销

### 使用 CanvasRenderer.SetColor (仅限颜色)

```c#
// 直接修改顶点颜色（最轻量级的方式）
graphic.canvasRenderer.SetColor(Color.red);
```

✅ **优点**：

- 不会创建新材质实例
- 性能最佳

❌ **限制**：

- 只能修改颜色
- 不影响其他材质属性

###  共享材质实例（优化方案）

```c#
// 创建共享材质
static Material sharedUIMaterial;

void Start() {
    if(sharedUIMaterial == null) {
        sharedUIMaterial = new Material(graphic.material);
    }
    graphic.material = sharedUIMaterial;
    graphic.material.SetColor("_Color", Color.red);
}
```

✅ **优点**：

- 多个 UI 元素共享同一个材质实例
- 减少 DrawCall

### UGUI 自定义 Shader + 参数传递

如果你使用自定义 Shader，可以通过以下方式传递参数：

```c#
// 在Shader中声明属性
Properties {
    _MyColor ("Color", Color) = (1,1,1,1)
}

// 在C#中修改
graphic.material.SetColor("_MyColor", Color.blue);
```

### UGUI 独有的继承 IMaterialModifier 接口

UGUI源码中有这样一个接口：

```csharp
namespace UnityEngine.UI
{
    /// <summary>
    /// Use this interface to modify a Material that renders a Graphic. The Material is modified before the it is passed to the CanvasRenderer.
    /// </summary>
    /// <remarks>
    /// When a Graphic sets a material that is passed (in order) to any components on the GameObject that implement IMaterialModifier. This component can modify the material to be used for rendering.
    /// </remarks>
    public interface IMaterialModifier
    {
        /// <summary>
        /// Perform material modification in this function.
        /// </summary>
        /// <param name="baseMaterial">The material that is to be modified</param>
        /// <returns>The modified material.</returns>
        Material GetModifiedMaterial(Material baseMaterial);
    }
}
```

我们可以写一个脚本继承这个 **IMaterialModifier** 接口，实现里面的 GetModifiedMaterial 方法：

```csharp
//以溶解（Dissolve）效果为例：
public class DissolveMaterialEffect : 
MonoBehaviour, //或者把MonoBehaviour 换成 UnityEngine.EventSystems.UIBehaviour，内置事件更丰富
IMaterialModifier
{
    public Material GetModifiedMaterial(Material baseMaterial)//此时baseMaterial还是默认材质
    {
        /*
        //对默认材质做处理，直接new或者自己设计对象池
        var mat = new Material(baseMaterial)
        {
        	hideFlags = HideFlags.HideAndDontSave;
        }
        //-------替换Shader----------
        //此时mat.shader.name为“UI/Default”
        if (GraphicsSettings.defaultRenderPipeline == null) //BuiltIn渲染管线
        {
        	//替换成你写的 BuiltIn shader，比如你的shader名为 “Hidden/UI/Default (UIDissolve)”
            mat.shader = Shader.Find($"Hidden/{mat.shader.name} (UIDissolve)");
        }
        else //SRP渲染管线
        {
        	//替换成你写的 urp shader，比如你的shader名为 “Hidden/SRP_UI/Default (UIDissolve)”
            mat.shader = Shader.Find($"Hidden/SRP_{mat.shader.name} (UIDissolve)");
        }
        return mat;
        */
        return baseMaterial;//返回的是你指定的材质
    }
}
```

具体 Shader 的写法例如这样（使用_MainTex作为变量名，这个原理和 [定义MainTex变量](./Shader Graph示例汇总/Shader Graph示例汇总.md#定义MainTex变量) 是相通的）：

```c
Shader "Hidden/SRP_UI/Default (UIDissolve)"
{
	Properties
	{
         //[PerRendererData]修饰符允许你为每个使用该着色器的对象提供不同的值
         //_MainTex 是固定名称，可以获取到 Image 的 Source 用图
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        //....
    }
    //...
}
```

**注意：**

这种做法是连带材质球和shader都隐式地做了替换，shader要放在Resources目录下才能找到，并且凡是创建实例都可能会增加DrawCall的。和直接修改 graphic.material 性能上没啥区别。

> 题外话：类似的接口还有 IMeshModifier，继承需要实现 `public virtual void ModifyMesh(VertexHelper vh)` 方法

## 性能对比

| 方法                      | 材质实例 | DrawCall影响 | 适用场景       |
| :------------------------ | :------- | :----------- | :------------- |
| 直接修改 graphic.material | 创建实例 | 可能增加     | 简单修改       |
| canvasRenderer.SetColor   | 无       | 不影响       | 仅改颜色       |
| 共享材质实例              | 共享实例 | 可合批       | 多个UI相同修改 |
| MaterialPropertyBlock     | ❌ 不支持 | -            | -              |

## 最佳实践建议

1. **如果只需改颜色**：
   - 优先使用 `canvasRenderer.SetColor()`
2. **如果需要修改其他属性**：
   - 使用共享材质实例方案
   - 确保多个 UI 元素使用相同的修改
3. **需要完全不同的外观**：
   - 直接使用不同的材质资源
4. **大量动态 UI 元素**：
   - 考虑使用 `MaskableGraphic` 和动态合批
   - 尽量减少运行时材质修改

UGUI 的设计更倾向于静态 UI，频繁修改材质属性并不是它的强项。如果项目需要高度动态的 UI 效果，可能需要考虑使用更专业的 UI 解决方案或自定义渲染方案。



# 最终，制作组件：防编辑器运行时污染材质球

**MatFixInEditor.cs** 代码如下：

```csharp
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MatFixInEditor : MonoBehaviour
{
    public class MatFixData
    {
        public Material mat;
        public int count = 0;
    }
    public static Dictionary<string, MatFixData> matDic = new Dictionary<string, MatFixData>();
    public static string RegMaterial(Material matAsset)
    {
        if(matAsset == null) return string.Empty;
        var path = AssetDatabase.GetAssetPath(matAsset);
        if(path.StartsWith("Assets/"))
        {
            if(!matDic.ContainsKey(path))
            {
                
                var data = new MatFixData() { mat = new Material(matAsset) };
                data.mat.name += " (Editor Runtime)";
                data.count = 1;
                matDic.Add(path, data);
            }
            else
            {
                matDic[path].count++;
            }
            return path;
        }
        return string.Empty;
    }
    public static void UnRegMaterial(string path)
    {
        if(matDic.TryGetValue(path, out var matData))
        {
            matData.count--;
            if(matData.count <= 0)
            {
                DestroyImmediate(matData.mat);
                matDic.Remove(path);
            }
        }
    }

    #if UNITY_EDITOR
    private List<string> matPaths = new List<string>();
    void Awake()
    {
        //处理 UGUI 材质球
        var graphics = GetComponentsInChildren<Graphic>(true);
        for(int i=0;graphics != null && i<graphics.Length;i++)
        {
            var graphic = graphics[i];
            if(graphic.material != null && graphic.material != graphic.defaultMaterial)
            {
                var path = RegMaterial(graphic.material);
                if(!string.IsNullOrEmpty(path))
                {
                    matPaths.Add(path);
                    graphic.material = matDic[path].mat;
                }
            }
        }
        //处理 Renderer 材质球
        var rds = GetComponentsInChildren<Renderer>(true);
        for(int i=0;rds != null && i<rds.Length;i++)
        {
            var renderer = rds[i];
            if(renderer == null) continue;
            //var mats = renderer.materials;//这句话会导致材质球全部实例化,避免使用
            var mats = renderer.sharedMaterials;
            if(mats != null)
            {
                bool isChanged = false;
                for(int k=0,len = mats.Length; k<len; k++)
                {
                    var mt = mats[k];
                    if(mt == null) continue;
                    var path = RegMaterial(mt);
                    if(!string.IsNullOrEmpty(path))
                    {
                        matPaths.Add(path);
                        mats[k] = matDic[path].mat;
                        isChanged = true;
                    }
                }
                if(isChanged)
                {
                    renderer.materials = mats;
                }
            }
        }
    }
    void OnDestroy()
    {
        foreach(var path in matPaths)
        {
            UnRegMaterial(path);
        }
        matPaths.Clear();
    }
    void OnApplicationQuit()
    {
        //再整体清理一遍，防止编辑器清理不完全（正常是会清理的）导致内存溢出。
        var itor = matDic.GetEnumerator();
        while(itor.MoveNext())
        {
            DestroyImmediate(itor.Current.Value.mat);
        }
        matDic.Clear();
    }

    [ContextMenu("功能介绍")]
    private void Describe()
    {
        EditorUtility.DisplayDialog("Mat Fix In Editor介绍",
@"功能：
防止在编辑器运行时，原始材质文件受到污染。

作用范围：
UGUI元素和各种Renderer。

原理：
实例化材质（仅编辑器运行时），并且保持原本的复用性。

使用建议：
在代码中控制Renderer的材质球时推荐使用sharedMaterial而不是material。", "好的");
    }
    #endif
}
```

