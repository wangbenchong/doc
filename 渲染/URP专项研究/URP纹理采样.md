# 最简示例

```c
HLSLINCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
CBUFFER_START(UnityPerMaterial)
    TEXTURED2D(_MainTex);//Dx11里，一个shader中允许使用128张纹理
    SAMPLER(sampler_MainTex);//Dx11里，一个shader最多16个纹理采样器
	//采样器复用，名字中带Repeat/Clamp/Mirror、Point/Bilinear/Trilinear来指定Wrap/Filter
	SamplerState(MyRepeatPointSampler)//可以替代sampler_MainTex，自定义独立采样器
    float4 _MainTex_ST;//引擎传入，不需控制 Scaling & Offset
CBUFFER_END
ENDHLSL
Pass
{
    Tags{"LightMode"="UniversalForward"}
    HLSLPROGRAM
    #pragma vertex UnlitPassVertex
    #pragma fragment UnlitPassFragment
    struct Attribute
    {
        float3 posOS : POSITION;//命名规范 OS/WS/VS/CS
        float2 uv : TexCOORD0;
    };
    struct Varying
    {
        float4 posCS : SV_POSITION;
        float2 uv : TEXCOORD0;
    };
    Varying UnlitPassVertex(Attribute IN)
    {
        Varing OUT = (Varying)0;
        OUT.posCS = mul(UNITY_MATRIX_MVP, float4(In.posOS,1));
        OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);//In.uv * _MainTex_ST.xy + _MainTex_ST.zw
    }
    ENDHLSL
}

```

# 纹理对象

```c
//Texture2DArray
TEXTURE2D_ARRAY(textureName);
SAMPLER(sampler_textureName);
//...
flout4 color = 	SAMPLE_TEXTURE2D_ARRAY(textureName, sampler_textureName,ui,index);
flout4 color = 	SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, sampler_textureName,ui,lod);
```

```c
//Texture3D
TEXTURE3D(textureName);
SAMPLER(sampler_textureName);
//...
float4 color = SAMPLE_TEXTURE3D(textureName, sampler_textureName, uvw);
float4 color = SAMPLE_TEXTURE3D_LOD(textureName, sampler_textureName, uvw, lod);
//use 3D uv coord(commonly referred to as uvw)
```

```c
//TextureCube立方体纹理 
TEXTURECUBE(textureName);
SAMPLER(sampler_textureName);
//...
float4 color = SAMPLE_TEXTURECUBE(textureName, sampler_textureName, dir);
float4 color = SAMPLE_TEXTURECUBE_LOD(textureName, sampler_textureName, dir, lod);
//use 3D uv coord (named dir here, as it is typically a direction)
```

```c
//TextureCubeArray
TEXTURECUBE_ARRAY(textureName);
SAMPLER(sampler_textureName);
//...
float4 color = SAMPLE_TEXTURECUBE_ARRAY(textureName, sampler_textureName, dir, index);
float4 color = SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, sampler_textureName, dir, lod);
```

