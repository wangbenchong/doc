# Unity6相关官方文档

[Unity - Manual: Using the Universal Render Pipeline](https://docs.unity3d.com/6000.0/Documentation/Manual/universal-render-pipeline.html)

# 摘抄与翻译_总纲



The Universal Render Pipeline (URP) is a prebuilt [Scriptable Render Pipeline](https://docs.unity3d.com/6000.0/Documentation/Manual/scriptable-render-pipeline-introduction.html), made by Unity. URP provides artist-friendly workflows that let you quickly and easily create optimized graphics across a range of platforms, from mobile to high-end consoles and PCs.
*通用渲染管线 （URP） 是由 Unity 制作的预构建[可编程渲染管线](https://docs.unity3d.com/6000.0/Documentation/Manual/scriptable-render-pipeline-introduction.html)。URP 提供对艺术家友好的工作流程，让您可以快速轻松地在各种平台（从移动设备到高端游戏机和 PC）上创建优化的图形。*

| **Page**                                                     | **Description**                                              |
| :----------------------------------------------------------- | :----------------------------------------------------------- |
| [Introduction to URP](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/urp-introduction.html)<br />*URP简介* | Learn about how URP creates optimized graphics across a range of platforms.<br />*了解 URP 如何在各种平台上创建优化的图形。* |
| [Requirements and compatibility](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/requirements.html)<br />*要求和兼容性* | Understand the compatibility of URP package versions with different Unity Editor versions, and system requirements.<br />*了解 URP 包版本与不同 Unity Editor 版本的兼容性以及系统要求。* |
| [What’s new in URP 17 (Unity 6)](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/whats-new/urp-whats-new.html)<br />*URP17新特性* | Find out what’s changed in URP 17, and check the documentation for the affected areas.<br />*了解 URP 17 中的更改，并查看受影响区域的文档。* |
| [Get started](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/introduction-landing.html)<br />*URP入门* | Resources for understanding fundamental concepts and assets in URP, and installing and upgrading.<br />*用于了解 URP 中的基本概念和资产以及安装和升级的资源。* |
| [Graphics quality settings](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/urp-quality-settings-landing.html)<br />*图形质量设定* | Approaches for using a URP Asset or the URP Config package to change the quality settings of a project.<br />*使用 URP 资源或 URP Config 包更改工程质量设置的方法。* |
| [Add anti-aliasing](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/anti-aliasing.html)<br />*添加抗锯齿* | To reduce jagged lines, add Fast Approximate Anti-aliasing (FXAA), Subpixel Morphological Anti-aliasing (SMAA), Temporal Anti-aliasing (TAA) or Multisample Anti-aliasing (MSAA).<br />*要减少锯齿状线条，请添加快速近似抗锯齿 （FXAA）、子像素形态抗锯齿 （SMAA）、时间抗锯齿 （TAA） 或多重采样抗锯齿 （MSAA）。* |
| [Custom rendering and post-processing](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/customizing-urp.html)<br />*自定义渲染和后处理* | Techniques for customizing and extending the rendering process in URP.<br />*在 URP 中自定义和扩展渲染过程的技术。* |
| [URP reference](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/urp-reference-landing.html)<br />*URP参考* | Explore the properties and settings in the Universal Render Pipeline to customize rendering.<br />*探索 Universal Render Pipeline 中的属性和设置以自定义渲染。* |

**Additional resources *其他资源***

- [Render pipelines *渲染管线*](https://docs.unity3d.com/6000.0/Documentation/Manual/render-pipelines.html)
- [Choosing a render pipeline *选择渲染管线*](https://docs.unity3d.com/6000.0/Documentation/Manual/choose-a-render-pipeline-landing.html)
- [Execute rendering commands in a custom render pipeline *在自定义渲染管道中执行渲染命令*](https://docs.unity3d.com/Packages/com.unity.render-pipelines.core@17.0/manual/srp-using-scriptable-render-context.html)





# 要求和兼容性

## Unity 编辑器兼容性

下表显示了 URP 包版本与不同 Unity Editor 版本的兼容性。

| 包版本      | 最低 Unity 版本 | 最高 Unity 版本 |
| :---------- | :-------------- | :-------------- |
| 16.0.x 版本 | 2023.2          | 2023 年 x       |
| 15.0.x 版本 | 2023.1          | 2023.1          |
| 14.0.x 版本 | 2022.2          | 2022 年 x       |
| 13.x.x      | 2022.1          | 2022.1          |
| 12.0.x 版本 | 2021.2          | 2021.3          |
| 11.0.0      | 2021.1          | 2021.1          |
| 10.x 版本   | 2020.2          | 2020.3          |
| 9.x-preview | 2020.1          | 2020.2          |
| 8.x         | 2020.1          | 2020.1          |
| 7.x         | 2019.3          | 2019.4          |

自 Unity 2021.1 发布以来，图形包是[核心 Unity 包](https://docs.unity3d.com/2021.2/Documentation/Manual/pack-core.html)。

对于每个 Unity 版本（Alpha、Beta、补丁版本），主 Unity 安装程序都包含以下图形包的最新版本：SRP Core、URP、HDRP、**Shader** Graph、VFX Graph。自 Unity 2021.1 版本以来，Package Manager 仅显示图形包的主要修订版（所有 Unity 2021.1.x 版本均为 11.0.0，所有 Unity 2021.2.x 版本均为 12.0.0）。

您可以使用 Package Manager 或通过修改文件从磁盘安装不同版本的图形包。`manifest.json`

## 渲染管线兼容性

使用 URP 创建的项目与高清渲染管线 （HDRP） 或内置渲染管线不兼容。在开始开发之前，您必须决定在项目中使用哪个渲染管线。有关选择渲染管道的信息，请查看[渲染管道](https://docs.unity3d.com/2019.3/Documentation/Manual/render-pipelines.html)部分。

## Unity Player 系统要求

此软件包不会添加任何额外的特定于平台的要求。适用 Unity Player 的一般系统要求。有关 Unity 系统要求的更多信息，请查看 [Unity 的系统要求](https://docs.unity3d.com/Manual/system-requirements.html)。

## 其他资源

- [渲染管线功能比较参考](https://docs.unity3d.com/6000.0/Documentation/Manual/render-pipelines-feature-comparison.html)
- [内置渲染管线的硬件要求](https://docs.unity3d.com/6000.0/Documentation/Manual/RenderTech-HardwareRequirements.html)

# URP17新特性

有关 URP 17 中所做的更改的完整列表，请参阅[更改日志](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/changelog/CHANGELOG.html)。

## 特征

本节包含此版本中新增功能的概述。

### 渲染图形系统

在此版本中，Unity 引入了[render graph渲染图](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/render-graph.html)系统。渲染图系统是在 **可编程渲染管线应用程序接口**（SRP API）之上构建的框架。此系统改进了自定义和维护渲染管线的方式。

渲染图系统减少了 URP 使用的内存量，并使内存管理更加高效。渲染图系统仅分配帧实际使用的资源，您不再需要编写复杂的逻辑来处理资源分配并考虑极少数最坏情况。渲染图系统还可以在计算和图形队列之间生成正确的同步点，从而减少帧时间。

[通过 Render Graph Viewer](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/render-graph-viewer-reference.html)，您可以可视化渲染过程、使用帧资源以及调试渲染过程。

有关渲染图系统的更多信息，请参阅[渲染图](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/render-graph.html)文档。

### 后处理中的 Alpha 处理设置

URP 17 添加了 **Alpha 处理**设置（**URP Asset** > **Post-processing** > **Alpha Processing**）。如果启用此设置，URP 会将后处理输出渲染为具有 Alpha 通道的渲染纹理。在以前的版本中，URP 通过将 Alpha 值替换为 1 来丢弃 Alpha 通道。

渲染目标需要具有 Alpha 通道的格式。对于 SDR（HDR 关闭），相机颜色缓冲区格式必须为 RGBA8；对于 HDR（64 位），相机颜色缓冲区格式必须为 RGBA16F。您可以使用 **URP Asset** **> Quality** 中的设置来配置格式。

此功能的示例使用案例：

- 在游戏中渲染**用户界面**，例如平视显示器。您可以渲染多个**render textures 渲染纹理**使用不同的**后处理**配置，并使用 Alpha 通道编写最终输出。
- 渲染角色自定义屏幕，其中 Unity 使用不同的后处理效果渲染背景界面和 3D 角色，并使用 Alpha 通道混合它们。
- **XR 系列**需要支持视频直通的应用程序。

### 减少 CPU 上的渲染工作

URP 17 包含新功能，可让您通过将某些任务移动到 GPU 并减少 CPU 上的工作负载来加快渲染过程。

#### GPU Resident（意为驻留）  Drawer

URP 17 包括一个名为 **GPU Resident Drawer** 的新渲染系统。

该系统自动使用 [BatchRendererGroup API](https://docs.unity3d.com/Manual/batch-renderer-group.html) 进行绘制**游戏对象**使用 GPU 实例化，从而减少绘制调用的数量并释放 CPU 处理时间。

有关 GPU Resident Drawer的更多信息，请参阅[减少 CPU 上的渲染工作](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/reduce-rendering-work-on-cpu.html)部分。

#### GPU 遮挡剔除

使用 GPU 时**遮挡剔除**时，Unity 使用 GPU 而不是 CPU 在对象被其他对象遮挡时将其排除在渲染之外。Unity 使用此信息来加快**场景**
它们有很多遮挡。

有关 GPU 遮挡剔除的更多信息，请参阅[减少 CPU 上的渲染工作](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/gpu-culling.html)部分。

### Forward+ 渲染路径中的注视点渲染

Forward+**渲染路径**现在支持注视点渲染（foveated rendering）。

### 相机历史记录 API

此版本包含[摄像机历史 API](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/api/UnityEngine.Rendering.Universal.UniversalCameraHistory.html)，它允许您访问每个摄像机的历史纹理，并在自定义渲染通道中使用它们。History 纹理是 Unity 为每个纹理渲染的颜色和深度纹理**照相机**
在前几帧中。

您可以将历史纹理用于渲染算法，这些算法使用来自一个或多个先前帧的帧数据。

URP 实现每个摄像机的颜色和深度纹理历史记录以及自定义渲染通道的历史访问。

### Rendering Debugger 中的 Mipmap Streaming 部分

[Rendering Debugger](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/features/rendering-debugger.html) 现在包含一个 **Mipmap Streaming** 部分。此部分允许您检查纹理流活动。

### 时空后处理 （STP）

时空后处理 （STP） 通过放大 Unity 以较低分辨率渲染的帧来优化 GPU 性能并提高视觉质量。STP 适用于支持计算的桌面平台、控制台和移动设备**着色**

要启用 STP，请在活动的 **URP 资源**中，选择 **Quality** > **Upscaling Filter** > **Spatial Temporal Post-Processing （STP）。**

## 改进

本节包含此版本中主要改进的概述。

### 自适应探针体积 （APV）

此版本包含对 [Adaptive Probe Volume](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/probevolumes.html) 的以下改进：

- [APV 照明场景混合](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/probevolumes-bakedifferentlightingsetups.html)。
- [APV 天空遮挡支持](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/probevolumes-skyocclusion.html)。
- [APV 磁盘流式处理](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/probevolumes-streaming.html)。

### Volume框架增强功能

此版本在所有平台（尤其是移动平台）上对 Volume Framework 的 CPU 性能进行了优化。现在，您可以设置全局Volume默认值并在质量设置中覆盖它们。

###  阴影纹理分辨率

**Shadow Resolution （阴影分辨率**） 属性现在包含 Main Light （主光源） 和 Additional Lights （附加光源） 的选项。

### 使用 URP Config 包更改渲染管线设置

[URP Config 包](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/URP-Config-Package.html)允许您更改 Editor 界面中不可用的某些渲染管线设置。

例如，您可以[更改可见光的最大数量](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/rendering/forward-plus-rendering-path-limitations.html)。

### URP 文档已移至 Unity 手册

Unity 6 中通用渲染管线的文档已从单独的 URP 文档站点移至主 Unity 手册。我们重新构建了特定于 URP 的图形页面和通用图形页面，使其更加关注用户结果。此更改的目的是提高 URP 文档的可发现性和读者体验。

指向单独 URP 站点上的页面的链接现在重定向到主手册（或等效手册）中重新定位的页面。

[URP 脚本 API](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@17.0/api/index.html) 文档仍位于单独的网站上。

## 已解决的问题

有关 URP 17 中已解决的问题的完整列表，请参阅[更改日志](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/changelog/CHANGELOG.html)。

## 已知问题

有关 URP 17 中已知问题的信息，请参阅[已知问题](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/known-issues.html)部分。

# URP入门

用于了解通用渲染管道 （URP） 中的基本概念和资源以及安装和升级的资源。

| **页**                                                       | **描述**                                          |
| :----------------------------------------------------------- | :------------------------------------------------ |
| [URP 基础知识](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/urp-concepts.html) | 用于了解渲染循环和不同**渲染路径** 在 URP 中。    |
| [安装和升级 URP](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/InstallingAndConfiguringURP.html) | 创建使用 URP 的项目，或从内置渲染管线升级到 URP。 |
| [配置以获得更好的性能](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/configure-for-better-performance.html) | 禁用或更改对性能有较大影响的 URP 设置和功能。     |

其他资源：

- [渲染管道](https://docs.unity3d.com/6000.0/Documentation/Manual/render-pipelines.html)

## 在 Universal Render Pipeline 中渲染

**通用渲染管线**（URP）使用以下组件来渲染场景：

- URP 渲染器。URP 包含以下渲染器：
  - [通用渲染器](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/urp-universal-renderer.html)。
  - [2D 渲染器](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/Setup.html)。
- URP自带的着色器所使用的[着色模型](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/shading-model.html)
- 照相机
- [URP 资产](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/universalrp-asset.html)

下图显示了 URP Universal Renderer 的帧渲染循环。

![URP 通用渲染器，前向渲染路径](https://docs.unity3d.com/6000.0/Documentation/uploads/urp/Graphics/Rendering_Flowchart.png)URP 通用渲染器，前向渲染路径

当[渲染管线在 Graphics Settings 中处于激活状态](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/InstallURPIntoAProject.html)时，Unity 会使用 URP 来渲染项目中的所有摄像机，包括游戏视图和场景视图中的摄像机、**反射探针**Reflection Probes、以及Inspector上的预览窗口。

URP 渲染器为每个摄像机执行一个 Camera 循环，该循环执行以下步骤：

1. 剔除场景中的渲染对象
2. 为渲染器构建数据
3. 执行将图像输出到**帧缓冲区framebuffer**的渲染器。



### 相机循环

Camera （摄像机） 循环执行以下步骤：

| 步               | 描述                                                         |
| :--------------- | :----------------------------------------------------------- |
| **设置剔除参数** | 配置用于确定剔除系统如何剔除光源和阴影的参数。您可以使用自定义渲染器覆盖渲染管道的这一部分。 |
| **剔除**         | 使用上一步中的剔除参数来计算摄像机可见的可见渲染器、阴影投射物和光源的列表。剔除参数和摄像机[图层距离](https://docs.unity3d.com/ScriptReference/Camera-layerCullDistances.html)会影响剔除和渲染性能。 |
| **构建渲染数据** | 根据剔除输出、[URP 资源](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/universalrp-asset.html)的质量设置、[照相机](https://docs.unity3d.com/6000.0/Documentation/Manual/Cameras.html) 和当前正在运行的平台来构建 .渲染数据告诉渲染器摄像机和当前所选平台所需的渲染工作量和质量。`RenderingData` |
| **设置渲染器**   | 构建渲染通道列表，并根据渲染数据将它们排入执行队列。您可以使用自定义渲染器覆盖渲染管道的这一部分。 |
| **执行渲染器**   | 执行队列中的每个渲染传递。渲染器将 Camera 图像输出到帧缓冲区。 |

# 图形质量设定



# 添加抗锯齿



# URP参考



# 自定义渲染和后处理







