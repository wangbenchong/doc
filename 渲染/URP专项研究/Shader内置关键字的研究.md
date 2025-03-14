# 前言

最近发现Shader中有一些特殊的宏，比如一句`#pragma multi_compile _ _ADDITIONAL_LIGHTS`就可以打开`_ADDITIONAL_LIGHTS`宏，但是`#pragma multi_compile _ _WBC`还要在C#调用EnableKeyword函数后才会打开 `_WBC` 宏。

很好奇这个_ADDITIONAL_LIGHTS是如何做到的，结果后来查看urp文档发现它是内置关键字，与自定义宏（如 _WBC）是生来不同的。

[Class ShaderKeywordStrings | Universal RP | 14.0.11 (unity3d.com)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/api/UnityEngine.Rendering.Universal.ShaderKeywordStrings.html)

# UnityEngine.Rendering.Universal.ShaderKeywordStrings

根据文档我查了C#源代码，发现这个类UnityEngine.Rendering.Universal.ShaderKeywordStrings里面都是关键字变量，对应内置脚本UniversalRenderPipelineCore.cs:

```c#
UnityEngine.Rendering.BuiltinShaderDefinepublic static class ShaderKeywordStrings
{
    public const string MainLightShadows = "_MAIN_LIGHT_SHADOWS";
    public const string MainLightShadowCascades = "_MAIN_LIGHT_SHADOWS_CASCADE";
    public const string MainLightShadowScreen = "_MAIN_LIGHT_SHADOWS_SCREEN";
    public const string CastingPunctualLightShadow = "_CASTING_PUNCTUAL_LIGHT_SHADOW"; // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
    public const string AdditionalLightsVertex = "_ADDITIONAL_LIGHTS_VERTEX";
    public const string AdditionalLightsPixel = "_ADDITIONAL_LIGHTS";
    internal const string ClusteredRendering = "_CLUSTERED_RENDERING";
    public const string AdditionalLightShadows = "_ADDITIONAL_LIGHT_SHADOWS";
    public const string ReflectionProbeBoxProjection = "_REFLECTION_PROBE_BOX_PROJECTION";
    public const string ReflectionProbeBlending = "_REFLECTION_PROBE_BLENDING";
    public const string SoftShadows = "_SHADOWS_SOFT";
    public const string MixedLightingSubtractive = "_MIXED_LIGHTING_SUBTRACTIVE"; // Backward compatibility
    public const string LightmapShadowMixing = "LIGHTMAP_SHADOW_MIXING";
    public const string ShadowsShadowMask = "SHADOWS_SHADOWMASK";
    public const string LightLayers = "_LIGHT_LAYERS";
    public const string RenderPassEnabled = "_RENDER_PASS_ENABLED";
    public const string BillboardFaceCameraPos = "BILLBOARD_FACE_CAMERA_POS";
    public const string LightCookies = "_LIGHT_COOKIES";

    public const string DepthNoMsaa = "_DEPTH_NO_MSAA";
    public const string DepthMsaa2 = "_DEPTH_MSAA_2";
    public const string DepthMsaa4 = "_DEPTH_MSAA_4";
    public const string DepthMsaa8 = "_DEPTH_MSAA_8";

    public const string LinearToSRGBConversion = "_LINEAR_TO_SRGB_CONVERSION";
    internal const string UseFastSRGBLinearConversion = "_USE_FAST_SRGB_LINEAR_CONVERSION";

    public const string DBufferMRT1 = "_DBUFFER_MRT1";
    public const string DBufferMRT2 = "_DBUFFER_MRT2";
    public const string DBufferMRT3 = "_DBUFFER_MRT3";
    public const string DecalNormalBlendLow = "_DECAL_NORMAL_BLEND_LOW";
    public const string DecalNormalBlendMedium = "_DECAL_NORMAL_BLEND_MEDIUM";
    public const string DecalNormalBlendHigh = "_DECAL_NORMAL_BLEND_HIGH";

    public const string SmaaLow = "_SMAA_PRESET_LOW";
    public const string SmaaMedium = "_SMAA_PRESET_MEDIUM";
    public const string SmaaHigh = "_SMAA_PRESET_HIGH";
    public const string PaniniGeneric = "_GENERIC";
    public const string PaniniUnitDistance = "_UNIT_DISTANCE";
    public const string BloomLQ = "_BLOOM_LQ";
    public const string BloomHQ = "_BLOOM_HQ";
    public const string BloomLQDirt = "_BLOOM_LQ_DIRT";
    public const string BloomHQDirt = "_BLOOM_HQ_DIRT";
    public const string UseRGBM = "_USE_RGBM";
    public const string Distortion = "_DISTORTION";
    public const string ChromaticAberration = "_CHROMATIC_ABERRATION";
    public const string HDRGrading = "_HDR_GRADING";
    public const string TonemapACES = "_TONEMAP_ACES";
    public const string TonemapNeutral = "_TONEMAP_NEUTRAL";
    public const string FilmGrain = "_FILM_GRAIN";
    public const string Fxaa = "_FXAA";
    public const string Dithering = "_DITHERING";
    public const string ScreenSpaceOcclusion = "_SCREEN_SPACE_OCCLUSION";
    public const string PointSampling = "_POINT_SAMPLING";
    public const string Rcas = "_RCAS";
    public const string Gamma20 = "_GAMMA_20";

    public const string HighQualitySampling = "_HIGH_QUALITY_SAMPLING";

    public const string DOWNSAMPLING_SIZE_2 = "DOWNSAMPLING_SIZE_2";
    public const string DOWNSAMPLING_SIZE_4 = "DOWNSAMPLING_SIZE_4";
    public const string DOWNSAMPLING_SIZE_8 = "DOWNSAMPLING_SIZE_8";
    public const string DOWNSAMPLING_SIZE_16 = "DOWNSAMPLING_SIZE_16";
    public const string _SPOT = "_SPOT";
    public const string _DIRECTIONAL = "_DIRECTIONAL";
    public const string _POINT = "_POINT";
    public const string _DEFERRED_STENCIL = "_DEFERRED_STENCIL";
    public const string _DEFERRED_FIRST_LIGHT = "_DEFERRED_FIRST_LIGHT";
    public const string _DEFERRED_MAIN_LIGHT = "_DEFERRED_MAIN_LIGHT";
    public const string _GBUFFER_NORMALS_OCT = "_GBUFFER_NORMALS_OCT";
    public const string _DEFERRED_MIXED_LIGHTING = "_DEFERRED_MIXED_LIGHTING";
    public const string LIGHTMAP_ON = "LIGHTMAP_ON";
    public const string DYNAMICLIGHTMAP_ON = "DYNAMICLIGHTMAP_ON";
    public const string _ALPHATEST_ON = "_ALPHATEST_ON";
    public const string DIRLIGHTMAP_COMBINED = "DIRLIGHTMAP_COMBINED";
    public const string _DETAIL_MULX2 = "_DETAIL_MULX2";
    public const string _DETAIL_SCALED = "_DETAIL_SCALED";
    public const string _CLEARCOAT = "_CLEARCOAT";
    public const string _CLEARCOATMAP = "_CLEARCOATMAP";
    public const string DEBUG_DISPLAY = "DEBUG_DISPLAY";

    public const string _EMISSION = "_EMISSION";
    public const string _RECEIVE_SHADOWS_OFF = "_RECEIVE_SHADOWS_OFF";
    public const string _SURFACE_TYPE_TRANSPARENT = "_SURFACE_TYPE_TRANSPARENT";
    public const string _ALPHAPREMULTIPLY_ON = "_ALPHAPREMULTIPLY_ON";
    public const string _ALPHAMODULATE_ON = "_ALPHAMODULATE_ON";
    public const string _NORMALMAP = "_NORMALMAP";

    public const string EDITOR_VISUALIZATION = "EDITOR_VISUALIZATION";

    // XR
    public const string UseDrawProcedural = "_USE_DRAW_PROCEDURAL";
}




internal static class ShaderPropertyId
{
    public static readonly int glossyEnvironmentColor = Shader.PropertyToID("_GlossyEnvironmentColor");
    public static readonly int subtractiveShadowColor = Shader.PropertyToID("_SubtractiveShadowColor");

    public static readonly int glossyEnvironmentCubeMap = Shader.PropertyToID("_GlossyEnvironmentCubeMap");
    public static readonly int glossyEnvironmentCubeMapHDR = Shader.PropertyToID("_GlossyEnvironmentCubeMap_HDR");

    public static readonly int ambientSkyColor = Shader.PropertyToID("unity_AmbientSky");
    public static readonly int ambientEquatorColor = Shader.PropertyToID("unity_AmbientEquator");
    public static readonly int ambientGroundColor = Shader.PropertyToID("unity_AmbientGround");

    public static readonly int time = Shader.PropertyToID("_Time");
    public static readonly int sinTime = Shader.PropertyToID("_SinTime");
    public static readonly int cosTime = Shader.PropertyToID("_CosTime");
    public static readonly int deltaTime = Shader.PropertyToID("unity_DeltaTime");
    public static readonly int timeParameters = Shader.PropertyToID("_TimeParameters");

    public static readonly int scaledScreenParams = Shader.PropertyToID("_ScaledScreenParams");
    public static readonly int worldSpaceCameraPos = Shader.PropertyToID("_WorldSpaceCameraPos");
    public static readonly int screenParams = Shader.PropertyToID("_ScreenParams");
    public static readonly int projectionParams = Shader.PropertyToID("_ProjectionParams");
    public static readonly int zBufferParams = Shader.PropertyToID("_ZBufferParams");
    public static readonly int orthoParams = Shader.PropertyToID("unity_OrthoParams");
    public static readonly int globalMipBias = Shader.PropertyToID("_GlobalMipBias");

    public static readonly int screenSize = Shader.PropertyToID("_ScreenSize");

    public static readonly int viewMatrix = Shader.PropertyToID("unity_MatrixV");
    public static readonly int projectionMatrix = Shader.PropertyToID("glstate_matrix_projection");
    public static readonly int viewAndProjectionMatrix = Shader.PropertyToID("unity_MatrixVP");

    public static readonly int inverseViewMatrix = Shader.PropertyToID("unity_MatrixInvV");
    public static readonly int inverseProjectionMatrix = Shader.PropertyToID("unity_MatrixInvP");
    public static readonly int inverseViewAndProjectionMatrix = Shader.PropertyToID("unity_MatrixInvVP");

    public static readonly int cameraProjectionMatrix = Shader.PropertyToID("unity_CameraProjection");
    public static readonly int inverseCameraProjectionMatrix = Shader.PropertyToID("unity_CameraInvProjection");
    public static readonly int worldToCameraMatrix = Shader.PropertyToID("unity_WorldToCamera");
    public static readonly int cameraToWorldMatrix = Shader.PropertyToID("unity_CameraToWorld");

    public static readonly int cameraWorldClipPlanes = Shader.PropertyToID("unity_CameraWorldClipPlanes");

    public static readonly int billboardNormal = Shader.PropertyToID("unity_BillboardNormal");
    public static readonly int billboardTangent = Shader.PropertyToID("unity_BillboardTangent");
    public static readonly int billboardCameraParams = Shader.PropertyToID("unity_BillboardCameraParams");

    public static readonly int sourceTex = Shader.PropertyToID("_SourceTex");
    public static readonly int scaleBias = Shader.PropertyToID("_ScaleBias");
    public static readonly int scaleBiasRt = Shader.PropertyToID("_ScaleBiasRt");

    // Required for 2D Unlit Shadergraph master node as it doesn't currently support hidden properties.
    public static readonly int rendererColor = Shader.PropertyToID("_RendererColor");
}
```

