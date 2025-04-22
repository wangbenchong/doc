# 常用操作

- 查看所有快捷键：Edit > Shotcuts... > ShaderGraph分页

- 开关 BlackBoard 面板：Shift+1

- 开关 Inspector 面板：Shift+2

- 开关 Main Preview 浮窗：Shift+3

- 切换 Color Mode（比较喜欢 Heatmap，可以查看性能消耗，颜色越亮消耗越大）：Shift+4

- 建组：框选节点，Ctrl+G

- 取消建组：选中组，Ctrl+U

- 切换选中节点折叠（仅部分节点支持折叠）：Ctrl+P

- 切换预览折叠（若没有选中节点则切换所有节点）：Ctrl+T

- 添加 Redirect 折点（需选中边）：Ctrl+R

- 保存：Ctrl+S

- 另存为：Ctrl+Shift+S

- 建子图：框选节点，右键菜单

- 子图改路径：BlackBoard标题下方小字是可以双击改名的（对应序列化变量名：Path）

- 选中当前Graph文件：左上角箭头展开 > Show In Project

  

# UI效果注意事项

## 定义MainTex变量

这种做法便于让shader直接把Image的source图当作_MainTex。但会在开启ShaderGraph窗口的情况下报如下错误：

```
Two properties with the same reference name (_MainTex) produce different HLSL properties
UnityEditor.EditorApplication:Internal_CallUpdateFunctions ()
```

忽视这个报错即可。

# 彩色扫光UI（基于运算）

如图（Trangle Wave换成Sin也是可以的，为了性能和线性表现所以选了前者）：

 ![](./img/彩色扫光UI基于运算.jpg)



# 彩色扫光UI（基于贴图）

如图：

![](./img/彩色扫光UI基于贴图.jpg)

# 雪花

格子:

 ![](./img/格子.jpg)

圆：

 ![](./img/圆1.jpg)

极坐标拆分两通道：

 ![](./img/极坐标拆分两通道.jpg)

最终通过极坐标完成雪花：

 ![](./img/雪花.jpg)

额外扩展——漫画线：

 ![](./img/极坐标漫画线.jpg)
