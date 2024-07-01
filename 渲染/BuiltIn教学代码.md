# 基础

BuiltIn特征：

- CGPROGRAM / ENDCG
- sampler2D _MainTex;
- UnityObjectToClipPos();
- tex2D();

Urp代码特征：

- HLSLINCLUE / HLSLPROGRAM / ENDHLSL
- CBUFFER_START(UnityPerMaterial)  CBUFFER_END
- TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
- TransformObjectToHClip();
- SAMPLE_TEXTURE2D();

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

