# 前置知识

显示气泡的核心代码

```c#
using System.Windows.Forms;//这句话会报错找不到Forms 
//解决方法：VS里需要在解决方案侧边栏右击引用，选择添加引用。
//在程序集->框架 里面就可以找到System.Windows.Forms，打勾，确定

notifyTool = new NotifyIcon();
//设置气泡弹出时使用的程序图标
System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
System.IO.Stream imgStream = asm.GetManifestResourceStream("SVNNotifyer.Resources.256.ico");
notifyTool.Icon =  Resources._256;//SystemIcons.Information;
notifyTool.Visible = true;


notifyTool.ShowBalloonTip(time, title, str, ToolTipIcon.None);//时间单位为毫秒
```

显示弹窗的核心代码（目前暂未用到，可做技术积累）

```c#
using System.Windows.Forms;

string message = "You did not enter a server name. Cancel this operation?";
string caption = "Error Detected in Input";
MessageBoxButtons buttons = MessageBoxButtons.YesNo;
DialogResult result;

// Displays the MessageBox.
result = MessageBox.Show(message, caption, buttons);
if (result == System.Windows.Forms.DialogResult.Yes)
{}
```



# SVNNotifyer

exe 文件下载：[ SVNNotifyer.exe ](./SVNNotifyer.exe)

使用方法：可放到任意位置（比如桌面），可做快捷方式到桌面，支持设参（可设 0 到 2 个参数，第一个参数为分支路径，第二个参数为监听谁的提交，空格可用#代替），正确运行后，会不定期收到其他人向 svn 提交的气泡提醒。

C#控制台代码：

