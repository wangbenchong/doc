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
   enable = true
   type = "command"
   icon = "🌟"
   hint = "生成链接（光标后面）"
   keyword = "afterLink"
   callback = """
   () => {
       const textAfter = this.inputs.textAfter.split(String.fromCharCode(92)).join('/');
       const lastSlashIndex = textAfter.lastIndexOf('/')
       // 使用三目运算符处理不同情况
       const fileName = lastSlashIndex === -1
       ? textAfter // 如果没有斜杠，返回整个字符串
       : lastSlashIndex === textAfter.length - 1
       ? '' // 如果斜杠是最后一个字符，返回空字符串
       : textAfter.slice(lastSlashIndex + 1); // 否则截取最后一个斜杠之后的部分
       const cnt = `[${fileName}](${textAfter})`
       const { range, bookmark } = this.utils.getRangy()
       bookmark.start = 0
       bookmark.end += this.inputs.textAfter.length
       range.moveToBookmark(bookmark)
       range.deleteContents()
       this.utils.insertText(null, cnt, false)
   }
   """
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "引用"
   keyword = "Blockquote"
   callback = "() => File.editor.stylize.toggleIndent('blockquote')"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "水平分割线"
   keyword = "Hr"
   callback = "() => File.editor.stylize.insertBlock('hr')"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "内容目录"
   keyword = "Toc"
   callback = "() => File.editor.stylize.insertBlock('toc')"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "snippet"
   icon = "👕"
   hint = "高亮"
   keyword = "hightlight"
   cursorOffset = [ -4, -2 ]
   callback = "==高亮=="
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "删除线"
   keyword = "Delete"
   callback = "() =>File.editor.stylize.toggleStyle('del')"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "下划线"
   keyword = "Underline"
   callback = "() => File.editor.stylize.toggleStyle('underline')"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "内联公式"
   keyword = "InlineMath"
   callback = "() => File.editor.stylize.toggleStyle('inline_math')"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "注释"
   keyword = "Comment"
   callback = "() => File.editor.stylize.toggleStyle('comment')"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "👕"
   hint = "清除样式"
   keyword = "ClearStyle"
   callback = "() => File.editor.stylize.clearStyle()"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "command"
   icon = "🧰"
   hint = "帮助"
   keyword = "Help"
   callback = "() => this.call()"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "gen-snp"
   icon = "🧩"
   hint = "日期时间"
   keyword = "Datetime"
   callback = "() => new Date().toLocaleString('chinese', {hour12: false})"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "gen-snp"
   icon = "🧩"
   hint = "日期"
   keyword = "Date"
   callback = "() => {let day = new Date(); return `${day.getFullYear()}/${day.getMonth() + 1}/${day.getDate()}`}"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "gen-snp"
   icon = "🧩"
   hint = "时间"
   keyword = "Time"
   callback = "() => {let day = new Date(); return `${day.getHours()}:${day.getMinutes()}:${day.getSeconds()}`}"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "gen-snp"
   icon = "🧩"
   hint = "时间戳"
   keyword = "Timestamp"
   callback = "() => new Date().getTime().toString()"
   
   [[slash_commands.COMMANDS]]
   enable = true
   type = "gen-snp"
   icon = "🧩"
   hint = "星期"
   keyword = "Week"
   callback = "() => '星期' + '日一二三四五六'.charAt((new Date()).getDay())"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "gen-snp"
   icon = "🌟"
   hint = "示例：插入"
   keyword = "insert"
   callback = "(text) => 'abc@def@gh'.replace(/@/g, this.inputs.textAfter)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "🌟"
   keyword = "TableGenerator"
   callback = "(col, row) => { col = parseInt(col); row = parseInt(row); const c = ['      ', ' ---- ', ...Array(row - 1).fill('      ')].map(e => `|${Array(col).fill(e).join('|')}|`).join('\\n'); this.utils.insertText(null, c, false) }"
   
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
   icon = "📝"
   hint = "一级标题"
   keyword = "H1"
   callback = "() => File.editor.stylize.changeBlock('header1', undefined, true)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "二级标题"
   keyword = "H2"
   callback = "() => File.editor.stylize.changeBlock('header2', undefined, true)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "三级标题"
   keyword = "H3"
   callback = "() => File.editor.stylize.changeBlock('header3', undefined, true)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "四级标题"
   keyword = "H4"
   callback = "() => File.editor.stylize.changeBlock('header4', undefined, true)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "五级标题"
   keyword = "H5"
   callback = "() => File.editor.stylize.changeBlock('header5', undefined, true)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "六级标题"
   keyword = "H6"
   callback = "() => File.editor.stylize.changeBlock('header6', undefined, true)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "段落"
   keyword = "Paragraph"
   callback = "() => File.editor.stylize.changeBlock('paragraph')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "提升标题等级"
   keyword = "IncreaseHeaderLevel"
   callback = "() => File.editor.stylize.increaseHeaderLevel()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "降低标题等级"
   keyword = "DecreaseHeaderLevel"
   callback = "() => File.editor.stylize.decreaseHeaderLevel()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "表格"
   keyword = "Table"
   callback = "() => File.editor.tableEdit.insertTable()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "公式块"
   keyword = "BlockMath"
   callback = "() => File.editor.stylize.toggleMathBlock()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "代码块"
   keyword = "BlockCode"
   callback = "() => File.editor.stylize.toggleFences()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "有序列表"
   keyword = "OrderedList"
   callback = "() => File.editor.stylize.toggleIndent('ol')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "无序列表"
   keyword = "UnorderedList"
   callback = "() => File.editor.stylize.toggleIndent('ul')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "任务列表"
   keyword = "Tasklist"
   callback = "() => File.editor.stylize.toggleIndent('tasklist')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "增加列表缩进"
   keyword = "ListMoreIndent"
   callback = "() => File.editor.UserOp.moreIndent(File.editor)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "减少列表缩进"
   keyword = "ListLessIndent"
   callback = "() => File.editor.UserOp.lessIndent(File.editor)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "在上方插入段落"
   keyword = "InsertParagraphAbove"
   callback = "() => File.editor.UserOp.insertParagraph(true)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "在下方插入段落"
   keyword = "InsertParagraphBelow"
   callback = "() => File.editor.UserOp.insertParagraph(false)"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "链接引用"
   keyword = "DefLink"
   callback = "() => File.editor.stylize.insertBlock('def_link')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "脚注"
   keyword = "DefFootnote"
   callback = "() => File.editor.stylize.insertBlock('def_footnote')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "📝"
   hint = "元信息"
   keyword = "FrontMatter"
   callback = "() => File.editor.stylize.insertMetaBlock()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "👕"
   hint = "粗体"
   keyword = "Strong"
   callback = "() => File.editor.stylize.toggleStyle('strong')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "👕"
   hint = "斜体"
   keyword = "Em"
   callback = "() => File.editor.stylize.toggleStyle('em')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "👕"
   hint = "代码"
   keyword = "Code"
   callback = "() => File.editor.stylize.toggleStyle('code')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "👕"
   hint = "超链接"
   keyword = "Link"
   callback = "() => File.editor.stylize.toggleStyle('link')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "👕"
   hint = "图像"
   keyword = "Image"
   callback = "() => File.editor.stylize.toggleStyle('image')"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "🧰"
   hint = "至顶部"
   keyword = "JumpTop"
   callback = "() => File.editor.selection.jumpTop()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "🧰"
   hint = "至底部"
   keyword = "JumpBottom"
   callback = "() => File.editor.selection.jumpBottom()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "🧰"
   hint = "至行首"
   keyword = "JumpToLineStart"
   callback = "() => File.editor.selection.jumpToLineStart()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "command"
   icon = "🧰"
   hint = "至行尾"
   keyword = "JumpToLineEnd"
   callback = "() => File.editor.selection.jumpToLineEnd()"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "snippet"
   icon = "🧩"
   hint = "示例片段"
   keyword = "SnippetExample"
   callback = "https://github.com/obgnail/typora_plugin"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "snippet"
   icon = "🧩"
   hint = "光标偏移示例"
   keyword = "cursorOffsetExample"
   cursorOffset = [ -31, -18 ]
   callback = "感谢您使用 Typora Plugin，如果本项目帮助到您，欢迎 STAR"
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "snippet"
   scope = "inline_math"
   icon = "🧩"
   keyword = "alpha"
   callback = "\\alpha "
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "snippet"
   scope = "inline_math"
   icon = "🧩"
   keyword = "beta"
   callback = "\\beta "
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "snippet"
   scope = "inline_math"
   icon = "🧩"
   keyword = "epsilon"
   callback = "\\epsilon "
   
   [[slash_commands.COMMANDS]]
   enable = false
   type = "snippet"
   scope = "inline_math"
   icon = "🧩"
   keyword = "rightarrow"
   callback = "\\rightarrow "
   
   [auto_number]
   ENABLE_IMAGE = false
   ENABLE_FENCE = false
   
   [pie_menu]
   ENABLE = true
   
   ```
   
   > 自定义代码每次更改要  `重启Typora` 并  `shift+F12开启开发者模式` 来做测试
   >
   > 
   
   这样可以用斜杠 / 快速地插入自定义的字符串（上面的例子是方便插入居左、居中的图片，有其他想法可以参照这个来配置，双引号等可采用C语言转义字符，具体可搜索toml语法，插件的md文档中也有提及）。
   
5. 重启 Typora。

6. 侧边栏“文件”分页内，右键空白勾选 “文档树”；“大纲”分页内，右键空白勾选 “大纲视图（可折叠）”；偏好设置里Markdown->代码块->勾选“显示行号”。

# 以上资源地址备份

链接：https://pan.baidu.com/s/1EcQsKHYF6Y-gvToy1dzWFQ?pwd = jmkr 
提取码：jmkr 