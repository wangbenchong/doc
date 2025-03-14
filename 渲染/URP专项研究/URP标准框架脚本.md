本文是根据优梦创客的《URP从0到1训练营》系列视频整理的模板代码

# 模板Unlit shader脚本

文件名为URPUnlitTemp.shader

```c
Shader "URPFramework/#NAME#"
{
    Properties{
		_BaseMap("Base Map" , 2D) = "white" {}
		_BaseColor( "Base Color" , Color ) = ( 1, 1, 1, 1 )
		[Toggle(_ALPHATEST_ON)] _AlphaTestToggle("Alpha Clipping" , Float) = 0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		[Enum(Off,0,Front,1,Back,2)]_Cull("Cull Mode", Float) = 2.0
    }
    SubShader{
		Tags{
			 "RenderPipeline" = "UniversalPipeline" 
			 "RenderType" = "Opaque"
			 "Queue" = "Geometry"
		}

		HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			CBUFFER_START(UnityPerMaterial)
			float4 _BaseMap_ST;
			float4 _BaseColor;
			float _Cutoff;
			CBUFFER_END
		ENDHLSL

        pass{Name "Unlit"
			Cull [_Cull]
			HLSLPROGRAM
			#pragma vertex vert
            #pragma fragment frag

			#pragma shader_feature _ALPHATEST_ON

			//贴图变量，防止和其他Pass的贴图变量冲突
			CBUFFER_START(UnityPerMaterial)
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);
			CBUFFER_END

			struct appdata
			{
				float3 pos : POSITION;
				float2 uv : TEXCOORD0;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
                /*常与#pragma multi_compile_instancing指令一起使用，后者告诉Unity此shader支持实例化渲染。*/
			};
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
                 //UNITY_VERTEX_INPUT_INSTANCE_ID
                 //UNITY_VERTEX_OUTPUT_STEREO
                /*另一个Unity的宏，用于支持立体渲染（stereoscopic rendering）。它可能会添加一些额外的字段来支持左右眼视图。*/
			};
			
			v2f vert(appdata IN)
			{
				v2f OUT = (v2f)0;
				OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
				VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.pos);
				OUT.pos = vertexInput.positionCS;
				return OUT;
			}
			
			half4 frag(v2f IN) : SV_TARGET
			{
				half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
				#if _ALPHATEST_ON
				clip(baseMap.a - _Cutoff);
				#endif
				half4 color = baseMap * _BaseColor;
				return color;
			}
			ENDHLSL
		}
		//阴影，忽略SRP Batcher的话也可以改用
		//UsePass "Universal Render Pipeline/ShadowCaster"
		pass{Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
			ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
			//-------材质相关&每个Pass都要---------
            #pragma shader_feature _ALPHATEST_ON
			#pragma multi_compile_instancing// GPU Instancing
			//----------阴影相关（仅这个Pass）--------
			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW//支持局部光（点光源和聚光灯）
            //--------------------------------------

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
		//开启Depth Priming的情况必须写这个Pass否则看不到材质，但在开启SSAO的情况，依然会看不到材质，还需写其他Pass
		pass{Name "DepthOnly"
			Tags{"LightMode" = "DepthOnly"}
			ZWrite On
            ZTest LEqual
			ColorMask 0

            HLSLPROGRAM
			//-------材质相关&每个Pass都要---------
            #pragma shader_feature _ALPHATEST_ON
			#pragma multi_compile_instancing// GPU Instancing
			//--------------------------------------

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
		}
		//支持SSAO, MSAA,等等
		pass{Name "DepthNormals"
			Tags{"LightMode" = "DepthNormals"}
			ZWrite On
            ZTest LEqual
			Cull[_Cull]

            HLSLPROGRAM
			//-------材质相关-----
			#pragma shader_feature_local _NORMAL_MAP
			//-------材质相关&每个Pass都要---------
            #pragma shader_feature _ALPHATEST_ON
			#pragma multi_compile_instancing// GPU Instancing
			//--------------------------------------

            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
			ENDHLSL
		}
    }
	FallBack "Hidden/Universal Render Pipeline/FallbackError" //填写故障情况下的最保守shader的pass
}
```



# 通过C#编辑器脚本创建

