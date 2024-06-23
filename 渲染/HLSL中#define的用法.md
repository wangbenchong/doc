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
#define A B//这里B可以是任何内容，常数、变量、函数、空格乃至这些的集合体但是#if这种不行，支持代码多行，只要换行加上\
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

# #define的作用范围

- 在不同Pass内的#define互不影响
- 在不同SubShader内的#define互不影响
- #define不可以写在函数体内

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
#if defined(FEATURE_A)//其他写法有 #ifdef、取反则是#ifndef、#if !defined()， 连接符还有 ||等
    // 这部分代码仅在FEATURE_A被定义时编译和执行  
#endif
```

1. `#if A`

这种写法检查的是宏`A`的值。如果`A`被定义为非零值，那么`#if A`后面的代码块会被编译；如果`A`被定义为零或者根本没有被定义，那么代码块将不会被编译。

例如：

```c
#define FEATURE_B 1
  
#if FEATURE_B //等同于写成#if FEATURE_B == 1, 如果写成#if 0 相当于注释， 而写成#if 1则相当于没写
    // 这部分代码仅在FEATURE_B的值为非零时编译和执行  
#endif
```

在这个例子中，如果`FEATURE_B`被定义为1（或其他非零值），那么`#if FEATURE_B`后的代码会被编译。如果`FEATURE_B`被定义为0，那么代码将不会被编译。

**区别总结**：

- `#if defined(A)` 只关心`A`是否被定义，不关心其值。
- `#if A` 关心`A`的值，只有当`A`为非零时，条件才为真。

在实际编程中，根据具体需求选择使用哪种方式。如果你只想检查某个功能是否被启用（而不关心其具体配置），你可能会使用`#if defined(A)`。如果你需要根据配置的具体值来决定是否编译某段代码，你应该使用`#if A`。

# #pragma用法

## 内置 or 自定义？双标！

- 在unity shader中为什么只写一句`#pragma multi_compile _ _ADDITIONAL_LIGHTS`就可以打开`_ADDITIONAL_LIGHTS`宏，但是`#pragma multi_compile _ _WBC`还要在C#调用EnableKeyword函数后才会打开  `_WBC`  宏呢？

  答：在Unity的Shader编程中，`#pragma multi_compile` 是一个预处理指令，它用于编译Shader的多个变体，以便在运行时根据需要在这些变体之间进行选择。然而，`#pragma multi_compile` 的使用方式和行为可能因不同的参数和上下文而有所不同。
  
  对于 `#pragma multi_compile _ _ADDITIONAL_LIGHTS`，Unity的Shader编译器默认知道 `_ADDITIONAL_LIGHTS` 是一个内置的关键字，用于控制是否包含额外的光照计算。当这个预处理指令被包含在Shader中时，Unity的Shader编译器会自动为启用和禁用 `_ADDITIONAL_LIGHTS` 的情况生成两个Shader变体。
  
  然而，对于自定义的关键字，如 `_WBC`，情况就不同了。Unity的Shader编译器并不知道 `_WBC` 是什么，除非你明确地告诉它。这就是为什么你需要通过C#脚本来调用 `Shader.EnableKeyword` 方法来启用 `_WBC` 关键字。这告诉Unity的Shader系统在运行时选择那个启用了 `_WBC` 的Shader变体。
  
  总结一下：
  
  - `_ADDITIONAL_LIGHTS` 是一个Unity Shader系统内置的关键字，用于控制是否包含额外的光照计算。当你使用 `#pragma multi_compile` 包含它时，Unity的Shader编译器会自动生成两个Shader变体（一个启用，一个禁用）。
  - 对于自定义的关键字（如 `_WBC`），你需要通过C#脚本来调用 `Shader.EnableKeyword` 方法来告诉Unity的Shader系统要启用哪个关键字，并生成相应的Shader变体。
  
  注意：在使用 `#pragma multi_compile` 时，你还需要注意它的性能影响。为每个关键字组合生成一个Shader变体可能会显著增加构建时间和Shader的内存占用。因此，你应该谨慎使用它，并确保你真正需要生成所有的变体。在某些情况下，使用 `#pragma shader_feature` 或其他预处理指令可能更为合适。




