# 数组

- 数组可以用for进行遍历

- Unity只能从C#脚本设置Vector（float4）和 Float数组。无法从Property材质属性快（2D纹理数组除外）进行设置。

```c
float4 _VectorArray[10]; //Vector array << Shader.SetGlobalVector
float _FloatArray[10];//Float array << Shader.SetGlobalFloat

void ArrayExample_float(out float OutF)
{
  float add = 0;
  [unroll]
  //“展开”之意，相当于重复写代码，过多会导致内存占用、代码膨胀。但效率会比不写高一些。
  //如果已知循环次数（不基于变量）并且次数也不多的话，同时再满足循环不会提前退出，则“展开”循环的性能更高
  //亦可写作[unroll(10)], 更推荐这种指明次数的，否则shader自行判断的话可能会根据性能等原因减少次数
  for(int i=0;i<10;i++)
  {
      add += _FloatArray[i];
  }
  OutF = add;
}
```


数组不可以声明在CBUFFER中（否则会影响SRP Batcher），可以声明在CBUFFER外

```c
HLSLINCLUDE
    CBUFFER_START(UnityPerMaterial)
    CBUFFER_END
    //声明在这里
    //C#:Shader.SetGlobalFloatArray("_FloatArray", new []{1f,1f,1f,1f})
    float _FloatArray[4];//长度上限1024，若需更大要使用缓冲区StructuredBuffer<>对象,C#
	//C#:Shader.SetGlobalVectorArray("_VectorArray", new []{new Vector4(1f,1f,1f,1f),new Vector4(0f,0f,0f,0f)})
    float4 _VectorArray[2];
ENDHLSL
```

# 2D纹理数组

可用于2D纹理精灵动画（帧动画）、多贴图过渡融合

```c
properties
{
    //都注释掉，还是直接用C#控制，防止影响SRP Batcher
    //面板上展示效果和Main Texture区别不大
	//_MultiTextures("Multiple Textures", 2DArray) = ""{}//花括号可加可不加
    //_CurLayer("Current Layer", Range(0,1)) = 0//默认采样第0层,只有两张贴图
}
SubShader
{
    Tags{"RenderPipeLine"="UniversalPipeLine" "Queue"="Geometry"}
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    CBUFFER_START(UnityPerMaterial)
    CBUFFER_END
        //写到CBUFFER外面不干扰SRP Batcher
        TEXTURE2D_ARRAY(_MultiTextures);
    	SAMPLER(sampler_MultiTextures);
    	float4 _MultiTextures_ST;//如使用主纹理ST可不写
	    float _CurLayer;
    ENDHLSL
        
    Pass
    {
        //...
        float4 UnlitPassFragment(Varing IN) : SV_TARGET
        {
	        float4 color = SAMPLE_TEXTURE2D_ARRAY(_MultiTextures, sampler_MultiTextures, In.uv, _CurLayer);
            return color;
        }
    }
}
```

C#调用

```c#
public Texture2D[] ordinaryTextures;//图片的可读写要打勾
public int currentLayer;//若改成float可考虑做成多图过渡混合
//最终用这个传给shader
private Texture2DArray texture2DArray;
private void CreateTextureArray()
{
    //Create Texture2DArray, Texture2D的子类
    texture2DArray = new
        Texture2DArray(ordinaryTextures[0].width,
                      ordinaryTextures[0].height,ordinaryTextures.Length,
                      TextureFormat.RGBA32, true, false);
    //Apply setting
    texture2DArray.filterMode = FilterMode.Bilinear;
    texture2DArray.wrapMode = TextureWrapMode.Repeat;
    //Loop through ordinary textures and copy pixels to the Texture2DArray
    for(int i=0; i<ordinaryTextures.Length; i++)
    {
        texture2DArray.SetPixels(ordinaryTextures[i].GetPixels(0), i, 0);
    }
    //Apply our changes
    texture2DArray.Apply();
}
void Start()
{
    CreateTextureArray();
}
void Update()
{
    Shader.SetGlobalTexture("_CurLayer", currentLayer);
    //和设置Texture2D一样
    Shader.SetGlobalTexture("_MultiTextures", texture2DArray);
}
```

