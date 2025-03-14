# GPU Resident Drawer

[Unity6：GPU Resident Drawer真有用吗？_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV18QqtYaE3y)

旨在GPU当中进行图形渲染。SRP Batcher 管理材质批处理，而 GPU Resident Drawer 管理实例网格的长期驻留，二者可以协同工作。

## 配置地点

首先在 Project Settings要做如下配置：

```
Graphics
    BatchRendererGroup Variants (从Strip if no Entities Graphics package改成Keep All，即不要去裁剪没有用到的内容)
```

同时还需使用Forward+管线，配置在Universal Renderer Data上：

```
Rendering
    Rendering Path： 从Forward改成Forward+，可以明显改善多光源的性能
```

在 Universal Renderer Pipeline Asset资源上，面板配置如下：

```
Rendering
    GPU Resident Drawer（从Disabled改成Instanced Drawing，作用于多个同类型对象，比GPU instance先进在于减少CPU到GPU的drawcall（即Batches），如果场景中物体各不相同，则不必开启，总的来说性能提升不明显甚至有可能负优化）
```

# 遮挡剔除

[Unity6：GPU遮挡剔除性能如何？_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1TpkQYMEwf/)

GPU Resident Drawer的功能之一，利用GPU代替CPU做遮挡剔除。目前（2024年底）移动端不支持，高通芯片强制禁用了。6000.0.23之前版本是可以开启但是高通芯片上物体会丢失。目前属于鸡肋更新。

## 配置地点

其开关配置在 Universal Renderer Pipeline Asset资源上，面板如下：

```c
Rendering
    GPU Resident Drawer ：Instanced Drawing
    	Small-Mesh Screen-Percentage
    	GPU Occlusion Culling（勾选）
```

## 查看遮挡热力图

### Rendering Debugger

菜单栏：Window/Analysis/Rendering Debugger
GPU Resident Drawer的Occlusion Culling中，勾选Occlusion Test Overlay

### scene视图

越红代表遮挡剔除的效率越高（但Batches变高，打断了合批），越黄蓝代表效率越低
