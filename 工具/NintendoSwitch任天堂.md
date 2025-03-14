# 游戏资源/系统整合包下载

[3K-switch游戏下载，个人首选，单次付费终身使用且稳定](https://www.3kns.com/portal.php)

其他：

[switch520–专业的switch游戏下载网站](https://www.520switch.com/)

[Switch618|免费Switch游戏PC游戏下载 - SWITCH618游戏公益分享](https://www.switch618.com/)

[国外nswgame网站免费免登录，FreeDlink下载较慢](https://nswgame.com/)

# 软破机系统升降级

安装包分两部分：大气层整合包+switch系统包，通常两者是版本对应的，不过大气层整合包的高版本也兼容旧的switch系统版本。

由于误联网导致软破机系统升级的，重新执行安装流程（仅留Nintendo文件夹和整合包还有金手指），用daybreak可以降级。如果遇到unknown pkg1 version报错，那就用适当的整合包重新覆盖（删除sd卡上原有）上去（注意：即便是与switch系统包对应的大气层整合包，在降级时也可能报错，此时沿用高版本大气层只降级switch系统版本即可）。

# 实机更新需提前备份

需要备份金手指和特殊应用（如MissionControl支持ps4手柄适配，GBA模拟器等）

## MissionControl

[ndeadly/MissionControl: Use controllers from other consoles natively on your Nintendo Switch via Bluetooth. No dongles or other external hardware neccessary.](https://github.com/ndeadly/MissionControl)

使用方式：

PS手柄同时按住PS +share按钮直至灯光闪烁；Xbox Ones按住西瓜按钮，直到LED开始闪烁。然后按住背面靠近充电口的小配对按钮，直到LED开始快速闪烁。
[MissionControl插件switch连接其他游戏机手柄 使用教程_哔哩哔哩_bilibili](https://www.bilibili.com/video/av380521186/?vd_source=563d44869c3ecebb1867233573d16b7b)

目录结构(建议每次删掉重装新版)：
atmosphere/contents/010000000000bd00
atmosphere/exefs_patches/bluetooth_patches
atmosphere/contents/btm_patches

## GBA模拟器

分两个部分nsp和Sloop+文件夹，nsp通过dbi安装不必存在于switch目录，文件夹放sd卡最外层即可

目录结构：
Sloop+

## 王国之泪金手指

使用方式：
L 加方向键↓呼出特斯拉

目录结构：
atmosphere/contents/0100F2C0115B6000

# 实机安装游戏

通过相册里的DBI来安装，具体是MTP那个选项，会提示跟电脑usb连接，连好之后直接通过电脑安装；如果安装过程中断导致sd卡内存没释放，可通过 DBI里的工具-->清除孤立文件 来释放空间。

# Prod.Keys/Yuzu模拟器（PC/Android）/ Firmware下载

[Prod.Keys](https://emuyuzu.com/prod-keys/)

[Yuzu Emulator APK For Android, Windows & Linux PCs Download](https://emuyuzu.com/emulator-download/)

[Yuzu Firmware Download Latest Version 18.1.0（需要梯子）](https://emuyuzu.com/yuzu-firmware/)

# Android模拟器Suyu

[Suyu Emulator — A familiar Nintendo Switch emulator](https://suyu.dev/)

# NSCB软件魔改工具

最后版本：v1.01b

汉化改良版，支持魔改的版本更多：
[zdm65477730/NSC_BUILDER at v1.01b-1](https://github.com/zdm65477730/NSC_BUILDER/tree/v1.01b-1)

原版（已多年未更新）：[GitHub - julesontheroad/NSC_BUILDER: Nintendo Switch Cleaner and Builder. A batchfile, python and html script based in hacbuild and Nut's python libraries. Designed initially to erase titlerights encryption from nsp files and make multicontent nsp/xci files, nowadays is a multicontent tool specialized in batch processing and file information, someone called it a Switch's knife and he may be right.](https://github.com/julesontheroad/NSC_BUILDER)

网盘备份分享的文件：NSCBx1.0.1b.rar， 已整合18.1.0key
链接: https://pan.baidu.com/s/1xUHz7qA5iT9ianObe3pkrw?pwd=bbwx 提取码: bbwx 
--来自百度网盘超级会员v7的分享

需要随switch系统更新不断维护的文件：ztools/keys.txt  （实际就是用Prod.keys改的文件名）

启动后是否自动更新：
如果不需要（通常不需要），zconfig/zconfig_no_update.7z解压到当前文件夹即可；
如果需要更新，zconfig/zconfig_update.7z解压到当前文件夹即可。

