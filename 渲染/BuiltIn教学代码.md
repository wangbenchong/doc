# 基础

## BuiltIn和Urp特征区别

BuiltIn特征：

- CGPROGRAM / ENDCG
- #include "UnityCG.cginc"//不是必须写
- #include "Lighting.cginc"//这句写了上一句就可以省了
- sampler2D _MainTex;
- float4 _MainTex_ST;//存缩放和偏移
- struct及形参命名：appdata v, return o, v2f i
- UnityObjectToClipPos();//vert下
- o.uv = data.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;//vert下
- tex2D();//frag下

Urp代码特征：

- Tags { "RenderPipeline"="UniversalPipeline" }
- HLSLINCLUE / HLSLPROGRAM / ENDHLSL
- #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
- CBUFFER_START(UnityPerMaterial),  CBUFFER_END
- TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
- float4 _MainTex_ST;//只声明就好
- stuct及形参命名：Attributes IN, return OUT, Varyings IN
- TransformObjectToHClip();//vert下
- OUT.uv = TRANSFORM_TEX(IN.uv,  _MainTex); //vert下，自动调用 _MainTex_ST
- SAMPLE_TEXTURE2D();//frag下

## BuiltIn和Urp共通点

基本一致的数学和时间函数：

float4 _Time, _SinTime, _CosTime, unity_DeltaTime

cell(), floor(),  round(), max(), min(), lerp(), clamp(), saturate() , sin(), cos(), tan(), asin(), dot(), cross(), abs(), sqrt(), pow()等等



## SubShader Tags

[【Shader进阶】SubShader块标签Tags——DisableBatching-CSDN博客](https://blog.csdn.net/qq_39574690/article/details/105400316)

# 光照

漫反射：兰伯特、半兰伯特

```c
//获得直射光的光方向，标准化向量
half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
//兰伯特公式运算
half3 diffuse = _LightColor0.rgb * _DiffuseColor.rgb * max(0, dot(worldNormal, worldLightDir));
//半兰伯特公式运算
half3 diffuse = _LightColor0.rgb * _DiffuseColor.rgb * (dot(worldNormal, worldLightDir) * 0.5 + 0.5);
```

菲涅尔反射

```c
// 菲涅尔因子计算，_FresnelScale取值0.1到5，默认1，_fresnelPower取值1到10
float fresnelFactor = _FresnelScale * pow(1.0 + dot(normal, viewDir), _FresnelPower);
fresnelFactor = saturate(fresnelFactor); // 限制在0到1之间

// 混合基础颜色和反射颜色
half4 refl = tex2D(_ReflectionTex, i.uv); // 假设反射贴图与基础贴图UV相同
half4 finalColor = lerp(col, refl, fresnelFactor);
```

Phong光照模型

```c
 // Phong 高光反射
half3 reflectDirWS = normalize(reflect(-lightDirWS, normalWS));
half spec = pow(max(0, dot(reflectDirWS, viewDirWS)), _Shininess);//_Shininess取值8~255默认20
```

Blinn-Phong光照模型

```c
// Blinn-Phong 高光反射
half3 halfDirWS = normalize(lightDirWS + viewDirWS); //计算半角向量
//使用半角向量代替反射光线向量
half spec = pow(max(0, dot(normalWS, halfDirWS)), _Shininess);//_Shininess取值8~255默认20
```



# 控制指令

属于输出合并阶段

## 裁剪Cull

## Alpha测试

## 深度测试ZTest

## 深度写入ZWrite

## 混合Blend

## 模板测试Stencil

