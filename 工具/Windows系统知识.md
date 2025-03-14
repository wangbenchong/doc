# 系统激活破解

*以下均已转存度盘我的资源*

## GitHub知己知彼

软件名：HEU KMS Activator

许可证快过期的时候试了下，默认智能激活执行后win11可以永久破解。该软件破解Windows、Office都很好用。需要注意Edge浏览器下载rar后会被Windows defender查杀，在浏览器下载里选择仍然保留即可。

[Releases · zbezj/HEU_KMS_Activator](https://github.com/zbezj/HEU_KMS_Activator/releases)

## 亦是美

软件名：DragonKMS

用这个软件可以把家庭版破解成专业版

[KMS在线激活Win11 / 10 / 8 / 7和Office / Visio / Project 2021~2010之DragonKMS神龙版-网络教程与技术 -亦是美网络 (yishimei.cn)](http://www.yishimei.cn/network/319.html?=23:31:39)

# 通过组策略永久禁用Windows 11上的自动更新

可以通过调整组策略在Windows 11上阻止自动下载更新。但是，只能在Windows 11 Pro 上使用这些步骤，家庭版没有此工具。

要从Windows 11上的组策略永久禁用自动更新，请按照以下步骤操作：

按 Windows + R 键盘快捷键，输入“gpedit.msc”并点击确定打开“本地组策略编辑器”。打开以下路径：

计算机配置 > 管理模板 > Windows组件 > Windows更新 > 管理最终用户体验
双击打开“配置自动更新”策略。


选择“已禁用”选项以永久禁用Windows更新。


点击应用按钮，点击确定按钮。

完成这些步骤后，Windows更新将不再自动安装系统更新。但是，仍然可以通过设置手动检查更新。

# 通过注册表在Windows 11上禁用自动更新

如果使用的是Windows 11 Home版本，可以使用注册表禁用更新。要在注册表中禁用Windows 11更新，请按照以下步骤操作：

按 Windows + R 键盘快捷键，输入“regedit”打开注册表编辑器。导航到以下路径：

计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows
右键单击“Windows”，选择新建，然后选择“项”。将新键项命名为“WindowsUpdate”，然后按Enter键。

右键单击“WindowsUpdate”，选择新建，然后选择“项”，将新建的项命名为“AU”，然后按Enter键。

右键单击“AU”，选择新建，然后选择DWORD（32位）值选项，命名为“NoAutoUpdate”，然后按Enter键。

双击新创建“NoAutoUpdate”，将其值从0更改为1，点击“确定”按钮。


重新启动计算机。

完成这些步骤后，Windows更新不再自动下载。但是，您仍然可以通过设置手动检查更新。

 作者：傻大个科技 https://www.bilibili.com/read/cv28426364/ 出处：bilibili



# 家庭版开启组策略方法

　首先我们打开记事本，并输入以下内容（注意空格）：

```text
@echo off

pushd "%~dp0"

dir /b C:\Windows\servicing\Packages\Microsoft-Windows-GroupPolicy-ClientExtensions-Package~3*.mum >List.txt

dir /b C:\Windows\servicing\Packages\Microsoft-Windows-GroupPolicy-ClientTools-Package~3*.mum >>List.txt

for /f %%i in ('findstr /i . List.txt 2^>nul') do dism /online /norestart /add-package:"C:\Windows\servicing\Packages\%%i"

pause
```

接下来选择文件另存为，文件类型选择所有文件，名称随意，扩展名为“.cmd”把它保存下来。

接下来右键以管理员身份运行这个文件，再打开搜索输入“gpedit.msc”查看一下，你熟悉的组策略是不是又有了。



