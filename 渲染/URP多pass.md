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