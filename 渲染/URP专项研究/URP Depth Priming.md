# Depth Priming（Pre-Z pass）

从Unity2021开始，新增了Depth Priming功能，优化渲染性能，减少Overdraw，尽量把不需要渲染的内容截断在片元着色器之前。

# 作用

   弥补Early-Z的漏网之鱼，Early-Z失效的原因：

- 开启了AlphaTest
- 手动调用了Clip/Discard指令丢弃了片元
- 关闭了DepthTest或手动修改了GPU插值得到的深度

# 配置地点

其开关配置在 Universal Renderer Data资源上，面板如下：

```c
Rendering
    RenderingPath : Forward/Deferred
    	Depth Priming Mode : Auto/Force/Disabled//只有Disable是关闭,那些没写 DepthOnly Pass 的材质会隐藏，只剩下阴影
```



Depth Priming Mode指定何时执行深度引导 (depth priming)。深度引导是一种优化方法，用于检查 URP 在[基础摄像机](https://docs.unity3d.com/cn/Packages/com.unity.render-pipelines.universal@12.1/manual/camera-types-and-render-type.html#base-camera)的不透明渲染通道中不需要渲染的像素。它会使用在深度预通道中生成的深度缓冲区。选项包括：
• **Disabled**：URP 不执行深度引导。
• **Auto**：URP 为需要深度预通道的渲染通道执行深度引导。
• **Forced**：URP 始终执行深度引导。为此，它还会为每个渲染通道执行深度预通道。

仅当 **Rendering Path** 设置为 **Forward** 时，才会显示此属性；移动平台只有设为Force时才会开启。

# 副作用

- 当开启时材质shader必须写 DepthOnly Pass，否则不可见。补救代码见 ***URP标准框架脚本_无光照.md***
- 当场景不复杂，Overdraw不是造成GPU效率的瓶颈，可能造成负优化
- 手机上（tiled GPU即TBR架构）Depth Priming与MSAA同时开启带来的额外开销
- DrawCall或其他图形API调用的开销

# 调试

若开启成功，Frame Debug中会多出一条DepthPrepass/RenderLoop.DrawSRPBatcher，其中会有若干SRP Batch。而真正渲染对象的DrawOpqueObjects/RendrLoop.DrawSRPBatcher中的若干SRP Batch，其ZTest已经由LEqual变成了Equal。

# 完整视频介绍

https://www.bilibili.com/video/BV1XS4y1S7fw
