# 制作剪影外发光 UI 图标

这种风格简约而高级，既能突出自身又不抢占画面。大致效果：

 ![](./img/ps_01.jpg)

制作思路：

- 复制到新图层 1 用钢笔抠图，背景图层 0 用纯黑填充
- 双击图层 1，使用混合效果：外发光
  - 混合选项：混合模式变亮，填充不透明度设为 0，挖空选无
  - 外发光选项：混合模式变亮，不透明度 64，杂色 0，方法柔和，扩展小一点（0 到 1 自己斟酌），大小 46 像素
- 合并 0、1 两个图层到图层 0，提高一点亮度，对比度也可微调，选择模式 Ctrl+A，Ctrl+C 复制
- 新建图层 1，填充纯白色，并添加蒙版，通道中点亮并选中这个蒙版，Ctrl+V 粘贴
- 隐藏图层 0，将图层 1 导出为 png 图片即可



# 【插件】批量导出图层为文件

- 下载：[github](https://github.com/antipalindrome/Photoshop-Export-Layers-to-Files-Fast)，[度盘备份](https://pan.baidu.com/s/1qHYwS1XUiLHXazqEaUou4Q?pwd=pa66)
- 安装：把文件夹丢到 PS 安装目录下的 `Presets\Scripts` 目录中即可
- 使用：菜单栏，文件 > 脚本 > Export Layers To Files (Fast)

# 【插件】支持 DDS 格式图片

插件名：Intel® Texture Works Plugin for Photoshop

可通过 [github](https://github.com/GameTechDev/Intel-Texture-Works-Plugin) 或 [官网](https://gametechdev.github.io/Intel-Texture-Works-Plugin/) 下载

## 入门 （安装）

1. 关闭 Photoshop
2. 下载 IntelTextureWorks_1.0.4.zip 文件并在本地计算机上展开它
3. 从以下任一解压缩文件夹中复制所需的插件
   - .../IntelTextureWorks_1.0.4\Plugins\x64\IntelTextureWorks.8bi
   - .../IntelTextureWorks_1.0.4\Plugins\Win32\IntelTextureWorks.8bi
4. 将插件粘贴到相应的 Photoshop Plugin 文件夹中
   - D:\Program Files\Adobe Photoshop CC 2014\Required\Plug-Ins\File Formats
   - D:\Program Files\Adobe\Adobe Photoshop CS6 (64 Bit)\Plug-ins\File Formats
5. 从以下位置复制立方体贴图脚本：
   - .../IntelTextureWorks_1.0.4\PhotoshopScripts\IntelTextureWorks-ConvertCubeMap.jsx
   - .../IntelTextureWorks_1.0.4\PhotoshopScripts\IntelTextureWorks-CubeMapGaussianBlur.jsx
6. 将立方体贴图脚本粘贴到：
   - D:\Program Files\Adobe Photoshop CC 2014\Presets\Scripts

## 通过插件保存文件

1. 文件 > 另存为 Save as
2. 选择 Intel **Texture Works >（®\*.DDS）**

## 加载通过插件保存的文件

多个常驻 DDS 插件可能会导致加载时出现纹理显示错误。为避免这种情况，请使用以下过程重新加载使用适用于 Photoshop 的 Intel® Texture Works 插件保存的纹理

1. 文件 > 打开方式
2. 选择 **Intel® Texture Works （\*.DDS）** 作为类型
3. 选择所需的 mipmap 加载选项（如果适用）
4. 选择所需的颜色配置文件加载选项



# 批量编辑图片

以批量另存dds为png图片为例：

1. 录制动作，要录到 打开、存储、关闭 三部分。
2. 菜单栏：文件 > 自动 > 批处理
   1. 选择刚录制好的动作
   2. 设置源：文件夹
   3. 勾选：覆盖动作中的 “打开” 命令
   4. 错误：将错误记录到文件
   5. 目标：文件夹
   6. 勾选：覆盖动作中的 “存储为” 命令
   7. 文件命名：文档名称、扩展名（小写）
3. 点击 “确定” 执行批处理。



# 常用动作

## 磨皮

- [磨皮.atn](./磨皮.atn)
