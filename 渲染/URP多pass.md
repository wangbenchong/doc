# RenderObject

不用写C#，纯面板操作，低可定制/操控

- URP项目Settings目录下有PipelineAsset和RendererData两类资源。

- RendererData的实例会填入到PipelineAsset的RendererList中。

- RenderData面板有“Add Renderer Feature”按钮，点击可以添加Render Objects。

Render Objects面板信息：

```c
Name//名字，备注用
Event//执行时机
/*
BeforeRendering：在场景开始渲染之前触发的事件。这可以用于执行一些预渲染的初始化工作。
BeforeReflections：在反射贴图渲染之前触发的事件（如果场景使用了反射）。
AfterReflections：在反射贴图渲染之后触发的事件。
BeforeRenderingShadows：在阴影渲染之前触发的事件。
AfterRenderingShadows：在阴影渲染之后触发的事件。
BeforeRenderingOpaques：在不透明物体（Opaque objects）开始渲染之前触发的事件。这通常意味着透明物体（Transparent objects）和半透明物体（Cutout objects）还没有开始渲染。
AfterRenderingOpaques：在不透明物体渲染完成之后触发的事件。这之后，通常开始渲染透明或半透明物体。
BeforeRenderingSkybox：在天空盒（Skybox）渲染之前触发的事件。
AfterRenderingSkybox：在天空盒渲染之后触发的事件。
BeforeRenderingTransparents：在透明物体开始渲染之前触发的事件。
AfterRenderingTransparents：在透明物体渲染完成之后触发的事件。
AfterRendering：在整个场景渲染完成之后触发的事件。这可以用于执行一些后渲染的清理工作。
*/
Filters//过滤
{
	Queue//指定Queue的材质
	Layer Mask//指定Layer的物体
	LightMode Tags//指定LightMode的Pass，名字自定义不用写成预设名字
}
Overrides//重载
{
	Material//使用指定材质，只能固定参数
    {
        Pass Index//索引编号
    }
	Depth//Toggle深度
    {
        Write Depth//Toggle
        Depth Test//Disable/Never/Less/Equal/LessEqual/Greater/NotEqual/GreaterEqual/Always
    }
	Stencil//Toggle模板
    {
        Value//滑杆（0~15）
        Compare Function//Disable/.../Always
 //Keep/Zero/Replace/IncrementSaturate/DecrementSaturate/Invert/IncrementWrap/DecrementWrap
        Pass
        Fail
        Z Fail
    }
	Camera//Toggle
    {
        Field Of View//滑杆（4~179，默认60）
        Position Offset//Vector3
        Restore//Toggle
    }
}

```

# RenderFeature

要写很多C#，高可定制/操控

代码写法参考：

[超小白向URP Scriptable Render Feature写法！——理论篇 - 哔哩哔哩 (bilibili.com)](https://www.bilibili.com/read/cv17521507/)

[超小白向URP Scriptable Render Feature写法——实现老式CRT电视效果！ - 哔哩哔哩 (bilibili.com)](https://www.bilibili.com/read/cv17531935)

[unity在URP中实现自定义Volume_unity volume-CSDN博客](https://blog.csdn.net/xdedzl/article/details/114746972)



# 利用shader中的LightMode标签

如果给两个pass的其中一个打上"LightMode" = "SRPDefaultUnlit"（不写默认就是这个，为了兼容builtin预留的标签，具体可查看unity安装目录下的DrawObjectsPass.cs脚本） 这样的Tags标签（另一个是”UniversalForward”）则两个Pass都会执行，并且“SRPDefaultUnlit”会先执行。但是通过frameDebugger能够查看到，说明没有合批。该手段只能在双pass执行的时候用一用。超过两个就只能RenderFeature了。