# 官网

[免费的作谱软件 | MuseScore](https://musescore.org/zh-hans)

[免费的midi歌曲资源下载](https://www.aigei.com/music/midi/?term=is_vip_false)

# 基础玩法

安装主体：[MuseScore Studio ](https://muse-cdn.com/Muse_Hub.exe)

如果电脑只有C盘，就一路Next安装；

如果电脑有D盘，建议不要安在默认的C盘，安装目录建议为：D:\\Program Files\\MuseScore 4

# 进阶玩法

如果不满足于本体自带的音色和效果器，并且已经安装了本体MuseScore，那么就需要安装Muse Hub了。

## 安装MuseHub

选装：[Muse Hub](https://muse-cdn.com/Muse_Hub.exe)

安装前注意先关闭MuseScore软件。

Muse Hub的安装过程比较顺利，是强制安装到C盘的。安装路径也是固定的：

"C:\Program Files\WindowsApps\Muse.MuseHub_1.0.2.800_x64__rb9pth70m6nz6\Muse.exe"

这个Muse Hub占用C盘只有50MB还算无所谓，但Muse Hub的音色下载文件是非常大的，全下载会有将近20个G。所以如果电脑有D盘，并且不希望Muse Hub的缓存文件都放在C盘而是希望放到D盘的话，请做以下的步骤（强烈推荐）：

1. 装好Muse Hub后，不要运行它，如果已经打开就立刻关掉Muse Hub（任务栏小图标右键退出，如果10秒内再次弹出就再右键退出一次）。

2. 到C盘这个路径：C:\ProgramData\MuseHub\Settings.json 用记事本打开，把原本里面MuseDownloadLocation、MuseDownloadLocation这两个对应的两个文件夹到资源管理器里删除掉，再编辑成如下：

   ```c
   {
     "EffectInstallLocation": "D:\\Program Files\\MuseScore 4\\SoundDocument\\VST3",
     "ContentInstallLocation": "C:\\ProgramData\\MuseHub",
     "ApplicationInstallLocation": "D:\\Program Files\\MuseScore 4",
     "MuseDownloadLocation": "D:\\Program Files\\MuseScore 4\\SoundDocument\\Muse Hub Downloads",
     "EnableCommunityAcceleration": true,
     "AutomaticallyKeepApplicationsUpToDate": false,
     "AutomaticallyUpdateOnMeteredConnections": false,
     "AutomaticallyKeepSoundsUpToDate": true
   }
   ```

3. 打开MuseScore软件，编辑->偏好设置->文件夹，依次复制粘贴文件夹路径如下：

   - 乐谱：D:/Program Files/MuseScore 4/SoundDocument/Document

   - 样式：D:/Program Files/MuseScore 4/SoundDocument/Styles

   - 模板：D:/Program Files/MuseScore 4/SoundDocument/Templates

   - 插件：D:/Program Files/MuseScore 4/SoundDocument/plugin

   - 声音字体：D:/Program Files/MuseScore 4/SoundDocument/Muse Hub Downloads/Instruments

   - VST3：D:/Program Files/MuseScore 4/SoundDocument/VST3

     然后点“好”按钮。

4. 到“我的文档”里删除MuseScore、MuseHub相关文件夹（如果有的话）

5. 关掉MuseScore软件

## 安装完MuseHub之后

1. 打开MuseHub软件
2. 在MuseHub界面右上角有个小齿轮，点击它。
3. StarUp选项的滑动开关关闭掉，防止MuseHub开机自启动。
4. 小齿轮左边有几个切页，选中其中的【主页】
5. ”特色”那里，有【Muse Sound】和【GUITAS VOL.1】来回滚动，把这两个下面的【GET】按钮都点一下。就开始下载音色了（下载进度在界面靠下位置）。
6. 音色下载完，选择【效果】切页，把头几个效果（主要是混响，其他随意）下面的【GET】按钮点一下。就开始下载效果了。
7. 都下载好之后，就可以打开MuseScore了（也可以通过MuseHub的【主页】点击第一个应用程序来打开MuseScore）。
8. MuseScore里面的混响器此时可以选择你刚刚下载的音色和效果了。