```c#
using SVNNotifyer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace SVNNotifyer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.DoMain(args);
        }
        //-----------------------------------------
        private string SvnPath;
        private string CheckAuthorName;
        private NotifyIcon notifyTool;
        Timer aTimer;
        private Process p;
        //已经打印过的log的最小、最大版本
        private long logVerMin = long.MaxValue;
        private long logVerMax = 0;
        private bool bLaterThanWin7 = false;
        private long NotifyOKTime = -1;
        private const int NotifyOKTimeAdd = 7500;//Win7中两个气泡的最短间隔(毫秒)
        private long RuningTime = 0;//程序监测阶段运行时长(毫秒)
        private const int SLEEP_TIME = 120;//主线程睡眠时间(毫秒)
        private char[] TRIMCHAR = new char[] { ' ', '\"', '\'' };
        public void DoMain(string[] args)
        {
            /*
             +------------------------------------------------------------------------------+
            |                    |   PlatformID    |   Major version   |   Minor version   |
            +------------------------------------------------------------------------------+
            | Windows 95         |  Win32Windows   |         4         |          0        |
            | Windows 98         |  Win32Windows   |         4         |         10        |
            | Windows Me         |  Win32Windows   |         4         |         90        |
            | Windows NT 4.0     |  Win32NT        |         4         |          0        |
            | Windows 2000       |  Win32NT        |         5         |          0        |
            | Windows XP         |  Win32NT        |         5         |          1        |
            | Windows 2003       |  Win32NT        |         5         |          2        |
            | Windows Vista      |  Win32NT        |         6         |          0        |
            | Windows 2008       |  Win32NT        |         6         |          0        |
            | Windows 7          |  Win32NT        |         6         |          1        |
            | Windows 2008 R2    |  Win32NT        |         6         |          1        |
            | Windows 8          |  Win32NT        |         6         |          2        |
            | Windows 8.1        |  Win32NT        |         6         |          3        |
            +------------------------------------------------------------------------------+
            | Windows 10         |  Win32NT        |        10         |          0        |
            +------------------------------------------------------------------------------+
             */
            bLaterThanWin7 = (Environment.OSVersion.Version.Major * 1000 + Environment.OSVersion.Version.Minor) > 6001;
            if (bLaterThanWin7)
            {
                Console.WriteLine(@"Win10若不能正常显示气泡，可修改注册表HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced");
                Console.WriteLine(@"设置或添加项EnableBalloonTips(类型REG_DWORD)并设置取值为1");
            }
            Console.WriteLine(@"支持exe参数指令(推荐用快捷方式)，规则为：svn路径 [被监听提交者全名]，其中路径和名字中出现的空格请用#代替，不区分大小写");
            Console.WriteLine("-------------------");
            bool argFormatIsRight = true;
            while (true)
            {
                if (argFormatIsRight && args.Length > 0)
                {
                    SvnPath = args[0].Replace("#", " ");
                }
                else
                {
                    Console.WriteLine("请输入svn目录路径:");
                    SvnPath = Console.ReadLine();
                }
                SvnPath = SvnPath.Trim(TRIMCHAR);
                string pathForCheck = SvnPath.ToLower();
                if (pathForCheck.StartsWith("http://")
                    || pathForCheck.StartsWith("https://")
                    || pathForCheck.StartsWith("file:///"))
                {
                    break;
                }
                else
                {
                    argFormatIsRight = false;
                    Console.Clear();
                    Console.WriteLine("路径非法,合法开头仅限以下三种：http://、https://、file:///");
                }
            }
            SvnPath = "\"" + SvnPath + "\"";//最后强制用双引号包一下，兼容路径带空格的情况
            if (argFormatIsRight && args.Length > 1)
            {
                CheckAuthorName = args[1].Replace("#"," ");
            }
            else
            {
                Console.WriteLine("请输入监听提交者的名字，填空代表监听全部:");
                CheckAuthorName = Console.ReadLine();
            }
            CheckAuthorName = CheckAuthorName.Trim(TRIMCHAR);
            if (string.IsNullOrEmpty(CheckAuthorName))
            {
                Console.WriteLine("开始监听全部提交者 ...");
            }
            else
            {
                Console.WriteLine("开始监听提交者 "+CheckAuthorName+" ...");
            }
            if (bLaterThanWin7)
            {
                WriteLine("空格键或回车可以暂停/恢复监测,其他键退出程序");
            }
            WriteLine("-------------------");
            p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.ErrorDataReceived += OutputCallBackError;
            p.OutputDataReceived += OutputCallBack;

            notifyTool = new NotifyIcon();
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            System.IO.Stream imgStream = asm.GetManifestResourceStream("SVNNotifyer.Resources.256.ico");
            notifyTool.Icon =  Resources._256;//SystemIcons.Information;
            notifyTool.Visible = true;
            aTimer = new Timer();
            aTimer.Interval = 120000;//120 seconds
            aTimer.Enabled = true;
            aTimer.Elapsed += new ElapsedEventHandler(Update);
            p.StandardInput.WriteLine("svn log -l 1 " + SvnPath);//在执行Update前先做一次拉取，拿到最高版本号
            p.StandardInput.AutoFlush = true;
            //不阻塞地检查按键
            while (true)
            {
                System.Threading.Thread.Sleep(SLEEP_TIME);
                if (bLaterThanWin7)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo c = Console.ReadKey(true);//传参：如果为 true，则不显示按下的键；否则为 false。
                        if (c.KeyChar == ' ' || c.KeyChar == '\r')
                        {
                            aTimer.Enabled = !aTimer.Enabled;
                            Console.WriteLine(aTimer.Enabled ? "=====已恢复=====" : "=====已暂停=====");
                        }
                        else if (c.KeyChar == '\n')
                        {
                            continue;
                        }
                        else
                        {
                            aTimer.Close();
                            p.Close();
                            return;
                        }
                    }
                }
                else
                {
                    RuningTime += SLEEP_TIME;
                    FlushWriteLine();
                    FlushTip();
                }
            }
        }
       
        public void Update(object source, ElapsedEventArgs e)
        {
            p.StandardInput.WriteLine(string.Format("svn log -r r{0}:head ", logVerMax) + SvnPath);
            p.StandardInput.AutoFlush = true;
        }

        //控制台逐行打印log的顺序阶段
        enum ElogState
        {
            rubish,//无用行
            //title,//标题行
            content,//日志行
        }
        private ElogState CurLogState = ElogState.rubish;
        string logTitle = string.Empty;
        string logContent = string.Empty;
        string logTimeStr = string.Empty;
        public void OutputCallBack(object sender, DataReceivedEventArgs e)
        {
            switch(CurLogState)
            {
                case ElogState.rubish:
                    if(e.Data.StartsWith("r") && e.Data.Contains("|") && e.Data.Contains("line"))
                    {
                        string[] strArr = e.Data.Split('|');
                        string strVer = strArr[0].Trim(' ');
                        string strAuthor = strArr[1].Trim(' ');
                        long versionNum;
                        long.TryParse(strVer.Replace("r",""), out versionNum);
                        if(versionNum < logVerMin || versionNum > logVerMax)
                        {
                            if (string.IsNullOrEmpty(CheckAuthorName) || CheckAuthorName.ToLower().Equals(strAuthor.ToLower()))
                            {
                                logVerMin = Math.Min(versionNum, logVerMin);
                                logVerMax = Math.Max(versionNum, logVerMax);
                                logTitle = strVer + " " + strAuthor;
                                logTimeStr = strArr[2].Trim(' ').Split(' ')[1];
                                CurLogState = ElogState.content;
                            }
                        }
                    }
                    break;
                case ElogState.content:
                    if(e.Data.StartsWith("--------------"))
                    {
                        CurLogState = ElogState.rubish;
                        logContent = logContent.TrimEnd('\n');
                        TipLog();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            logContent += (e.Data + "\n");
                        }
                    }
                    break;
            }

        }
        public void OutputCallBackError(object sender, DataReceivedEventArgs e)
        {
            WriteLine(e.Data);
        }
        public void TipLog()
        {
            if (bLaterThanWin7)
            {
                Tip(string.Empty, (logTitle + " " + logContent).Replace("\n", ""));
            }
            else
            {
                TipBuff(logTitle, logContent);
            }
            WriteLine(logTitle + " " + logTimeStr);
            WriteLine(logContent);
            WriteLine("-------------------");
            logTitle = string.Empty;
            logContent = string.Empty;
        }
        private const int TIME_TIP_MAX = 8000;
        private const int TIME_TIP_MIN = 3500;
        private const int TIME_PER_LETTER = 150;//假设3秒看20个字
        public void Tip(string title, string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return;
            }
            int time = (title.Length + str.Length) * TIME_PER_LETTER;
            if(time > TIME_TIP_MAX)
            {
                time = TIME_TIP_MAX;
            }
            else if(time < TIME_TIP_MIN)
            {
                time = TIME_TIP_MIN;
            }
            notifyTool.ShowBalloonTip(time, title, str, ToolTipIcon.None);
        }
        private List<string> BuffWriteLine = new List<string>();
        public void WriteLine(string str)
        {
            if(bLaterThanWin7)
            {
                Console.WriteLine(str);
            }
            else
            {
                BuffWriteLine.Add(str);
            }
        }
        //win7及以下版本用这个释放控制台输出缓存
        public void FlushWriteLine()
        {
            int count = BuffWriteLine.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(BuffWriteLine[i]);
                }
                BuffWriteLine.Clear();
            }
        }
        class StringVec2
        {
            public string x;
            public string y;
            public StringVec2(string x, string y)
            {
                this.x = x;
                this.y = y;
            }
        }
        private List<StringVec2> BuffNotify = new List<StringVec2>();
        public void TipBuff(string title, string str)
        {
            BuffNotify.Add(new StringVec2(title, str));
        }
        public void FlushTip()
        {
            if(BuffNotify.Count > 0)
            {
                if(RuningTime < NotifyOKTime)
                {
                    return;
                }
                StringVec2 vec = BuffNotify[0];
                Tip(vec.x, vec.y);
                NotifyOKTime = RuningTime + NotifyOKTimeAdd;
                BuffNotify.RemoveAt(0);
            }
        }
    }
}

```



