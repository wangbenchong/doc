# EXE执行程序：

直链接：[文件批量合并拖拽版.exe](文件批量合并拖拽版.exe)

网盘备份链接: https://pan.baidu.com/s/1fpBU9aMuldcLkQXPxu3z0Q?pwd=j5xx 提取码: j5xx 
--来自百度网盘超级会员v7的分享

有两个exe文件，推荐使用**拖拽款**。把exe文件放到任意位置（例如桌面），选中你要合并的若干文件，拖拽到这个exe上，就会打开一个黑色窗口，按照提示查看合并的顺序，**最终合并的文件**与第一个文件在同一文件夹下，名称也会参照第一个文件的名称（附加了_Mix后缀）。

# 视频合并需要额外注意：

如果合并的文件是视频文件（比如扩展名为ts的视频文件），那么合并后的视频文件右键属性查看时长有可能是错误的（比如合并前有两个视频，一个10秒，另一个15秒，合并后的视频还是15秒而不是25秒）。这会导致不同播放器播放合并后的视频出现各种问题（进度条拖到最后但是视频依然没有播放完，或者虽然拖动进度条到一半但实际进度却超过一半，中间过程丢失...等等问题）。那么如何把合并视频的时长修改正确呢？以下是解决方法：

- 使用**小丸工具箱**（官方网站：https://maruko.appinn.me/）
- 计算真实的视频时长：全选所有原视频，右键属性查看总时长。
- 打开**小丸工具箱**，**常用**分页的**其他**中有个**结束时刻**，把它设置成计算后的总时长。
- 拖拽合并后的视频到**其他**下面的视频空槽中，输出空槽会自动填写导出文件的名称（可二次编辑）。
- 点击**截取**按钮，等待新的视频文件生成。（不出意外的话，新视频文件既完成了文件合并，也完成了时长合并，完美）

# 附C#控制台程序源代码：

```c#
#define DRAG_WAY
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        public static void CombineFile(List<FileInfo> infile, String outfileName)
        {
            int b;
            int n = infile.Count;
            FileStream[] fileIn = new FileStream[n];
            using (FileStream fileOut = new FileStream(outfileName, FileMode.Create))
            {
                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        Console.WriteLine(infile[i].FullName + " 写入中...");
                        fileIn[i] = new FileStream(infile[i].FullName, FileMode.Open);
                        while ((b = fileIn[i].ReadByte()) != -1)
                            fileOut.WriteByte((byte)b);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        fileIn[i].Close();
                    }
                }
            }
        }
#if DRAG_WAY //拖拽版（更直观好用）
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("没有提供任何文件路径,请重新运行此程序（把多个文件拖拽到此程序上面运行）");
                Console.WriteLine("=========按回车可退出========");
                Console.ReadLine();
                return;
            }
            List<FileInfo> fileList = new List<FileInfo>();
            string destFileName = string.Empty;
            Console.WriteLine("即将以如下顺序合并文件：");
            for (int i = 0; i < args.Length; i++)
            {
                FileInfo file = new FileInfo(args[i]);
                if (!file.Exists)
                {
                    Console.WriteLine("错误的文件路径"+ args[i]);
                    continue;
                }
                if(string.IsNullOrEmpty(destFileName))
                {
                    destFileName = file.DirectoryName + "/" + file.Name.Replace(file.Extension, "") + "_Mix" + file.Extension;
                }
                Console.WriteLine(args[i]);
                fileList.Add(file);
            }
            Console.WriteLine("=========确认以这个顺序执行文件合并？回车键确认========");
            Console.ReadLine();
            Console.WriteLine("合并文件到："+ destFileName);
            CombineFile(fileList, destFileName);
            Console.WriteLine("=========执行完毕,按回车可退出========");
            Console.ReadLine();
        }

#else //自选合并or重命名版，要把程序放到对应文件夹下使用，规则特定，通用性不高
        static string ExtSearchName;
        static string ExtName;
        static int Max = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("请输入扩展名(不含.);");
            string Ext = Console.ReadLine();
            Console.WriteLine("合并选1，重命名选2");
            int Choose;
            int.TryParse(Console.ReadLine(), out Choose);
            ExtSearchName = "*." + Ext;
            ExtName = "." + Ext;
            if (Choose != 1 && Choose != 2 || string.IsNullOrEmpty(Ext) || Ext.ToLower() == "exe" || Ext.Contains("."))
            {
                Console.WriteLine("=========非法输入=========");
            }
            else if (Choose == 1)
            {
                Console.WriteLine("=========开始执行合并=========");
                DirectoryInfo dinfo = new DirectoryInfo("./");
                var files = dinfo.GetFiles(ExtSearchName);
                List<FileInfo> list = new List<FileInfo>();
                list.AddRange(files);
                list.Sort(SortFile);
                DirectoryInfo outInfo;
                if (Directory.Exists("./Output"))
                {
                    outInfo = new DirectoryInfo("./Output");
                }
                else
                {
                    outInfo = Directory.CreateDirectory("./Output");
                }

                var date = System.DateTime.Now;
                string name = $"{date.Year.ToString("0000")}{date.Month.ToString("00")}{date.Day.ToString("00")}_{date.Hour.ToString("00")}{date.Minute.ToString("00")}{date.Second.ToString("00")}";
                CombineFile(list,"./Output/"+ name + ExtName);
            }
            else if (Choose == 2)
            {
                Console.WriteLine("=========开始执行重命名========");
                DirectoryInfo dinfo = new DirectoryInfo("./");
                var files = dinfo.GetFiles(ExtSearchName);
                List<FileInfo> list = new List<FileInfo>();
                list.AddRange(files);
                list.Sort(SortFile);
                int leng = Max.ToString().Length;
                if (leng < 1)
                {
                    leng = 1;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var file = list[i];
                    Console.WriteLine(file.FullName);
                    int n = 0;
                    if (!int.TryParse(file.Name.Replace(ExtName, ""), out n))
                    {
                        continue;
                    }
                    string name = n.ToString();
                    int sLeng = name.Length;
                    while (sLeng < leng)
                    {
                        name = "0" + name;
                        sLeng = name.Length;
                    }
                    try
                    {
                        File.Move("./" + file.Name, "./" + name + ExtName);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Console.WriteLine("=========执行完毕,按回车可退出========");
            Console.ReadLine();
        }
        static int SortFile(FileInfo f1, FileInfo f2)
        {
            int n1 = 0, n2 = 0;
            int.TryParse(f1.Name.Replace(ExtName, ""), out n1);
            int.TryParse(f2.Name.Replace(ExtName, ""), out n2);
            Max = Math.Max(n1, Max);
            Max = Math.Max(n2, Max);
            return n1 - n2;
        }
#endif
    }
}

```
