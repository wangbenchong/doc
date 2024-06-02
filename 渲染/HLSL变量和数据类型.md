# 标量

## 标量类型包括

- bool-对或错，很少直接用bool，通常是用浮点，如属性块里

  ```c
  //勾选
  [Toggle]_IsXXX("Is XXX", Float) = 1
  //或者写成滑杆
  [IntRange]_IsXXX("Is XXX", Range(0, 1)) = 1
  ```

  然后CBUFFER里这样写

  ```c
  HLSLINCLUDE
      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
      CBUFFER_START(UnityPerMaterial)
      	half _IsXXX;
      CBUFFER_END
  ENDHLSL
  ```

  调用变量时就当是bool在用

  ```c
  if(_IsXXX){
  }else{}
  ```

  

- float-32位浮点数。通常用于世界空间位置、纹理坐标或涉及复杂函数（例如三角函数或幂/幂）的标量计算。

- half-16位浮点数。通常用于短矢量、方向、物体空间位置、颜色。

- double-64位浮点数。不能用作输入/输出，请参阅此处的注释。

- real-当函数可以支持half或float时，在URP/HDRP中使用。默认为half（假定平台支持），除非着色器指定"#define PREFER_HALF 0"，否则它将使用浮点精度。ShaderLibrary函数中的许多常见数学函数都使用此类型。

- int-32位有符号整数

- uint-32位无符号整数（GLES除外，不支持此整数，而是定义为int）。

## 注意Fixed

- HLSL不支持fixed，只在buildin里使用。所以从内置管线升级时，请改用half
- 11位定点数，范围为-2到2。通常用于LDR颜色
- 是来自CG语法的东西，所有平台现在都只是将其转换为half，即使在CGPROGRAM中也是如此

# 向量

- Float4 包含4个浮点的浮点向量
- Half3 半精度向量，3个分量
- Int2 整型向量，2个分量
- 从技术上讲float1，他也是一个一维向量，要采用数组语法访问。

# 矩阵

- float4x4 -4行4列
- int4x3 -4行3列
- half2x1 -2行1列
- float1x4 -1行4列


## Unity有内置的变换矩阵用于空间变换，例如

- UNITY_MATRIX_M -模型矩阵，从对象空间（也叫模型空间）转换为世界空间

- UNITY_MATRIX_V -视图矩阵，从世界空间转换为视图空间

- UNITY_MATRIX_P -投影矩阵，从视图空间转换为投影空间（也叫裁剪空间）

- UNITY_MATRIX_VP -查看投影矩阵，从世界空间转换为投影空间


## 变换流程：M->V->P->NDC->S(屏幕)

## 逆变换版本

- UNITY_MATRIX_I_M -逆模型矩阵，从世界空间转换到对象空间
- UNITY_MATRIX_I_V -逆视图矩阵，从视图控件转换为世界空间
- UNITY_MATRIX_I_p -逆投影矩阵，从投影空间转换为视图空间
- UNITY_MATRIX_I_VP -逆视图投影矩阵，从投影空间转换为世界空间

## 矩阵访问方式

- 0开始的行列访问方式，如 

  ```
  ._m00
  ._m33
  ```

  

- 1开始的行列访问方式，如 

  ```
  ._11
  ._44
  ```

  

- 数组访问，如

  ```
  [0][0]
  [3][3]
  ```

- Swizzle, 如

  ```
  ._m00_m11
  ._11_22
  ```

  
