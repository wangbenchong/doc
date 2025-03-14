# 传统Light Probe Group

[212-光照探针实现间接光影3【unity2022入门教程】-技术美术入门系列-32_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1RY411M7WZ/?spm_id_from=333.1387.favlist.content.click&vd_source=563d44869c3ecebb1867233573d16b7b)

相关笔记：[图形-技术美工相关/14-光照探针.md · chutianshu/AwesomeUnityTutorial - 码云 - 开源中国](https://gitee.com/chutianshu1981/AwesomeUnityTutorial/blob/main/图形-技术美工相关/14-光照探针.md)

# Unity6 Adaptive Probe Volume 自适应探针体积

[Unity 6新特性：Adaptive Probe Volumes（自适应光照探针）_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1GWwmeSETr/?spm_id_from=333.1387.favlist.content.click&vd_source=563d44869c3ecebb1867233573d16b7b)

代码级解析：[Unity6 Adaptive Probe Volume详解 - 知乎](https://zhuanlan.zhihu.com/p/8218046347)

具体可见文档：[Unity6新特性](../../Unity/新特性/Unity6.md)

## 使用步骤

- Settings目录下，Pipeline Asset配置文件中，Lighting分类下，将Light Probe System类型更改为Adaptive Probe Volumes 
- 创建一些可以实际打光的物体，标记为static，做一些发光物体
- Lighting窗口：Scene分页
  - 在Realtime Lighting分析类下，取消勾选Realtime Global Illumination；
  - 在Mixed Lighting分类下，勾选Baked Global Illumination，同时Lighting Mode设置为Baked Indirect
- Lighting窗口：点击按钮“Generate Lighting”执行烘焙
- Hierarchy中，`右键 > Light > Adaptive Probe Volume` 来创建对应物体
