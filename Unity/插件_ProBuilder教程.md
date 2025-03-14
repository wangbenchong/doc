# 一、前言

嗨，大家好，我是新发。
最近工作需要用到ProBuilder在Unity中搭建场景原型，网上教程也不是很多，有的教程是旧版的。我之前学过Blender，所以上手ProBuilder很快，今天我就写篇稍微详细点的ProBuilder教程吧。

注：本文我使用的Unity版本为2021.2.17f1c1，ProBuilder插件版本为5.0.4

![](https://i-blog.csdnimg.cn/blog_migrate/f329d00c8c9cb4f1cfa1348ee4a739d5.png)

# 二、安装插件

打开PackageManager，搜索ProBuilder，即可安装，目前最新版本是5.0.4，

如果你的工程使用的是URP通用渲染管线或者HDRP高清渲染管线，记得导入对应的Support包，

![](https://i-blog.csdnimg.cn/blog_migrate/e1d6dbf477d37cb5082cabe52c2acd56.png)

安装完毕后，可以在Tools菜单下看到ProBuilder的菜单，如下

![](https://i-blog.csdnimg.cn/blog_migrate/469bcbb23d787c53aa2b15ede8a65cec.png)

我们点击Tools/ProBuilder/ProBuilder Window菜单，即可打开ProBuilder编辑器窗口，如下

![](https://i-blog.csdnimg.cn/blog_migrate/b74ff6496fa7048f88f00ab3bfad9a61.png)

# 三、基础操作

## 切换编辑器模式

我们在ProBuilder编辑器窗口空白处鼠标右键，可以切换窗口模式，

![](https://i-blog.csdnimg.cn/blog_migrate/5d3613a6bc0a564d85ae2c63b6707d0b.png)

### Open as Floating Window

选择Open as Floating Window，会以悬浮窗口的形式显示编辑器，![](https://i-blog.csdnimg.cn/blog_migrate/0e5440472e913e34bd2ac38eeb2acdc6.png)

这种方式一般是我们把Scene窗口以Maximize最大化显示的时候使用，方便操作，![](https://i-blog.csdnimg.cn/blog_migrate/a50cf2cf05042f484c3a225ca1e449bc.png)

如下![](https://i-blog.csdnimg.cn/blog_migrate/2fb041bd00755f406a1cf45290ff5b08.png)

### Open as Dockable Window

选择Open as Dockable Window，我们可以把窗口以标签页的形式嵌入到编辑器界面中，这样编辑器布局会简洁一些，但这种模式下，如果把Scene窗口最大化，就得先把ProBuilder窗口独立移出来才能操作，
![](https://i-blog.csdnimg.cn/blog_migrate/2c5734b91b2f86b90e880ef06711074e.png)

### Use Text Mode

这种模式，菜单以文字形式显示，（个人更喜欢文字显示）
![](https://i-blog.csdnimg.cn/blog_migrate/60dfccbe67e2465e83b8b6c33031ee62.png)

### Use Icon Mode

这种模式，菜单以图标的形式显示，记不住单词的可以使用图标模式，
![](https://i-blog.csdnimg.cn/blog_migrate/8374a838786ddd0798a4f506069f0979.png)

## 创建基础模型

点击New Shape，
![](https://i-blog.csdnimg.cn/blog_migrate/45be5dd0cb5a831a3926b3bfcac6b54b.png)

在Scene窗口中会出现一个Create Shape小窗口，
![](https://i-blog.csdnimg.cn/blog_migrate/864009801b88eaa9e925dec550964beb.png)

有12种基础模型可以选择，
![](https://i-blog.csdnimg.cn/blog_migrate/cc305df9926dbe118b65cf78f053b639.png)

下面我们挨个进行讲解。

### Arch拱门

我们先点选第一个按钮，它是一个拱门模型，把鼠标移动到**Scene**视图的空白处，先水平面拉出一个平面，然后鼠标往上移动即可在竖直方向上拉出拱门模型，最后点击鼠标左键即可完成，

![](https://i-blog.csdnimg.cn/blog_migrate/cb277df0ce7d1e40b5bc018f7bfb3b9e.gif)

我们也可以按住**Shift**键不放，当鼠标在**Scene**窗口中悬停时就可以看到有个浅蓝色的模型出现，我们在合适的位置点击鼠标左键即可创建出模型，如下
![](https://i-blog.csdnimg.cn/blog_migrate/cbda34a1fa6740c725ce85fa37f1faaa.gif)

我们可以选中某个面调整拱门的形状，如下
![](https://i-blog.csdnimg.cn/blog_migrate/35a38e72c2ce419743b51aabdb03c108.gif)


如果想精确设置大小，可以在**Inspector**窗口中的**Pro Builder Shaper**组件下设置**Size**的值，

注：不建议修改**Transform**的**Scale**来调整模型大小
![](https://i-blog.csdnimg.cn/blog_migrate/a32f163b673eb8a285450c1104566799.png)

在**Create Shape**小窗口中，有模型的基础参数，
![](https://i-blog.csdnimg.cn/blog_migrate/3a27999111bd8f14dadbdfa8dea51405.png)


我们可以调节拱门的厚度：**Thickness**参数

![](https://i-blog.csdnimg.cn/blog_migrate/f601656b7ece1fc4d39e066b1a1e32a3.gif)

修改拱门的边数：**Side Count**参数
![](https://i-blog.csdnimg.cn/blog_migrate/193eedf17ada3c596262a97ca80caf8d.gif)

调整拱门的环形角度：**Arch Circumference**参数，

![](https://i-blog.csdnimg.cn/blog_migrate/e126214a919879864ec3c6606d6216c0.gif)

设置拱门的地面是否缝合：**End Caps**参数，
![](https://i-blog.csdnimg.cn/blog_migrate/c4830179a9d390c61bfc434e2e1db9ff.gif)



设置是否进行法线平滑：**Smooth**参数，
![](https://i-blog.csdnimg.cn/blog_migrate/5fb29f3d6903649195e1bc9435d9f6f1.gif)

如果我们已经设置好参数了，就可以按**Esc**键退出创建模式了，
![](https://i-blog.csdnimg.cn/blog_migrate/5966d6e3a4d3e7c5ba3ce09e809e03c9.gif)

接着我们就可以像操作普通物体那样对它进行平移、缩放、旋转等操作了。
如果想要重新设置模型参数，可以点击**Edit Shape**按钮，
![](https://i-blog.csdnimg.cn/blog_migrate/7907a840a768e1f907411617a2ff3021.png)

此时在**Scene**窗口中就会出现模型参数窗口了，我们可以重新对模型参数进行调整，
![](https://i-blog.csdnimg.cn/blog_migrate/db40796a4ed46525017ca70f3aef4f6b.png)

### Cone圆锥体

同理，我们创建**Cone**圆锥体，
![](https://i-blog.csdnimg.cn/blog_migrate/644a08e1763596b109fe174da356f512.gif)

参数很简单，一个**Sides Count**边数，一个**Smooth**法线平滑，
![](https://i-blog.csdnimg.cn/blog_migrate/4b7b3db54cf2912fb7f39930950dd606.png)

### **Cube**立方体

立方体效果如下，参数只有基本的**Size**，
![](https://i-blog.csdnimg.cn/blog_migrate/bdffdb15ea7a0212a86363eb0745e667.gif)

### Cylinder圆柱体

圆柱体效果如下，
![](https://i-blog.csdnimg.cn/blog_migrate/b5ff2680bb3e6c365a5ac2176612fa7e.gif)

参数如下
![](https://i-blog.csdnimg.cn/blog_migrate/afa79063582ceea2902fcb7d844d2f96.png)

其中高度切分是指把圆柱从横截面环切成**N**份，如下
![](https://i-blog.csdnimg.cn/blog_migrate/cc914017081a09c53408f48f216f90d5.gif)

环切会产生新的点线面，我们可以根据需要对点线面进行操作，下文会讲解点线面的操作。

### Door门

门效果如下，
![](https://i-blog.csdnimg.cn/blog_migrate/eb8aeba0a3f22d447013e4a9c6ddfe8a.gif)

参数如下
![](https://i-blog.csdnimg.cn/blog_migrate/73e03bdaa25d48759e04b0e59484e58f.png)

### **Pipe**管道

管道效果如下，
![](https://i-blog.csdnimg.cn/blog_migrate/e3025377499873093f4cefe8be73776d.gif)

参数与圆柱体基本一样
![](https://i-blog.csdnimg.cn/blog_migrate/baced18876103da4a26ff19df2065ea8.png)

### Plane平面

平面是最简单的了，
![](https://i-blog.csdnimg.cn/blog_migrate/04cc36ef5ddd1ed3a410713d6300ac24.gif)

我们可以调整**Height Cuts**和**Width Cuts**来对平面进行细分，如果你想做一个表面起伏的水面，就需要这个表面有足够的顶点数量，然后通关噪声对顶点做偏移，
![](https://i-blog.csdnimg.cn/blog_migrate/8c593e5ba4ae66fe3c39a49ab6c0a354.gif)

### **Prism**棱柱

棱柱效果如下，
![](https://i-blog.csdnimg.cn/blog_migrate/3e43adcae3aabca4db25b93ab4d6cb0f.gif)

只有基础的**Size**参数调整，
![](https://i-blog.csdnimg.cn/blog_migrate/da8e568dddc5a4f3384770bc36573351.png)

我们可以另外通过点线面的操作来对它进行编辑，下文会讲点线面的操作。

### Sphere球体

球体效果如下，
![](https://i-blog.csdnimg.cn/blog_migrate/a2f0de1402d337cbf2b4d9efa7340028.gif)

想要调整球的大小，建议**不要**直接修改**Transform**的**Scale**，而是调整模型的**Size**参数，
![](https://i-blog.csdnimg.cn/blog_migrate/7ae0bc207572bdd984166f590c5452b6.png)

球的参数如下
![](https://i-blog.csdnimg.cn/blog_migrate/b79655da8f1d32d9c37f258096ac44d7.png)

其中表面细分越高，球的表面越精细，但顶点数也越多，建议不要设置得太高
![](https://i-blog.csdnimg.cn/blog_migrate/1c3da816b1bc54646e11f5e3265cecea.gif)

### Sprite平面

**Sprite**平面和**Plane**平面差不多，只不过**Sprite**平面不能进行细分，它只有四个顶点，两个三角面，
![](https://i-blog.csdnimg.cn/blog_migrate/1b29440faded1377334eda154c7cb2f6.gif)

### Stairs楼梯

楼梯效果如下，
![](https://i-blog.csdnimg.cn/blog_migrate/d369a06f19a0c9f4d952f399cafcada2.gif)

我们可以调整楼梯台阶的数量：**Steps Count**参数，
![](https://i-blog.csdnimg.cn/blog_migrate/6a5ddd6066761f739625353e134b7a20.gif)

我们也可以把台阶模式切换为**Height**，然后设置台阶的高度：**Steps Height**参数，它会自动计算台阶的数量，
![](https://i-blog.csdnimg.cn/blog_migrate/2d681380d881c4b9b4effc4063e78543.gif)

我们可以调整**Circumference**参数来调整楼梯的环绕角度，如下
![](https://i-blog.csdnimg.cn/blog_migrate/f2d31fba36535434ebc4a5be4eaef94d.gif)

**Sides**参数可以决定是否要有楼梯侧面，
![](https://i-blog.csdnimg.cn/blog_migrate/df09c4786a541a64f4e9c8fef971130e.gif)

### Turus甜甜圈

甜甜圈效果如下，
![](https://i-blog.csdnimg.cn/blog_migrate/41aa762b75ae5f4d960fd8e04b500464.gif)

调节**Tube Radius**可以调整内圈大小
![](https://i-blog.csdnimg.cn/blog_migrate/50fae8cb9a3f738fedaa0e2d1f4f1905.gif)

**Raws**参数可以控制横向环切的数量
![](https://i-blog.csdnimg.cn/blog_migrate/f3cf6a0c958afdf2d32bd24ffef629d0.gif)

同理，**Columes**参数控制竖向环切数量
![](https://i-blog.csdnimg.cn/blog_migrate/0b474c9902da9f7a4a0931e9e226ad44.gif)

**Hor. Circ.**参数可以调整水平环绕角度
![](https://i-blog.csdnimg.cn/blog_migrate/4fda5b727a0a6f2836fdfe1c19ef077d.gif)

同理，**Vert. Circ.**控制竖直方向的环绕角度
![](https://i-blog.csdnimg.cn/blog_migrate/d811ea4353e0c45ac5f9915e4402b3ae.gif)

## 点线面基础操作

演示完**12**个基础模型的创建效果，可以看到，他们其实都是由**点、线、面**组成，我们可以对这些点线面进行编辑，来改变模型的形状。我们可以在**Scene**窗口的顶部看到四个按钮，分别是**对象按钮、点按钮、线按钮、面按钮**，如下
![](https://i-blog.csdnimg.cn/blog_migrate/d017846dfedf163a859b68dd259f042d.png)

### 操作对象

#### 平移、旋转、缩放

点击对象按钮，进入对象操作模式，我们可以对模型整体进行移动、旋转、缩放等，这些基础操作相信大家都非常熟悉了，
![](https://i-blog.csdnimg.cn/blog_migrate/6a3b2ef8053a11d2e487dac47a78411b.gif)

#### 固定0.25米间隔移动

如果我们想让物体以**0.25米**为间隔进行移动，可以按住**Ctrl键**不放进行移动，这样可以方便对齐物体，如下
![](https://i-blog.csdnimg.cn/blog_migrate/3951600d2ac10c26e2a00882cf200fd8.gif)

需要说一下，在**Unity**中**1**米的长度是多长呢？我们可以靠近模型表面，看到默认纹理是一个一个的正方形，每个正方形的边长就是**1米**，我们可以看到纹理中有个**1METER**的字样，
![](https://i-blog.csdnimg.cn/blog_migrate/6d234356097ec100b61dea4d39637ee0.png)

我们设置**Transform**的**Position**的时候，**XYZ**的单位也是米，
![](https://i-blog.csdnimg.cn/blog_migrate/1c5fb4fba9cce69b5f1574223677566e.png)

当我们创建一个**Cube**时，默认创建的就是一个边长为**1**米的立方体，

![](https://i-blog.csdnimg.cn/blog_migrate/db06ae17b9d8a4a0411fa3c98f91e08f.png)
![](https://i-blog.csdnimg.cn/blog_migrate/017d5a2373968ed359c1d4647b830018.png)

#### 固定15度间隔旋转

同理，按住**Ctrl**键，旋转物体会以**15**度为间隔进行旋转
![](https://i-blog.csdnimg.cn/blog_migrate/cecd0f608f48b5fbdd05108975ce9136.gif)

#### 固定1米缩放

注：一般不建议直接对模型进行缩放，容易造成尺度不统一，引起不必要的麻烦~

按住**Ctrl**不放，缩放时会按**1**米的尺度进行缩放，如下
![](https://i-blog.csdnimg.cn/blog_migrate/c80c2ee771c7f7139ea0c1ccd1c774ce.gif)

#### 吸附控制点

我们移动物体时，需要把鼠标移动到物体的中心轴上才能控制物体，默认情况下，物体的中心轴在角落里，如下
![](https://i-blog.csdnimg.cn/blog_migrate/3c37ba0bddb64fd0b394c20c052ecb45.png)

我们可以按住**V**键不放，这样就会根据鼠标的位置吸附到一个控制点进行操作了，如下
![](https://i-blog.csdnimg.cn/blog_migrate/15f24341f9aa957d6102c566f8496065.gif)

### 操作点

#### 移动点

进入点编辑模式，
![](https://i-blog.csdnimg.cn/blog_migrate/1c2487eeb7947a5657473117d85b1953.png)

我们可以选中模型的顶点，进行移动
![](https://i-blog.csdnimg.cn/blog_migrate/670da980f9af9acf54c78ec43a4dd202.gif)

#### 框选多个点

如果想选中多个点，可以先选中模型整体，然后框选顶点进行操作，如下，不过像下面这样的操作其实就等效于操作线了，我们可以直接使用线操作模式，
![](https://i-blog.csdnimg.cn/blog_migrate/d2daf52149ff60ff04a5950fa5265f21.gif)

#### Shift加选点

我们也可以选中某个点之后，按住**Shift**不放，然后点击新的顶点来进行加选，如下
![](https://i-blog.csdnimg.cn/blog_migrate/5c310445e8c66ee7222fb00bdb431f5a.gif)

#### 合并顶点：Collapse Vertices

我们可以选中多个点，然后点击**Collapse Vertices**进行顶点合并，如下
![](https://i-blog.csdnimg.cn/blog_migrate/11f24113c8dc0ff3b5505270be2c3b4c.gif)

### 操作线

进入线操作模式
![](https://i-blog.csdnimg.cn/blog_migrate/3da9256d8822c059e00a3024101f2adf.png)

#### 移动、旋转、缩放

可以对线进行移动、旋转、缩放等操作，如下
![](https://i-blog.csdnimg.cn/blog_migrate/0393e5f7f75d2f3aa1281a93a053862f.gif)

#### 多选（可款选，可Shift加选）

同样，我们可以框选 或 按住**Shift**键加选来选中多条线进行操作，
![](https://i-blog.csdnimg.cn/blog_migrate/74899808cc1b642d993be82facc89afc.gif)

#### 添加倒角：Bevel

我们可以选中某个边，然后点击**Bevel**添加倒角，如下
![](https://i-blog.csdnimg.cn/blog_migrate/f4208302592093e1c59beef2d5e3676d.gif)

我们可以点击**Bevel**按钮旁边的**+号**按钮进行设置，可以设置倒角的距离，

吐槽：不能像**Blender**那样设置倒角的进度，只能手工倒角，有点费手
![](https://i-blog.csdnimg.cn/blog_migrate/69dc8b02a66353379b34354200b399b7.png)

![](https://i-blog.csdnimg.cn/blog_migrate/9076d2524ef8b06a0408a8ab59a55af4.png)



#### 细分线：Subdivide Edges

我们可以选中线，然后点击**Subdivide Edges**进行细分，

注：为了方便看清线，我把**Scene**视图的**Shading Mode**设置为**WireFrame**模式（网格模式）

![](https://i-blog.csdnimg.cn/blog_migrate/b7a94c78801e9c08a7b908d0a4de5cbf.gif)

#### 环切：Insert Edge Loop

选中某条线，然后点击**Insert Edge Loop**，可以对这条线所在的垂直面进行环切（相当于新增一圈的线，可以对新增的这圈线进行编辑），如下
![](https://i-blog.csdnimg.cn/blog_migrate/b7a554db0ae479019df59282cae5f185.gif)

#### 刀切：Cut Tool

我们可以点击**Cut Tool**按钮，像刀子一样在面上进行切割（切割会新增线，形成新的三角面），如下
![](https://i-blog.csdnimg.cn/blog_migrate/e4a61dc782dbdc289667b27a14d3b4b5.gif)

#### 填充线所在的面：Fill Hole

假设现在有一个面是有一个洞，我们想把它不上，可以选中这个洞的某条边，然后点击**Fill Hole**即可填充这个面，
![](https://i-blog.csdnimg.cn/blog_migrate/e96687c0d6e3366877fe4c85e65814bb.gif)

#### 挤出线：Extrude Edges

我们可以选中某条线，然后点击**Extrude Edges**，挤出这条线，如下
![](https://i-blog.csdnimg.cn/blog_migrate/a997177ca741fc2e59523d03e2a1dc46.gif)

我们也可以选中线然后按住**Shift**不放，移动鼠标直接挤出线，如下
![](https://i-blog.csdnimg.cn/blog_migrate/ef2e7e35fb87ecf74868f6678cdeffea.gif)

#### 连接线：Connect Edges

选中两条线，然后点击**Connect Edges**按钮，即可连接这两条线的中心位置，可以调整新增出来的连接线，如下
![](https://i-blog.csdnimg.cn/blog_migrate/d4274e8a70754f60d92b5f48bfe867ce.gif)

#### 环选相连的线：Select Edge Loop

选中某条线，然后点击**Select Edge Loop**即可环选相连的线，如下
![](https://i-blog.csdnimg.cn/blog_migrate/61c04f8923890cd2fc776b57be3e007e.gif)

#### 环选不相连的线：Select Edge Ring

选中某条线，点击**Select Edge Ring**即可环选不相连的线，如下
![](https://i-blog.csdnimg.cn/blog_migrate/0ae0959f61f1ced4e2f3fa24f903b5e6.gif)

### 操作面

进入面操作模式，
![](https://i-blog.csdnimg.cn/blog_migrate/a9a3daf6dcd4a3ba2d3f92b5e7e8a1b9.png)

#### 移动、旋转、缩放

选中面之后，可进行移动、旋转、缩放，如下

![](https://i-blog.csdnimg.cn/blog_migrate/0577cdf7dff609193f56b346c4380ed3.gif)

#### 细分三角面：Triangulate Faces

我们选中面，它并不是一个三角面，我们可以点击**Triangulate Faces**把它细分为三角面，如下
![](https://i-blog.csdnimg.cn/blog_migrate/c967e5b905051a2b1ea3b2aa9fb97eeb.gif)

#### 倒角：Bevel

与线倒角一样，面也可以进行倒角，选中面，然后点击**Bevel**按钮即可进行面倒角，如下
![](https://i-blog.csdnimg.cn/blog_migrate/9ede7197106f8486c348a629a9d33a47.gif)

#### 挤出面：Extrude Faces

选中面，点击**Extrude Faces**即可基础面，如下
![](https://i-blog.csdnimg.cn/blog_migrate/487d482134db96187fdc6353cfa3bc22.gif)

我们也可以选中面然后按住**Shift**不放直接基础面，如下
![](https://i-blog.csdnimg.cn/blog_migrate/f8701f245e69c3714d6fa24f15776635.gif)

我们可以点击**Extrude Faces**按钮旁边的**+号**，设置挤出的参数
![](https://i-blog.csdnimg.cn/blog_migrate/78dc12675ff8ab7ead5e519549ddad76.png)

**individural Faces**表示按每个面独立挤出；
**Vertex Normal**表示按顶点法向方向挤出；
**Face Normal**表示按面法线方向挤出。
![](https://i-blog.csdnimg.cn/blog_migrate/fb1e716056d24914deb333e434981006.png)

我给大家演示一下**Individural Faces**挤出和**Face Normal**挤出的区别。
先看一下**Individural Faces**挤出
![](https://i-blog.csdnimg.cn/blog_migrate/3cb1c8658c39fcd207acd2c8a39f7bed.gif)

接着看**Face Normal**挤出，
![](https://i-blog.csdnimg.cn/blog_migrate/9e1acdfae06c02d571a509303c0bd21b.gif)

#### 复制面：Duplicate Faces

选中面，然后点击**Duplicate Faces**即可复制面，如下
![](https://i-blog.csdnimg.cn/blog_migrate/244cf2cc1e988702a3c6d8f6fb05b937.gif)

#### 剪切面：Detach Faces

选中面，然后点击**Detch Faces**即可把面剪切出来，如下
![](https://i-blog.csdnimg.cn/blog_migrate/1080ef5c383339592af0f187cf40e6db.gif)

#### 删除面：Delete Face

选中面，然后点击**Delete Face**（或者按**Backspace**回退键）即可删除面，如下
![](https://i-blog.csdnimg.cn/blog_migrate/5cfe9b17977b052d7eb454b6f68780a9.gif)

#### 缩放挤出挤出面

选中面，缩放时按住**Shift**键不放，即可在平面内挤出面，如下
![](https://i-blog.csdnimg.cn/blog_migrate/d8324be16586e1d199333243b47852d6.gif)

#### 环选多个面：Select Face Loop / Select Face Ring

选中面，然后点击**Select Face Loop**即可在数值方向上环选多个平面，点击**Select Face Ring**即可在水平方向环选多个平面，如下
![](https://i-blog.csdnimg.cn/blog_migrate/1b1e14556453bfcf95ec2f4f9e0300c0.gif)
![](https://i-blog.csdnimg.cn/blog_migrate/a03a4de3677a43ed162d5a7890af43e6.gif)



## 上色

我们创建的模型默认是白色的，
![](https://i-blog.csdnimg.cn/blog_migrate/082513a9606f302758e5eb79ab52db3b.png)


现在我们给它们上色。

### 设置顶点颜色

#### 模型所有顶点上色

切换到对象模式，
![](https://i-blog.csdnimg.cn/blog_migrate/6294b4c8da1e7409af8a4d175aec5b22.png)

选中物体，然后点击**Vertex Colors**按钮，
![](https://i-blog.csdnimg.cn/blog_migrate/8ad49771c4e9052d96f31e9bad2436e9.png)

此时会弹出调色盘，
![](https://i-blog.csdnimg.cn/blog_migrate/9d9db8625abb3710adbe5241b5d123e3.png)

选择自己喜欢的颜色，点击**Apply**按钮，即可给物体所有顶点上色，如下

![](https://i-blog.csdnimg.cn/blog_migrate/f39c16a3fdb9c85da480412534279f48.png)

#### 单独顶点设置颜色

我们可以切换到顶点模式，
![](https://i-blog.csdnimg.cn/blog_migrate/2dea85520b8661f547e8176456ff5223.png)

选中要上色的顶点，然后选择喜欢的颜色，点击**Apply**按钮即可，
![](https://i-blog.csdnimg.cn/blog_migrate/d73c27efefd37c170bb5b7ecde52c26f.png)

### 设置材质球

#### 创建材质球

**ProBuilder**创建的模型默认使用**ProBuilderDefault**材质球，
![](https://i-blog.csdnimg.cn/blog_migrate/d64318f5ae013f923188035eb98eb2f9.png)

我们可以自己新建材质球，替换为我们自己的材质球，如下
![](https://i-blog.csdnimg.cn/blog_migrate/462c8fdd5de0787cda35c3366735f222.png)
![](https://i-blog.csdnimg.cn/blog_migrate/d75734bd19fe615a2ea7ec6790c59dae.png)



#### 添加材质球到工具中

建议把材质球添加到**ProBuilder**工具中，点击**Material Editor**，
![](https://i-blog.csdnimg.cn/blog_migrate/9e0940d90143123c23e06857e89efb51.png)

把材质球添加到列表中，
![](https://i-blog.csdnimg.cn/blog_migrate/13fd7c23a5e7711fa83384c5a70049a3.png)

这样我们可以通过快捷键快速设置物体材质，比如按**Alt + 2**即可给物体设置**Green**材质球，
![](https://i-blog.csdnimg.cn/blog_migrate/e133255ec67cce98e1102e0281eda7c8.gif)

#### 碰撞体材质

场景有时候需要一些空气墙或者碰撞触发器，我们可以点击**Set Trigger**将模型设置为触发器材质，点击**Set Collider**将模型设置为碰撞体材质，
![](https://i-blog.csdnimg.cn/blog_migrate/643e1796ce1429a9f449d582c187f821.png)

如下
![](https://i-blog.csdnimg.cn/blog_migrate/eae30c80fa1df6abb81c20a0492e89fc.gif)

## 其他操作

### 设置默认材质

上面我们提到，**ProBuilder**会给模型自动设置一个**ProBuilderDefault**材质球，我们在设置中修改**ProBuilder**的默认材质。
点击菜单**Edit/Preferences...**，然后点击**ProBuilder**按钮，找到**Mesh Settings**下的**Material**参数，设置你自己的材质球即可，
![](https://i-blog.csdnimg.cn/blog_migrate/0bb60b9d42ae2ec6ac9b65181cabcb8a.png)

### 修改Pivot中心点

创建模型是，默认**Pivot**是在物体的角落，如下
![](https://i-blog.csdnimg.cn/blog_migrate/96cbb69e953781d319f8bcfad6784976.png)

我们可以点击**Center Pivot**按钮，修改中心到物体的几何中心位置，如下
![](https://i-blog.csdnimg.cn/blog_migrate/728e772ab3f98af6e45fd4ba30da6623.png)![](https://i-blog.csdnimg.cn/blog_migrate/d03f97126dc2968f0e85b87a29a0ec60.png)

我们也可以在顶点模式下，选中某个点，然后点击**Set Pivot**按钮将选中的顶点设置为**Pivot**，
![](https://i-blog.csdnimg.cn/blog_migrate/6a690dc895b5ee63611fa320f1de6c84.png)

我们还可以在边模式下，选中某条边，点击**Set Pivot**按钮，把边的中心点设置为**Pivot**
![](https://i-blog.csdnimg.cn/blog_migrate/d9ce1a916102a082fec2777536013c36.png)

一般我们做门的时候就会用到这个技巧，这样物体旋转就是绕着这个边旋转，
![](https://i-blog.csdnimg.cn/blog_migrate/6ea3e56878720deb12a574e415f98e63.gif)

### 镜像对称物体：Mirror Objects

我们创建好模型后，想要让物体沿着某个轴镜像对称，可以点击**Mirror Objects**按钮，
![](https://i-blog.csdnimg.cn/blog_migrate/f551a13943613f62ce41a5d819df114b.png)

效果如下
![](https://i-blog.csdnimg.cn/blog_migrate/fcb42afd242f4957e8d128a78e09949e.gif)

我们可以设置对称的轴，
![](https://i-blog.csdnimg.cn/blog_migrate/8da7556d23152f35ea680226f7280272.png)

比如改为沿着**Y轴**镜像对称，
![](https://i-blog.csdnimg.cn/blog_migrate/a9c44b7cdbf7f4234bd07d6337fd7a90.png)

效果如下
![](https://i-blog.csdnimg.cn/blog_migrate/8ba6c9161ac97ca4c404334a90eb9d38.gif)

### 法线平滑

假设我现在有这么一个模型，我只想给某一个侧面做法线平滑，如下
![](https://i-blog.csdnimg.cn/blog_migrate/b0f9e26e00b3d5181398198b0b2d5801.png)

我们可以选中这个侧面的几个面，
![](https://i-blog.csdnimg.cn/blog_migrate/44aa4ef28b10c216a103e0b02320a082.png)

然后点击**Smoothing**按钮，然后设置一个平滑分组即可
![](https://i-blog.csdnimg.cn/blog_migrate/f2a84757af6356784733103ae8ccb7a5.png)

可以看到，我们这个侧面看起来变得平滑了

注：这里只是通过法线来影响光的反射方向从而达到平滑的错觉，实际上模型的边还是硬边
![](https://i-blog.csdnimg.cn/blog_migrate/22aae2d588a74d891810bf0e4228e7f4.png)



### 导出模型

当我们制作好了模型，比如我制作了一个泳池模型，现在想把它导入到其他三维建模软件中修改，
![](https://i-blog.csdnimg.cn/blog_migrate/7ab9f36d4c081a8b790164023b455f7c.png)

我们可以点击**Export**按钮
![](https://i-blog.csdnimg.cn/blog_migrate/2c45d25c3f9fce175be773007d694a4d.png)

将模型导出
![](https://i-blog.csdnimg.cn/blog_migrate/f080a8baa0e22ebf3f85941acc3e738a.png)

导出格式我们可以点击**Export**按钮旁边的**+号**进行设置，
![](https://i-blog.csdnimg.cn/blog_migrate/201eb4aa9ce3c05387353322be722cf5.png)

默认是**obj**格式
![](https://i-blog.csdnimg.cn/blog_migrate/ef4233fdc1025e7cb00e54e9d07fbfab.png)

我们可以使用**Blender**打开导出的**obj**模型文件，
![](https://i-blog.csdnimg.cn/blog_migrate/9a8da5d0dfd6ee2ae20dc24f60a1400a.png)

# 四、完毕

好了，我是林新发，https://blog.csdn.net/linxinfa
一个在小公司默默奋斗的Unity开发者，希望可以帮助更多想学Unity的人，共勉~