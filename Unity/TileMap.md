# 第三方工具——Tiled

官网：[Tiled | Flexible level editor](https://www.mapeditor.org/)

Github:  https://github.com/mapeditor/tiled

用法：独立形成一个Tiled工程，分层管理，导出文本数据，整理数据为二进制文件（可做独立exe工具），unity导入二进制数据（相关编辑器，按位存储，bundle加载），ugui复刻平铺地图（奇数行和偶数行横向坐标错位逻辑，滑动时循环利用地块减少物体数量）

## 二进制文件存储示例：

在游戏关卡编辑中，有一个200*200的格子地图（长宽是可变的），每个格子的数据只可能是0，1，2，3这四种，把这个格子地图的数据尽量节省的保存为二进制文件，请给出具体方案。

###  保存地图尺寸

在保存地图数据之前，我们首先需要保存地图的宽度和高度。这样，在读取数据时，我们可以知道要读取多少行和列的数据。

### 计算每行所需的字节数

由于每个格子需要2位（bit）来表示，我们可以计算出每行需要多少个字节来存储。这可以通过将每行的格子数（即地图的宽度）乘以2，然后除以8（因为1字节=8位）来得到。但是，由于我们不能有“部分字节”，所以我们需要对结果进行向上取整，以确保有足够的字节来存储每行的所有数据。

### 保存地图数据

现在我们可以遍历地图的每一行和每一列，将每个格子的值转换为2位，并存储到相应的字节中。这通常涉及到位操作，如左移、右移、按位与和按位或。

### 写入文件

使用`BinaryWriter`，我们可以先将地图的宽度和高度写入文件，然后写入地图数据。

### 读取文件

读取文件时，我们首先需要读取地图的宽度和高度，然后根据这些信息来读取和解析地图数据。

### 具体实现

以下是一个Unity C#脚本的示例，它演示了如何将可变尺寸的格子地图数据保存为二进制文件，并从二进制文件中读取数据：

```csharp
using System;
using System.IO;
using UnityEngine;
 
public class VariableMapSaver : MonoBehaviour
{
    // 假设这是你的可变尺寸格子地图数据
    private int[,] mapData;
    private int mapWidth;
    private int mapHeight;
 
    // 初始化地图数据（这里只是示例，你可以根据实际情况来填充数据）
    void Start()
    {
        // 假设我们有一个150x250的地图作为示例
        mapWidth = 150;
        mapHeight = 250;
        mapData = new int[mapHeight, mapWidth];
 
        RandomizeMapData();
        SaveMapToBinaryFile("VariableMapData.bin");
 
        // 读取并验证保存的数据
        int[,] loadedMapData = LoadMapFromBinaryFile("VariableMapData.bin");
        Debug.Log("Loaded map data: First value is " + loadedMapData[0, 0] + ", last value is " + loadedMapData[mapHeight - 1, mapWidth - 1]);
    }
 
    // 随机填充地图数据作为示例
    private void RandomizeMapData()
    {
        System.Random random = new System.Random();
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                mapData[y, x] = random.Next(0, 4); // 随机生成0, 1, 2, 或3
            }
        }
    }
 
    // 将地图数据保存为二进制文件
    private void SaveMapToBinaryFile(string filePath)
    {
        byte[] fileData = new byte[(mapWidth * 2 + 7) / 8 * mapHeight + sizeof(int) * 2]; // 每行需要的字节数加上存储宽高的4个字节
 
        int byteIndex = 0;
 
        // 先写入宽和高（使用4个字节的整数来表示）
        byte[] widthBytes = BitConverter.GetBytes(mapWidth);
        byte[] heightBytes = BitConverter.GetBytes(mapHeight);
        Array.Copy(widthBytes, 0, fileData, byteIndex, widthBytes.Length);
        byteIndex += widthBytes.Length;
        Array.Copy(heightBytes, 0, fileData, byteIndex, heightBytes.Length);
        byteIndex += heightBytes.Length;
 
        // 写入地图数据
        for (int y = 0; y < mapHeight; y++)
        {
            int bitIndex = 0;
            for (int x = 0; x < mapWidth; x++)
            {
                int cellValue = mapData[y, x];
                int bytePosition = byteIndex + (bitIndex / 8);
                int bitOffset = 6 - (bitIndex % 8); // 从高位到低位填充
 
                // 设置对应的2位
                fileData[bytePosition] = (byte)((fileData[bytePosition] & ~(3 << bitOffset)) | ((cellValue & 3) << bitOffset));
 
                bitIndex += 2;
            }
            byteIndex += (mapWidth * 2 + 7) / 8; // 每行需要的字节数，向上取整
        }
 
        // 写入二进制文件
        using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
        {
            writer.Write(fileData);
        }
 
        Debug.Log("Map data saved to " + filePath);
    }
 
    // 从二进制文件中读取地图数据
    private int[,] LoadMapFromBinaryFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
 
        // 读取宽和高
        int mapWidthRead = BitConverter.ToInt32(fileData, 0);
        int mapHeightRead = BitConverter.ToInt32(fileData, 4);
 
        int[,] loadedMapData = new int[mapHeightRead, mapWidthRead];
 
        int byteIndex = 8; // 跳过宽和高的4个字节 * 2
 
        // 读取地图数据
        for (int y = 0; y < mapHeightRead; y++)
        {
            int bitIndex = 0;
            for (int x = 0; x < mapWidthRead; x++)
            {
                int bytePosition = byteIndex + (bitIndex / 8);
                int bitOffset = 6 - (bitIndex % 8); // 从高位到低位读取
 
                // 获取对应的2位
                int cellValue = (fileData[bytePosition] >> bitOffset) & 3;
 
                loadedMapData[y, x] = cellValue;
 
                bitIndex += 2;
            }
            byteIndex += (mapWidthRead * 2 + 7) / 8; // 每行需要的字节数，向上取整
        }
 
        return loadedMapData;
    }
}
```

### 注意事项

- 在这个示例中，我们首先保存了地图的宽度和高度（各占4个字节），然后保存了地图数据。
- 我们使用了`BitConverter.GetBytes`和`BitConverter.ToInt32`来将整数转换为字节数组和从字节数组转换回整数。这确保了在不同平台上整数的大小和字节序（endianness）是一致的。
- 在读取数据时，我们跳过了存储宽度和高度的字节，然后按照与保存时相同的逻辑来解析地图数据。
- 请注意，这个示例假设地图的宽度和高度不会超过`int`类型能够表示的最大值，并且文件大小不会超过`File.ReadAllBytes`方法能够处理的限制。在实际应用中，你可能需要添加额外的错误处理和验证逻辑来处理这些情况。



# 如何使用Unity自带的TileMap做出六边形地图

随着RogueLike游戏的火爆，基于像素和砖块(Tile)的游戏类型也再次兴起，而Unity也推出了他们官方的Tile工具——TileMap，但是TileMap默认的Tile是方形的，如果有些开发者(比如我)不想做类似于《节奏地牢》的方形砖块，而想做类似于《文明》或是《为了国王》的六边形地块，用Unity自带的TileMap该如何实现呢？



而Unity虽然足够强大，为我们准备好了六边形Tile的接口，但依然有很多需要注意的地方，想要将默认为方形的TileMap转化为六边形的TileMap，在目前最新的Unity版本(2018.3Beta)下，我们往往需要做以下修改：

1. 将Grid中的Tile类型修改
2. 根据需要修改Grid中Tile的大小
3. 修改TileMap锚点的偏移量

## 更改Grid中的Tile类型

可能很多用过TileMap的同学都知道，当我们在一个场景中create一个TileMap节点出来的时候，Unity会自动帮我们生成一个Grid父节点和一个Tilemap子节点。

```c#
Grid
    Tilemap
```

我个人的理解是外层的Grid主要是用于设置Tilemap的整个网格的相关属性，而内层的Tilemap主要是设置Tilemap在渲染时的相关属性，是和每个tile块息息相关的设置，而我们现在要调节的是网格单元的形状，那自然是要修改Grid的属性。而修改的内容也很简单，将Cell Layout这个下拉菜单改为Hexagon(六边形)就可以了。

```c#
Cell Layout: Hexagon
```



在做完这一步修改后，你会发现Scene视图下的Tilemap网格已经变成了六边形的。



## 更改Grid中Tile的大小

但是如果你觉得这样就大功告成了，那你就错了，你会发现，把美工同学准备的正六边形的图片放进去之后，图片明显比网格单元在纵向上要长！这是为什么呢，原因是正六边形的格子长宽比不是像矩形格子那样显而易见，比如正方形就是1:1，而我们修改了Cell Layout之后，Unity并不会自动帮我们修改这个长宽比，所以我们需要简单的计算一下此时我们需要的长宽比是多少，比如正六边形如下图所示

```c#
 /\
|  |  //高：4
 \/
//宽：2倍根号3
```



所以在经过简单的四舍五入之后，我们把Grid中Tile的大小调成1:1.15就可以得到正六边形的格子了。

```c#
Grid（Component）
	Cell size:(1, 1.15, 0)
	Cell Layout: Hexagon
	Cell Swizzle: XYZ
```



## 修改TileMap锚点的偏移量

实际上，这时候你如果去拿对应位置的单元格的位置，你会发现，拿到的数据不对，这是其实是因为Unity的Tilemap在转成六边形后因为未知原因需要将Tilemap中的Tile Anchor即锚点改成0，但是并不晓得什么原理，改成这样就对了

```c#
Tilemap（Component）
	Tile Anchor:(0,0,0)
```



## 六边形模式下，每个砖块与相邻砖块的关系

在矩形模式下，想要确定一个砖块的相邻砖块是非常简单的，比如要找出与砖块(1, 1)相邻的8个砖块，我相信没有人不会，那就是(0, 0)、(0, 1)、(0, 2)、(1, 0)、(1, 2)、(2, 0)、(2, 1)、(2, 2)。

```c#
(2,0) (2,1) (2,2)
(1,0) (1,1) (1,2)
(0,0) (1,0) (2,0)
```



而如果说是六边形情况下的相邻砖块，因为底层也是用的矩阵存储的，但是相邻砖块却只有6个了，这让初次接触的读者可能有点摸不着头脑，所以这6个应该是哪6个呢，让我们来试一试不就好了！

让我们在基础的生成砖块的代码的基础上，在Update函数中加入如下代码:

```c#
if(Input.GetMouseButtonUp (0))
{
    Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Debug.Log(tilemap.WorldToCell(mousePositionInWorld));
}
```

这样，如果读者已经按笔者之前说的三点调节好了Tilemap的属性的话，在点击界面的时候，在Unity的Console窗口中就会打印出我们当前点击的砖块在Tilemap中的坐标了，而从(1, 1)位置的格子左边那个格开始，顺时针点一圈，得到的打印结果是

```c#
(0,1,0)
(1,2,0)
(2,2,0)
(2,1,0)
(2,0,0)
(1,0,0)
```



让我们把它转化成矩阵的布局就是

```c#
_ 2 3		  2 3
1 _ 4		1	  4
_ 6 5		  6 5
```



所以我们只要把这6个偏移量设置为方向偏移，就可以在矩阵数据中正确取到六边形模式下的相邻格啦！！！然后很多小朋友就这么自信的去做了，结果发现得到的效果并不对，比如在实现用键盘控制角色的行走时，经常跳格！(比如笔者我)实际上是因为，这种六边形模式下的相邻格判断，实际上是分**奇偶行**的，而上面的这六个偏移，只适合y的坐标是奇数的情况下，比如(1, 1)，而偶数的情况下，用相同的方法我们可以得到如下偏移量。



```c#
2 3 _		  2 3
1 _ 4		1	  4
6 5 _		  6 5
```

# RuleTile

Ruletile是Tilemap的拓展插件，一样需要通过package manager来下载

[2D游戏神器-RuleTile - 知乎](https://zhuanlan.zhihu.com/p/358610244)

[Unity记录：按照Rule Tile规则动态生成地图（从脚本中使用RuleTile）_tilemap绘制动态物体-CSDN博客](https://blog.csdn.net/mkr67n/article/details/108340749)
