DNATween是抽离于原版NGUITween，并进行了大幅的优化与改进。形成一个独立插件，可图形化实现3D物体动效或者2D的UI动效。具有便捷的扩展性，理论上想控制什么动效都可以。

代码方面，通过对父类代码[DNATweener.cs](./DNATweener.cs)（必要）和其编辑器代码[DNATweenerEditor.cs](./Editor/DNATweenerEditor.cs)（非必要）进行派生（派生的代码量很小，因为父类代码已包含了时间轴、事件处理等基础功能），实现具体的各种动效，具体如下：

- [DNATweenAlpha.cs](./DNATweenAlpha.cs) 透明度渐变动画
- [DNATweenColor.cs](./DNATweenColor) 颜色渐变动画
- [DNATweenFillAmount.cs](./DNATweenFillAmount.cs) UGUI图片填充度渐变动画
- [DNATweenFOV.cs](./DNATweenFOV.cs) 相机视角范围FOV渐变动画
- [DNATweenNumber.cs](./DNATweenNumber.cs) UGUI数字跑分动画
- [DNATweenNumber_Float.cs](./DNATweenNumber_Float.cs) UGUI数字跑分动画
- [DNATweenOrthoSize.cs](./DNATweenOrthoSize.cs) 正交相机的视图范围渐变动画
- [DNATweenPosition.cs](./DNATweenPosition.cs) 位移变化，支持在平面做直线运动的基础上，在运动方向的法向方向做偏移所形成的曲线路径
- [DNATweenPositionBezier.cs](./DNATweenPositionBezier.cs)   贝塞尔曲线位移，需要安装AssetStore的[Bezier Solution插件](https://assetstore.unity.com/packages/tools/level-design/bezier-solution-113074)，可代替BezierWalkerWithSpeed组件实现更丰富功能
- [DNATweenRectTransformPosition.cs](./DNATweenRectTransformPosition.cs) 位移动画，控制RectTransform的anchoredPosition3D，从而使动画适配不同长宽比屏幕
- [DNATweenRectTranSize.cs](./DNATweenRectTranSize.cs) 控制RectTransform矩形尺寸的动画
- [DNATweenRotation.cs](./DNATweenRotation.cs) 局部坐标系旋转动画
- [DNATweenScale.cs](./DNATweenScale.cs) 缩放动画
- [DNATweenScroll.cs](./DNATweenScroll.cs) UGUI滑动区域进度缓动跳转动画
- [DNATweenSimpleBar.cs](./DNATweenSimpleBar.cs) UGUI进度条进度变化动画
- [DNATweenTransform.cs](./DNATweenTransform.cs) 通过Transform来指定起止点，来做直线位移动画
- [DNATweenVolume.cs](./DNATweenVolume.cs) 控制音量变化

另外还可以通过以下组件将上面的效果进行任意组合：

- [DNATweenTweener.cs](./DNATweenTweener.cs)，本身也是派生自DNATweener，可以批量控制任意多个DNATweener效果，还能选择使这些效果共用一个Update方法来节省性能。

