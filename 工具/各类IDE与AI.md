# IDE

**IDE**（Integrated Development Environment，集成开发环境）是一种**将多种编程工具整合到单一图形界面**的软件，旨在**提升开发效率**。与纯文本编辑器（如记事本、Vim）或基础代码编辑器（如 Sublime Text）不同，IDE 通常深度整合了**编码、调试、编译、测试、版本控制**等全流程功能。

## Rider

*与游戏引擎 Unity、UE 集成度最高*

### 资源链接：

链接：https://pan.baidu.com/s/1qa1MkupwHCbIEEK1sHUUPg?pwd = hpdt
提取码：hpdt
--来自百度网盘超级会员 V7 的分享

其中有个  `Rider破解教程.rar`  也包含源头资源链接，是从某宝上整理的，如过期记得联系卖家。

### 使用心得：

最常用快捷键 ctrl+q 搜索，alt+回车呼出当前位置上下文操作，F12 跳转到引用，其他忘了可以到教学里复习

刚装完需要汉化，到 **设置-插件** 那里，搜 chinese，大约 28M 的那个插件就是。

rider 自带的 AI（从 2024 版开始有）需要正版付费用户登录，而破解版我推荐使用第三方 AI，百度的 comate 插件就不错（2021 版以后都支持，就是安装下载慢）。

以下是常用设置：

- 【设置】编辑器-常规-编辑器标签页-勾选“标记已修改”
- 【设置】编辑器-配色方案-常规-错误和警告-未使用的代码那里，修改颜色为暗青色（推荐 578389）并勾选斜体（若改回原色就填 787878），用来区分 AI 自动补全的代码和非活跃变体代码颜色。
- 【设置】编辑器-嵌入提示-C++-参数名称提示-函数形参的可见性：Push-to-Hint（长按 ctrl 键）
- 【设置】编辑器-嵌入提示-C++-参数名称提示-宏形参的可见性：Push-to-Hint（长按 ctrl 键）
- 【设置】编辑器-嵌入提示-C++-其他-预处理程序指令提示-可见性：Push-to-Hint（长按 ctrl 键）
- 【设置】编辑器-常规-外观-勾选“滚动时显示粘性行”
- 【设置】编辑器-检查设置-检查严重性-C++-常见做法和代码改进-取消勾选“局部变量可被设为 const”和“形参可被设为 const”
- 【检查严重性】其他语言-校对-取消勾选“拼写错误”
- 【菜单栏】视图-活动编辑器-自动换行（酌情处理）
- 【设置】编辑器-CodeVision-非活跃的着色器变体分支（酌情处理）
- 鼠标晃到右上角代码分析报错那里，鼠标悬停出气泡后可以设置代码分析的开关，如误操作关闭可重复操作开启
- 在【设置】里搜 "命名不一致"，c++里的约束违规取消勾选，其他酌情处理。
- 关闭 Win11 系统下输入法的快捷键：时间和语言 > 语言和区域 > 按键---热键  简体/繁体中文输入切换（ctrl+shift+F）
- 【设置】语言和框架-Unity 引擎-取消勾选“在 Unity 中自动刷新”

### 插件安装路径：

C:\Users\用户名\AppData\Roaming\JetBrains\Rider2024.1\plugins

### AI 插件

