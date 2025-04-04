

# 图压

[度盘链接](https://pan.baidu.com/s/1424f9Bemtjk9gnigIbn5tw?pwd=h7h4)，解压直接运行，无需安装
先用原尺寸做压缩，否则画面损失会很多。压缩数值到9基本是极限。

# GIF动态图压缩

## 编辑GIF的工具：UleadGIFAnimator

通过网盘分享的文件：UleadGIFAnimator_5.10_Single.zip
链接: https://pan.baidu.com/s/1nO5blo67Bhox0DyRCOOwNA?pwd=y7hu 提取码: y7hu 
--来自百度网盘超级会员v7的分享

按顺序来执行压缩：

1. 裁剪尺寸（框选-编辑-修整画布）
2. 删帧（无法批量删间隔帧，但可通过shift连选来删连续帧）
3. 编辑-调整图像大小

## 用PhotoShop辅助编辑

UleadGIF无法处理尺寸超过2000的大图，此时需要用Photoshop打开，每帧对应Photoshop每个图层。

### 批量导出图层为文件的 Photoshop 插件

- 下载：[github](https://github.com/antipalindrome/Photoshop-Export-Layers-to-Files-Fast)，[度盘备份](https://pan.baidu.com/s/1qHYwS1XUiLHXazqEaUou4Q?pwd=pa66)
- 安装：把文件夹丢到PS安装目录下的`Presets\Scripts`目录中即可
- 使用：菜单栏，文件 > 脚本 > Export Layers To Files (Fast)

### 再次导回 UleadGIFAnimator

- 可以通过菜单栏 `文件 > 动画向导` 按照指引导入文件；也可以通过 `文件 > 打开图像` 先导入第一帧（这样尺寸会自动设定好），再通过 `文件 > 添加图像` 插入后续帧。

## 网页在线版

以下只收录不需要登录注册的网站

- 不改尺寸的情况下压缩率还不错：[GIF 压缩器 | 在线压缩 GIF 动画](https://www.freeconvert.com/zh/gif-compressor)
- 可以预览，调参数：[图像调整器 | 免费轻松在线调整图像大小](https://imageresizer.com/zh)

# Markdown文档插图规范

遵循这个规范，合理使用相对路径，可以省去图床的麻烦，即便在gittee网页上也能直接显示出插图（github目前暂时还不行）。

相对路径示例：

```
[](./img/xxxxx.jpg)
```

非法路径

```c
错误使用斜杠，导致网页、Obsidian无法显示图片，仅Typora支持，兼容性差
[](.\img\xxxxx.jpg)
```

另外尽量使用常用图片格式，目前探明各平台都兼容可用的图片格式：png、jpg、gif