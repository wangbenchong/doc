# C#主体代码

先放最终成品：[Wav转Mp3.rar](./Wav转Mp3.rar)

默认会处理同级目录的所有wav文件（如果也需要处理子文件夹，到config.txt去改配置）。因为有一个dll无法整合到exe中无法做成单exe文件，故做成了文件夹（含exe和dll以及一个config.txt配置文件）。使用时把文件夹整体放到需要处理的目录之下，再双击exe执行。

```csharp
using System;
using System.IO;
using NAudio.Wave;
using NAudio.Lame;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            int sampleRate = 44100;
            bool deleteBaseFile = false;
            bool includeChildFolder = false;
            // 获取当前目录
            string currentDirectory = Directory.GetCurrentDirectory().Replace("\\","/");
            string configPath = Path.Combine(currentDirectory, "config.txt");
            if (File.Exists(configPath))
            {
                try
                {
                    string valueStr = string.Empty;
                    // 读取配置文件内容
                    string configContent = File.ReadAllText(configPath);
                    if(configContent.Contains("\n"))
                    {
                        valueStr = configContent.Replace("\r", "").Split('\n')[0];
                    }
                    deleteBaseFile = configContent.Contains("转换后删除源文件：是");
                    includeChildFolder = configContent.Contains("包含子文件夹：是");
                    int configSampleRate;
                    int.TryParse(valueStr, out configSampleRate);
                    if(configSampleRate < 16000 || configSampleRate > 192000)
                    {
                        Console.WriteLine($"采样率配置超出范围，使用默认采样率{sampleRate}Hz与原始wav文件采样率的最小值");
                    }
                    else
                    {
                        sampleRate = configSampleRate;
                        Console.WriteLine($"使用配置的采样率{sampleRate}Hz与原始wav文件采样率的最小值");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"读取config.txt出错: {ex.Message}. 使用默认采样率{sampleRate}Hz与原始wav文件采样率的最小值");
                }
            }
            else
            {
                Console.WriteLine($"未找到config.txt，使用默认采样率{sampleRate}Hz与原始wav文件采样率的最小值");
            }
            int lastIndex = currentDirectory.LastIndexOf("/");
            currentDirectory = currentDirectory.Substring(0, lastIndex);

            // 遍历当前目录下的所有文件
            foreach (string filePath in Directory.GetFiles(currentDirectory, "*.wav", includeChildFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                // 构建输出文件路径
                string outputFilePath = Path.ChangeExtension(filePath, ".mp3");
                // 转换WAV到MP3
                bool success = ConvertWavToMp3(filePath, outputFilePath, sampleRate);

                if(!success)
                {
                    Console.WriteLine($"转换{filePath}失败");
                    continue;
                }
                //如果配置了转换后删除源文件，那就删除
                if (deleteBaseFile)
                {
                    File.Delete(filePath);
                }
                Console.WriteLine($"已转换: {filePath.Replace("\\", "/")} -> {outputFilePath.Replace("\\", "/")}");
            }
            Console.WriteLine("转换完毕，按任意键退出");
            Console.ReadKey();
        }

        static bool ConvertWavToMp3(string inputFilePath, string outputFilePath, int sampleRate)
        {
            try
            {
                using (var reader = new AudioFileReader(inputFilePath))
                {
                    sampleRate = Math.Min(reader.WaveFormat.SampleRate, sampleRate);
                    // 如果声道数大于 2，则转换为立体声
                    if (reader.WaveFormat.Channels > 2)
                    {
                        var targetFormat = new WaveFormat(sampleRate, 2); // 转换为立体声
                        using (var resampler = new MediaFoundationResampler(reader, targetFormat))
                        {
                            MediaFoundationEncoder.EncodeToMp3(resampler, outputFilePath, sampleRate);
                        }
                    }
                    else
                    {
                        // 直接处理单声道或立体声流
                        using (var writer = new LameMP3FileWriter(outputFilePath, reader.WaveFormat, sampleRate / 1000))
                        {
                            reader.CopyTo(writer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
```

# 安装Nuget包

需要安装两个Nuget包才能编译通过：NAudio和NAudio.Lame

在这里查看Nuget包：https://www.nuget.org/packages

安装：VS菜单栏 > 工具 > NuGet包管理器 > 程序包管理控制台

以NAudio为例，输入指令（最新已是2.21版，但要使用和自己VS2015能兼容的版本）：

```
NuGet\Install-Package NAudio -Version 1.10.0
```

再装个NAudio.Lame，依赖于NAudio，同样注意使用兼容的版本：

```
NuGet\Install-Package NAudio.Lame -Version 1.0.9
```

题外话：以上包的核心dll文件都在工程目录的package文件夹中

# 导出单一exe文件

## 方法：Fody

因为引入了两个Nuget包，所以正常发布exe之外还要带上几个dll文件，很不方便。安装Fody包即可解决:

- [VS2015使用Costura.Fody将dll打包到exe_fody 打包 dll-CSDN博客](https://blog.csdn.net/u012842630/article/details/117233577)
- 注意VS2015不支持高版本，适当的版本是Fody：4.2.1， Costura.Fody：3.3.3

不过引入Fody之后，虽然NAudio.dll和NAudio.Lame.dll都嵌入exe了，但是libmp3lame.32.dll还是没嵌入exe中。以下是一些相关搜索，但貌似不能解决问题，目前还是需要让libmp3lame.32.dll和exe一起发布：

- [C# 关于Costura.Fody无法打包所有dll解决方案-CSDN博客](https://blog.csdn.net/PLA12147111/article/details/105571501)
- [.NET 合并程序集（将 dll 合并到 exe 中） - 朱志 - 博客园](https://www.cnblogs.com/zhuzhi0819/p/12931691.html)，[.NET(C#) 使用Costura.Fody将程序发布成单个exe文件-CJavaPy](https://www.cjavapy.com/article/2696/)

## 方法：使用 ILMerge

ILMerge 是一个将多个程序集合并为一个程序集的工具。

1. 下载并安装 [ILMerge](https://github.com/dotnet/ILMerge)。另：[exe文件但貌似不灵](https://www.mediafire.com/file/kvx36vanbotoe3h/IlMerge.rar/file)

2. 在项目目录下创建一个批处理文件（例如 `merge.bat`）然后执行，内容如下：

   ```bat
   ilmerge /out:merged.exe YourApp.exe NAudio.dll NAudio.Lame.dll
   ```

3. 生成的 `merged.exe` 文件将包含所有依赖项。



## 方法：VS自带

根据DeepSeek介绍，也有直接在`.csproj` 文件 配置的方法，但我试了不好使，是vs版本过低不支持（需要.NET 5+），所以该法仅供参考：

```xml
<PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework> <!-- 根据你的目标框架修改 -->
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier> <!-- 根据目标平台修改 -->
</PropertyGroup>
```