- Comate：百度旗下
- 通义灵码：阿里旗下
- DeepSeek：[本地部署 deepseekR1，rider+qwen coder 提高生产力 - 知乎](https://zhuanlan.zhihu.com/p/22052237965)
  可以通过安装 Continue 插件（也许 Cline 更好？待验证），输入 DeepSeek 的 API key 之后即可使用，配置文件会生成在

  ```c#
  C:\Users\用户名\.continue
  ```

## VSCode

*轻量但强大的 IDE。最佳的 AI 工具试验田。*

### 联动Unity

- Unity端安装Visual Studio Editor包
- VSCode端安装 Unity 插件（会自动连带安装依赖插件`.NET Install Tool` 和 `C# Dev Kit`）
- 通过Unity打开VSCode会让工程目录根目录自动生成 `.vscode` 目录
- 如果VSCode资源管理器看不到 .vscode 目录、看到有（工作区）标记、C#代码查不到跨文件引用、代码提示下拉菜单缺少API等问题。请处理：
  - 删除工作区配置文件（根目录扩展名 .code-workspace）。因为VSCode 会优先读取 `.code-workspace` 的设置，而非 `.vscode/settings.json`
  - 如果不删除工作区配置文件，那需要知道怎么维护它，暂不赘述。
- 调试：侧边栏调试面板，▶️Attach to Unity 中的 ▶️ 按钮（快捷键F5）。
  - 这个配置具体在：.vscode 目录中的 launch.json 文件，如无法调试可尝试重新生成 .vscode。

### AI 插件

- 主要负责自动代码补全：Comate（内置zulu很好用）、通义灵码
- Cline插件：具体下文跳转 [VSCode 安装 DeepSeek](#VSCode 安装 DeepSeek)

> 参考文章：
> [VScode+Cline+Deepseek 实现媲美 cursor 的代码自动生成](https://blog.51cto.com/u_14249042/13045983)

### 代码格式化

推荐安装 **Prettier** 插件，可以把没有换行的 js 脚本格式化成带换行缩进的方式（右键菜单：使用...格式化文档）。

### 大纲预览

推荐安装 **Outline Map** 插件，类似于 VA 插件的 VA Outline 功能。

但是 Outline Map 不支持 shader 文件的大纲展示。所以我自己做了一个插件基本支持了shader，hlsl，glsl语法，安装插件后，不但原生大纲可以显示内容了，连带 Outline Map 插件也一并支持显示了。

- Github地址：[wangbenchong/vscode-shader-outline](https://github.com/wangbenchong/vscode-shader-outline)
- .vsix 插件安装包：[shader-outline-0.0.3.vsix](./files/shader-outline-0.0.3.vsix)

### 支持MarkDown文档

推荐安装 **Office Viewer** 插件，如果不喜欢这个插件自带的VSCode颜色主题，可以在VSCode左下角齿轮里改回自己惯用的颜色主题（目前使用最舒适的是VS2019 Dark主题）。

### C盘空间占用

日常使用会在 `C:\Users\用户名\AppData\Roaming\Code\User\globalStorage` 中建立一些文件，可酌情清理

### 解决 Git 相关终端指令中文乱码

VSCode默认的源代码管理（Ctrl+Shift+G）有个图形界面可以显示git提交日志的树状图，但是默认情况下中文显示乱码（乱码表现为方块 `□` 而非问号 `?`），这是由于 git 输出日志的编码不是标准的utf8 ，验证方式——在终端输入以下指令：

```bash
#如果日志量很大也可以不用 --all
git log --graph --oneline --all
```

如果打印乱码则证实了我们的推断，那么执行这两条指令即可

```bash
#处理提交信息的编码
git config --global i18n.commitencoding utf-8
#处理日志输出的编码
git config --global i18n.logoutputencoding utf-8
```

Git有两个重要的国际化配置项：`i18n.commitEncoding`和`i18n.logOutputEncoding`。这两个配置分别处理提交信息的编码和日志输出的编码。

当用户设置了`i18n.commitEncoding`为utf-8时，这告诉Git在提交信息时使用UTF-8编码存储。如果之前没有设置，Git可能默认使用系统的本地编码（比如GBK），导致在跨平台或不同环境中查看时出现乱码。不过，用户的问题主要是在日志显示，所以这个配置可能更多是预防提交时的编码问题。

而`i18n.logOutputEncoding`设置为utf-8，则是确保`git log`等命令输出日志时使用UTF-8编码。这样，当VSCode的终端或界面使用UTF-8来显示时，中文字符就能正确渲染，而不会出现乱码。特别是在Windows系统下，终端的默认编码可能不是UTF-8，这时候明确指定输出编码就非常重要了。

### Git功能额外扩展插件

如果不满足于VSCode默认的源代码管理（Ctrl+Shift+G），还可以使用以下扩展：

1. **Git Graph 扩展**（最常用）
   安装后点击底部状态栏的图标，会弹出交互式提交树，支持：
   - 分支合并关系可视化（类似 `git log --graph`）
   - 点击提交查看详情
   - 右键执行 checkout/reset/rebase 等操作
   - 按下 `Ctrl+Shift+P` 打开命令面板，输入 `Git: View History (git log)` 或 `Git Graph: View` 即可调出。
2. **GitLens 扩展**（增强型整合）
   在源代码文件的行号旁显示提交记录，通过 `GitLens: Commit Graph` 命令可打开提交树视图。

## VS

- 常用插件：VA
- 注意：新版 Unity 打包 LC2CPP 版本的 PC 端需要新版 VS（截止 2025 年初，需要 VS2022 社区版）

## Script Inspector 3

*Unity 插件，内置在 Unity 中的 IDE*

https://www.bilibili.com/video/BV1Pe4y1p77D

## NotePad++

纯文本编辑器，但支持自定义语法，比如自定义 shader 语法：[shader_(Notepad++语法).xml](./files/shader_(Notepad++语法).xml)

## Cursor

*Anysphere 旗下*

- [（官网）下载：Cursor - The AI Code Editor](https://www.cursor.com/cn)
- [（官网）对话次数用量查询](https://www.cursor.com/cn/settings)
- 聊天的三种模式：

  - Agent模式：AI可以自主学习您的代码库，并代表您进行代码库范围的更改
  - Ask模式：获取关于代码库的解释和答案，并规划功能
  - Manual模式：进行集中编辑，仅使用您提供的上下文
- Cursor内置模型的一些集成功能（给Cursor充钱的所有卖点）：

  - **Tab功能**

    Tab是Cursor的智能代码补全功能，具有以下特点：

    1. 多行编辑 - 可以一次性建议多行代码的修改，节省您的时间
    2. 智能重写 - 可以修复您的代码错误和拼写错误
    3. 光标预测 - 预测您的下一个光标位置，使代码导航更加流畅
    4. 始终开启 - 一旦启用，Tab功能会持续工作，根据您最近的更改建议编辑

    Tab还支持聊天标签页功能，让您可以同时运行多个AI对话。使用⌘+T(Mac)或Ctrl+T(Windows/Linux)创建新标签页。
  - **Apply from chat功能**（这个在VSCode用Cline插件是自带的）

    Apply功能允许您将AI聊天中的代码建议直接应用到您的代码库：

    1. 当AI在聊天中建议代码更改时，您可以查看差异视图中的建议更改
    2. 点击代码块顶部的播放按钮，将建议的代码直接应用到您的代码库中
    3. 在Ask模式下，您需要明确点击"Apply"按钮才能应用更改
    4. 应用更改后，您可以决定是保留还是放弃这些更改

    Cursor使用自定义训练的模型来应用更改，即使是处理数千行的大型文件也能在几秒钟内完成。
  - **Composer功能**（这个在VSCode用Cline插件是自带的）

    1. 多文件编辑：同时创建或修改多个文件
    2. 完整应用生成：基于高级描述开发完整应用
    3. 上下文理解：AI考虑整个项目结构和现有代码
    4. 交互式改进：提供额外指令来完善生成的代码
- 通过安装插件**改为中文**：菜单栏File > Preference > Extension（扩展），搜索**Chinese**，安装后重启
- 同理，安装 **Cursor Stats 插件**可以在右下角**查看剩余对话次数**（新账号送50次收费模型，每月送200次 gpt-40-mini 或cursor-small 模型，用完要交钱）
- 使用已单独付费模型（以 deepseek 为例）:

  - 打开 设置页面 > Models 分页
  - 点击 “+ Add model”
  - 输入“deepseek-chat” 或 “deepseek-reasoner”（都是 deepseek 最新版模型名字，具体可到 [deepseek官网]([首次调用 API | DeepSeek API Docs](https://api-docs.deepseek.com/zh-cn/)) 确认）
  - 在 `OpenAI API Key` 选项下面展开 `Override OpenAI Base URL`，输入 https://api.deepseek.com 点击Save， 再填入对应的的 API Key，点击 Verify。
  - （注：使用已单独付费的模型可以绕过cursor的收费，但也缺少了Cursor内置模型的一些集成功能（见上文），即对话模式只能设置为Ask，不能用Agent，因此也不能用MCP。不给Cursor掏钱，就不给你好功能，是时候换用VSCode了）
- 当然，也可以通过 [一些手段]([Cursor 全攻略：注册、使用到无限续杯，一次性讲清楚 - 知乎](https://zhuanlan.zhihu.com/p/23874722853)) 无限白嫖Cursor的内置模型，如果不嫌折腾的话。

# 常见 AI 大模型

## DeepSeek

官网：[DeepSeek | 深度求索](https://www.deepseek.com/)

*可用手机号注册*

创建自己的 APIKeys：[DeepSeek 开放平台](https://platform.deepseek.com/api_keys)  

目前的问题：~~自 2025.2.7 至 2025.2.13，因服务器压力已关闭充值渠道，且无免费额度，导致新用户无法使用，会在 IDE 里报 402 错误。~~

### VSCode 安装 DeepSeek

- 先在 VSCode 上装个 Cline 插件，填入 deepseek 的 API key，选则“deepseek-chat” 或 “deepseek-reasoner”模型（都是 deepseek 最新版模型名字，具体可到 [deepseek官网]([首次调用 API | DeepSeek API Docs](https://api-docs.deepseek.com/zh-cn/)) 确认），即可使用 deepseek。
- 以下配置可以增进使用体验：

  1. [解决 VSCode 中 Cline 终端集成问题](https://blog.csdn.net/weixin_43627179/article/details/144898326)，摘要如下：

     - 遇到的问题：在与 Cline 对话的过程中发现报错：Cline won’t be able to view the command’s output.
     - 如果 PowerShell 配置文件不存在，需要先创建它：

     ```powershell
     New-Item -Path $PROFILE -ItemType File -Force
     ```

     - 在 PowerShell 配置文件中添加以下代码：

     ```powershell
     if ($env:TERM_PROGRAM -eq "vscode") { 
         . "$(code --locate-shell-integration-path pwsh)" 
     }
     ```

     - 如果遇到脚本执行被阻止的错误，需要修改执行策略：

     ```powershell
     Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
     ```

     - 完成以上步骤后，重启 VSCode。
     - 在终端中运行以下命令测试集成是否成功：

     ```powershell
     echo "终端测试 - 如果看到此消息，说明终端工作正常"
     ```
  2. 安装 [Node.js — Run JavaScript Everywhere](https://nodejs.org/en)
  3. 安装 PowerShell 7，让 Cline 可以在调用 PowerShell 指令的时候更强大（比如支持&&作为语句分割符）

     - [【官方引导】从 Windows PowerShell 5.1 迁移到 PowerShell 7](https://learn.microsoft.com/zh-cn/powershell/scripting/whats-new/migrating-from-windows-powershell-51-to-powershell-7?view=powershell-7.5) 或直接通过 [github 下载](https://github.com/PowerShell/PowerShell/releases)
     - 安装后重启 VSCode，终端自动使用新版 PS（如果没有就到终端视图右上角，展开箭头自行切换）
     - 测试执行 PowerShell 指令： node -v && npm -v ，语法通过就证明装好了。
  4. 取消勾选 Cline 插件的 Enable Checkpoints 设置，否则 VSCode 打开过的项目有可能在 "C:\Users\wang\AppData\Roaming\Code\User\globalStorage\saoudrizwan.claude-dev\checkpoints" 目录生成很大的.git 文件夹（git 本地仓库），其内容是 **完全拷贝一份项目**（为了确保分析的完整性、提供回滚能力、隔离工作环境），我觉得通常来说不需要这个功能（即便需要也应该想办法把做目录迁移、并且做一些 git 忽略），于是清空了 checkpoints 目录下的所有内容，防止C盘被撑满。

## 文心一言

*百度旗下*

[文心一言](https://yiyan.baidu.com/)

[文心快码(Baidu Comate)·更懂你的智能代码助手](https://comate.baidu.com/zh)，目前仍然推荐用它来做 VSCode 的 ==代码补全==。

## 豆包

*抖音旗下*

[豆包 MarsCode - 编程助手](https://www.marscode.cn/home)

## 混元

腾讯旗下，可通过下载“腾讯元宝”客户端来使用，腾讯元宝内置混元、满血 deepseek 两个模型。==用它翻译技术文章很不错==。

网页版：https://yuanbao.tencent.com/chat/

## Claude

需要梯子，有客户端，https://www.anthropic.com/

如果用Cursor内置模型可以不翻墙

# 其他 AI 相关

## 游戏引擎使用 AI

### Unity 的 Muse

收费，体验一般，相关包：

- Unity Muse Chat（包名 com.unity.muse.chat）
- Sentis (包名 com.unity.sentis)
- Muse Sprite Tool
- Muse Texture Tool

### Unity 自动生成指定规则的Mono脚本

- 视频：[Unity AI脚本生成器_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1ftZJYBEqH)
- Github原版插件（非package）：https://github.com/OSCAR-hi/Unity-Scripts-Generator
- 在此基础上我做了一些修改：[DeepSeek Script Generator](../Unity/DeepSeek Script Generator/ReadMe.md)

## MCP

### MCP科普及常用工具

- [MCP是啥？技术原理是什么？一个视频搞懂MCP的一切。Windows系统配置MCP，Cursor Cline使用MCP_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1AnQNYxEsy)  [YouTube版](https://www.youtube.com/watch?v=McNRkd5CxFY&t=16s)
- [一口气介绍15种MCP工具，CherryStudio配置MCP_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1MjZ5YrESn)  [YouTube版](https://www.youtube.com/watch?v=iqnE6jt2lPU)
- [MCP幼儿园级教程，让你的AI自己干活！【小白教程】](https://www.youtube.com/watch?v=jz4SxHZhJok)
- [Cursor+ MCP + Blender = 人人都能3D建模？_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1iSQoY4EQB?spm_id_from=333.788.recommend_more_video.-1&vd_source=563d44869c3ecebb1867233573d16b7b)

### Unity 中使用 MCP

看了以下几个视频了解到有MCP（Model Context Protocol，让AI更有执行能力）这种概念，不了解MCP的可以先看看：

- [使用 AI 操作 Unity 来开发游戏_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1AkoUYZEf6)
- [Unity+MCP AI 操作 Unity 来开发游戏_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1kzoWYXECJ)

以下是通过安装Unity包来让Untiy支持MCP的几个版本（有的版本测试暂未成功，可能缺少某些依赖环境或者安装时的最新版有bug，但可见其繁琐程度，所以测试不通过的仅列出，留待观察）：

- 版本 1，Unity 包（支持 Claude Desktop, Cursor）：[justinpbarnett/unity-mcp](https://github.com/justinpbarnett/unity-mcp?path=/UnityMcpBridge)

  > 测试不通过：暂时卡在建 python server 这一步
  >
- 版本 2，Unity 包（支持 Claude Desktop, Windsurf, Cursor）：[CoderGamester/mcp-unity](https://github.com/CoderGamester/mcp-unity)

  > Claude Desktop：暂时卡在 Claude 设置好 MCP 之后有失败 Failed 标记。
  >
  > Cursor：**直接成功**，但只能使用内置模型，不支持填入 apikey 的模型（无法开启Agent对话模式），而想要使用内置模型需要给 Cursor 支付昂贵的费用，不划算。
  >
  > **VSCode+Cline+deepseek**：**成功，推荐采用**。具体步骤：
  >
  > 1. [VSCode 安装 DeepSeek](#VSCode 安装 DeepSeek)
  > 2. Unity包安装好以后（为了方便扩展功能，建议 [把包迁移到Packages目录](../Unity/将Package包迁移出Library目录.md)），菜单栏 Tools > MCP Unity > Server Window，窗口打开后复制里面的代码
  > 3. 插件中有个 VsCodeWorkspaceUtils 脚本，其中的 AddPackageCacheToWorkspace() 方法内容建议注释掉，不然强制生成VSCode工作区文件（.code-workspace扩展名）会破坏VSCode的C#支持，导致无法跨文件查找代码引用。
  > 4. 到VSCode里对Cline插件的AI说：帮我创建一个mcp服务器，配置如下：（粘贴代码）
  > 5. AI会自动在 `c:\Users\用户名\AppData\Roaming\Code\User\globalStorage\saoudrizwan.claude-dev\settings\cline_mcp_settings.json` 中记录你之前粘贴的代码（整个过程会展示给你看，每一步都是经过你的同意才会执行）。这代表着Cline的MCP服务器已完成安装配置（查看已安装的MCP服务器可以在Cline右上角加号旁边的 MCP Servers 选项 -> Installed 分页下）。
  > 6. 对AI说：在unity里打印以下日志：这条日志通过VScode调用MCP打印
  > 7. 如果Unity的Console成功打印了这个日志，代表MCP功能已经走通了。
  >
  > 
- 版本 3，Unity 包（支持 Claude Desktop）：[quazaai/UnityMCPIntegration](https://github.com/quazaai/UnityMCPIntegration)

  > 测试不通过：卡在建立server这一步，安装Untiy包之后尝试通过编辑器窗口直接启动服务器失败。
  >
- 版本 4 ，Unity 包（C#控制台程序，支持 OpenAI，计划支持 deepseek）：[rob1997/CereBro](https://github.com/rob1997/CereBro)

  > 待验证但搁置，因为AI聊天的部分是单独做了一个程序，这部分功能做的有点没必要，像版本2那样直接用VSCode的Cline插件就好。
  >

### UE 中使用 MCP

- [【一】MCP-to-虚幻引擎新 Ai 技术强势来袭。你准备好了吗？(只需输入提示词就可做完整游戏）_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1yiR1Y7EUv)
- [【二】虚幻引擎 MCP 服务器入门五大核心技巧（附赠 Cursor/Windsurf/GitHub Copilot 用户专属秘籍）_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1zMdhYPERu)

## 文生图

- 文心一言网页版：[文心一言](https://yiyan.baidu.com/)
- DeekSeek（默认不支持，但可通过一些技巧实现）：

  - [佬教你让 deepseek 支持文生图 - 知乎](https://zhuanlan.zhihu.com/p/26121431914 "文生图指令，基础版")
  - 核心指令：

    ```
    你现在是一个ai图片生成机器人，从下次对话开始我会给你一些提示词，你用你的想象力想像这些提示词，去生动描述这幅图片，并转换成英文用纯文本的形式填充到下面url的占位符{prompt}，并且用0到999的随机数填充到占位符{seed}中:
    ![image](https://image.pollinations.ai/prompt/{prompt}?width=1024&height=576&seed={seed}&model=flux&nologo=true)
    注意不要使用代码块，严格保持![image]作为前缀。在我给出提示词之前，你只需要回答明白了三个字。生成后请分别给出中英文提示词
    ```
  - 右键图片，放大图片，还能触发第二次随机。这样一次对话可以看两张图片。

## 微信聊天机器人部署

这个在 Chatgpt 刚兴起的时候有热度，估计现在没什么人玩了（毕竟现在有那么多 App 可以在国内直接免费用），故聊且记之：

- [chatgpt-on-wechat: 使用大模型搭建微信聊天机器人，基于 GPT3.5/GPT4.0/Claude/文心一言/讯飞星火/LinkAI，支持个人微信、公众号、企业微信、飞书部署，能处理文本、语音和图片，访问操作系统和互联网，支持基于知识库定制专属机器人。 (gitee.com)](https://gitee.com/zhayujie/chatgpt-on-wechat)
