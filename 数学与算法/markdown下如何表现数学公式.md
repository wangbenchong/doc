## Latex工具网址

[Online Visual Math Editor for Latex and MathML (math-editor.online)](https://math-editor.online/)



## 矩阵

$$
矩阵：
\begin{bmatrix}
	T_x & T_y & T_z \\
	B_x & B_y & B_z \\
	N_x & N_y & N_z
\end{bmatrix}
行列式：
\begin{array}{|}
	1 & 2 & 3 \\
	4 & 5 & 6 \\
	7 & 8 & 9
\end{array}
另一种：
  
\left | \begin{array}{ccc}  
1 & 6 & 9 \\  
7 & 90 & f(x) \\  
9 & \psi(x) & g(x)  
\end{array} \right |  

向量：
\begin{pmatrix}  
a_{11} & a_{12} \\  
a_{21} & a_{22}  
\end{pmatrix}
$$

### 向量点乘

$$
\begin{align*}  
\vec{a} &= (a_1, a_2, \ldots, a_n) \\  
\vec{b} &= (b_1, b_2, \ldots, b_n) \\  
\vec{a} \cdot \vec{b} &= a_1 \times b_1 + a_2 \times b_2 + \ldots + a_n \times b_n \\
\mathbf{a} \cdot \mathbf{b} &= \left| \mathbf{a} \right| \left| \mathbf{b} \right| \cos \theta
\end{align*}
$$



## 除法公式

$$
pixel= \frac{normal+1}{2}
$$

## 乘法公式

$$
normal = pixel\times 2-1
$$

## 开方公式

$$
x=\frac{-b\pm\sqrt{b^{2}-4ac}}{2a}
开n次方:\sqrt[n]{3}
$$

## SVG绘图

官网：[入门 - SVG：可缩放矢量图形 | MDN (mozilla.org)](https://developer.mozilla.org/zh-CN/docs/Web/SVG/Tutorial/Getting_Started)

Html的 svg 标签能够真正做到自由作图：

<div style="display: flex; justify-content: center; align-items: center; ">
<svg width="240" height="170">
<title>SVG Sample</title>
<desc>This is a sample to use SVG in markdown on the website cnblogs.</desc>
<circle cx="70" cy="95" r="50" style="stroke: grey; fill: none;"/>
<circle cx="170" cy="95" r="50" style="stroke: grey; fill: grey;"/>
</svg></div>





<div style="display: flex; justify-content: center; align-items: center; ">  
<svg version="1.1"
 baseProfile="full"
 width="300" height="200"
 xmlns="http://www.w3.org/2000/svg">
<rect width="100%" height="100%" fill="red" />
<circle cx="150" cy="100" r="80" fill="green" />
<text x="150" y="125" font-size="60" text-anchor="middle" fill="white">SVG</text>
</svg></div>






因为没有现成的绘制函数图的语言，所以可用SVG绘制函数图：



<div style="display: flex; justify-content: center; align-items: center; ">  
<svg width="400" height="200">
<!-- 定义箭头，但这段代码在Typora下无意义，而且有更简单方法，即文末放一个空的Mermaid时序图代码块
<defs>  
    <marker id="arrowhead" refX="5" refY="5" markerWidth="6" markerHeight="6" orient="auto" style="fill: grey;">  
        <path d="M 0 0 L 10 5 L 0 10 z" />  
    </marker>  
</defs> -->
<!-- 绘制x轴和标签 -->  
<line x1="0" y1="100" x2="390" y2="100" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />  
<text x="390" y="90" style="fill: grey;">x</text>  
<!-- 绘制y轴和标签 -->  
<line x1="200" y1="200" x2="200" y2="4" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />  
<text x="185" y="10" style="fill: grey; transform: rotate(-90 200 100);">y</text>
<!-- 其他类型的箭头 -->  
<line x1="0" y1="120" x2="390" y2="120" style="stroke: grey; stroke-width: 2; marker-end: url(#crosshead);" />
<line x1="0" y1="140" x2="390" y2="140" style="stroke: grey; stroke-width: 2; marker-end: url(#filled-head);" />
<line x1="0" y1="160" x2="390" y2="160" style="stroke: grey; stroke-width: 2; stroke-dasharray:10,5; marker-end: url(#sequencenumber);" />  
</svg></div>








<div style="display: flex; justify-content: center; align-items: center; ">  
<svg width="400" height="200">
<!-- 绘制x轴和标签 -->  
<line x1="0" y1="100" x2="390" y2="100" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />  
<text x="390" y="90" style="fill: grey;">x</text>  
<!-- 绘制y轴和标签 -->  
<line x1="200" y1="200" x2="200" y2="4" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />  
<text x="185" y="10" style="fill: grey; transform: rotate(-90 200 100);">y</text>  
<!-- 绘制sin(x)曲线（这里使用二次贝塞尔曲线近似） -->  
<path d="M50,100 C50,100 125,250 200,100 C200,100 275,-50 350,100"  
      style="stroke: blue; stroke-width: 2; fill: none;" />
<!-- 画点，以便确定曲线的参数，之后可以注释掉 --> 
<circle cx="6" cy="6" r="3" style="fill: green;"/>
<circle cx="50" cy="100" r="3" style="fill: red;"/>
<circle cx="125" cy="170" r="3" style="fill: pink;"/>
<circle cx="200" cy="100" r="3" style="fill: red;"/>
<circle cx="275" cy="30" r="3" style="fill: yellow;"/>
<circle cx="350" cy="100" r="3" style="fill: red;"/>
</svg></div>




右手定理

<div style="display: flex; justify-content: center; align-items: center; ">
<svg width="400" height="200">
<!-- 绘制x轴和标签 -->
<line x1="200" y1="100" x2="320" y2="180" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />
<text x="330" y="170" style="fill: grey;">x</text>
<!-- 绘制y轴和标签 -->
<line x1="200" y1="100" x2="200" y2="4" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />  
<text x="180" y="10" style="fill: grey; transform: rotate(-90 200 100);">y</text>
<!-- 绘制z轴和标签 -->
<line x1="200" y1="100" x2="80" y2="180" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />
<text x="65" y="180" style="fill: grey; transform: rotate(-90 200 100);">z</text>
<!-- 手部分 -->
<ellipse cx="200" cy="120" rx="50" ry="40" fill="orange" />
<ellipse cx="230" cy="90" rx="18" ry="50" fill="orange" />
<ellipse cx="184" cy="146" rx="30" ry="10" style="stroke: white; stroke-width: 3; fill: orange" />
<ellipse cx="178" cy="130" rx="30" ry="12" style="stroke: white; stroke-width: 3; fill: orange" />
<ellipse cx="176" cy="110" rx="30" ry="12" style="stroke: white; stroke-width: 3; fill: orange" />
<ellipse cx="178" cy="92" rx="30" ry="12" style="stroke: white; stroke-width: 3; fill: orange" />
</svg></div>






左手定理

<div style="display: flex; justify-content: center; align-items: center; "> 
<svg width="400" height="200"> 
<!-- 绘制x轴和标签 -->  
<line x1="200" y1="100" x2="320" y2="180" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />
<text x="330" y="170" style="fill: grey;">x</text>
<!-- 绘制y轴和标签 -->
<line x1="200" y1="100" x2="200" y2="4" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />
<text x="180" y="10" style="fill: grey; transform: rotate(-90 200 100);">y</text>
<!-- 绘制z轴和标签 -->
<line x1="200" y1="100" x2="80" y2="180" style="stroke: grey; stroke-width: 2; marker-end: url(#arrowhead);" />
<text x="65" y="180" style="fill: grey; transform: rotate(-90 200 100);">z</text>
<!-- 手部分 -->
<ellipse cx="200" cy="120" rx="50" ry="40" fill="orange" />
<ellipse cx="170" cy="90" rx="18" ry="50" fill="orange" />
<ellipse cx="216" cy="146" rx="30" ry="10" style="stroke: white; stroke-width: 3; fill: orange" />
<ellipse cx="222" cy="130" rx="30" ry="12" style="stroke: white; stroke-width: 3; fill: orange" />
<ellipse cx="224" cy="110" rx="30" ry="12" style="stroke: white; stroke-width: 3; fill: orange" />
<ellipse cx="222" cy="92" rx="30" ry="12" style="stroke: white; stroke-width: 3; fill: orange" />
</svg></div>








<div style="display: flex; justify-content: center; align-items: center; ">
<!--  绘画按照200*100，呈现扩大一倍400*200 -->
<svg viewBox="0 0 200 100" width="400" height="200">
  <rect width="100%" height="100%" fill="green" />
  <path
    fill="none"
    stroke="lightgrey"
    d="M20,50 C20,-50 180,150 180,50 C180-50 20,150 20,50 z" />
  <circle r="5" fill="red">
    <animateMotion
      dur="10s"
      repeatCount="indefinite"
      path="M20,50 C20,-50 180,150 180,50 C180-50 20,150 20,50 z" />
  </circle>
</svg></div>





题外话：[Desmos | 图形计算器](https://www.desmos.com/calculator?lang=zh-CN)





## Mermaid绘图

具体参见官网：[About Mermaid | Mermaid](https://mermaid.js.org/intro/)

### 流程图

节点样式，方向可以是LR、RL、TB或TD、BT

 ```mermaid
 graph TB  
 A  
 B[bname]  
 C(cname)  
 D((dname))  
 E>ename]  
 F{fname}
 ```

连线

 ```mermaid
 graph LR  
 A-->B
 B-->C
 C-->D
 D-->A
 ```

 连线样式

 ```mermaid
 graph TD
 A0--->B0
 A1-->B1
 A2---B2
 A3--text---B3
 A4--text-->B4
 A5-.-B5
 A6-.->B6
 A7-.text.-B7
 A8-.text.->B8
 A9===B9
 A10==>B10
 A11 o--o B11
 A12 x--x B12
 A13 <--> B13
 ```

节点名称可以包含转义字符

```mermaid
graph LR
        A["A double quote:#quot;"]-->B["A dec char:<br>#9829;"]
```

子图

```mermaid
graph LR
subgraph g1
a1-->b1
end
subgraph g2
%%下面这句可以声明子流程图方向
%%direction TB
a2-->b2
end
subgraph g3
a3-->b3
end
a3-->a2
g2-->g1
```

### 时序图

```mermaid
sequenceDiagram
%%下面这句可以把Alice和John包裹在一起，记得配合end使用，Yellow也可以写成rgb(250,50,250)
%%box Yellow 饭店
%%下面这句可以起别名
participant Alice as AliceABC
%%下面这句可以把John画成小人，同时给John起别名JohnAAA
actor John as JohnAAA
%%end
Alice->>John: Hello John, how are you ?
John-->>Alice: Great!
Alice->>John: Hung,you are better .
John-->>Alice: yeah, Just not bad.
John--)Alice: Hi.
John-)Alice: Hi.
John-x Alice: Hi.
rect rgb(0, 200, 0)
Alice ->> John: 是尾号1234的乘客吗
rect rgb(50, 150, 200)
John ->> Alice: 对
end
Alice ->> John: 上车吧
end

autonumber
John--)Alice: Hi.
John-)Alice: Hi.
John-x Alice: Hi.

```

中等复杂时序图

```mermaid
 sequenceDiagram  
 participant server  
 participant CA  
 participant client  
 
 server->>CA: 这是我的公钥  
 CA-->>server: 下发证书  
 server->client: 建立连接  
 client->>server: 我要 RSA 算法加密的公钥  
 server-->>client: 下发证书与公钥  
 client-->>server: 上报通过公钥加密的随机数  
 server->>server: 生成对称加密秘钥  
 client-->server: 加密通信  
 client-->server: 加密通信  
 client-xserver: 关闭连接  
```

复杂时序图

```mermaid
 sequenceDiagram  
 participant Doctor  
 participant Bob  
 
 note right of Bob: Bob is a patient  
 
 loop Look Bob each hour  
 Doctor->>Bob: How are you?  
 alt Bob is sick  
 Bob->>Doctor: Not so good.  
 else  
 Bob->>Doctor: Fine, thank you.  
 end  
 loop Ask about patient  
 Doctor-->Bob: Inquire about the situation  
 end  
 opt Extra response  
 Bob->>Doctor:Thanks for asking  
 end  
 end   
 
 note right of Doctor: hourly ask finished
```

### 甘特图

```mermaid
gantt
dateFormat YYYY-MM-DD
section S1
T1: 2014-01-01, 9d
section S2
T2: 2014-01-11, 9d
section S3
T3: 2014-01-02, 9d
```

复杂甘特图

```mermaid
 gantt
     dateFormat  YYYY-MM-DD
     title Adding GANTT diagram functionality to mermaid
     section A section
     Completed task             :done,    des1, 2014-01-06,2014-01-08
     Active task                :active,  des2, 2014-01-09, 3d
     Future task                :         des3, after des2, 5d
     Future task2               :         des4, after des3, 5d
 
     section Critical tasks
     Completed task in the critical line :crit, done, 2014-01-06,24h
     Implement parser and jison          :crit, done, after des1, 2d
     Create tests for parser             :crit, active, 3d
     Future task in critical line        :crit, 5d
     Create tests for renderer           :2d
     Add to mermaid                      :1d
 
     section Documentation
     Describe gantt syntax               :active, a1, after des1, 3d
     Add gantt diagram to demo page      :after a1  , 20h
     Add another diagram to demo page    :doc1, after a1  , 48h
 
     section Last section
     Describe gantt syntax               :after doc1, 3d
     Add gantt diagram to demo page      : 20h
     Add another diagram to demo page    : 48h
```

### 饼图

```mermaid
pie showData
title 一天都在干什么
"学习" : 12
"睡觉" : 9
"其它" : 3
```

### 用户行程图

```mermaid
journey
  title 毕业设计安排
  section 选题阶段
    导师申报论文题目 : 7 : 导师
    学生与导师双选 : 7 : 学生, 导师
    导师下达任务书 : 7 : 导师
  section 调研阶段
    学生搜集资料阅读资料 : 4 : 导师, 学生
    学生上交综述与译文 : 5 : 导师, 学生
  section 开题阶段
    完成开题报告与开题答辩 : 7 : 导师, 学生, 专业系, 学院
  section 撰写阶段
    写完三稿论文和设计说明书，经过三位老师评审 : 6 : 导师, 学生
    论文进度期中检查 : 6 : 专业系, 学院
  section 答辩评定阶段
    组织答辩 : 4 : 导师, 答辩小组, 答辩委员会
    成绩评定 : 3 : 导师, 答辩小组, 答辩委员会, 学院
  section 补充
    二次答辩 : 1 : 学院
  section 资料归档
    收尾工作 : 9 : 学院
```

### 柱状图

```mermaid
xychart-beta
    title "Sales Revenue"
    x-axis [jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec]
    y-axis "Revenue (in $)" 4000 --> 11000
    bar [5000, 6000, 7500, 8200, 9500, 10500, 11000, 10200, 9200, 8500, 7000, 6000]
    line [5000, 6000, 7500, 8200, 9500, 10500, 11000, 10200, 9200, 8500, 7000, 6000]

```

### 象限图

```mermaid
quadrantChart
    title Reach and engagement of campaigns
    x-axis Low Reach --> High Reach
    y-axis Low Engagement --> High Engagement
    quadrant-1 We should expand
    quadrant-2 Need to promote
    quadrant-3 Re-evaluate
    quadrant-4 May be improved
    Campaign A: [0.3, 0.6]
    Campaign B: [0.45, 0.23]
    Campaign C: [0.57, 0.69]
    Campaign D: [0.78, 0.34]
    Campaign E: [0.40, 0.34]
    Campaign F: [0.35, 0.78]

```

### 类视图

[保姆级教程--类图怎么画-CSDN博客](https://blog.csdn.net/q584401071/article/details/122201102)

```mermaid
classDiagram
Class01 <|-- AveryLongClass : Cool
Class03 *-- Class04
Class05 o-- Class06
Class07 .. Class08
Class09 --> C2 : Where am i?
Class09 --* C3
Class09 --|> Class07
Class07 : equals()
Class07 : Object[] elementData
Class01 : size()
Class01 : int chimp
Class01 : int gorilla
Class08 <--> C2: Cool label
```

### Git分支图

```mermaid
gitGraph
   commit
   commit
   branch develop
   commit
   commit
   commit
   checkout main
   commit
   commit
```

再复杂一点：

```mermaid
gitGraph:
    commit "Ashish"
    branch newbranch
    checkout newbranch
    commit id:"1111"
    commit tag:"test"
    checkout main
    commit type: HIGHLIGHT
    commit
    merge newbranch
    commit
    branch b2
    commit
```

### 关系图

[实体关系图_百度百科 (baidu.com)](https://baike.baidu.com/item/实体关系图/9005309)

```mermaid
erDiagram
    CUSTOMER ||--o{ ORDER : places
    ORDER ||--|{ LINE-ITEM : contains
    CUSTOMER }|..|{ DELIVERY-ADDRESS : uses
```



```mermaid
sequenceDiagram
actor 已经到底了
```

