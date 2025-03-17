## 需求

因为markdown文档我平时都放到git仓库，仓库里很可能除了md文件还有需要存一些其他文件（比如一个全都是cs脚本的文件夹），用Typora等markdown查看工具是无法在侧边栏直接查看这些文件的。我想到的替代方案是：在这个文件夹下建立一个readme.md目录文件，把所有脚本文件做成链接记录进去，这样点击链接就可以用第三方查看工具（PC上我用的VSCode，安卓上我用的Code Viewer）来查看代码了。我把这个需求通过具体描述向AI询问：

```
写一个C#程序控制台exe程序，拖拽一个文件夹到这个exe文件上，可以把这个文件夹（包括子文件夹）下所有的cs脚本以指定的方式打印出来，比如文件夹下有两个cs脚本a.cs和b.cs，那么打印两行文字：

- [a.cs](./a.cs)
- [b.cs](./b.cs) 

如果还有子文件夹child，其下有一个c.cs文件，那么再打印：

- [c.cs](./child/c.cs)
```

## 代码实现

AI回答的代码基本直接可用，下面是稍加修正的最终代码：

```csharp
using System;
using System.IO;
using System.Windows.Forms;//解决方案右键引用->添加引用

class Program
{
    [STAThread]//Clipboard类需要在单线程单元（STA）模式下运行
    static void Main(string[] args)
    {
        bool isFirstTime = true;
        while (true)
        {
            //文件夹路径
            string folderPath = string.Empty;
            // 检查是否提供了文件夹路径作为参数
            if(isFirstTime)
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("方式一：拖拽一个文件夹到这个exe文件上（已执行失败）");
                    Console.WriteLine("改为方式二：请拖拽一个文件夹到这个窗口上");
                    folderPath = Console.ReadLine().Replace("\"", "");
                }
                else
                {
                    folderPath = args[0];
                }
                isFirstTime = false;
            }
            else
            {
                Console.WriteLine("方式二：请拖拽一个文件夹到这个窗口上");
                folderPath = Console.ReadLine().Replace("\"", "");
            }

            // 检查路径是否存在且是文件夹
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("文件夹路径不合法");
                PauseProgram();
                break;
            }

            // 调用方法来遍历文件夹并打印 .cs 文件
            PrintCSharpFiles(folderPath);
            PauseProgram();
        }
    }

    static void PrintCSharpFiles(string folderPath)
    {
        // 获取所有 .cs 文件(如果需要列出所有文件可以把*.cs改成*.*)，包括子文件夹中的文件
        string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
        StringBuilder sb = new StringBuilder();

        // 遍历每个文件并打印相对路径
        foreach (string file in files)
        {
            // 获取相对于根文件夹的路径
            string relativePath = file.Substring(folderPath.Length + 1).Replace("\\","/");
            var line = $"- [{Path.GetFileName(file)}](./{relativePath})";
            Console.WriteLine(line);
            sb.AppendLine(line);
        }
        Clipboard.SetText(sb.ToString());//把打印结果全部存到Windows系统剪切板
    }
    static void PauseProgram()
    {
        Console.WriteLine("=========按回车可继续========");
        Console.ReadLine();
    }
}
```

## 生成控制台程序

[以markdown形式列出所有文件.exe](./以markdown形式列出所有文件.exe)

