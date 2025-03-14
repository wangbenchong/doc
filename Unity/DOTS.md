# 简介

Unity中的DOTS（Data-Oriented Technology Stack）是一种新架构，旨在提高游戏和应用程序的性能，特别是在处理大量数据时。以下是对DOTS技术的详细介绍：

## 核心理念

DOTS的核心理念是数据导向编程，它通过将数据与行为分离来优化性能，充分利用现代多核处理器的能力。

## 关键组成部分

### **ECS（Entity Component System）**

- **实体（Entity）**：实体是游戏世界中的基本对象，可以是任何东西，如角色、道具或环境元素。实体本身不包含数据或行为，只是一个标识符，用来指示某个对象的存在。
- **组件（Component）**：组件是附加到实体上的数据容器，包含实体的状态信息（如位置、速度、生命值等）。组件是纯数据，不包含任何逻辑。
- **系统（System）**：系统是处理组件数据的逻辑单元，负责对数据进行逻辑操作，对具有特定Component的特定Entity进行操作。系统会遍历所有具有特定组件的实体，并执行相应的操作。

### **Job System**

- Job System允许开发者将计算任务分解为多个小的、独立的任务（Job），这些任务可以并行执行，从而充分利用多核处理器的能力。
- Job System提供了调度机制，可以在运行时动态安排Job的执行顺序，优化CPU资源的使用。
- Job System通过编译时检查和数据访问模式，确保在并行执行时不会发生数据竞争和冲突。

### **Burst Compiler**

Burst Compiler是一个高性能的编译器，它利用 LLVM（Low-Level Virtual Machine）可以将C#代码编译为高度优化的原生代码（Native Code），特别适用于数学密集型操作（如物理模拟、动画、粒子系统等）。通过提前编译，Burst 可以减少运行时的 JIT（Just-In-Time）编译开销，从而提升运行时性能。如果你在项目中使用 Entities、Jobs System 或 Burst 编译的代码，Burst Compile 是必需的。

#### **为什么每次启动时都需要 Burst Compile？**

1. **代码变化检测**：
   - Unity 会检测项目中是否有代码发生变化（包括脚本、DOTS 相关代码等）。如果有变化，Burst 编译器会重新编译相关代码，以确保生成的机器码是最新的。
2. **平台和目标设备的变化**：
   - 如果你切换了目标平台（例如从 PC 切换到 Android），Burst 需要重新编译代码，因为不同平台的机器码是不同的。
3. **缓存机制**：
   - Burst 编译器会缓存编译结果，但如果缓存失效（例如 Unity 版本更新、项目设置更改等），它会重新编译。



#### **是否可以跳过 Burst Compile？**

1. **如果未使用 DOTS 或 Burst**：

   - 如果你的项目没有使用 DOTS 或 Burst 编译器，可以通过关闭 Burst 来跳过这一过程：

     ~~Edit > Project Settings > Player。在 Other Settings 中，找到 Burst 选项并禁用 Enable Burst Compilation。~~

     菜单栏 > Jobs > Burst > 取消勾选 Enable Compilation

2. **如果使用 DOTS 或 Burst**：

   - 如果你使用了 DOTS 或 Burst，则不能跳过这一过程，因为 Burst 编译是生成高性能代码的必要步骤。



#### **如何加快 Burst Compile 的速度？**

1. **减少代码变化**：
   - 尽量避免频繁修改代码，以减少 Burst 重新编译的次数。
2. **使用增量编译**：
   - Burst 支持增量编译，只有修改的部分会重新编译。确保 Unity 的缓存机制正常工作。
3. **升级 Unity 版本**：
   - Unity 的后续版本可能会优化 Burst 编译器的性能，升级到最新版本可能会加快编译速度。
4. **关闭不必要的 Burst 编译**：
   - 如果你确定某些代码不需要 Burst 编译，可以在代码中添加 `[BurstDiscard]` 属性，跳过这些代码的 Burst 编译。



## 性能优化机制

1. **内存布局优化**：DOTS强调数据的内存布局，以提高缓存命中率和减少内存访问延迟。通过将相关数据存储在一起，可以提高数据访问的效率。
2. **减少GC（垃圾回收）**：通过使用结构化数据和避免频繁的内存分配，DOTS可以减少垃圾回收的频率，从而提高性能。
3. **Chunks机制**：Chunks是固定大小的内存块，用于存储多个实体及其对应的组件数据。这种设计确保了内存的连续性，使得CPU在访问数据时能够更高效地利用缓存，从而减少cache miss的发生。

## 应用场景

1. **大规模场景**：DOTS特别适合需要处理大量实体的场景，如大型开放世界游戏、模拟器和实时策略游戏。
2. **高性能需求**：对于需要高帧率和低延迟的应用（如VR/AR），DOTS提供了必要的性能支持。

## 学习资源

1. **官方文档**：[Entities overview | Entities | 1.3.8](https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/index.html)
   以及插件通过packagemanager下载后Library下的~documents文件夹
2. **社区资源**：

- [游戏开发RAIN：Unity ECS DOTS 教程合集：最新Unity DOTS进阶与项目实战 #ECS#Component#System#Entity_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV192k7YXEnX/)
- [Metaverse大衍神君：《DOTS之路》系列课程——课程介绍_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV19B4y177hY)

- [可盖大人ProMAX：Unity6-DOTS](https://space.bilibili.com/1751223347/channel/collectiondetail?sid=2643803)

- [Unity3D面试题《DOTS-ECS系列》_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1Bw4m1m75c)

- [（课堂）Unity6 DOTS万人同屏项目实战-官剑铭_哔哩哔哩_bilibili](https://www.bilibili.com/cheese/play/ss22793)

还有一些Unity官方社区的文章：

- [Job System之Hello World - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5f4ba346edbc2a0020dcec6d)
- [Unity2020.1中如何安装DOTS的Entities包？ - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5f2fb15eedbc2a002041671b)
- [ECS入门之Hello World - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5f54ea0bedbc2a0021827638)
- [【实战】使用Job来修改Transform - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5f5e1afaedbc2a001f17a85b)

- [ECS的核心概念 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5f672c0aedbc2a5d37eeea3f)

- [ECS中的Entity实体 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5f706d0bedbc2a00206032fa)

- [ECS之Component组件 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5fad0967edbc2a0020aca915)
- [ECS之System系统 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5fb5df01edbc2a002100daeb)

- [在ECS系统中使用Entities.ForEach - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5fbf1843edbc2a66e29fcda5)

- [在ECS系统中使用Job.WithCode - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5fc86125edbc2a415d143f06)
- [在ECS系统中使用IJobChunk作业 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5fd6d038edbc2a49b5e5d12d)

总之，Unity DOTS是一个强大的工具集，通过ECS、Job System和Burst Compiler等组件，开发者可以创建高效、可扩展的应用程序，充分利用现代硬件的能力。