# GitNotifyer

exe 文件下载：[ GitNotifyer.exe ](./GitNotifyer.exe)

使用方法：必须放到 git 工程根目录，但可做快捷方式到桌面，支持设参（可设 0 到 2 个参数，第一个参数为分支路径，第二个参数为监听谁的提交，空格可用#代替），正确运行后，会不定期收到其他人向 git 提交的气泡提醒。

C#控制台代码：

```c#
using GitNotifyer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace GitNotifyer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.DoMain(args);
        }
        //-----------------------------------------
        //控制台逐行打印log的顺序阶段
        enum ElogState
        {
            Version,//版本行
            Author,//作者行
            Date,//日期行
            content,//日志行
        }
        //已经打印过的log版本
        private string logVerMax = string.Empty;
        private ElogState CurLogState = ElogState.Version;
        private string logVersion = string.Empty;
        private string logAuthorName = string.Empty;
        private string logDateStr = string.Empty;
        private string logTimeStr = string.Empty;
        private string logContent = string.Empty;
        private string BranchName = "master";
        private string CheckAuthorName;
        private string CheckAuthorNameCommand;
        private NotifyIcon notifyTool;
        private Timer aTimer;
        private Process p;
        private bool bLaterThanWin7 = false;
        private long NotifyOKTime = -1;
        private const int NotifyOKTimeAdd = 7500;//Win7中两个气泡的最短间隔(毫秒)
        private long RuningTime = 0;//程序监测阶段运行时长(毫秒)
        private const int SLEEP_TIME = 120;//主线程睡眠时间(毫秒)
        private char[] TRIMCHAR = new char[] { ' ', '\"', '\'' };
        public void DoMain(string[] args)
        {
            /*
             +------------------------------------------------------------------------------+
            |                    |   PlatformID    |   Major version   |   Minor version   |
            +------------------------------------------------------------------------------+
            | Windows 95         |  Win32Windows   |         4         |          0        |
            | Windows 98         |  Win32Windows   |         4         |         10        |
            | Windows Me         |  Win32Windows   |         4         |         90        |
            | Windows NT 4.0     |  Win32NT        |         4         |          0        |
            | Windows 2000       |  Win32NT        |         5         |          0        |
            | Windows XP         |  Win32NT        |         5         |          1        |
            | Windows 2003       |  Win32NT        |         5         |          2        |
            | Windows Vista      |  Win32NT        |         6         |          0        |
            | Windows 2008       |  Win32NT        |         6         |          0        |
            | Windows 7          |  Win32NT        |         6         |          1        |
            | Windows 2008 R2    |  Win32NT        |         6         |          1        |
            | Windows 8          |  Win32NT        |         6         |          2        |
            | Windows 8.1        |  Win32NT        |         6         |          3        |
            +------------------------------------------------------------------------------+
            | Windows 10         |  Win32NT        |        10         |          0        |
            +------------------------------------------------------------------------------+
             */
            bLaterThanWin7 = (Environment.OSVersion.Version.Major * 1000 + Environment.OSVersion.Version.Minor) > 6001;
            if (bLaterThanWin7)
            {
                Console.WriteLine(@"Win10若不能正常显示气泡，可修改注册表HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced");
                Console.WriteLine(@"设置或添加项EnableBalloonTips(类型REG_DWORD)并设置取值为1");
            }
            Console.WriteLine(@"【注意】该exe需要放在git工程下运行");
            Console.WriteLine(@"支持exe参数指令(推荐用快捷方式)，规则为：[master或其他分支名称][被监听提交者名称]");
            Console.WriteLine(@"这两个名称中出现的空格请用#代替，其中提交者名称可以是区分大小写的部分字符串。");
            Console.WriteLine("-------------------");
            switch(args.Length)
            {
                case 0:
                    Console.WriteLine("请输入分支名称（填空则为主干master）:");
                    BranchName = Console.ReadLine();
                    Console.WriteLine("请输入监听提交者的名字（填空代表监听全部）:");
                    CheckAuthorName = Console.ReadLine();
                    break;
                case 1:
                    BranchName = args[0];
                    Console.WriteLine("请输入监听提交者的名字（填空代表监听全部）:");
                    CheckAuthorName = Console.ReadLine();
                    break;
                case 2:
                default:
                    BranchName = args[0];
                    CheckAuthorName = args[1];
                    break;
            }
            BranchName = BranchName.Replace("#", " ").Trim(TRIMCHAR);
            CheckAuthorName = CheckAuthorName.Replace("#", " ").Trim(TRIMCHAR);
            if(string.IsNullOrEmpty(BranchName))
            {
                BranchName = "master";
            }
            if (string.IsNullOrEmpty(CheckAuthorName))
            {
                Console.WriteLine("开始监听全部提交者 ...");
                CheckAuthorNameCommand = string.Empty;
            }
            else
            {
                Console.WriteLine("开始监听提交者 "+CheckAuthorName+" ...");
                CheckAuthorNameCommand = string.Format(" --author=\"{0}\"", CheckAuthorName);
            }
            if (bLaterThanWin7)
            {
                WriteLine("空格键或回车可以暂停/恢复监测,其他键退出程序");
            }
            WriteLine("-------------------");
            p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.ErrorDataReceived += OutputCallBackError;
            p.OutputDataReceived += OutputCallBack;

            notifyTool = new NotifyIcon();
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            System.IO.Stream imgStream = asm.GetManifestResourceStream("GitNotifyer.Resources.256.ico");
            notifyTool.Icon =  Resources._256;//SystemIcons.Information;
            notifyTool.Visible = true;
            aTimer = new Timer();
            aTimer.Interval = 120000;//120 seconds
            aTimer.Enabled = true;
            aTimer.Elapsed += new ElapsedEventHandler(Update);
            p.StandardInput.WriteLine("git config --global core.quotepath false");
            p.StandardInput.WriteLine("git config --global gui.encoding utf-8");
            p.StandardInput.WriteLine("git config --global i18n.commit.encoding utf-8");
            p.StandardInput.WriteLine("git config --global i18n.logoutputencoding gbk");
            p.StandardInput.WriteLine("set LESSCHARSET=gbk");
            p.StandardInput.AutoFlush = false;
            p.StandardInput.WriteLine("git fetch origin "+ BranchName);
            p.StandardInput.WriteLine("git log remotes/origin/"+ BranchName + " --date=iso -n1" + CheckAuthorNameCommand);//在执行Update前先做一次拉取，拿到最高版本号
            p.StandardInput.AutoFlush = true;
            //不阻塞地检查按键
            while (true)
            {
                System.Threading.Thread.Sleep(SLEEP_TIME);
                if (bLaterThanWin7)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo c = Console.ReadKey(true);//传参：如果为 true，则不显示按下的键；否则为 false。
                        if (c.KeyChar == ' ' || c.KeyChar == '\r')
                        {
                            aTimer.Enabled = !aTimer.Enabled;
                            Console.WriteLine(aTimer.Enabled ? "=====已恢复=====" : "=====已暂停=====");
                        }
                        else if (c.KeyChar == '\n')
                        {
                            continue;
                        }
                        else
                        {
                            aTimer.Close();
                            p.Close();
                            return;
                        }
                    }
                }
                else
                {
                    RuningTime += SLEEP_TIME;
                    FlushWriteLine();
                    FlushTip();
                }
            }
        }
       
        public void Update(object source, ElapsedEventArgs e)
        {
            if(string.IsNullOrEmpty(logVerMax))
            {
                return;
            }
            p.StandardInput.AutoFlush = false;
            p.StandardInput.WriteLine("git fetch origin " + BranchName);
            p.StandardInput.WriteLine(string.Format("git log remotes/origin/{0} --date=iso --reverse {1}..HEAD", BranchName, logVerMax) + CheckAuthorNameCommand);
            p.StandardInput.AutoFlush = true;
        }
        
       
        public void OutputCallBack(object sender, DataReceivedEventArgs e)
        {
            if(CurLogState == ElogState.Version && e.Data.StartsWith("commit "))
            {
                logVersion = e.Data.Split(' ')[1];
                if (!logVersion.Equals(logVerMax))
                {
                    logVerMax = logVersion;
                }
            }
            else if(CurLogState == ElogState.Version && e.Data.StartsWith("Author: "))
            {
                logAuthorName = string.Empty;
                string[] arr = e.Data.Split(' ');
                for(int i=1;i<arr.Length;i++)
                {
                    string arrstr = arr[i];
                    if(arrstr.StartsWith("<"))
                    {
                        break;
                    }
                    if(!string.IsNullOrEmpty(logAuthorName))
                    {
                        logAuthorName += " ";
                    }
                    logAuthorName += arrstr;
                }
                CurLogState = ElogState.Author;
            }
            else if(CurLogState == ElogState.Author && e.Data.StartsWith("Date: "))
            {
                string[] arr = e.Data.Split(' ');
                logDateStr = arr[arr.Length - 3];
                logTimeStr = arr[arr.Length - 2];
                CurLogState = ElogState.Date;
            }
            else if(string.IsNullOrWhiteSpace(e.Data))
            {
                if(CurLogState == ElogState.Date)
                {
                    logContent = string.Empty;
                }
                else if(CurLogState == ElogState.content)
                {
                    TipLog();
                    CurLogState = ElogState.Version;
                }
            }
            else
            {
                if (CurLogState == ElogState.Date)
                {
                    CurLogState = ElogState.content;
                }
                if (CurLogState == ElogState.content)
                {
                    if (!string.IsNullOrEmpty(logContent))
                    {
                        logContent += "\n";
                    }
                    logContent += e.Data.TrimStart(' ');
                }
            }
        }
        public void OutputCallBackError(object sender, DataReceivedEventArgs e)
        {
            string str = e.Data;
            //屏蔽fetch指令的正常返回信息，git的fetch结果是error返回
            if(string.IsNullOrWhiteSpace(str) || str.StartsWith("From ") || str.Contains(BranchName) && str.Contains(" -> "))
            {
                return;
            }
            WriteLine(str);
        }
        public void TipLog()
        {
            if (bLaterThanWin7)
            {
                Tip(string.Empty, (logAuthorName + " " + logContent).Replace("\n", ""));
            }
            else
            {
                TipBuff(logAuthorName + " " + logTimeStr, logContent);
            }
            WriteLine(logAuthorName + " " + logDateStr + "/" + logTimeStr + " 版本号: " + logVersion);
            WriteLine(logContent);
            WriteLine("-------------------");
            logAuthorName = string.Empty;
            logTimeStr = string.Empty;
            logContent = string.Empty;
        }
        private const int TIME_TIP_MAX = 8000;
        private const int TIME_TIP_MIN = 3500;
        private const int TIME_PER_LETTER = 150;//假设3秒看20个字
        public void Tip(string title, string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return;
            }
            int time = (title.Length + str.Length) * TIME_PER_LETTER;
            if(time > TIME_TIP_MAX)
            {
                time = TIME_TIP_MAX;
            }
            else if(time < TIME_TIP_MIN)
            {
                time = TIME_TIP_MIN;
            }
            notifyTool.ShowBalloonTip(time, title, str, ToolTipIcon.None);
        }
        private List<string> BuffWriteLine = new List<string>();
        public void WriteLine(string str)
        {
            if(bLaterThanWin7)
            {
                Console.WriteLine(str);
            }
            else
            {
                BuffWriteLine.Add(str);
            }
        }
        //win7及以下版本用这个释放控制台输出缓存
        public void FlushWriteLine()
        {
            int count = BuffWriteLine.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(BuffWriteLine[i]);
                }
                BuffWriteLine.Clear();
            }
        }
        class StringVec2
        {
            public string x;
            public string y;
            public StringVec2(string x, string y)
            {
                this.x = x;
                this.y = y;
            }
        }
        private List<StringVec2> BuffNotify = new List<StringVec2>();
        public void TipBuff(string title, string str)
        {
            BuffNotify.Add(new StringVec2(title, str));
        }
        public void FlushTip()
        {
            if(BuffNotify.Count > 0)
            {
                if(RuningTime < NotifyOKTime)
                {
                    return;
                }
                StringVec2 vec = BuffNotify[0];
                Tip(vec.x, vec.y);
                NotifyOKTime = RuningTime + NotifyOKTimeAdd;
                BuffNotify.RemoveAt(0);
            }
        }
    }
}
```

# Git 小常识

因为这篇文章和 git 关系比较密切，所以平时一些 git 的用法心得也记在这里吧：

## Git的两种仓库类型

1. 有remote配置：从remote克隆（带origin）

2. 无remote配置：git init（纯本地仓库）

   相当于执行了 `git init git add . git commit -m "Initial"`，全程无需remote端，只有一个 .git 隐藏文件夹。该仓库特性验证： git -C "具体路径" remote -v 将显示无remote配置

