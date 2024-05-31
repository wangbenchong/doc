# 破解步骤

找到Tyora安装目录，依次找到这个文件

1.

resources\page-dist\static\js\LicenseIndex...chunk.js

用记事本打开它，

查找【e.hasActivated="true"==e.hasActivated,】

替换为【e.hasActivated="true"=="true",】

2.

resources\page-dist\license.html

用记事本打开它，

查找【</body></html>】

替换为【</body><script>window.onload=function(){setTimeout(()=>{window.close();},5);}</script></html>】

高版本可以考虑timeout设成10

3.

resources\locales\zh-Hans.lproj\Panel.json

查找【“UNREGISTERED”:"未激活",】

替换为【“UNREGISTERED”:" ",】

4.

以上操作完已经可以正常使用，仅有“许可证信息”/“我的许可证”页面无法打开，左下角存在“x”（可手动点击关闭但重新打开软件会重新出现）

如果弹窗提示错误，点击“->Learn Data Recovery”再关闭浏览器就行了

# 进阶设置

添加插件

https://github.com/obgnail/typora_plugin

下载脚本后：

1. 找到 Typora 安装路径，包含 `window.html` 的文件夹 A。（不同版本的 Typora 的文件夹结构可能不同，在我这是 `Typora/resources/app`，推荐使用 everything 找一下）
2. 打开文件夹 A，将源码的 plugin 文件夹粘贴进该文件夹下。
3. 打开文件 `A/window.html`。搜索文件内容 `<script src="./app/window/frame.js" defer="defer"></script>`，并在后面加入 `<script src="./plugin/index.js" defer="defer"></script>`。保存。（不同版本的 Typora 查找的内容可能不同，其实就是查找导入 frame.js 的 script 标签）
4. 重启 Typora。