- 但是如果写了#pragma multi_compile _ _ADDITIONAL_LIGHTS，那么即便在C#调用DisableKeyword也无法关闭_ADDITIONAL_LIGHTS宏，这是为什么？

  答：在Unity的Shader编程中，`#pragma multi_compile` 是一个预处理指令，它用于编译Shader的多个变体，以便在运行时根据需要在这些变体之间进行选择。然而，`#pragma multi_compile` 的使用方式和行为可能因不同的参数和上下文而有所不同。

  对于 `#pragma multi_compile _ _ADDITIONAL_LIGHTS`，Unity的Shader编译器默认知道 `_ADDITIONAL_LIGHTS` 是一个内置的关键字，用于控制是否包含额外的光照计算。当这个预处理指令被包含在Shader中时，Unity的Shader编译器会自动为启用和禁用 `_ADDITIONAL_LIGHTS` 的情况生成两个Shader变体。

  然而，对于自定义的关键字，如 `_WBC`，情况就不同了。Unity的Shader编译器并不知道 `_WBC` 是什么，除非你明确地告诉它。这就是为什么你需要通过C#脚本来调用 `Shader.EnableKeyword` 方法来启用 `_WBC` 关键字。这告诉Unity的Shader系统在运行时选择那个启用了 `_WBC` 的Shader变体。

**总结一下：**

- _ADDITIONAL_LIGHTS 是一个Unity Shader系统内置的关键字，用于控制是否包含额外的光照计算。当你使用 `#pragma multi_compile` 包含它时，Unity的Shader编译器会自动生成两个Shader变体（一个启用，一个禁用）。
- 对于自定义的关键字（如 `_WBC`），你需要通过C#脚本来调用 `Shader.EnableKeyword` 方法来告诉Unity的Shader系统要启用哪个关键字，并生成相应的Shader变体。
- 注意：在使用 `#pragma multi_compile` 时，你还需要注意它的性能影响。为每个关键字组合生成一个Shader变体可能会显著增加构建时间和Shader的内存占用。因此，你应该谨慎使用它，并确保你真正需要生成所有的变体。在某些情况下，使用 `#pragma shader_feature` 或其他预处理指令可能更为合适。

## 自定义宏要守规则

像`#pragma shader_feature A`这种写法，如果Property里没有对应   [Toggle(A)]  变量在控制，那么默认就是没定义（倒也没做undef，只是没定义而已）；如果Toggle控制成打勾了，就相当于`#define A 1`。也就是说，凡是写#pragma shader_feature但是没做Toggle的，除非运行时C#调用material.EnableKeyword，否则不会走声明逻辑。

## shader_feature 和 multi_compile的区别

其实并没有太大区别，都能用C#代码`material.EnableKeyword`动态切换。而且在只考虑两个关键词A和B的情况下，`#pragma shader_feature` 和 `#pragma multi_compile` 都会生成相同的三种变体（无关键词、仅A、仅B），如果还考虑A和B同时开启的情况，那么还会多出一个变体。但它们的关键区别在于如何处理这些变体以及它们对运行时性能的影响。

`#pragma shader_feature` 和 `#pragma multi_compile` 的主要区别在于：

1. 编译时行为
   - `#pragma shader_feature`：Unity会为所有启用的`shader_feature`关键词组合生成Shader变体。这意味着，如果你有两个`shader_feature`定义的关键词A和B，并且你的场景中有物体使用了这些关键词的不同组合，Unity将只为这些实际用到的组合生成变体。未使用的组合不会被编译。
   - `#pragma multi_compile`：无论是否所有变体都会被实际使用，Unity都会为所有可能的关键词组合生成Shader变体。这可能导致更多的Shader变体被编译，从而增加编译时间和磁盘空间占用。
2. 运行时行为
   - 两者都允许在运行时通过`Material.EnableKeyword`和`Material.DisableKeyword`来启用或禁用关键词，从而影响Shader的渲染行为。但是，由于`multi_compile`生成了所有可能的变体，因此它在运行时切换关键词时通常具有更好的性能，因为所有变体都已经被预先编译并加载到内存中。而`shader_feature`在运行时可能需要动态地重新编译未使用的变体，这可能会导致性能下降。
3. 适用场景
   - `#pragma shader_feature` 更适合那些你知道只会使用特定关键词组合的场景。由于它只为实际使用的组合生成变体，因此可以节省编译时间和磁盘空间。
   - `#pragma multi_compile` 更适合那些需要在运行时动态切换不同渲染效果的场景。由于它生成了所有可能的变体，因此可以确保在运行时快速切换而无需重新编译。

总结来说，虽然`#pragma shader_feature`和`#pragma multi_compile`在只考虑两个关键词时生成的变体数量可能相同，但它们在编译时和运行时的行为以及适用场景上有所不同。`multi_compile`更适合需要在运行时动态切换的场景，因为它可以确保所有变体都已经被预先编译并加载到内存中，从而提供更好的性能。

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

# 关于#undef

该指令可取消宏定义，用法：`#undef A`

尽管标准的HLSL语言规范可能不支持`#undef`指令，但在Unity的着色器系统中，由于Unity的着色器编译器和预处理器提供了额外的功能，你可以看到这样的指令被使用。
