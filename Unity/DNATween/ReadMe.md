DNATween 是抽离于原版 NGUITween，并进行了大幅的优化与改进。形成一个独立插件，可图形化实现 3D 物体动效或者 2D 的 UI 动效。具有便捷的扩展性，理论上想控制什么动效都可以。

代码方面，通过对父类代码 [DNATweener.cs](./DNATweener.cs)（必要）和其编辑器代码 [DNATweenerEditor.cs](./Editor/DNATweenerEditor.cs)（非必要）进行派生（派生的代码量很小，因为父类代码已包含了时间轴、事件处理等基础功能），实现具体的各种动效，具体如下：

## 位移、旋转、缩放相关

- [DNATweenPosition.cs](./DNATweenPosition.cs) 直线位移动画，可切换局部、世界坐标系。也可以通过在2D平面上沿运动方向的法向方向做偏移来形成的简单的曲线位移。
- [DNATweenSplineCart](./DNATweenSplineCart.cs)  一种可视化编辑贝塞尔曲线位移（路径点编辑是基于数据的），需要安装 Cinemachine3.x 版本插件，功能比 Cinemachine 自带的 `CinemachineSplineCart` 要强大很多。
- [DNATweenPositionBezier.cs](./DNATweenPositionBezier.cs)   另一种可视化编辑贝塞尔曲线位移（路径点编辑是基于物体的），需要安装 AssetStore 的 [Bezier Solution 插件](https://assetstore.unity.com/packages/tools/level-design/bezier-solution-113074)，可代替 `BezierWalkerWithSpeed` 组件实现更丰富功能
- [DNATweenRectTransformPosition.cs](./DNATweenRectTransformPosition.cs) UI位移动画，控制 `RectTransform` 的 `anchoredPosition3D`，从而使动画适配不同长宽比屏幕
- [DNATweenTransform.cs](./DNATweenTransform.cs) 通过 Transform 来指定起止点，来做直线位移动画
- [DNATweenRotation.cs](./DNATweenRotation.cs) 局部坐标系旋转动画
- [DNATweenScale.cs](./DNATweenScale.cs) 缩放动画

## 其他Tween：

- [DNATweenAlpha.cs](./DNATweenAlpha.cs) 透明度渐变动画
- [DNATweenColor.cs](./DNATweenColor) 颜色渐变动画
- [DNATweenFillAmount.cs](./DNATweenFillAmount.cs) UGUI 图片填充度渐变动画
- [DNATweenFOV.cs](./DNATweenFOV.cs) 相机视角范围 FOV 渐变动画
- [DNATweenNumber.cs](./DNATweenNumber.cs) UGUI 数字跑分动画
- [DNATweenNumber_Float.cs](./DNATweenNumber_Float.cs) UGUI 数字跑分动画
- [DNATweenOrthoSize.cs](./DNATweenOrthoSize.cs) 正交相机的视图范围渐变动画
- [DNATweenRectTranSize.cs](./DNATweenRectTranSize.cs) 控制 RectTransform 矩形尺寸的动画
- [DNATweenScroll.cs](./DNATweenScroll.cs) UGUI 滑动区域进度缓动跳转动画
- [DNATweenSimpleBar.cs](./DNATweenSimpleBar.cs) UGUI 进度条进度变化动画
- [DNATweenVolume.cs](./DNATweenVolume.cs) 控制音量变化

## 组合效果以及总控

另外还可以通过以下组件将上面的效果进行任意组合：

- [DNATweenTweener.cs](./DNATweenTweener.cs)，本身也是派生自 DNATweener，可以批量控制任意多个 DNATweener 效果，使这些效果共用一个 Update 方法来节省性能。还可以指定这些子 Tweener 是否使用自己的进度曲线。也可以利用 DNATweenTweener 的时间轴功能（在 `Other Default` 分类下，有个 `Time Point List`），总控一系列动画或事件（几乎可以取代 Unity 的 Timeline 插件，某些方面还更灵活）。

## 额外扩展工具脚本

- Attribute 目录：一些常用Attribute
- UI 目录：一些常用UI组件
