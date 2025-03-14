[【Unity】批处理和实例化的底层优化原理_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1ZmSgYsEME/)

[【Unity】SRP Batcher的底层优化原理_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1BYSVYaENy/)

关于SRP Batcher 的官方文章：[可编程渲染管线 SRP Batcher - Unity 手册](https://docs.unity.cn/cn/2019.4/Manual/SRPBatcher.html)

检测方法：1.FrameDebugger 2.打包后使用RenderDoc（更准确）

群里聊到的一些细碎知识点：

- SRPBatch合批是合的SetPass的数量，并不能减少DrawCall的数量，需要GPUInstance


- 术语：VS消耗——顶点Shader消耗， PS消耗——片元Shader消耗


- 性能消耗最主要看的是Draw Call？还是SetPass Calls？
  答：urp下看SetPass Calls，意为改变渲染状态。SetPassCalls减少确实能减少一些消耗，但是DrawCall的消耗还是大头，SRPBatch就是刚才说的减少Setpass

- GPUInstance是在GPU上给一个模型绘制多次，比如说，我有x个一样的对象。没有GPUInstance之前，是一个DC渲染一个对象。有GPUInstance之后是在一个DC里面渲染x个对象。


- GPUInstance要实现会麻烦一点，最简单的就是在使用支持GPUInstance的材质球上勾选使用，然后Unity就会尝试使用实例化


- SRPBatch和GPUInstance是一定程度上互斥的


- 那如何判断是改该使用SRPBatch还是GPUInstance？
  GPUInstance和SRPBatch都支持的话会优先SRPBatch如果你非要使用GPUInstance就要先打断SRPBatch



关于静态合批和动态合批：
静态合批就是预先合成一个大模型，占用内存，已经不吃香了，现在基本可以放弃
动态合批是在CPU层面对小模型进行合并，占用CPU资源，Unity会选择性合并，常见在粒子的渲染这样很多小Mesh的情况
很多时候都是在模型层面就合了，GPU是不知道的，截帧也看不出来