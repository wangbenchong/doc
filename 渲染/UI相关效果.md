# Unity6 ShaderGraph 实现UI效果

[【油管】Shader Graph: UGUI Shaders Sample](https://www.youtube.com/watch?v=LuS-TDTI8mU)

使用方法：升级到 Unity 6000.0.40f1 (或更高版本) 然后在 window > package manager  > shader graph（版本 17.0.4及以上）中载入Sample："UGUI Shaders"。其中包含了超过50种UI常用效果，并且不会增加图片内存开销。

其中毛玻璃效果需要开启 `Render Pipeline Asset`（管线配置文件）设置里的 `Opaque Texture`
关于 Opaque Texture 的介绍：[通用渲染管线资源 | Universal RP | 12.1.1](https://docs.unity3d.com/cn/Packages/com.unity.render-pipelines.universal@12.1/manual/universalrp-asset.html)

另外，在编辑器运行时为了避免污染原始材质文件，可搭配 [ **UGUIMatFixInEditor 组件**](./防止Unity运行时修改材质球文件.md) 使用

# UIEffect

将UI相关的效果实现的非常完整，应有尽有（UI软遮罩看我另一篇文章 [UI软遮罩](../Unity/Unity知识外链.md)）

github版本：[wangbenchong/UIEffect: UIEffect is an effect component for uGUI element in Unity. Let's decorate your UI with effects! (github.com)](https://github.com/wangbenchong/UIEffect)

gittee仓库备份：[UIEffect: 在github上看到一个日本人写的各种ui效果，我把它从built-in扩展到支持urp，并在最新版Unity6上测试运行通过 (gitee.com)](https://gitee.com/wangbenchong/uieffect)

原作者：[https://github.com/mob-sakai/UIEffect](https://github.com/mob-sakai/UIEffect)

截至目前2024/8/7，原作者还在弄4.0的预览版（里面有些TMPro插件支持的代码还没写完，但也鸽了一年多了）可持续关注。

# ParticleEffectForUGUI

[mob-sakai/ParticleEffectForUGUI: Render particle effect in UnityUI(uGUI). Maskable, sortable, and no extra Camera/RenderTexture/Canvas.](https://github.com/mob-sakai/ParticleEffectForUGUI)

备注：mob-sakai很多仓库都是UGUI效果

# FancyScrollView

酷炫的ScrollView效果：https://github.com/setchi/FancyScrollView

# uGUI-Hypertext

老式Text实现超链接效果：https://github.com/setchi/uGUI-Hypertext
