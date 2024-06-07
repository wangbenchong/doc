# 模板shader脚本文件名为URPUnlitTemp.shader

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
			};
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
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
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A//(可选)ALBDO通道的Alpha作为平滑度
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
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A//(可选)ALBDO通道的Alpha作为平滑度
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
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A//(可选)ALBDO通道的Alpha作为平滑度
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

