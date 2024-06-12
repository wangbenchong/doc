# 关于#define

在查阅了众多hlsl源码后，我总结#define的用法如下：

情况1：

```c
//定义空宏
#define A//这里A可以是一个词，也可以后面跟(参数1, 参数2)形成一个函数的声明, 但仅仅是声明，没有实现
```

情况2：

```c
//将B定义为A
#define A B//这里B可以是任何内容，常数、变量、函数、或者这些的集合体，支持代码多行，只要换行加上\
```

这里着重说一下情况2，这个B的灵活性真的相当高。尤其是A为函数的时候，这个B可以随意的折腾A里面的参数，甚至可以把A里面的参数和其他内容相拼接（用##）：

```c
//##的用法作为字符串拼接，是经过Unity特殊处理的，正常hlsl不支持这个写法
#define SHAPE_LIGHT(index)\
//-------以下是B部分---------------
    TEXTURE2D(_ShapeLightTexture##index);\
    SAMPLER(sampler_ShapeLightTexture##index);\
    half2 _ShapeLightBlendFactors##index;\
    half4 _ShapeLightMaskFilter##index;\
    half4 _ShapeLightInvertedFilter##index;
```



# #define花式用法：批量化定义同名函数

我在hlsl脚本中看到这么一句：

```c
// These clamping function to max of floating point 16 bit are use to prevent INF in code in case of extreme value
TEMPLATE_1_REAL(ClampToFloat16Max, value, return min(value, HALF_MAX))
```

当时看到宏函数中居然某个参数还有return关键字，这不是语法错误吗？

随后我在源代码（Macros.hlsl）中找到了一个类似于TEMPLATE_1_REAL的名字TEMPLATE_1_INT，并发现了它的定义代码，这似乎能解释TEMPLATE_1_REAL的含义。代码如下：

```c
#define TEMPLATE_1_INT(FunctionName, Parameter1, FunctionBody) \
    int    FunctionName(int    Parameter1) { FunctionBody; } \
    int2   FunctionName(int2   Parameter1) { FunctionBody; } \
    int3   FunctionName(int3   Parameter1) { FunctionBody; } \
    int4   FunctionName(int4   Parameter1) { FunctionBody; } \
    uint   FunctionName(uint   Parameter1) { FunctionBody; } \
    uint2  FunctionName(uint2  Parameter1) { FunctionBody; } \
    uint3  FunctionName(uint3  Parameter1) { FunctionBody; } \
    uint4  FunctionName(uint4  Parameter1) { FunctionBody; }
```

**用法详解**：

明白了，这个 `TEMPLATE_1_INT` 宏定义展示了如何为一个函数名称和函数体生成多个重载版本，这些版本接受不同类型的整数参数（`int`, `int2`, `int3`, `int4`, `uint`, `uint2`, `uint3`, `uint4`）。

基于这个定义，我们可以推测 `TEMPLATE_1_REAL` 宏（尽管你没有直接给出它的定义）可能是为浮点数类型（如 `float`, `half`, `half3`, `half4`, `float2`, `float3`, `float4` 等）生成类似的重载版本。

这样的宏通常用于简化代码，避免为每个数据类型手动编写重复的函数定义。

对于最初看到的 `TEMPLATE_1_REAL(ClampToFloat16Max, value, return min(value, HALF_MAX))` 调用，如果 `TEMPLATE_1_REAL` 的定义与 `TEMPLATE_1_INT` 类似，那么它可能会生成类似于以下的函数集：

```c
half   ClampToFloat16Max(half   value) { return min(value, HALF_MAX); }  
half2  ClampToFloat16Max(half2  value) { return min(value, half2(HALF_MAX, HALF_MAX)); }  
half3  ClampToFloat16Max(half3  value) { return min(value, half3(HALF_MAX, HALF_MAX, HALF_MAX)); }  
half4  ClampToFloat16Max(half4  value) { return min(value, half4(HALF_MAX, HALF_MAX, HALF_MAX, HALF_MAX)); }  
// 类似地，也可能有 float, float2, float3, float4 的版本
```

