# Unity 自动生成指定规则的Mono脚本

- 视频：[Unity AI脚本生成器_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1ftZJYBEqH)
- 源自Github插件（非package）：https://github.com/OSCAR-hi/Unity-Scripts-Generator
- 个人做了一些修改，保持逻辑的稳定、简洁。
- 目前只是基础版，后续会做改进，让它能在Unity里做更多事。

## 使用方法

- 将本文所在文件夹整体拷贝到Unity项目中（目录位置没有限制，可自行指定），Project Setting 会有 DeepSeek Script Generator 选项，其中设置这样改：
  - URL不用改，API Key 填自己的 key
  - Model 改成 **deepseek-chat** （对应DeepSeek-V3）或者 **deepseek-reasoner** （对应DeepSeek-R1）
  - Scripts output path：输出脚本文件夹路径可以改一下
- 使用规则：
  - 选中任何物体，在Inspector面板最下面会多出一个 `Generate Component` 按钮
  - 点击按钮，在弹框中输入一段指令，如 “点击该物体会随机改变该物体的缩放为0.5倍到3倍之间”。
  - 如果勾选 “Forget prior commands” 就是忘记过往的AI对话。
  - 点击`Generate and Add` 按钮，稍作等待就会生成mono脚本并挂载到当前物体上。验证脚本逻辑需运行游戏。

## 目前包含以下脚本

- [DeepSeekAPI.cs](./Editor/DeepSeekAPI.cs)
- [Encryption.cs](./Editor/Encryption.cs)
- [ScriptGeneratorButton.cs](./Editor/ScriptGeneratorButton.cs)
- [ScriptGeneratorSettings.cs](./Editor/ScriptGeneratorSettings.cs)
- [ScriptGeneratorWindow.cs](./Editor/ScriptGeneratorWindow.cs)