```c#
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
 
public class CreateURPShader
{
    public const string shaderTemplateUnlitPath = "Assets/CustomShaderGUI/Editor/Template/URPUnlitTemp.shader";

    [MenuItem("Assets/Create/Shader/URP Shader Unlit")]
    public static void CreatURPShaderUnlitTemplate()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
        ScriptableObject.CreateInstance<URPShadertAsset>(),
        GetSelectedPathOrFallback() + "/Unlit.shader",
        null,
       shaderTemplateUnlitPath);
    }

    //获取选择的路径
    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}    
 
 
class URPShadertAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }
    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();//读取模板内容
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
        text = Regex.Replace(text, "#NAME#", fileNameWithoutExtension);//将模板的#NAME# 替换成文件名

        //写入文件，并导入资源
        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
}  
```

#  全功能支持PBR光照shader脚本

```c
Shader "URPFramework/SimpleLit"
{
    Properties
    {
		_BaseMap("Base Map" , 2D) = "white" {}
		_BaseColor( "Base Color" , Color ) = ( 1, 1, 1, 1 )
		//Alpha测试    	
		[Toggle(_ALPHATEST_ON)] _AlphaTestToggle("Open Alpha Clipping" , Float) = 0
		//Alpha测试阈值
    	_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		//剔除模式  	
		[Enum(Off,0,Front,1,Back,2)]_Cull("Cull Mode", Float) = 2.0
		//光照贴图  	
    	[Toggle(LIGHTMAP_ON)] _LightMapToggle("Use Light Map", Float) = 1
    	//法线贴图
		[Toggle(_NORMALMAP)]_NormalMapToggle("Use Normal Map", Float) = 0
		[NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
    	//自发光
		[Toggle(_EMISSION)]_EmissionToggle("Open Emission", Float) = 0
		[HDR]_EmissionColor("Emission Color", Color) = (0, 0, 0, 1)//自发光颜色
		[NoscaleOffset]_EmissionMap("Emission Map", 2D) = "white" {}
    	
    	//非PBR，使用Blinnphong处理光照
    	[Header(BlinnPhong without PBR)]
		[Toggle(_SPECGLOSSMAP)]_SpecGlossMapToggle("Use Specular Gloss Map", Float) = 0
    	
		[Header(Common info of BlinnPhong and PBR)]
    	//SurfaceInput.hlsl, 光泽度的来源，从基础纹理的Alpha值(如果开启)或者从Specular(关闭)
		[Toggle(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)]_GlossSource("Glossiness Source: On(from Albedo Alpha) / Off(from Specular)", Float) = 0
    	_SpecColor( "Specular Color" , Color ) = ( 0.5, 0.5, 0.5, 0.5 )
    	_MetallicSpecGlossMap("Metallic/Specular Gloss Map", 2D) = "white" {}
		//平滑度
    	_Smoothness("Smooth", Range(0.0, 1.0)) = 0.5
    	//----------------------------------------------------
    	[Header(About PBR)]
    	[Toggle(_USE_PBR)]_USE_PBR("Open PBR", Float) = 0
    	[Toggle(_SPECULAR_SETUP)] _MetallicSetup("Work flow: On(Specular) / Off(Metallic)", Float) = 0
    	[Toggle(_METALLICSPECGLOSSMAP)]_MetallicSpecGlossMapToggle("Use Metallic/Specular Gloss Map", Float) = 0
    	_Metallic("Metallic", Range(0,1)) = 0
    	//环境遮蔽
    	[Header(Occlusion in PBR)]
    	[Toggle(_OCCLUSIONMAP)] _OcclusionMapToggle("Use Occlusion Mapping", Float) = 0
    	[NoScaleOffset] _OcclusionMap("Occlusion Map", 2D) = "white" {}
    	_OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0
    	//----------------------------------------------------
    	[Header(Turn Off Functions)]
    	[Toggle(_SPECULARHIGHLIGHTS_OFF)]_SpecularHighlightsOff("Turn Off Specular Highlights", Float) = 0
    	[Toggle(_ENVIRONMENTREFLECTIONS_OFF)]_EnvironmentReflectionsOff("Turn Off Environment Reflections", Float) = 0
    	[Toggle(_RECEIVE_SHADOWS_OFF)]_ReceiveShadowsOff("Turn Off Receive Shadows", Float) = 0
    }
    SubShader
    {
		Tags
		{
			 "RenderPipeline" = "UniversalPipeline" 
			 "RenderType" = "Opaque"
			 "Queue" = "Geometry"
		}

		HLSLINCLUDE
			//包含Core、SurfaceData、Packing、CommonMaterial
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			CBUFFER_START(UnityPerMaterial)
			float4 _BaseMap_ST;
			half4 _BaseColor;
			float _Cutoff;
			float4 _EmissionColor;
			float4 _SpecColor;
			float _Metallic;
			float _Smoothness;
			float _OcclusionStrength;
			CBUFFER_END
		ENDHLSL

        pass
        {
        	Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}
			Cull [_Cull]
			HLSLPROGRAM
			//包含BRDF、Debugging3D、GlobalIllumination、RealtimeLights、AmbientOcclusion、DBuffer
			//RealtimeLights又包含AmbientOcclusion、Input、Shadows、LightCookie、Clustering
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#pragma vertex SimpleLitPassVertex
            #pragma fragment SimpleLitPassFragment
			
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _EMISSION
			//使用了_local_fragment后缀。这通常意味着这个特性只在片段着色器（Fragment Shader）中有效
			#pragma shader_feature_local_fragment _SPECGLOSSMAP//BinnPhong
			#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP//PBR
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature _OCCLUSIONMAP
			#pragma shader_feature _SPECULAR_SETUP
			#pragma shader_feature_local_fragment _USE_PBR//PBR
			#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
			#pragma shader_feature_local_fragment _RECEIVE_SHADOWS_OFF
			
			#ifndef _USE_PBR
				#define _SPECULAR_COLOR// 总是打开高光颜色, BlinnPhong
			#endif
			
			//Lighting.hlsl,打开光照贴图开关，否则无法使用Lightmap，比如自己和地面都设置成static，烘焙后自己会有地面的反光色
			#pragma multi_compile _ LIGHTMAP_ON
			//宏逻辑见Shadows.hlsl，开启阴影,一旦定义_RECEIVE_SHADOWS_OFF就失效了
			#pragma multi_compile _MAIN_LIGHT_SHADOWS//主光源阴影,如果前面插个_,默认不开
			#pragma multi_compile _MAIN_LIGHT_SHADOWS_CASCADE//主光源阴影级联贴图，根据光源位置决定贴图索引,写了就开
			#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION//动态屏幕空间遮挡，写了就开
			#pragma multi_compile_fragment _ _SHADOWS_SOFT//阴影抗锯齿，写了就开
			//这两个写了就打开，是系统内置宏
			#pragma multi_compile _ADDITIONAL_LIGHT_SHADOWS //多光源阴影,写了就开
			#pragma multi_compile _ADDITIONAL_LIGHTS//多光源,可以把阴影局部照亮,lighting.hlsl,写了就开
			
			TEXTURE2D(_MetallicSpecGlossMap);SAMPLER(sampler_MetallicSpecGlossMap);
			TEXTURE2D(_OcclusionMap);SAMPLER(sampler_OcclusionMap);

			struct Attributes
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;

				#ifdef _NORMALMAP
					float4 tangentOS : TANGENT;
				#endif

				float2 uv : TEXCOORD0;
				float2 lightmapUV : TEXCOORD1;
			};
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				//lighting.hlsl, 宏声明，光照贴图   或  球谐信息
				DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
				float3 positionWS : TEXCOORD2;//世界空间位置
				#ifdef _NORMALMAP
					half4 normalWS : TEXCOORD3;
					half4 tangentWS : TEXCOORD4;
					half4 bitangentWS : TEXCOORD5;
				#else
					float3 normalWS : TEXCOORD3;//世界空间法线
				#endif

				#ifdef _ADDITIONAL_LIGHTS_VERTEX
					half4 fogFactorAndVertexLight : TEXCOORD6;//x:雾效，yzw:顶点光照
				#else
					half fogFactor : TEXCOORD6;
				#endif

				#ifdef 	REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
					float4 shadowCoord : TEXCOORD7;
				#endif
			};
			
			Varyings SimpleLitPassVertex(Attributes IN)
			{
				Varyings OUT = (Varyings)0;
				VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS);
				OUT.positionCS = positionInputs.positionCS;
				OUT.positionWS = positionInputs.positionWS;
				
				VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS
					#ifdef _NORMALMAP
					, IN.tangentOS
					#endif
				);
				
				#ifdef _NORMALMAP
					half3 viewDir = GetWorldSpaceViewDir(positionInputs.positionWS);
					OUT.normalWS = half4(normalInputs.normalWS, viewDir.x);
					OUT.tangentWS = half4(normalInputs.tangentWS, viewDir.y);
					OUT.bitangentWS = half4(normalInputs.bitangentWS, viewDir.z);
				#else
					OUT.normalWS = NormalizeNormalPerVertex(normalInputs.normalWS);
				#endif

				half fogFactor = ComputeFogFactor(positionInputs.positionCS.z);
				#ifdef _ADDITIONAL_LIGHTS_VERTEX
					half3 vertexLight = VertexLighting(positionInputs.positionWS, normalInputs.normalWS);
					//雾效和顶点光放在一个字段里
					OUT.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				#else
					OUT.fogFactor = fogFactor;
				#endif

				#ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
					OUT.shadowCoord = GetShadowCoord(positionInputs);
				#endif

				//都是Lighting.hlsl里的宏函数
				//定义LIGHTMAP_ON就做事
				OUTPUT_LIGHTMAP_UV(IN.lightmapUV, unity_LightmapST, OUT.lightmapUV);
				//未定义LIGHTMAP_ON就做事
				OUTPUT_SH(OUT.normalWS, OUT.vertexSH);//球谐函数（Spherical Harmonics）,结果存到OUT.vertexSH

				OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
				return OUT;
			}
			//TEXTURE2D_PARAM是定义贴图和采样器的简写， BlinnPhong
			half4 SampleSpecularSmoothness(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
			{
				half4 specularSmoothness = half4(0, 0, 0, 1);
				#ifdef _SPECGLOSSMAP
				specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
				#elif defined(_SPECULAR_COLOR)
				specularSmoothness = specColor;
				#endif

				#ifdef _GLOSSINESS_FROM_BASE_ALPHA
				specularSmoothness.a = alpha;
				#endif

				return specularSmoothness;
			}
			//PBR
			half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
			{
			    half4 specGloss;

				#ifdef _METALLICSPECGLOSSMAP
				    specGloss = half4(SAMPLE_TEXTURE2D(_MetallicSpecGlossMap, sampler_MetallicSpecGlossMap, uv));
				    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
				        specGloss.a = albedoAlpha * _Smoothness;
				    #else
				        specGloss.a *= _Smoothness;
				    #endif
				#else // _METALLICSPECGLOSSMAP
				    #if _SPECULAR_SETUP
				        specGloss.rgb = _SpecColor.rgb;
				    #else
				        specGloss.rgb = _Metallic.rrr;
				    #endif

				    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
				        specGloss.a = albedoAlpha * _Smoothness;
				    #else
				        specGloss.a = _Smoothness;
				    #endif
				#endif

			    return specGloss;
			}
			half SampleOcclusion(float2 uv)
			{
				#ifndef _USE_PBR
					return 1.0;
				#endif
				#ifdef _OCCLUSIONMAP
					#if defined(SHADER_API_GLES)
						return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
					#else
						half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
						return LerpWhiteTo(occ, _OcclusionStrength);
					#endif
				#endif
				return 1.0;				
			}
			void InitSurfaceData(Varyings IN, out SurfaceData surfaceData)
			{
				surfaceData = (SurfaceData)0;

				half4 albedoAlpha = SampleAlbedoAlpha(IN.uv, _BaseMap, sampler_BaseMap);
				
				half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
				
				surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor.a, _Cutoff);
				surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
				surfaceData.normalTS = SampleNormal(IN.uv, _BumpMap, sampler_BumpMap);
				surfaceData.emission = SampleEmission(IN.uv, _EmissionColor.rgb, _EmissionMap, sampler_EmissionMap);
				surfaceData.occlusion = SampleOcclusion(IN.uv);

				#ifndef _USE_PBR//BlinnPhong
					half4 specular = SampleSpecularSmoothness(IN.uv, baseMap.a, _SpecColor, _MetallicSpecGlossMap, sampler_MetallicSpecGlossMap);
					surfaceData.specular = specular.rgb;
					surfaceData.smoothness = specular.a * _Smoothness;
				#else//PBR
					half4 specGloss = SampleMetallicSpecGloss(IN.uv, albedoAlpha.a);
					#if defined(_SPECULAR_SETUP)//高光
						surfaceData.metallic = 1.0h;
						surfaceData.specular = specGloss.rgb;
					#else//金属
						surfaceData.metallic = specGloss.r;
						surfaceData.specular = half3(0.0h, 0.0h, 0.0h);
					#endif
					surfaceData.smoothness = specGloss.a;
				#endif
			}
			void InitInputData(Varyings IN, half3 normalTS, out InputData inputData)
			{
				inputData = (InputData)0;
				inputData.positionWS = IN.positionWS;
				#ifdef _NORMALMAP
					inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(IN.tangentWS.xyz, IN.bitangentWS.xyz, IN.normalWS.xyz));
					half3 viewDirWS = half3(IN.normalWS.w, IN.tangentWS.w, IN.bitangentWS.w);
				#else
					inputData.normalWS = IN.normalWS;
					half3 viewDirWS = GetWorldSpaceViewDir(IN.positionWS);
				#endif
				//这个函数标准化更安全，考虑了除0的情况
				inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				viewDirWS = SafeNormalize(viewDirWS);
				inputData.viewDirectionWS = viewDirWS;
				#ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
					//顶点着色器中已经计算好的阴影坐标
					inputData.shadowCoord = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)//主灯计算阴影
					inputData.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
				#else
					inputData.shadowCoord = float4(0, 0, 0, 0);
				#endif
				//如果有额外灯
				#ifdef _ADDITIONAL_LIGHTS_VERTEX
					//顶点光
					inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
					//雾效信息
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#else
					inputData.vertexLighting = half3(0, 0, 0);
					inputData.fogCoord = IN.fogFactor.x;
				#endif
				//烘焙GI信息
				inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.vertexSH, inputData.normalWS);
				//屏幕空间UV
				inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);
				//阴影遮罩信息，shadowMask贴图
				inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);
			}
			half4 SimpleLitPassFragment(Varyings IN) : SV_TARGET
			{
				SurfaceData surfaceData;
				InitSurfaceData(IN, surfaceData);
				InputData inputData;
				InitInputData(IN, surfaceData.normalTS, inputData);
				//利用Lighting.hlsl中的函数
				half4 color = 0;
				
				#ifdef _USE_PBR
				color = UniversalFragmentPBR(inputData, surfaceData);
				#else
				color = UniversalFragmentBlinnPhong(inputData, surfaceData);
				#endif
				
				color.rgb = MixFog(color.rgb, inputData.fogCoord);
				return color;				
			}
			ENDHLSL
		}
		//阴影，忽略SRP Batcher的话也可以改用
		//UsePass "Universal Render Pipeline/ShadowCaster"
		pass
		{
			Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
			ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
			//-------材质相关&每个Pass都要---------
            #pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A//(可选)ALBDO通道的Alpha作为平滑度
			#pragma multi_compile_instancing// GPU Instancing
			//----------阴影相关（仅这个Pass）--------
			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW//支持局部光（点光源和聚光灯）
            //--------------------------------------

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
			
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
		//开启Depth Priming的情况必须写这个Pass否则看不到材质，但在开启SSAO的情况，依然会看不到材质，还需写其他Pass
		pass
		{
			Name "DepthOnly"
			Tags{"LightMode" = "DepthOnly"}
			ZWrite On
            ZTest LEqual
			ColorMask 0

            HLSLPROGRAM
			//-------材质相关&每个Pass都要---------
            #pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A//(可选)ALBDO通道的Alpha作为平滑度
			#pragma multi_compile_instancing// GPU Instancing
			//--------------------------------------

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
			
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
		}
		//支持SSAO, MSAA传统抗锯齿（现代还有FXAA模糊、taa高性能、dlss平台局限）
		pass
		{
			Name "DepthNormals"
			Tags{"LightMode" = "DepthNormals"}
			ZWrite On
            ZTest LEqual
			Cull[_Cull]

            HLSLPROGRAM
			//-------材质相关-----
			#pragma shader_feature_local _NORMAL_MAP
			//-------材质相关&每个Pass都要---------
            #pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A//(可选)ALBDO通道的Alpha作为平滑度
			#pragma multi_compile_instancing// GPU Instancing
			//--------------------------------------

            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment
			
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
			ENDHLSL
		}
    }
	FallBack "Hidden/Universal Render Pipeline/FallbackError" //填写故障情况下的最保守shader的pass
}
```

