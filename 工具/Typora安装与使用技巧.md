# 官方正版下载

官方更新日志：

[Typora — stable release channel (typoraio.cn)](https://typoraio.cn/releases/stable.html)

# 破解方法一：工具

- 工具压缩包：破解 Typora.rar

- 下载：见[文末百度网盘](#以上资源地址备份)

- 步骤：见包内readme文档

- 不足：激活过程需要联网，内网可能需要手动破解

# 破解方法二：手动

找到 Typora 安装目录，依次找到这个文件

- resources\page-dist\static\js\LicenseIndex...chunk.js

  用记事本打开它，查找

  ```javascript
  e.hasActivated="true"==e.hasActivated,
  ```
  替换为
  ```javascript
  e.hasActivated="true"=="true",
  ```

- resources\page-dist\license.html

  用记事本打开它，查找
  ```html
  </body></html>
  ```

  替换为

  ```html
  </body><script>window.onload=function(){setTimeout(()=>  {window.close();},15);}</script></html>
  ```

- resources\locales\zh-Hans.lproj\Panel.json
  
  查找
  
  ```javascript
  "UNREGISTERED":"未激活",
  ```
  
  替换为
  
  ```javascript
  "UNREGISTERED":" ",
  ```

以上操作完和激活效果基本相同，仅存以下不足：
- 不能打开多个窗口但可通过插件支持单窗口多标签（后文会提到）
- “许可证信息”/“我的许可证”页面无法打开，左下角存在“x”（可手动点击关闭但重新打开软件会重新出现）
- 极小概率（上文已通过加长timeout为15基本避免）弹窗提示错误，点击“-> Learn Data Recovery”再关闭浏览器就行了。


# 进阶设置

添加插件

https://github.com/obgnail/typora_plugin

下载脚本后：

1. 找到 Typora 安装路径，包含 `window.html` 的文件夹 resources。（不同版本的 Typora 的文件夹结构可能不同，在我这是 `Typora/resources`，推荐使用 everything 找一下）

2. 打开文件夹resources，将源码的 plugin 文件夹粘贴进该文件夹下（如果是更新，先把里面的resources\plugin\global\settings\settings.user.toml备份一下）。

3. ↓↓ *新版插件可能不再需要手动做这个步骤，执行plugin/bin/install_windows_amd_x64.exe即可* ↓↓
   ~~打开文件 `resources/window.html`。搜索文件内容~~

   ```html
   <script src="./appsrc/window/frame.js" defer="defer"></script>
   ```

   ~~并在后面加入~~

   ```html
   <script src="./plugin/index.js" defer="defer"></script>
   ```

   ~~并保存。（不同版本的 Typora 查找的内容可能不同，其实就是查找导入 frame.js 的 script 标签）~~

4. 打开resources\plugin\global\settings\settings.default.toml，找到*[slash_commands]*，其中COMMANDS列表**开头**插入以下指令：（建议不要直接改settings.default.toml，而是把slash_commands整段代码抄到settings.user.toml再做修改，重新运行Typora这个user文件会自动代码格式化并使新配置生效，更新的时候跳过这个user文件即可保持私人定制性的延续）

   

   直接按照 **settings.default.toml **的写法来改 **settings.user.toml**（把slash_commands整段代码抄过来）：

   ```js
   //...以上部分省略...
   COMMANDS = [
   	{ enable = true, type = "snippet", icon = "👕", hint = "插入图片（居左）", keyword = "img1", cursorOffset = [-5, -5],  callback = " ![](./img/.jpg)" },
   	{ enable = true, type = "snippet", icon = "👕", hint = "插入图片（居中）", keyword = "img2" cursorOffset = [-5, -5], callback = "![](./img/.jpg)" },
   //建议把其他不需要的设成enable = false 以精简下拉列表，并且方便以后开关
   //...以下部分省略...
   ```

   自动格式化后 **settings.user.toml** 变成这样（把slash_commands这段代码完全做了格式化）：

   ```js
   [[slash_commands.COMMANDS]]
   enable = true
   type = "snippet"
   icon = "👕"
   hint = "插入图片（居左）"
   keyword = "img1"
   cursorOffset = [ -5, -5 ]
   callback = " ![](./img/.jpg)"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "snippet"
   icon = "👕"
   hint = "插入图片（居中）"
   keyword = "img2"
   cursorOffset = [ -5, -5 ]
   callback = "![](./img/.jpg)"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "🌟"
   hint = "箭头（默认上，_1下）"
   keyword = "arrow"
   callback = """
   (params) => {
       // 去掉 textBefore 中最后一个斜杠及其之后的所有字符
       const lastSlashIndex = this.inputs.textBefore.lastIndexOf('/')
       const textBefore = lastSlashIndex !== -1
           ? this.inputs.textBefore.slice(0, lastSlashIndex)
           : this.inputs.textBefore;
       // 获取斜杠前后的内容，拼接成整行内容
       const fullText = textBefore + this.inputs.textAfter
       // 构造替换后的内容，支持多次替换
       let cnt = fullText
       if (!params || params.length === 0) {
           cnt = '↑↑ *@* ↑↑'.replace(/@/g, fullText)
       }
       else
       {
           const d = params[0]
           if(d === "1"){
               cnt = '↓↓ *@* ↓↓'.replace(/@/g, fullText)
           }
       }
       // 获取当前行的范围
       const { range, bookmark } = this.utils.getRangy()
       // 调整范围
       bookmark.start = 0
       bookmark.end += this.inputs.textAfter.length
       // 删除原有内容
       range.moveToBookmark(bookmark)
       range.deleteContents()
       // 插入新内容
       this.utils.insertText(null, cnt, false)
       // 光标漂移
       setTimeout(() => {
           const { range, bookmark } = this.utils.getRangy()
           const target = [...bookmark.containerNode.childNodes].findLast(e => e.classList.contains("md-pair-s"))
           target.classList.add("md-expand")
           const mark = target.querySelector(".md-meta.md-after").previousElementSibling
           range.setStartAfter(mark)
           range.setEndAfter(mark)
           range.select()
       }, 150)
   }
   """
   
   
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "gen-snp"
   icon = "🌟"
   hint = "示例：插入"
   keyword = "insert"
   callback = "(text) => 'abc@def@gh'.replace(/@/g, this.inputs.textAfter)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "gen-snp"
   icon = "🌟"
   keyword = "BlockCodeGenerator"
   callback = "(...langs) => langs.map(l => '```' + l.toLowerCase() + '\\n```').join('\\n\\n')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "gen-snp"
   icon = "🌟"
   keyword = "CalloutGenerator"
   callback = "(...types) => types.map(t => `> [!${t.toUpperCase()}]\\n>\\n> `).join('\\n\\n')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "🌟"
   keyword = "TableGenerator"
   callback = "(col, row) => { col = parseInt(col); row = parseInt(row); const c = ['      ', ' ---- ', ...Array(row - 1).fill('      ')].map(e => `|${Array(col).fill(e).join('|')}|`).join('\\n'); this.utils.insertText(null, c, false) }"
   
   //...以下部分省略...
   ```
   
   
   
   这样可以用斜杠 / 快速地插入自定义的字符串（上面的例子是方便插入居左、居中的图片，有其他想法可以参照这个来配置，双引号等可采用C语言转义字符，具体可搜索toml语法，插件的md文档中也有提及）。
   
5. 重启 Typora。

6. 侧边栏“文件”分页内，右键空白勾选 “文档树”；“大纲”分页内，右键空白勾选 “大纲视图（可折叠）”；偏好设置里Markdown->代码块->勾选“显示行号”。

# 以上资源地址备份

链接：https://pan.baidu.com/s/1EcQsKHYF6Y-gvToy1dzWFQ?pwd = jmkr 
提取码：jmkr 