注意，这里我假设 `HALF_MAX` 是一个常量，它代表了 `half` 类型的最大值。对于 `half2`, `half3`, `half4`，我使用了相同的 `HALF_MAX` 值来填充向量的每个组件，但这取决于具体的实现和需求。

这样的宏定义和函数调用在着色器编程中很常见，特别是当你需要为多种数据类型编写相似的函数时。它们可以提高代码的可读性和可维护性，同时减少重复的代码量。



# #if defined(A)和#if A的区别

在HLSL（和C/C++预处理器中），`#if defined(A)` 和 `#if A` 是两种不同的预处理器指令，它们用于条件编译，但它们的语义和行为有所不同。

1. `#if defined(A) 或者 #ifdef A`

这种写法检查的是宏`A`是否已经被定义，而不关心`A`的值是什么。`defined(A)`会在宏`A`已定义时返回`true`（或非零值），否则返回`false`（或零）。这通常用于检查某个功能或配置是否已经启用，而不关心具体的配置值。

例如：

```c
#if defined(FEATURE_A)  
    // 这部分代码仅在FEATURE_A被定义时编译和执行  
#endif
```

1. `#if A`

这种写法检查的是宏`A`的值。如果`A`被定义为非零值，那么`#if A`后面的代码块会被编译；如果`A`被定义为零或者根本没有被定义，那么代码块将不会被编译。

例如：

```c
#define FEATURE_B 1  
  
#if FEATURE_B  
    // 这部分代码仅在FEATURE_B的值为非零时编译和执行  
#endif
```

在这个例子中，如果`FEATURE_B`被定义为1（或其他非零值），那么`#if FEATURE_B`后的代码会被编译。如果`FEATURE_B`被定义为0，那么代码将不会被编译。

**区别总结**：

- `#if defined(A)` 只关心`A`是否被定义，不关心其值。
- `#if A` 关心`A`的值，只有当`A`为非零时，条件才为真。

在实际编程中，根据具体需求选择使用哪种方式。如果你只想检查某个功能是否被启用（而不关心其具体配置），你可能会使用`#if defined(A)`。如果你需要根据配置的具体值来决定是否编译某段代码，你应该使用`#if A`。



# 空宏可以是“包含守卫”

在HLSL（以及许多其他C/C++风格的编程语言中），`#ifndef`, `#define`, 和 `#endif` 这样的预处理指令组合通常用于防止头文件（在HLSL中可能是着色器文件）被重复包含（或“多重包含”）。这种技术被称为“包含守卫”（Include Guard）或“头文件保护”。

具体来说，当你有一个HLSL文件（比如`MyShader.hlsl`），并且这个文件被多个其他文件包含时，你可能会遇到以下问题：

- 如果`MyShader.hlsl`包含了某些函数或全局变量的定义，那么这些定义会在每个包含它的文件中重复，导致编译错误（比如“多重定义”错误）。
- 即使`MyShader.hlsl`只包含声明或宏定义，多次包含也可能导致意外的副作用或编译时警告。

为了避免这些问题，你可以使用包含守卫。在你的`MyShader.hlsl`文件的开头和结尾，你可以这样做：

```c
// MyShader.hlsl  
#ifndef MYSHADER_HLSL  
#define MYSHADER_HLSL  
  
// ... 这里是着色器代码 ...  
  
#endif // MYSHADER_HLSL
```

这里的工作原理是：

- `#ifndef MYSHADER_HLSL` 检查是否已经定义了`MYSHADER_HLSL`。
- 如果`MYSHADER_HLSL`没有被定义（即这是文件第一次被包含），那么`#define MYSHADER_HLSL`会定义它，并且随后的代码（在`#ifndef`和`#endif`之间）会被包含。
- 如果`MYSHADER_HLSL`已经被定义（即文件已经被包含过一次），那么`#ifndef`后的代码会被跳过，从而防止了多重包含。

这种技术可以确保每个HLSL文件只被包含一次，无论它被其他文件包含了多少次。这是一种常见的编程实践，用于管理复杂的项目依赖和构建过程。