# 查看变体工具

```c#
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

public class ShaderVariantWindow : EditorWindow
{
    private static ShaderVariantWindow editor;
    [MenuItem("Window/展示Shader变体",false,6)]
    static void ShowWindow()
    {
        editor = GetWindow<ShaderVariantWindow>();
        editor.titleContent = new GUIContent("展示Shader变体");
        editor.Show();
    }
    
    private bool isShare = true;
    private string keyword = "";
    
    [FormerlySerializedAs("OnVar")] [SerializeField]
    private List<string> OnKeys = new List<string>();
    private SerializedObject _serializedObject_OnKeys;
    private SerializedProperty _serializedProperty_OnKeys;
    [FormerlySerializedAs("OffVar")] [SerializeField]
    private List<string> OffKeys = new List<string>();
    private SerializedObject _serializedObject_OffKeys;
    private SerializedProperty _serializedProperty_OffKeys;
    private Material mat
    {
        get
        {
            if(selectMat)
            {
                return selectMat;
            }
            //在非运行情况下强制使用sharedMaterial，防止console报错
            if (isShare || !EditorApplication.isPlaying)
            {
                return m_Renderer?.sharedMaterial;
            }
            else
            {
                return m_Renderer?.material;
            }
        }
    }

    private Material selectMat;
    private Renderer m_Renderer;
    private void OnSelectionChange()
    {
        if(Selection.activeObject && Selection.activeObject is Material)
        {
            Debug.Log(Selection.activeObject.name);
            selectMat = Selection.activeObject as Material;
            m_Renderer = null;
        }
        else
        {
            selectMat = null;
            m_Renderer = Selection.activeGameObject?.GetComponent<Renderer>();
        }
        RefreshVar();
    }
    
    void OnEnable()
    {
        if (editor == null)
        {
            editor = GetWindow<ShaderVariantWindow>();
            editor.autoRepaintOnSceneChange = true;
        }
        _serializedObject_OnKeys = new SerializedObject(this);
        _serializedProperty_OnKeys = _serializedObject_OnKeys.FindProperty("OnKeys");
        _serializedObject_OffKeys = new SerializedObject(this);
        _serializedProperty_OffKeys = _serializedObject_OffKeys.FindProperty("OffKeys");
    }

    void OnGUI()
    {
        if (IsHideUI())
        {
            EditorGUILayout.HelpBox("请选择一个渲染器或材质球", MessageType.Info);
            return;
        }
        EditorGUIUtility.labelWidth = 60;
        if (EditorApplication.isPlaying)
        {
            EditorGUI.BeginChangeCheck();
            isShare = EditorGUILayout.Toggle("Is Shared Mat", isShare);
            if (EditorGUI.EndChangeCheck())
            {
                RefreshVar();
            }    
        }
        EditorGUILayout.BeginHorizontal();
        keyword = EditorGUILayout.TextField("Keyword", keyword);
        if (GUILayout.Button("On", GUILayout.Width(40)))
        {
            OnClickOnOff(true);
        }

        if (GUILayout.Button("Off", GUILayout.Width(40)))
        {
            OnClickOnOff(false);
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("Refresh"))
        {
            RefreshVar();
        }
        
        _serializedObject_OnKeys.Update();
        _serializedObject_OffKeys.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_serializedProperty_OnKeys, true);
        EditorGUILayout.PropertyField(_serializedProperty_OffKeys, true);
        
        //结束检查是否有修改
        if (EditorGUI.EndChangeCheck())
        {
            //提交修改
            _serializedObject_OnKeys.ApplyModifiedProperties();
            _serializedObject_OffKeys.ApplyModifiedProperties();
        }
        if (GUILayout.Button("Clear Off Key"))
        {
            ClearOffKey(); 
        }
    }

    private bool IsHideUI()
    {
        return !selectMat && (!m_Renderer || !m_Renderer.sharedMaterial);
    }

    private void RefreshVar()
    {
        if (IsHideUI())
        {
            return;
        }
        OnKeys.Clear();
        OffKeys.Clear();
        List<string> keywords = new List<string>();
        keywords.AddRange(mat.shaderKeywords);
        foreach (var item in keywords)
        {
            if (mat.IsKeywordEnabled(item))
                OnKeys.Add(item);
            else
                OffKeys.Add(item);
        }
    }

    private void OnClickOnOff(bool isClickOn)
    {
        if (string.IsNullOrWhiteSpace(keyword) || keyword.Contains(" "))
        {
            return;
        }
        if (isClickOn)
        {
            mat.EnableKeyword(keyword);    
        }
        else
        {
            mat.DisableKeyword(keyword);
        }
        RefreshVar();
    }

    private void ClearOffKey()
    {
        RefreshVar();
        if(OffKeys.Count == 0)
        {
            return;
        }
        List<string> keywords = new List<string>();
        keywords.AddRange(mat.shaderKeywords);
        foreach (var t in OffKeys)
        {
            keywords.Remove(t);
        }
        mat.shaderKeywords = keywords.ToArray();
        OffKeys.Clear();
    }
}
```

