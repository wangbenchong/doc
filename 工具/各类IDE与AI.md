

# IDE

## Rider

*与游戏引擎Unity、UE集成度最高*

### 资源链接：

链接：https://pan.baidu.com/s/1qa1MkupwHCbIEEK1sHUUPg?pwd=hpdt 
提取码：hpdt 
--来自百度网盘超级会员V7的分享

其中有个  `Rider破解教程.rar`  也包含源头资源链接，是从某宝上整理的，如过期记得联系卖家。

### 使用心得：

最常用快捷键ctrl+q搜索，alt+回车呼出当前位置上下文操作，F12跳转到引用，其他忘了可以到教学里复习

刚装完需要汉化，到**设置-插件**那里，搜chinese，大约28M的那个插件就是。

rider自带的AI（从2024版开始有）需要正版付费用户登录，而破解版我推荐使用第三方AI，百度的comate插件就不错（2021版以后都支持，就是安装下载慢）。

【设置】编辑器-常规-编辑器标签页-勾选“标记已修改”

【设置】编辑器-配色方案-常规-错误和警告-未使用的代码那里，修改颜色为暗青色（推荐578389）并勾选斜体（若改回原色就填787878），用来区分AI自动补全的代码和非活跃变体代码颜色。

【设置】编辑器-嵌入提示-C++-参数名称提示-函数形参的可见性：Push-to-Hint（长按ctrl键）

【设置】编辑器-嵌入提示-C++-参数名称提示-宏形参的可见性：Push-to-Hint（长按ctrl键）

【设置】编辑器-嵌入提示-C++-其他-预处理程序指令提示-可见性：Push-to-Hint（长按ctrl键）

【设置】编辑器-常规-外观-勾选“滚动时显示粘性行”

【设置】编辑器-检查设置-检查严重性-C++-常见做法和代码改进-取消勾选“局部变量可被设为const”和“形参可被设为const”

【检查严重性】其他语言-校对-取消勾选“拼写错误”

【菜单栏】视图-活动编辑器-自动换行（酌情处理）

【设置】编辑器-CodeVision-非活跃的着色器变体分支（酌情处理）

鼠标晃到右上角代码分析报错那里，鼠标悬停出气泡后可以设置代码分析的开关，如误操作关闭可重复操作开启

在【设置】里搜"命名不一致"，c++里的约束违规取消勾选，其他酌情处理。

关闭Win11系统下输入法的快捷键：时间和语言>语言和区域>按键---热键  简体/繁体中文输入切换（ctrl+shift+F）

【设置】语言和框架-Unity引擎-取消勾选“在Unity中自动刷新”

### 插件安装路径：

C:\Users\用户名\AppData\Roaming\JetBrains\Rider2024.1\plugins

### AI插件

- Comate：百度旗下

- 通义灵码：阿里旗下

- DeepSeek：[本地部署deepseekR1，rider+qwen coder提高生产力 - 知乎](https://zhuanlan.zhihu.com/p/22052237965)
  可以通过安装Continue插件，输入DeepSeek的API key之后即可使用，配置文件会生成在

  ```c#
  C:\Users\用户名\.continue
  ```



## VSCode

*免费、最佳的AI工具试验田*

常用AI插件：Comate、通义灵码

集成DeepSeek方面，目前有Continue、Cline等插件：

- [使用 DeepSeek API 配合 Continue 插件代替 Copilot - ccruiの博客](https://blog.ccrui.cn/archives/shi-yong-deepseek-api-pei-he-continue-cha-jian-dai-ti-copilot)
- [VScode+Cline+Deepseek实现媲美cursor的代码自动生成_星星猫的技术博客_51CTO博客](https://blog.51cto.com/u_14249042/13045983)

代码格式化方面，推荐安装**Prettier**插件，可以把没有换行的js脚本格式化成带换行缩进的方式（右键菜单：使用...格式化文档）。

## VS

常用插件：VA

## Script Inspector 3

*Unity插件，内置在Unity中的IDE*

https://www.bilibili.com/video/BV1Pe4y1p77D

## NotePad++

纯文本编辑器，但支持自定义语法

## Cursor

*OpenAI旗下*

AI：chatgpt



# 常见AI大模型

## DeepSeek

官网：[DeepSeek | 深度求索](https://www.deepseek.com/)

*可用手机号注册*

创建自己的APIKeys：[DeepSeek 开放平台](https://platform.deepseek.com/api_keys)[ ](sk-93148607c227415e8d6f9c0f13edfb18)[ ](https://platform.deepseek.com/api_keys)

目前的问题：自2025.2.7至今2025.2.13，因服务器压力已关闭充值渠道，且无免费额度，导致新用户无法使用，会在IDE里报402错误。

### 本地部署DeepSeek

下载LMStudio：[LM Studio - Discover, download, and run local LLMs](https://lmstudio.ai/)



## 文心一言

*百度旗下*

[文心一言](https://yiyan.baidu.com/)

[文心快码(Baidu Comate)·更懂你的智能代码助手](https://comate.baidu.com/zh)



## 豆包

*抖音旗下*

[豆包 MarsCode - 编程助手](https://www.marscode.cn/home)



# 其他AI相关

## 微信聊天机器人部署

[chatgpt-on-wechat: 使用大模型搭建微信聊天机器人，基于 GPT3.5/GPT4.0/Claude/文心一言/讯飞星火/LinkAI，支持个人微信、公众号、企业微信、飞书部署，能处理文本、语音和图片，访问操作系统和互联网，支持基于知识库定制专属机器人。 (gitee.com)](https://gitee.com/zhayujie/chatgpt-on-wechat)



## 游戏引擎使用AI

Unity可使用

- Unity Muse Chat（包名com.unity.muse.chat）
- Sentis (包名com.unity.sentis)
- Muse Sprite Tool
- Muse Texture Tool