# UnityEngine.Rendering.BuiltinShaderDefine

BuiltinShaderDefine.cs

```c#
#nullable disable
namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Defines set by editor when compiling shaders, based on the target platform and GraphicsTier.</para>
  /// </summary>
  public enum BuiltinShaderDefine
  {
    /// <summary>
    ///   <para>UNITY_NO_DXT5nm is set when compiling shader for platform that do not support DXT5NM, meaning that normal maps will be encoded in RGB instead.</para>
    /// </summary>
    UNITY_NO_DXT5nm,
    /// <summary>
    ///   <para>UNITY_NO_RGBM is set when compiling shader for platform that do not support RGBM, so dLDR will be used instead.</para>
    /// </summary>
    UNITY_NO_RGBM,
    UNITY_USE_NATIVE_HDR,
    /// <summary>
    ///   <para>UNITY_ENABLE_REFLECTION_BUFFERS is set when deferred shading renders reflection probes in deferred mode. With this option set reflections are rendered into a per-pixel buffer. This is similar to the way lights are rendered into a per-pixel buffer. UNITY_ENABLE_REFLECTION_BUFFERS is on by default when using deferred shading, but you can turn it off by setting “No support” for the Deferred Reflections shader option in Graphics Settings. When the setting is off, reflection probes are rendered per-object, similar to the way forward rendering works.</para>
    /// </summary>
    UNITY_ENABLE_REFLECTION_BUFFERS,
    /// <summary>
    ///   <para>UNITY_FRAMEBUFFER_FETCH_AVAILABLE is set when compiling shaders for platforms where framebuffer fetch is potentially available.</para>
    /// </summary>
    UNITY_FRAMEBUFFER_FETCH_AVAILABLE,
    /// <summary>
    ///   <para>UNITY_ENABLE_NATIVE_SHADOW_LOOKUPS enables use of built-in shadow comparison samplers on OpenGL ES 2.0.</para>
    /// </summary>
    UNITY_ENABLE_NATIVE_SHADOW_LOOKUPS,
    /// <summary>
    ///   <para>UNITY_METAL_SHADOWS_USE_POINT_FILTERING is set if shadow sampler should use point filtering on iOS Metal.</para>
    /// </summary>
    UNITY_METAL_SHADOWS_USE_POINT_FILTERING,
    UNITY_NO_CUBEMAP_ARRAY,
    /// <summary>
    ///   <para>UNITY_NO_SCREENSPACE_SHADOWS is set when screenspace cascaded shadow maps are disabled.</para>
    /// </summary>
    UNITY_NO_SCREENSPACE_SHADOWS,
    /// <summary>
    ///   <para>UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS is set when Semitransparent Shadows are enabled.</para>
    /// </summary>
    UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS,
    /// <summary>
    ///   <para>UNITY_PBS_USE_BRDF1 is set if Standard Shader BRDF1 should be used.</para>
    /// </summary>
    UNITY_PBS_USE_BRDF1,
    /// <summary>
    ///   <para>UNITY_PBS_USE_BRDF2 is set if Standard Shader BRDF2 should be used.</para>
    /// </summary>
    UNITY_PBS_USE_BRDF2,
    /// <summary>
    ///   <para>UNITY_PBS_USE_BRDF3 is set if Standard Shader BRDF3 should be used.</para>
    /// </summary>
    UNITY_PBS_USE_BRDF3,
    /// <summary>
    ///   <para>UNITY_NO_FULL_STANDARD_SHADER is set if Standard shader BRDF3 with extra simplifications should be used.</para>
    /// </summary>
    UNITY_NO_FULL_STANDARD_SHADER,
    /// <summary>
    ///   <para>UNITY_SPECCUBE_BLENDING is set if Reflection Probes Box Projection is enabled.</para>
    /// </summary>
    UNITY_SPECCUBE_BOX_PROJECTION,
    /// <summary>
    ///   <para>UNITY_SPECCUBE_BLENDING is set if Reflection Probes Blending is enabled.</para>
    /// </summary>
    UNITY_SPECCUBE_BLENDING,
    /// <summary>
    ///   <para>UNITY_ENABLE_DETAIL_NORMALMAP is set if Detail Normal Map should be sampled if assigned.</para>
    /// </summary>
    UNITY_ENABLE_DETAIL_NORMALMAP,
    /// <summary>
    ///   <para>SHADER_API_MOBILE is set when compiling shader for mobile platforms.</para>
    /// </summary>
    SHADER_API_MOBILE,
    /// <summary>
    ///   <para>SHADER_API_DESKTOP is set when compiling shader for "desktop" platforms.</para>
    /// </summary>
    SHADER_API_DESKTOP,
    /// <summary>
    ///   <para>UNITY_HARDWARE_TIER1 is set when compiling shaders for GraphicsTier.Tier1.</para>
    /// </summary>
    UNITY_HARDWARE_TIER1,
    /// <summary>
    ///   <para>UNITY_HARDWARE_TIER2 is set when compiling shaders for GraphicsTier.Tier2.</para>
    /// </summary>
    UNITY_HARDWARE_TIER2,
    /// <summary>
    ///   <para>UNITY_HARDWARE_TIER3 is set when compiling shaders for GraphicsTier.Tier3.</para>
    /// </summary>
    UNITY_HARDWARE_TIER3,
    /// <summary>
    ///   <para>UNITY_COLORSPACE_GAMMA is set when compiling shaders for Gamma Color Space.</para>
    /// </summary>
    UNITY_COLORSPACE_GAMMA,
    /// <summary>
    ///   <para>UNITY_LIGHT_PROBE_PROXY_VOLUME is set when Light Probe Proxy Volume feature is supported by the current graphics API and is enabled in the. You can only set a Graphics Tier in the Built-in Render Pipeline.</para>
    /// </summary>
    UNITY_LIGHT_PROBE_PROXY_VOLUME,
    /// <summary>
    ///   <para>UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS is set automatically for platforms that don't require full floating-point precision support in fragment shaders.</para>
    /// </summary>
    UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS,
    /// <summary>
    ///   <para>UNITY_LIGHTMAP_DLDR_ENCODING is set when lightmap textures are using double LDR encoding to store the values in the texture.</para>
    /// </summary>
    UNITY_LIGHTMAP_DLDR_ENCODING,
    /// <summary>
    ///   <para>UNITY_LIGHTMAP_RGBM_ENCODING is set when lightmap textures are using RGBM encoding to store the values in the texture.</para>
    /// </summary>
    UNITY_LIGHTMAP_RGBM_ENCODING,
    /// <summary>
    ///   <para>UNITY_LIGHTMAP_FULL_HDR is set when lightmap textures are not using any encoding to store the values in the texture.</para>
    /// </summary>
    UNITY_LIGHTMAP_FULL_HDR,
    /// <summary>
    ///   <para>Is virtual texturing enabled and supported on this platform.</para>
    /// </summary>
    UNITY_VIRTUAL_TEXTURING,
    /// <summary>
    ///   <para>Unity enables UNITY_PRETRANSFORM_TO_DISPLAY_ORIENTATION when Vulkan pre-transform is enabled and supported on the target build platform.</para>
    /// </summary>
    UNITY_PRETRANSFORM_TO_DISPLAY_ORIENTATION,
    /// <summary>
    ///   <para>Unity enables UNITY_ASTC_NORMALMAP_ENCODING when DXT5nm-style normal maps are used on Android, iOS or tvOS.</para>
    /// </summary>
    UNITY_ASTC_NORMALMAP_ENCODING,
    /// <summary>
    ///   <para>SHADER_API_ES30 is set when the Graphics API is OpenGL ES 3 and the minimum supported OpenGL ES 3 version is OpenGL ES 3.0.</para>
    /// </summary>
    SHADER_API_GLES30,
    /// <summary>
    ///   <para>Unity sets UNITY_UNIFIED_SHADER_PRECISION_MODEL if, in Player Settings, you set Shader Precision Model to Unified.</para>
    /// </summary>
    UNITY_UNIFIED_SHADER_PRECISION_MODEL,
  }
}
```

# Shader变量的后缀和前缀

- _MainTex_TexelSize 里的 _TexelSize 后缀


是贴图 _MainTex 的像素尺寸大小，值： Vector4(1 / width, 1 / height, width, height)
 half2 offs = _MainTex_TexelSize.xy * half2(1,0) *  _BlurSize;

- _MainTex_ST 里的 _ST 后缀

是贴图_MainTex的tiling和offset的四元数
_MainTex_ST.xy 是tiling的值
_MainTex_ST.zw 是offset的值
代码如：_

```c
_// Transforms 2D UV by scale/bias property, ##是连接的意思
#define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
```



- sample_前缀

  TEXTURE2D(_SelfTexture2D);
  float4 _SelfTexture2D_TexelSize;
  SAMPLER(sampler_SelfTexture2D);
