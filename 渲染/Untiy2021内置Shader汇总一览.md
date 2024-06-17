# Core

## 根目录
### ACES

```c
half3 unity_to_ACES(half3 x)
half3 ACES_to_unity(half3 x)
half3 unity_to_ACEScg(half3 x)
half3 ACEScg_to_unity(half3 x)
half ACES_to_ACEScc(half x)
half ACES_to_ACEScc_fast(half x)
half3 ACES_to_ACEScc(half3 x)
half ACEScc_to_ACES(half x)
half3 ACEScc_to_ACES(half3 x)
float3 ACES_to_ACEScg(float3 x)
float3 ACEScg_to_ACES(float3 x)
half rgb_2_saturation(half3 rgb)
half rgb_2_yc(half3 rgb)
half rgb_2_hue(half3 rgb)
half center_hue(half hue, half centerH)
half sigmoid_shaper(half x)
half glow_fwd(half ycIn, half glowGainIn, half glowMid)
half segmented_spline_c5_fwd(half x)
half segmented_spline_c9_fwd(half x)
half3 RRT(half3 aces)
half3 Y_2_linCV(half3 Y, half Ymax, half Ymin)
half3 XYZ_2_xyY(half3 XYZ)
half3 xyY_2_XYZ(half3 xyY)
float3 darkSurround_to_dimSurround(float3 linearCV)
half moncurve_r(half y, half gamma, half offs)
half bt1886_r(half L, half gamma, half Lw, half Lb)
half roll_white_fwd(
    half x,       // color value to adjust (white scaled to around 1.0)
    half new_wht, // white adjustment (e.g. 0.9 for 10% darkening)
    half width    // adjusted width (e.g. 0.25 for top quarter of the tone scale)
    )
half3 linear_to_bt1886(half3 x, half gamma, half Lw, half Lb)
half3 ODT_RGBmonitor_100nits_dim(half3 oces)
half3 ODT_RGBmonitor_D60sim_100nits_dim(half3 oces)
half3 ODT_Rec709_100nits_dim(half3 oces)
half3 ODT_Rec709_D60sim_100nits_dim(half3 oces)
half3 ODT_Rec2020_100nits_dim(half3 oces)
half3 ODT_P3DCI_48nits(half3 oces)
```



### AreaLighting

```c
real3 ComputeEdgeFactor(real3 V1, real3 V2)
real IntegrateEdge(real3 V1, real3 V2)
real DiffuseSphereLightIrradiance(real sinSqSigma, real cosOmega)
real3 PolygonFormFactor(real4x3 L)
real PolygonIrradianceFromVectorFormFactor(float3 F)
real PolygonIrradiance(real4x3 L)
real LineFpo(real tLDDL, real lrcpD, real rcpD)
real LineFwt(real tLDDL, real l)
real LineIrradiance(real l1, real l2, real3 normal, real3 tangent)
real ComputeLineWidthFactor(real3x3 invM, real3 ortho)
real LTCEvaluate(real3 P1, real3 P2, real3 B, real3x3 invM)
```



### BC6H

```c
float CalcMSLE(float3 a, float3 b)
float3 Quantize7(float3 x)
float3 Quantize9(float3 x)
float3 Quantize10(float3 x)
float3 Unquantize7(float3 x)
float3 Unquantize9(float3 x)
float3 Unquantize10(float3 x)
uint ComputeIndex3(float texelPos, float endPoint0Pos, float endPoint1Pos )
uint ComputeIndex4(float texelPos, float endPoint0Pos, float endPoint1Pos )
void SignExtend(inout float3 v1, uint mask, uint signFlag )
float3 FinishUnquantize( float3 endpoint0Unq, float3 endpoint1Unq, float weight )
void EncodeMode11( inout uint4 block, inout float blockMSLE, float3 texels[ 16 ] )
```



### BSDF

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
struct CBSDF
{
    float3 diffR; // Diffuse  reflection   (T -> MS -> T, same sides)
    float3 specR; // Specular reflection   (R, RR, TRT, etc)
    float3 diffT; // Diffuse  transmission (rough T or TT, opposite sides)
    float3 specT; // Specular transmission (T, TT, TRRT, etc)
};
real F_Schlick(real f0, real f90, real u)
real F_Schlick(real f0, real u)
real3 F_Schlick(real3 f0, real f90, real u)
real3 F_Schlick(real3 f0, real u)
real F_Transm_Schlick(real f0, real f90, real u)
real F_Transm_Schlick(real f0, real u)
real3 F_Transm_Schlick(real3 f0, real f90, real u)
real3 F_Transm_Schlick(real3 f0, real u)
real F_FresnelDielectric(real ior, real u)
real3 F_FresnelConductor(real3 eta, real3 etak2, real cosTheta)
TEMPLATE_2_REAL(IorToFresnel0, transmittedIor, incidentIor, return Sq((transmittedIor - incidentIor) / (transmittedIor + incidentIor)) )
real IorToFresnel0(real transmittedIor)
TEMPLATE_1_REAL(Fresnel0ToIor, fresnel0, return ((1.0 + sqrt(fresnel0)) / (1.0 - sqrt(fresnel0))) )
TEMPLATE_1_REAL(ConvertF0ForAirInterfaceToF0ForClearCoat15, fresnel0, return saturate(-0.0256868 + fresnel0 * (0.326846 + (0.978946 - 0.283835 * fresnel0) * fresnel0)))
TEMPLATE_1_REAL(ConvertF0ForAirInterfaceToF0ForClearCoat15Fast, fresnel0, return saturate(fresnel0 * (fresnel0 * 0.526868 + 0.529324) - 0.0482256))
real3 GetIorN(real3 f0, real3 edgeTint)
real3 getIorK2(real3 f0, real3 n)
real3 CoatRefract(real3 X, real3 N, real ieta)
float Lambda_GGX(float roughness, float3 V)
real D_GGXNoPI(real NdotH, real roughness)
real D_GGX(real NdotH, real roughness)
real G_MaskingSmithGGX(real NdotV, real roughness)
real GetSmithJointGGXPartLambdaV(real NdotV, real roughness)
real V_SmithJointGGX(real NdotL, real NdotV, real roughness, real partLambdaV)
real V_SmithJointGGX(real NdotL, real NdotV, real roughness)
real DV_SmithJointGGX(real NdotH, real NdotL, real NdotV, real roughness, real partLambdaV)
real DV_SmithJointGGX(real NdotH, real NdotL, real NdotV, real roughness)
real GetSmithJointGGXPartLambdaVApprox(real NdotV, real roughness)
real V_SmithJointGGXApprox(real NdotL, real NdotV, real roughness, real partLambdaV)
real V_SmithJointGGXApprox(real NdotL, real NdotV, real roughness)
real D_GGXAnisoNoPI(real TdotH, real BdotH, real NdotH, real roughnessT, real roughnessB)
real D_GGXAniso(real TdotH, real BdotH, real NdotH, real roughnessT, real roughnessB)
real GetSmithJointGGXAnisoPartLambdaV(real TdotV, real BdotV, real NdotV, real roughnessT, real roughnessB)
real V_SmithJointGGXAniso(real TdotV, real BdotV, real NdotV, real TdotL, real BdotL, real NdotL, real roughnessT, real roughnessB, real partLambdaV)
real V_SmithJointGGXAniso(real TdotV, real BdotV, real NdotV, real TdotL, real BdotL, real NdotL, real roughnessT, real roughnessB)
real DV_SmithJointGGXAniso(real TdotH, real BdotH, real NdotH, real NdotV,
                           real TdotL, real BdotL, real NdotL,
                           real roughnessT, real roughnessB, real partLambdaV)
real DV_SmithJointGGXAniso(real TdotH, real BdotH, real NdotH,
                           real TdotV, real BdotV, real NdotV,
                           real TdotL, real BdotL, real NdotL,
                           real roughnessT, real roughnessB)
float GetProjectedRoughness(float TdotV, float BdotV, float NdotV, float roughnessT, float roughnessB)
real LambertNoPI()
real Lambert()
real DisneyDiffuseNoPI(real NdotV, real NdotL, real LdotV, real perceptualRoughness)
real DisneyDiffuse(real NdotV, real NdotL, real LdotV, real perceptualRoughness)
#endif
real3 DiffuseGGXNoPI(real3 albedo, real NdotV, real NdotL, real NdotH, real LdotV, real roughness)
real3 DiffuseGGX(real3 albedo, real NdotV, real NdotL, real NdotH, real LdotV, real roughness)
real3 EvalSensitivity(real opd, real shift)
real3 EvalIridescence(real eta_1, real cosTheta1, real iridescenceThickness, real3 baseLayerFresnel0, real iorOverBaseLayer = 0.0)
```



### Color

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ACES.hlsl"
real Gamma20ToLinear(real c)
real3 Gamma20ToLinear(real3 c)
real4 Gamma20ToLinear(real4 c)
real LinearToGamma20(real c)
real3 LinearToGamma20(real3 c)
real4 LinearToGamma20(real4 c)
real Gamma22ToLinear(real c)
real3 Gamma22ToLinear(real3 c)
real4 Gamma22ToLinear(real4 c)
real LinearToGamma22(real c)
real3 LinearToGamma22(real3 c)
real4 LinearToGamma22(real4 c)
real SRGBToLinear(real c)
real2 SRGBToLinear(real2 c)
real3 SRGBToLinear(real3 c)
real4 SRGBToLinear(real4 c)
real LinearToSRGB(real c)
real2 LinearToSRGB(real2 c)
real3 LinearToSRGB(real3 c)
real4 LinearToSRGB(real4 c)
real FastSRGBToLinear(real c)
real2 FastSRGBToLinear(real2 c)
real3 FastSRGBToLinear(real3 c)
real4 FastSRGBToLinear(real4 c)
real FastLinearToSRGB(real c)
real2 FastLinearToSRGB(real2 c)
real3 FastLinearToSRGB(real3 c)
real4 FastLinearToSRGB(real4 c)
real Luminance(real3 linearRgb)
real Luminance(real4 linearRgba)
real AcesLuminance(real3 linearRgb)
real AcesLuminance(real4 linearRgba)
real ScotopicLuminance(real3 xyzRgb)
real ScotopicLuminance(real4 xyzRgba)
real3 RGBToYCoCg(real3 rgb)
real3 YCoCgToRGB(real3 YCoCg)
real YCoCgCheckBoardEdgeFilter(real centerLum, real2 a0, real2 a1, real2 a2, real2 a3)
float3 LinearToLMS(float3 x)
float3 LMSToLinear(float3 x)
real3 RgbToHsv(real3 c)
real3 HsvToRgb(real3 c)
real RotateHue(real value, real low, real hi)
float3 SoftLight(float3 base, float3 blend)
real3 LinearToPQ(real3 x, real maxPQValue)
real3 LinearToPQ(real3 x)
real3 PQToLinear(real3 x, real maxPQValue)
real3 PQToLinear(real3 x)
real LinearToLogC_Precise(real x)
float3 LinearToLogC(float3 x)
real LogCToLinear_Precise(real x)
float3 LogCToLinear(float3 x)
real3 Desaturate(real3 value, real saturation)
real FastTonemapPerChannel(real c)
real2 FastTonemapPerChannel(real2 c)
real3 FastTonemap(real3 c)
real4 FastTonemap(real4 c)
real3 FastTonemap(real3 c, real w)
real4 FastTonemap(real4 c, real w)
real FastTonemapPerChannelInvert(real c)
real2 FastTonemapPerChannelInvert(real2 c)
real3 FastTonemapInvert(real3 c)
real4 FastTonemapInvert(real4 c)
real3 ApplyLut3D(TEXTURE3D_PARAM(tex, samplerTex), float3 uvw, float2 scaleOffset)
real3 ApplyLut2D(TEXTURE2D_PARAM(tex, samplerTex), float3 uvw, float3 scaleOffset)
real3 GetLutStripValue(float2 uv, float4 params)
float3 NeutralCurve(float3 x, real a, real b, real c, real d, real e, real f)
real3 NeutralTonemap(real3 x)
real EvalCustomSegment(real x, real4 segmentA, real2 segmentB)
real EvalCustomCurve(real x, real3 curve, real4 toeSegmentA, real2 toeSegmentB, real4 midSegmentA, real2 midSegmentB, real4 shoSegmentA, real2 shoSegmentB)
real3 CustomTonemap(real3 x, real3 curve, real4 toeSegmentA, real2 toeSegmentB, real4 midSegmentA, real2 midSegmentB, real4 shoSegmentA, real2 shoSegmentB)
real3 InvertibleTonemap(real3 x)
real3 InvertibleTonemapInverse(real3 x)
float3 AcesTonemap(float3 aces)
half4 EncodeRGBM(half3 color)
half3 DecodeRGBM(half4 rgbm)
```



### Common

```c
#include "Packages/com.unity.render-pipelines.gamecore/ShaderLibrary/API/GameCore.hlsl"
#include "Packages/com.unity.render-pipelines.xboxone/ShaderLibrary/API/XBoxOne.hlsl"
#include "Packages/com.unity.render-pipelines.ps4/ShaderLibrary/API/PSSL.hlsl"
#include "Packages/com.unity.render-pipelines.ps5/ShaderLibrary/API/PSSL.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/D3D11.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/Metal.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/Vulkan.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/Switch.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/GLCore.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/GLES3.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/GLES2.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/Validate.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Macros.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Random.hlsl"
#if REAL_IS_HALF
#define TEMPLATE_1_REAL TEMPLATE_1_HALF
#else
#define TEMPLATE_1_REAL TEMPLATE_1_FLT
#endif
int BitFieldExtractSignExtend(int data, uint offset, uint numBits)
uint BitFieldInsert(uint mask, uint src, uint dst)
bool IsBitSet(uint data, uint offset)
void SetBit(inout uint data, uint offset)
void ClearBit(inout uint data, uint offset)
void ToggleBit(inout uint data, uint offset)
TEMPLATE_1_REAL(WaveReadLaneFirst, scalarValue, return scalarValue)
TEMPLATE_1_INT(WaveReadLaneFirst, scalarValue, return scalarValue)
TEMPLATE_2_INT(Mul24, a, b, return a * b)
TEMPLATE_3_INT(Mad24, a, b, c, return a * b + c)
TEMPLATE_3_REAL(Min3, a, b, c, return min(min(a, b), c))
TEMPLATE_3_INT(Min3, a, b, c, return min(min(a, b), c))
TEMPLATE_3_REAL(Max3, a, b, c, return max(max(a, b), c))
TEMPLATE_3_INT(Max3, a, b, c, return max(max(a, b), c))
TEMPLATE_3_REAL(Avg3, a, b, c, return (a + b + c) * 0.33333333)
float2 GetQuadOffset(int2 screenPos)
float QuadReadAcrossX(float value, int2 screenPos)
float QuadReadAcrossY(float value, int2 screenPos)
float QuadReadAcrossDiagonal(float value, int2 screenPos)
float3 QuadReadFloat3AcrossX(float3 val, int2 positionSS)
float4 QuadReadFloat4AcrossX(float4 val, int2 positionSS)
float3 QuadReadFloat3AcrossY(float3 val, int2 positionSS)
float4 QuadReadFloat4AcrossY(float4 val, int2 positionSS)
float3 QuadReadFloat3AcrossDiagonal(float3 val, int2 positionSS)
float4 QuadReadFloat4AcrossDiagonal(float4 val, int2 positionSS)
float CubeMapFaceID(float3 dir)
bool IsNaN(float x)
bool AnyIsNaN(float2 v)
bool AnyIsNaN(float3 v)
bool AnyIsNaN(float4 v)
bool IsInf(float x)
bool AnyIsInf(float2 v)
bool AnyIsInf(float3 v)
bool AnyIsInf(float4 v)
bool IsFinite(float x)
float SanitizeFinite(float x)
bool IsPositiveFinite(float x)
float SanitizePositiveFinite(float x)
real DegToRad(real deg)
real RadToDeg(real rad)
TEMPLATE_1_REAL(Sq, x, return (x) * (x))
TEMPLATE_1_INT(Sq, x, return (x) * (x))
bool IsPower2(uint x)
real FastACosPos(real inX)
real FastACos(real inX)
real FastASin(real x)
real FastATanPos(real x)
real FastATan(real x)
real FastAtan2(real y, real x)
uint FastLog2(uint x)
TEMPLATE_2_REAL(PositivePow, base, power, return pow(abs(base), power))
TEMPLATE_2_REAL(SafePositivePow, base, power, return pow(max(abs(base), real(REAL_EPS)), power))
TEMPLATE_2_FLT(SafePositivePow_float, base, power, return pow(max(abs(base), float(FLT_EPS)), power))
TEMPLATE_2_HALF(SafePositivePow_half, base, power, return pow(max(abs(base), half(HALF_EPS)), power))
float Eps_float() 
float Min_float() 
float Max_float() 
half Eps_half() 
half Min_half() 
half Max_half()
float CopySign(float x, float s, bool ignoreNegZero = true)
float FastSign(float s, bool ignoreNegZero = true)
real3 Orthonormalize(real3 tangent, real3 normal)
TEMPLATE_3_REAL(Remap01, x, rcpLength, startTimesRcpLength, return saturate(x * rcpLength - startTimesRcpLength))
TEMPLATE_3_REAL(Remap10, x, rcpLength, endTimesRcpLength, return saturate(endTimesRcpLength - x * rcpLength))
real2 RemapHalfTexelCoordTo01(real2 coord, real2 size)
real2 Remap01ToHalfTexelCoord(real2 coord, real2 size)
real Smoothstep01(real x)
real Smootherstep01(real x)
real Smootherstep(real a, real b, real t)
float3 NLerp(float3 A, float3 B, float t)
float Length2(float3 v)
real Pow4(real x)
TEMPLATE_3_FLT(RangeRemap, min, max, t, return saturate((t - min) / (max - min)))
float4x4 Inverse(float4x4 m)
float ComputeTextureLOD(float2 uvdx, float2 uvdy, float2 scale, float bias = 0.0)
float ComputeTextureLOD(float2 uv, float bias = 0.0)
float ComputeTextureLOD(float2 uv, float2 texelSize, float bias = 0.0)
float ComputeTextureLOD(float3 duvw_dx, float3 duvw_dy, float3 duvw_dz, float scale, float bias = 0.0)
uint GetMipCount(TEXTURE2D_PARAM(tex, smp))
float4 tex1D(sampler1D x, float v)              
float4 tex2D(sampler2D x, float2 v)             
float4 tex3D(sampler3D x, float3 v)             
float4 texCUBE(samplerCUBE x, float3 v)         
float4 tex1Dbias(sampler1D x, in float4 t)              
float4 tex2Dbias(sampler2D x, in float4 t)              
float4 tex3Dbias(sampler3D x, in float4 t)              
float4 texCUBEbias(samplerCUBE x, in float4 t)          
float4 tex1Dlod(sampler1D x, in float4 t)           
float4 tex2Dlod(sampler2D x, in float4 t)           
float4 tex3Dlod(sampler3D x, in float4 t)           
float4 texCUBElod(samplerCUBE x, in float4 t)       
float4 tex1Dgrad(sampler1D x, float t, float dx, float dy)              
float4 tex2Dgrad(sampler2D x, float2 t, float2 dx, float2 dy)           
float4 tex3Dgrad(sampler3D x, float3 t, float3 dx, float3 dy)           
float4 texCUBEgrad(samplerCUBE x, float3 t, float3 dx, float3 dy)       
float4 tex1D(sampler1D x, float t, float dx, float dy)              
float4 tex2D(sampler2D x, float2 t, float2 dx, float2 dy)           
float4 tex3D(sampler3D x, float3 t, float3 dx, float3 dy)           
float4 texCUBE(samplerCUBE x, float3 t, float3 dx, float3 dy)       
float4 tex1Dproj(sampler1D s, in float2 t)              
float4 tex1Dproj(sampler1D s, in float4 t)              
float4 tex2Dproj(sampler2D s, in float3 t)              
float4 tex2Dproj(sampler2D s, in float4 t)              
float4 tex3Dproj(sampler3D s, in float4 t)              
float4 texCUBEproj(samplerCUBE s, in float4 t)
float2 DirectionToLatLongCoordinate(float3 unDir)
float3 LatlongToDirectionCoordinate(float2 coord)
float Linear01DepthFromNear(float depth, float4 zBufferParam)
float Linear01Depth(float depth, float4 zBufferParam)
float LinearEyeDepth(float depth, float4 zBufferParam)
float LinearEyeDepth(float2 positionNDC, float deviceDepth, float4 invProjParam)
float LinearEyeDepth(float3 positionWS, float4x4 viewMatrix)
float EncodeLogarithmicDepthGeneralized(float z, float4 encodingParams)
float DecodeLogarithmicDepthGeneralized(float d, float4 decodingParams)
float EncodeLogarithmicDepth(float z, float4 encodingParams)
float DecodeLogarithmicDepth(float d, float4 encodingParams)
real4 CompositeOver(real4 front, real4 back)
void CompositeOver(real3 colorFront, real3 alphaFront,
                   real3 colorBack,  real3 alphaBack,
                   out real3 color,  out real3 alpha)
float4 ComputeClipSpacePosition(float2 positionNDC, float deviceDepth)
float4 ComputeClipSpacePosition(float3 position, float4x4 clipSpaceTransform = k_identity4x4)
float3 ComputeNormalizedDeviceCoordinatesWithZ(float3 position, float4x4 clipSpaceTransform = k_identity4x4)
float2 ComputeNormalizedDeviceCoordinates(float3 position, float4x4 clipSpaceTransform = k_identity4x4)
float3 ComputeViewSpacePosition(float2 positionNDC, float deviceDepth, float4x4 invProjMatrix)
float3 ComputeWorldSpacePosition(float2 positionNDC, float deviceDepth, float4x4 invViewProjMatrix)
float3 ComputeWorldSpacePosition(float4 positionCS, float4x4 invViewProjMatrix)
// Note: if you modify this struct, be sure to update the CustomPassFullscreenShader.template
struct PositionInputs
{
    float3 positionWS;  // World space position(could be camera-relative)
    float2 positionNDC; // Normalized screen coordinates within the viewport  : [0, 1) (with the half-pixel offset)
    uint2  positionSS;  // Screen space pixel coordinates: [0, NumPixels)
    uint2  tileCoord;   // Screen tile coordinates       : [0, NumTiles)
    float  deviceDepth; // Depth from the depth buffer   : [0, 1] (typically reversed)
    float  linearDepth; // View space Z coordinate       : [Near, Far]
};
PositionInputs GetPositionInput(float2 positionSS, float2 invScreenSize, uint2 tileCoord)
PositionInputs GetPositionInput(float2 positionSS, float2 invScreenSize)
PositionInputs GetPositionInput(float2 positionSS, float2 invScreenSize, float3 positionWS)
PositionInputs GetPositionInput(float2 positionSS, float2 invScreenSize, float deviceDepth, float linearDepth, float3 positionWS, uint2 tileCoord)
PositionInputs GetPositionInput(float2 positionSS, float2 invScreenSize, float deviceDepth, float linearDepth, float3 positionWS)
PositionInputs GetPositionInput(float2 positionSS, float2 invScreenSize, float deviceDepth,
    float4x4 invViewProjMatrix, float4x4 viewMatrix,
    uint2 tileCoord)
PositionInputs GetPositionInput(float2 positionSS, float2 invScreenSize, float deviceDepth,
                                float4x4 invViewProjMatrix, float4x4 viewMatrix)
void ApplyDepthOffsetPositionInput(float3 V, float depthOffsetVS, float3 viewForwardDir, float4x4 viewProjMatrix, inout PositionInputs posInput)
real4 PackHeightmap(real height)
real UnpackHeightmap(real4 height)
real4 PackHeightmap(real height)
real UnpackHeightmap(real4 height)
bool HasFlag(uint bitfield, uint flag)
real3 SafeNormalize(float3 inVec)
bool IsNormalized(float3 inVec)
real SafeDiv(real numer, real denom)
real SafeSqrt(real x)
real SinFromCos(real cosX)
real SphericalDot(real cosTheta1, real phi1, real cosTheta2, real phi2)
float2 GetFullScreenTriangleTexCoord(uint vertexID)
float4 GetFullScreenTriangleVertexPosition(uint vertexID, float z = UNITY_NEAR_CLIP_VALUE)
float2 GetQuadTexCoord(uint vertexID)
float4 GetQuadVertexPosition(uint vertexID, float z = UNITY_NEAR_CLIP_VALUE)
void LODDitheringTransition(uint2 fadeMaskSeed, float ditherFactor)
uint GetStencilValue(uint2 stencilBufferVal)
float SharpenAlpha(float alpha, float alphaClipTreshold)
TEMPLATE_1_REAL(ClampToFloat16Max, value, return min(value, HALF_MAX))
```



### CommonLighting

```c
real3 MapCubeToSphere(real3 v)
real ComputeCubeToSphereMapSqMagnitude(real3 v)
real ComputeCubemapTexelSolidAngle(real3 L, real texelArea)
real ComputeCubemapTexelSolidAngle(real2 uv)
real ConvertEvToLuminance(real ev)
real ConvertLuminanceToEv(real luminance)
real DistanceWindowing(real distSquare, real rangeAttenuationScale, real rangeAttenuationBias)
real SmoothDistanceWindowing(real distSquare, real rangeAttenuationScale, real rangeAttenuationBias)
real SmoothWindowedDistanceAttenuation(real distSquare, real distRcp, real rangeAttenuationScale, real rangeAttenuationBias)
real AngleAttenuation(real cosFwd, real lightAngleScale, real lightAngleOffset)
real SmoothAngleAttenuation(real cosFwd, real lightAngleScale, real lightAngleOffset)
real PunctualLightAttenuation(real4 distances, real rangeAttenuationScale, real rangeAttenuationBias,
                              real lightAngleScale, real lightAngleOffset)
real EllipsoidalDistanceAttenuation(real3 unL, real3 axis, real invAspectRatio,
                                    real rangeAttenuationScale, real rangeAttenuationBias)
real EllipsoidalDistanceAttenuation(real3 unL, real3 invHalfDim,
                                    real rangeAttenuationScale, real rangeAttenuationBias)
real BoxDistanceAttenuation(real3 unL, real3 invHalfDim,
                            real rangeAttenuationScale, real rangeAttenuationBias)
real2 GetIESTextureCoordinate(real3x3 lightToWord, real3 L)
real GetHorizonOcclusion(real3 V, real3 normalWS, real3 vertexNormal, real horizonFade)
real GetSpecularOcclusionFromAmbientOcclusion(real NdotV, real ambientOcclusion, real roughness)
real3 GTAOMultiBounce(real visibility, real3 albedo)
real SphericalCapIntersectionSolidArea(real cosC1, real cosC2, real cosB)
real GetSpecularOcclusionFromBentAO_ConeCone(real3 V, real3 bentNormalWS, real3 normalWS, real ambientOcclusion, real roughness)
real GetSpecularOcclusionFromBentAO(real3 V, real3 bentNormalWS, real3 normalWS, real ambientOcclusion, real roughness)
real ComputeWrappedDiffuseLighting(real NdotL, real w)
real ComputeWrappedPowerDiffuseLighting(real NdotL, real w, real p)
real ComputeMicroShadowing(real AO, real NdotL, real opacity)
real3 ComputeShadowColor(real shadow, real3 shadowTint, real penumbraFlag)
real3 ComputeShadowColor(real3 shadow, real3 shadowTint, real penumbraFlag)
real ClampNdotV(real NdotV)
void GetBSDFAngle(real3 V, real3 L, real NdotL, real NdotV,
                  out real LdotV, out real NdotH, out real LdotH, out real invLenLV)
real3 GetViewReflectedNormal(real3 N, real3 V, out real NdotV)
real3x3 GetLocalFrame(real3 localZ)
real3x3 GetLocalFrame(real3 localZ, real3 localX)
real3x3 GetOrthoBasisViewNormal(real3 V, real3 N, real unclampedNdotV, bool testSingularity = false)
bool IsMatchingLightLayer(uint lightLayers, uint renderingLayers)
```



### CommonMaterial

```c
real PerceptualRoughnessToRoughness(real perceptualRoughness)
real RoughnessToPerceptualRoughness(real roughness)
real RoughnessToPerceptualSmoothness(real roughness)
real PerceptualSmoothnessToRoughness(real perceptualSmoothness)
real PerceptualSmoothnessToPerceptualRoughness(real perceptualSmoothness)
real BeckmannRoughnessToGGXRoughness(real roughnessBeckmann)
real PerceptualRoughnessBeckmannToGGX(real perceptualRoughnessBeckmann)
real GGXRoughnessToBeckmannRoughness(real roughnessGGX)
real PerceptualRoughnessToPerceptualSmoothness(real perceptualRoughness)
real ClampRoughnessForAnalyticalLights(real roughness)
real ClampRoughnessForRaytracing(real roughness)
real ClampPerceptualRoughnessForRaytracing(real perceptualRoughness)
void ConvertValueAnisotropyToValueTB(real value, real anisotropy, out real valueT, out real valueB)
void ConvertAnisotropyToRoughness(real perceptualRoughness, real anisotropy, out real roughnessT, out real roughnessB)
void ConvertRoughnessTAndAnisotropyToRoughness(real roughnessT, real anisotropy, out real roughness)
real ConvertRoughnessTAndBToRoughness(real roughnessT, real roughnessB)
void ConvertRoughnessToAnisotropy(real roughnessT, real roughnessB, out real anisotropy)
void ConvertAnisotropyToClampRoughness(real perceptualRoughness, real anisotropy, out real roughnessT, out real roughnessB)
real RoughnessToVariance(real roughness)
real VarianceToRoughness(real variance)
float DecodeVariance(float gradientW)
float NormalFiltering(float perceptualSmoothness, float variance, float threshold)
float ProjectedSpaceNormalFiltering(float perceptualSmoothness, float variance, float threshold)
float GeometricNormalVariance(float3 geometricNormalWS, float screenSpaceVariance)
float GeometricNormalFiltering(float perceptualSmoothness, float3 geometricNormalWS, float screenSpaceVariance, float threshold)
float ProjectedSpaceGeometricNormalFiltering(float perceptualSmoothness, float3 geometricNormalWS, float screenSpaceVariance, float threshold)
float TextureNormalVariance(float avgNormalLength)
float TextureNormalFiltering(float perceptualSmoothness, float avgNormalLength, float threshold)
float3 ComputeDiffuseColor(float3 baseColor, float metallic)
float3 ComputeFresnel0(float3 baseColor, float metallic, float dielectricF0)
real3 BlendNormalWorldspaceRNM(real3 n1, real3 n2, real3 vtxNormal)
real3 BlendNormalRNM(real3 n1, real3 n2)
real3 BlendNormal(real3 n1, real3 n2)
real3 ComputeTriplanarWeights(real3 normal)
void GetTriplanarCoordinate(float3 position, out float2 uvXZ, out float2 uvXY, out float2 uvZY)
real LerpWhiteTo(real b, real t)
real3 LerpWhiteTo(real3 b, real t)
```



### CommonShadow

```c
// Calculates the offset to use for sampling the shadow map, based on the surface normal
real3 GetShadowPosOffset(real NdotL, real3 normalWS, real2 invShadowMapSize)
```



### Debug

```c
real3 GetIndexColor(int index)
bool SampleDebugFont(int2 pixCoord, uint digit)
bool SampleDebugFontNumber(int2 pixCoord, uint number)
bool SampleDebugFontNumber3Digits(int2 pixCoord, uint number)
float4 OverlayHeatMap(uint2 pixCoord, uint2 tileSize, uint n, uint maxN, float opacity)
float4 GetStreamingMipColor(uint mipCount, float4 mipInfo)
float4 GetSimpleMipCountColor(uint mipCount)
float4 GetMipLevelColor(float2 uv, float4 texelSize)
float3 GetDebugMipColor(float3 originalColor, float4 texelSize, float2 uv)
float3 GetDebugMipCountColor(float3 originalColor, uint mipCount)
float3 GetDebugStreamingMipColor(uint mipCount, float4 mipInfo)
float3 GetDebugStreamingMipColorBlended(float3 originalColor, uint mipCount, float4 mipInfo)
float3 GetDebugMipColorIncludingMipReduction(float3 originalColor, uint mipCount, float4 texelSize, float2 uv, float4 mipInfo)
float3 GetDebugMipReductionColor(uint mipCount, float4 mipInfo)
real3 GetColorCodeFunction(real value, real4 threshold)
```



### EntityLighting

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
real3 SHEvalLinearL0L1(real3 N, real4 shAr, real4 shAg, real4 shAb)
real3 SHEvalLinearL1(real3 N, real3 shAr, real3 shAg, real3 shAb)
real3 SHEvalLinearL2(real3 N, real4 shBr, real4 shBg, real4 shBb, real4 shC)
half3 SampleSH9(half4 SHCoefficients[7], half3 N)
float3 SampleSH9(float4 SHCoefficients[7], float3 N)
void SampleProbeVolumeSH4(TEXTURE3D_PARAM(SHVolumeTexture, SHVolumeSampler), float3 positionWS, float3 normalWS, float3 backNormalWS, float4x4 WorldToTexture, float transformToLocal, float texelSizeX, float3 probeVolumeMin, float3 probeVolumeSizeInv, inout float3 bakeDiffuseLighting, inout float3 backBakeDiffuseLighting)
float3 SampleProbeVolumeSH4(TEXTURE3D_PARAM(SHVolumeTexture, SHVolumeSampler), float3 positionWS, float3 normalWS, float4x4 WorldToTexture, float transformToLocal, float texelSizeX, float3 probeVolumeMin, float3 probeVolumeSizeInv)
void SampleProbeVolumeSH9(TEXTURE3D_PARAM(SHVolumeTexture, SHVolumeSampler), float3 positionWS, float3 normalWS, float3 backNormalWS, float4x4 WorldToTexture, float transformToLocal, float texelSizeX, float3 probeVolumeMin, float3 probeVolumeSizeInv, inout float3 bakeDiffuseLighting, inout float3 backBakeDiffuseLighting)
real4 PackEmissiveRGBM(real3 rgb)
real3 UnpackLightmapRGBM(real4 rgbmInput, real4 decodeInstructions)
real3 UnpackLightmapDoubleLDR(real4 encodedColor, real4 decodeInstructions)
real3 DecodeLightmap(real4 encodedIlluminance, real4 decodeInstructions)
real3 DecodeHDREnvironment(real4 encodedIrradiance, real4 decodeInstructions)
real3 SampleSingleLightmap(TEXTURE2D_LIGHTMAP_PARAM(lightmapTex, lightmapSampler), LIGHTMAP_EXTRA_ARGS, float4 transform, bool encodedLightmap, real4 decodeInstructions)
void SampleDirectionalLightmap(TEXTURE2D_LIGHTMAP_PARAM(lightmapTex, lightmapSampler), TEXTURE2D_LIGHTMAP_PARAM(lightmapDirTex, lightmapDirSampler), LIGHTMAP_EXTRA_ARGS, float4 transform, float3 normalWS, float3 backNormalWS, bool encodedLightmap, real4 decodeInstructions, inout real3 bakeDiffuseLighting, inout real3 backBakeDiffuseLighting)
real3 SampleDirectionalLightmap(TEXTURE2D_LIGHTMAP_PARAM(lightmapTex, lightmapSampler), TEXTURE2D_LIGHTMAP_PARAM(lightmapDirTex, lightmapDirSampler), LIGHTMAP_EXTRA_ARGS, float4 transform, float3 normalWS, bool encodedLightmap, real4 decodeInstructions)
```



### Filtering

```c
real2 BSpline2Left(real2 x)
real2 BSpline2Middle(real2 x)
real2 BSpline2Right(real2 x)
real2 BSpline3Leftmost(real2 x)
real2 BSpline3MiddleLeft(real2 x)
real2 BSpline3MiddleRight(real2 x)
real2 BSpline3Rightmost(real2 x)
void BicubicFilter(float2 fracCoord, out float2 weights[2], out float2 offsets[2])
void BiquadraticFilter(float2 fracCoord, out float2 weights[2], out float2 offsets[2])
float4 SampleTexture2DBiquadratic(TEXTURE2D_PARAM(tex, smp), float2 coord, float4 texSize)
float4 SampleTexture2DBicubic(TEXTURE2D_PARAM(tex, smp), float2 coord, float4 texSize, float2 maxCoord, uint unused /* needed to match signature of texarray version below */)
float4 SampleTexture2DBicubic(TEXTURE2D_ARRAY_PARAM(tex, smp), float2 coord, float4 texSize, float2 maxCoord, uint slice)
```



### GeometricTools

```c
float3 Rotate(float3 pivot, float3 position, float3 rotationAxis, float angle)
float3x3 RotationFromAxisAngle(float3 A, float sinAngle, float cosAngle)
bool SolveQuadraticEquation(float a, float b, float c, out float2 roots)
bool IntersectRayAABB(float3 rayOrigin, float3 rayDirection,
                      float3 boxMin,    float3 boxMax,
                      float  tMin,       float tMax,
                  out float  tEntr,  out float tExit)
float IntersectRayAABBSimple(float3 start, float3 dir, float3 boxMin, float3 boxMax)
bool IntersectRaySphere(float3 start, float3 dir, float radius, out float2 intersections)
float IntersectRaySphereSimple(float3 start, float3 dir, float radius)
float3 IntersectRayPlane(float3 rayOrigin, float3 rayDirection, float3 planeOrigin, float3 planeNormal)
bool IntersectRayPlane(float3 rayOrigin, float3 rayDirection, float3 planePosition, float3 planeNormal, out float t)
bool IntersectRayCone(float3 rayOrigin,  float3 rayDirection,
                      float3 coneOrigin, float3 coneDirection,
                      float3 coneAxisX,  float3 coneAxisY,
                      float tMin, float tMax,
                      out float tEntr, out float tExit)
bool IntersectSphereAABB(float3 position, float radius, float3 aabbMin, float3 aabbMax)
float DistancePointBox(float3 position, float3 boxMin, float3 boxMax)
float3 ProjectPointOnPlane(float3 position, float3 planePosition, float3 planeNormal)
float DistanceFromPlane(float3 p, float4 plane)
bool CullTriangleFrustum(float3 p0, float3 p1, float3 p2, float epsilon, float4 frustumPlanes[6], int numPlanes)
bool4 CullFullTriangleAndEdgesFrustum(float3 p0, float3 p1, float3 p2, float epsilon, float4 frustumPlanes[6], int numPlanes)
bool3 CullTriangleEdgesFrustum(float3 p0, float3 p1, float3 p2, float epsilon, float4 frustumPlanes[6], int numPlanes)
bool CullTriangleBackFaceView(float3 p0, float3 p1, float3 p2, float epsilon, float3 V, float winding)
bool CullTriangleBackFace(float3 p0, float3 p1, float3 p2, float epsilon, float3 viewPos, float winding)
```



### GraniteShaderLibBase

```c
struct GraniteTranslationTexture
struct GraniteCacheTexture
struct GraniteStreamingTextureConstantBuffer
struct GraniteStreamingTextureCubeConstantBuffer
struct GraniteTilesetConstantBuffer
struct GraniteConstantBuffers
struct GraniteCubeConstantBuffers
struct GraniteLookupData
struct GraniteLODLookupData
gra_Float4 GranitePrivate_SampleArray(in GraniteCacheTexture tex, in gra_Float3 texCoord)
gra_Float4 GranitePrivate_SampleGradArray(in GraniteCacheTexture tex, in gra_Float3 texCoord, in gra_Float2 dX, in gra_Float2 dY)
gra_Float4 GranitePrivate_SampleLevelArray(in GraniteCacheTexture tex, in gra_Float3 texCoord, in float level)
gra_Float4 GranitePrivate_Sample(in GraniteCacheTexture tex, in gra_Float2 texCoord)
gra_Float4 GranitePrivate_SampleLevel(in GraniteCacheTexture tex, in gra_Float2 texCoord, in float level)
gra_Float4 GranitePrivate_SampleGrad(in GraniteCacheTexture tex, in gra_Float2 texCoord, in gra_Float2 dX, in gra_Float2 dY)
gra_Float4 GranitePrivate_Load(in GraniteTranslationTexture tex, in gra_Int3 location)
gra_Float4 GranitePrivate_SampleLevel_Translation(in GraniteTranslationTexture tex, in gra_Float2 texCoord, in float level)
float GranitePrivate_Saturate(in float value)
uint GranitePrivate_FloatAsUint(float value)
float GranitePrivate_Pow2(uint exponent)
gra_Float2 GranitePrivate_RepeatUV(in gra_Float2 uv, in GraniteStreamingTextureConstantBuffer grSTCB)
gra_Float2 GranitePrivate_UdimUV(in gra_Float2 uv, in GraniteStreamingTextureConstantBuffer grSTCB)
gra_Float2 GranitePrivate_ClampUV(in gra_Float2 uv, in GraniteStreamingTextureConstantBuffer grSTCB)
gra_Float2 GranitePrivate_MirrorUV(in gra_Float2 uv, in GraniteStreamingTextureConstantBuffer grSTCB)
gra_Float4 GranitePrivate_PackTileId(in gra_Float2 tileXY, in float level, in float textureID);
gra_Float4 Granite_DebugPackedTileId64(in gra_Float4 PackedTile)
gra_Float3 Granite_UnpackNormal(in gra_Float4 PackedNormal, float scale)
gra_Float3 Granite_UnpackNormal(in gra_Float4 PackedNormal)
GraniteTilesetConstantBuffer Granite_ApplyResolutionOffset(in GraniteTilesetConstantBuffer INtsCB, in float resolutionOffsetPow2)
GraniteTilesetConstantBuffer Granite_SetMaxAnisotropy(in GraniteTilesetConstantBuffer INtsCB, in float maxAnisotropyLog2)
void Granite_ApplyResolutionOffset(inout GraniteTilesetConstantBuffer tsCB, in float resolutionOffsetPow2)
void Granite_SetMaxAnisotropy(inout GraniteTilesetConstantBuffer tsCB, in float maxAnisotropyLog2)
gra_Float2 Granite_Transform(in GraniteStreamingTextureConstantBuffer grSTCB, in gra_Float2 textureCoord)
gra_Float4 Granite_MergeResolveOutputs(in gra_Float4 resolve0, in gra_Float4 resolve1, in gra_Float2 pixelLocation)
gra_Float4 Granite_PackTileId(in gra_Float4 unpackedTileID)
void Granite_DitherResolveOutput(in gra_Float4 resolve, in RWTexture2D<GRA_UNORM gra_Float4> resolveTexture, in gra_Float2 screenPos, in float alpha)
float GranitePrivate_CalcMiplevelAnisotropic(in GraniteTilesetConstantBuffer tsCB, in GraniteStreamingTextureConstantBuffer grSTCB, in gra_Float2 ddxTc, in gra_Float2 ddyTc)
float GranitePrivate_CalcMiplevelLinear(in  GraniteTilesetConstantBuffer tsCB, in GraniteStreamingTextureConstantBuffer grSTCB, in gra_Float2 ddxTc, in gra_Float2 ddyTc)
gra_Float4 GranitePrivate_PackTileId(in gra_Float2 tileXY, in float level, in float textureID)
gra_Float4 GranitePrivate_UnpackTileId(in gra_Float4 packedTile)
gra_Float3 GranitePrivate_TranslateCoord(in GraniteTilesetConstantBuffer tsCB, in gra_Float2 inputTexCoord, in gra_Float4 translationData, in int layer, out gra_Float2 numPagesOnLevel)
gra_Float4 GranitePrivate_DrawDebugTiles(in gra_Float4 sourceColor, in gra_Float2 textureCoord, in gra_Float2 numPagesOnLevel)
gra_Float4 GranitePrivate_MakeResolveOutput(in GraniteTilesetConstantBuffer tsCB, in gra_Float2 tileXY, in float level)
gra_Float4 GranitePrivate_ResolverPixel(in GraniteTilesetConstantBuffer tsCB, in gra_Float2 inputTexCoord, in float LOD)
void GranitePrivate_CalculateCubemapCoordinates(in gra_Float3 inputTexCoord, in gra_Float3 dVx, in gra_Float3 dVy, in GraniteStreamingTextureCubeConstantBuffer transforms, out int faceIdx, out gra_Float2 texCoord, out gra_Float2 dX, out gra_Float2 dY)
void GranitePrivate_CalculateCubemapCoordinates(in gra_Float3 inputTexCoord, in GraniteStreamingTextureCubeConstantBuffer transforms, out int faceIdx, out gra_Float2 texCoord, out gra_Float2 dX, out gra_Float2 dY)
gra_Float2 Granite_GetTextureDimensions(in GraniteStreamingTextureConstantBuffer grSTCB)
```



### ImageBasedLighting

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/BSDF.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Random.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Sampling/Sampling.hlsl"
real PerceptualRoughnessToMipmapLevel(real perceptualRoughness, uint maxMipLevel)
real PerceptualRoughnessToMipmapLevel(real perceptualRoughness)
real PerceptualRoughnessToMipmapLevel(real perceptualRoughness, real NdotR)
real MipmapLevelToPerceptualRoughness(real mipmapLevel)
float3 ComputeViewFacingNormal(float3 V, float3 T)
real3 GetAnisotropicModifiedNormal(real3 grainDir, real3 N, real3 V, real anisotropy)
void GetGGXAnisotropicModifiedNormalAndRoughness(real3 bitangentWS, real3 tangentWS, real3 N, real3 V, real anisotropy, real perceptualRoughness, out real3 iblN, out real iblPerceptualRoughness)
real3 GetSpecularDominantDir(real3 N, real3 R, real perceptualRoughness, real NdotV)
void SampleGGXDir(real2 u, real3 V, real3x3 localToWorld, real roughness, out real3 L, out real NdotL, out real NdotH, out real VdotH, bool VeqN = false)
void SampleAnisoGGXDir(real2 u, real3 V, real3 N, real3 tangentX, real3 tangentY, real roughnessT, real roughnessB, out real3 H, out real3 L)
void SampleAnisoGGXVisibleNormal(float2 u, float3 V, float3x3 localToWorld, float roughnessX, float roughnessY, out float3 localV, out float3 localH, out float  VdotH)
void SampleGGXVisibleNormal(float2 u, float3 V, float3x3 localToWorld, float roughness, out float3 localV, out float3 localH,out float VdotH)
void ImportanceSampleLambert(real2 u, real3x3 localToWorld, out real3 L, out real NdotL, out real weightOverPdf)
void ImportanceSampleGGX(real2 u, real3 V, real3x3 localToWorld, real roughness, real NdotV, out real3 L, out real VdotH, out real NdotL, out real weightOverPdf)
void ImportanceSampleAnisoGGX(real2 u, real3 V, real3x3 localToWorld, real roughnessT, real roughnessB, real NdotV, out real3   L, out real VdotH, out real NdotL, out real weightOverPdf)
real4 IntegrateGGXAndDisneyDiffuseFGD(real NdotV, real roughness, uint sampleCount = 4096)
uint GetIBLRuntimeFilterSampleCount(uint mipLevel)
real4 IntegrateLD(TEXTURECUBE_PARAM(tex, sampl), TEXTURE2D(ggxIblSamples), real3 V, real3 N, real roughness, real index, real invOmegaP, uint sampleCount, bool prefilter, bool usePrecomputedSamples)
real4 IntegrateLDCharlie(TEXTURECUBE_PARAM(tex, sampl), real3 N, real roughness, uint sampleCount, real invFaceCenterTexelSolidAngle)
uint BinarySearchRow(uint j, real needle, TEXTURE2D(haystack), uint n)
real4 IntegrateLD_MIS(TEXTURECUBE_PARAM(envMap, sampler_envMap), TEXTURE2D(marginalRowDensities), TEXTURE2D(conditionalDensities), real3 V, real3 N, real roughness, real invOmegaP, uint width, uint height, uint sampleCount, bool prefilter)
float InfluenceFadeNormalWeight(float3 normal, float3 centerToPos)
```



### Macros

```c
//全部都是定义
// Some shader compiler don't support to do multiple ## for concatenation inside the same macro, it require an indirection.
// This is the purpose of this macro
#define MERGE_NAME(X, Y) X##Y
#define CALL_MERGE_NAME(X, Y) MERGE_NAME(X, Y)
// These define are use to abstract the way we sample into a cubemap array.
// Some platform don't support cubemap array so we fallback on 2D latlong
#ifdef  UNITY_NO_CUBEMAP_ARRAY
#define TEXTURECUBE_ARRAY_ABSTRACT TEXTURE2D_ARRAY
#define TEXTURECUBE_ARRAY_PARAM_ABSTRACT TEXTURE2D_ARRAY_PARAM
#define TEXTURECUBE_ARRAY_ARGS_ABSTRACT TEXTURE2D_ARRAY_ARGS
#define SAMPLE_TEXTURECUBE_ARRAY_LOD_ABSTRACT(textureName, samplerName, coord3, index, lod) SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, DirectionToLatLongCoordinate(coord3), index, lod)
#else
#define TEXTURECUBE_ARRAY_ABSTRACT TEXTURECUBE_ARRAY
#define TEXTURECUBE_ARRAY_PARAM_ABSTRACT TEXTURECUBE_ARRAY_PARAM
#define TEXTURECUBE_ARRAY_ARGS_ABSTRACT TEXTURECUBE_ARRAY_ARGS
#define SAMPLE_TEXTURECUBE_ARRAY_LOD_ABSTRACT(textureName, samplerName, coord3, index, lod) SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)
#endif
#define PI          3.14159265358979323846
#define TWO_PI      6.28318530717958647693
#define FOUR_PI     12.5663706143591729538
#define INV_PI      0.31830988618379067154
#define INV_TWO_PI  0.15915494309189533577
#define INV_FOUR_PI 0.07957747154594766788
#define HALF_PI     1.57079632679489661923
#define INV_HALF_PI 0.63661977236758134308
#define LOG2_E      1.44269504088896340736
#define INV_SQRT2   0.70710678118654752440
#define PI_DIV_FOUR 0.78539816339744830961
#define MILLIMETERS_PER_METER 1000
#define METERS_PER_MILLIMETER rcp(MILLIMETERS_PER_METER)
#define CENTIMETERS_PER_METER 100
#define METERS_PER_CENTIMETER rcp(CENTIMETERS_PER_METER)
#define FLT_INF  asfloat(0x7F800000)
#define FLT_EPS  5.960464478e-8  // 2^-24, machine epsilon: 1 + EPS = 1 (half of the ULP for 1.0f)
#define FLT_MIN  1.175494351e-38 // Minimum normalized positive floating-point number
#define FLT_MAX  3.402823466e+38 // Maximum representable floating-point number
#define HALF_EPS 4.8828125e-4    // 2^-11, machine epsilon: 1 + EPS = 1 (half of the ULP for 1.0f)
#define HALF_MIN 6.103515625e-5  // 2^-14, the same value for 10, 11 and 16-bit: https://www.khronos.org/opengl/wiki/Small_Float_Formats
#define HALF_MIN_SQRT 0.0078125  // 2^-7 == sqrt(HALF_MIN), useful for ensuring HALF_MIN after x^2
#define HALF_MAX 65504.0
#define UINT_MAX 0xFFFFFFFFu
#define INT_MAX  0x7FFFFFFF
#ifdef SHADER_API_GLES
#define GENERATE_INT_FLOAT_1_ARG(FunctionName, Parameter1, FunctionBody) \
    float  FunctionName(float  Parameter1) { FunctionBody; } \
    int    FunctionName(int  Parameter1) { FunctionBody; }
#else
#define GENERATE_INT_FLOAT_1_ARG(FunctionName, Parameter1, FunctionBody) \
    float  FunctionName(float  Parameter1) { FunctionBody; } \
    uint   FunctionName(uint  Parameter1) { FunctionBody; } \
    int    FunctionName(int  Parameter1) { FunctionBody; }
#endif
#define TEMPLATE_1_FLT(FunctionName, Parameter1, FunctionBody) \
    float  FunctionName(float  Parameter1) { FunctionBody; } \
    float2 FunctionName(float2 Parameter1) { FunctionBody; } \
    float3 FunctionName(float3 Parameter1) { FunctionBody; } \
    float4 FunctionName(float4 Parameter1) { FunctionBody; }
#define TEMPLATE_1_HALF(FunctionName, Parameter1, FunctionBody) \
    half  FunctionName(half  Parameter1) { FunctionBody; } \
    half2 FunctionName(half2 Parameter1) { FunctionBody; } \
    half3 FunctionName(half3 Parameter1) { FunctionBody; } \
    half4 FunctionName(half4 Parameter1) { FunctionBody; } \
    float  FunctionName(float  Parameter1) { FunctionBody; } \
    float2 FunctionName(float2 Parameter1) { FunctionBody; } \
    float3 FunctionName(float3 Parameter1) { FunctionBody; } \
    float4 FunctionName(float4 Parameter1) { FunctionBody; }
#ifdef SHADER_API_GLES
    #define TEMPLATE_1_INT(FunctionName, Parameter1, FunctionBody) \
    int    FunctionName(int    Parameter1) { FunctionBody; } \
    int2   FunctionName(int2   Parameter1) { FunctionBody; } \
    int3   FunctionName(int3   Parameter1) { FunctionBody; } \
    int4   FunctionName(int4   Parameter1) { FunctionBody; }
#else
    #define TEMPLATE_1_INT(FunctionName, Parameter1, FunctionBody) \
    int    FunctionName(int    Parameter1) { FunctionBody; } \
    int2   FunctionName(int2   Parameter1) { FunctionBody; } \
    int3   FunctionName(int3   Parameter1) { FunctionBody; } \
    int4   FunctionName(int4   Parameter1) { FunctionBody; } \
    uint   FunctionName(uint   Parameter1) { FunctionBody; } \
    uint2  FunctionName(uint2  Parameter1) { FunctionBody; } \
    uint3  FunctionName(uint3  Parameter1) { FunctionBody; } \
    uint4  FunctionName(uint4  Parameter1) { FunctionBody; }
#endif
#define TEMPLATE_2_FLT(FunctionName, Parameter1, Parameter2, FunctionBody) \
    float  FunctionName(float  Parameter1, float  Parameter2) { FunctionBody; } \
    float2 FunctionName(float2 Parameter1, float2 Parameter2) { FunctionBody; } \
    float3 FunctionName(float3 Parameter1, float3 Parameter2) { FunctionBody; } \
    float4 FunctionName(float4 Parameter1, float4 Parameter2) { FunctionBody; }
#define TEMPLATE_2_HALF(FunctionName, Parameter1, Parameter2, FunctionBody) \
    half  FunctionName(half  Parameter1, half  Parameter2) { FunctionBody; } \
    half2 FunctionName(half2 Parameter1, half2 Parameter2) { FunctionBody; } \
    half3 FunctionName(half3 Parameter1, half3 Parameter2) { FunctionBody; } \
    half4 FunctionName(half4 Parameter1, half4 Parameter2) { FunctionBody; } \
    float  FunctionName(float  Parameter1, float  Parameter2) { FunctionBody; } \
    float2 FunctionName(float2 Parameter1, float2 Parameter2) { FunctionBody; } \
    float3 FunctionName(float3 Parameter1, float3 Parameter2) { FunctionBody; } \
    float4 FunctionName(float4 Parameter1, float4 Parameter2) { FunctionBody; }
#ifdef SHADER_API_GLES
    #define TEMPLATE_2_INT(FunctionName, Parameter1, Parameter2, FunctionBody) \
    int    FunctionName(int    Parameter1, int    Parameter2) { FunctionBody; } \
    int2   FunctionName(int2   Parameter1, int2   Parameter2) { FunctionBody; } \
    int3   FunctionName(int3   Parameter1, int3   Parameter2) { FunctionBody; } \
    int4   FunctionName(int4   Parameter1, int4   Parameter2) { FunctionBody; }
#else
    #define TEMPLATE_2_INT(FunctionName, Parameter1, Parameter2, FunctionBody) \
    int    FunctionName(int    Parameter1, int    Parameter2) { FunctionBody; } \
    int2   FunctionName(int2   Parameter1, int2   Parameter2) { FunctionBody; } \
    int3   FunctionName(int3   Parameter1, int3   Parameter2) { FunctionBody; } \
    int4   FunctionName(int4   Parameter1, int4   Parameter2) { FunctionBody; } \
    uint   FunctionName(uint   Parameter1, uint   Parameter2) { FunctionBody; } \
    uint2  FunctionName(uint2  Parameter1, uint2  Parameter2) { FunctionBody; } \
    uint3  FunctionName(uint3  Parameter1, uint3  Parameter2) { FunctionBody; } \
    uint4  FunctionName(uint4  Parameter1, uint4  Parameter2) { FunctionBody; }
#endif
#define TEMPLATE_3_FLT(FunctionName, Parameter1, Parameter2, Parameter3, FunctionBody) \
    float  FunctionName(float  Parameter1, float  Parameter2, float  Parameter3) { FunctionBody; } \
    float2 FunctionName(float2 Parameter1, float2 Parameter2, float2 Parameter3) { FunctionBody; } \
    float3 FunctionName(float3 Parameter1, float3 Parameter2, float3 Parameter3) { FunctionBody; } \
    float4 FunctionName(float4 Parameter1, float4 Parameter2, float4 Parameter3) { FunctionBody; }
#define TEMPLATE_3_HALF(FunctionName, Parameter1, Parameter2, Parameter3, FunctionBody) \
    half  FunctionName(half  Parameter1, half  Parameter2, half  Parameter3) { FunctionBody; } \
    half2 FunctionName(half2 Parameter1, half2 Parameter2, half2 Parameter3) { FunctionBody; } \
    half3 FunctionName(half3 Parameter1, half3 Parameter2, half3 Parameter3) { FunctionBody; } \
    half4 FunctionName(half4 Parameter1, half4 Parameter2, half4 Parameter3) { FunctionBody; } \
    float  FunctionName(float  Parameter1, float  Parameter2, float  Parameter3) { FunctionBody; } \
    float2 FunctionName(float2 Parameter1, float2 Parameter2, float2 Parameter3) { FunctionBody; } \
    float3 FunctionName(float3 Parameter1, float3 Parameter2, float3 Parameter3) { FunctionBody; } \
    float4 FunctionName(float4 Parameter1, float4 Parameter2, float4 Parameter3) { FunctionBody; }
#ifdef SHADER_API_GLES
    #define TEMPLATE_3_INT(FunctionName, Parameter1, Parameter2, Parameter3, FunctionBody) \
    int    FunctionName(int    Parameter1, int    Parameter2, int    Parameter3) { FunctionBody; } \
    int2   FunctionName(int2   Parameter1, int2   Parameter2, int2   Parameter3) { FunctionBody; } \
    int3   FunctionName(int3   Parameter1, int3   Parameter2, int3   Parameter3) { FunctionBody; } \
    int4   FunctionName(int4   Parameter1, int4   Parameter2, int4   Parameter3) { FunctionBody; }
#else
    #define TEMPLATE_3_INT(FunctionName, Parameter1, Parameter2, Parameter3, FunctionBody) \
    int    FunctionName(int    Parameter1, int    Parameter2, int    Parameter3) { FunctionBody; } \
    int2   FunctionName(int2   Parameter1, int2   Parameter2, int2   Parameter3) { FunctionBody; } \
    int3   FunctionName(int3   Parameter1, int3   Parameter2, int3   Parameter3) { FunctionBody; } \
    int4   FunctionName(int4   Parameter1, int4   Parameter2, int4   Parameter3) { FunctionBody; } \
    uint   FunctionName(uint   Parameter1, uint   Parameter2, uint   Parameter3) { FunctionBody; } \
    uint2  FunctionName(uint2  Parameter1, uint2  Parameter2, uint2  Parameter3) { FunctionBody; } \
    uint3  FunctionName(uint3  Parameter1, uint3  Parameter2, uint3  Parameter3) { FunctionBody; } \
    uint4  FunctionName(uint4  Parameter1, uint4  Parameter2, uint4  Parameter3) { FunctionBody; }
#endif
#ifdef SHADER_API_GLES
    #define TEMPLATE_SWAP(FunctionName) \
    void FunctionName(inout real  a, inout real  b) { real  t = a; a = b; b = t; } \
    void FunctionName(inout real2 a, inout real2 b) { real2 t = a; a = b; b = t; } \
    void FunctionName(inout real3 a, inout real3 b) { real3 t = a; a = b; b = t; } \
    void FunctionName(inout real4 a, inout real4 b) { real4 t = a; a = b; b = t; } \
    void FunctionName(inout int    a, inout int    b) { int    t = a; a = b; b = t; } \
    void FunctionName(inout int2   a, inout int2   b) { int2   t = a; a = b; b = t; } \
    void FunctionName(inout int3   a, inout int3   b) { int3   t = a; a = b; b = t; } \
    void FunctionName(inout int4   a, inout int4   b) { int4   t = a; a = b; b = t; } \
    void FunctionName(inout bool   a, inout bool   b) { bool   t = a; a = b; b = t; } \
    void FunctionName(inout bool2  a, inout bool2  b) { bool2  t = a; a = b; b = t; } \
    void FunctionName(inout bool3  a, inout bool3  b) { bool3  t = a; a = b; b = t; } \
    void FunctionName(inout bool4  a, inout bool4  b) { bool4  t = a; a = b; b = t; }
#else
    #if REAL_IS_HALF
        #define TEMPLATE_SWAP(FunctionName) \
        void FunctionName(inout real  a, inout real  b) { real  t = a; a = b; b = t; } \
        void FunctionName(inout real2 a, inout real2 b) { real2 t = a; a = b; b = t; } \
        void FunctionName(inout real3 a, inout real3 b) { real3 t = a; a = b; b = t; } \
        void FunctionName(inout real4 a, inout real4 b) { real4 t = a; a = b; b = t; } \
        void FunctionName(inout float  a, inout float  b) { float  t = a; a = b; b = t; } \
        void FunctionName(inout float2 a, inout float2 b) { float2 t = a; a = b; b = t; } \
        void FunctionName(inout float3 a, inout float3 b) { float3 t = a; a = b; b = t; } \
        void FunctionName(inout float4 a, inout float4 b) { float4 t = a; a = b; b = t; } \
        void FunctionName(inout int    a, inout int    b) { int    t = a; a = b; b = t; } \
        void FunctionName(inout int2   a, inout int2   b) { int2   t = a; a = b; b = t; } \
        void FunctionName(inout int3   a, inout int3   b) { int3   t = a; a = b; b = t; } \
        void FunctionName(inout int4   a, inout int4   b) { int4   t = a; a = b; b = t; } \
        void FunctionName(inout uint   a, inout uint   b) { uint   t = a; a = b; b = t; } \
        void FunctionName(inout uint2  a, inout uint2  b) { uint2  t = a; a = b; b = t; } \
        void FunctionName(inout uint3  a, inout uint3  b) { uint3  t = a; a = b; b = t; } \
        void FunctionName(inout uint4  a, inout uint4  b) { uint4  t = a; a = b; b = t; } \
        void FunctionName(inout bool   a, inout bool   b) { bool   t = a; a = b; b = t; } \
        void FunctionName(inout bool2  a, inout bool2  b) { bool2  t = a; a = b; b = t; } \
        void FunctionName(inout bool3  a, inout bool3  b) { bool3  t = a; a = b; b = t; } \
        void FunctionName(inout bool4  a, inout bool4  b) { bool4  t = a; a = b; b = t; }
    #else
        #define TEMPLATE_SWAP(FunctionName) \
        void FunctionName(inout real  a, inout real  b) { real  t = a; a = b; b = t; } \
        void FunctionName(inout real2 a, inout real2 b) { real2 t = a; a = b; b = t; } \
        void FunctionName(inout real3 a, inout real3 b) { real3 t = a; a = b; b = t; } \
        void FunctionName(inout real4 a, inout real4 b) { real4 t = a; a = b; b = t; } \
        void FunctionName(inout int    a, inout int    b) { int    t = a; a = b; b = t; } \
        void FunctionName(inout int2   a, inout int2   b) { int2   t = a; a = b; b = t; } \
        void FunctionName(inout int3   a, inout int3   b) { int3   t = a; a = b; b = t; } \
        void FunctionName(inout int4   a, inout int4   b) { int4   t = a; a = b; b = t; } \
        void FunctionName(inout uint   a, inout uint   b) { uint   t = a; a = b; b = t; } \
        void FunctionName(inout uint2  a, inout uint2  b) { uint2  t = a; a = b; b = t; } \
        void FunctionName(inout uint3  a, inout uint3  b) { uint3  t = a; a = b; b = t; } \
        void FunctionName(inout uint4  a, inout uint4  b) { uint4  t = a; a = b; b = t; } \
        void FunctionName(inout bool   a, inout bool   b) { bool   t = a; a = b; b = t; } \
        void FunctionName(inout bool2  a, inout bool2  b) { bool2  t = a; a = b; b = t; } \
        void FunctionName(inout bool3  a, inout bool3  b) { bool3  t = a; a = b; b = t; } \
        void FunctionName(inout bool4  a, inout bool4  b) { bool4  t = a; a = b; b = t; }
    #endif
#endif
// MACRO from Legacy Untiy
// Transforms 2D UV by scale/bias property
#define TRANSFORM_TEX(tex, name) ((tex.xy) * name##_ST.xy + name##_ST.zw)
#define GET_TEXELSIZE_NAME(name) (name##_TexelSize)
#if UNITY_REVERSED_Z
#define COMPARE_DEVICE_DEPTH_CLOSER(shadowMapDepth, zDevice)      (shadowMapDepth >  zDevice)
#define COMPARE_DEVICE_DEPTH_CLOSEREQUAL(shadowMapDepth, zDevice) (shadowMapDepth >= zDevice)
#else
#define COMPARE_DEVICE_DEPTH_CLOSER(shadowMapDepth, zDevice)      (shadowMapDepth <  zDevice)
#define COMPARE_DEVICE_DEPTH_CLOSEREQUAL(shadowMapDepth, zDevice) (shadowMapDepth <= zDevice)
#endif
#endif // UNITY_MACROS_INCLUDED
```



### MetaPass

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
CBUFFER_START(UnityMetaPass)
    bool4 unity_MetaVertexControl;
    bool4 unity_MetaFragmentControl;
    int unity_VisualizationMode;
CBUFFER_END
struct UnityMetaInput
#ifdef EDITOR_VISUALIZATION
#define EDITORVIZ_PBR_VALIDATION_ALBEDO         0
#define EDITORVIZ_PBR_VALIDATION_METALSPECULAR  1
#define EDITORVIZ_TEXTURE                       2
#define EDITORVIZ_SHOWLIGHTMASK                 3
uniform sampler2D unity_EditorViz_Texture;
uniform half4 unity_EditorViz_Texture_ST;
uniform int unity_EditorViz_UVIndex;
uniform half4 unity_EditorViz_Decode_HDR;
uniform bool unity_EditorViz_ConvertToLinearSpace;
uniform half4 unity_EditorViz_ColorMul;
uniform half4 unity_EditorViz_ColorAdd;
uniform half unity_EditorViz_Exposure;
uniform sampler2D unity_EditorViz_LightTexture;
uniform sampler2D unity_EditorViz_LightTextureB;
#define unity_EditorViz_ChannelSelect unity_EditorViz_ColorMul
#define unity_EditorViz_Color         unity_EditorViz_ColorAdd
#define unity_EditorViz_LightType     unity_EditorViz_UVIndex
uniform float4x4 unity_EditorViz_WorldToLight;
float2 UnityMetaVizUV(int uvIndex, float2 uv0, float2 uv1, float2 uv2, float4 st)
void UnityEditorVizData(float3 positionOS, float2 uv0, float2 uv1, float2 uv2, float4 st, out float2 VizUV, out float4 LightCoord)
void UnityEditorVizData(float3 positionOS, float2 uv0, float2 uv1, float2 uv2, out float2 VizUV, out float4 LightCoord)
float4 UnityMetaVertexPosition(float3 vertex, float2 uv1, float2 uv2, float4 lightmapST, float4 dynlightmapST)
float4 UnityMetaVertexPosition(float3 vertex, float2 uv1, float2 uv2)
float unity_OneOverOutputBoost;
float unity_MaxOutputValue;
float unity_UseLinearSpace;
half4 UnityMetaFragment (UnityMetaInput IN)
```



### NormalSurfaceGradient

```c
void SurfaceGradientGenBasisTB(float3 nrmVertexNormal, float3 sigmaX, float3 sigmaY, float flipSign, float2 texST, out float3 vT, out float3 vB)
real3 SurfaceGradientFromTBN(real2 deriv, real3 vT, real3 vB)
real3 SurfaceGradientFromPerturbedNormal(real3 nrmVertexNormal, real3 v)
real3 SurfaceGradientFromVolumeGradient(real3 nrmVertexNormal, real3 grad)
real3 SurfaceGradientFromTriplanarProjection(real3 nrmVertexNormal, real3 triplanarWeights, real2 deriv_xplane, real2 deriv_yplane, real2 deriv_zplane)
real3 SurfaceGradientResolveNormal(real3 nrmVertexNormal, real3 surfGrad)
real2 ConvertTangentSpaceNormalToHeightMapGradient(real2 normalXY, real rcpNormalZ, real scale)
real3 SurfaceGradientFromTangentSpaceNormalAndFromTBN(real3 normalTS, real3 vT, real3 vB, real scale = 1.0)
real2 UnpackDerivativeNormalRGB(real4 packedNormal, real scale = 1.0)
real2 UnpackDerivativeNormalAG(real4 packedNormal, real scale = 1.0)
real2 UnpackDerivativeNormalRGorAG(real4 packedNormal, real scale = 1.0)
```



### Packing

```c
real3 PackNormalMaxComponent(real3 n)
real3 UnpackNormalMaxComponent(real3 n)
real2 PackNormalOctRectEncode(real3 n)
real3 UnpackNormalOctRectEncode(real2 f)
float2 PackNormalOctQuadEncode(float3 n)
float3 UnpackNormalOctQuadEncode(float2 f)
real2 PackNormalHemiOctEncode(real3 n)
real3 UnpackNormalHemiOctEncode(real2 f)
real2 PackNormalTetraEncode(float3 n, out uint faceIndex)
real3 UnpackNormalTetraEncode(real2 f, uint faceIndex)
real3 UnpackNormalRGB(real4 packedNormal, real scale = 1.0)
real3 UnpackNormalRGBNoScale(real4 packedNormal)
real3 UnpackNormalAG(real4 packedNormal, real scale = 1.0)
real3 UnpackNormalmapRGorAG(real4 packedNormal, real scale = 1.0)
real2 PackNormalTetraEncode(float3 n, out uint faceIndex)
real3 UnpackNormalTetraEncode(real2 f, uint faceIndex)
real3 UnpackNormalRGB(real4 packedNormal, real scale = 1.0)
real3 UnpackNormalRGBNoScale(real4 packedNormal)
real3 UnpackNormalAG(real4 packedNormal, real scale = 1.0)
real3 UnpackNormalmapRGorAG(real4 packedNormal, real scale = 1.0)
real3 UnpackNormal(real4 packedNormal)
real3 UnpackNormalScale(real4 packedNormal, real bumpScale)
real4 PackToLogLuv(real3 vRGB)
real3 UnpackFromLogLuv(real4 vLogLuv)
uint PackToR11G11B10f(float3 rgb)
float3 UnpackFromR11G11B10f(uint rgb)
float4 UnpackFromR8G8B8A8(uint rgba)
real4 UnpackQuat(real4 packedQuat)
real PackInt(uint i, uint numBits)
uint UnpackInt(real f, uint numBits)
real PackByte(uint i)
uint UnpackByte(real f)
real PackShort(uint i)
uint UnpackShort(real f)
real PackShortLo(uint i)
real PackShortHi(uint i)
real Pack2Byte(real2 inputs)
real2 Unpack2Byte(real inputs)
real PackFloatInt(real f, uint i, real maxi, real precision)
void UnpackFloatInt(real val, real maxi, real precision, out real f, out uint i)
real PackFloatInt8bit(real f, uint i, real maxi)
void UnpackFloatInt8bit(real val, real maxi, out real f, out uint i)
real PackFloatInt10bit(real f, uint i, real maxi)
void UnpackFloatInt10bit(real val, real maxi, out real f, out uint i)
real PackFloatInt16bit(real f, uint i, real maxi)
void UnpackFloatInt16bit(real val, real maxi, out real f, out uint i)
uint PackFloatToUInt(real src, uint offset, uint numBits)
real UnpackUIntToFloat(uint src, uint offset, uint numBits)
uint PackToR10G10B10A2(real4 rgba)
real4 UnpackFromR10G10B10A2(uint rgba)
real2 PackFloatToR8G8(real f)
real UnpackFloatFromR8G8(real2 f)
float3 PackFloat2To888(float2 f)
float2 Unpack888ToFloat2(float3 x)
float PackFloat2To8(float2 f)
float2 Unpack8ToFloat2(float f)
```



### ParallaxMapping

```c
half3 GetViewDirectionTangentSpace(half4 tangentWS, half3 normalWS, half3 viewDirWS)
half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
```



### PerPixelDisplacement

```c
real2 MERGE_NAME(ParallaxOcclusionMapping,POM_NAME_ID)
real2 ParallaxOcclusionMapping(real lod, real lodThreshold, int numSteps, real3 viewDirTS, PerPixelHeightDisplacementParam ppdParam, out real outHeight POM_USER_DATA_PARAMETERS)
```



### PhysicalCamera

```c
float ComputeEV100(float aperture, float shutterSpeed, float ISO)
float ComputeEV100FromAvgLuminance(float avgLuminance, float calibrationConstant)
float ComputeEV100FromAvgLuminance(float avgLuminance)
float ConvertEV100ToExposure(float EV100, float exposureScale)
float ConvertEV100ToExposure(float EV100)
float ComputeISO(float aperture, float shutterSpeed, float targetEV100)
float ComputeLuminanceAdaptation(float previousLuminance, float currentLuminance, float speedDarkToLight, float speedLightToDark, float deltaTime)
```



### Random

```c
float Hash(uint s)
#if !defined(SHADER_API_GLES)
uint JenkinsHash(uint x)
uint JenkinsHash(uint2 v)
uint JenkinsHash(uint3 v)
uint JenkinsHash(uint4 v)
float ConstructFloat(int m) 
float ConstructFloat(uint m)
float GenerateHashedRandomFloat(uint x)
float GenerateHashedRandomFloat(uint2 v)
float GenerateHashedRandomFloat(uint3 v)
float GenerateHashedRandomFloat(uint4 v)
float2 InitRandom(float2 input)
#endif // SHADER_API_GLES
float InterleavedGradientNoise(float2 pixCoord, int frameCount)
uint XorShift(inout uint rngState)
#endif // UNITY_RANDOM_INCLUDED
```



### Refraction

```c
struct RefractionModelResult
{
    real  dist;       // length of the transmission during refraction through the shape
    float3 positionWS; // out ray position
    real3 rayWS;      // out ray direction
};
RefractionModelResult RefractionModelSphere(real3 V, float3 positionWS, real3 normalWS, real ior, real thickness)
RefractionModelResult RefractionModelBox(real3 V, float3 positionWS, real3 normalWS, real ior, real thickness)
```



### SDF2D

```c
float CircleSDF(float2 position, float radius)
float RectangleSDF(float2 position, float2 bound)
float EllipseSDF(float2 position, float2 r)
```



### SpaceFillingCurves

```c
uint Part1By1(uint x)
uint Part1By2(uint x)
uint Compact1By1(uint x)
uint Compact1By2(uint x)
uint EncodeMorton2D(uint2 coord)
uint EncodeMorton3D(uint3 coord)
uint2 DecodeMorton2D(uint code)
uint3 DecodeMorton3D(uint code)
uint InterleaveQuad(uint2 quad)
uint2 DeinterleaveQuad(uint code)
```



### SpaceTransforms

```c
float4x4 GetObjectToWorldMatrix()
float4x4 GetWorldToObjectMatrix()
float4x4 GetPrevObjectToWorldMatrix()
float4x4 GetPrevWorldToObjectMatrix()
float4x4 GetWorldToViewMatrix()
float4x4 GetWorldToHClipMatrix()
float4x4 GetViewToHClipMatrix()
float3 GetAbsolutePositionWS(float3 positionRWS)
float3 GetCameraRelativePositionWS(float3 positionWS)
real GetOddNegativeScale()
float3 TransformObjectToWorld(float3 positionOS)
float3 TransformWorldToObject(float3 positionWS)
float3 TransformWorldToView(float3 positionWS)
float4 TransformObjectToHClip(float3 positionOS)
float4 TransformWorldToHClip(float3 positionWS)
float4 TransformWViewToHClip(float3 positionVS)
float3 TransformObjectToWorldDir(float3 dirOS, bool doNormalize = true)
float3 TransformWorldToObjectDir(float3 dirWS, bool doNormalize = true)
real3 TransformWorldToViewDir(real3 dirWS, bool doNormalize = false)
real3 TransformWorldToHClipDir(real3 directionWS, bool doNormalize = false)
float3 TransformObjectToWorldNormal(float3 normalOS, bool doNormalize = true)
float3 TransformWorldToObjectNormal(float3 normalWS, bool doNormalize = true)
real3x3 CreateTangentToWorld(real3 normal, real3 tangent, real flipSign)
real3 TransformTangentToWorld(float3 dirTS, real3x3 tangentToWorld)
real3 TransformWorldToTangent(real3 dirWS, real3x3 tangentToWorld)
real3 TransformTangentToObject(real3 dirTS, real3x3 tangentToWorld)
real3 TransformObjectToTangent(real3 dirOS, real3x3 tangentToWorld)
```



### Tessellation

```c
#define TESSELLATION_INTERPOLATE_BARY(name, bary) output.name = input0.name * bary.x +  input1.name * bary.y +  input2.name * bary.z
real3 PhongTessellation(real3 positionWS, real3 p0, real3 p1, real3 p2, real3 n0, real3 n1, real3 n2, real3 baryCoords, real shape)
real3 GetScreenSpaceTessFactor(real3 p0, real3 p1, real3 p2, real4x4 viewProjectionMatrix, real4 screenSize, real triangleSize)
real3 GetDistanceBasedTessFactor(real3 p0, real3 p1, real3 p2, real3 cameraPosWS, real tessMinDist, real tessMaxDist)
real4 CalcTriTessFactorsFromEdgeTessFactors(real3 triVertexFactors)
```



### Texture

```c
struct GLES2UnsupportedSamplerState {};
UNITY_BARE_SAMPLER(default_sampler_Linear_Repeat);
struct UnitySamplerState
{
	UNITY_BARE_SAMPLER(samplerstate);
};
UnitySamplerState UnityBuildSamplerStateStructInternal(SAMPLER(samplerstate))
struct UnityTexture2D//拿struct当类用
{
    TEXTURE2D(tex);
    UNITY_BARE_SAMPLER(samplerstate);
    float4 texelSize;
    float4 scaleTranslate;
    //Functions
    float4 Sample(UnitySamplerState s, float2 uv)
    float4 SampleLevel(UnitySamplerState s, float2 uv, float lod)
    float4 SampleBias(UnitySamplerState s, float2 uv, float bias)
    float4 SampleGrad(UnitySamplerState s, float2 uv, float2 dpdx, float2 dpdy)
    float2 GetTransformedUV(float2 uv)
    float CalculateLevelOfDetail(UnitySamplerState s, float2 uv)
    float4 Sample(SAMPLER(s), float2 uv)
    float4 SampleLevel(SAMPLER(s), float2 uv, float lod)
    float4 SampleBias(SAMPLER(s), float2 uv, float bias)
    float4 SampleGrad(SAMPLER(s), float2 uv, float2 dpdx, float2 dpdy)
    float4 SampleCmpLevelZero(SAMPLER_CMP(s), float2 uv, float cmp)
    float4 Load(int3 pixel)
    float CalculateLevelOfDetail(SAMPLER(s), float2 uv)
    float4 Gather(UnitySamplerState s, float2 uv)
    float4 GatherRed(UnitySamplerState s, float2 uv)
    float4 GatherGreen(UnitySamplerState s, float2 uv)
    float4 GatherBlue(UnitySamplerState s, float2 uv)
    float4 GatherAlpha(UnitySamplerState s, float2 uv)
    float4 Gather(SAMPLER(s), float2 uv)
    float4 GatherRed(SAMPLER(s), float2 uv)
    float4 GatherGreen(SAMPLER(s), float2 uv)
    float4 GatherBlue(SAMPLER(s), float2 uv)
    float4 GatherAlpha(SAMPLER(s), float2 uv)
};
float4 tex2D(UnityTexture2D tex, float2 uv)                 
float4 tex2Dlod(UnityTexture2D tex, float4 uv0l)            
float4 tex2Dbias(UnityTexture2D tex, float4 uv0b)           
UnityTexture2D UnityBuildTexture2DStructInternal(TEXTURE2D_PARAM(tex, samplerstate), float4 texelSize, float4 scaleTranslate)
struct UnityTexture2DArray
{
    TEXTURE2D_ARRAY(tex);
    UNITY_BARE_SAMPLER(samplerstate);
    //Functions
    float4 Sample(UnitySamplerState s, float3 uv)
    float4 SampleLevel(UnitySamplerState s, float3 uv, float lod)
    float4 SampleBias(UnitySamplerState s, float3 uv, float bias)
    float4 SampleGrad(UnitySamplerState s, float3 uv, float2 dpdx, float2 dpdy)
    float4 Sample(SAMPLER(s), float3 uv)
    float4 SampleLevel(SAMPLER(s), float3 uv, float lod)
    float4 SampleBias(SAMPLER(s), float3 uv, float bias)
    float4 SampleGrad(SAMPLER(s), float3 uv, float2 dpdx, float2 dpdy)
    float4 SampleCmpLevelZero(SAMPLER_CMP(s), float3 uv, float cmp)
    float4 Load(int4 pixel)
};
                                                     
UnityTexture2DArray UnityBuildTexture2DArrayStructInternal(TEXTURE2D_ARRAY_PARAM(tex, samplerstate))
struct UnityTextureCube
{
    TEXTURECUBE(tex);
    UNITY_BARE_SAMPLER(samplerstate);
    //Functions
    float4 Sample(UnitySamplerState s, float3 dir)
    float4 SampleLevel(UnitySamplerState s, float3 dir, float lod)
    float4 SampleBias(UnitySamplerState s, float3 dir, float bias)
    float4 Sample(SAMPLER(s), float3 dir)
    float4 SampleLevel(SAMPLER(s), float3 dir, float lod)
    float4 SampleBias(SAMPLER(s), float3 dir, float bias)
    float4 Gather(UnitySamplerState s, float3 dir)
    float4 Gather(SAMPLER(s), float3 dir)
};
float4 texCUBE(UnityTextureCube tex, float3 dir)                        
float4 texCUBEbias(UnityTextureCube tex, float4 dirBias)                
UnityTextureCube UnityBuildTextureCubeStructInternal(TEXTURECUBE_PARAM(tex, samplerstate))
struct UnityTexture3D
{
    TEXTURE3D(tex);
    UNITY_BARE_SAMPLER(samplerstate);
    //Functions
    float4 Sample(UnitySamplerState s, float3 uvw)
    float4 SampleLevel(UnitySamplerState s, float3 uvw, float lod)      
    float4 Sample(SAMPLER(s), float3 uvw)                               
    float4 SampleLevel(SAMPLER(s), float3 uvw, float lod)               
    float4 Load(int4 pixel)
};
float4 tex3D(UnityTexture3D tex, float3 uvw)                            
UnityTexture3D UnityBuildTexture3DStructInternal(TEXTURE3D_PARAM(tex, samplerstate))
```



### TextureStack

```c
#include "VirtualTexturing.hlsl"
#include "Packing.hlsl"
#if defined(UNITY_VIRTUAL_TEXTURING) && !defined(FORCE_VIRTUAL_TEXTURING_OFF)
struct StackInfo
{
    GraniteLookupData lookupData;
    GraniteLODLookupData lookupDataLod;
    float4 resolveOutput;
};
struct VTProperty
{
    GraniteConstantBuffers grCB;
    GraniteTranslationTexture translationTable;
    GraniteCacheTexture cacheLayer[4];
    int layerCount;
    int layerIndex[4];
};
#ifdef TEXTURESTACK_CLAMP
    #define GR_LOOKUP Granite_Lookup_Clamp_Linear
    #define GR_LOOKUP_LOD Granite_Lookup_Clamp
#else
    #define GR_LOOKUP Granite_Lookup_Anisotropic
    #define GR_LOOKUP_LOD Granite_Lookup
#endif
#ifndef RESOLVE_SCALE_OVERRIDE
#define RESOLVE_SCALE_OVERRIDE float2(1,1)
#endif
#ifndef VT_CACHE_SAMPLER
    #define VT_CACHE_SAMPLER sampler_clamp_trilinear_aniso4
    SAMPLER(VT_CACHE_SAMPLER);
#endif
//Compute Buffer缓冲区
StructuredBuffer<GraniteTilesetConstantBuffer> _VTTilesetBuffer;
#define DECLARE_STACK_CB(stackName) \ //承接下行
    float4 stackName##_atlasparams[2]
#define DECLARE_STACK_BASE(stackName) \//..可以跟好几行，变量和函数都包括
GraniteTilesetConstantBuffer GetConstantBuffer(GraniteStreamingTextureConstantBuffer textureParamBlock)
#define jj2(a, b) a##b
#define jj(a, b) jj2(a, b)
#define DECLARE_STACK_LAYER(stackName, layerSamplerName, layerIndex) \
TEXTURE2D_ARRAY(stackName##_c##layerIndex);
#define DECLARE_BUILD_PROPERTIES(stackName, layers, layer0Index, layer1Index, layer2Index, layer3Index)\//etc    
#define DECLARE_STACK(stackName, layer0SamplerName)\
    DECLARE_STACK_BASE(stackName)\
    DECLARE_STACK_LAYER(stackName, layer0SamplerName, 0)\
    DECLARE_BUILD_PROPERTIES(stackName, 1, 0, 0, 0, 0)
#define DECLARE_STACK2(stackName, layer0SamplerName, layer1SamplerName)\
    DECLARE_STACK_BASE(stackName)\
    DECLARE_STACK_LAYER(stackName, layer0SamplerName, 0)\
    DECLARE_STACK_LAYER(stackName, layer1SamplerName, 1)\
    DECLARE_BUILD_PROPERTIES(stackName, 2, 0, 1, 1, 1)
#define DECLARE_STACK3(stackName, layer0SamplerName, layer1SamplerName, layer2SamplerName)\
    DECLARE_STACK_BASE(stackName)\
    DECLARE_STACK_LAYER(stackName, layer0SamplerName, 0)\
    DECLARE_STACK_LAYER(stackName, layer1SamplerName, 1)\
    DECLARE_STACK_LAYER(stackName, layer2SamplerName, 2)\
    DECLARE_BUILD_PROPERTIES(stackName, 3, 0, 1, 2, 2)
#define DECLARE_STACK4(stackName, layer0SamplerName, layer1SamplerName, layer2SamplerName, layer3SamplerName)\
    DECLARE_STACK_BASE(stackName)\
    DECLARE_STACK_LAYER(stackName, layer0SamplerName, 0)\
    DECLARE_STACK_LAYER(stackName, layer1SamplerName, 1)\
    DECLARE_STACK_LAYER(stackName, layer2SamplerName, 2)\
    DECLARE_STACK_LAYER(stackName, layer3SamplerName, 3)\
    DECLARE_BUILD_PROPERTIES(stackName, 4, 0, 1, 2, 3)
#define PrepareStack(inputParams, stackName) PrepareVT_##stackName(inputParams)
#define SampleStack(info, lodMode, quality, textureName) SampleVT_##textureName(info, lodMode, quality)
#define GetResolveOutput(info) info.resolveOutput
#define PackResolveOutput(output) Granite_PackTileId(output)
StackInfo PrepareVT(VTProperty vtProperty, VtInputParameters vtParams)
float4 SampleVTLayer(VTProperty vtProperty, VtInputParameters vtParams, StackInfo info, int layerIndex)
float4 GetPackedVTFeedback(float4 feedback)
#define VIRTUAL_TEXTURING_SHADER_ENABLED
#define DECLARE_BUILD_PROPERTIES(stackName, layers, layer0, layer1, layer2, layer3)//etc
#define DECLARE_STACK(stackName, layer0)//etc
#define DECLARE_STACK2(stackName, layer0, layer1)//etc
#define DECLARE_STACK3(stackName, layer0, layer1, layer2)//etc
#define DECLARE_STACK4(stackName, layer0, layer1, layer2, layer3)//etc
#define DECLARE_STACK_CB(stackName)
struct StackInfo
{
    VtInputParameters vt;
};
struct VTProperty
{
    int layerCount;
    TEXTURE2D(Layer0);
    TEXTURE2D(Layer1);
    TEXTURE2D(Layer2);
    TEXTURE2D(Layer3);
	#ifndef SHADER_API_GLES
    SAMPLER(samplerLayer0);
    SAMPLER(samplerLayer1);
    SAMPLER(samplerLayer2);
    SAMPLER(samplerLayer3);
	#endif
};
StackInfo MakeStackInfo(VtInputParameters vt)
#define PrepareStack(inputParams, stackName) MakeStackInfo(inputParams)
#define SampleStack(info, vtLevelMode, quality, texture) \
    SampleVTFallbackToTexture(info, vtLevelMode, TEXTURE2D_ARGS(texture, sampler##texture))
float4 SampleVTFallbackToTexture(StackInfo info, int vtLevelMode, TEXTURE2D_PARAM(layerTexture, layerSampler))
StackInfo PrepareVT(VTProperty vtProperty, VtInputParameters vtParams)
#define SampleVTLayer(vtProperty, vtParams, info, layerIndex) \
    SampleVTFallbackToTexture(info, vtParams.levelMode, TEXTURE2D_ARGS(vtProperty.Layer##layerIndex, vtProperty.samplerLayer##layerIndex))
#define GetResolveOutput(info) float4(1,1,1,1)
#define PackResolveOutput(output) output
#define GetPackedVTFeedback(feedback) feedback
#define TEXTURETYPE_DEFAULT 0                   // LayerTextureType.Default
#define TEXTURETYPE_NORMALTANGENTSPACE 1        // LayerTextureType.NormalTangentSpace
#define TEXTURETYPE_NORMALOBJECTSPACE 2         // LayerTextureType.NormalObjectSpace
struct VTPropertyWithTextureType
{
    VTProperty vtProperty;
    int layerTextureType[4];
};
VTPropertyWithTextureType AddTextureType(VTProperty vtProperty, int layer0TextureType, int layer1TextureType = TEXTURETYPE_DEFAULT, int layer2TextureType = TEXTURETYPE_DEFAULT, int layer3TextureType = TEXTURETYPE_DEFAULT)
float4 ApplyTextureType(float4 value, int textureType)
#define SampleVTLayerWithTextureType(vtProperty, vtParams, info, layerIndex) \
    ApplyTextureType(SampleVTLayer(vtProperty.vtProperty, vtParams, info, layerIndex), vtProperty.layerTextureType[layerIndex])
```



### UnityDOTSInstancing

```c
//##是C++里的连接写法，Unity在此做了特殊处理
#define UNITY_DOTS_INSTANCING_CONCAT2(a, b) a ## b
#define UNITY_DOTS_INSTANCING_CONCAT4(a, b, c, d) a ## b ## c ## d
ByteAddressBuffer unity_DOTSInstanceData;
struct DOTSVisibleData
{
    uint4 VisibleData;
};
CBUFFER_START(UnityInstancingDOTS_InstanceVisibility)
	DOTSVisibleData unity_DOTSVisibleInstances[UNITY_INSTANCED_ARRAY_SIZE];
CBUFFER_END
uint GetDOTSInstanceIndex()
uint ComputeDOTSInstanceDataAddress(uint metadata, uint stride)
#define DEFINE_DOTS_LOAD_INSTANCE_SCALAR(type, conv, sizeof_type) \
type LoadDOTSInstancedData_##type(uint metadata) \
{ \
    uint address = ComputeDOTSInstanceDataAddress(metadata, sizeof_type); \
    return conv(unity_DOTSInstanceData.Load(address)); \
}
#define DEFINE_DOTS_LOAD_INSTANCE_VECTOR(type, width, conv, sizeof_type) \
type##width LoadDOTSInstancedData_##type##width(uint metadata) \
{ \
    uint address = ComputeDOTSInstanceDataAddress(metadata, sizeof_type * width); \
    return conv(unity_DOTSInstanceData.Load##width(address)); \
}
DEFINE_DOTS_LOAD_INSTANCE_SCALAR(float, asfloat, 4)
DEFINE_DOTS_LOAD_INSTANCE_SCALAR(int,   int,     4)
DEFINE_DOTS_LOAD_INSTANCE_SCALAR(uint,  uint,    4)
DEFINE_DOTS_LOAD_INSTANCE_SCALAR(half,  half,    2)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(float, 2, asfloat, 4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(float, 3, asfloat, 4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(float, 4, asfloat, 4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(int,   2, int2,    4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(int,   3, int3,    4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(int,   4, int4,    4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(uint,  2, uint2,   4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(uint,  3, uint3,   4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(uint,  4, uint4,   4)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(half,  2, half2,   2)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(half,  3, half3,   2)
DEFINE_DOTS_LOAD_INSTANCE_VECTOR(half,  4, half4,   2)
float4x4 LoadDOTSInstancedData_float4x4(uint metadata)
float4x4 LoadDOTSInstancedData(float4x4 dummy, uint metadata) 
float4x4 LoadDOTSInstancedData_float4x4_from_float3x4(uint metadata)
float2x4 LoadDOTSInstancedData_float2x4(uint metadata)
float2x4 LoadDOTSInstancedData(float2x4 dummy, uint metadata) 
```



### UnityInstancing

```c
//省略各种眼花缭乱的#define
void UnitySetupInstanceID(uint inputInstanceID)
void UNITY_INSTANCING_PROCEDURAL_FUNC()
//不同分支的同名#define
#define UNITY_TRANSFER_INSTANCE_ID(input, output)   output.instanceID = UNITY_GET_INSTANCE_ID(input)
#define UNITY_SETUP_INSTANCE_ID(input) DEFAULT_UNITY_SETUP_INSTANCE_ID(input)
```



### Version

```c
#define SHADER_LIBRARY_VERSION_MAJOR 12
#define SHADER_LIBRARY_VERSION_MINOR 1

#define VERSION_GREATER_EQUAL(major, minor) ((SHADER_LIBRARY_VERSION_MAJOR > major) || ((SHADER_LIBRARY_VERSION_MAJOR == major) && (SHADER_LIBRARY_VERSION_MINOR >= minor)))
#define VERSION_LOWER(major, minor) ((SHADER_LIBRARY_VERSION_MAJOR < major) || ((SHADER_LIBRARY_VERSION_MAJOR == major) && (SHADER_LIBRARY_VERSION_MINOR < minor)))
#define VERSION_EQUAL(major, minor) ((SHADER_LIBRARY_VERSION_MAJOR == major) && (SHADER_LIBRARY_VERSION_MINOR == minor))
```



### VirtualTexturing

```c
struct VtInputParameters
{
    float2 uv;
    float lodOrOffset;
    float2 dx;
    float2 dy;
    int addressMode;
    int filterMode;
    int levelMode;
    int uvMode;
    int sampleQuality;
    int enableGlobalMipBias;
};
int VirtualTexturingLookup(
    in GraniteConstantBuffers grCB,
    in GraniteTranslationTexture translationTable,
    in VtInputParameters input,
    out GraniteLookupData graniteLookupData,
    out float4 resolveResult
)
int VirtualTexturingSample(
    in GraniteTilesetConstantBuffer tsCB,
    in GraniteLookupData graniteLookupData,
    in GraniteCacheTexture cacheTexture,
    in int layer,
    in int levelMode,
    in int quality,
    out float4 result)
```



### VolumeRendering

```c
real TransmittanceFromOpticalDepth(real opticalDepth)
real3 TransmittanceFromOpticalDepth(real3 opticalDepth)
real OpacityFromOpticalDepth(real opticalDepth)
real3 OpacityFromOpticalDepth(real3 opticalDepth)
real OpticalDepthFromOpacity(real opacity)
real3 OpticalDepthFromOpacity(real3 opacity)
real4 LinearizeRGBA(real4 value)
real4 LinearizeRGBD(real4 value)
real4 DelinearizeRGBA(real4 value)
real4 DelinearizeRGBD(real4 value)
real OpticalDepthHomogeneousMedium(real extinction, real intervalLength)
real TransmittanceHomogeneousMedium(real extinction, real intervalLength)
real TransmittanceIntegralHomogeneousMedium(real extinction, real intervalLength)
real ComputeHeightFogMultiplier(real height, real baseHeight, real2 heightExponents)
real OpticalDepthHeightFog(real baseExtinction, real baseHeight, real2 heightExponents, real cosZenith, real startHeight, real intervalLength)
real OpticalDepthHeightFog(real baseExtinction, real baseHeight, real2 heightExponents, real cosZenith, real startHeight)
real TransmittanceHeightFog(real baseExtinction, real baseHeight, real2 heightExponents, real cosZenith, real startHeight, real intervalLength)
real TransmittanceHeightFog(real baseExtinction, real baseHeight, real2 heightExponents, real cosZenith, real startHeight)
real IsotropicPhaseFunction()
real RayleighPhaseFunction(real cosTheta)
real HenyeyGreensteinPhasePartConstant(real anisotropy)
real HenyeyGreensteinPhasePartVarying(real anisotropy, real cosTheta)
real HenyeyGreensteinPhaseFunction(real anisotropy, real cosTheta)
real CornetteShanksPhasePartConstant(real anisotropy)
real CornetteShanksPhasePartSymmetrical(real cosTheta)
real CornetteShanksPhasePartAsymmetrical(real anisotropy, real cosTheta)
real CornetteShanksPhasePartVarying(real anisotropy, real cosTheta)
real CornetteShanksPhaseFunction(real anisotropy, real cosTheta)
void ImportanceSampleHomogeneousMedium(real rndVal, real extinction, real intervalLength, out real offset, out real weight)
void ImportanceSampleExponentialMedium(real rndVal, real extinction, real falloff, out real offset, out real rcpPdf)
void ImportanceSamplePunctualLight(real rndVal, real3 lightPosition, real lightSqRadius, real3 rayOrigin, real3 rayDirection, real tMin, real tMax, out real t, out real sqDist, out real rcpPdf)
real ImportanceSampleRayleighPhase(real rndVal)
real3 TransmittanceColorAtDistanceToAbsorption(real3 transmittanceColor, real atDistance)
```



## API

### D3D11

```c
// This file assume SHADER_API_D3D11 is defined
#define UNITY_UV_STARTS_AT_TOP 1
#define UNITY_REVERSED_Z 1
#define UNITY_NEAR_CLIP_VALUE (1.0)
// This value will not go through any matrix projection conversion
#define UNITY_RAW_FAR_CLIP_VALUE (0.0)
#define VERTEXID_SEMANTIC SV_VertexID
#define INSTANCEID_SEMANTIC SV_InstanceID
#define FRONT_FACE_SEMANTIC SV_IsFrontFace
#define FRONT_FACE_TYPE bool
#define IS_FRONT_VFACE(VAL, FRONT, BACK) ((VAL) ? (FRONT) : (BACK))

// Only for d3d11 we need to have specific sv_position qualifiers in the case of a conservative depth offset
#ifdef _CONSERVATIVE_DEPTH_OFFSET
#undef SV_POSITION_QUALIFIERS
#define SV_POSITION_QUALIFIERS linear noperspective centroid
#undef DEPTH_OFFSET_SEMANTIC
#define DEPTH_OFFSET_SEMANTIC SV_DepthLessEqual
#endif

#define CBUFFER_START(name) cbuffer name {
#define CBUFFER_END };

#define PLATFORM_SUPPORTS_EXPLICIT_BINDING
#define PLATFORM_NEEDS_UNORM_UAV_SPECIFIER
#define PLATFORM_SUPPORTS_BUFFER_ATOMICS_IN_PIXEL_SHADER
#define PLATFORM_SUPPORTS_PRIMITIVE_ID_IN_PIXEL_SHADER


// flow control attributes
#define UNITY_BRANCH        [branch]
#define UNITY_FLATTEN       [flatten]
#define UNITY_UNROLL        [unroll]
#define UNITY_UNROLLX(_x)   [unroll(_x)]
#define UNITY_LOOP          [loop]

// Initialize arbitrary structure with zero values.
// Do not exist on some platform, in this case we need to have a standard name that call a function that will initialize all parameters to 0
#define ZERO_INITIALIZE(type, name) name = (type)0;
#define ZERO_INITIALIZE_ARRAY(type, name, arraySize) { for (int arrayIndex = 0; arrayIndex < arraySize; arrayIndex++) { name[arrayIndex] = (type)0; } }

// Texture util abstraction

#define CALCULATE_TEXTURE2D_LOD(textureName, samplerName, coord2) textureName.CalculateLevelOfDetail(samplerName, coord2)

// Texture abstraction

#define TEXTURE2D(textureName)                Texture2D textureName
#define TEXTURE2D_ARRAY(textureName)          Texture2DArray textureName
#define TEXTURECUBE(textureName)              TextureCube textureName
#define TEXTURECUBE_ARRAY(textureName)        TextureCubeArray textureName
#define TEXTURE3D(textureName)                Texture3D textureName

#define TEXTURE2D_FLOAT(textureName)          TEXTURE2D(textureName)
#define TEXTURE2D_ARRAY_FLOAT(textureName)    TEXTURE2D_ARRAY(textureName)
#define TEXTURECUBE_FLOAT(textureName)        TEXTURECUBE(textureName)
#define TEXTURECUBE_ARRAY_FLOAT(textureName)  TEXTURECUBE_ARRAY(textureName)
#define TEXTURE3D_FLOAT(textureName)          TEXTURE3D(textureName)

#define TEXTURE2D_HALF(textureName)           TEXTURE2D(textureName)
#define TEXTURE2D_ARRAY_HALF(textureName)     TEXTURE2D_ARRAY(textureName)
#define TEXTURECUBE_HALF(textureName)         TEXTURECUBE(textureName)
#define TEXTURECUBE_ARRAY_HALF(textureName)   TEXTURECUBE_ARRAY(textureName)
#define TEXTURE3D_HALF(textureName)           TEXTURE3D(textureName)

#define TEXTURE2D_SHADOW(textureName)         TEXTURE2D(textureName)
#define TEXTURE2D_ARRAY_SHADOW(textureName)   TEXTURE2D_ARRAY(textureName)
#define TEXTURECUBE_SHADOW(textureName)       TEXTURECUBE(textureName)
#define TEXTURECUBE_ARRAY_SHADOW(textureName) TEXTURECUBE_ARRAY(textureName)

#define RW_TEXTURE2D(type, textureName)       RWTexture2D<type> textureName
#define RW_TEXTURE2D_ARRAY(type, textureName) RWTexture2DArray<type> textureName
#define RW_TEXTURE3D(type, textureName)       RWTexture3D<type> textureName

#define SAMPLER(samplerName)                  SamplerState samplerName
#define SAMPLER_CMP(samplerName)              SamplerComparisonState samplerName
#define ASSIGN_SAMPLER(samplerName, samplerValue) samplerName = samplerValue

#define TEXTURE2D_PARAM(textureName, samplerName)                 TEXTURE2D(textureName),         SAMPLER(samplerName)
#define TEXTURE2D_ARRAY_PARAM(textureName, samplerName)           TEXTURE2D_ARRAY(textureName),   SAMPLER(samplerName)
#define TEXTURECUBE_PARAM(textureName, samplerName)               TEXTURECUBE(textureName),       SAMPLER(samplerName)
#define TEXTURECUBE_ARRAY_PARAM(textureName, samplerName)         TEXTURECUBE_ARRAY(textureName), SAMPLER(samplerName)
#define TEXTURE3D_PARAM(textureName, samplerName)                 TEXTURE3D(textureName),         SAMPLER(samplerName)

#define TEXTURE2D_SHADOW_PARAM(textureName, samplerName)          TEXTURE2D(textureName),         SAMPLER_CMP(samplerName)
#define TEXTURE2D_ARRAY_SHADOW_PARAM(textureName, samplerName)    TEXTURE2D_ARRAY(textureName),   SAMPLER_CMP(samplerName)
#define TEXTURECUBE_SHADOW_PARAM(textureName, samplerName)        TEXTURECUBE(textureName),       SAMPLER_CMP(samplerName)
#define TEXTURECUBE_ARRAY_SHADOW_PARAM(textureName, samplerName)  TEXTURECUBE_ARRAY(textureName), SAMPLER_CMP(samplerName)

#define TEXTURE2D_ARGS(textureName, samplerName)                textureName, samplerName
#define TEXTURE2D_ARRAY_ARGS(textureName, samplerName)          textureName, samplerName
#define TEXTURECUBE_ARGS(textureName, samplerName)              textureName, samplerName
#define TEXTURECUBE_ARRAY_ARGS(textureName, samplerName)        textureName, samplerName
#define TEXTURE3D_ARGS(textureName, samplerName)                textureName, samplerName

#define TEXTURE2D_SHADOW_ARGS(textureName, samplerName)         textureName, samplerName
#define TEXTURE2D_ARRAY_SHADOW_ARGS(textureName, samplerName)   textureName, samplerName
#define TEXTURECUBE_SHADOW_ARGS(textureName, samplerName)       textureName, samplerName
#define TEXTURECUBE_ARRAY_SHADOW_ARGS(textureName, samplerName) textureName, samplerName

#define PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               textureName.Sample(samplerName, coord2)
#define PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      textureName.SampleLevel(samplerName, coord2, lod)
#define PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    textureName.SampleBias(samplerName, coord2, bias)
#define PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              textureName.SampleGrad(samplerName, coord2, dpdx, dpdy)
#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  textureName.Sample(samplerName, float3(coord2, index))
#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         textureName.SampleLevel(samplerName, float3(coord2, index), lod)
#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       textureName.SampleBias(samplerName, float3(coord2, index), bias)
#define PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) textureName.SampleGrad(samplerName, float3(coord2, index), dpdx, dpdy)
#define PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             textureName.Sample(samplerName, coord3)
#define PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    textureName.SampleLevel(samplerName, coord3, lod)
#define PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  textureName.SampleBias(samplerName, coord3, bias)
#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                textureName.Sample(samplerName, float4(coord3, index))
#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       textureName.SampleLevel(samplerName, float4(coord3, index), lod)
#define PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     textureName.SampleBias(samplerName, float4(coord3, index), bias)
#define PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               textureName.Sample(samplerName, coord3)
#define PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      textureName.SampleLevel(samplerName, coord3, lod)

#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)                               PLATFORM_SAMPLE_TEXTURE2D(textureName, samplerName, coord2)
#define SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)                      PLATFORM_SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod)
#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)                    PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias)
#define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)              PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy)
#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)                  PLATFORM_SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)
#define SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)         PLATFORM_SAMPLE_TEXTURE2D_ARRAY_LOD(textureName, samplerName, coord2, index, lod)
#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)       PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias)
#define SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy) PLATFORM_SAMPLE_TEXTURE2D_ARRAY_GRAD(textureName, samplerName, coord2, index, dpdx, dpdy)
#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)                             PLATFORM_SAMPLE_TEXTURECUBE(textureName, samplerName, coord3)
#define SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)                    PLATFORM_SAMPLE_TEXTURECUBE_LOD(textureName, samplerName, coord3, lod)
#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)                  PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias)
#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)                PLATFORM_SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)
#define SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)       PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_LOD(textureName, samplerName, coord3, index, lod)
#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)     PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)
#define SAMPLE_TEXTURE3D(textureName, samplerName, coord3)                               PLATFORM_SAMPLE_TEXTURE3D(textureName, samplerName, coord3)
#define SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)                      PLATFORM_SAMPLE_TEXTURE3D_LOD(textureName, samplerName, coord3, lod)

#define SAMPLE_TEXTURE2D_SHADOW(textureName, samplerName, coord3)                    textureName.SampleCmpLevelZero(samplerName, (coord3).xy, (coord3).z)
#define SAMPLE_TEXTURE2D_ARRAY_SHADOW(textureName, samplerName, coord3, index)       textureName.SampleCmpLevelZero(samplerName, float3((coord3).xy, index), (coord3).z)
#define SAMPLE_TEXTURECUBE_SHADOW(textureName, samplerName, coord4)                  textureName.SampleCmpLevelZero(samplerName, (coord4).xyz, (coord4).w)
#define SAMPLE_TEXTURECUBE_ARRAY_SHADOW(textureName, samplerName, coord4, index)     textureName.SampleCmpLevelZero(samplerName, float4((coord4).xyz, index), (coord4).w)

#define SAMPLE_DEPTH_TEXTURE(textureName, samplerName, coord2)          SAMPLE_TEXTURE2D(textureName, samplerName, coord2).r
#define SAMPLE_DEPTH_TEXTURE_LOD(textureName, samplerName, coord2, lod) SAMPLE_TEXTURE2D_LOD(textureName, samplerName, coord2, lod).r

#define LOAD_TEXTURE2D(textureName, unCoord2)                                   textureName.Load(int3(unCoord2, 0))
#define LOAD_TEXTURE2D_LOD(textureName, unCoord2, lod)                          textureName.Load(int3(unCoord2, lod))
#define LOAD_TEXTURE2D_MSAA(textureName, unCoord2, sampleIndex)                 textureName.Load(unCoord2, sampleIndex)
#define LOAD_TEXTURE2D_ARRAY(textureName, unCoord2, index)                      textureName.Load(int4(unCoord2, index, 0))
#define LOAD_TEXTURE2D_ARRAY_MSAA(textureName, unCoord2, index, sampleIndex)    textureName.Load(int3(unCoord2, index), sampleIndex)
#define LOAD_TEXTURE2D_ARRAY_LOD(textureName, unCoord2, index, lod)             textureName.Load(int4(unCoord2, index, lod))
#define LOAD_TEXTURE3D(textureName, unCoord3)                                   textureName.Load(int4(unCoord3, 0))
#define LOAD_TEXTURE3D_LOD(textureName, unCoord3, lod)                          textureName.Load(int4(unCoord3, lod))

#define PLATFORM_SUPPORT_GATHER
#define GATHER_TEXTURE2D(textureName, samplerName, coord2)                textureName.Gather(samplerName, coord2)
#define GATHER_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index)   textureName.Gather(samplerName, float3(coord2, index))
#define GATHER_TEXTURECUBE(textureName, samplerName, coord3)              textureName.Gather(samplerName, coord3)
#define GATHER_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index) textureName.Gather(samplerName, float4(coord3, index))
#define GATHER_RED_TEXTURE2D(textureName, samplerName, coord2)            textureName.GatherRed(samplerName, coord2)
#define GATHER_GREEN_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherGreen(samplerName, coord2)
#define GATHER_BLUE_TEXTURE2D(textureName, samplerName, coord2)           textureName.GatherBlue(samplerName, coord2)
#define GATHER_ALPHA_TEXTURE2D(textureName, samplerName, coord2)          textureName.GatherAlpha(samplerName, coord2)
```



### GLCore

```c
//类似D3D11，定义TEXTURE2D、SAMPLE_TEXTURE2D等
```



### GLES2

```c
//类似D3D11，定义TEXTURE2D、SAMPLE_TEXTURE2D等
```



### GLES3

```c
//类似D3D11，定义TEXTURE2D、SAMPLE_TEXTURE2D等
```



### Metal

```c
//类似D3D11，定义TEXTURE2D、SAMPLE_TEXTURE2D等
```



### Switch

```c
//类似D3D11，定义TEXTURE2D、SAMPLE_TEXTURE2D等
```



### Vulkan

```c
//类似D3D11，定义TEXTURE2D、SAMPLE_TEXTURE2D等
```



## Sampling
### Fibonacci

```c
real2 Fibonacci2dSeq(real fibN1, real fibN2, uint i)
#define GOLDEN_RATIO 1.618033988749895
#define GOLDEN_ANGLE 2.399963229728653
real2 Golden2dSeq(uint i, real n)
real2 Fibonacci2d(uint i, uint sampleCount)
real2 SampleDiskGolden(uint i, uint sampleCount)
real2 SampleDiskFibonacci(uint i, uint sampleCount)
real2 SampleHemisphereFibonacci(uint i, uint sampleCount)
real2 SampleSphereFibonacci(uint i, uint sampleCount)
```



### Hammersley

```c
uint ReverseBits32(uint bits)
real VanDerCorputBase2(uint i)
real2 Hammersley2dSeq(uint i, uint sequenceLength)
real2 Hammersley2d(uint i, uint sampleCount)
```



### SampleUVMapping

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Sampling/SampleUVMappingInternal.hlsl"
struct UVMapping
{
    int mappingType;
    float2 uv;  // Current uv or planar uv
    // Triplanar specific
    float2 uvZY;
    float2 uvXZ;
    float2 uvXY;
    float3 normalWS; // vertex normal
    float3 triplanarWeights;
	#ifdef SURFACE_GRADIENT
    // tangent basis to use when mappingType is UV_MAPPING_UVSET
    // these are vertex level in world space
    float3 tangentWS;
    float3 bitangentWS;
    // TODO: store also object normal map for object triplanar
	#endif
};
//不同的#include有细微区别，因为同名故在此略去
#define ADD_FUNC_SUFFIX(Name) MERGE_NAME(Name, Lod)
#define SAMPLE_TEXTURE_FUNC(textureName, samplerName, uvMapping, lod) SAMPLE_TEXTURE2D_LOD(textureName, samplerName, uvMapping, 

#define SAMPLE_UVMAPPING_TEXTURE2D(textureName, samplerName, uvMapping)             SampleUVMapping(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, 0.0) // Last 0.0 is unused
#define SAMPLE_UVMAPPING_TEXTURE2D_LOD(textureName, samplerName, uvMapping, lod)    SampleUVMappingLod(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, lod)
#define SAMPLE_UVMAPPING_TEXTURE2D_BIAS(textureName, samplerName, uvMapping, bias)  SampleUVMappingBias(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, bias)
#define SAMPLE_UVMAPPING_NORMALMAP(textureName, samplerName, uvMapping, scale)              SampleUVMappingNormal(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, 0.0)
#define SAMPLE_UVMAPPING_NORMALMAP_LOD(textureName, samplerName, uvMapping, scale, lod)     SampleUVMappingNormalLod(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, lod)
#define SAMPLE_UVMAPPING_NORMALMAP_BIAS(textureName, samplerName, uvMapping, scale, bias)   SampleUVMappingNormalBias(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, bias)
#define SAMPLE_UVMAPPING_NORMALMAP_AG(textureName, samplerName, uvMapping, scale)              SampleUVMappingNormalAG(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, 0.0)
#define SAMPLE_UVMAPPING_NORMALMAP_AG_LOD(textureName, samplerName, uvMapping, scale, lod)     SampleUVMappingNormalAGLod(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, lod)
#define SAMPLE_UVMAPPING_NORMALMAP_AG_BIAS(textureName, samplerName, uvMapping, scale, bias)   SampleUVMappingNormalAGBias(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, bias)
#define SAMPLE_UVMAPPING_NORMALMAP_RGB(textureName, samplerName, uvMapping, scale)              SampleUVMappingNormalRGB(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, 0.0)
#define SAMPLE_UVMAPPING_NORMALMAP_RGB_LOD(textureName, samplerName, uvMapping, scale, lod)     SampleUVMappingNormalRGBLod(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, lod)
#define SAMPLE_UVMAPPING_NORMALMAP_RGB_BIAS(textureName, samplerName, uvMapping, scale, bias)   SampleUVMappingNormalRGBBias(TEXTURE2D_ARGS(textureName, samplerName), uvMapping, scale, bias)
```



### SampleUVMappingInternal

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Sampling/SampleUVMappingNormalInternal.hlsl"
real4 ADD_FUNC_SUFFIX(SampleUVMapping)(TEXTURE2D_PARAM(textureName, samplerName), UVMapping uvMapping, real param)
#define ADD_NORMAL_FUNC_SUFFIX(Name) MERGE_NAME(Name, AG)
#define UNPACK_NORMAL_FUNC UnpackNormalAG
#define UNPACK_DERIVATIVE_FUNC UnpackDerivativeNormalAG
#undef ADD_NORMAL_FUNC_SUFFIX
#undef UNPACK_NORMAL_FUNC
#undef UNPACK_DERIVATIVE_FUNC
```



### SampleUVMappingNormalInternal

```c
real3 ADD_FUNC_SUFFIX(ADD_NORMAL_FUNC_SUFFIX(SampleUVMappingNormal))(TEXTURE2D_PARAM(textureName, samplerName), UVMapping uvMapping, real scale, real param)
```



### Sampling

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Sampling/Fibonacci.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Sampling/Hammersley.hlsl"
real3 SphericalToCartesian(real cosPhi, real sinPhi, real cosTheta)
real3 SphericalToCartesian(real phi, real cosTheta)
real3 TransformGLtoDX(real3 v)
real3 ConvertEquiarealToCubemap(real u, real v)
real2 CubemapTexelToNVC(uint2 unPositionTXS, uint cubemapSize)
real3 CubemapTexelToDirection(real2 positionNVC, uint faceId)
real2 SampleDiskUniform(real u1, real u2)
real2 SampleDiskCubic(real u1, real u2)
real3 SampleConeUniform(real u1, real u2, real cos_theta)
real3 SampleSphereUniform(real u1, real u2)
real3 SampleHemisphereCosine(real u1, real u2)
real3 SampleHemisphereCosine(real u1, real u2, real3 normal)
real3 SampleHemisphereUniform(real u1, real u2)
void SampleSphere(real2   u,
                  real4x4 localToWorld,
                  real    radius,
              out real    lightPdf,
              out real3   P,
              out real3   Ns)
void SampleHemisphere(real2   u,
                      real4x4 localToWorld,
                      real    radius,
                  out real    lightPdf,
                  out real3   P,
                  out real3   Ns)
void SampleCylinder(real2   u,
                    real4x4 localToWorld,
                    real    radius,
                    real    width,
                out real    lightPdf,
                out real3   P,
                out real3   Ns)
void SampleRectangle(real2   u,
                     real4x4 localToWorld,
                     real    width,
                     real    height,
                 out real    lightPdf,
                 out real3   P,
                 out real3   Ns)
void SampleDisk(real2   u,
                real4x4 localToWorld,
                real    radius,
            out real    lightPdf,
            out real3   P,
            out real3   Ns)
void SampleCone(real2 u, real cosHalfAngle,
                out real3 dir, out real rcpPdf)
real3 SampleConeStrata(uint sampleIdx, real rcpSampleCount, real cosHalfApexAngle)
```



## Shadow

### ShadowSamplingTent

```c
real SampleShadow_GetTriangleTexelArea(real triangleHeight)
void SampleShadow_GetTexelAreas_Tent_3x3(real offset, out real4 computedArea, out real4 computedAreaUncut)
void SampleShadow_GetTexelWeights_Tent_3x3(real offset, out real4 computedWeight)
void SampleShadow_GetTexelWeights_Tent_5x5(real offset, out real3 texelsWeightsA, out real3 texelsWeightsB)
void SampleShadow_GetTexelWeights_Tent_7x7(real offset, out real4 texelsWeightsA, out real4 texelsWeightsB)
void SampleShadow_ComputeSamples_Tent_3x3(real4 shadowMapTexture_TexelSize, real2 coord, out real fetchesWeights[4], out real2 fetchesUV[4])
void SampleShadow_ComputeSamples_Tent_5x5(real4 shadowMapTexture_TexelSize, real2 coord, out real fetchesWeights[9], out real2 fetchesUV[9])
void SampleShadow_ComputeSamples_Tent_7x7(real4 shadowMapTexture_TexelSize, real2 coord, out real fetchesWeights[16], out real2 fetchesUV[16])
```



# Universal  ShaderLibrary

## 根目录
### AmbientOcclusion

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
TEXTURE2D_X(_ScreenSpaceOcclusionTexture);
SAMPLER(sampler_ScreenSpaceOcclusionTexture);
struct AmbientOcclusionFactor
{
    half indirectAmbientOcclusion;
    half directAmbientOcclusion;
};
half SampleAmbientOcclusion(float2 normalizedScreenSpaceUV)
AmbientOcclusionFactor GetScreenSpaceAmbientOcclusion(float2 normalizedScreenSpaceUV)
AmbientOcclusionFactor CreateAmbientOcclusionFactor(float2 normalizedScreenSpaceUV, half occlusion)
AmbientOcclusionFactor CreateAmbientOcclusionFactor(InputData inputData, SurfaceData surfaceData)
```



### BRDF

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/BSDF.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Deprecated.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
#define kDielectricSpec half4(0.04, 0.04, 0.04, 1.0 - 0.04) // standard dielectric reflectivity coef at incident angle (= 4%)

struct BRDFData
{
    half3 albedo;
    half3 diffuse;
    half3 specular;
    half reflectivity;
    half perceptualRoughness;
    half roughness;
    half roughness2;
    half grazingTerm;

    // We save some light invariant BRDF terms so we don't have to recompute
    // them in the light loop. Take a look at DirectBRDF function for detailed explaination.
    half normalizationTerm;     // roughness * 4.0 + 2.0
    half roughness2MinusOne;    // roughness^2 - 1.0
};
half ReflectivitySpecular(half3 specular)
half OneMinusReflectivityMetallic(half metallic)
half MetallicFromReflectivity(half reflectivity)
inline void InitializeBRDFDataDirect(half3 albedo, half3 diffuse, half3 specular, half reflectivity, half oneMinusReflectivity, half smoothness, inout half alpha, out BRDFData outBRDFData)
inline void InitializeBRDFDataDirect(half3 diffuse, half3 specular, half reflectivity, half oneMinusReflectivity, half smoothness, inout half alpha, out BRDFData outBRDFData)
inline void InitializeBRDFData(half3 albedo, half metallic, half3 specular, half smoothness, inout half alpha, out BRDFData outBRDFData)
inline void InitializeBRDFData(inout SurfaceData surfaceData, out BRDFData brdfData)
half3 ConvertF0ForClearCoat15(half3 f0)
inline void InitializeBRDFDataClearCoat(half clearCoatMask, half clearCoatSmoothness, inout BRDFData baseBRDFData, out BRDFData outBRDFData)
BRDFData CreateClearCoatBRDFData(SurfaceData surfaceData, inout BRDFData brdfData)
half3 EnvironmentBRDFSpecular(BRDFData brdfData, half fresnelTerm)
half3 EnvironmentBRDF(BRDFData brdfData, half3 indirectDiffuse, half3 indirectSpecular, half fresnelTerm)
half3 EnvironmentBRDFClearCoat(BRDFData brdfData, half clearCoatMask, half3 indirectSpecular, half fresnelTerm)
half DirectBRDFSpecular(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
half3 DirectBDRF(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS, bool specularHighlightsOff)
half3 DirectBRDF(BRDFData brdfData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS)
```



### Clustering

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
uint ClusteringSelect4(uint4 v, uint i)
struct ClusteredLightLoop
{
    uint baseIndex;
    uint tileMask;
    uint wordIndex;
    uint bitIndex;
    uint zBinMinMask;
    uint zBinMaxMask;
#if LIGHTS_PER_TILE > 32
    uint wordMin;
    uint wordMax;
#endif
};
ClusteredLightLoop ClusteredLightLoopInit(float2 normalizedScreenSpaceUV, float3 positionWS)
bool ClusteredLightLoopNextWord(inout ClusteredLightLoop state)
bool ClusteredLightLoopNextLight(inout ClusteredLightLoop state)
uint ClusteredLightLoopGetLightIndex(ClusteredLightLoop state)
```



### Core

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Deprecated.hlsl"
// Structs
struct VertexPositionInputs
{
    float3 positionWS; // World space position
    float3 positionVS; // View space position
    float4 positionCS; // Homogeneous clip space position
    float4 positionNDC;// Homogeneous normalized device coordinates
};
struct VertexNormalInputs
{
    real3 tangentWS;
    real3 bitangentWS;
    float3 normalWS;
};
//--------分情况做不同定义，略去细节---------
#define UNITY_Z_0_FAR_FROM_CLIPSPACE(coord)
#define SLICE_ARRAY_INDEX
#define TEXTURE2D_X(textureName)
#define TEXTURE2D_X_PARAM(textureName, samplerName)
#define TEXTURE2D_X_PARAM(textureName, samplerName)
#define TEXTURE2D_X_ARGS(textureName, samplerName)
#define TEXTURE2D_X_HALF(textureName)
#define TEXTURE2D_X_FLOAT(textureName)
#define LOAD_TEXTURE2D_X(textureName, unCoord2)
#define LOAD_TEXTURE2D_X_LOD(textureName, unCoord2, lod)
#define SAMPLE_TEXTURE2D_X(textureName, samplerName, coord2)
#define SAMPLE_TEXTURE2D_X_LOD(textureName, samplerName, coord2, lod)
#define GATHER_TEXTURE2D_X(textureName, samplerName, coord2)
#define GATHER_RED_TEXTURE2D_X(textureName, samplerName, coord2)
#define GATHER_GREEN_TEXTURE2D_X(textureName, samplerName, coord2)
#define GATHER_BLUE_TEXTURE2D_X(textureName, samplerName, coord2)
//----------其他各种定义--------------------------------------
#define SAMPLE_TEXTURE2D(textureName, samplerName, coord2)\
	PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2,  _GlobalMipBias.x)
#define SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2, bias) \
	PLATFORM_SAMPLE_TEXTURE2D_BIAS(textureName, samplerName, coord2,  (bias + _GlobalMipBias.x))
 #define SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, dpdx, dpdy) \
	PLATFORM_SAMPLE_TEXTURE2D_GRAD(textureName, samplerName, coord2, (dpdx * _GlobalMipBias.y), (dpdy * _GlobalMipBias.y))
#define SAMPLE_TEXTURE2D_ARRAY(textureName, samplerName, coord2, index) \
    PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, _GlobalMipBias.x)
#define SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, bias) \
    PLATFORM_SAMPLE_TEXTURE2D_ARRAY_BIAS(textureName, samplerName, coord2, index, (bias + _GlobalMipBias.x))
#define SAMPLE_TEXTURECUBE(textureName, samplerName, coord3) \
    PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, _GlobalMipBias.x)
#define SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, bias) \
    PLATFORM_SAMPLE_TEXTURECUBE_BIAS(textureName, samplerName, coord3, (bias + _GlobalMipBias.x))
#define SAMPLE_TEXTURECUBE_ARRAY(textureName, samplerName, coord3, index)\
    PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, _GlobalMipBias.x)
#define SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, bias)\
    PLATFORM_SAMPLE_TEXTURECUBE_ARRAY_BIAS(textureName, samplerName, coord3, index, (bias + _GlobalMipBias.x))
#define VT_GLOBAL_MIP_BIAS_MULTIPLIER (_GlobalMipBias.y)
```



### DBuffer

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
//------分情况做不同定义，略去细节--------
#define OUTPUT_DBUFFER(NAME)
#define DECLARE_DBUFFER_TEXTURE(NAME)
#define FETCH_DBUFFER(NAME, TEX, unCoord2)
#define ENCODE_INTO_DBUFFER(DECAL_SURFACE_DATA, NAME)
#define DECODE_FROM_DBUFFER(NAME, DECAL_SURFACE_DATA)
//-----------------------------
void EncodeIntoDBuffer(DecalSurfaceData surfaceData
    , out DBufferType0 outDBuffer0
#if defined(_DBUFFER_MRT2) || defined(_DBUFFER_MRT3)
    , out DBufferType1 outDBuffer1
#endif
#if defined(_DBUFFER_MRT3)
    , out DBufferType2 outDBuffer2
#endif
)
void DecodeFromDBuffer(
    DBufferType0 inDBuffer0
#if defined(_DBUFFER_MRT2) || defined(_DBUFFER_MRT3)
    , DBufferType1 inDBuffer1
#endif
#if defined(_DBUFFER_MRT3) || defined(DECALS_4RT)
    , DBufferType2 inDBuffer2
#endif
    , out DecalSurfaceData surfaceData
)
DECLARE_DBUFFER_TEXTURE(_DBufferTexture);
void ApplyDecal(float4 positionCS,
    inout half3 baseColor,
    inout half3 specularColor,
    inout half3 normalWS,
    inout half metallic,
    inout half occlusion,
    inout half smoothness)
void ApplyDecalToBaseColor(float4 positionCS, inout half3 baseColor)
void ApplyDecalToBaseColorAndNormal(float4 positionCS, inout half3 baseColor, inout half3 normalWS)
void ApplyDecalToSurfaceData(float4 positionCS, inout SurfaceData surfaceData, inout InputData inputData)
```



### DecalInput

```c
struct DecalSurfaceData
{
    half4 baseColor;
    half4 normalWS;
    half3 emissive;
    half metallic;
    half occlusion;
    half smoothness;
    half MAOSAlpha;
};
```



### DeclareDepthTexture

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

TEXTURE2D_X_FLOAT(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);

float SampleSceneDepth(float2 uv)
{
    return SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(uv)).r;
}
float LoadSceneDepth(uint2 uv)
{
    return LOAD_TEXTURE2D_X(_CameraDepthTexture, uv).r;
}
```



### DeclareNormalsTexture

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

TEXTURE2D_X_FLOAT(_CameraNormalsTexture);
SAMPLER(sampler_CameraNormalsTexture);
float3 SampleSceneNormals(float2 uv)
{
    float3 normal = SAMPLE_TEXTURE2D_X(_CameraNormalsTexture, sampler_CameraNormalsTexture, UnityStereoTransformScreenSpaceTex(uv)).xyz;

    #if defined(_GBUFFER_NORMALS_OCT)
    float2 remappedOctNormalWS = Unpack888ToFloat2(normal); // values between [ 0,  1]
    float2 octNormalWS = remappedOctNormalWS.xy * 2.0 - 1.0;    // values between [-1, +1]
    normal = UnpackNormalOctQuadEncode(octNormalWS);
    #endif

    return normal;
}
float3 LoadSceneNormals(uint2 uv)
{
    float3 normal = LOAD_TEXTURE2D_X(_CameraNormalsTexture, uv).xyz;

    #if defined(_GBUFFER_NORMALS_OCT)
    float2 remappedOctNormalWS = Unpack888ToFloat2(normal); // values between [ 0,  1]
    float2 octNormalWS = remappedOctNormalWS.xy * 2.0 - 1.0;    // values between [-1, +1]
    normal = UnpackNormalOctQuadEncode(octNormalWS);
    #endif

    return normal;
}
```



### DeclareOpaqueTexture

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

TEXTURE2D_X(_CameraOpaqueTexture);
SAMPLER(sampler_CameraOpaqueTexture);

float3 SampleSceneColor(float2 uv)
{
    return SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, UnityStereoTransformScreenSpaceTex(uv)).rgb;
}
float3 LoadSceneColor(uint2 uv)
{
    return LOAD_TEXTURE2D_X(_CameraOpaqueTexture, uv).rgb;
}
```



### Deprecated

```c
// Stereo-related bits
#define SCREENSPACE_TEXTURE         TEXTURE2D_X
#define SCREENSPACE_TEXTURE_FLOAT   TEXTURE2D_X_FLOAT
#define SCREENSPACE_TEXTURE_HALF    TEXTURE2D_X_HALF

// Typo-fixes, re-route to new name for backwards compatiblity (if there are external dependencies).
#define kDieletricSpec kDielectricSpec
#define DirectBDRF     DirectBRDF

// Deprecated: not using consistent naming convention
#if defined(USING_STEREO_MATRICES)
#define unity_StereoMatrixIP unity_StereoMatrixInvP
#define unity_StereoMatrixIVP unity_StereoMatrixInvVP
#endif

// Previously used when rendering with DrawObjectsPass.
// Global object render pass data containing various settings.
// x,y,z are currently unused
// w is used for knowing whether the object is opaque(1) or alpha blended(0)
half4 _DrawObjectPassData;

#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
// _AdditionalShadowsIndices was deprecated - To get the first shadow slice index for a light, use GetAdditionalLightShadowParams(lightIndex).w [see Shadows.hlsl]
#define _AdditionalShadowsIndices _AdditionalShadowParams_SSBO
// _AdditionalShadowsBuffer was deprecated - To access a shadow slice's matrix, use _AdditionalLightsWorldToShadow_SSBO[shadowSliceIndex] - To access other shadow parameters, use GetAdditionalLightShadowParams(int lightIndex) [see Shadows.hlsl]
#define _AdditionalShadowsBuffer _AdditionalLightsWorldToShadow_SSBO
#endif

// Deprecated: even when USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA is defined we do not this structure anymore, because worldToShadowMatrix and shadowParams must be stored in arrays of different sizes
// To get the first shadow slice index for a light, use GetAdditionalLightShadowParams(lightIndex).w [see Shadows.hlsl]
// To access other shadow parameters, use GetAdditionalLightShadowParams(int lightIndex)[see Shadows.hlsl]
struct ShadowData
{
    float4x4 worldToShadowMatrix; // per-shadow-slice
    float4 shadowParams;          // per-casting-light
};
```



### GlobalIllumination

GI就是GlobalIllumination的缩写，意为全局光照

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

half3 SampleSH(half3 normalWS)
half3 SampleSHVertex(half3 normalWS)
half3 SampleSHPixel(half3 L2Term, half3 normalWS)
half3 SampleLightmap(float2 staticLightmapUV, float2 dynamicLightmapUV, half3 normalWS)
half3 SampleLightmap(float2 staticLightmapUV, half3 normalWS)
half3 BoxProjectedCubemapDirection(half3 reflectionWS, float3 positionWS, float4 cubemapPositionWS, float4 boxMin, float4 boxMax)
float CalculateProbeWeight(float3 positionWS, float4 probeBoxMin, float4 probeBoxMax)
half CalculateProbeVolumeSqrMagnitude(float4 probeBoxMin, float4 probeBoxMax)
half3 CalculateIrradianceFromReflectionProbes(half3 reflectVector, float3 positionWS, half perceptualRoughness)
half3 GlossyEnvironmentReflection(half3 reflectVector, float3 positionWS, half perceptualRoughness, half occlusion)
half3 GlossyEnvironmentReflection(half3 reflectVector, half perceptualRoughness, half occlusion)
half3 SubtractDirectMainLightFromLightmap(Light mainLight, half3 normalWS, half3 bakedGI)
half3 GlobalIllumination(BRDFData brdfData, BRDFData brdfDataClearCoat, float clearCoatMask,
    half3 bakedGI, half occlusion, float3 positionWS,
    half3 normalWS, half3 viewDirectionWS)
    half3 GlobalIllumination(BRDFData brdfData, half3 bakedGI, half occlusion, float3 positionWS, half3 normalWS, half3 viewDirectionWS)
half3 GlobalIllumination(BRDFData brdfData, BRDFData brdfDataClearCoat, float clearCoatMask,
    half3 bakedGI, half occlusion,
    half3 normalWS, half3 viewDirectionWS)
half3 GlobalIllumination(BRDFData brdfData, half3 bakedGI, half occlusion, half3 normalWS, half3 viewDirectionWS)
void MixRealtimeAndBakedGI(inout Light light, half3 normalWS, inout half3 bakedGI)
void MixRealtimeAndBakedGI(inout Light light, half3 normalWS, inout half3 bakedGI, half4 shadowMask)
void MixRealtimeAndBakedGI(inout Light light, half3 normalWS, inout half3 bakedGI, AmbientOcclusionFactor aoFactor)

//----------定义-----------
#if defined(UNITY_DOTS_INSTANCING_ENABLED)
#define LIGHTMAP_NAME unity_Lightmaps
#define LIGHTMAP_INDIRECTION_NAME unity_LightmapsInd
#define LIGHTMAP_SAMPLER_NAME samplerunity_Lightmaps
#define LIGHTMAP_SAMPLE_EXTRA_ARGS staticLightmapUV, unity_LightmapIndex.x
#else
#define LIGHTMAP_NAME unity_Lightmap
#define LIGHTMAP_INDIRECTION_NAME unity_LightmapInd
#define LIGHTMAP_SAMPLER_NAME samplerunity_Lightmap
#define LIGHTMAP_SAMPLE_EXTRA_ARGS staticLightmapUV
#endif
#if defined(LIGHTMAP_ON) && defined(DYNAMICLIGHTMAP_ON)
#define SAMPLE_GI(staticLmName, dynamicLmName, shName, normalWSName) SampleLightmap(staticLmName, dynamicLmName, normalWSName)
#elif defined(DYNAMICLIGHTMAP_ON)
#define SAMPLE_GI(staticLmName, dynamicLmName, shName, normalWSName) SampleLightmap(0, dynamicLmName, normalWSName)
#elif defined(LIGHTMAP_ON)
#define SAMPLE_GI(staticLmName, shName, normalWSName) SampleLightmap(staticLmName, 0, normalWSName)
#else
#define SAMPLE_GI(staticLmName, shName, normalWSName) SampleSHPixel(shName, normalWSName)
#endif
```



### Input

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderTypes.cs.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Deprecated.hlsl"
// Note: #include order is important here.
// UnityInput.hlsl must be included before UnityInstancing.hlsl, so constant buffer
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
// UniversalDOTSInstancing.hlsl must be included after UnityInstancing.hlsl
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UniversalDOTSInstancing.hlsl"
#include "Packages/com.unity.visualeffectgraph/Shaders/VFXMatricesOverride.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
struct InputData
{
    float3  positionWS;
    float4  positionCS;
    float3   normalWS;
    half3   viewDirectionWS;
    float4  shadowCoord;
    half    fogCoord;
    half3   vertexLighting;
    half3   bakedGI;
    float2  normalizedScreenSpaceUV;
    half4   shadowMask;
    half3x3 tangentToWorld;

    #if defined(DEBUG_DISPLAY)
    half2   dynamicLightmapUV;
    half2   staticLightmapUV;
    float3  vertexSH;

    half3 brdfDiffuse;
    half3 brdfSpecular;
    float2 uv;
    uint mipCount;

    // texelSize :
    // x = 1 / width
    // y = 1 / height
    // z = width
    // w = height
    float4 texelSize;

    // mipInfo :
    // x = quality settings minStreamingMipLevel
    // y = original mip count for texture
    // z = desired on screen mip level
    // w = loaded mip level
    float4 mipInfo;
    #endif
};

#define MAX_VISIBLE_LIGHTS_UBO  32
#define MAX_VISIBLE_LIGHTS_SSBO 256

// Keep in sync with RenderingUtils.useStructuredBuffer
#define USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA 0

#define RENDERING_LIGHT_LAYERS_MASK (255)
#define RENDERING_LIGHT_LAYERS_MASK_SHIFT (0)
#define DEFAULT_LIGHT_LAYERS (RENDERING_LIGHT_LAYERS_MASK >> RENDERING_LIGHT_LAYERS_MASK_SHIFT)
// Must match: UniversalRenderPipeline.maxVisibleAdditionalLights
#if defined(SHADER_API_MOBILE) && (defined(SHADER_API_GLES) || defined(SHADER_API_GLES30))
    #define MAX_VISIBLE_LIGHTS 16
#elif defined(SHADER_API_MOBILE) || (defined(SHADER_API_GLCORE) && !defined(SHADER_API_SWITCH)) || defined(SHADER_API_GLES) || defined(SHADER_API_GLES3) // Workaround because SHADER_API_GLCORE is also defined when SHADER_API_SWITCH is
    #define MAX_VISIBLE_LIGHTS 32
#else
    #define MAX_VISIBLE_LIGHTS 256
#endif

// Match with values in UniversalRenderPipeline.cs
#define MAX_ZBIN_VEC4S 1024
#define MAX_TILE_VEC4S 4096
#if MAX_VISIBLE_LIGHTS < 32
    #define LIGHTS_PER_TILE 32
#else
    #define LIGHTS_PER_TILE MAX_VISIBLE_LIGHTS
#endif

half4 _GlossyEnvironmentColor;
half4 _SubtractiveShadowColor;

half4 _GlossyEnvironmentCubeMap_HDR;
TEXTURECUBE(_GlossyEnvironmentCubeMap);
SAMPLER(sampler_GlossyEnvironmentCubeMap);

#define _InvCameraViewProj unity_MatrixInvVP
float4 _ScaledScreenParams;

float4 _MainLightPosition;
half4 _MainLightColor;
half4 _MainLightOcclusionProbes;
uint _MainLightLayerMask;

// xyz are currently unused
// w: directLightStrength
half4 _AmbientOcclusionParam;

half4 _AdditionalLightsCount;

#if USE_CLUSTERED_LIGHTING
// Directional lights would be in all clusters, so they don't go into the cluster structure.
// Instead, they are stored first in the light buffer.
uint _AdditionalLightsDirectionalCount;
// The number of Z-bins to skip based on near plane distance.
uint _AdditionalLightsZBinOffset;
// Scale from view-space Z to Z-bin.
float _AdditionalLightsZBinScale;
// Scale from screen-space UV [0, 1] to tile coordinates [0, tile resolution].
float2 _AdditionalLightsTileScale;
uint _AdditionalLightsTileCountX;
#endif

#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
StructuredBuffer<LightData> _AdditionalLightsBuffer;
StructuredBuffer<int> _AdditionalLightsIndices;
#else
// GLES3 causes a performance regression in some devices when using CBUFFER.
#ifndef SHADER_API_GLES3
CBUFFER_START(AdditionalLights)
#endif
float4 _AdditionalLightsPosition[MAX_VISIBLE_LIGHTS];
half4 _AdditionalLightsColor[MAX_VISIBLE_LIGHTS];
half4 _AdditionalLightsAttenuation[MAX_VISIBLE_LIGHTS];
half4 _AdditionalLightsSpotDir[MAX_VISIBLE_LIGHTS];
half4 _AdditionalLightsOcclusionProbes[MAX_VISIBLE_LIGHTS];
float _AdditionalLightsLayerMasks[MAX_VISIBLE_LIGHTS]; // we want uint[] but Unity api does not support it.
#ifndef SHADER_API_GLES3
CBUFFER_END
#endif
#endif

#if USE_CLUSTERED_LIGHTING
    CBUFFER_START(AdditionalLightsZBins)
        float4 _AdditionalLightsZBins[MAX_ZBIN_VEC4S];
    CBUFFER_END
    CBUFFER_START(AdditionalLightsTiles)
        float4 _AdditionalLightsTiles[MAX_TILE_VEC4S];
    CBUFFER_END
#endif

#define UNITY_MATRIX_M     unity_ObjectToWorld
#define UNITY_MATRIX_I_M   unity_WorldToObject
#define UNITY_MATRIX_V     unity_MatrixV
#define UNITY_MATRIX_I_V   unity_MatrixInvV
#define UNITY_MATRIX_P     OptimizeProjectionMatrix(glstate_matrix_projection)
#define UNITY_MATRIX_I_P   unity_MatrixInvP
#define UNITY_MATRIX_VP    unity_MatrixVP
#define UNITY_MATRIX_I_VP  unity_MatrixInvVP
#define UNITY_MATRIX_MV    mul(UNITY_MATRIX_V, UNITY_MATRIX_M)
#define UNITY_MATRIX_T_MV  transpose(UNITY_MATRIX_MV)
#define UNITY_MATRIX_IT_MV transpose(mul(UNITY_MATRIX_I_M, UNITY_MATRIX_I_V))
#define UNITY_MATRIX_MVP   mul(UNITY_MATRIX_VP, UNITY_MATRIX_M)
#define UNITY_PREV_MATRIX_M   unity_MatrixPreviousM
#define UNITY_PREV_MATRIX_I_M unity_MatrixPreviousMI
```



### Lighting

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/BRDF.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/GlobalIllumination.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/AmbientOcclusion.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"

half3 LightingLambert(half3 lightColor, half3 lightDir, half3 normal)
half3 LightingSpecular(half3 lightColor, half3 lightDir, half3 normal, half3 viewDir, half4 specular, half smoothness)
half3 LightingPhysicallyBased(BRDFData brdfData, BRDFData brdfDataClearCoat,
    half3 lightColor, half3 lightDirectionWS, half lightAttenuation,
    half3 normalWS, half3 viewDirectionWS,
    half clearCoatMask, bool specularHighlightsOff)
half3 LightingPhysicallyBased(BRDFData brdfData, BRDFData brdfDataClearCoat, Light light, half3 normalWS, half3 viewDirectionWS, half clearCoatMask, bool specularHighlightsOff)
half3 LightingPhysicallyBased(BRDFData brdfData, Light light, half3 normalWS, half3 viewDirectionWS)
half3 LightingPhysicallyBased(BRDFData brdfData, half3 lightColor, half3 lightDirectionWS, half lightAttenuation, half3 normalWS, half3 viewDirectionWS)
half3 LightingPhysicallyBased(BRDFData brdfData, Light light, half3 normalWS, half3 viewDirectionWS, bool specularHighlightsOff)
half3 LightingPhysicallyBased(BRDFData brdfData, half3 lightColor, half3 lightDirectionWS, half lightAttenuation, half3 normalWS, half3 viewDirectionWS, bool specularHighlightsOff)
half3 VertexLighting(float3 positionWS, half3 normalWS)
struct LightingData
{
    half3 giColor;
    half3 mainLightColor;
    half3 additionalLightsColor;
    half3 vertexLightingColor;
    half3 emissionColor;
};
half3 CalculateLightingColor(LightingData lightingData, half3 albedo)
half4 CalculateFinalColor(LightingData lightingData, half alpha)
half4 CalculateFinalColor(LightingData lightingData, half3 albedo, half alpha, float fogCoord)
LightingData CreateLightingData(InputData inputData, SurfaceData surfaceData)
half3 CalculateBlinnPhong(Light light, InputData inputData, SurfaceData surfaceData)
half4 UniversalFragmentPBR(InputData inputData, SurfaceData surfaceData)
half4 UniversalFragmentPBR(InputData inputData, half3 albedo, half metallic, half3 specular,
    half smoothness, half occlusion, half3 emission, half alpha)
half4 UniversalFragmentBlinnPhong(InputData inputData, SurfaceData surfaceData)
half4 UniversalFragmentBlinnPhong(InputData inputData, half3 diffuse, half4 specularGloss, half smoothness, half3 emission, half alpha, half3 normalTS)
half4 UniversalFragmentBakedLit(InputData inputData, SurfaceData surfaceData)
half4 UniversalFragmentBakedLit(InputData inputData, half3 color, half alpha, half3 normalTS)
//-------定义--------------
#if defined(LIGHTMAP_ON)
    #define DECLARE_LIGHTMAP_OR_SH(lmName, shName, index) float2 lmName : TEXCOORD##index
    #define OUTPUT_LIGHTMAP_UV(lightmapUV, lightmapScaleOffset, OUT) OUT.xy = lightmapUV.xy * lightmapScaleOffset.xy + lightmapScaleOffset.zw;
    #define OUTPUT_SH(normalWS, OUT)
#else
    #define DECLARE_LIGHTMAP_OR_SH(lmName, shName, index) half3 shName : TEXCOORD##index
    #define OUTPUT_LIGHTMAP_UV(lightmapUV, lightmapScaleOffset, OUT)
    #define OUTPUT_SH(normalWS, OUT) OUT.xyz = SampleSHVertex(normalWS)
#endif
```



### MetaInput

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/MetaPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

#define MetaInput UnityMetaInput
#define MetaFragment UnityMetaFragment
float4 MetaVertexPosition(float4 positionOS, float2 uv1, float2 uv2, float4 uv1ST, float4 uv2ST)
{
    return UnityMetaVertexPosition(positionOS.xyz, uv1, uv2, uv1ST, uv2ST);
}
```



### NormalReconstruction

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
float4x4 _NormalReconstructionMatrix[2];
float GetRawDepth(float2 uv)
// inspired by keijiro's depth inverse projection
// https://github.com/keijiro/DepthInverseProjection
// constructs view space ray at the far clip plane from the screen uv
// then multiplies that ray by the linear 01 depth
float3 ViewSpacePosAtScreenUV(float2 uv)
float3 ViewSpacePosAtPixelPosition(float2 positionSS)
half3 ReconstructNormalDerivative(float2 positionSS)
// Taken from https://gist.github.com/bgolus/a07ed65602c009d5e2f753826e8078a0
// unity's compiled fragment shader stats: 33 math, 3 tex
half3 ReconstructNormalTap3(float2 positionSS)
// Taken from https://gist.github.com/bgolus/a07ed65602c009d5e2f753826e8078a0
// unity's compiled fragment shader stats: 50 math, 4 tex
half3 ReconstructNormalTap4(float2 positionSS)
// Taken from https://gist.github.com/bgolus/a07ed65602c009d5e2f753826e8078a0
// unity's compiled fragment shader stats: 54 math, 5 tex
half3 ReconstructNormalTap5(float2 positionSS)
// Taken from https://gist.github.com/bgolus/a07ed65602c009d5e2f753826e8078a0
// unity's compiled fragment shader stats: 66 math, 9 tex
half3 ReconstructNormalTap9(float2 positionSS)
```



### Particles

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ParticlesInstancing.hlsl"
struct ParticleParams
{
    float4 positionWS;
    float4 vertexColor;
    float4 projectedPosition;
    half4 baseColor;
    float3 blendUv;
    float2 uv;
};
void InitParticleParams(VaryingsParticle input, out ParticleParams output)
half4 MixParticleColor(half4 baseColor, half4 particleColor, half4 colorAddSubDiff)
float SoftParticles(float near, float far, float4 projection)
half CameraFade(float near, float far, float4 projection)
half3 AlphaModulate(half3 albedo, half alpha)
half3 Distortion(float4 baseColor, float3 normal, half strength, half blend, float4 projection)
half4 BlendTexture(TEXTURE2D_PARAM(_Texture, sampler_Texture), float2 uv, float3 blendUv)
half3 SampleNormalTS(float2 uv, float3 blendUv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
half4 GetParticleColor(half4 color)
void GetParticleTexcoords(out float2 outputTexcoord, out float3 outputTexcoord2AndBlend, in float4 inputTexcoords, in float inputBlend)
void GetParticleTexcoords(out float2 outputTexcoord, in float2 inputTexcoord)
    
//-------定义--------------
#if defined(_ALPHAPREMULTIPLY_ON)
    #define ALBEDO_MUL albedo
#else
    #define ALBEDO_MUL albedo.a
#endif
#if defined(_ALPHAPREMULTIPLY_ON)
    #define SOFT_PARTICLE_MUL_ALBEDO(albedo, val) albedo * val
#elif defined(_ALPHAMODULATE_ON)
    #define SOFT_PARTICLE_MUL_ALBEDO(albedo, val) half4(lerp(half3(1.0, 1.0, 1.0), albedo.rgb, albedo.a * val), albedo.a * val)
#else
    #define SOFT_PARTICLE_MUL_ALBEDO(albedo, val) albedo * half4(1.0, 1.0, 1.0, val)
#endif
```



### ParticlesInstancing

```c
struct DefaultParticleInstanceData
{
    float3x4 transform;
    uint color;
    float animFrame;
};
StructuredBuffer<UNITY_PARTICLE_INSTANCE_DATA> unity_ParticleInstanceData;
float4 unity_ParticleUVShiftData;
half unity_ParticleUseMeshColors;

void ParticleInstancingMatrices(out float4x4 objectToWorld, out float4x4 worldToObject)
void ParticleInstancingSetup()
```



### RealtimeLights

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/AmbientOcclusion.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LightCookie/LightCookie.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Clustering.hlsl"
uint GetMeshRenderingLightLayer()
struct Light
{
    half3   direction;
    half3   color;
    float   distanceAttenuation; // full-float precision required on some platforms
    half    shadowAttenuation;
    uint    layerMask;
};
float DistanceAttenuation(float distanceSqr, half2 distanceAttenuation)
half AngleAttenuation(half3 spotDirection, half3 lightDirection, half2 spotAttenuation)
Light GetMainLight()
Light GetMainLight(float4 shadowCoord)
Light GetMainLight(float4 shadowCoord, float3 positionWS, half4 shadowMask)
Light GetMainLight(InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor)
Light GetAdditionalPerObjectLight(int perObjectLightIndex, float3 positionWS)
uint GetPerObjectLightIndexOffset()
int GetPerObjectLightIndex(uint index)
Light GetAdditionalLight(uint i, float3 positionWS)
Light GetAdditionalLight(uint i, float3 positionWS, half4 shadowMask)
Light GetAdditionalLight(uint i, InputData inputData, half4 shadowMask, AmbientOcclusionFactor aoFactor)
int GetAdditionalLightsCount()
half4 CalculateShadowMask(InputData inputData)
        
//-------定义--------------
// WebGL1 does not support the variable conditioned for loops used for additional lights
#if !defined(_USE_WEBGL1_LIGHTS) && defined(UNITY_PLATFORM_WEBGL) && !defined(SHADER_API_GLES3)
    #define _USE_WEBGL1_LIGHTS 1
    #define _WEBGL1_MAX_LIGHTS 8
#else
    #define _USE_WEBGL1_LIGHTS 0
#endif
//------多可能定义，细节省略----------
#define LIGHT_LOOP_BEGIN(lightCount)
#define LIGHT_LOOP_END
```



### ShaderGraphFunctions

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
float shadergraph_LWSampleSceneDepth(float2 uv)
float3 shadergraph_LWSampleSceneColor(float2 uv)
float3 shadergraph_LWBakedGI(float3 positionWS, float3 normalWS, float2 uvStaticLightmap, float2 uvDynamicLightmap, bool applyScaling)
float3 shadergraph_LWReflectionProbe(float3 viewDir, float3 normalOS, float lod)
void shadergraph_LWFog(float3 positionOS, out float4 color, out float density)
float3x3 BuildTangentToWorld(float4 tangentWS, float3 normalWS)
//一些定义
#define SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv) shadergraph_LWSampleSceneDepth(uv)
#define SHADERGRAPH_SAMPLE_SCENE_COLOR(uv) shadergraph_LWSampleSceneColor(uv)
#define SHADERGRAPH_BAKED_GI(positionWS, normalWS, uvStaticLightmap, uvDynamicLightmap, applyScaling) shadergraph_LWBakedGI(positionWS, normalWS, uvStaticLightmap, uvDynamicLightmap, applyScaling)
#define SHADERGRAPH_REFLECTION_PROBE(viewDir, normalOS, lod) shadergraph_LWReflectionProbe(viewDir, normalOS, lod)
#define SHADERGRAPH_FOG(position, color, density) shadergraph_LWFog(position, color, density)
#define SHADERGRAPH_AMBIENT_SKY unity_AmbientSky
#define SHADERGRAPH_AMBIENT_EQUATOR unity_AmbientEquator
#define SHADERGRAPH_AMBIENT_GROUND unity_AmbientGround
```



### ShaderTypes.cs

```c
// Generated from UnityEngine.Rendering.Universal.ShaderInput+LightData
// PackingRules = Exact
struct LightData
{
    float4 position;
    float4 color;
    float4 attenuation;
    float4 spotDirection;
    float4 occlusionProbeChannels;
    uint layerMask;
};
```



### ShaderVariablesFunctions.deprecated

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

// Deprecated: A confusingly named and duplicate function that scales clipspace to unity NDC range. (-w < x(-y) < w --> 0 < xy < w)
// Use GetVertexPositionInputs().positionNDC instead for vertex shader
// Or a similar function in Common.hlsl, ComputeNormalizedDeviceCoordinatesWithZ()
float4 ComputeScreenPos(float4 positionCS)
{
    float4 o = positionCS * 0.5f;
    o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
    o.zw = positionCS.zw;
    return o;
}
```



### ShaderVariablesFunctions

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.deprecated.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl"

VertexPositionInputs GetVertexPositionInputs(float3 positionOS)
VertexNormalInputs GetVertexNormalInputs(float3 normalOS)
VertexNormalInputs GetVertexNormalInputs(float3 normalOS, float4 tangentOS)
float4 GetScaledScreenParams()
bool IsPerspectiveProjection()
float3 GetCameraPositionWS()
float3 GetCurrentViewPosition()
float3 GetViewForwardDir()
float3 GetWorldSpaceViewDir(float3 positionWS)
half3 GetObjectSpaceNormalizeViewDir(float3 positionOS)
half3 GetWorldSpaceNormalizeViewDir(float3 positionWS)
void GetLeftHandedViewSpaceMatrices(out float4x4 viewMatrix, out float4x4 projMatrix)
void AlphaDiscard(real alpha, real cutoff, real offset = real(0.0))
half OutputAlpha(half outputAlpha, half surfaceType = half(0.0))
half3 NormalizeNormalPerVertex(half3 normalWS)
float3 NormalizeNormalPerVertex(float3 normalWS)
half3 NormalizeNormalPerPixel(half3 normalWS)
float3 NormalizeNormalPerPixel(float3 normalWS)
real ComputeFogFactorZ0ToFar(float z)
real ComputeFogFactor(float zPositionCS)
half ComputeFogIntensity(half fogFactor)
real InitializeInputDataFog(float4 positionWS, real vertFogFactor)
float ComputeFogIntensity(float fogFactor)
half3 MixFogColor(half3 fragColor, half3 fogColor, half fogFactor)
float3 MixFogColor(float3 fragColor, float3 fogColor, float fogFactor)
half3 MixFog(half3 fragColor, half fogFactor)
float3 MixFog(float3 fragColor, float fogFactor)
half LinearDepthToEyeDepth(half rawDepth)
float LinearDepthToEyeDepth(float rawDepth)
void TransformScreenUV(inout float2 uv, float screenHeight)
void TransformScreenUV(inout float2 uv)
void TransformNormalizedScreenUV(inout float2 uv)
float2 GetNormalizedScreenSpaceUV(float2 positionCS)
float2 GetNormalizedScreenSpaceUV(float4 positionCS)
float2 TransformStereoScreenSpaceTex(float2 uv, float w)
float2 UnityStereoTransformScreenSpaceTex(float2 uv)
```



### Shadows

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Shadow/ShadowSamplingTent.hlsl"
#include "Core.hlsl"
//---多种可能定义，省略细节-----
#define SHADOWMASK_NAME
#define SHADOWMASK_SAMPLER_NAME
#define SHADOWMASK_SAMPLE_EXTRA_ARGS
#define SAMPLE_SHADOWMASK(uv)
//--------------
SCREENSPACE_TEXTURE(_ScreenSpaceShadowmapTexture);
SAMPLER(sampler_ScreenSpaceShadowmapTexture);
TEXTURE2D_SHADOW(_MainLightShadowmapTexture);
SAMPLER_CMP(sampler_MainLightShadowmapTexture);
TEXTURE2D_SHADOW(_AdditionalLightsShadowmapTexture);
SAMPLER_CMP(sampler_AdditionalLightsShadowmapTexture);
#define BEYOND_SHADOW_FAR(shadowCoord) shadowCoord.z <= 0.0 || shadowCoord.z >= 1.0
struct ShadowSamplingData
{
    half4 shadowOffset0;
    half4 shadowOffset1;
    half4 shadowOffset2;
    half4 shadowOffset3;
    float4 shadowmapSize;
};
ShadowSamplingData GetMainLightShadowSamplingData()
ShadowSamplingData GetAdditionalLightShadowSamplingData()
half4 GetMainLightShadowParams()
half4 GetAdditionalLightShadowParams(int lightIndex)
half SampleScreenSpaceShadowmap(float4 shadowCoord)
real SampleShadowmapFiltered(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord, ShadowSamplingData samplingData)
real SampleShadowmap(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord, ShadowSamplingData samplingData, half4 shadowParams, bool isPerspectiveProjection = true)
half ComputeCascadeIndex(float3 positionWS)
float4 TransformWorldToShadowCoord(float3 positionWS)
half MainLightRealtimeShadow(float4 shadowCoord)
half AdditionalLightRealtimeShadow(int lightIndex, float3 positionWS, half3 lightDirection)
half GetMainLightShadowFade(float3 positionWS)
half GetAdditionalLightShadowFade(float3 positionWS)
half MixRealtimeAndBakedShadows(half realtimeShadow, half bakedShadow, half shadowFade)
half BakedShadow(half4 shadowMask, half4 occlusionProbeChannels)
half MainLightShadow(float4 shadowCoord, float3 positionWS, half4 shadowMask, half4 occlusionProbeChannels)
half AdditionalLightShadow(int lightIndex, float3 positionWS, half3 lightDirection, half4 shadowMask, half4 occlusionProbeChannels)
float4 GetShadowCoord(VertexPositionInputs vertexInput)
float3 ApplyShadowBias(float3 positionWS, float3 normalWS, float3 lightDirection)
float GetShadowFade(float3 positionWS)
float ApplyShadowFade(float shadowAttenuation, float3 positionWS)
half GetMainLightShadowStrength()
half GetAdditionalLightShadowStrenth(int lightIndex)
real SampleShadowmap(float4 shadowCoord, TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), ShadowSamplingData samplingData, half shadowStrength, bool isPerspectiveProjection = true)
half AdditionalLightRealtimeShadow(int lightIndex, float3 positionWS)
```



### SSAO

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
TEXTURE2D_X(_BaseMap);
TEXTURE2D_X(_ScreenSpaceOcclusionTexture);
SAMPLER(sampler_BaseMap);
SAMPLER(sampler_ScreenSpaceOcclusionTexture);
// SSAO Settings
#define INTENSITY _SSAOParams.x
#define RADIUS _SSAOParams.y
#define DOWNSAMPLE _SSAOParams.z
#define SAMPLE_COUNT 3//两种情况在此省略
#define unity_eyeIndex 0//两种情况在此省略
#define SCREEN_PARAMS        GetScaledScreenParams()
#define SAMPLE_BASEMAP(uv)   SAMPLE_TEXTURE2D_X(_BaseMap, sampler_BaseMap, UnityStereoTransformScreenSpaceTex(uv));

half4 PackAONormal(half ao, half3 n)
half3 GetPackedNormal(half4 p)
half GetPackedAO(half4 p)
half EncodeAO(half x)
half CompareNormal(half3 d1, half3 d2)
half2 CosSin(half theta)
half GetRandomUVForSSAO(float u, int sampleIndex)
float2 GetScreenSpacePosition(float2 uv)
half3 PickSamplePoint(float2 uv, int sampleIndex)
float SampleAndGetLinearEyeDepth(float2 uv)
half3 ReconstructViewPos(float2 uv, float depth)
half3 ReconstructNormal(float2 uv, float depth, float3 vpos)
half3 SampleNormal(float2 uv)
void SampleDepthNormalView(float2 uv, out float depth, out half3 normal, out half3 vpos)
half4 SSAO(Varyings input) : SV_Target
half4 Blur(float2 uv, float2 delta) : SV_Target
half BlurSmall(float2 uv, float2 delta)
half4 HorizontalBlur(Varyings input) : SV_Target
half4 VerticalBlur(Varyings input) : SV_Target
half4 FinalBlur(Varyings input) : SV_Target
```



### SurfaceData

```c
// Must match Universal ShaderGraph master node
struct SurfaceData
{
    half3 albedo;
    half3 specular;
    half  metallic;
    half  smoothness;
    half3 normalTS;
    half3 emission;
    half  occlusion;
    half  alpha;
    half  clearCoatMask;
    half  clearCoatSmoothness;
};
```



### SurfaceInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);
float4 _BaseMap_TexelSize;
float4 _BaseMap_MipInfo;
TEXTURE2D(_BumpMap);
SAMPLER(sampler_BumpMap);
TEXTURE2D(_EmissionMap);
SAMPLER(sampler_EmissionMap);

half Alpha(half albedoAlpha, half4 color, half cutoff)
half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
```



### UnityGBuffer

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
struct FragmentOutput
{
    half4 GBuffer0 : SV_Target0;
    half4 GBuffer1 : SV_Target1;
    half4 GBuffer2 : SV_Target2;
    half4 GBuffer3 : SV_Target3; // Camera color attachment

    #ifdef GBUFFER_OPTIONAL_SLOT_1
    GBUFFER_OPTIONAL_SLOT_1_TYPE GBuffer4 : SV_Target4;
    #endif
    #ifdef GBUFFER_OPTIONAL_SLOT_2
    half4 GBuffer5 : SV_Target5;
    #endif
    #ifdef GBUFFER_OPTIONAL_SLOT_3
    half4 GBuffer6 : SV_Target6;
    #endif
};
float PackMaterialFlags(uint materialFlags)
uint UnpackMaterialFlags(float packedMaterialFlags)
half3 PackNormal(half3 n)
half3 UnpackNormal(half3 pn)
FragmentOutput SurfaceDataToGbuffer(SurfaceData surfaceData, InputData inputData, half3 globalIllumination, int lightingMode)
SurfaceData SurfaceDataFromGbuffer(half4 gbuffer0, half4 gbuffer1, half4 gbuffer2, int lightingMode)
FragmentOutput BRDFDataToGbuffer(BRDFData brdfData, InputData inputData, half smoothness, half3 globalIllumination, half occlusion = 1.0)
BRDFData BRDFDataFromGbuffer(half4 gbuffer0, half4 gbuffer1, half4 gbuffer2)
InputData InputDataFromGbufferAndWorldPosition(half4 gbuffer2, float3 wsPos)
//附各种定义
#define OUTPUT_SHADOWMASK 1 //或2 或3 多种情况在此省略
#if _RENDER_PASS_ENABLED
    #define GBUFFER_OPTIONAL_SLOT_1 GBuffer4
    #define GBUFFER_OPTIONAL_SLOT_1_TYPE float
#if OUTPUT_SHADOWMASK && defined(_LIGHT_LAYERS)
    #define GBUFFER_OPTIONAL_SLOT_2 GBuffer5
    #define GBUFFER_OPTIONAL_SLOT_3 GBuffer6
    #define GBUFFER_LIGHT_LAYERS GBuffer5
    #define GBUFFER_SHADOWMASK GBuffer6
#elif OUTPUT_SHADOWMASK
    #define GBUFFER_OPTIONAL_SLOT_2 GBuffer5
    #define GBUFFER_SHADOWMASK GBuffer5
#elif defined(_LIGHT_LAYERS)
    #define GBUFFER_OPTIONAL_SLOT_2 GBuffer5
    #define GBUFFER_LIGHT_LAYERS GBuffer5
#endif //#if OUTPUT_SHADOWMASK && defined(_LIGHT_LAYERS)
#else
    #define GBUFFER_OPTIONAL_SLOT_1_TYPE half4
#if OUTPUT_SHADOWMASK && defined(_LIGHT_LAYERS)
    #define GBUFFER_OPTIONAL_SLOT_1 GBuffer4
    #define GBUFFER_OPTIONAL_SLOT_2 GBuffer5
    #define GBUFFER_LIGHT_LAYERS GBuffer4
    #define GBUFFER_SHADOWMASK GBuffer5
#elif OUTPUT_SHADOWMASK
    #define GBUFFER_OPTIONAL_SLOT_1 GBuffer4
    #define GBUFFER_SHADOWMASK GBuffer4
#elif defined(_LIGHT_LAYERS)
    #define GBUFFER_OPTIONAL_SLOT_1 GBuffer4
    #define GBUFFER_LIGHT_LAYERS GBuffer4
#endif //#if OUTPUT_SHADOWMASK && defined(_LIGHT_LAYERS)
#endif //#if _RENDER_PASS_ENABLED
#define kLightingInvalid  -1  // No dynamic lighting: can aliase any other material type as they are skipped using stencil
#define kLightingLit       1  // lit shader
#define kLightingSimpleLit 2  // Simple lit shader
#define kMaterialFlagReceiveShadowsOff        1 // Does not receive dynamic shadows
#define kMaterialFlagSpecularHighlightsOff    2 // Does not receivce specular
#define kMaterialFlagSubtractiveMixedLighting 4 // The geometry uses subtractive mixed lighting
#define kMaterialFlagSpecularSetup            8 // Lit material use specular setup instead of metallic setup
#define kLightFlagSubtractiveMixedLighting    4 // The light uses subtractive mixed lighting.
```



### UnityInput

```c
// Current pass transforms.
#define glstate_matrix_projection     unity_StereoMatrixP[unity_StereoEyeIndex] // goes through GL.GetGPUProjectionMatrix()
#define unity_MatrixV                 unity_StereoMatrixV[unity_StereoEyeIndex]
#define unity_MatrixInvV              unity_StereoMatrixInvV[unity_StereoEyeIndex]
#define unity_MatrixInvP              unity_StereoMatrixInvP[unity_StereoEyeIndex]
#define unity_MatrixVP                unity_StereoMatrixVP[unity_StereoEyeIndex]
#define unity_MatrixInvVP             unity_StereoMatrixInvVP[unity_StereoEyeIndex]

// Camera transform (but the same as pass transform for XR).
#define unity_CameraProjection        unity_StereoCameraProjection[unity_StereoEyeIndex] // Does not go through GL.GetGPUProjectionMatrix()
#define unity_CameraInvProjection     unity_StereoCameraInvProjection[unity_StereoEyeIndex]
#define unity_WorldToCamera           unity_StereoMatrixV[unity_StereoEyeIndex] // Should be unity_StereoWorldToCamera but no use-case in XR pass
#define unity_CameraToWorld           unity_StereoMatrixInvV[unity_StereoEyeIndex] // Should be unity_StereoCameraToWorld but no use-case in XR pass
#define _WorldSpaceCameraPos          unity_StereoWorldSpaceCameraPos[unity_StereoEyeIndex]
#define UNITY_LIGHTMODEL_AMBIENT (glstate_lightmodel_ambient * 2)
// ----------------------------------------------------------------------------
real4 glstate_lightmodel_ambient;
real4 unity_AmbientSky;
real4 unity_AmbientEquator;
real4 unity_AmbientGround;
real4 unity_IndirectSpecColor;
float4 unity_FogParams;
real4  unity_FogColor;

#if !defined(USING_STEREO_MATRICES)
float4x4 glstate_matrix_projection;
float4x4 unity_MatrixV;
float4x4 unity_MatrixInvV;
float4x4 unity_MatrixInvP;
float4x4 unity_MatrixVP;
float4x4 unity_MatrixInvVP;
float4 unity_StereoScaleOffset;
int unity_StereoEyeIndex;
#endif

real4 unity_ShadowColor;
// ----------------------------------------------------------------------------
// Unity specific
TEXTURECUBE(unity_SpecCube0);
SAMPLER(samplerunity_SpecCube0);
TEXTURECUBE(unity_SpecCube1);
SAMPLER(samplerunity_SpecCube1);

// Main lightmap
TEXTURE2D(unity_Lightmap);
SAMPLER(samplerunity_Lightmap);
TEXTURE2D_ARRAY(unity_Lightmaps);
SAMPLER(samplerunity_Lightmaps);

// Dynamic lightmap
TEXTURE2D(unity_DynamicLightmap);
SAMPLER(samplerunity_DynamicLightmap);
// TODO ENLIGHTEN: Instanced GI

// Dual or directional lightmap (always used with unity_Lightmap, so can share sampler)
TEXTURE2D(unity_LightmapInd);
TEXTURE2D_ARRAY(unity_LightmapsInd);
TEXTURE2D(unity_DynamicDirectionality);
// TODO ENLIGHTEN: Instanced GI
// TEXTURE2D_ARRAY(unity_DynamicDirectionality);

TEXTURE2D(unity_ShadowMask);
SAMPLER(samplerunity_ShadowMask);
TEXTURE2D_ARRAY(unity_ShadowMasks);
SAMPLER(samplerunity_ShadowMasks);

// ----------------------------------------------------------------------------

// TODO: all affine matrices should be 3x4.
// TODO: sort these vars by the frequency of use (descending), and put commonly used vars together.
// Note: please use UNITY_MATRIX_X macros instead of referencing matrix variables directly.
float4x4 _PrevViewProjMatrix;
float4x4 _ViewProjMatrix;
float4x4 _NonJitteredViewProjMatrix;
float4x4 _ViewMatrix;
float4x4 _ProjMatrix;
float4x4 _InvViewProjMatrix;
float4x4 _InvViewMatrix;
float4x4 _InvProjMatrix;
float4   _InvProjParam;
float4   _ScreenSize;       // {w, h, 1/w, 1/h}
float4   _FrustumPlanes[6]; // {(a, b, c) = N, d = -dot(N, P)} [L, R, T, B, N, F]

float4x4 OptimizeProjectionMatrix(float4x4 M)
{
    // Matrix format (x = non-constant value).
    // Orthographic Perspective  Combined(OR)
    // | x 0 0 x |  | x 0 x 0 |  | x 0 x x |
    // | 0 x 0 x |  | 0 x x 0 |  | 0 x x x |
    // | x x x x |  | x x x x |  | x x x x | <- oblique projection row
    // | 0 0 0 1 |  | 0 0 x 0 |  | 0 0 x x |
    // Notice that some values are always 0.
    // We can avoid loading and doing math with constants.
    M._21_41 = 0;
    M._12_42 = 0;
    return M;
}

```



### UnityTypes

```c
// Match  UnityEngine.TextureWrapMode
#define URP_TEXTURE_WRAP_MODE_REPEAT       0
#define URP_TEXTURE_WRAP_MODE_CLAMP        1
#define URP_TEXTURE_WRAP_MODE_MIRROR       2
#define URP_TEXTURE_WRAP_MODE_MIRROR_ONCE  3
// Additional NULL case for shaders
#define URP_TEXTURE_WRAP_MODE_NONE        -1

// Match  UnityEngine.LightType
#define URP_LIGHT_TYPE_SPOT        0
#define URP_LIGHT_TYPE_DIRECTIONAL 1
#define URP_LIGHT_TYPE_POINT       2
// Area and Rectangle are aliases
#define URP_LIGHT_TYPE_AREA        3
#define URP_LIGHT_TYPE_RECTANGLE   3
#define URP_LIGHT_TYPE_DISC        4
```



### UniversalDOTSInstancing

```c
#ifdef UNITY_DOTS_INSTANCING_ENABLED
#undef unity_ObjectToWorld
#undef unity_WorldToObject
#undef unity_MatrixPreviousM
#undef unity_MatrixPreviousMI
// TODO: This might not work correctly in all cases, double check!
UNITY_DOTS_INSTANCING_START(BuiltinPropertyMetadata)
    UNITY_DOTS_INSTANCED_PROP(float3x4, unity_ObjectToWorld)
    UNITY_DOTS_INSTANCED_PROP(float3x4, unity_WorldToObject)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_LODFade)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_WorldTransformParams)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_RenderingLayer)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_LightData)
    UNITY_DOTS_INSTANCED_PROP(float2x4, unity_LightIndices)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_ProbesOcclusion)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SpecCube0_HDR)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_LightmapST)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_LightmapIndex)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_DynamicLightmapST)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SHAr)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SHAg)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SHAb)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SHBr)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SHBg)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SHBb)
    UNITY_DOTS_INSTANCED_PROP(float4,   unity_SHC)
    UNITY_DOTS_INSTANCED_PROP(float3x4, unity_MatrixPreviousM)
    UNITY_DOTS_INSTANCED_PROP(float3x4, unity_MatrixPreviousMI)
UNITY_DOTS_INSTANCING_END(BuiltinPropertyMetadata)

// Note: Macros for unity_ObjectToWorld, unity_WorldToObject, unity_MatrixPreviousM and unity_MatrixPreviousMI are declared in UnityInstancing.hlsl
// because of some special handling
#define unity_LODFade               UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_LODFade)
#define unity_WorldTransformParams  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_WorldTransformParams)
#define unity_RenderingLayer        UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_RenderingLayer)
#define unity_LightData             UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_LightData)
#define unity_LightIndices          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float2x4, Metadataunity_LightIndices)
#define unity_ProbesOcclusion       UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_ProbesOcclusion)
#define unity_SpecCube0_HDR         UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SpecCube0_HDR)
#define unity_LightmapST            UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_LightmapST)
#define unity_LightmapIndex         UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_LightmapIndex)
#define unity_DynamicLightmapST     UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_DynamicLightmapST)
#define unity_SHAr                  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SHAr)
#define unity_SHAg                  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SHAg)
#define unity_SHAb                  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SHAb)
#define unity_SHBr                  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SHBr)
#define unity_SHBg                  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SHBg)
#define unity_SHBb                  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SHBb)
#define unity_SHC                   UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4,   Metadataunity_SHC)
#endif
```



### UniversalMetaPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 uv0          : TEXCOORD0;
    float2 uv1          : TEXCOORD1;
    float2 uv2          : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS   : SV_POSITION;
    float2 uv           : TEXCOORD0;
#ifdef EDITOR_VISUALIZATION
    float2 VizUV        : TEXCOORD1;
    float4 LightCoord   : TEXCOORD2;
#endif
};

Varyings UniversalVertexMeta(Attributes input)
{
    Varyings output = (Varyings)0;
    output.positionCS = UnityMetaVertexPosition(input.positionOS.xyz, input.uv1, input.uv2);
    output.uv = TRANSFORM_TEX(input.uv0, _BaseMap);
#ifdef EDITOR_VISUALIZATION
    UnityEditorVizData(input.positionOS.xyz, input.uv0, input.uv1, input.uv2, output.VizUV, output.LightCoord);
#endif
    return output;
}

half4 UniversalFragmentMeta(Varyings fragIn, MetaInput metaInput)
{
#ifdef EDITOR_VISUALIZATION
    metaInput.VizUV = fragIn.VizUV;
    metaInput.LightCoord = fragIn.LightCoord;
#endif

    return UnityMetaFragment(metaInput);
}
```



### Unlit

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

half4 UniversalFragmentUnlit(InputData inputData, SurfaceData surfaceData)
{
    half3 albedo = surfaceData.albedo;

    #if defined(DEBUG_DISPLAY)
    half4 debugColor;

    if (CanDebugOverrideOutputColor(inputData, surfaceData, debugColor))
    {
        return debugColor;
    }
    #endif

    half4 finalColor = half4(albedo + surfaceData.emission, surfaceData.alpha);

    return finalColor;
}

// Deprecated: Use the version which takes "SurfaceData" instead of passing all of these arguments.
half4 UniversalFragmentUnlit(InputData inputData, half3 color, half alpha)
{
    SurfaceData surfaceData;

    surfaceData.albedo = color;
    surfaceData.alpha = alpha;
    surfaceData.emission = 0;
    surfaceData.metallic = 0;
    surfaceData.occlusion = 1;
    surfaceData.smoothness = 1;
    surfaceData.specular = 0;
    surfaceData.clearCoatMask = 0;
    surfaceData.clearCoatSmoothness = 1;
    surfaceData.normalTS = half3(0, 0, 1);

    return UniversalFragmentUnlit(inputData, surfaceData);
}
```



### VisualEffectVertex

```c
// Wrapper vertex invocations for VFX. Necesarry to work around various null input geometry issues for vertex input layout on DX12 and Vulkan.
#if NULL_GEOMETRY_INPUT
PackedVaryings VertVFX(uint vertexID : VERTEXID_SEMANTIC, uint instanceID : INSTANCEID_SEMANTIC)
{
    Attributes input;
    ZERO_INITIALIZE(Attributes, input);

    input.vertexID = vertexID;
    input.instanceID = instanceID;

    return vert(input);
}
#else
PackedVaryings VertVFX(Attributes input)
{
    return vert(input);
}
#endif
```



## Debug

### Debugging2D

```c
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl"
#if defined(DEBUG_DISPLAY)//---------------

#define SETUP_DEBUG_TEXTURE_DATA_2D(inputData, positionWS, texture)    SetupDebugDataTexture(inputData, positionWS, texture##_TexelSize, texture##_MipInfo, GetMipCount(TEXTURE2D_ARGS(texture, sampler##texture)))
#define SETUP_DEBUG_DATA_2D(inputData, positionWS)  SetupDebugData(inputData, positionWS)

void SetupDebugData(inout InputData2D inputData, float3 positionWS)
void SetupDebugDataTexture(inout InputData2D inputData, float3 positionWS, float4 texelSize, float4 mipInfo, uint mipCount)
bool CalculateDebugColorMaterialSettings(in SurfaceData2D surfaceData, in InputData2D inputData, inout half4 debugColor)
bool CalculateDebugColorForRenderingSettings(in SurfaceData2D surfaceData, in InputData2D inputData, inout half4 debugColor)
bool CalculateDebugColorLightingSettings(inout SurfaceData2D surfaceData, inout InputData2D inputData, inout half4 debugColor)
    bool CalculateDebugColorValidationSettings(in SurfaceData2D surfaceData, in InputData2D inputData, inout half4 debugColor)
}
bool CanDebugOverrideOutputColor(inout SurfaceData2D surfaceData, inout InputData2D inputData, inout half4 debugColor)
#else//-------------------------
#define SETUP_DEBUG_TEXTURE_DATA_2D(inputData, positionWS, texture)
#define SETUP_DEBUG_DATA_2D(inputData, positionWS)
#endif//---------------------------
```



### Debugging3D

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl"
#if defined(DEBUG_DISPLAY)//-------------------
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/BRDF.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/GlobalIllumination.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

#define SETUP_DEBUG_TEXTURE_DATA(inputData, uv, texture)    SetupDebugDataTexture(inputData, uv, texture##_TexelSize, texture##_MipInfo, GetMipCount(TEXTURE2D_ARGS(texture, sampler##texture)))
void SetupDebugDataTexture(inout InputData inputData, float2 uv, float4 texelSize, float4 mipInfo, uint mipCount)
void SetupDebugDataBrdf(inout InputData inputData, half3 brdfDiffuse, half3 brdfSpecular)
bool UpdateSurfaceAndInputDataForDebug(inout SurfaceData surfaceData, inout InputData inputData)
bool CalculateValidationMetallic(half3 albedo, half metallic, inout half4 debugColor)
bool CalculateValidationColorForDebug(in InputData inputData, in SurfaceData surfaceData, inout half4 debugColor)
}
bool CalculateColorForDebugMaterial(in InputData inputData, in SurfaceData surfaceData, inout half4 debugColor)
}
bool CalculateColorForDebug(in InputData inputData, in SurfaceData surfaceData, inout half4 debugColor)
half3 CalculateDebugShadowCascadeColor(in InputData inputData)
half4 CalculateDebugLightingComplexityColor(in InputData inputData, in SurfaceData surfaceData)
bool CanDebugOverrideOutputColor(inout InputData inputData, inout SurfaceData surfaceData, inout BRDFData brdfData, inout half4 debugColor)
bool CanDebugOverrideOutputColor(inout InputData inputData, inout SurfaceData surfaceData, inout half4 debugColor)
#else//------------------------------
#define SETUP_DEBUG_TEXTURE_DATA(inputData, uv, texture)
#endif//----------------------------
```



### DebuggingCommon

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebugViewEnums.cs.hlsl"
#if #defined(DEBUG_DISPLAY)//-----------
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Debug.hlsl"
// Material settings...
int _DebugMaterialMode;
int _DebugVertexAttributeMode;
int _DebugMaterialValidationMode;

// Rendering settings...
int _DebugFullScreenMode;
int _DebugSceneOverrideMode;
int _DebugMipInfoMode;
int _DebugValidationMode;

// Lighting settings...
int _DebugLightingMode;
int _DebugLightingFeatureFlags;

half _DebugValidateAlbedoMinLuminance = 0.01;
half _DebugValidateAlbedoMaxLuminance = 0.90;
half _DebugValidateAlbedoSaturationTolerance = 0.214;
half _DebugValidateAlbedoHueTolerance = 0.104;
half3 _DebugValidateAlbedoCompareColor = half3(0.5, 0.5, 0.5);

half _DebugValidateMetallicMinValue = 0;
half _DebugValidateMetallicMaxValue = 0.9;

float4 _DebugColor;
float4 _DebugColorInvalidMode;
float4 _DebugValidateBelowMinThresholdColor;
float4 _DebugValidateAboveMaxThresholdColor;
half3 GetDebugColor(uint index)
bool TryGetDebugColorInvalidMode(out half4 debugColor)
uint GetMipMapLevel(float2 nonNormalizedUVCoordinate)
bool CalculateValidationAlbedo(half3 albedo, out half4 color)
bool CalculateColorForDebugSceneOverride(out half4 color)
#endif//---------------------
bool IsAlphaDiscardEnabled()
bool IsFogEnabled()
bool IsLightingFeatureEnabled(uint bitMask)
bool IsOnlyAOLightingFeatureEnabled()
```



### DebuggingFullscreen

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl"
#if defined(DEBUG_DISPLAY)//----------------------
int _ValidationChannels;
float _RangeMinimum;
float _RangeMaximum;
TEXTURE2D_X(_DebugTexture);
TEXTURE2D(_DebugTextureNoStereo);
SAMPLER(sampler_DebugTexture);
half4 _DebugTextureDisplayRect;
int _DebugRenderTargetSupportsStereo;
bool CalculateDebugColorRenderingSettings(half4 color, float2 uv, inout half4 debugColor)
bool CalculateDebugColorValidationSettings(half4 color, float2 uv, inout half4 debugColor)
bool CanDebugOverrideOutputColor(half4 color, float2 uv, inout half4 debugColor)
#endif
```



### DebugViewEnums.cs

自动生成代码

```c
//
// This file was automatically generated. Please don't edit by hand. Execute Editor command [ Edit > Rendering > Generate Shader Includes ] instead
//

#ifndef DEBUGVIEWENUMS_CS_HLSL
#define DEBUGVIEWENUMS_CS_HLSL
//
// UnityEngine.Rendering.Universal.DebugMaterialMode:  static fields
//
#define DEBUGMATERIALMODE_NONE (0)
#define DEBUGMATERIALMODE_ALBEDO (1)
#define DEBUGMATERIALMODE_SPECULAR (2)
#define DEBUGMATERIALMODE_ALPHA (3)
#define DEBUGMATERIALMODE_SMOOTHNESS (4)
#define DEBUGMATERIALMODE_AMBIENT_OCCLUSION (5)
#define DEBUGMATERIALMODE_EMISSION (6)
#define DEBUGMATERIALMODE_NORMAL_WORLD_SPACE (7)
#define DEBUGMATERIALMODE_NORMAL_TANGENT_SPACE (8)
#define DEBUGMATERIALMODE_LIGHTING_COMPLEXITY (9)
#define DEBUGMATERIALMODE_METALLIC (10)
#define DEBUGMATERIALMODE_SPRITE_MASK (11)

//
// UnityEngine.Rendering.Universal.DebugVertexAttributeMode:  static fields
//
#define DEBUGVERTEXATTRIBUTEMODE_NONE (0)
#define DEBUGVERTEXATTRIBUTEMODE_TEXCOORD0 (1)
#define DEBUGVERTEXATTRIBUTEMODE_TEXCOORD1 (2)
#define DEBUGVERTEXATTRIBUTEMODE_TEXCOORD2 (3)
#define DEBUGVERTEXATTRIBUTEMODE_TEXCOORD3 (4)
#define DEBUGVERTEXATTRIBUTEMODE_COLOR (5)
#define DEBUGVERTEXATTRIBUTEMODE_TANGENT (6)
#define DEBUGVERTEXATTRIBUTEMODE_NORMAL (7)

//
// UnityEngine.Rendering.Universal.DebugMaterialValidationMode:  static fields
//
#define DEBUGMATERIALVALIDATIONMODE_NONE (0)
#define DEBUGMATERIALVALIDATIONMODE_ALBEDO (1)
#define DEBUGMATERIALVALIDATIONMODE_METALLIC (2)

//
// UnityEngine.Rendering.Universal.DebugFullScreenMode:  static fields
//
#define DEBUGFULLSCREENMODE_NONE (0)
#define DEBUGFULLSCREENMODE_DEPTH (1)
#define DEBUGFULLSCREENMODE_ADDITIONAL_LIGHTS_SHADOW_MAP (2)
#define DEBUGFULLSCREENMODE_MAIN_LIGHT_SHADOW_MAP (3)

//
// UnityEngine.Rendering.Universal.DebugSceneOverrideMode:  static fields
//
#define DEBUGSCENEOVERRIDEMODE_NONE (0)
#define DEBUGSCENEOVERRIDEMODE_OVERDRAW (1)
#define DEBUGSCENEOVERRIDEMODE_WIREFRAME (2)
#define DEBUGSCENEOVERRIDEMODE_SOLID_WIREFRAME (3)
#define DEBUGSCENEOVERRIDEMODE_SHADED_WIREFRAME (4)

//
// UnityEngine.Rendering.Universal.DebugMipInfoMode:  static fields
//
#define DEBUGMIPINFOMODE_NONE (0)
#define DEBUGMIPINFOMODE_LEVEL (1)
#define DEBUGMIPINFOMODE_COUNT (2)
#define DEBUGMIPINFOMODE_RATIO (3)

//
// UnityEngine.Rendering.Universal.DebugPostProcessingMode:  static fields
//
#define DEBUGPOSTPROCESSINGMODE_DISABLED (0)
#define DEBUGPOSTPROCESSINGMODE_AUTO (1)
#define DEBUGPOSTPROCESSINGMODE_ENABLED (2)

//
// UnityEngine.Rendering.Universal.DebugValidationMode:  static fields
//
#define DEBUGVALIDATIONMODE_NONE (0)
#define DEBUGVALIDATIONMODE_HIGHLIGHT_NAN_INF_NEGATIVE (1)
#define DEBUGVALIDATIONMODE_HIGHLIGHT_OUTSIDE_OF_RANGE (2)

//
// UnityEngine.Rendering.Universal.PixelValidationChannels:  static fields
//
#define PIXELVALIDATIONCHANNELS_RGB (0)
#define PIXELVALIDATIONCHANNELS_R (1)
#define PIXELVALIDATIONCHANNELS_G (2)
#define PIXELVALIDATIONCHANNELS_B (3)
#define PIXELVALIDATIONCHANNELS_A (4)

//
// UnityEngine.Rendering.Universal.DebugLightingMode:  static fields
//
#define DEBUGLIGHTINGMODE_NONE (0)
#define DEBUGLIGHTINGMODE_SHADOW_CASCADES (1)
#define DEBUGLIGHTINGMODE_LIGHTING_WITHOUT_NORMAL_MAPS (2)
#define DEBUGLIGHTINGMODE_LIGHTING_WITH_NORMAL_MAPS (3)
#define DEBUGLIGHTINGMODE_REFLECTIONS (4)
#define DEBUGLIGHTINGMODE_REFLECTIONS_WITH_SMOOTHNESS (5)

//
// UnityEngine.Rendering.Universal.DebugLightingFeatureFlags:  static fields
//
#define DEBUGLIGHTINGFEATUREFLAGS_NONE (0)
#define DEBUGLIGHTINGFEATUREFLAGS_GLOBAL_ILLUMINATION (1)
#define DEBUGLIGHTINGFEATUREFLAGS_MAIN_LIGHT (2)
#define DEBUGLIGHTINGFEATUREFLAGS_ADDITIONAL_LIGHTS (4)
#define DEBUGLIGHTINGFEATUREFLAGS_VERTEX_LIGHTING (8)
#define DEBUGLIGHTINGFEATUREFLAGS_EMISSION (16)
#define DEBUGLIGHTINGFEATUREFLAGS_AMBIENT_OCCLUSION (32)


#endif

```



## LightCookie
### LightCookie

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LightCookie/LightCookieInput.hlsl"

#if defined(_LIGHT_COOKIES)
    #ifndef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
        #define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1
    #endif
#endif
float2 ComputeLightCookieUVDirectional(float4x4 worldToLight, float3 samplePositionWS, float4 atlasUVRect, uint2 uvWrap)
float2 ComputeLightCookieUVSpot(float4x4 worldToLightPerspective, float3 samplePositionWS, float4 atlasUVRect)
float2 ComputeLightCookieUVPoint(float4x4 worldToLight, float3 samplePositionWS, float4 atlasUVRect)
real3 SampleMainLightCookie(float3 samplePositionWS)
real3 SampleAdditionalLightCookie(int perObjectLightIndex, float3 samplePositionWS)
```



### LightCookieInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LightCookie/LightCookieTypes.hlsl"

// Textures
TEXTURE2D(_MainLightCookieTexture);
TEXTURE2D(_AdditionalLightsCookieAtlasTexture);

// Samplers
SAMPLER(sampler_MainLightCookieTexture);
SAMPLER(sampler_AdditionalLightsCookieAtlasTexture);

// Buffers
// GLES3 causes a performance regression in some devices when using CBUFFER.
#ifndef SHADER_API_GLES3
CBUFFER_START(LightCookies)
#endif
    float4x4 _MainLightWorldToLight;
    float _AdditionalLightsCookieEnableBits[(MAX_VISIBLE_LIGHTS + 31) / 32];
    float _MainLightCookieTextureFormat;
    float _AdditionalLightsCookieAtlasTextureFormat;
#ifndef SHADER_API_GLES3
CBUFFER_END
#endif
    
#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
    StructuredBuffer<float4x4> _AdditionalLightsWorldToLightBuffer;
	// UV rect into light cookie atlas (xy: uv offset, zw: uv size)
    StructuredBuffer<float4>   _AdditionalLightsCookieAtlasUVRectBuffer; 
    StructuredBuffer<float>    _AdditionalLightsLightTypeBuffer;
#else
    #ifndef SHADER_API_GLES3
        CBUFFER_START(AdditionalLightsCookies)
    #endif
        float4x4 _AdditionalLightsWorldToLights[MAX_VISIBLE_LIGHTS];
        // (xy: uv size, zw: uv offset)
        float4 _AdditionalLightsCookieAtlasUVRects[MAX_VISIBLE_LIGHTS];
        float _AdditionalLightsLightTypes[MAX_VISIBLE_LIGHTS];
    #ifndef SHADER_API_GLES3
        CBUFFER_END
    #endif
#endif
            
float4x4 GetLightCookieWorldToLightMatrix(int lightIndex)
float4 GetLightCookieAtlasUVRect(int lightIndex)
int GetLightCookieLightType(int lightIndex)
bool IsMainLightCookieTextureRGBFormat()
bool IsMainLightCookieTextureAlphaFormat()
bool IsAdditionalLightsCookieAtlasTextureRGBFormat()
bool IsAdditionalLightsCookieAtlasTextureAlphaFormat()
real4 SampleMainLightCookieTexture(float2 uv)
real4 SampleAdditionalLightsCookieAtlasTexture(float2 uv)
bool IsMainLightCookieEnabled()
bool IsLightCookieEnabled(int lightBufferIndex)
```



### LightCookieTypes

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityTypes.hlsl"

// Types
#define URP_LIGHT_COOKIE_FORMAT_NONE (-1)
#define URP_LIGHT_COOKIE_FORMAT_RGB (0)
#define URP_LIGHT_COOKIE_FORMAT_ALPHA (1)
#define URP_LIGHT_COOKIE_FORMAT_RED (2)
```



# Universal Shaders

## 根目录
### BakedLitDepthNormalsPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS   : POSITION;
    float2 uv           : TEXCOORD0;
    half3 normalOS      : NORMAL;
    half4 tangentOS     : TANGENT;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 vertex       : SV_POSITION;
    float2 uv           : TEXCOORD0;
    half3 normalWS      : TEXCOORD1;

    #if defined(_NORMALMAP)
        half4 tangentWS : TEXCOORD3;
    #endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
Varyings DepthNormalsVertex(Attributes input)
float4 DepthNormalsFragment(Varyings input) : SV_TARGET
```



### BakedLitForwardPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes
{
    float4 positionOS       : POSITION;
    float2 uv               : TEXCOORD0;
    float2 staticLightmapUV : TEXCOORD1;
    float3 normalOS         : NORMAL;
    float4 tangentOS        : TANGENT;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float3 uv0AndFogCoord : TEXCOORD0; // xy: uv0, z: fogCoord
    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 1);
    half3 normalWS : TEXCOORD2;

    #if defined(_NORMALMAP)
    half4 tangentWS : TEXCOORD3;
    #endif

    #if defined(DEBUG_DISPLAY)
    float3 positionWS : TEXCOORD4;
    float3 viewDirWS : TEXCOORD5;
    #endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
Varyings BakedLitForwardPassVertex(Attributes input)
half4 BakedLitForwardPassFragment(Varyings input) : SV_Target
```



### BakedLitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _BaseColor;
half _Cutoff;
half _Glossiness;
half _Metallic;
half _Surface;
CBUFFER_END

#ifdef UNITY_DOTS_INSTANCING_ENABLED
UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
  UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
  UNITY_DOTS_INSTANCED_PROP(float , _Cutoff)
  UNITY_DOTS_INSTANCED_PROP(float , _Glossiness)
  UNITY_DOTS_INSTANCED_PROP(float , _Metallic)
  UNITY_DOTS_INSTANCED_PROP(float , _Surface)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

#define _BaseColor  UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_BaseColor)
#define _Cutoff     UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Cutoff)
#define _Glossiness UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Glossiness)
#define _Metallic   UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Metallic)
#define _Surface    UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Surface)
#endif
```



### BakedLitMetaPass

```c
#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitMetaPass.hlsl"
```



### DepthNormalsPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 positionOS     : POSITION;
    float4 tangentOS      : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float3 normal       : NORMAL;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS   : SV_POSITION;
    float2 uv           : TEXCOORD1;
    float3 normalWS                 : TEXCOORD2;

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
Varyings DepthNormalsVertex(Attributes input)
half4 DepthNormalsFragment(Varyings input) : SV_TARGET
```



### DepthOnlyPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct Attributes
{
    float4 position     : POSITION;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv           : TEXCOORD0;
    float4 positionCS   : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
Varyings DepthOnlyVertex(Attributes input)
half4 DepthOnlyFragment(Varyings input) : SV_TARGET
```



### LitDepthNormalsPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
struct Attributes
{
    float4 positionOS     : POSITION;
    float4 tangentOS      : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float3 normal       : NORMAL;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS   : SV_POSITION;
    float2 uv           : TEXCOORD1;
    half3 normalWS     : TEXCOORD2;

    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS    : TEXCOORD4;    // xyz: tangent, w: sign
    #endif

    half3 viewDirWS    : TEXCOORD5;

    #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    half3 viewDirTS     : TEXCOORD8;
    #endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings DepthNormalsVertex(Attributes input)
half4 DepthNormalsFragment(Varyings input) : SV_TARGET
```



### LitForwardPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
// keep this file in sync with LitGBufferPass.hlsl
struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float2 staticLightmapUV   : TEXCOORD1;
    float2 dynamicLightmapUV  : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv         : TEXCOORD0;
	#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    float3 positionWS : TEXCOORD1;
	#endif
    float3 normalWS   : TEXCOORD2;
	#if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
	// xyz: tangent, w: sign
    half4 tangentWS   : TEXCOORD3;
	#endif
	#ifdef _ADDITIONAL_LIGHTS_VERTEX
	// x: fogFactor, yzw: vertex light
    half4 fogFactorAndVertexLight   : TEXCOORD5; 
	#else
    half  fogFactor   : TEXCOORD5;
	#endif
	#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord  : TEXCOORD6;
	#endif
	#if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    half3 viewDirTS    : TEXCOORD7;
	#endif
    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 8);
	#ifdef DYNAMICLIGHTMAP_ON
	// Dynamic lightmap UVs
    float2  dynamicLightmapUV : TEXCOORD9; 
	#endif
    float4 positionCS               : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
Varyings LitPassVertex(Attributes input)
// Used in Standard (Physically Based) shader
half4 LitPassFragment(Varyings input) : SV_Target
```



### LitGBufferPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float2 staticLightmapUV   : TEXCOORD1;
    float2 dynamicLightmapUV  : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float2 uv          : TEXCOORD0;
	#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    float3 positionWS  : TEXCOORD1;
	#endif
    half3 normalWS     : TEXCOORD2;
	#if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS    : TEXCOORD3;    // xyz: tangent, w: sign
	#endif
	#ifdef _ADDITIONAL_LIGHTS_VERTEX
    half3 vertexLighting : TEXCOORD4;    // xyz: vertex lighting
	#endif
	#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord   : TEXCOORD5;
	#endif
	#if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    half3 viewDirTS      : TEXCOORD6;
	#endif
    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 7);
	#ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD8; // Dynamic lightmap UVs
	#endif
    float4 positionCS     : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
// Used in Standard (Physically Based) shader
Varyings LitGBufferPassVertex(Attributes input)
// Used in Standard (Physically Based) shader
FragmentOutput LitGBufferPassFragment(Varyings input)
```



### LitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
#if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
#define _DETAIL
#endif

// NOTE: Do not ifdef the properties here as SRP batcher can not handle different layouts.
CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
float4 _DetailAlbedoMap_ST;
half4 _BaseColor;
half4 _SpecColor;
half4 _EmissionColor;
half _Cutoff;
half _Smoothness;
half _Metallic;
half _BumpScale;
half _Parallax;
half _OcclusionStrength;
half _ClearCoatMask;
half _ClearCoatSmoothness;
half _DetailAlbedoMapScale;
half _DetailNormalMapScale;
half _Surface;
CBUFFER_END

// NOTE: Do not ifdef the properties for dots instancing, but ifdef the actual usage.
// Otherwise you might break CPU-side as property constant-buffer offsets change per variant.
// NOTE: Dots instancing is orthogonal to the constant buffer above.
#ifdef UNITY_DOTS_INSTANCING_ENABLED
UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DOTS_INSTANCED_PROP(float4, _SpecColor)
    UNITY_DOTS_INSTANCED_PROP(float4, _EmissionColor)
    UNITY_DOTS_INSTANCED_PROP(float , _Cutoff)
    UNITY_DOTS_INSTANCED_PROP(float , _Smoothness)
    UNITY_DOTS_INSTANCED_PROP(float , _Metallic)
    UNITY_DOTS_INSTANCED_PROP(float , _BumpScale)
    UNITY_DOTS_INSTANCED_PROP(float , _Parallax)
    UNITY_DOTS_INSTANCED_PROP(float , _OcclusionStrength)
    UNITY_DOTS_INSTANCED_PROP(float , _ClearCoatMask)
    UNITY_DOTS_INSTANCED_PROP(float , _ClearCoatSmoothness)
    UNITY_DOTS_INSTANCED_PROP(float , _DetailAlbedoMapScale)
    UNITY_DOTS_INSTANCED_PROP(float , _DetailNormalMapScale)
    UNITY_DOTS_INSTANCED_PROP(float , _Surface)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

#define _BaseColor              UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_BaseColor)
#define _SpecColor              UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_SpecColor)
#define _EmissionColor          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_EmissionColor)
#define _Cutoff                 UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Cutoff)
#define _Smoothness             UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Smoothness)
#define _Metallic               UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Metallic)
#define _BumpScale              UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_BumpScale)
#define _Parallax               UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Parallax)
#define _OcclusionStrength      UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_OcclusionStrength)
#define _ClearCoatMask          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_ClearCoatMask)
#define _ClearCoatSmoothness    UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_ClearCoatSmoothness)
#define _DetailAlbedoMapScale   UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_DetailAlbedoMapScale)
#define _DetailNormalMapScale   UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_DetailNormalMapScale)
#define _Surface                UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Surface)
#endif

TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);

#ifdef _SPECULAR_SETUP
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
#else
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
#endif

half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
half SampleOcclusion(float2 uv)
half2 SampleClearCoat(float2 uv)
void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
```



### LitMetaPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UniversalMetaPass.hlsl"

half4 UniversalFragmentMetaLit(Varyings input) : SV_Target
{
    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.Emission = surfaceData.emission;
    return UniversalFragmentMeta(input, metaInput);
}
```



### ShadowCasterPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

float3 _LightDirection;
float3 _LightPosition;

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv           : TEXCOORD0;
    float4 positionCS   : SV_POSITION;
};
float4 GetShadowPositionHClip(Attributes input)
Varyings ShadowPassVertex(Attributes input)
half4 ShadowPassFragment(Varyings input) : SV_TARGET
```



### SimpleLitDepthNormalsPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes
{
    float4 positionOS   : POSITION;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float3 normal       : NORMAL;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS      : SV_POSITION;
    float2 uv              : TEXCOORD1;

    #ifdef _NORMALMAP
        half4 normalWS    : TEXCOORD2;    // xyz: normal, w: viewDir.x
        half4 tangentWS   : TEXCOORD3;    // xyz: tangent, w: viewDir.y
        half4 bitangentWS : TEXCOORD4;    // xyz: bitangent, w: viewDir.z
    #else
        half3 normalWS    : TEXCOORD2;
        half3 viewDir     : TEXCOORD3;
    #endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
Varyings DepthNormalsVertex(Attributes input)
half4 DepthNormalsFragment(Varyings input) : SV_TARGET
```



### SimpleLitForwardPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
struct Attributes
{
    float4 positionOS    : POSITION;
    float3 normalOS      : NORMAL;
    float4 tangentOS     : TANGENT;
    float2 texcoord      : TEXCOORD0;
    float2 staticLightmapUV    : TEXCOORD1;
    float2 dynamicLightmapUV    : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float2 uv              : TEXCOORD0;
    float3 positionWS      : TEXCOORD1;    // xyz: posWS
    #ifdef _NORMALMAP
	half4 normalWS         : TEXCOORD2; // xyz: normal, w: viewDir.x
	half4 tangentWS        : TEXCOORD3; // xyz: tangent, w: viewDir.y
	half4 bitangentWS      : TEXCOORD4; // xyz: bitangent, w: viewDir.z
    #else
	half3  normalWS        : TEXCOORD2;
    #endif
    #ifdef _ADDITIONAL_LIGHTS_VERTEX
	// x: fogFactor, yzw: vertex light
	half4 fogFactorAndVertexLight : TEXCOORD5; 
    #else
	half  fogFactor               : TEXCOORD5;
    #endif
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	float4 shadowCoord            : TEXCOORD6;
    #endif
    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 7);
	#ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD8; // Dynamic lightmap UVs
	#endif
    float4 positionCS         : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
// Used in Standard (Simple Lighting) shader
Varyings LitPassVertexSimple(Attributes input)
// Used for StandardSimpleLighting shader
half4 LitPassFragmentSimple(Varyings input) : SV_Target
```



### SimpleLitGBufferPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
// keep this file in sync with LitForwardPass.hlsl
struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float2 staticLightmapUV   : TEXCOORD1;
    float2 dynamicLightmapUV  : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float2 uv         : TEXCOORD0;
    float3 posWS      : TEXCOORD1;// xyz: posWS
    #ifdef _NORMALMAP
	half4 normal      : TEXCOORD2;// xyz: normal, w: viewDir.x
	half4 tangent     : TEXCOORD3;// xyz: tangent, w: viewDir.y
	half4 bitangent   : TEXCOORD4;// xyz: bitangent, w: viewDir.z
    #else
	half3  normal     : TEXCOORD2;
    #endif
    #ifdef _ADDITIONAL_LIGHTS_VERTEX
	half3 vertexLighting : TEXCOORD5; // xyz: vertex light
    #endif
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	float4 shadowCoord   : TEXCOORD6;
    #endif
    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 7);
	#ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD8; // Dynamic lightmap UVs
	#endif
    float4 positionCS               : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
// Used in Standard (Simple Lighting) shader
Varyings LitPassVertexSimple(Attributes input)
// Used for StandardSimpleLighting shader
FragmentOutput LitPassFragmentSimple(Varyings input)
```



### SimpleLitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
CBUFFER_START(UnityPerMaterial)
    float4 _BaseMap_ST;
    half4 _BaseColor;
    half4 _SpecColor;
    half4 _EmissionColor;
    half _Cutoff;
    half _Surface;
CBUFFER_END
#ifdef UNITY_DOTS_INSTANCING_ENABLED//----------------
UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
	UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
	UNITY_DOTS_INSTANCED_PROP(float4, _SpecColor)
	UNITY_DOTS_INSTANCED_PROP(float4, _EmissionColor)
	UNITY_DOTS_INSTANCED_PROP(float , _Cutoff)
	UNITY_DOTS_INSTANCED_PROP(float , _Surface)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)
#define _BaseColor     UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_BaseColor)
#define _SpecColor     UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_SpecColor)
#define _EmissionColor UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_EmissionColor)
#define _Cutoff        UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Cutoff)
#define _Surface       UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Surface)
#endif//-----------------------------
TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
half4 SampleSpecularSmoothness(float2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
inline void InitializeSimpleLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
```



### SimpleLitMetaPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UniversalMetaPass.hlsl"

half4 UniversalFragmentMetaSimple(Varyings input) : SV_Target
{
    float2 uv = input.uv;
    MetaInput metaInput;
    metaInput.Albedo = _BaseColor.rgb * SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).rgb;
    metaInput.Emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

    return UniversalFragmentMeta(input, metaInput);
}
```



### UnlitDepthNormalsPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
struct Attributes
{
    float3 normal       : NORMAL;
    float4 positionOS   : POSITION;
    float4 tangentOS    : TANGENT;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float4 positionCS   : SV_POSITION;
    float3 normalWS     : TEXCOORD1;

    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
Varyings DepthNormalsVertex(Attributes input)
float4 DepthNormalsFragment(Varyings input) : SV_TARGET
```



### UnlitForwardPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    #if defined(DEBUG_DISPLAY)
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float2 uv : TEXCOORD0;
    float fogCoord : TEXCOORD1;
    float4 positionCS : SV_POSITION;
    #if defined(DEBUG_DISPLAY)
    float3 positionWS : TEXCOORD2;
    float3 normalWS : TEXCOORD3;
    float3 viewDirWS : TEXCOORD4;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, out InputData inputData)
Varyings UnlitPassVertex(Attributes input)
half4 UnlitPassFragment(Varyings input) : SV_Target
```



### UnlitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
CBUFFER_START(UnityPerMaterial)
    float4 _BaseMap_ST;
    half4 _BaseColor;
    half _Cutoff;
    half _Surface;
CBUFFER_END
#ifdef UNITY_DOTS_INSTANCING_ENABLED
UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
    UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DOTS_INSTANCED_PROP(float , _Cutoff)
    UNITY_DOTS_INSTANCED_PROP(float , _Surface)
UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)
#define _BaseColor UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata_BaseColor)
#define _Cutoff    UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Cutoff)
#define _Surface   UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata_Surface)
#endif
```



### UnlitMetaPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UniversalMetaPass.hlsl"

half4 UniversalFragmentMetaUnlit(Varyings input) : SV_Target
{
    MetaInput metaInput = (MetaInput)0;
    metaInput.Albedo = _BaseColor.rgb * SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).rgb;

    return UniversalFragmentMeta(input, metaInput);
}
```



## 根目录（shader）
### BakedLit

```c
Properties
{
    [MainTexture] _BaseMap("Texture", 2D) = "white" {}
    [MainColor]   _BaseColor("Color", Color) = (1, 1, 1, 1)
    _Cutoff("AlphaCutout", Range(0.0, 1.0)) = 0.5
    _BumpMap("Normal Map", 2D) = "bump" {}

    // BlendMode
    _Surface("__surface", Float) = 0.0
    _Blend("__mode", Float) = 0.0
    _Cull("__cull", Float) = 2.0
    [ToggleUI] _AlphaClip("__clip", Float) = 0.0
    [HideInInspector] _BlendOp("__blendop", Float) = 0.0
    [HideInInspector] _SrcBlend("__src", Float) = 1.0
    [HideInInspector] _DstBlend("__dst", Float) = 0.0
    [HideInInspector] _ZWrite("__zw", Float) = 1.0

    // Editmode props
    _QueueOffset("Queue offset", Float) = 0.0

    [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
    Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "ShaderModel"="4.5"}
    LOD 100
    Blend [_SrcBlend][_DstBlend]
    ZWrite [_ZWrite]
    Cull [_Cull]
    Pass
    {
        Name "BakedLit"
        Tags{ "LightMode" = "UniversalForwardOnly" }
        HLSLPROGRAM
        //...
        #pragma vertex BakedLitForwardPassVertex
        #pragma fragment BakedLitForwardPassFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitForwardPass.hlsl"
        ENDHLSL
    }
    Pass
    {
        Tags{"LightMode" = "DepthOnly"}
        ZWrite On
        ColorMask 0
        HLSLPROGRAM
        //...
        #pragma vertex DepthOnlyVertex
        #pragma fragment DepthOnlyFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
        ENDHLSL
    }
    // This pass is used when drawing to a _CameraNormalsTexture texture with the forward renderer or the depthNormal prepass with the deferred renderer.
    Pass
    {
        Name "DepthNormalsOnly"
        Tags{"LightMode" = "DepthNormalsOnly"}
        ZWrite On
        Cull[_Cull]
        HLSLPROGRAM
        //...
        #pragma vertex DepthNormalsVertex
        #pragma fragment DepthNormalsFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitDepthNormalsPass.hlsl"
        ENDHLSL
    }
    // Same as DepthNormals pass, but used for deferred renderer and forwardOnly materials.
    Pass
    {
        Name "DepthNormalsOnly"
        Tags{"LightMode" = "DepthNormalsOnly"}
        ZWrite On
        Cull[_Cull]
        HLSLPROGRAM
        //...
        #pragma vertex DepthNormalsVertex
        #pragma fragment DepthNormalsFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
        ENDHLSL
    }
    // This pass it not used during regular rendering, only for lightmap baking.
    Pass
    {
        Name "Meta"
        Tags{"LightMode" = "Meta"}
        Cull Off
        HLSLPROGRAM
        //...
        #pragma vertex UniversalVertexMeta
        #pragma fragment UniversalFragmentMetaUnlit
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitMetaPass.hlsl"
        ENDHLSL
    }
    Pass
    {
        Name "Universal2D"
        Tags{ "LightMode" = "Universal2D" }
        Blend[_SrcBlend][_DstBlend]
        ZWrite[_ZWrite]
        Cull[_Cull]
        HLSLPROGRAM
        //...
        #pragma vertex vert
        #pragma fragment frag
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
        ENDHLSL
    }
}
SubShader
{
    Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "ShaderModel"="2.0"}
    LOD 100
    Blend [_SrcBlend][_DstBlend]
    ZWrite [_ZWrite]
    Cull [_Cull]
    Pass
    {
        Name "BakedLit"
        Tags{ "LightMode" = "UniversalForwardOnly" }
        HLSLPROGRAM
        //...
        #pragma vertex BakedLitForwardPassVertex
        #pragma fragment BakedLitForwardPassFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitForwardPass.hlsl"
        ENDHLSL
    }
    Pass
    {
        Tags{"LightMode" = "DepthOnly"}
        ZWrite On
        ColorMask 0
        HLSLPROGRAM
        //...
        #pragma vertex DepthOnlyVertex
        #pragma fragment DepthOnlyFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
        ENDHLSL
    }
    // This pass is used when drawing to a _CameraNormalsTexture texture
    Pass
    {
        Name "DepthNormals"
        Tags{"LightMode" = "DepthNormals"}
        ZWrite On
        Cull[_Cull]
        HLSLPROGRAM
        //...
        #pragma vertex DepthNormalsVertex
        #pragma fragment DepthNormalsFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitDepthNormalsPass.hlsl"
        ENDHLSL
    }
    // This pass it not used during regular rendering, only for lightmap baking.
    Pass
    {
        Name "Meta"
        Tags{"LightMode" = "Meta"}
        Cull Off
        HLSLPROGRAM
        //...
        #pragma vertex UniversalVertexMeta
        #pragma fragment UniversalFragmentMetaUnlit
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitMetaPass.hlsl"
        ENDHLSL
    }
    Pass
    {
        Name "Universal2D"
        Tags{ "LightMode" = "Universal2D" }
        Blend[_SrcBlend][_DstBlend]
        ZWrite[_ZWrite]
        Cull[_Cull]
        HLSLPROGRAM
       	//...
        #pragma vertex vert
        #pragma fragment frag
        #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
        ENDHLSL
    }
}
```



### CameraMotionVectors

```c
SubShader
{
	Pass
	{
		Cull Off
		ZWrite On
		ZTest Always
		HLSLPROGRAM
		#pragma exclude_renderers d3d11_9x gles
		#pragma target 3.5
		#pragma vertex vert
		#pragma fragment frag
		// -------------------------------------
		// Includes
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
		#if defined(USING_STEREO_MATRICES)
		float4x4 _PrevViewProjMStereo[2];
		#define _PrevViewProjM _PrevViewProjMStereo[unity_StereoEyeIndex]
		#else
		#define  _PrevViewProjM _PrevViewProjMatrix
		#endif
		// -------------------------------------
		// Structs
		struct Attributes
		{
			uint vertexID   : SV_VertexID;
		};
		struct Varyings
		{
			float4 position : SV_POSITION;
		};
		// -------------------------------------
		// Vertex
		Varyings vert(Attributes input)
		{
			Varyings output;
			output.position = GetFullScreenTriangleVertexPosition(input.vertexID);
			return output;
		}
		// -------------------------------------
		// Fragment
		half4 frag(Varyings input, out float outDepth : SV_Depth) : SV_Target
        {
			// Calculate PositionInputs
			half depth = LoadSceneDepth(input.position.xy).x;
			outDepth = depth;
			half2 screenSize = _ScreenSize.zw;
			PositionInputs positionInputs = GetPositionInput(input.position.xy, screenSize, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
			// Calculate positions
			float4 previousPositionVP = mul(_PrevViewProjM, float4(positionInputs.positionWS, 1.0));
			float4 positionVP = mul(UNITY_MATRIX_VP, float4(positionInputs.positionWS, 1.0));
			previousPositionVP.xy *= rcp(previousPositionVP.w);
			positionVP.xy *= rcp(positionVP.w);
			// Calculate velocity
			float2 velocity = (positionVP.xy - previousPositionVP.xy);
			#if UNITY_UV_STARTS_AT_TOP
				velocity.y = -velocity.y;
			#endif
			// Convert velocity from Clip space (-1..1) to NDC 0..1 space
			// Note it doesn't mean we don't have negative value, we store negative or positive offset in NDC space.
			// Note: ((positionVP * 0.5 + 0.5) - (previousPositionVP * 0.5 + 0.5)) = (velocity * 0.5)
			return half4(velocity.xy * 0.5, 0, 0);
		}
		ENDHLSL
	}
}

```



### ComplexLit

```c
// Complex Lit is superset of Lit, but provides
// advanced material properties and is always forward rendered.
// It also has higher hardware and shader model requirements.
Properties
{
	// Specular vs Metallic workflow
	_WorkflowMode("WorkflowMode", Float) = 1.0
	[MainTexture] _BaseMap("Albedo", 2D) = "white" {}
	[MainColor] _BaseColor("Color", Color) = (1,1,1,1)
	_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
	_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
	_SmoothnessTextureChannel("Smoothness texture channel", Float) = 0
	_Metallic("Metallic", Range(0.0, 1.0)) = 0.0
	_MetallicGlossMap("Metallic", 2D) = "white" {}
	_SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
	_SpecGlossMap("Specular", 2D) = "white" {}
	[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
	[ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0
	_BumpScale("Scale", Float) = 1.0
	_BumpMap("Normal Map", 2D) = "bump" {}
	_Parallax("Scale", Range(0.005, 0.08)) = 0.005
	_ParallaxMap("Height Map", 2D) = "black" {}
	_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
	_OcclusionMap("Occlusion", 2D) = "white" {}
	[HDR] _EmissionColor("Color", Color) = (0,0,0)
	_EmissionMap("Emission", 2D) = "white" {}
	_DetailMask("Detail Mask", 2D) = "white" {}
	_DetailAlbedoMapScale("Scale", Range(0.0, 2.0)) = 1.0
	_DetailAlbedoMap("Detail Albedo x2", 2D) = "linearGrey" {}
	_DetailNormalMapScale("Scale", Range(0.0, 2.0)) = 1.0
	[Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}
	[ToggleUI] _ClearCoat("Clear Coat", Float) = 0.0
	_ClearCoatMap("Clear Coat Map", 2D) = "white" {}
	_ClearCoatMask("Clear Coat Mask", Range(0.0, 1.0)) = 0.0
	_ClearCoatSmoothness("Clear Coat Smoothness", Range(0.0, 1.0)) = 1.0
	// Blending state
	_Surface("__surface", Float) = 0.0
	_Blend("__mode", Float) = 0.0
	_Cull("__cull", Float) = 2.0
	[ToggleUI] _AlphaClip("__clip", Float) = 0.0
	[HideInInspector] _BlendOp("__blendop", Float) = 0.0
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
	[HideInInspector] _ZWrite("__zw", Float) = 1.0
	[ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
	// Editmode props
	_QueueOffset("Queue offset", Float) = 0.0
	[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
	[HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
	[HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
// Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings  this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this  material work with both Universal Render Pipeline and Builtin Unity Pipeline
	Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "ComplexLit" "IgnoreProjector" = "True" "ShaderModel"="4.5"}
	LOD 300
	// ------------------------------------------------------------------
	// Forward only pass.
	// Acts also as an opaque forward fallback for deferred rendering.
	Pass
	{
		// Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Render Pipeline
		Name "ForwardLit"
		Tags{"LightMode" = "UniversalForwardOnly"}
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local_fragment _OCCLUSIONMAP
		#pragma shader_feature_local_fragment _ _CLEARCOAT _CLEARCOATMAP
		#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#pragma vertex LitPassVertex
		#pragma fragment LitPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ZTest LEqual
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture with the forward renderer or the depthNormal prepass with the deferred renderer.
	Pass
	{
		Name "DepthNormalsOnly"
		Tags{"LightMode" = "DepthNormalsOnly"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT // forward-only variant
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
		ENDHLSL
	}
		// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaLit
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature EDITOR_VISUALIZATION
		#pragma shader_feature_local_fragment _SPECGLOSSMAP
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Universal2D"
		Tags{ "LightMode" = "Universal2D" }
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex vert
		#pragma fragment frag
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
		ENDHLSL
	}
}
SubShader
{
	// Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
	// this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
	// material work with both Universal Render Pipeline and Builtin Unity Pipeline
	Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel"="2.0"}
	LOD 300
	Pass
	{
		// Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Render Pipeline
		Name "ForwardLit"
		Tags{"LightMode" = "UniversalForwardOnly"}
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local_fragment _OCCLUSIONMAP
		#pragma shader_feature_local_fragment _ _CLEARCOAT _CLEARCOATMAP
		#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		#pragma vertex LitPassVertex
		#pragma fragment LitPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ZTest LEqual
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaLit
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SPECGLOSSMAP
		#pragma shader_feature EDITOR_VISUALIZATION
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Universal2D"
		Tags{ "LightMode" = "Universal2D" }
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
		ENDHLSL
	}
}
//////////////////////////////////////////////////////
FallBack "Hidden/Universal Render Pipeline/Lit"
FallBack "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LitShader"

```



### Lit

```c
Properties
{
	// Specular vs Metallic workflow
	_WorkflowMode("WorkflowMode", Float) = 1.0
	[MainTexture] _BaseMap("Albedo", 2D) = "white" {}
	[MainColor] _BaseColor("Color", Color) = (1,1,1,1)
	_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
	_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
	_SmoothnessTextureChannel("Smoothness texture channel", Float) = 0
	_Metallic("Metallic", Range(0.0, 1.0)) = 0.0
	_MetallicGlossMap("Metallic", 2D) = "white" {}
	_SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
	_SpecGlossMap("Specular", 2D) = "white" {}
	[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
	[ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0
	_BumpScale("Scale", Float) = 1.0
	_BumpMap("Normal Map", 2D) = "bump" {}
	_Parallax("Scale", Range(0.005, 0.08)) = 0.005
	_ParallaxMap("Height Map", 2D) = "black" {}
	_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
	_OcclusionMap("Occlusion", 2D) = "white" {}
	[HDR] _EmissionColor("Color", Color) = (0,0,0)
	_EmissionMap("Emission", 2D) = "white" {}
	_DetailMask("Detail Mask", 2D) = "white" {}
	_DetailAlbedoMapScale("Scale", Range(0.0, 2.0)) = 1.0
	_DetailAlbedoMap("Detail Albedo x2", 2D) = "linearGrey" {}
	_DetailNormalMapScale("Scale", Range(0.0, 2.0)) = 1.0
	[Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}
	// SRP batching compatibility for Clear Coat (Not used in Lit)
	[HideInInspector] _ClearCoatMask("_ClearCoatMask", Float) = 0.0
	[HideInInspector] _ClearCoatSmoothness("_ClearCoatSmoothness", Float) = 0.0
	// Blending state
	_Surface("__surface", Float) = 0.0
	_Blend("__blend", Float) = 0.0
	_Cull("__cull", Float) = 2.0
	[ToggleUI] _AlphaClip("__clip", Float) = 0.0
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
	[HideInInspector] _ZWrite("__zw", Float) = 1.0
	[ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
	// Editmode props
	_QueueOffset("Queue offset", Float) = 0.0
	// ObsoleteProperties
	[HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
	[HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
	[HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
	[HideInInspector] _Glossiness("Smoothness", Float) = 0.0
	[HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0
	[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
	[HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
	[HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
	// Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
	// this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
	// material work with both Universal Render Pipeline and Builtin Unity Pipeline
	Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel"="4.5"}
	LOD 300
	// ------------------------------------------------------------------
	//  Forward pass. Shades all light in a single pass. GI + emission + Fog
	Pass
	{
		// Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Render Pipeline
		Name "ForwardLit"
		Tags{"LightMode" = "UniversalForward"}
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local_fragment _OCCLUSIONMAP
		#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#pragma vertex LitPassVertex
		#pragma fragment LitPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ZTest LEqual
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		// Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Render Pipeline
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		ZWrite[_ZWrite]
		ZTest LEqual
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		//#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local_fragment _OCCLUSIONMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		//#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#pragma vertex LitGBufferPassVertex
		#pragma fragment LitGBufferPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitGBufferPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaLit
		#pragma shader_feature EDITOR_VISUALIZATION
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SPECGLOSSMAP
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Universal2D"
		Tags{ "LightMode" = "Universal2D" }
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex vert
		#pragma fragment frag
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
		ENDHLSL
	}
}
SubShader
{
	// Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
	// this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
	// material work with both Universal Render Pipeline and Builtin Unity Pipeline
	Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel"="2.0"}
	LOD 300
	// ------------------------------------------------------------------
	//  Forward pass. Shades all light in a single pass. GI + emission + Fog
	Pass
	{
		// Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Render Pipeline
		Name "ForwardLit"
		Tags{"LightMode" = "UniversalForward"}
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local_fragment _OCCLUSIONMAP
		#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma vertex LitPassVertex
		#pragma fragment LitPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ZTest LEqual
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _PARALLAXMAP
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaLit
		#pragma shader_feature EDITOR_VISUALIZATION
		#pragma shader_feature_local_fragment _SPECULAR_SETUP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
		#pragma shader_feature_local_fragment _SPECGLOSSMAP
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Universal2D"
		Tags{ "LightMode" = "Universal2D" }
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LitShader"

```



### ObjectMotionVectors

```c
SubShader
{
	Pass
	{
		// Lightmode tag required setup motion vector parameters by C++ (legacy Unity)
		Tags{ "LightMode" = "MotionVectors" }
		HLSLPROGRAM
		// Required to compile gles 2.0 with standard srp library
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x
		#pragma target 3.0
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma vertex vert
		#pragma fragment frag
		// -------------------------------------
		// Includes
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#if defined(USING_STEREO_MATRICES)
		float4x4 _PrevViewProjMStereo[2];
		#define _PrevViewProjM _PrevViewProjMStereo[unity_StereoEyeIndex]
		#else
		#define  _PrevViewProjM _PrevViewProjMatrix
		#endif
		// -------------------------------------
		// Structs
		struct Attributes
		{
			float4 position             : POSITION;
			float3 positionOld          : TEXCOORD4;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float4 positionCS           : SV_POSITION;
			float4 positionVP           : TEXCOORD0;
			float4 previousPositionVP   : TEXCOORD1;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};
		// -------------------------------------
		// Vertex
		Varyings vert(Attributes input)
		{
			Varyings output = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_TRANSFER_INSTANCE_ID(input, output);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
			output.positionCS = TransformObjectToHClip(input.position.xyz);
			// this works around an issue with dynamic batching
			// potentially remove in 5.4 when we use instancing
			#if defined(UNITY_REVERSED_Z)
				output.positionCS.z -= unity_MotionVectorsParams.z * output.positionCS.w;
			#else
				output.positionCS.z += unity_MotionVectorsParams.z * output.positionCS.w;
			#endif
			output.positionVP = mul(UNITY_MATRIX_VP, mul(UNITY_MATRIX_M, input.position));
			const float4 prevPos = (unity_MotionVectorsParams.x == 1) ? float4(input.positionOld, 1) : input.position;
			output.previousPositionVP = mul(_PrevViewProjM, mul(unity_MatrixPreviousM, prevPos));
			return output;
		}
		// -------------------------------------
		// Fragment
		half4 frag(Varyings input) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
			// Note: unity_MotionVectorsParams.y is 0 is forceNoMotion is enabled
			bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
			if (forceNoMotion)
			{
				return float4(0.0, 0.0, 0.0, 0.0);
			}
			// Calculate positions
			float4 posVP = input.positionVP;
			float4 prevPosVP = input.previousPositionVP;
			posVP.xy *= rcp(posVP.w);
			prevPosVP.xy *= rcp(prevPosVP.w);
			// Calculate velocity
			float2 velocity = (posVP.xy - prevPosVP.xy);
			#if UNITY_UV_STARTS_AT_TOP
				velocity.y = -velocity.y;
			#endif
			// Convert from Clip space (-1..1) to NDC 0..1 space.
			// Note it doesn't mean we don't have negative value, we store negative or positive offset in NDC space.
			// Note: ((positionCS * 0.5 + 0.5) - (previousPositionCS * 0.5 + 0.5)) = (velocity * 0.5)
			return float4(velocity.xy * 0.5, 0, 0);
		}
		ENDHLSL
	}
}

```



### SimpleLit

```c
// Shader targeted for low end devices. Single Pass Forward Rendering.
// Keep properties of StandardSpecular shader for upgrade reasons.
Properties
{
	[MainTexture] _BaseMap("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}
	[MainColor]   _BaseColor("Base Color", Color) = (1, 1, 1, 1)
	_Cutoff("Alpha Clipping", Range(0.0, 1.0)) = 0.5
	_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
	_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
	_SpecGlossMap("Specular Map", 2D) = "white" {}
	_SmoothnessSource("Smoothness Source", Float) = 0.0
	_SpecularHighlights("Specular Highlights", Float) = 1.0
	[HideInInspector] _BumpScale("Scale", Float) = 1.0
	[NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
	[HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
	[NoScaleOffset]_EmissionMap("Emission Map", 2D) = "white" {}
	// Blending state
	_Surface("__surface", Float) = 0.0
	_Blend("__blend", Float) = 0.0
	_Cull("__cull", Float) = 2.0
	[ToggleUI] _AlphaClip("__clip", Float) = 0.0
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
	[HideInInspector] _ZWrite("__zw", Float) = 1.0
	[ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
	// Editmode props
	_QueueOffset("Queue offset", Float) = 0.0
	// ObsoleteProperties
	[HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
	[HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
	[HideInInspector] _Shininess("Smoothness", Float) = 0.0
	[HideInInspector] _GlossinessSource("GlossinessSource", Float) = 0.0
	[HideInInspector] _SpecSource("SpecularHighlights", Float) = 0.0
	[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
	[HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
	[HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit" "IgnoreProjector" = "True" "ShaderModel"="4.5"}
	LOD 300
	Pass
	{
		Name "ForwardLit"
		Tags { "LightMode" = "UniversalForward" }
		// Use same blending / depth states as Standard shader
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#pragma vertex LitPassVertexSimple
		#pragma fragment LitPassFragmentSimple
		#define BUMP_SCALE_NOT_SUPPORTED 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ZTest LEqual
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		ZWrite[_ZWrite]
		ZTest LEqual
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		//#pragma shader_feature _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		//#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#pragma vertex LitPassVertexSimple
		#pragma fragment LitPassFragmentSimple
		#define BUMP_SCALE_NOT_SUPPORTED 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitGBufferPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{ "LightMode" = "Meta" }
		Cull Off
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaSimple
		#pragma shader_feature EDITOR_VISUALIZATION
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _SPECGLOSSMAP
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitMetaPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Universal2D"
		Tags{ "LightMode" = "Universal2D" }
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex vert
		#pragma fragment frag
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
		ENDHLSL
	}
}
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit" "IgnoreProjector" = "True" "ShaderModel"="2.0"}
	LOD 300
	Pass
	{
		Name "ForwardLit"
		Tags { "LightMode" = "UniversalForward" }
		// Use same blending / depth states as Standard shader
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma vertex LitPassVertexSimple
		#pragma fragment LitPassFragmentSimple
		#define BUMP_SCALE_NOT_SUPPORTED 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ZTest LEqual
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{ "LightMode" =  "Meta" }
		Cull Off
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaSimple
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _SPECGLOSSMAP
		#pragma shader_feature EDITOR_VISUALIZATION
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitMetaPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Universal2D"
		Tags{ "LightMode" = "Universal2D" }
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
		ENDHLSL
	}
}
Fallback  "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.SimpleLitShader"
```



### SpatialMappingOcclusion

```c
SubShader
{
	Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry-1" }
	LOD 100
	ZWrite On
	ZTest LEqual
	Colormask 0
	Cull Off
	Pass
	{
		Name "Spatial Mapping Occlusion"
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#include "UnlitInput.hlsl"
		struct Attributes
		{
			float4 positionOS       : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float4 vertex  : SV_POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};
		Varyings vert(Attributes input)
		{
			Varyings output = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_TRANSFER_INSTANCE_ID(input, output);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
			VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
			output.vertex = vertexInput.positionCS;
			return output;
		}
		half4 frag(Varyings input) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
			return half4(0,0,0,0);
		}
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"

```



### SpatialMappingWireframe

```c
Properties
{
	_WireThickness ("Wire Thickness", RANGE(0, 800)) = 100
}
SubShader
{
	Tags {"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
	LOD 100
	Pass
	{
		Name "Spatial Mapping Wireframe"
		// Wireframe shader based on the the following
		// http://developer.download.nvidia.com/SDK/10/direct3d/Source/SolidWireframe/Doc/SolidWireframe.pdf
		HLSLPROGRAM
		#pragma require geometry
		#pragma vertex vert
		#pragma geometry geom
		#pragma fragment frag
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#include "UnlitInput.hlsl"
		float _WireThickness;
		struct Attributes
		{
			float4 positionOS       : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct v2g
		{
			float4 projectionSpaceVertex : SV_POSITION;
			float4 worldSpacePosition : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};
		v2g vert(Attributes input)
		{
			v2g output = (v2g)0;
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
			VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
			output.projectionSpaceVertex = vertexInput.positionCS;
			output.worldSpacePosition = mul(UNITY_MATRIX_M, input.positionOS);
			return output;
		}
		struct g2f
		{
			float4 projectionSpaceVertex : SV_POSITION;
			float4 worldSpacePosition : TEXCOORD0;
			float4 dist : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};
		[maxvertexcount(3)]
		void geom(triangle v2g i[3], inout TriangleStream<g2f> triangleStream)
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i[0]);
			float2 p0 = i[0].projectionSpaceVertex.xy / i[0].projectionSpaceVertex.w;
			float2 p1 = i[1].projectionSpaceVertex.xy / i[1].projectionSpaceVertex.w;
			float2 p2 = i[2].projectionSpaceVertex.xy / i[2].projectionSpaceVertex.w;
			float2 edge0 = p2 - p1;
			float2 edge1 = p2 - p0;
			float2 edge2 = p1 - p0;
			// To find the distance to the opposite edge, we take the
			// formula for finding the area of a triangle Area = Base/2 * Height,
			// and solve for the Height = (Area * 2)/Base.
			// We can get the area of a triangle by taking its cross product
			// divided by 2.  However we can avoid dividing our area/base by 2
			// since our cross product will already be double our area.
			float area = abs(edge1.x * edge2.y - edge1.y * edge2.x);
			float wireThickness = 800 - _WireThickness;
			g2f o;
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.worldSpacePosition = i[0].worldSpacePosition;
			o.projectionSpaceVertex = i[0].projectionSpaceVertex;
			o.dist.xyz = float3( (area / length(edge0)), 0.0, 0.0) * o.projectionSpaceVertex.w * wireThickness;
			o.dist.w = 1.0 / o.projectionSpaceVertex.w;
			triangleStream.Append(o);
			o.worldSpacePosition = i[1].worldSpacePosition;
			o.projectionSpaceVertex = i[1].projectionSpaceVertex;
			o.dist.xyz = float3(0.0, (area / length(edge1)), 0.0) * o.projectionSpaceVertex.w * wireThickness;
			o.dist.w = 1.0 / o.projectionSpaceVertex.w;
			triangleStream.Append(o);
			o.worldSpacePosition = i[2].worldSpacePosition;
			o.projectionSpaceVertex = i[2].projectionSpaceVertex;
			o.dist.xyz = float3(0.0, 0.0, (area / length(edge2))) * o.projectionSpaceVertex.w * wireThickness;
			o.dist.w = 1.0 / o.projectionSpaceVertex.w;
			triangleStream.Append(o);
		}
		half4 frag(g2f i) : SV_Target
		{
			float minDistanceToEdge = min(i.dist[0], min(i.dist[1], i.dist[2])) * i.dist[3];
			// Early out if we know we are not on a line segment.
			if(minDistanceToEdge > 0.9)
			{
				return half4(0,0,0,0);
			}
			// Smooth our line out
			float t = exp2(-2 * minDistanceToEdge * minDistanceToEdge);
			const half4 colors[11] = {
					half4(1.0, 1.0, 1.0, 1.0),  // White
					half4(1.0, 0.0, 0.0, 1.0),  // Red
					half4(0.0, 1.0, 0.0, 1.0),  // Green
					half4(0.0, 0.0, 1.0, 1.0),  // Blue
					half4(1.0, 1.0, 0.0, 1.0),  // Yellow
					half4(0.0, 1.0, 1.0, 1.0),  // Cyan/Aqua
					half4(1.0, 0.0, 1.0, 1.0),  // Magenta
					half4(0.5, 0.0, 0.0, 1.0),  // Maroon
					half4(0.0, 0.5, 0.5, 1.0),  // Teal
					half4(1.0, 0.65, 0.0, 1.0), // Orange
					half4(1.0, 1.0, 1.0, 1.0)   // White
				};
			float cameraToVertexDistance = length(_WorldSpaceCameraPos - i.worldSpacePosition.xyz);
			int index = clamp(floor(cameraToVertexDistance), 0, 10);
			half4 wireColor = colors[index];
			half4 finalColor = lerp(float4(0,0,0,1), wireColor, t);
			finalColor.a = t;
			return finalColor;
		}
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
```



### Unlit

```c
Properties
{
	[MainTexture] _BaseMap("Texture", 2D) = "white" {}
	[MainColor] _BaseColor("Color", Color) = (1, 1, 1, 1)
	_Cutoff("AlphaCutout", Range(0.0, 1.0)) = 0.5
	// BlendMode
	_Surface("__surface", Float) = 0.0
	_Blend("__mode", Float) = 0.0
	_Cull("__cull", Float) = 2.0
	[ToggleUI] _AlphaClip("__clip", Float) = 0.0
	[HideInInspector] _BlendOp("__blendop", Float) = 0.0
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
	[HideInInspector] _ZWrite("__zw", Float) = 1.0
	// Editmode props
	_QueueOffset("Queue offset", Float) = 0.0
	// ObsoleteProperties
	[HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
	[HideInInspector] _Color("Base Color", Color) = (0.5, 0.5, 0.5, 1)
	[HideInInspector] _SampleGI("SampleGI", float) = 0.0 // needed from bakedlit
}
SubShader
{
	Tags {"RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "ShaderModel"="4.5"}
	LOD 100
	Blend [_SrcBlend][_DstBlend]
	ZWrite [_ZWrite]
	Cull [_Cull]
	Pass
	{
		Name "Unlit"
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile _ DEBUG_DISPLAY
		#pragma vertex UnlitPassVertex
		#pragma fragment UnlitPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthNormalsOnly"
		Tags{"LightMode" = "DepthNormalsOnly"}
		ZWrite On
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT // forward-only variant
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaUnlit
		#pragma shader_feature EDITOR_VISUALIZATION
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitMetaPass.hlsl"
		ENDHLSL
	}
}
SubShader
{
	Tags {"RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "ShaderModel"="2.0"}
	LOD 100
	Blend [_SrcBlend][_DstBlend]
	ZWrite [_ZWrite]
	Cull [_Cull]
	Pass
	{
		Name "Unlit"
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile _ DEBUG_DISPLAY
		#pragma vertex UnlitPassVertex
		#pragma fragment UnlitPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitForwardPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthNormalsOnly"
		Tags{"LightMode" = "DepthNormalsOnly"}
		ZWrite On
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore
		#pragma target 2.0
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT // forward-only variant
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma only_renderers gles gles3 glcore d3d11
		#pragma target 2.0
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaUnlit
		#pragma shader_feature EDITOR_VISUALIZATION
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitMetaPass.hlsl"
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.UnlitShader"
```




## 2D/Include
### CombinedShapeLightShared

```c
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
half _HDREmulationScale;
half _UseSceneLighting;
half4 CombinedShapeLightShared(in SurfaceData2D surfaceData, in InputData2D inputData)
```



### InputData2D

```c
struct InputData2D
{
    float2 uv;
    half2 lightingUV;
    #if defined(DEBUG_DISPLAY)
    float3 positionWS;
    float4 texelSize;
    float4 mipInfo;
    uint mipCount;
    #endif
};
void InitializeInputData(float2 uv, half2 lightingUV, out InputData2D inputData)
{
    inputData = (InputData2D)0;
    inputData.uv = uv;
    inputData.lightingUV = lightingUV;
}
void InitializeInputData(float2 uv, out InputData2D inputData)
{
    InitializeInputData(uv, 0, inputData);
}
```



### LightingUtility

```c
//--------多情况定义，省略具体----
#define NORMALS_LIGHTING_COORDS(TEXCOORDA, TEXCOORDB)
#define TRANSFER_NORMALS_LIGHTING(output, worldSpacePos)
#define APPLY_NORMALS_LIGHTING(input, lightColor)
#define NORMALS_LIGHTING_VARIABLES
//------定义，省略具体---------
#define SHADOW_COORDS(TEXCOORDA)
#define SHADOW_VARIABLES
#define APPLY_SHADOWS(input, color, intensity)
#define TRANSFER_SHADOWS(output)
#define SHAPE_LIGHT(index)
```



### NormalsRenderingShared

```c
half4 NormalsRenderingShared(half4 color, half3 normalTS, half3 tangent, half3 bitangent, half3 normal)
{
    half4 normalColor;
    half3 normalWS = TransformTangentToWorld(normalTS, half3x3(tangent.xyz, bitangent.xyz, normal.xyz));
    normalColor.rgb = 0.5 * ((normalWS)+1);
    normalColor.a = color.a;  // used for blending
    return normalColor;
}
```



### ShadowProjectVertex

```c
struct Attributes
{
    float3 vertex : POSITION;
    float4 tangent: TANGENT;
    float4 extrusion : COLOR;
};
struct Varyings
{
    float4 vertex : SV_POSITION;
};
uniform float3 _LightPos;
uniform float4x4 _ShadowModelMatrix; // This is a custom model matrix without scaling
uniform float4x4 _ShadowModelInvMatrix;
uniform float3 _ShadowModelScale;    // This is the scale
uniform float  _ShadowRadius;
Varyings ProjectShadow(Attributes v)
```



### SpriteMaskShared

```c
// alpha below which a mask should discard a pixel, thereby preventing the stencil buffer from being marked with the Mask's presence
half  _Cutoff;

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
struct Attributes
{
    float4 positionOS : POSITION;
    half2  texcoord : TEXCOORD0;
};
struct Varyings
{
    float4 positionCS : SV_POSITION;
    half2  uv : TEXCOORD0;
};
Varyings MaskRenderingVertex(Attributes input)
half4 MaskRenderingFragment(Varyings input) : SV_Target
```



### SurfaceData2D

```c
struct SurfaceData2D
{
    half3 albedo;
    half alpha;
    half4 mask;
    half3 normalTS;
};
void InitializeSurfaceData(half3 albedo, half alpha, half4 mask, half3 normalTS, out SurfaceData2D surfaceData)
void InitializeSurfaceData(half3 albedo, half alpha, half4 mask, out SurfaceData2D surfaceData)
void InitializeSurfaceData(half3 albedo, half alpha, out SurfaceData2D surfaceData)
```



## 2D(Shader)
### Light2D-Point-Volumetric

```c
SubShader
{
	Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
	Pass
	{
		Blend One One
		ZWrite Off
		Cull Off
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_local USE_POINT_LIGHT_COOKIES __
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
		struct Attributes
		{
			float3 positionOS : POSITION;
			float2 texcoord   : TEXCOORD0;
		};
		struct Varyings
		{
			float4  positionCS: SV_POSITION;
			half2   uv        : TEXCOORD0;
			half2   lookupUV  : TEXCOORD2;// This is used for light relative direction
			SHADOW_COORDS(TEXCOORD5)
		};
		#if USE_POINT_LIGHT_COOKIES
		TEXTURE2D(_PointLightCookieTex);
		SAMPLER(sampler_PointLightCookieTex);
		#endif
		TEXTURE2D(_FalloffLookup);
		SAMPLER(sampler_FalloffLookup);
		half _FalloffIntensity;
		TEXTURE2D(_LightLookup);
		SAMPLER(sampler_LightLookup);
		half4 _LightLookup_TexelSize;
		half4   _LightColor;
		half    _VolumeOpacity;
		float4   _LightPosition;
		float4x4 _LightInvMatrix;
		float4x4 _LightNoRotInvMatrix;
		half    _LightZDistance;
		// 1-0 where 1 is the value at 0 degrees and 1 is the value at 180 degrees
		half    _OuterAngle;
		// 1-0 where 1 is the value at 0 degrees and 1 is the value at 180 degrees
		half    _InnerAngleMult;
		// 1-0 where 1 is the value at the center and 0 is the value at the outer radius
		half    _InnerRadiusMult;
		half    _InverseHDREmulationScale;
		half    _IsFullSpotlight;
		SHADOW_VARIABLES
		Varyings vert(Attributes input)
		{
			Varyings output = (Varyings)0;
			output.positionCS = TransformObjectToHClip(input.positionOS);
			output.uv = input.texcoord;
			float4 worldSpacePos;
			worldSpacePos.xyz = TransformObjectToWorld(input.positionOS);
			worldSpacePos.w = 1;
			float4 lightSpacePos = mul(_LightInvMatrix, worldSpacePos);
			float4 lightSpaceNoRotPos = mul(_LightNoRotInvMatrix, worldSpacePos);
			float halfTexelOffset = 0.5 * _LightLookup_TexelSize.x;
			output.lookupUV = 0.5 * (lightSpacePos.xy + 1) + halfTexelOffset;
			TRANSFER_SHADOWS(output)
			return output;
		}
		half4 frag(Varyings input) : SV_Target
		{
			half4 lookupValue = SAMPLE_TEXTURE2D(_LightLookup, sampler_LightLookup, input.lookupUV);  // r = distance, g = angle, b = x direction, a = y direction
			// Inner Radius
			half attenuation = saturate(_InnerRadiusMult * lookupValue.r);   // This is the code to take care of our inner radius
			// Spotlight
			half  spotAttenuation = saturate((_OuterAngle - lookupValue.g + _IsFullSpotlight) * _InnerAngleMult);
			attenuation = attenuation * spotAttenuation;
			half2 mappedUV;
			mappedUV.x = attenuation;
			mappedUV.y = _FalloffIntensity;
			attenuation = SAMPLE_TEXTURE2D(_FalloffLookup, sampler_FalloffLookup, mappedUV).r;
			#if USE_POINT_LIGHT_COOKIES
			half4 cookieColor = SAMPLE_TEXTURE2D(_PointLightCookieTex, sampler_PointLightCookieTex, input.lookupUV);
			half4 lightColor = cookieColor * _LightColor * attenuation;
			#else
			half4 lightColor = _LightColor * attenuation;
			#endif
			APPLY_SHADOWS(input, lightColor, _ShadowVolumeIntensity);
			return _VolumeOpacity * lightColor * _InverseHDREmulationScale;
		}
		ENDHLSL
	}
}
```



### Light2D-Point

```c
Properties
{
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
}
SubShader
{
	Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
	Pass
	{
		Blend [_SrcBlend][_DstBlend]
		ZWrite Off
		Cull Off
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_local USE_POINT_LIGHT_COOKIES __
		#pragma multi_compile_local LIGHT_QUALITY_FAST __
		#pragma multi_compile_local USE_NORMAL_MAP __
		#pragma multi_compile_local USE_ADDITIVE_BLENDING __
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
		struct Attributes
		{
			float3 positionOS : POSITION;
			float2 texcoord   : TEXCOORD0;
		};
		struct Varyings
		{
			float4  positionCS: SV_POSITION;
			half2   uv        : TEXCOORD0;
			half2   lookupUV  : TEXCOORD2;  // This is used for light relative direction
			NORMALS_LIGHTING_COORDS(TEXCOORD4, TEXCOORD5)
			SHADOW_COORDS(TEXCOORD6)
		};
		#if USE_POINT_LIGHT_COOKIES
		TEXTURE2D(_PointLightCookieTex);
		SAMPLER(sampler_PointLightCookieTex);
		#endif
		TEXTURE2D(_FalloffLookup);
		SAMPLER(sampler_FalloffLookup);
		half _FalloffIntensity;
		TEXTURE2D(_LightLookup);
		SAMPLER(sampler_LightLookup);
		half4 _LightLookup_TexelSize;
		NORMALS_LIGHTING_VARIABLES
		SHADOW_VARIABLES
		half4       _LightColor;
		float4x4    _LightInvMatrix;
		float4x4    _LightNoRotInvMatrix;
		// 1-0 where 1 is the value at 0 degrees and 1 is the value at 180 degrees
		half        _OuterAngle;
		// 1-0 where 1 is the value at 0 degrees and 1 is the value at 180 degrees
		half        _InnerAngleMult;
		// 1-0 where 1 is the value at the center and 0 is the value at the outer radius
		half        _InnerRadiusMult;
		half        _InverseHDREmulationScale;
		half        _IsFullSpotlight;
		Varyings vert(Attributes input)
		{
			Varyings output = (Varyings)0;
			output.positionCS = TransformObjectToHClip(input.positionOS);
			output.uv = input.texcoord;
			float4 worldSpacePos;
			worldSpacePos.xyz = TransformObjectToWorld(input.positionOS);
			worldSpacePos.w = 1;
			float4 lightSpacePos = mul(_LightInvMatrix, worldSpacePos);
			float4 lightSpaceNoRotPos = mul(_LightNoRotInvMatrix, worldSpacePos);
			float halfTexelOffset = 0.5 * _LightLookup_TexelSize.x;
			output.lookupUV = 0.5 * (lightSpacePos.xy + 1) + halfTexelOffset;
			TRANSFER_NORMALS_LIGHTING(output, worldSpacePos)
			TRANSFER_SHADOWS(output)
			return output;
		}
		half4 frag(Varyings input) : SV_Target
		{
			// r = distance, g = angle, b = x direction, a = y direction
			half4 lookupValue = SAMPLE_TEXTURE2D(_LightLookup, sampler_LightLookup, input.lookupUV);  
			// Inner Radius
			// This is the code to take care of our inner radius
			half attenuation = saturate(_InnerRadiusMult * lookupValue.r);
			// Spotlight
			half  spotAttenuation = saturate((_OuterAngle - lookupValue.g + _IsFullSpotlight) * _InnerAngleMult);
			attenuation = attenuation * spotAttenuation;
			half2 mappedUV;
			mappedUV.x = attenuation;
			mappedUV.y = _FalloffIntensity;
			attenuation = SAMPLE_TEXTURE2D(_FalloffLookup, sampler_FalloffLookup, mappedUV).r;
			#if USE_POINT_LIGHT_COOKIES
			half4 cookieColor = SAMPLE_TEXTURE2D(_PointLightCookieTex, sampler_PointLightCookieTex, input.lookupUV);
			half4 lightColor = cookieColor * _LightColor;
			#else
			half4 lightColor = _LightColor;
			#endif
			#if USE_ADDITIVE_BLENDING
			lightColor *= attenuation;
			#else
			lightColor.a = attenuation;
			#endif
			APPLY_NORMALS_LIGHTING(input, lightColor);
			APPLY_SHADOWS(input, lightColor, _ShadowIntensity);
			return lightColor * _InverseHDREmulationScale;
		}
		ENDHLSL
	}
}
```



### Light2D-Shape-Volumetric

```c
SubShader
{
	Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
	Pass
	{
		Blend SrcAlpha One
		ZWrite Off
		ZTest Off
		Cull Off
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_local SPRITE_LIGHT __
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
		struct Attributes
		{
			float3 positionOS   : POSITION;
			float4 color        : COLOR;
			half2  uv           : TEXCOORD0;
		};
		struct Varyings
		{
			float4  positionCS  : SV_POSITION;
			half4   color       : COLOR;
			half2   uv          : TEXCOORD0;
			SHADOW_COORDS(TEXCOORD1)
		};
		half4 _LightColor;
		half  _FalloffDistance;
		half  _VolumeOpacity;
		half  _InverseHDREmulationScale;
		#ifdef SPRITE_LIGHT
		// This can either be a sprite texture uv or a falloff texture
		TEXTURE2D(_CookieTex);
		SAMPLER(sampler_CookieTex);
		#else
		uniform half  _FalloffIntensity;
		TEXTURE2D(_FalloffLookup);
		SAMPLER(sampler_FalloffLookup);
		#endif
		SHADOW_VARIABLES
		Varyings vert(Attributes attributes)
		{
			Varyings o = (Varyings)0;
			float3 positionOS = attributes.positionOS;
			positionOS.x = positionOS.x + _FalloffDistance * attributes.color.r;
			positionOS.y = positionOS.y + _FalloffDistance * attributes.color.g;
			o.positionCS = TransformObjectToHClip(positionOS);
			o.color = _LightColor * _InverseHDREmulationScale;
			o.color.a = _LightColor.a * _VolumeOpacity;
			#ifdef SPRITE_LIGHT
			o.uv = attributes.uv;
			#else
			o.uv = float2(attributes.color.a, _FalloffIntensity);
			#endif
			TRANSFER_SHADOWS(o)
			return o;
		}
		half4 frag(Varyings i) : SV_Target
		{
			half4 color = i.color;
			#if SPRITE_LIGHT
			color *= SAMPLE_TEXTURE2D(_CookieTex, sampler_CookieTex, i.uv);
			#else
			color.a = i.color.a * SAMPLE_TEXTURE2D(_FalloffLookup, sampler_FalloffLookup, i.uv).r;
			#endif
			APPLY_SHADOWS(i, color, _ShadowVolumeIntensity);
			return color;
		}
		ENDHLSL
	}
}
```



### Light2D-Shape

```c
Properties
{
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
}
SubShader
{
	Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
	Pass
	{
		Blend[_SrcBlend][_DstBlend]
		ZWrite Off
		Cull Off
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_local SPRITE_LIGHT __
		#pragma multi_compile_local USE_NORMAL_MAP __
		#pragma multi_compile_local LIGHT_QUALITY_FAST __
		#pragma multi_compile_local USE_ADDITIVE_BLENDING __
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
		struct Attributes
		{
			float3 positionOS   : POSITION;
			float4 color        : COLOR;
			float2 uv           : TEXCOORD0;
		};
		struct Varyings
		{
			float4  positionCS  : SV_POSITION;
			half4   color       : COLOR;
			half2   uv          : TEXCOORD0;
			SHADOW_COORDS(TEXCOORD1)
			NORMALS_LIGHTING_COORDS(TEXCOORD2, TEXCOORD3)
		};
		half    _InverseHDREmulationScale;
		half4   _LightColor;
		half    _FalloffDistance;
		#ifdef SPRITE_LIGHT
		// This can either be a sprite texture uv or a falloff texture
		TEXTURE2D(_CookieTex);
		SAMPLER(sampler_CookieTex);
		#else
		half    _FalloffIntensity;
		TEXTURE2D(_FalloffLookup);
		SAMPLER(sampler_FalloffLookup);
		#endif
		NORMALS_LIGHTING_VARIABLES
		SHADOW_VARIABLES
		Varyings vert(Attributes attributes)
		{
			Varyings o = (Varyings)0;
			float3 positionOS = attributes.positionOS;
			positionOS.x = positionOS.x + _FalloffDistance * attributes.color.r;
			positionOS.y = positionOS.y + _FalloffDistance * attributes.color.g;
			o.positionCS = TransformObjectToHClip(positionOS);
			o.color = _LightColor * _InverseHDREmulationScale;
			o.color.a = attributes.color.a;
			#ifdef SPRITE_LIGHT
			o.uv = attributes.uv;
			#else
			o.uv = float2(o.color.a, _FalloffIntensity);
			#endif
			float4 worldSpacePos;
			worldSpacePos.xyz = TransformObjectToWorld(positionOS);
			worldSpacePos.w = 1;
			TRANSFER_NORMALS_LIGHTING(o, worldSpacePos)
			TRANSFER_SHADOWS(o)
			return o;
		}
		half4 frag(Varyings i) : SV_Target
		{
			half4 color = i.color;
		#if SPRITE_LIGHT
			half4 cookie = SAMPLE_TEXTURE2D(_CookieTex, sampler_CookieTex, i.uv);
			#if USE_ADDITIVE_BLENDING
			color *= cookie * cookie.a;
			#else
			color *= cookie;
			#endif
		#else
			#if USE_ADDITIVE_BLENDING
			color *= SAMPLE_TEXTURE2D(_FalloffLookup, sampler_FalloffLookup, i.uv).r;
			#else
			color.a = SAMPLE_TEXTURE2D(_FalloffLookup, sampler_FalloffLookup, i.uv).r;
			#endif
		#endif
			APPLY_NORMALS_LIGHTING(i, color);
			APPLY_SHADOWS(i, color, _ShadowIntensity);
			return color;
		}
		ENDHLSL
	}
}
```



### Shadow2D-Projected

```c
Properties
{
	[HideInInspector] _ShadowColorMask("__ShadowColorMask", Float) = 0
}
SubShader
{
	Tags { "RenderType"="Transparent" }
	Cull Off
	BlendOp Add
	ZWrite Off
	ZTest Always
	// This pass draws the projected shadow and sets the composite shadow bit.
	Pass
	{
		// Bit 0: Composite Shadow Bit, Bit 1: Global Shadow Bit
		Stencil
		{
			Ref         5
			ReadMask    4
			WriteMask   1
			Comp        NotEqual
			Pass        Replace
			Fail        Keep
		}
		ColorMask [_ShadowColorMask]
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/ShadowProjectVertex.hlsl"
		Varyings vert (Attributes v)
		{
			return ProjectShadow(v);
		}
		half4 frag (Varyings i) : SV_Target
		{
			return half4(1,1,1,1);
		}
		ENDHLSL
	}
	// Sets the global shadow bit, and clears the composite shadow bit
	Pass
	{
		// Bit 0: Composite Shadow Bit, Bit 1: Global Shadow Bit
		Stencil
		{
			Ref         3
			WriteMask   2
			ReadMask    1
			Comp        Equal
			Pass        Replace
			Fail        Keep
		}
		// We only want to change the stencil value in this pass
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/ShadowProjectVertex.hlsl"
		Varyings vert (Attributes v)
		{
			return ProjectShadow(v);
		}
		half4 frag (Varyings i) : SV_Target
		{
			return half4(1,1,1,1);
		}
		ENDHLSL
	}
}
```



### Shadow2D-Shadow-Sprite

```c
Properties
{
	_MainTex ("Texture", 2D) = "white" {}
	[HideInInspector] _ShadowColorMask("__ShadowColorMask", Int) = 1
}
SubShader
{
	Tags { "RenderType"="Transparent" }
	Cull Off
	BlendOp Add
	Blend One One, One One
	ZWrite Off
	ZTest Always
	Pass
	{
		//Bit 0: Composite Shadow Bit, Bit 1: Global Shadow Bit
		Stencil
		{
			Ref  0
			Comp Equal
			Pass Keep
			Fail Keep
		}
		ColorMask [_ShadowColorMask]
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		struct Attributes
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};
		struct Varyings
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};
		sampler2D _MainTex;
		float4    _MainTex_ST;
		Varyings vert (Attributes v)
		{
			Varyings o;
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}
		half4 frag(Varyings i) : SV_Target
		{
			half4 main = tex2D(_MainTex, i.uv);
			half color = main.a;
			return half4(color, color, color, color);
		}
		ENDHLSL
	}
}
```



### Shadow2D-Unshadow-Geometry

```c
SubShader
{
	Tags { "RenderType"="Transparent" }
	Cull Off
	ZWrite Off
	ZTest Always
	Pass
	{
		//Bit 0: Composite Shadow Bit, Bit 1: Global Shadow Bit, Bit2: Caster Mask Bit
		Stencil
		{
			Ref  4
			ReadMask  4
			WriteMask 4
			Comp NotEqual
			Pass Replace
			Fail Keep
		}
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		struct Attributes
		{
			float4 vertex : POSITION;
		};
		struct Varyings
		{
			float4 vertex : SV_POSITION;
		};
		Varyings vert (Attributes v)
		{
			Varyings o;
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			return o;
		}
		half4 frag(Varyings i) : SV_Target
		{
			return half4(0,0,0,0);
		}
		ENDHLSL
	}
	Pass
	{
		//Bit 0: Composite Shadow Bit, Bit 1: Global Shadow Bit, Bit2: Caster Mask Bit
		Stencil
		{
			Ref  4
			ReadMask  4
			WriteMask 4
			Comp Equal
			Pass Zero
			Fail Keep
		}
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		struct Attributes
		{
			float4 vertex : POSITION;
		};
		struct Varyings
		{
			float4 vertex : SV_POSITION;
		};
		Varyings vert(Attributes v)
		{
			Varyings o;
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			return o;
		}
		half4 frag(Varyings i) : SV_Target
		{
			return half4(0,0,0,0);
		}
		ENDHLSL
	}
}
```



### Shadow2D-Unshadow-Sprite

```c
Properties
{
	_MainTex ("Texture", 2D) = "white" {}
	[HideInInspector] _ShadowColorMask("__ShadowColorMask", Int) = 0
}
SubShader
{
	Tags { "RenderType"="Transparent" }
	Cull Off
	BlendOp Add
	Blend OneMinusSrcColor SrcColor, OneMinusSrcAlpha SrcAlpha
	ZWrite Off
	ZTest Always
	Pass
	{
		//Bit 0: Composite Shadow Bit, Bit 1: Global Shadow Bit
		Stencil
		{
			Ref  1
			Comp Equal
			Pass Keep
			Fail Keep
		}
		ColorMask [_ShadowColorMask]
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		struct Attributes
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};
		struct Varyings
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};
		sampler2D _MainTex;
		float4    _MainTex_ST;
		Varyings vert (Attributes v)
		{
			Varyings o;
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}
		half4 frag(Varyings i) : SV_Target
		{
			half4 main = tex2D(_MainTex, i.uv);
			half color = 1-main.a;
			return half4(color, color, color, color);
		}
		ENDHLSL
	}
}
```



### Sprite-Lit-Default

```c
Properties
{
	_MainTex("Diffuse", 2D) = "white" {}
	_MaskTex("Mask", 2D) = "white" {}
	_NormalMap("Normal Map", 2D) = "bump" {}
	// Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
	[HideInInspector] _Color("Tint", Color) = (1,1,1,1)
	[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
	[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
	[HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
	[HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
}
SubShader
{
	Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
	Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
	Cull Off
	ZWrite Off
	Pass
	{
		Tags { "LightMode" = "Universal2D" }
		HLSLPROGRAM
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#pragma vertex CombinedShapeLightVertex
		#pragma fragment CombinedShapeLightFragment
		#pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
		#pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
		#pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
		#pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
		#pragma multi_compile _ DEBUG_DISPLAY
		struct Attributes
		{
			float3 positionOS   : POSITION;
			float4 color        : COLOR;
			float2  uv          : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float4  positionCS  : SV_POSITION;
			half4   color       : COLOR;
			float2  uv          : TEXCOORD0;
			half2   lightingUV  : TEXCOORD1;
			#if defined(DEBUG_DISPLAY)
			float3  positionWS  : TEXCOORD2;
			#endif
			UNITY_VERTEX_OUTPUT_STEREO
		};
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		TEXTURE2D(_MaskTex);
		SAMPLER(sampler_MaskTex);
		half4 _MainTex_ST;
		float4 _Color;
		half4 _RendererColor;
		#if USE_SHAPE_LIGHT_TYPE_0
		SHAPE_LIGHT(0)
		#endif
		#if USE_SHAPE_LIGHT_TYPE_1
		SHAPE_LIGHT(1)
		#endif
		#if USE_SHAPE_LIGHT_TYPE_2
		SHAPE_LIGHT(2)
		#endif
		#if USE_SHAPE_LIGHT_TYPE_3
		SHAPE_LIGHT(3)
		#endif
		Varyings CombinedShapeLightVertex(Attributes v)
		{
			Varyings o = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.positionCS = TransformObjectToHClip(v.positionOS);
			#if defined(DEBUG_DISPLAY)
			o.positionWS = TransformObjectToWorld(v.positionOS);
			#endif
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.lightingUV = half2(ComputeScreenPos(o.positionCS / o.positionCS.w).xy);
			o.color = v.color * _Color * _RendererColor;
			return o;
		}
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"
		half4 CombinedShapeLightFragment(Varyings i) : SV_Target
		{
			const half4 main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			const half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
			SurfaceData2D surfaceData;
			InputData2D inputData;
			InitializeSurfaceData(main.rgb, main.a, mask, surfaceData);
			InitializeInputData(i.uv, i.lightingUV, inputData);
			return CombinedShapeLightShared(surfaceData, inputData);
		}
		ENDHLSL
	}
	Pass
	{
		Tags { "LightMode" = "NormalsRendering"}
		HLSLPROGRAM
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#pragma vertex NormalsRenderingVertex
		#pragma fragment NormalsRenderingFragment
		struct Attributes
		{
			float3 positionOS   : POSITION;
			float4 color        : COLOR;
			float2 uv           : TEXCOORD0;
			float4 tangent      : TANGENT;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float4  positionCS      : SV_POSITION;
			half4   color           : COLOR;
			float2  uv              : TEXCOORD0;
			half3   normalWS        : TEXCOORD1;
			half3   tangentWS       : TEXCOORD2;
			half3   bitangentWS     : TEXCOORD3;
			UNITY_VERTEX_OUTPUT_STEREO
		};
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		TEXTURE2D(_NormalMap);
		SAMPLER(sampler_NormalMap);
		half4 _NormalMap_ST;  // Is this the right way to do this?
		Varyings NormalsRenderingVertex(Attributes attributes)
		{
			Varyings o = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(attributes);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.positionCS = TransformObjectToHClip(attributes.positionOS);
			o.uv = TRANSFORM_TEX(attributes.uv, _NormalMap);
			o.color = attributes.color;
			o.normalWS = -GetViewForwardDir();
			o.tangentWS = TransformObjectToWorldDir(attributes.tangent.xyz);
			o.bitangentWS = cross(o.normalWS, o.tangentWS) * attributes.tangent.w;
			return o;
		}
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
		half4 NormalsRenderingFragment(Varyings i) : SV_Target
		{
			const half4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			const half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv));
			return NormalsRenderingShared(mainTex, normalTS, i.tangentWS.xyz, i.bitangentWS.xyz, i.normalWS.xyz);
		}
		ENDHLSL
	}
	Pass
	{
		Tags { "LightMode" = "UniversalForward" "Queue"="Transparent" "RenderType"="Transparent"}
		HLSLPROGRAM
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#pragma vertex UnlitVertex
		#pragma fragment UnlitFragment
		struct Attributes
		{
			float3 positionOS   : POSITION;
			float4 color        : COLOR;
			float2 uv           : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float4  positionCS      : SV_POSITION;
			float4  color           : COLOR;
			float2  uv              : TEXCOORD0;
			#if defined(DEBUG_DISPLAY)
			float3  positionWS  : TEXCOORD2;
			#endif
			UNITY_VERTEX_OUTPUT_STEREO
		};
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		float4 _MainTex_ST;
		float4 _Color;
		half4 _RendererColor;
		Varyings UnlitVertex(Attributes attributes)
		{
			Varyings o = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(attributes);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.positionCS = TransformObjectToHClip(attributes.positionOS);
			#if defined(DEBUG_DISPLAY)
			o.positionWS = TransformObjectToWorld(v.positionOS);
			#endif
			o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
			o.color = attributes.color * _Color * _RendererColor;
			return o;
		}
		float4 UnlitFragment(Varyings i) : SV_Target
		{
			float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			#if defined(DEBUG_DISPLAY)
			SurfaceData2D surfaceData;
			InputData2D inputData;
			half4 debugColor = 0;
			InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
			InitializeInputData(i.uv, inputData);
			SETUP_DEBUG_DATA_2D(inputData, i.positionWS);
			if(CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
			{
				return debugColor;
			}
			#endif
			return mainTex;
		}
		ENDHLSL
	}
}
Fallback "Sprites/Default"
```



### Sprite-Mask

```c
Properties
{
	[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
	[HideInInspector] _Cutoff ("Mask alpha cutoff", Range(0.0, 1.0)) = 0.0
}
HLSLINCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
ENDHLSL
SubShader
{
	Tags
	{
		"Queue"="Transparent"
		"IgnoreProjector"="True"
		"RenderType"="Transparent"
		"PreviewType"="Plane"
		"CanUseSpriteAtlas"="True"
		"RenderPipeline" = "UniversalPipeline"
	}
	Cull Off
	Lighting Off
	ZWrite Off
	Blend Off
	ColorMask 0
	Pass
	{
		Tags{ "LightMode" = "Universal2D" }
		HLSLPROGRAM
		#pragma vertex MaskRenderingVertex
		#pragma fragment MaskRenderingFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SpriteMaskShared.hlsl"
		ENDHLSL
	}
	Pass
	{
		Tags{ "LightMode" = "NormalsRendering" }
		HLSLPROGRAM
		#pragma vertex MaskRenderingVertex
		#pragma fragment MaskRenderingFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SpriteMaskShared.hlsl"
		ENDHLSL
	}
	Pass
	{
		Tags{ "LightMode" = "UniversalForward" }
		HLSLPROGRAM
		#pragma vertex MaskRenderingVertex
		#pragma fragment MaskRenderingFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SpriteMaskShared.hlsl"
		ENDHLSL
	}
}
```



### Sprite-Unlit-Default

```c
Properties
{
	_MainTex ("Sprite Texture", 2D) = "white" {}
	// Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
	[HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
	[HideInInspector] PixelSnap ("Pixel snap", Float) = 0
	[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
	[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
	[HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
	[HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
}
SubShader
{
	Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off
	ZWrite Off
	Pass
	{
		Tags { "LightMode" = "Universal2D" }
		HLSLPROGRAM
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#if defined(DEBUG_DISPLAY)
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
		#endif
		#pragma vertex UnlitVertex
		#pragma fragment UnlitFragment
		#pragma multi_compile _ DEBUG_DISPLAY
		struct Attributes
		{
			float3 positionOS   : POSITION;
			float4 color        : COLOR;
			float2 uv           : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float4  positionCS  : SV_POSITION;
			half4   color       : COLOR;
			float2  uv          : TEXCOORD0;
			#if defined(DEBUG_DISPLAY)
			float3  positionWS  : TEXCOORD2;
			#endif
			UNITY_VERTEX_OUTPUT_STEREO
		};
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		half4 _MainTex_ST;
		float4 _Color;
		half4 _RendererColor;
		Varyings UnlitVertex(Attributes v)
		{
			Varyings o = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.positionCS = TransformObjectToHClip(v.positionOS);
			#if defined(DEBUG_DISPLAY)
			o.positionWS = TransformObjectToWorld(v.positionOS);
			#endif
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.color = v.color * _Color * _RendererColor;
			return o;
		}
		half4 UnlitFragment(Varyings i) : SV_Target
		{
			float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			#if defined(DEBUG_DISPLAY)
			SurfaceData2D surfaceData;
			InputData2D inputData;
			half4 debugColor = 0;
			InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
			InitializeInputData(i.uv, inputData);
			SETUP_DEBUG_DATA_2D(inputData, i.positionWS);
			if(CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
			{
				return debugColor;
			}
			#endif
			return mainTex;
		}
		ENDHLSL
	}
	Pass
	{
		Tags { "LightMode" = "UniversalForward" "Queue"="Transparent" "RenderType"="Transparent"}
		HLSLPROGRAM
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#if defined(DEBUG_DISPLAY)
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
		#endif
		#pragma vertex UnlitVertex
		#pragma fragment UnlitFragment
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		struct Attributes
		{
			float3 positionOS   : POSITION;
			float4 color        : COLOR;
			float2 uv           : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float4  positionCS      : SV_POSITION;
			float4  color           : COLOR;
			float2  uv              : TEXCOORD0;
			#if defined(DEBUG_DISPLAY)
			float3  positionWS      : TEXCOORD2;
			#endif
			UNITY_VERTEX_OUTPUT_STEREO
		};
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		float4 _MainTex_ST;
		float4 _Color;
		half4 _RendererColor;
		Varyings UnlitVertex(Attributes attributes)
		{
			Varyings o = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(attributes);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.positionCS = TransformObjectToHClip(attributes.positionOS);
			#if defined(DEBUG_DISPLAY)
			o.positionWS = TransformObjectToWorld(attributes.positionOS);
			#endif
			o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
			o.color = attributes.color * _Color * _RendererColor;
			return o;
		}
		float4 UnlitFragment(Varyings i) : SV_Target
		{
			float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			#if defined(DEBUG_DISPLAY)
			SurfaceData2D surfaceData;
			InputData2D inputData;
			half4 debugColor = 0;
			InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
			InitializeInputData(i.uv, inputData);
			SETUP_DEBUG_DATA_2D(inputData, i.positionWS);
			if(CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
			{
				return debugColor;
			}
			#endif
			return mainTex;
		}
		ENDHLSL
	}
}
Fallback "Sprites/Default"
```



## Nature
### SpeedTree7BillboardInput

```c
#define SPEEDTREE_PI 3.14159265359
#define SPEEDTREE_ALPHATEST
half _Cutoff;
#include "SpeedTree7CommonInput.hlsl"
CBUFFER_START(UnityBillboardPerCamera)
float3 unity_BillboardNormal;
float3 unity_BillboardTangent;
float4 unity_BillboardCameraParams;
#define unity_BillboardCameraPosition (unity_BillboardCameraParams.xyz)
#define unity_BillboardCameraXZAngle (unity_BillboardCameraParams.w)
CBUFFER_END
CBUFFER_START(UnityBillboardPerBatch)
float4 unity_BillboardInfo; // x: num of billboard slices; y: 1.0f / (delta angle between slices)
float4 unity_BillboardSize; // x: width; y: height; z: bottom
float4 unity_BillboardImageTexCoords[16];
CBUFFER_END
#define _Surface 0.0 // Speed Trees are always opaque
```



### SpeedTree7BillboardPasses

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "SpeedTree7CommonPasses.hlsl"
void InitializeData(inout SpeedTreeVertexInput input, out half2 outUV, out half outHueVariation)
SpeedTreeVertexOutput SpeedTree7Vert(SpeedTreeVertexInput input)
SpeedTreeVertexDepthOutput SpeedTree7VertDepth(SpeedTreeVertexInput input)
```



### SpeedTree7CommonInput

```c
#if defined(SPEEDTREE_ALPHATEST)
#define _ALPHATEST_ON
#endif
#ifdef EFFECT_BUMP
    #define _NORMALMAP
#endif
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#ifdef ENABLE_WIND
    #define WIND_QUALITY_NONE       0
    #define WIND_QUALITY_FASTEST    1
    #define WIND_QUALITY_FAST       2
    #define WIND_QUALITY_BETTER     3
    #define WIND_QUALITY_BEST       4
    #define WIND_QUALITY_PALM       5
    uniform half _WindQuality;
    uniform half _WindEnabled;
    #include "SpeedTreeWind.cginc"
#endif
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;
float4 _MainTex_MipInfo;
#ifdef EFFECT_HUE_VARIATION
    half4 _HueVariation;
#endif
half4 _Color;
// Shadow Casting Light geometric parameters. These variables are used when applying the shadow Normal Bias and are set by UnityEngine.Rendering.Universal.ShadowUtils.SetupShadowCasterConstantBuffer in com.unity.render-pipelines.universal/Runtime/ShadowUtils.cs
// For Directional lights, _LightDirection is used when applying shadow Normal Bias.
// For Spot lights and Point lights, _LightPosition is used to compute the actual light direction because it is different at each shadow caster geometry vertex.
float3 _LightDirection;
float3 _LightPosition;
```



### SpeedTree7CommonPasses

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"
#include "SpeedTreeUtility.hlsl"
struct SpeedTreeVertexInput
{
    float4 vertex       : POSITION;
    float3 normal       : NORMAL;
    float4 tangent      : TANGENT;
    float4 texcoord     : TEXCOORD0;
    float4 texcoord1    : TEXCOORD1;
    float4 texcoord2    : TEXCOORD2;
    float2 texcoord3    : TEXCOORD3;
    half4 color         : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct SpeedTreeVertexOutput
{
    #ifdef VERTEX_COLOR
        half4 color                 : COLOR;
    #endif
    half3 uvHueVariation            : TEXCOORD0;
    #ifdef GEOM_TYPE_BRANCH_DETAIL
        half3 detail                : TEXCOORD1;
    #endif
    half4 fogFactorAndVertexLight   : TEXCOORD2;    // x: fogFactor, yzw: vertex light
    #ifdef EFFECT_BUMP
        half4 normalWS              : TEXCOORD3;    // xyz: normal, w: viewDir.x
        half4 tangentWS             : TEXCOORD4;    // xyz: tangent, w: viewDir.y
        half4 bitangentWS           : TEXCOORD5;    // xyz: bitangent, w: viewDir.z
    #else
        half3 normalWS              : TEXCOORD3;
        half3 viewDirWS             : TEXCOORD4;
    #endif
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        float4 shadowCoord          : TEXCOORD6;
    #endif
    float3 positionWS               : TEXCOORD7;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 8);
    float4 clipPos                  : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
struct SpeedTreeVertexDepthOutput
{
    half3 uvHueVariation            : TEXCOORD0;
    half3 viewDirWS                 : TEXCOORD1;
    float4 clipPos                  : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
struct SpeedTreeVertexDepthNormalOutput
{
    half3 uvHueVariation            : TEXCOORD0;
    float4 clipPos                  : SV_POSITION;
    #ifdef GEOM_TYPE_BRANCH_DETAIL
        half3 detail                : TEXCOORD1;
    #endif
    #ifdef EFFECT_BUMP
        half4 normalWS              : TEXCOORD2;    // xyz: normal, w: viewDir.x
        half4 tangentWS             : TEXCOORD3;    // xyz: tangent, w: viewDir.y
        half4 bitangentWS           : TEXCOORD4;    // xyz: bitangent, w: viewDir.z
    #else
        half3 normalWS              : TEXCOORD2;
        half3 viewDirWS             : TEXCOORD3;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(SpeedTreeVertexOutput input, half3 normalTS, out InputData inputData)
#ifdef GBUFFER
FragmentOutput SpeedTree7Frag(SpeedTreeVertexOutput input)
#else
half4 SpeedTree7Frag(SpeedTreeVertexOutput input) : SV_Target
#ifdef GBUFFER
FragmentOutput SpeedTree7Frag(SpeedTreeVertexOutput input)
#else
half4 SpeedTree7Frag(SpeedTreeVertexOutput input) : SV_Target
#endif
half4 SpeedTree7FragDepth(SpeedTreeVertexDepthOutput input) : SV_Target
```



### SpeedTree7Input

```c
#include "SpeedTree7CommonInput.hlsl"

#define SPEEDTREE_Y_UP
#ifdef GEOM_TYPE_BRANCH_DETAIL
    #define GEOM_TYPE_BRANCH
#endif
#if defined(GEOM_TYPE_FROND) || defined(GEOM_TYPE_LEAF) || defined(GEOM_TYPE_FACING_LEAF)
#define SPEEDTREE_ALPHATEST
    half _Cutoff;
#endif
#ifdef SCENESELECTIONPASS
    int _ObjectId;
    int _PassValue;
#endif
#ifdef GEOM_TYPE_BRANCH_DETAIL
    sampler2D _DetailTex;
#endif
#define _Surface 0.0 // Speed Trees are always opaque
```



### SpeedTree7Passes

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "SpeedTree7CommonPasses.hlsl"
void InitializeData(inout SpeedTreeVertexInput input, float lodValue)
SpeedTreeVertexOutput SpeedTree7Vert(SpeedTreeVertexInput input)
SpeedTreeVertexDepthOutput SpeedTree7VertDepth(SpeedTreeVertexInput input)
SpeedTreeVertexDepthNormalOutput SpeedTree7VertDepthNormal(SpeedTreeVertexInput input)
```



### SpeedTree8Input

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

#ifdef EFFECT_BUMP
    #define _NORMALMAP
#endif
#define _ALPHATEST_ON
#if defined(ENABLE_WIND) && !defined(_WINDQUALITY_NONE)
    #define SPEEDTREE_Y_UP
    #include "SpeedTreeWind.cginc"
    float _WindEnabled;
    UNITY_INSTANCING_BUFFER_START(STWind)
        UNITY_DEFINE_INSTANCED_PROP(float, _GlobalWindTime)
    UNITY_INSTANCING_BUFFER_END(STWind)
#endif
half4 _Color;
int _TwoSided;
#ifdef SCENESELECTIONPASS
    int _ObjectId;
    int _PassValue;
#endif
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;
float4 _MainTex_MipInfo;
#ifdef EFFECT_EXTRA_TEX
    sampler2D _ExtraTex;
#else
    half _Glossiness;
    half _Metallic;
#endif
#ifdef EFFECT_HUE_VARIATION
    half4 _HueVariationColor;
#endif
#ifdef EFFECT_BILLBOARD
    half _BillboardShadowFade;
#endif
#ifdef EFFECT_SUBSURFACE
    sampler2D _SubsurfaceTex;
    half4 _SubsurfaceColor;
    half _SubsurfaceIndirect;
#endif
// Shadow Casting Light geometric parameters. These variables are used when applying the shadow Normal Bias and are set by UnityEngine.Rendering.Universal.ShadowUtils.SetupShadowCasterConstantBuffer in com.unity.render-pipelines.universal/Runtime/ShadowUtils.cs
// For Directional lights, _LightDirection is used when applying shadow Normal Bias.
// For Spot lights and Point lights, _LightPosition is used to compute the actual light direction because it is different at each shadow caster geometry vertex.
float3 _LightDirection;
float3 _LightPosition;
#define GEOM_TYPE_BRANCH 0
#define GEOM_TYPE_FROND 1
#define GEOM_TYPE_LEAF 2
#define GEOM_TYPE_FACINGLEAF 3
#define _Surface 0.0 // Speed Trees are always opaque
```



### SpeedTree8Passes

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
#include "SpeedTreeUtility.hlsl"
struct SpeedTreeVertexInput
{
    float4 vertex       : POSITION;
    float3 normal       : NORMAL;
    float4 tangent      : TANGENT;
    float4 texcoord     : TEXCOORD0;
    float4 texcoord1    : TEXCOORD1;
    float4 texcoord2    : TEXCOORD2;
    float4 texcoord3    : TEXCOORD3;
    float4 color        : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct SpeedTreeVertexOutput
{
    half2 uv                     : TEXCOORD0;
    half4 color                  : TEXCOORD1;
    half4 fogFactorAndVertexLight: TEXCOORD2;// x: fogFactor, yzw: vertex light
    #ifdef EFFECT_BUMP
        half4 normalWS           : TEXCOORD3;// xyz: normal, w: viewDir.x
        half4 tangentWS          : TEXCOORD4;// xyz: tangent, w: viewDir.y
        half4 bitangentWS        : TEXCOORD5;// xyz: bitangent, w: viewDir.z
    #else
        half3 normalWS           : TEXCOORD3;
        half3 viewDirWS          : TEXCOORD4;
    #endif
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        float4 shadowCoord          : TEXCOORD6;
    #endif
    float3 positionWS               : TEXCOORD7;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 8);
    float4 clipPos                  : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
struct SpeedTreeVertexDepthOutput
{
    half2 uv                   : TEXCOORD0;
    half4 color                : TEXCOORD1;
    half3 viewDirWS            : TEXCOORD2;
    float4 clipPos             : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
struct SpeedTreeVertexDepthNormalOutput
{
    half2 uv                 : TEXCOORD0;
    half4 color              : TEXCOORD1;
    #ifdef EFFECT_BUMP
        half4 normalWS       : TEXCOORD2;// xyz: normal, w: viewDir.x
        half4 tangentWS      : TEXCOORD3;// xyz: tangent, w: viewDir.y
        half4 bitangentWS    : TEXCOORD4;// xyz: bitangent, w: viewDir.z
    #else
        half3 normalWS       : TEXCOORD2;
        half3 viewDirWS      : TEXCOORD3;
    #endif
    float4 clipPos           : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
struct SpeedTreeDepthNormalFragmentInput
{
    SpeedTreeVertexDepthNormalOutput interpolated;
	#ifdef EFFECT_BACKSIDE_NORMALS
    FRONT_FACE_TYPE facing : FRONT_FACE_SEMANTIC;
	#endif
};
struct SpeedTreeFragmentInput
{
    SpeedTreeVertexOutput interpolated;
	#ifdef EFFECT_BACKSIDE_NORMALS
    FRONT_FACE_TYPE facing : FRONT_FACE_SEMANTIC;
	#endif
};
void InitializeData(inout SpeedTreeVertexInput input, float lodValue)
SpeedTreeVertexOutput SpeedTree8Vert(SpeedTreeVertexInput input)
SpeedTreeVertexDepthOutput SpeedTree8VertDepth(SpeedTreeVertexInput input)
#ifdef GBUFFER
FragmentOutput SpeedTree8Frag(SpeedTreeFragmentInput input)
#else
half4 SpeedTree8Frag(SpeedTreeFragmentInput input) : SV_Target
#endif
half4 SpeedTree8FragDepth(SpeedTreeVertexDepthOutput input) : SV_Target
SpeedTreeVertexDepthNormalOutput SpeedTree8VertDepthNormal(SpeedTreeVertexInput input)
half4 SpeedTree8FragDepthNormal(SpeedTreeDepthNormalFragmentInput input) : SV_Target
```



### SpeedTreeUtility

```c
uint2 ComputeFadeMaskSeed(float3 V, uint2 positionSS)
```



## Nature(Shader)
### SpeedTree7

```c
Properties
{
	_Color("Main Color", Color) = (1,1,1,1)
	_HueVariation("Hue Variation", Color) = (1.0,0.5,0.0,0.1)
	_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	_DetailTex("Detail", 2D) = "black" {}
	_BumpMap("Normal Map", 2D) = "bump" {}
	_Cutoff("Alpha Cutoff", Range(0,1)) = 0.333
	[MaterialEnum(Off,0,Front,1,Back,2)] _Cull("Cull", Int) = 2
	[MaterialEnum(None,0,Fastest,1,Fast,2,Better,3,Best,4,Palm,5)] _WindQuality("Wind Quality", Range(0,5)) = 0
}
SubShader
{
	Tags
	{
		"Queue" = "Geometry"
		"IgnoreProjector" = "True"
		"RenderType" = "Opaque"
		"DisableBatching" = "LODFading"
		"RenderPipeline" = "UniversalPipeline"
		"UniversalMaterialType" = "SimpleLit"
	}
	LOD 400
	Cull [_Cull]
	Pass
	{
		Name "ForwardLit"
		Tags { "LightMode" = "UniversalForward" }
		HLSLPROGRAM
		#pragma vertex SpeedTree7Vert
		#pragma fragment SpeedTree7Frag
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer assumeuniformscaling maxcount:50
		#pragma shader_feature_local GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_MESH
		#pragma shader_feature_local EFFECT_BUMP
		#pragma shader_feature_local EFFECT_HUE_VARIATION
		#define ENABLE_WIND
		#define VERTEX_COLOR
		#include "SpeedTree7Input.hlsl"
		#include "SpeedTree7Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "SceneSelectionPass"
		Tags{"LightMode" = "SceneSelectionPass"}
		HLSLPROGRAM
		#pragma vertex SpeedTree7VertDepth
		#pragma fragment SpeedTree7FragDepth
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling maxcount:50
		#pragma shader_feature_local GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_MESH
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#define SCENESELECTIONPASS
		#include "SpeedTree7Input.hlsl"
		#include "SpeedTree7Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex SpeedTree7VertDepth
		#pragma fragment SpeedTree7FragDepth
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling maxcount:50
		#pragma shader_feature_local GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_MESH
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#define SHADOW_CASTER
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		#include "SpeedTree7Input.hlsl"
		#include "SpeedTree7Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma vertex SpeedTree7Vert
		#pragma fragment SpeedTree7Frag
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer assumeuniformscaling maxcount:50
		#pragma shader_feature_local GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_MESH
		#pragma shader_feature_local EFFECT_BUMP
		#pragma shader_feature_local EFFECT_HUE_VARIATION
		#define ENABLE_WIND
		#define VERTEX_COLOR
		#define GBUFFER
		#include "SpeedTree7Input.hlsl"
		#include "SpeedTree7Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex SpeedTree7VertDepth
		#pragma fragment SpeedTree7FragDepth
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling maxcount:50
		#pragma shader_feature_local GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_MESH
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#include "SpeedTree7Input.hlsl"
		#include "SpeedTree7Passes.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		HLSLPROGRAM
		#pragma vertex SpeedTree7VertDepthNormal
		#pragma fragment SpeedTree7FragDepthNormal
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling maxcount:50
		#pragma shader_feature_local GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_MESH
		#pragma shader_feature_local EFFECT_BUMP
		#define ENABLE_WIND
		#include "SpeedTree7Input.hlsl"
		#include "SpeedTree7Passes.hlsl"
		ENDHLSL
	}
}
Dependency "BillboardShader" = "Universal Render Pipeline/Nature/SpeedTree7 Billboard"
FallBack "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "SpeedTreeMaterialInspector"
```



### SpeedTree7Billboard

```c
Properties
{
	_Color("Main Color", Color) = (1,1,1,1)
	_HueVariation("Hue Variation", Color) = (1.0,0.5,0.0,0.1)
	_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap("Normal Map", 2D) = "bump" {}
	_Cutoff("Alpha Cutoff", Range(0,1)) = 0.333
	[MaterialEnum(None,0,Fastest,1)] _WindQuality("Wind Quality", Range(0,1)) = 0
}
SubShader
{
	Tags
	{
		"Queue" = "AlphaTest"
		"IgnoreProjector" = "True"
		"RenderType" = "TransparentCutout"
		"DisableBatching" = "LODFading"
		"RenderPipeline" = "UniversalPipeline"
		"UniversalMaterialType" = "SimpleLit"
	}
	LOD 400
	Pass
	{
		Name "ForwardLit"
		Tags { "LightMode" = "UniversalForward" }
		HLSLPROGRAM
		#pragma vertex SpeedTree7Vert
		#pragma fragment SpeedTree7Frag
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ _CLUSTERED_RENDERING
		#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
		#pragma multi_compile __ LOD_FADE_CROSSFADE
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma shader_feature_local EFFECT_BUMP
		#pragma shader_feature_local EFFECT_HUE_VARIATION
		#define ENABLE_WIND
		#include "SpeedTree7BillboardInput.hlsl"
		#include "SpeedTree7BillboardPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex SpeedTree7VertDepth
		#pragma fragment SpeedTree7FragDepth
		#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
		#pragma multi_compile __ LOD_FADE_CROSSFADE
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#define SHADOW_CASTER
		#include "SpeedTree7BillboardInput.hlsl"
		#include "SpeedTree7BillboardPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma vertex SpeedTree7Vert
		#pragma fragment SpeedTree7Frag
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		//#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
		#pragma multi_compile __ LOD_FADE_CROSSFADE
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma shader_feature_local EFFECT_BUMP
		#pragma shader_feature_local EFFECT_HUE_VARIATION
		#define ENABLE_WIND
		#define GBUFFER
		#include "SpeedTree7BillboardInput.hlsl"
		#include "SpeedTree7BillboardPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex SpeedTree7VertDepth
		#pragma fragment SpeedTree7FragDepth
		#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
		#pragma multi_compile __ LOD_FADE_CROSSFADE
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#include "SpeedTree7BillboardInput.hlsl"
		#include "SpeedTree7BillboardPasses.hlsl"
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
```



### SpeedTree8

```c
Properties
{
	_MainTex ("Base (RGB) Transparency (A)", 2D) = "white" {}
	_Color ("Color", Color) = (1,1,1,1)
	[Toggle(EFFECT_HUE_VARIATION)] _HueVariationKwToggle("Hue Variation", Float) = 0
	_HueVariationColor ("Hue Variation Color", Color) = (1.0,0.5,0.0,0.1)
	[Toggle(EFFECT_BUMP)] _NormalMapKwToggle("Normal Mapping", Float) = 0
	_BumpMap("Normal Map", 2D) = "bump" {}
	_ExtraTex ("Smoothness (R), Metallic (G), AO (B)", 2D) = "(0.5, 0.0, 1.0)" {}
	_Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
	_Metallic ("Metallic", Range(0.0, 1.0)) = 0.0
	[Toggle(EFFECT_SUBSURFACE)] _SubsurfaceKwToggle("Subsurface", Float) = 0
	_SubsurfaceTex ("Subsurface (RGB)", 2D) = "white" {}
	_SubsurfaceColor ("Subsurface Color", Color) = (1,1,1,1)
	_SubsurfaceIndirect ("Subsurface Indirect", Range(0.0, 1.0)) = 0.25
	[Toggle(EFFECT_BILLBOARD)] _BillboardKwToggle("Billboard", Float) = 0
	_BillboardShadowFade ("Billboard Shadow Fade", Range(0.0, 1.0)) = 0.5
	[Enum(No,2,Yes,0)] _TwoSided ("Two Sided", Int) = 2 // enum matches cull mode
	[KeywordEnum(None,Fastest,Fast,Better,Best,Palm)] _WindQuality ("Wind Quality", Range(0,5)) = 0
}
SubShader
{
	Tags
	{
		"Queue"="AlphaTest"
		"IgnoreProjector"="True"
		"RenderType"="TransparentCutout"
		"DisableBatching"="LODFading"
		"RenderPipeline" = "UniversalPipeline"
		"UniversalMaterialType" = "Lit"
	}
	LOD 400
	Cull [_TwoSided]
	Pass
	{
		Name "ForwardLit"
		Tags { "LightMode" = "UniversalForward" }
		HLSLPROGRAM
		#pragma vertex SpeedTree8Vert
		#pragma fragment SpeedTree8Frag
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ _CLUSTERED_RENDERING
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer assumeuniformscaling maxcount:50
		#pragma shader_feature_local _WINDQUALITY_NONE _WINDQUALITY_FASTEST _WINDQUALITY_FAST _WINDQUALITY_BETTER _WINDQUALITY_BEST _WINDQUALITY_PALM
		#pragma shader_feature_local EFFECT_BILLBOARD
		#pragma shader_feature_local EFFECT_HUE_VARIATION
		#pragma shader_feature_local EFFECT_SUBSURFACE
		#pragma shader_feature_local EFFECT_BUMP
		#pragma shader_feature_local EFFECT_EXTRA_TEX
		#define ENABLE_WIND
		#define EFFECT_BACKSIDE_NORMALS
		#include "SpeedTree8Input.hlsl"
		#include "SpeedTree8Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "SceneSelectionPass"
		Tags{"LightMode" = "SceneSelectionPass"}
		HLSLPROGRAM
		#pragma vertex SpeedTree8VertDepth
		#pragma fragment SpeedTree8FragDepth
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling maxcount:50
		#pragma shader_feature_local _WINDQUALITY_NONE _WINDQUALITY_FASTEST _WINDQUALITY_FAST _WINDQUALITY_BETTER _WINDQUALITY_BEST _WINDQUALITY_PALM
		#pragma shader_feature_local EFFECT_BILLBOARD
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#define SCENESELECTIONPASS
		#include "SpeedTree8Input.hlsl"
		#include "SpeedTree8Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma vertex SpeedTree8Vert
		#pragma fragment SpeedTree8Frag
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		//#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer assumeuniformscaling maxcount:50
		#pragma shader_feature_local _WINDQUALITY_NONE _WINDQUALITY_FASTEST _WINDQUALITY_FAST _WINDQUALITY_BETTER _WINDQUALITY_BEST _WINDQUALITY_PALM
		#pragma shader_feature_local EFFECT_BILLBOARD
		#pragma shader_feature_local EFFECT_HUE_VARIATION
		#pragma shader_feature_local EFFECT_SUBSURFACE
		#pragma shader_feature_local EFFECT_BUMP
		#pragma shader_feature_local EFFECT_EXTRA_TEX
		#define ENABLE_WIND
		#define EFFECT_BACKSIDE_NORMALS
		#define GBUFFER
		#include "SpeedTree8Input.hlsl"
		#include "SpeedTree8Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex SpeedTree8VertDepth
		#pragma fragment SpeedTree8FragDepth
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling maxcount:50
		#pragma shader_feature_local _WINDQUALITY_NONE _WINDQUALITY_FASTEST _WINDQUALITY_FAST _WINDQUALITY_BETTER _WINDQUALITY_BEST _WINDQUALITY_PALM
		#pragma shader_feature_local EFFECT_BILLBOARD
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#define SHADOW_CASTER
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		#include "SpeedTree8Input.hlsl"
		#include "SpeedTree8Passes.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma vertex SpeedTree8VertDepth
		#pragma fragment SpeedTree8FragDepth
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling maxcount:50
		#pragma shader_feature_local _WINDQUALITY_NONE _WINDQUALITY_FASTEST _WINDQUALITY_FAST _WINDQUALITY_BETTER _WINDQUALITY_BEST _WINDQUALITY_PALM
		#pragma shader_feature_local EFFECT_BILLBOARD
		#define ENABLE_WIND
		#define DEPTH_ONLY
		#include "SpeedTree8Input.hlsl"
		#include "SpeedTree8Passes.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		HLSLPROGRAM
		#pragma vertex SpeedTree8VertDepthNormal
		#pragma fragment SpeedTree8FragDepthNormal
		#pragma shader_feature_local _WINDQUALITY_NONE _WINDQUALITY_FASTEST _WINDQUALITY_FAST _WINDQUALITY_BETTER _WINDQUALITY_BEST _WINDQUALITY_PALM
		#pragma shader_feature_local EFFECT_BUMP
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		#pragma multi_compile_instancing
		#pragma multi_compile_vertex LOD_FADE_PERCENTAGE
		#pragma instancing_options assumeuniformscaling maxcount:50
		#define ENABLE_WIND
		#define EFFECT_BACKSIDE_NORMALS
		#include "SpeedTree8Input.hlsl"
		#include "SpeedTree8Passes.hlsl"
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "SpeedTree8ShaderGUI"
```



## Particles
### ParticlesDepthNormalsPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
VaryingsDepthNormalsParticle DepthNormalsVertex(AttributesDepthNormalsParticle input)
half4 DepthNormalsFragment(VaryingsDepthNormalsParticle input) : SV_TARGET
```



### ParticlesDepthOnlyPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
VaryingsDepthOnlyParticle DepthOnlyVertex(AttributesDepthOnlyParticle input)
half4 DepthOnlyFragment(VaryingsDepthOnlyParticle input) : SV_TARGET
```



### ParticlesEditorPass

```c
#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
float _ObjectId;
float _PassValue;
float4 _SelectionID;
VaryingsParticle vertParticleEditor(AttributesParticle input)
void fragParticleSceneClip(VaryingsParticle input)
half4 fragParticleSceneHighlight(VaryingsParticle input) : SV_Target
half4 fragParticleScenePicking(VaryingsParticle input) : SV_Target
```



### ParticlesInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
struct AttributesParticle
struct VaryingsParticle
struct AttributesDepthOnlyParticle
struct VaryingsDepthOnlyParticle
struct AttributesDepthNormalsParticle
struct VaryingsDepthNormalsParticle
```



### ParticlesLitForwardPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
void InitializeInputData(VaryingsParticle input, half3 normalTS, out InputData inputData)
VaryingsParticle ParticlesLitVertex(AttributesParticle input)
half4 ParticlesLitFragment(VaryingsParticle input) : SV_Target
```



### ParticlesLitGbufferPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
void InitializeInputData(VaryingsParticle input, half3 normalTS, out InputData inputData)
VaryingsParticle ParticlesGBufferVertex(AttributesParticle input)
FragmentOutput ParticlesGBufferFragment(VaryingsParticle input)
```



### ParticlesLitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
CBUFFER_START(UnityPerMaterial)
float4 _SoftParticleFadeParams;
float4 _CameraFadeParams;
float4 _BaseMap_ST;
half4 _BaseColor;
half4 _EmissionColor;
half4 _BaseColorAddSubDiff;
half _Cutoff;
half _Metallic;
half _Smoothness;
half _BumpScale;
half _DistortionStrengthScaled;
half _DistortionBlend;
half _Surface;
CBUFFER_END
TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
#define SOFT_PARTICLE_NEAR_FADE _SoftParticleFadeParams.x
#define SOFT_PARTICLE_INV_FADE_DISTANCE _SoftParticleFadeParams.y
#define CAMERA_NEAR_FADE _CameraFadeParams.x
#define CAMERA_INV_FADE_DISTANCE _CameraFadeParams.y
#if defined(_ALPHAPREMULTIPLY_ON)
#define ALBEDO_MUL albedo
#else
#define ALBEDO_MUL albedo.a
#endif
half4 SampleAlbedo(float2 uv, float3 blendUv, half4 color, float4 particleColor, float4 projectedPosition, TEXTURE2D_PARAM(albedoMap, sampler_albedoMap))
half4 SampleAlbedo(TEXTURE2D_PARAM(albedoMap, sampler_albedoMap), ParticleParams params)
inline void InitializeParticleLitSurfaceData(float2 uv, float3 blendUv, float4 particleColor, float4 projectedPosition, out SurfaceData outSurfaceData)
inline void InitializeParticleLitSurfaceData(ParticleParams params, out SurfaceData outSurfaceData)
```



### ParticlesSimpleLitForwardPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
void InitializeInputData(VaryingsParticle input, half3 normalTS, out InputData inputData)
VaryingsParticle ParticlesLitVertex(AttributesParticle input)
half4 ParticlesLitFragment(VaryingsParticle input) : SV_Target
```



### ParticlesSimpleLitGBufferPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
void InitializeInputData(VaryingsParticle input, half3 normalTS, out InputData inputData)
inline void InitializeParticleSimpleLitSurfaceData(VaryingsParticle input, out SurfaceData outSurfaceData)
VaryingsParticle ParticlesLitGBufferVertex(AttributesParticle input)
FragmentOutput ParticlesLitGBufferFragment(VaryingsParticle input)
```



### ParticlesSimpleLitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
CBUFFER_START(UnityPerMaterial)
    float4 _SoftParticleFadeParams;
    float4 _CameraFadeParams;
    float4 _BaseMap_ST;
    half4 _BaseColor;
    half4 _EmissionColor;
    half4 _BaseColorAddSubDiff;
    half4 _SpecColor;
    half _Cutoff;
    half _Smoothness;
    half _DistortionStrengthScaled;
    half _DistortionBlend;
    half _Surface;
CBUFFER_END
TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
#define SOFT_PARTICLE_NEAR_FADE _SoftParticleFadeParams.x
#define SOFT_PARTICLE_INV_FADE_DISTANCE _SoftParticleFadeParams.y
#define CAMERA_NEAR_FADE _CameraFadeParams.x
#define CAMERA_INV_FADE_DISTANCE _CameraFadeParams.y
#define _BumpScale 1.0
half4 SampleAlbedo(float2 uv, float3 blendUv, half4 color, float4 particleColor, float4 projectedPosition, TEXTURE2D_PARAM(albedoMap, sampler_albedoMap))
half4 SampleAlbedo(TEXTURE2D_PARAM(albedoMap, sampler_albedoMap), ParticleParams params)
half4 SampleSpecularSmoothness(float2 uv, float3 blendUv, half alpha, half4 specColor, TEXTURE2D_PARAM(specGlossMap, sampler_specGlossMap))
```



### ParticlesUnlitForwardPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Unlit.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
void InitializeInputData(VaryingsParticle input, SurfaceData surfaceData, out InputData inputData)
void InitializeSurfaceData(ParticleParams particleParams, out SurfaceData surfaceData)
VaryingsParticle vertParticleUnlit(AttributesParticle input)
half4 fragParticleUnlit(VaryingsParticle input) : SV_Target
```



### ParticlesUnlitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesInput.hlsl"
CBUFFER_START(UnityPerMaterial)
    float4 _SoftParticleFadeParams;
    float4 _CameraFadeParams;
    float4 _BaseMap_ST;
    half4 _BaseColor;
    half4 _EmissionColor;
    half4 _BaseColorAddSubDiff;
    half _Cutoff;
    half _DistortionStrengthScaled;
    half _DistortionBlend;
    half _Surface;
CBUFFER_END
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"
#define SOFT_PARTICLE_NEAR_FADE _SoftParticleFadeParams.x
#define SOFT_PARTICLE_INV_FADE_DISTANCE _SoftParticleFadeParams.y
#define CAMERA_NEAR_FADE _CameraFadeParams.x
#define CAMERA_INV_FADE_DISTANCE _CameraFadeParams.y
#define _BumpScale 1.0
half4 SampleAlbedo(float2 uv, float3 blendUv, half4 color, float4 particleColor, float4 projectedPosition, TEXTURE2D_PARAM(albedoMap, sampler_albedoMap))
half4 SampleAlbedo(TEXTURE2D_PARAM(albedoMap, sampler_albedoMap), ParticleParams params)
```



## Particles(Shader)
### ParticlesLit

```c
Properties
{
    [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
    [MainColor]   _BaseColor("Base Color", Color) = (1,1,1,1)
    _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
    _MetallicGlossMap("Metallic Map", 2D) = "white" {}
    [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
    _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
    _BumpScale("Scale", Float) = 1.0
    _BumpMap("Normal Map", 2D) = "bump" {}
    [HDR] _EmissionColor("Color", Color) = (0,0,0)
    _EmissionMap("Emission", 2D) = "white" {}
    [ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
    // -------------------------------------
    // Particle specific
    _SoftParticlesNearFadeDistance("Soft Particles Near Fade", Float) = 0.0
    _SoftParticlesFarFadeDistance("Soft Particles Far Fade", Float) = 1.0
    _CameraNearFadeDistance("Camera Near Fade", Float) = 1.0
    _CameraFarFadeDistance("Camera Far Fade", Float) = 2.0
    _DistortionBlend("Distortion Blend", Range(0.0, 1.0)) = 0.5
    _DistortionStrength("Distortion Strength", Float) = 1.0
    // -------------------------------------
    // Hidden properties - Generic
    _Surface("__surface", Float) = 0.0
    _Blend("__mode", Float) = 0.0
    _Cull("__cull", Float) = 2.0
    [ToggleUI] _AlphaClip("__clip", Float) = 0.0
    [HideInInspector] _BlendOp("__blendop", Float) = 0.0
    [HideInInspector] _SrcBlend("__src", Float) = 1.0
    [HideInInspector] _DstBlend("__dst", Float) = 0.0
    [HideInInspector] _ZWrite("__zw", Float) = 1.0
    // Particle specific
    _ColorMode("_ColorMode", Float) = 0.0
    [HideInInspector] _BaseColorAddSubDiff("_ColorMode", Vector) = (0,0,0,0)
    [ToggleOff] _FlipbookBlending("__flipbookblending", Float) = 0.0
    [ToggleUI] _SoftParticlesEnabled("__softparticlesenabled", Float) = 0.0
    [ToggleUI] _CameraFadingEnabled("__camerafadingenabled", Float) = 0.0
    [ToggleUI] _DistortionEnabled("__distortionenabled", Float) = 0.0
    [HideInInspector] _SoftParticleFadeParams("__softparticlefadeparams", Vector) = (0,0,0,0)
    [HideInInspector] _CameraFadeParams("__camerafadeparams", Vector) = (0,0,0,0)
    [HideInInspector] _DistortionStrengthScaled("Distortion Strength Scaled", Float) = 0.1
    // Editmode props
    _QueueOffset("Queue offset", Float) = 0.0
    // ObsoleteProperties
    [HideInInspector] _FlipbookMode("flipbook", Float) = 0
    [HideInInspector] _Glossiness("gloss", Float) = 0
    [HideInInspector] _Mode("mode", Float) = 0
    [HideInInspector] _Color("color", Color) = (1,1,1,1)
}
HLSLINCLUDE
//Particle shaders rely on "write" to CB syntax which is not supported by DXC
#pragma never_use_dxc
ENDHLSL
SubShader
{
    Tags
    {
        "RenderType" = "Opaque"
        "IgnoreProjector" = "True"
        "PreviewType" = "Plane"
        "PerformanceChecks" = "False"
        "RenderPipeline" = "UniversalPipeline"
        "UniversalMaterialType" = "Lit"
    }
    // ------------------------------------------------------------------
    //  Forward pass.
    Pass
    {
        // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
        // no LightMode tag are also rendered by Universal Render Pipeline
        Name "ForwardLit"
        Tags
        {
            "LightMode" = "UniversalForward"
        }
        BlendOp[_BlendOp]
        Blend[_SrcBlend][_DstBlend]
        ZWrite[_ZWrite]
        Cull[_Cull]
        HLSLPROGRAM
        #pragma target 2.0
        // -------------------------------------
        // Material Keywords
        #pragma shader_feature_local _NORMALMAP
        #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
        #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local_fragment _EMISSION
        #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
        // -------------------------------------
        // Particle Keywords
        #pragma shader_feature_local _FLIPBOOKBLENDING_ON
        #pragma shader_feature_local _SOFTPARTICLES_ON
        #pragma shader_feature_local _FADING_ON
        #pragma shader_feature_local _DISTORTION_ON
        #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        #pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
        // -------------------------------------
        // Universal Pipeline keywords
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile_fragment _ _LIGHT_COOKIES
        #pragma multi_compile _ _CLUSTERED_RENDERING
        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile_fog
        #pragma multi_compile_instancing
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma instancing_options procedural:ParticleInstancingSetup
        #pragma vertex ParticlesLitVertex
        #pragma fragment ParticlesLitFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitForwardPass.hlsl"
        ENDHLSL
    }
    // ------------------------------------------------------------------
    //  GBuffer pass.
    Pass
    {
        // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
        // no LightMode tag are also rendered by Universal Render Pipeline
        Name "GBuffer"
        Tags{"LightMode" = "UniversalGBuffer"}
        ZWrite[_ZWrite]
        Cull[_Cull]
        HLSLPROGRAM
        #pragma exclude_renderers gles
        #pragma target 2.0
        // -------------------------------------
        // Material Keywords
        #pragma shader_feature_local _NORMALMAP
        #pragma shader_feature_local_fragment _EMISSION
        #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
        #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
        // -------------------------------------
        // Particle Keywords
        #pragma shader_feature_local_fragment _ALPHATEST_ON
        #pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
        #pragma shader_feature_local _FLIPBOOKBLENDING_ON
        // -------------------------------------
        // Universal Pipeline keywords
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:ParticleInstancingSetup
        #pragma vertex ParticlesGBufferVertex
        #pragma fragment ParticlesGBufferFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitGbufferPass.hlsl"
        ENDHLSL
    }
    // ------------------------------------------------------------------
    //  Depth Only pass.
    Pass
    {
        Name "DepthOnly"
        Tags{"LightMode" = "DepthOnly"}
        ZWrite On
        ColorMask 0
        Cull[_Cull]
        HLSLPROGRAM
        #pragma target 2.0
        // -------------------------------------
        // Material Keywords
        #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature_local _ _FLIPBOOKBLENDING_ON
        #pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:ParticleInstancingSetup
        #pragma vertex DepthOnlyVertex
        #pragma fragment DepthOnlyFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthOnlyPass.hlsl"
        ENDHLSL
    }
    // This pass is used when drawing to a _CameraNormalsTexture texture
    Pass
    {
        Name "DepthNormals"
        Tags{"LightMode" = "DepthNormals"}
        ZWrite On
        Cull[_Cull]
        HLSLPROGRAM
        #pragma target 2.0
        // -------------------------------------
        // Material Keywords
        #pragma shader_feature_local _ _NORMALMAP
        #pragma shader_feature_local _ _FLIPBOOKBLENDING_ON
        #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:ParticleInstancingSetup
        #pragma vertex DepthNormalsVertex
        #pragma fragment DepthNormalsFragment
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthNormalsPass.hlsl"
        ENDHLSL
    }
    // ------------------------------------------------------------------
    //  Scene view outline pass.
    Pass
    {
        Name "SceneSelectionPass"
        Tags { "LightMode" = "SceneSelectionPass" }
        BlendOp Add
        Blend One Zero
        ZWrite On
        Cull Off
        HLSLPROGRAM
        #define PARTICLES_EDITOR_META_PASS
        #pragma target 2.0
        // -------------------------------------
        // Particle Keywords
        #pragma shader_feature_local_fragment _ALPHATEST_ON
        #pragma shader_feature_local _FLIPBOOKBLENDING_ON
        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:ParticleInstancingSetup
        #pragma vertex vertParticleEditor
        #pragma fragment fragParticleSceneHighlight
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
        ENDHLSL
    }
    // ------------------------------------------------------------------
    //  Scene picking buffer pass.
    Pass
    {
        Name "ScenePickingPass"
        Tags{ "LightMode" = "Picking" }
        BlendOp Add
        Blend One Zero
        ZWrite On
        Cull Off
        HLSLPROGRAM
        #define PARTICLES_EDITOR_META_PASS
        #pragma target 2.0
        // -------------------------------------
        // Particle Keywords
        #pragma shader_feature_local_fragment _ALPHATEST_ON
        #pragma shader_feature_local _FLIPBOOKBLENDING_ON
        // -------------------------------------
        // Unity defined keywords
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:ParticleInstancingSetup
        #pragma vertex vertParticleEditor
        #pragma fragment fragParticleScenePicking
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesLitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
        ENDHLSL
    }
    Pass
    {
        Name "Universal2D"
        Tags{ "LightMode" = "Universal2D" }
        Blend[_SrcBlend][_DstBlend]
        ZWrite[_ZWrite]
        Cull[_Cull]
        HLSLPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma shader_feature_local_fragment _ALPHATEST_ON
        #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
        #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
        ENDHLSL
    }
}
Fallback "Universal Render Pipeline/Particles/Simple Lit"
CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.ParticlesLitShader"
```



### ParticlesSimpleLit

```c
// ------------------------------------------
// Only directional light is supported for lit particles
// No shadow
// No distortion
Properties
{
	[MainTexture] _BaseMap("Base Map", 2D) = "white" {}
	[MainColor]   _BaseColor("Base Color", Color) = (1,1,1,1)
	_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
	_SpecGlossMap("Specular", 2D) = "white" {}
	_SpecColor("Specular", Color) = (1.0, 1.0, 1.0)
	_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
	_BumpScale("Scale", Float) = 1.0
	_BumpMap("Normal Map", 2D) = "bump" {}
	[HDR] _EmissionColor("Color", Color) = (0,0,0)
	_EmissionMap("Emission", 2D) = "white" {}
	_SmoothnessSource("Smoothness Source", Float) = 0.0
	_SpecularHighlights("Specular Highlights", Float) = 1.0
	[ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
	// -------------------------------------
	// Particle specific
	_SoftParticlesNearFadeDistance("Soft Particles Near Fade", Float) = 0.0
	_SoftParticlesFarFadeDistance("Soft Particles Far Fade", Float) = 1.0
	_CameraNearFadeDistance("Camera Near Fade", Float) = 1.0
	_CameraFarFadeDistance("Camera Far Fade", Float) = 2.0
	_DistortionBlend("Distortion Blend", Range(0.0, 1.0)) = 0.5
	_DistortionStrength("Distortion Strength", Float) = 1.0
	// -------------------------------------
	// Hidden properties - Generic
	_Surface("__surface", Float) = 0.0
	_Blend("__mode", Float) = 0.0
	_Cull("__cull", Float) = 2.0
	[ToggleUI] _AlphaClip("__clip", Float) = 0.0
	[HideInInspector] _BlendOp("__blendop", Float) = 0.0
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
	[HideInInspector] _ZWrite("__zw", Float) = 1.0
	// Particle specific
	_ColorMode("_ColorMode", Float) = 0.0
	[HideInInspector] _BaseColorAddSubDiff("_ColorMode", Vector) = (0,0,0,0)
	[ToggleOff] _FlipbookBlending("__flipbookblending", Float) = 0.0
	[ToggleUI] _SoftParticlesEnabled("__softparticlesenabled", Float) = 0.0
	[ToggleUI] _CameraFadingEnabled("__camerafadingenabled", Float) = 0.0
	[ToggleUI] _DistortionEnabled("__distortionenabled", Float) = 0.0
	[HideInInspector] _SoftParticleFadeParams("__softparticlefadeparams", Vector) = (0,0,0,0)
	[HideInInspector] _CameraFadeParams("__camerafadeparams", Vector) = (0,0,0,0)
	[HideInInspector] _DistortionStrengthScaled("Distortion Strength Scaled", Float) = 0.1
	// Editmode props
	_QueueOffset("Queue offset", Float) = 0.0
	// ObsoleteProperties
	[HideInInspector] _FlipbookMode("flipbook", Float) = 0
	[HideInInspector] _Glossiness("gloss", Float) = 0
	[HideInInspector] _Mode("mode", Float) = 0
	[HideInInspector] _Color("color", Color) = (1,1,1,1)
}
HLSLINCLUDE
//Particle shaders rely on "write" to CB syntax which is not supported by DXC
#pragma never_use_dxc
ENDHLSL
SubShader
{
	Tags{"RenderType" = "Opaque" "IgnoreProjector" = "True" "PreviewType" = "Plane" "PerformanceChecks" = "False" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit"}
	// ------------------------------------------------------------------
	//  Forward pass.
	Pass
	{
		// Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Render Pipeline
		Name "ForwardLit"
		Tags {"LightMode" = "UniversalForward"}
		BlendOp[_BlendOp]
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		// -------------------------------------
		// Particle Keywords
		#pragma shader_feature_local _FLIPBOOKBLENDING_ON
		#pragma shader_feature_local _SOFTPARTICLES_ON
		#pragma shader_feature_local _FADING_ON
		#pragma shader_feature_local _DISTORTION_ON
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
		#pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex ParticlesLitVertex
		#pragma fragment ParticlesLitFragment
		#define BUMP_SCALE_NOT_SUPPORTED 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitForwardPass.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  GBuffer pass.
	Pass
	{
		// Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Render Pipeline
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _EMISSION
		#pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
		// -------------------------------------
		// Particle Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
		#pragma shader_feature_local _FLIPBOOKBLENDING_ON
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex ParticlesLitGBufferVertex
		#pragma fragment ParticlesLitGBufferFragment
		#define BUMP_SCALE_NOT_SUPPORTED 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitGBufferPass.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  Depth Only pass.
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _ _ALPHATEST_ON
		#pragma shader_feature_local _ _FLIPBOOKBLENDING_ON
		#pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _ _NORMALMAP
		#pragma shader_feature_local _ _FLIPBOOKBLENDING_ON
		#pragma shader_feature_local _ _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  Scene view outline pass.
	Pass
	{
		Name "SceneSelectionPass"
		Tags { "LightMode" = "SceneSelectionPass" }
		BlendOp Add
		Blend One Zero
		ZWrite On
		Cull Off
		HLSLPROGRAM
		#define PARTICLES_EDITOR_META_PASS
		#pragma target 2.0
		// -------------------------------------
		// Particle Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local _FLIPBOOKBLENDING_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex vertParticleEditor
		#pragma fragment fragParticleSceneHighlight
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  Scene picking buffer pass.
	Pass
	{
		Name "ScenePickingPass"
		Tags{ "LightMode" = "Picking" }
		BlendOp Add
		Blend One Zero
		ZWrite On
		Cull Off
		HLSLPROGRAM
		#define PARTICLES_EDITOR_META_PASS
		#pragma target 2.0
		// -------------------------------------
		// Particle Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local _FLIPBOOKBLENDING_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex vertParticleEditor
		#pragma fragment fragParticleScenePicking
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesSimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Universal2D"
		Tags{ "LightMode" = "Universal2D" }
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
		ENDHLSL
	}
}
Fallback "Universal Render Pipeline/Particles/Unlit"
CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.ParticlesSimpleLitShader"
```



### ParticlesUnlit

```c
Properties
{
	[MainTexture] _BaseMap("Base Map", 2D) = "white" {}
	[MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
	_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
	_BumpMap("Normal Map", 2D) = "bump" {}
	[HDR] _EmissionColor("Color", Color) = (0,0,0)
	_EmissionMap("Emission", 2D) = "white" {}
	// -------------------------------------
	// Particle specific
	_SoftParticlesNearFadeDistance("Soft Particles Near Fade", Float) = 0.0
	_SoftParticlesFarFadeDistance("Soft Particles Far Fade", Float) = 1.0
	_CameraNearFadeDistance("Camera Near Fade", Float) = 1.0
	_CameraFarFadeDistance("Camera Far Fade", Float) = 2.0
	_DistortionBlend("Distortion Blend", Range(0.0, 1.0)) = 0.5
	_DistortionStrength("Distortion Strength", Float) = 1.0
	// -------------------------------------
	// Hidden properties - Generic
	_Surface("__surface", Float) = 0.0
	_Blend("__mode", Float) = 0.0
	_Cull("__cull", Float) = 2.0
	[ToggleUI] _AlphaClip("__clip", Float) = 0.0
	[HideInInspector] _BlendOp("__blendop", Float) = 0.0
	[HideInInspector] _SrcBlend("__src", Float) = 1.0
	[HideInInspector] _DstBlend("__dst", Float) = 0.0
	[HideInInspector] _ZWrite("__zw", Float) = 1.0
	// Particle specific
	_ColorMode("_ColorMode", Float) = 0.0
	[HideInInspector] _BaseColorAddSubDiff("_ColorMode", Vector) = (0,0,0,0)
	[ToggleOff] _FlipbookBlending("__flipbookblending", Float) = 0.0
	[ToggleUI] _SoftParticlesEnabled("__softparticlesenabled", Float) = 0.0
	[ToggleUI] _CameraFadingEnabled("__camerafadingenabled", Float) = 0.0
	[ToggleUI] _DistortionEnabled("__distortionenabled", Float) = 0.0
	[HideInInspector] _SoftParticleFadeParams("__softparticlefadeparams", Vector) = (0,0,0,0)
	[HideInInspector] _CameraFadeParams("__camerafadeparams", Vector) = (0,0,0,0)
	[HideInInspector] _DistortionStrengthScaled("Distortion Strength Scaled", Float) = 0.1
	// Editmode props
	_QueueOffset("Queue offset", Float) = 0.0
	// ObsoleteProperties
	[HideInInspector] _FlipbookMode("flipbook", Float) = 0
	[HideInInspector] _Mode("mode", Float) = 0
	[HideInInspector] _Color("color", Color) = (1,1,1,1)
}
HLSLINCLUDE
//Particle shaders rely on "write" to CB syntax which is not supported by DXC
#pragma never_use_dxc
ENDHLSL
SubShader
{
	Tags{"RenderType" = "Opaque" "IgnoreProjector" = "True" "PreviewType" = "Plane" "PerformanceChecks" = "False" "RenderPipeline" = "UniversalPipeline"}
	// ------------------------------------------------------------------
	//  Forward pass.
	Pass
	{
		Name "ForwardLit"
		BlendOp[_BlendOp]
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]
		Cull[_Cull]
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _EMISSION
		// -------------------------------------
		// Particle Keywords
		#pragma shader_feature_local _FLIPBOOKBLENDING_ON
		#pragma shader_feature_local _SOFTPARTICLES_ON
		#pragma shader_feature_local _FADING_ON
		#pragma shader_feature_local _DISTORTION_ON
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
		#pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
		#pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex vertParticleUnlit
		#pragma fragment fragParticleUnlit
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitForwardPass.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  Depth Only pass.
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull[_Cull]
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _ _ALPHATEST_ON
		#pragma shader_feature_local _ _FLIPBOOKBLENDING_ON
		#pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthOnlyPass.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture with the forward renderer or the depthNormal prepass with the deferred renderer.
	Pass
	{
		Name "DepthNormalsOnly"
		Tags{"LightMode" = "DepthNormalsOnly"}
		ZWrite On
		Cull[_Cull]
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma vertex DepthNormalsVertex
		#pragma fragment DepthNormalsFragment
		// -------------------------------------
		// Material Keywords
		#pragma shader_feature_local _ _NORMALMAP
		#pragma shader_feature_local _ _ALPHATEST_ON
		#pragma shader_feature_local_fragment _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT // forward-only variant
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  Scene view outline pass.
	Pass
	{
		Name "SceneSelectionPass"
		Tags { "LightMode" = "SceneSelectionPass" }
		BlendOp Add
		Blend One Zero
		ZWrite On
		Cull Off
		HLSLPROGRAM
		#define PARTICLES_EDITOR_META_PASS
		#pragma target 2.0
		// -------------------------------------
		// Particle Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local _FLIPBOOKBLENDING_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex vertParticleEditor
		#pragma fragment fragParticleSceneHighlight
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  Scene picking buffer pass.
	Pass
	{
		Name "ScenePickingPass"
		Tags{ "LightMode" = "Picking" }
		BlendOp Add
		Blend One Zero
		ZWrite On
		Cull Off
		HLSLPROGRAM
		#define PARTICLES_EDITOR_META_PASS
		#pragma target 2.0
		// -------------------------------------
		// Particle Keywords
		#pragma shader_feature_local_fragment _ALPHATEST_ON
		#pragma shader_feature_local _FLIPBOOKBLENDING_ON
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:ParticleInstancingSetup
		#pragma vertex vertParticleEditor
		#pragma fragment fragParticleScenePicking
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesEditorPass.hlsl"
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.ParticlesUnlitShader"
```



## PostProcessing
### Common

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Fullscreen.hlsl"
float4x4 _FullscreenProjMat;
float4 TransformFullscreenMesh(half3 positionOS)
Varyings VertFullscreenMesh(Attributes input)
SAMPLER(sampler_LinearClamp);
SAMPLER(sampler_LinearRepeat);
SAMPLER(sampler_PointClamp);
SAMPLER(sampler_PointRepeat);
half GetLuminance(half3 colorLinear)
real3 GetSRGBToLinear(real3 c)
real4 GetSRGBToLinear(real4 c)
real3 GetLinearToSRGB(real3 c)
real4 GetLinearToSRGB(real4 c)
half3 ApplyVignette(half3 input, float2 uv, float2 center, float intensity, float roundness, float smoothness, half3 color)
half3 ApplyTonemap(half3 input)
half3 ApplyColorGrading(half3 input, float postExposure, TEXTURE2D_PARAM(lutTex, lutSampler), float3 lutParams, TEXTURE2D_PARAM(userLutTex, userLutSampler), float3 userLutParams, float userLutContrib)
half3 ApplyGrain(half3 input, float2 uv, TEXTURE2D_PARAM(GrainTexture, GrainSampler), float intensity, float response, float2 scale, float2 offset)
half3 ApplyDithering(half3 input, float2 uv, TEXTURE2D_PARAM(BlueNoiseTexture, BlueNoiseSampler), float2 scale, float2 offset)
#define FXAA_SPAN_MAX   (8.0)
#define FXAA_REDUCE_MUL (1.0 / 8.0)
#define FXAA_REDUCE_MIN (1.0 / 128.0)
half3 FXAAFetch(float2 coords, float2 offset, TEXTURE2D_X(inputTexture))
half3 FXAALoad(int2 icoords, int idx, int idy, float4 sourceSize, TEXTURE2D_X(inputTexture))
half3 ApplyFXAA(half3 color, float2 positionNDC, int2 positionSS, float4 sourceSize, TEXTURE2D_X(inputTexture))
```



### SubpixelMorphologicalAntialiasing

```c
#if defined(SMAA_PRESET_LOW)
#define SMAA_THRESHOLD 0.15
#define SMAA_MAX_SEARCH_STEPS 4
#define SMAA_DISABLE_DIAG_DETECTION
#define SMAA_DISABLE_CORNER_DETECTION
#elif defined(SMAA_PRESET_MEDIUM)
#define SMAA_THRESHOLD 0.1
#define SMAA_MAX_SEARCH_STEPS 8
#define SMAA_DISABLE_DIAG_DETECTION
#define SMAA_DISABLE_CORNER_DETECTION
#elif defined(SMAA_PRESET_HIGH)
#define SMAA_THRESHOLD 0.1
#define SMAA_MAX_SEARCH_STEPS 16
#define SMAA_MAX_SEARCH_STEPS_DIAG 8
#define SMAA_CORNER_ROUNDING 25
#elif defined(SMAA_PRESET_ULTRA)
#define SMAA_THRESHOLD 0.05
#define SMAA_MAX_SEARCH_STEPS 32
#define SMAA_MAX_SEARCH_STEPS_DIAG 16
#define SMAA_CORNER_ROUNDING 25
#endif
#ifndef SMAA_THRESHOLD
#define SMAA_THRESHOLD 0.1
#endif
#ifndef SMAA_DEPTH_THRESHOLD
#define SMAA_DEPTH_THRESHOLD (0.1 * SMAA_THRESHOLD)
#endif
#ifndef SMAA_MAX_SEARCH_STEPS
#define SMAA_MAX_SEARCH_STEPS 16
#endif
#ifndef SMAA_MAX_SEARCH_STEPS_DIAG
#define SMAA_MAX_SEARCH_STEPS_DIAG 8
#endif
#ifndef SMAA_CORNER_ROUNDING
#define SMAA_CORNER_ROUNDING 25
#endif
#ifndef SMAA_LOCAL_CONTRAST_ADAPTATION_FACTOR
#define SMAA_LOCAL_CONTRAST_ADAPTATION_FACTOR 2.0
#endif
#ifndef SMAA_PREDICATION
#define SMAA_PREDICATION 0
#endif
#ifndef SMAA_PREDICATION_THRESHOLD
#define SMAA_PREDICATION_THRESHOLD 0.01
#endif
#ifndef SMAA_PREDICATION_SCALE
#define SMAA_PREDICATION_SCALE 2.0
#endif
#ifndef SMAA_PREDICATION_STRENGTH
#define SMAA_PREDICATION_STRENGTH 0.4
#endif
#ifndef SMAA_REPROJECTION
#define SMAA_REPROJECTION 0
#endif
#ifndef SMAA_UV_BASED_REPROJECTION
#define SMAA_UV_BASED_REPROJECTION 0
#endif
#ifndef SMAA_REPROJECTION_WEIGHT_SCALE
#define SMAA_REPROJECTION_WEIGHT_SCALE 30.0
#endif
#ifndef SMAA_INCLUDE_VS
#define SMAA_INCLUDE_VS 1
#endif
#ifndef SMAA_INCLUDE_PS
#define SMAA_INCLUDE_PS 1
#endif
#ifndef SMAA_AREATEX_SELECT
#if defined(SMAA_HLSL_3)
#define SMAA_AREATEX_SELECT(sample) sample.ra
#else
#define SMAA_AREATEX_SELECT(sample) sample.rg
#endif
#endif
#ifndef SMAA_SEARCHTEX_SELECT
#define SMAA_SEARCHTEX_SELECT(sample) sample.r
#endif
#ifndef SMAA_DECODE_VELOCITY
#define SMAA_DECODE_VELOCITY(sample) sample.rg
#endif
#define SMAA_AREATEX_MAX_DISTANCE 16
#define SMAA_AREATEX_MAX_DISTANCE_DIAG 20
#define SMAA_AREATEX_PIXEL_SIZE (1.0 / float2(160.0, 560.0))
#define SMAA_AREATEX_SUBTEX_SIZE (1.0 / 7.0)
#define SMAA_SEARCHTEX_SIZE float2(66.0, 33.0)
#define SMAA_SEARCHTEX_PACKED_SIZE float2(64.0, 16.0)
#define SMAA_CORNER_ROUNDING_NORM (float(SMAA_CORNER_ROUNDING) / 100.0)
#if defined(SMAA_HLSL_3)
#define SMAATexture2D(tex) sampler2D tex
#define SMAATexturePass2D(tex) tex
#define SMAASampleLevelZero(tex, coord) tex2Dlod(tex, float4(coord, 0.0, 0.0))
#define SMAASampleLevelZeroPoint(tex, coord) tex2Dlod(tex, float4(coord, 0.0, 0.0))
#define SMAASampleLevelZeroOffset(tex, coord, offset) tex2Dlod(tex, float4(coord + offset * SMAA_RT_METRICS.xy, 0.0, 0.0))
#define SMAASample(tex, coord) tex2D(tex, coord)
#define SMAASamplePoint(tex, coord) tex2D(tex, coord)
#define SMAASampleOffset(tex, coord, offset) tex2D(tex, coord + offset * SMAA_RT_METRICS.xy)
#define SMAA_FLATTEN
#define SMAA_BRANCH
#endif
#if defined(SMAA_HLSL_4) || defined(SMAA_HLSL_4_1)
#define SMAATexture2D(tex) TEXTURE2D_X(tex)
#define SMAATexture2D_Non_Array(tex) Texture2D tex
#define SMAATexturePass2D(tex) tex
#define SMAASampleLevelZero(tex, coord) SAMPLE_TEXTURE2D_X_LOD(tex, LinearSampler, coord, 0)
#define SMAASampleLevelZeroNoRescale(tex, coord) tex.SampleLevel(LinearSampler, coord, 0)
#define SMAASampleLevelZeroPoint(tex, coord) SAMPLE_TEXTURE2D_X_LOD(tex, PointSampler, coord, 0)
#define SMAASampleLevelZeroOffset(tex, coord, offset) SAMPLE_TEXTURE2D_X_LOD(tex, LinearSampler, coord + offset * SMAA_RT_METRICS.xy, 0)
#define SMAASample(tex, coord) SAMPLE_TEXTURE2D_X(tex, LinearSampler, coord)
#define SMAASamplePoint(tex, coord) SAMPLE_TEXTURE2D_X(tex, PointSampler, coord)
#define SMAASampleOffset(tex, coord, offset) SAMPLE_TEXTURE2D_X(tex, LinearSampler, coord + offset * SMAA_RT_METRICS.xy)
#define SMAA_FLATTEN [flatten]
#define SMAA_BRANCH [branch]
#define SMAATexture2DMS2(tex) Texture2DMS<float4, 2> tex
#define SMAALoad(tex, pos, sample) tex.Load(pos, sample)
#endif
#if defined(SMAA_GLSL_3) || defined(SMAA_GLSL_4)
#define SMAATexture2D(tex) sampler2D tex
#define SMAATexturePass2D(tex) tex
#define SMAASampleLevelZero(tex, coord) textureLod(tex, coord, 0.0)
#define SMAASampleLevelZeroPoint(tex, coord) textureLod(tex, coord, 0.0)
#define SMAASampleLevelZeroOffset(tex, coord, offset) textureLodOffset(tex, coord, 0.0, offset)
#define SMAASample(tex, coord) texture(tex, coord)
#define SMAASamplePoint(tex, coord) texture(tex, coord)
#define SMAASampleOffset(tex, coord, offset) texture(tex, coord, offset)
#define SMAA_FLATTEN
#define SMAA_BRANCH
#define lerp(a, b, t) mix(a, b, t)
#define saturate(a) clamp(a, 0.0, 1.0)
#if defined(SMAA_GLSL_4)
#define mad(a, b, c) fma(a, b, c)
#define SMAAGather(tex, coord) textureGather(tex, coord)
#else
#define mad(a, b, c) ((a) * (b) + (c))
#endif
#define float2 vec2
#define float3 vec3
#define float4 vec4
#define int2 ivec2
#define int3 ivec3
#define int4 ivec4
#define bool2 bvec2
#define bool3 bvec3
#define bool4 bvec4
#endif
#if !defined(SMAA_HLSL_3) && !defined(SMAA_HLSL_4) && !defined(SMAA_HLSL_4_1) && !defined(SMAA_GLSL_3) && !defined(SMAA_GLSL_4) && !defined(SMAA_CUSTOM_SL)
#error you must define the shading language: SMAA_HLSL_*, SMAA_GLSL_* or SMAA_CUSTOM_SL
#endif
float3 SMAAGatherNeighbours(float2 texcoord,
    float4 offset[3],
    SMAATexture2D(tex))
float2 SMAACalculatePredicatedThreshold(float2 texcoord,
    float4 offset[3],
    SMAATexture2D(predicationTex)) 
void SMAAMovc(bool2 cond, inout float2 variable, float2 value) 
void SMAAMovc(bool4 cond, inout float4 variable, float4 value) 
#if SMAA_INCLUDE_VS
void SMAAEdgeDetectionVS(float2 texcoord,
    out float4 offset[3]) 
void SMAABlendingWeightCalculationVS(float2 texcoord,
    out float2 pixcoord,
    out float4 offset[3]) 
void SMAANeighborhoodBlendingVS(float2 texcoord,
    out float4 offset) 
#endif // SMAA_INCLUDE_VS
#if SMAA_INCLUDE_PS//--------------------------------
float2 SMAALumaEdgeDetectionPS(float2 texcoord,
    float4 offset[3],
    SMAATexture2D(colorTex)
#if SMAA_PREDICATION
    , SMAATexture2D(predicationTex)
#endif
) 
float2 SMAAColorEdgeDetectionPS(float2 texcoord,
    float4 offset[3],
    SMAATexture2D(colorTex)
#if SMAA_PREDICATION
    , SMAATexture2D(predicationTex)
#endif
)
float2 SMAADepthEdgeDetectionPS(float2 texcoord,
    float4 offset[3],
    SMAATexture2D(depthTex)) 
#if !defined(SMAA_DISABLE_DIAG_DETECTION)
float2 SMAADecodeDiagBilinearAccess(float2 e) 
float4 SMAADecodeDiagBilinearAccess(float4 e) 
float2 SMAASearchDiag1(SMAATexture2D(edgesTex), float2 texcoord, float2 dir, out float2 e) 
float2 SMAASearchDiag2(SMAATexture2D(edgesTex), float2 texcoord, float2 dir, out float2 e) 
float2 SMAAAreaDiag(SMAATexture2D_Non_Array(areaTex), float2 dist, float2 e, float offset) 
float2 SMAACalculateDiagWeights(SMAATexture2D(edgesTex), SMAATexture2D_Non_Array(areaTex), float2 texcoord, float2 e, float4 subsampleIndices) 
#endif
float SMAASearchLength(SMAATexture2D_Non_Array(searchTex), float2 e, float offset) 
float SMAASearchXLeft(SMAATexture2D(edgesTex), SMAATexture2D_Non_Array(searchTex), float2 texcoord, float end) 
float SMAASearchXRight(SMAATexture2D(edgesTex), SMAATexture2D_Non_Array(searchTex), float2 texcoord, float end) 
float SMAASearchYUp(SMAATexture2D(edgesTex), SMAATexture2D_Non_Array(searchTex), float2 texcoord, float end) 
float SMAASearchYDown(SMAATexture2D(edgesTex), SMAATexture2D_Non_Array(searchTex), float2 texcoord, float end) 
float2 SMAAArea(SMAATexture2D_Non_Array(areaTex), float2 dist, float e1, float e2, float offset) 
void SMAADetectHorizontalCornerPattern(SMAATexture2D(edgesTex), inout float2 weights, float4 texcoord, float2 d) 
void SMAADetectVerticalCornerPattern(SMAATexture2D(edgesTex), inout float2 weights, float4 texcoord, float2 d) 
float4 SMAABlendingWeightCalculationPS(float2 texcoord,
    float2 pixcoord,
    float4 offset[3],
    SMAATexture2D(edgesTex),
    SMAATexture2D_Non_Array(areaTex),
    SMAATexture2D_Non_Array(searchTex),
    float4 subsampleIndices) 
#endif
#if SMAA_UV_BASED_REPROJECTION
float2 SMAAReproject(float2 texcoord)
#endif
float4 SMAANeighborhoodBlendingPS(float2 texcoord,
    float4 offset,
    SMAATexture2D(colorTex),
    SMAATexture2D(blendTex)
#if SMAA_REPROJECTION
    , SMAATexture2D(velocityTex)
#endif
) 
float4 SMAAResolvePS(float2 texcoord,
    SMAATexture2D(currentColorTex),
    SMAATexture2D(previousColorTex)
#if SMAA_REPROJECTION
    , SMAATexture2D(velocityTex)
#endif
) 
#ifdef SMAALoad
void SMAASeparatePS(float4 position,
    float2 texcoord,
    out float4 target0,
    out float4 target1,
    SMAATexture2DMS2(colorTexMS)) 
#endif
#endif // SMAA_INCLUDE_PS-----------------------
```



### SubpixelMorphologicalAntialiasingBridge

```c
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/SubpixelMorphologicalAntialiasing.hlsl"
#define SMAA_HLSL_4_1
#if _SMAA_PRESET_LOW
    #define SMAA_PRESET_LOW
#elif _SMAA_PRESET_MEDIUM
    #define SMAA_PRESET_MEDIUM
#else
    #define SMAA_PRESET_HIGH
#endif
TEXTURE2D_X(_ColorTexture);
TEXTURE2D_X(_BlendTexture);
TEXTURE2D(_AreaTexture);
TEXTURE2D(_SearchTexture);
float4 _Metrics;
#define SMAA_RT_METRICS _Metrics
#define SMAA_AREATEX_SELECT(s) s.rg
#define SMAA_SEARCHTEX_SELECT(s) s.a
#define LinearSampler sampler_LinearClamp
#define PointSampler sampler_PointClamp
#if UNITY_COLORSPACE_GAMMA
    #define GAMMA_FOR_EDGE_DETECTION (1)
#else
    #define GAMMA_FOR_EDGE_DETECTION (1/2.2)
#endif
struct VaryingsEdge
VaryingsEdge VertEdge(Attributes input)
float4 FragEdge(VaryingsEdge input) : SV_Target
struct VaryingsBlend
VaryingsBlend VertBlend(Attributes input)
float4 FragBlend(VaryingsBlend input) : SV_Target
struct VaryingsNeighbor
VaryingsNeighbor VertNeighbor(Attributes input)
float4 FragNeighbor(VaryingsNeighbor input) : SV_Target
```



## PostProcessing(Shader)
### Bloom

```c
HLSLINCLUDE
#pragma exclude_renderers gles
#pragma multi_compile_local _ _USE_RGBM
#pragma multi_compile _ _USE_DRAW_PROCEDURAL
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
TEXTURE2D_X(_SourceTex);
float4 _SourceTex_TexelSize;
TEXTURE2D_X(_SourceTexLowMip);
float4 _SourceTexLowMip_TexelSize;
float4 _Params; // x: scatter, y: clamp, z: threshold (linear), w: threshold knee
#define Scatter             _Params.x
#define ClampMax            _Params.y
#define Threshold           _Params.z
#define ThresholdKnee       _Params.w
half4 EncodeHDR(half3 color)
{
#if _USE_RGBM
	half4 outColor = EncodeRGBM(color);
#else
	half4 outColor = half4(color, 1.0);
#endif
#if UNITY_COLORSPACE_GAMMA
	return half4(sqrt(outColor.xyz), outColor.w); // linear to γ
#else
	return outColor;
#endif
}
half3 DecodeHDR(half4 color)
{
#if UNITY_COLORSPACE_GAMMA
	color.xyz *= color.xyz; // γ to linear
#endif
#if _USE_RGBM
	return DecodeRGBM(color);
#else
	return color.xyz;
#endif
}
half4 FragPrefilter(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
#if _BLOOM_HQ
	float texelSize = _SourceTex_TexelSize.x;
	half4 A = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(-1.0, -1.0));
	half4 B = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(0.0, -1.0));
	half4 C = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(1.0, -1.0));
	half4 D = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(-0.5, -0.5));
	half4 E = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(0.5, -0.5));
	half4 F = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(-1.0, 0.0));
	half4 G = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv);
	half4 H = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(1.0, 0.0));
	half4 I = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(-0.5, 0.5));
	half4 J = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(0.5, 0.5));
	half4 K = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(-1.0, 1.0));
	half4 L = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(0.0, 1.0));
	half4 M = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + texelSize * float2(1.0, 1.0));
	half2 div = (1.0 / 4.0) * half2(0.5, 0.125);
	half4 o = (D + E + I + J) * div.x;
	o += (A + B + G + F) * div.y;
	o += (B + C + H + G) * div.y;
	o += (F + G + L + K) * div.y;
	o += (G + H + M + L) * div.y;
	half3 color = o.xyz;
#else
	half3 color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv).xyz;
#endif
	// User controlled clamp to limit crazy high broken spec
	color = min(ClampMax, color);
	// Thresholding
	half brightness = Max3(color.r, color.g, color.b);
	half softness = clamp(brightness - Threshold + ThresholdKnee, 0.0, 2.0 * ThresholdKnee);
	softness = (softness * softness) / (4.0 * ThresholdKnee + 1e-4);
	half multiplier = max(brightness - Threshold, softness) / max(brightness, 1e-4);
	color *= multiplier;
	// Clamp colors to positive once in prefilter. Encode can have a sqrt, and sqrt(-x) == NaN. Up/Downsample passes would then spread the NaN.
	color = max(color, 0);
	return EncodeHDR(color);
}
half4 FragBlurH(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float texelSize = _SourceTex_TexelSize.x * 2.0;
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	// 9-tap gaussian blur on the downsampled source
	half3 c0 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - float2(texelSize * 4.0, 0.0)));
	half3 c1 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - float2(texelSize * 3.0, 0.0)));
	half3 c2 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - float2(texelSize * 2.0, 0.0)));
	half3 c3 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - float2(texelSize * 1.0, 0.0)));
	half3 c4 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv                               ));
	half3 c5 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + float2(texelSize * 1.0, 0.0)));
	half3 c6 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + float2(texelSize * 2.0, 0.0)));
	half3 c7 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + float2(texelSize * 3.0, 0.0)));
	half3 c8 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + float2(texelSize * 4.0, 0.0)));
	half3 color = c0 * 0.01621622 + c1 * 0.05405405 + c2 * 0.12162162 + c3 * 0.19459459
				+ c4 * 0.22702703
				+ c5 * 0.19459459 + c6 * 0.12162162 + c7 * 0.05405405 + c8 * 0.01621622;
	return EncodeHDR(color);
}
half4 FragBlurV(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float texelSize = _SourceTex_TexelSize.y;
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	// Optimized bilinear 5-tap gaussian on the same-sized source (9-tap equivalent)
	half3 c0 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - float2(0.0, texelSize * 3.23076923)));
	half3 c1 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - float2(0.0, texelSize * 1.38461538)));
	half3 c2 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv                                      ));
	half3 c3 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + float2(0.0, texelSize * 1.38461538)));
	half3 c4 = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + float2(0.0, texelSize * 3.23076923)));
	half3 color = c0 * 0.07027027 + c1 * 0.31621622
				+ c2 * 0.22702703
				+ c3 * 0.31621622 + c4 * 0.07027027;
	return EncodeHDR(color);
}
half3 Upsample(float2 uv)
{
	half3 highMip = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv));
#if _BLOOM_HQ && !defined(SHADER_API_GLES)
	half3 lowMip = DecodeHDR(SampleTexture2DBicubic(TEXTURE2D_X_ARGS(_SourceTexLowMip, sampler_LinearClamp), uv, _SourceTexLowMip_TexelSize.zwxy, (1.0).xx, unity_StereoEyeIndex));
#else
	half3 lowMip = DecodeHDR(SAMPLE_TEXTURE2D_X(_SourceTexLowMip, sampler_LinearClamp, uv));
#endif
	return lerp(highMip, lowMip, Scatter);
}
half4 FragUpsample(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	half3 color = Upsample(UnityStereoTransformScreenSpaceTex(input.uv));
	return EncodeHDR(color);
}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "Bloom Prefilter"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragPrefilter
			#pragma multi_compile_local _ _BLOOM_HQ
		ENDHLSL
	}
	Pass
	{
		Name "Bloom Blur Horizontal"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragBlurH
		ENDHLSL
	}
	Pass
	{
		Name "Bloom Blur Vertical"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragBlurV
		ENDHLSL
	}
	Pass
	{
		Name "Bloom Upsample"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragUpsample
			#pragma multi_compile_local _ _BLOOM_HQ
		ENDHLSL
	}
}
```



### BokehDepthOfField

```c
HLSLINCLUDE
#pragma exclude_renderers gles
#pragma multi_compile_local_fragment _ _USE_FAST_SRGB_LINEAR_CONVERSION
#pragma multi_compile _ _USE_DRAW_PROCEDURAL
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
// Do not change this without changing PostProcessPass.PrepareBokehKernel()
#define SAMPLE_COUNT            42
// Toggle this to reduce flickering - note that it will reduce overall bokeh energy and add
// a small cost to the pre-filtering pass
#define COC_LUMA_WEIGHTING      0
TEXTURE2D_X(_SourceTex);
TEXTURE2D_X(_DofTexture);
TEXTURE2D_X(_FullCoCTexture);
half4 _SourceSize;
half4 _HalfSourceSize;
half4 _DownSampleScaleFactor;
half4 _CoCParams;
half4 _BokehKernel[SAMPLE_COUNT];
half4 _BokehConstants;
#define FocusDist       _CoCParams.x
#define MaxCoC          _CoCParams.y
#define MaxRadius       _CoCParams.z
#define RcpAspect       _CoCParams.w
half FragCoC(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	float depth = LOAD_TEXTURE2D_X(_CameraDepthTexture, _SourceSize.xy * uv).x;
	float linearEyeDepth = LinearEyeDepth(depth, _ZBufferParams);
	half coc = (1.0 - FocusDist / linearEyeDepth) * MaxCoC;
	half nearCoC = clamp(coc, -1.0, 0.0);
	half farCoC = saturate(coc);
	return saturate((farCoC + nearCoC + 1.0) * 0.5);
}
half4 FragPrefilter(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
#if SHADER_TARGET >= 45 && defined(PLATFORM_SUPPORT_GATHER)
	// Sample source colors
	half4 cr = GATHER_RED_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv);
	half4 cg = GATHER_GREEN_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv);
	half4 cb = GATHER_BLUE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv);
	half3 c0 = half3(cr.x, cg.x, cb.x);
	half3 c1 = half3(cr.y, cg.y, cb.y);
	half3 c2 = half3(cr.z, cg.z, cb.z);
	half3 c3 = half3(cr.w, cg.w, cb.w);
	// Sample CoCs
	half4 cocs = GATHER_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, uv) * 2.0 - 1.0;
	half coc0 = cocs.x;
	half coc1 = cocs.y;
	half coc2 = cocs.z;
	half coc3 = cocs.w;
#else
	float3 duv = _SourceSize.zwz * float3(0.5, 0.5, -0.5);
	float2 uv0 = uv - duv.xy;
	float2 uv1 = uv - duv.zy;
	float2 uv2 = uv + duv.zy;
	float2 uv3 = uv + duv.xy;
	// Sample source colors
	half3 c0 = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv0).xyz;
	half3 c1 = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv1).xyz;
	half3 c2 = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv2).xyz;
	half3 c3 = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv3).xyz;
	// Sample CoCs
	half coc0 = SAMPLE_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, uv0).x * 2.0 - 1.0;
	half coc1 = SAMPLE_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, uv1).x * 2.0 - 1.0;
	half coc2 = SAMPLE_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, uv2).x * 2.0 - 1.0;
	half coc3 = SAMPLE_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, uv3).x * 2.0 - 1.0;
#endif
#if COC_LUMA_WEIGHTING
	// Apply CoC and luma weights to reduce bleeding and flickering
	half w0 = abs(coc0) / (Max3(c0.x, c0.y, c0.z) + 1.0);
	half w1 = abs(coc1) / (Max3(c1.x, c1.y, c1.z) + 1.0);
	half w2 = abs(coc2) / (Max3(c2.x, c2.y, c2.z) + 1.0);
	half w3 = abs(coc3) / (Max3(c3.x, c3.y, c3.z) + 1.0);
	// Weighted average of the color samples
	half3 avg = c0 * w0 + c1 * w1 + c2 * w2 + c3 * w3;
	avg /= max(w0 + w1 + w2 + w3, 1e-5);
#else
	half3 avg = (c0 + c1 + c2 + c3) / 4.0;
#endif
	// Select the largest CoC value
	half cocMin = min(coc0, Min3(coc1, coc2, coc3));
	half cocMax = max(coc0, Max3(coc1, coc2, coc3));
	half coc = (-cocMin > cocMax ? cocMin : cocMax) * MaxRadius;
	// Premultiply CoC
	avg *= smoothstep(0, _SourceSize.w * 2.0, abs(coc));
#if defined(UNITY_COLORSPACE_GAMMA)
	avg = GetSRGBToLinear(avg);
#endif
	return half4(avg, coc);
}
void Accumulate(half4 samp0, float2 uv, half4 disp, inout half4 farAcc, inout half4 nearAcc)
{
	half4 samp = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + disp.wy);
	// Compare CoC of the current sample and the center sample and select smaller one
	half farCoC = max(min(samp0.a, samp.a), 0.0);
	// Compare the CoC to the sample distance & add a small margin to smooth out
	half farWeight = saturate((farCoC - disp.z + _BokehConstants.y) / _BokehConstants.y);
	half nearWeight = saturate((-samp.a - disp.z + _BokehConstants.y) / _BokehConstants.y);
	// Cut influence from focused areas because they're darkened by CoC premultiplying. This is only
	// needed for near field
	nearWeight *= step(_BokehConstants.x, -samp.a);
	// Accumulation
	farAcc += half4(samp.rgb, 1.0h) * farWeight;
	nearAcc += half4(samp.rgb, 1.0h) * nearWeight;
}
half4 FragBlur(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	half4 samp0 = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv);
	half4 farAcc = 0.0;  // Background: far field bokeh
	half4 nearAcc = 0.0; // Foreground: near field bokeh
	// Center sample isn't in the kernel array, accumulate it separately
	Accumulate(samp0, uv, 0.0, farAcc, nearAcc);
	UNITY_LOOP
	for (int si = 0; si < SAMPLE_COUNT; si++)
	{
		Accumulate(samp0, uv, _BokehKernel[si], farAcc, nearAcc);
	}
	// Get the weighted average
	farAcc.rgb /= farAcc.a + (farAcc.a == 0.0);     // Zero-div guard
	nearAcc.rgb /= nearAcc.a + (nearAcc.a == 0.0);
	// Normalize the total of the weights for the near field
	nearAcc.a *= PI / (SAMPLE_COUNT + 1);
	// Alpha premultiplying
	half alpha = saturate(nearAcc.a);
	half3 rgb = lerp(farAcc.rgb, nearAcc.rgb, alpha);
	return half4(rgb, alpha);
}
half4 FragPostBlur(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	// 9-tap tent filter with 4 bilinear samples
	float4 duv = _SourceSize.zwzw * _DownSampleScaleFactor.zwzw * float4(0.5, 0.5, -0.5, 0);
	half4 acc;
	acc  = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - duv.xy);
	acc += SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv - duv.zy);
	acc += SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + duv.zy);
	acc += SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv + duv.xy);
	return acc * 0.25;
}
half4 FragComposite(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	half4 dof = SAMPLE_TEXTURE2D_X(_DofTexture, sampler_LinearClamp, uv);
	half coc = SAMPLE_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, uv).r;
	coc = (coc - 0.5) * 2.0 * MaxRadius;
	// Convert CoC to far field alpha value
	float ffa = smoothstep(_SourceSize.w * 2.0, _SourceSize.w * 4.0, coc);
	half4 color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv);
#if defined(UNITY_COLORSPACE_GAMMA)
	color = GetSRGBToLinear(color);
#endif
	half alpha = Max3(dof.r, dof.g, dof.b);
	color = lerp(color, half4(dof.rgb, alpha), ffa + dof.a - ffa * dof.a);
#if defined(UNITY_COLORSPACE_GAMMA)
	color = GetLinearToSRGB(color);
#endif
	return color;
}
ENDHLSL
SubShader
{
	Tags { "RenderPipeline" = "UniversalPipeline" }
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "Bokeh Depth Of Field CoC"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragCoC
			#pragma target 4.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Prefilter"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragPrefilter
			#pragma target 4.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Blur"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragBlur
			#pragma target 4.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Post Blur"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragPostBlur
			#pragma target 4.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Composite"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragComposite
			#pragma target 4.5
		ENDHLSL
	}
}
// SM3.5 fallbacks - needed because of the use of Gather
SubShader
{
	Tags { "RenderPipeline" = "UniversalPipeline" }
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "Bokeh Depth Of Field CoC"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragCoC
			#pragma target 3.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Prefilter"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragPrefilter
			#pragma target 3.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Blur"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragBlur
			#pragma target 3.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Post Blur"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragPostBlur
			#pragma target 3.5
		ENDHLSL
	}
	Pass
	{
		Name "Bokeh Depth Of Field Composite"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragComposite
			#pragma target 3.5
		ENDHLSL
	}
}
```



### CameraMotionBlur

```c
HLSLINCLUDE
#pragma exclude_renderers gles
#pragma multi_compile _ _USE_DRAW_PROCEDURAL
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Random.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
TEXTURE2D_X(_SourceTex);
#if defined(USING_STEREO_MATRICES)
float4x4 _PrevViewProjMStereo[2];
#define _PrevViewProjM  _PrevViewProjMStereo[unity_StereoEyeIndex]
#define _ViewProjM unity_MatrixVP
#else
float4x4 _ViewProjM;
float4x4 _PrevViewProjM;
#endif
half _Intensity;
half _Clamp;
half4 _SourceSize;
struct VaryingsCMB
{
	float4 positionCS    : SV_POSITION;
	float4 uv            : TEXCOORD0;
	UNITY_VERTEX_OUTPUT_STEREO
};
VaryingsCMB VertCMB(Attributes input)
{
	VaryingsCMB output;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
	#if _USE_DRAW_PROCEDURAL
	GetProceduralQuad(input.vertexID, output.positionCS, output.uv.xy);
	#else
	output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
	output.uv.xy = input.uv;
	#endif
	float4 projPos = output.positionCS * 0.5;
	projPos.xy = projPos.xy + projPos.w;
	output.uv.zw = projPos.xy;
	return output;
}
half2 ClampVelocity(half2 velocity, half maxVelocity)
{
	half len = length(velocity);
	return (len > 0.0) ? min(len, maxVelocity) * (velocity * rcp(len)) : 0.0;
}
// Per-pixel camera velocity
half2 GetCameraVelocity(float4 uv)
{
	float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_PointClamp, uv.xy).r;
#if UNITY_REVERSED_Z
	depth = 1.0 - depth;
#endif
	depth = 2.0 * depth - 1.0;
	float3 viewPos = ComputeViewSpacePosition(uv.zw, depth, unity_CameraInvProjection);
	float4 worldPos = float4(mul(unity_CameraToWorld, float4(viewPos, 1.0)).xyz, 1.0);
	float4 prevPos = worldPos;
	float4 prevClipPos = mul(_PrevViewProjM, prevPos);
	float4 curClipPos = mul(_ViewProjM, worldPos);
	half2 prevPosCS = prevClipPos.xy / prevClipPos.w;
	half2 curPosCS = curClipPos.xy / curClipPos.w;
	return ClampVelocity(prevPosCS - curPosCS, _Clamp);
}
half3 GatherSample(half sampleNumber, half2 velocity, half invSampleCount, float2 centerUV, half randomVal, half velocitySign)
{
	half  offsetLength = (sampleNumber + 0.5h) + (velocitySign * (randomVal - 0.5h));
	float2 sampleUV = centerUV + (offsetLength * invSampleCount) * velocity * velocitySign;
	return SAMPLE_TEXTURE2D_X(_SourceTex, sampler_PointClamp, sampleUV).xyz;
}
half4 DoMotionBlur(VaryingsCMB input, int iterations)
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv.xy);
	half2 velocity = GetCameraVelocity(float4(uv, input.uv.zw)) * _Intensity;
	half randomVal = InterleavedGradientNoise(uv * _SourceSize.xy, 0);
	half invSampleCount = rcp(iterations * 2.0);
	half3 color = 0.0;
	UNITY_UNROLL
	for (int i = 0; i < iterations; i++)
	{
		color += GatherSample(i, velocity, invSampleCount, uv, randomVal, -1.0);
		color += GatherSample(i, velocity, invSampleCount, uv, randomVal,  1.0);
	}
	return half4(color * invSampleCount, 1.0);
}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "Camera Motion Blur - Low Quality"
		HLSLPROGRAM
			#pragma vertex VertCMB
			#pragma fragment Frag
			half4 Frag(VaryingsCMB input) : SV_Target
			{
				return DoMotionBlur(input, 2);
			}
		ENDHLSL
	}
	Pass
	{
		Name "Camera Motion Blur - Medium Quality"
		HLSLPROGRAM
			#pragma vertex VertCMB
			#pragma fragment Frag
			half4 Frag(VaryingsCMB input) : SV_Target
			{
				return DoMotionBlur(input, 3);
			}
		ENDHLSL
	}
	Pass
	{
		Name "Camera Motion Blur - High Quality"
		HLSLPROGRAM
			#pragma vertex VertCMB
			#pragma fragment Frag
			half4 Frag(VaryingsCMB input) : SV_Target
			{
				return DoMotionBlur(input, 4);
			}
		ENDHLSL
	}
}
```



### EdgeAdaptiveSpatialUpsampling

```c
HLSLINCLUDE
#pragma multi_compile_vertex _ _USE_DRAW_PROCEDURAL
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
TEXTURE2D_X(_SourceTex);
float4 _SourceSize;
#define FSR_INPUT_TEXTURE _SourceTex
#define FSR_INPUT_SAMPLER sampler_LinearClamp
#include "Packages/com.unity.render-pipelines.core/Runtime/PostProcessing/Shaders/FSRCommon.hlsl"
half4 FragEASU(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	uint2 integerUv = uv * _ScreenParams.xy;
	half3 color = ApplyEASU(integerUv);
	// Convert to linearly encoded color before we pass our output over to RCAS
	#if UNITY_COLORSPACE_GAMMA
	color = GetSRGBToLinear(color);
	#else
	color = Gamma20ToLinear(color);
	#endif
	return half4(color, 1.0);
}
ENDHLSL
/// Shader that performs the EASU (upscaling) component of the two part FidelityFX Super Resolution technique
/// The second part of the technique (RCAS) is handled in the FinalPost shader
/// Note: This shader requires shader target 4.5 because it relies on texture gather instructions
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "EASU"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragEASU
			#pragma target 4.5
		ENDHLSL
	}
}
```



### FinalPost

```c
HLSLINCLUDE
	#pragma exclude_renderers gles
	#pragma multi_compile_local_fragment _ _POINT_SAMPLING _RCAS
	#pragma multi_compile_local_fragment _ _FXAA
	#pragma multi_compile_local_fragment _ _FILM_GRAIN
	#pragma multi_compile_local_fragment _ _DITHERING
	#pragma multi_compile_local_fragment _ _LINEAR_TO_SRGB_CONVERSION
	#pragma multi_compile _ _USE_DRAW_PROCEDURAL
	#pragma multi_compile_fragment _ DEBUG_DISPLAY
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingFullscreen.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
	TEXTURE2D_X(_SourceTex);
	TEXTURE2D(_Grain_Texture);
	TEXTURE2D(_BlueNoise_Texture);
	float4 _SourceSize;
	float2 _Grain_Params;
	float4 _Grain_TilingParams;
	float4 _Dithering_Params;
	#if SHADER_TARGET >= 45
		#define FSR_INPUT_TEXTURE _SourceTex
		#define FSR_INPUT_SAMPLER sampler_LinearClamp
		#include "Packages/com.unity.render-pipelines.core/Runtime/PostProcessing/Shaders/FSRCommon.hlsl"
	#endif
	#define GrainIntensity          _Grain_Params.x
	#define GrainResponse           _Grain_Params.y
	#define GrainScale              _Grain_TilingParams.xy
	#define GrainOffset             _Grain_TilingParams.zw
	#define DitheringScale          _Dithering_Params.xy
	#define DitheringOffset         _Dithering_Params.zw
	half4 Frag(Varyings input) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
		float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
		float2 positionNDC = uv;
		int2   positionSS  = uv * _SourceSize.xy;
		#if _POINT_SAMPLING
		half3 color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_PointClamp, uv).xyz;
		#elif _RCAS && SHADER_TARGET >= 45
		half3 color = ApplyRCAS(positionSS);
		// When Unity is configured to use gamma color encoding, we must convert back from linear after RCAS is performed.
		// (The input color data for this shader variant is always linearly encoded because RCAS requires it)
		#if UNITY_COLORSPACE_GAMMA
		color = GetLinearToSRGB(color);
		#endif
		#else
		half3 color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uv).xyz;
		#endif
		#if _FXAA
		{
			color = ApplyFXAA(color, positionNDC, positionSS, _SourceSize, _SourceTex);
		}
		#endif
		#if _FILM_GRAIN
		{
			color = ApplyGrain(color, positionNDC, TEXTURE2D_ARGS(_Grain_Texture, sampler_LinearRepeat), GrainIntensity, GrainResponse, GrainScale, GrainOffset);
		}
		#endif
		#if _LINEAR_TO_SRGB_CONVERSION
		{
			color = LinearToSRGB(color);
		}
		#endif
		#if _DITHERING
		{
			color = ApplyDithering(color, positionNDC, TEXTURE2D_ARGS(_BlueNoise_Texture, sampler_PointRepeat), DitheringScale, DitheringOffset);
		}
		#endif
		half4 finalColor = half4(color, 1);
		#if defined(DEBUG_DISPLAY)
		half4 debugColor = 0;
		if(CanDebugOverrideOutputColor(finalColor, uv, debugColor))
		{
			return debugColor;
		}
		#endif
		return finalColor;
	}
ENDHLSL
/// Standard FinalPost shader variant with support for FSR
/// Note: FSR requires shader target 4.5 because it relies on texture gather instructions
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "FinalPost"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
			#pragma target 4.5
		ENDHLSL
	}
}
/// Fallback version of FinalPost shader which lacks support for FSR
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "FinalPost"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
		ENDHLSL
	}
}
```



### GaussianDepthOfField

```c
HLSLINCLUDE
#pragma target 3.5
#pragma exclude_renderers gles
#pragma multi_compile _ _USE_DRAW_PROCEDURAL
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
TEXTURE2D_X(_SourceTex);
TEXTURE2D_X(_ColorTexture);
TEXTURE2D_X(_FullCoCTexture);
TEXTURE2D_X(_HalfCoCTexture);
float4 _SourceSize;
float4 _DownSampleScaleFactor;
float3 _CoCParams;
#define FarStart        _CoCParams.x
#define FarEnd          _CoCParams.y
#define MaxRadius       _CoCParams.z
#define BLUR_KERNEL 0
#if BLUR_KERNEL == 0
// Offsets & coeffs for optimized separable bilinear 3-tap gaussian (5-tap equivalent)
const static int kTapCount = 3;
const static float kOffsets[] = {
	-1.33333333,
	 0.00000000,
	 1.33333333
};
const static half kCoeffs[] = {
	 0.35294118,
	 0.29411765,
	 0.35294118
};
#elif BLUR_KERNEL == 1
// Offsets & coeffs for optimized separable bilinear 5-tap gaussian (9-tap equivalent)
const static int kTapCount = 5;
const static float kOffsets[] = {
	-3.23076923,
	-1.38461538,
	 0.00000000,
	 1.38461538,
	 3.23076923
};
const static half kCoeffs[] = {
	 0.07027027,
	 0.31621622,
	 0.22702703,
	 0.31621622,
	 0.07027027
};
#endif
half FragCoC(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	float depth = LOAD_TEXTURE2D_X(_CameraDepthTexture, _SourceSize.xy * uv).x;
	depth = LinearEyeDepth(depth, _ZBufferParams);
	half coc = (depth - FarStart) / (FarEnd - FarStart);
	return saturate(coc);
}
struct PrefilterOutput
{
	half  coc   : SV_Target0;
	half3 color : SV_Target1;
};
PrefilterOutput FragPrefilter(Varyings input)
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
#if _HIGH_QUALITY_SAMPLING
	// Use a rotated grid to minimize artifacts coming from horizontal and vertical boundaries
	// "High Quality Antialiasing" [Lorach07]
	const int kCount = 5;
	const float2 kTaps[] = {
		float2( 0.0,  0.0),
		float2( 0.9, -0.4),
		float2(-0.9,  0.4),
		float2( 0.4,  0.9),
		float2(-0.4, -0.9)
	};
	half3 colorAcc = 0.0;
	half farCoCAcc = 0.0;
	UNITY_UNROLL
	for (int i = 0; i < kCount; i++)
	{
		float2 tapCoord = _SourceSize.zw * kTaps[i] + uv;
		half3 tapColor = SAMPLE_TEXTURE2D_X(_ColorTexture, sampler_LinearClamp, tapCoord).xyz;
		half coc = SAMPLE_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, tapCoord).x;
		// Pre-multiply CoC to reduce bleeding of background blur on focused areas
		colorAcc += tapColor * coc;
		farCoCAcc += coc;
	}
	half3 color = colorAcc * rcp(kCount);
	half farCoC = farCoCAcc * rcp(kCount);
#else
	// Bilinear sampling the coc is technically incorrect but we're aiming for speed here
	half farCoC = SAMPLE_TEXTURE2D_X(_FullCoCTexture, sampler_LinearClamp, uv).x;
	// Fast bilinear downscale of the source target and pre-multiply the CoC to reduce
	// bleeding of background blur on focused areas
	half3 color = SAMPLE_TEXTURE2D_X(_ColorTexture, sampler_LinearClamp, uv).xyz;
	color *= farCoC;
#endif
	PrefilterOutput o;
	o.coc   = farCoC;
	o.color = color;
	return o;
}
half4 Blur(Varyings input, float2 dir, float premultiply)
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	// Use the center CoC as radius
	int2 positionSS = int2(_SourceSize.xy * _DownSampleScaleFactor.xy * uv);
	half samp0CoC = LOAD_TEXTURE2D_X(_HalfCoCTexture, positionSS).x;
	float2 offset = _SourceSize.zw * _DownSampleScaleFactor.zw * dir * samp0CoC * MaxRadius;
	half4 acc = 0.0;
	UNITY_UNROLL
	for (int i = 0; i < kTapCount; i++)
	{
		float2 sampCoord = uv + kOffsets[i] * offset;
		half sampCoC = SAMPLE_TEXTURE2D_X(_HalfCoCTexture, sampler_LinearClamp, sampCoord).x;
		half3 sampColor = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, sampCoord).xyz;
		// Weight & pre-multiply to limit bleeding on the focused area
		half weight = saturate(1.0 - (samp0CoC - sampCoC));
		acc += half4(sampColor, premultiply ? sampCoC : 1.0) * kCoeffs[i] * weight;
	}
	acc.xyz /= acc.w + 1e-4; // Zero-div guard
	return half4(acc.xyz, 1.0);
}
half4 FragBlurH(Varyings input) : SV_Target
{
	return Blur(input, float2(1.0, 0.0), 1.0);
}
half4 FragBlurV(Varyings input) : SV_Target
{
	return Blur(input, float2(0.0, 1.0), 0.0);
}
half4 FragComposite(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	half3 baseColor = LOAD_TEXTURE2D_X(_SourceTex, _SourceSize.xy * uv).xyz;
	half coc = LOAD_TEXTURE2D_X(_FullCoCTexture, _SourceSize.xy * uv).x;
#if _HIGH_QUALITY_SAMPLING && !defined(SHADER_API_GLES)
	half3 farColor = SampleTexture2DBicubic(TEXTURE2D_X_ARGS(_ColorTexture, sampler_LinearClamp), uv, _SourceSize * _DownSampleScaleFactor, 1.0, unity_StereoEyeIndex).xyz;
#else
	half3 farColor = SAMPLE_TEXTURE2D_X(_ColorTexture, sampler_LinearClamp, uv).xyz;
#endif
	half3 dstColor = 0.0;
	half dstAlpha = 1.0;
	UNITY_BRANCH
	if (coc > 0.0)
	{
		// Non-linear blend
		// "CryEngine 3 Graphics Gems" [Sousa13]
		half blend = sqrt(coc * TWO_PI);
		dstColor = farColor * saturate(blend);
		dstAlpha = saturate(1.0 - blend);
	}
	return half4(baseColor * dstAlpha + dstColor, 1.0);
}
ENDHLSL
SubShader
{
	Tags { "RenderPipeline" = "UniversalPipeline" }
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "Gaussian Depth Of Field CoC"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragCoC
		ENDHLSL
	}
	Pass
	{
		Name "Gaussian Depth Of Field Prefilter"
		HLSLPROGRAM
			#pragma vertex VertFullscreenMesh
			#pragma fragment FragPrefilter
			#pragma multi_compile_local _ _HIGH_QUALITY_SAMPLING
		ENDHLSL
	}
	Pass
	{
		Name "Gaussian Depth Of Field Blur Horizontal"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragBlurH
		ENDHLSL
	}
	Pass
	{
		Name "Gaussian Depth Of Field Blur Vertical"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragBlurV
		ENDHLSL
	}
	Pass
	{
		Name "Gaussian Depth Of Field Composite"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment FragComposite
			#pragma multi_compile_local _ _HIGH_QUALITY_SAMPLING
		ENDHLSL
	}
}
```



### LensFlareDataDriven

```c
SubShader
{
	// Additive
	Pass
	{
		Name "LensFlareAdditive"
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
		LOD 100
		Blend One One
		ZWrite Off
		Cull Off
		ZTest Always
		HLSLPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma exclude_renderers gles
		#pragma multi_compile_fragment _ FLARE_CIRCLE FLARE_POLYGON
		#pragma multi_compile_fragment _ FLARE_INVERSE_SDF
		#pragma multi_compile _ FLARE_OCCLUSION
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/Runtime/PostProcessing/Shaders/LensFlareCommon.hlsl"
		ENDHLSL
	}
	// Screen
	Pass
	{
		Name "LensFlareScreen"
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
		LOD 100
		Blend One OneMinusSrcColor
		BlendOp Max
		ZWrite Off
		Cull Off
		ZTest Always
		HLSLPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma exclude_renderers gles
		#pragma multi_compile_fragment _ FLARE_CIRCLE FLARE_POLYGON
		#pragma multi_compile_fragment _ FLARE_INVERSE_SDF
		#pragma multi_compile _ FLARE_OCCLUSION
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/Runtime/PostProcessing/Shaders/LensFlareCommon.hlsl"
		ENDHLSL
	}
	// Premultiply
	Pass
	{
		Name "LensFlarePremultiply"
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
		LOD 100
		Blend One OneMinusSrcAlpha
		ColorMask RGB
		ZWrite Off
		Cull Off
		ZTest Always
		HLSLPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma exclude_renderers gles
		#pragma multi_compile_fragment _ FLARE_CIRCLE FLARE_POLYGON
		#pragma multi_compile_fragment _ FLARE_INVERSE_SDF
		#pragma multi_compile _ FLARE_OCCLUSION
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/Runtime/PostProcessing/Shaders/LensFlareCommon.hlsl"
		ENDHLSL
	}
	// Lerp
	Pass
	{
		Name "LensFlareLerp"
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		ZWrite Off
		Cull Off
		ZTest Always
		HLSLPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma exclude_renderers gles
		#pragma multi_compile_fragment _ FLARE_CIRCLE FLARE_POLYGON
		#pragma multi_compile_fragment _ FLARE_INVERSE_SDF
		#pragma multi_compile _ FLARE_OCCLUSION
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/Runtime/PostProcessing/Shaders/LensFlareCommon.hlsl"
		ENDHLSL
	}
}
```



### LutBuilderHdr

```c
HLSLINCLUDE
	#pragma exclude_renderers gles
	#pragma multi_compile_local _ _TONEMAP_ACES _TONEMAP_NEUTRAL
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ACES.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	float4 _Lut_Params;         // x: lut_height, y: 0.5 / lut_width, z: 0.5 / lut_height, w: lut_height / lut_height - 1
	float4 _ColorBalance;       // xyz: LMS coeffs, w: unused
	float4 _ColorFilter;        // xyz: color, w: unused
	float4 _ChannelMixerRed;    // xyz: rgb coeffs, w: unused
	float4 _ChannelMixerGreen;  // xyz: rgb coeffs, w: unused
	float4 _ChannelMixerBlue;   // xyz: rgb coeffs, w: unused
	float4 _HueSatCon;          // x: hue shift, y: saturation, z: contrast, w: unused
	float4 _Lift;               // xyz: color, w: unused
	float4 _Gamma;              // xyz: color, w: unused
	float4 _Gain;               // xyz: color, w: unused
	float4 _Shadows;            // xyz: color, w: unused
	float4 _Midtones;           // xyz: color, w: unused
	float4 _Highlights;         // xyz: color, w: unused
	float4 _ShaHiLimits;        // xy: shadows min/max, zw: highlight min/max
	float4 _SplitShadows;       // xyz: color, w: balance
	float4 _SplitHighlights;    // xyz: color, w: unused
	TEXTURE2D(_CurveMaster);
	TEXTURE2D(_CurveRed);
	TEXTURE2D(_CurveGreen);
	TEXTURE2D(_CurveBlue);
	TEXTURE2D(_CurveHueVsHue);
	TEXTURE2D(_CurveHueVsSat);
	TEXTURE2D(_CurveSatVsSat);
	TEXTURE2D(_CurveLumVsSat);
	float EvaluateCurve(TEXTURE2D(curve), float t)
	{
		float x = SAMPLE_TEXTURE2D(curve, sampler_LinearClamp, float2(t, 0.0)).x;
		return saturate(x);
	}
	// Note: when the ACES tonemapper is selected the grading steps will be done using ACES spaces
	float3 ColorGrade(float3 colorLutSpace)
	{
		// Switch back to linear
		float3 colorLinear = LogCToLinear(colorLutSpace);
		// White balance in LMS space
		float3 colorLMS = LinearToLMS(colorLinear);
		colorLMS *= _ColorBalance.xyz;
		colorLinear = LMSToLinear(colorLMS);
		// Do contrast in log after white balance
		#if _TONEMAP_ACES
		float3 colorLog = ACES_to_ACEScc(unity_to_ACES(colorLinear));
		#else
		float3 colorLog = LinearToLogC(colorLinear);
		#endif
		colorLog = (colorLog - ACEScc_MIDGRAY) * _HueSatCon.z + ACEScc_MIDGRAY;
		#if _TONEMAP_ACES
		colorLinear = ACES_to_ACEScg(ACEScc_to_ACES(colorLog));
		#else
		colorLinear = LogCToLinear(colorLog);
		#endif
		// Color filter is just an unclipped multiplier
		colorLinear *= _ColorFilter.xyz;
		// Do NOT feed negative values to the following color ops
		colorLinear = max(0.0, colorLinear);
		// Split toning
		// As counter-intuitive as it is, to make split-toning work the same way it does in Adobe
		// products we have to do all the maths in gamma-space...
		float balance = _SplitShadows.w;
		float3 colorGamma = PositivePow(colorLinear, 1.0 / 2.2);
		float luma = saturate(GetLuminance(saturate(colorGamma)) + balance);
		float3 splitShadows = lerp((0.5).xxx, _SplitShadows.xyz, 1.0 - luma);
		float3 splitHighlights = lerp((0.5).xxx, _SplitHighlights.xyz, luma);
		colorGamma = SoftLight(colorGamma, splitShadows);
		colorGamma = SoftLight(colorGamma, splitHighlights);
		colorLinear = PositivePow(colorGamma, 2.2);
		// Channel mixing (Adobe style)
		colorLinear = float3(
			dot(colorLinear, _ChannelMixerRed.xyz),
			dot(colorLinear, _ChannelMixerGreen.xyz),
			dot(colorLinear, _ChannelMixerBlue.xyz)
		);
		// Shadows, midtones, highlights
		luma = GetLuminance(colorLinear);
		float shadowsFactor = 1.0 - smoothstep(_ShaHiLimits.x, _ShaHiLimits.y, luma);
		float highlightsFactor = smoothstep(_ShaHiLimits.z, _ShaHiLimits.w, luma);
		float midtonesFactor = 1.0 - shadowsFactor - highlightsFactor;
		colorLinear = colorLinear * _Shadows.xyz * shadowsFactor
					+ colorLinear * _Midtones.xyz * midtonesFactor
					+ colorLinear * _Highlights.xyz * highlightsFactor;
		// Lift, gamma, gain
		colorLinear = colorLinear * _Gain.xyz + _Lift.xyz;
		colorLinear = sign(colorLinear) * pow(abs(colorLinear), _Gamma.xyz);
		// HSV operations
		float satMult;
		float3 hsv = RgbToHsv(colorLinear);
		{
			// Hue Vs Sat
			satMult = EvaluateCurve(_CurveHueVsSat, hsv.x) * 2.0;
			// Sat Vs Sat
			satMult *= EvaluateCurve(_CurveSatVsSat, hsv.y) * 2.0;
			// Lum Vs Sat
			satMult *= EvaluateCurve(_CurveLumVsSat, Luminance(colorLinear)) * 2.0;
			// Hue Shift & Hue Vs Hue
			float hue = hsv.x + _HueSatCon.x;
			float offset = EvaluateCurve(_CurveHueVsHue, hue) - 0.5;
			hue += offset;
			hsv.x = RotateHue(hue, 0.0, 1.0);
		}
		colorLinear = HsvToRgb(hsv);
		// Global saturation
		luma = GetLuminance(colorLinear);
		colorLinear = luma.xxx + (_HueSatCon.yyy * satMult) * (colorLinear - luma.xxx);
		// YRGB curves
		// Conceptually these need to be in range [0;1] and from an artist-workflow perspective
		// it's easier to deal with
		colorLinear = FastTonemap(colorLinear);
		{
			const float kHalfPixel = (1.0 / 128.0) / 2.0;
			float3 c = colorLinear;
			// Y (master)
			c += kHalfPixel.xxx;
			float mr = EvaluateCurve(_CurveMaster, c.r);
			float mg = EvaluateCurve(_CurveMaster, c.g);
			float mb = EvaluateCurve(_CurveMaster, c.b);
			c = float3(mr, mg, mb);
			// RGB
			c += kHalfPixel.xxx;
			float r = EvaluateCurve(_CurveRed, c.r);
			float g = EvaluateCurve(_CurveGreen, c.g);
			float b = EvaluateCurve(_CurveBlue, c.b);
			colorLinear = float3(r, g, b);
		}
		colorLinear = FastTonemapInvert(colorLinear);
		colorLinear = max(0.0, colorLinear);
		return colorLinear;
	}
	float3 Tonemap(float3 colorLinear)
	{
		#if _TONEMAP_NEUTRAL
		{
			colorLinear = NeutralTonemap(colorLinear);
		}
		#elif _TONEMAP_ACES
		{
			// Note: input is actually ACEScg (AP1 w/ linear encoding)
			float3 aces = ACEScg_to_ACES(colorLinear);
			colorLinear = AcesTonemap(aces);
		}
		#endif
		return colorLinear;
	}
	float4 Frag(Varyings input) : SV_Target
	{
		// Lut space
		// We use Alexa LogC (El 1000) to store the LUT as it provides a good enough range
		// (~58.85666) and is good enough to be stored in fp16 without losing precision in the
		// darks
		float3 colorLutSpace = GetLutStripValue(input.uv, _Lut_Params);
		// Color grade & tonemap
		float3 gradedColor = ColorGrade(colorLutSpace);
		gradedColor = Tonemap(gradedColor);
		return float4(gradedColor, 1.0);
	}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "LutBuilderHdr"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
		ENDHLSL
	}
}
```



### LutBuilderLdr

```c
HLSLINCLUDE
	#pragma exclude_renderers gles
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	float4 _Lut_Params;         // x: lut_height, y: 0.5 / lut_width, z: 0.5 / lut_height, w: lut_height / lut_height - 1
	float4 _ColorBalance;       // xyz: LMS coeffs, w: unused
	half4 _ColorFilter;         // xyz: color, w: unused
	half4 _ChannelMixerRed;     // xyz: rgb coeffs, w: unused
	half4 _ChannelMixerGreen;   // xyz: rgb coeffs, w: unused
	half4 _ChannelMixerBlue;    // xyz: rgb coeffs, w: unused
	float4 _HueSatCon;          // x: hue shift, y: saturation, z: contrast, w: unused
	float4 _Lift;               // xyz: color, w: unused
	float4 _Gamma;              // xyz: color, w: unused
	float4 _Gain;               // xyz: color, w: unused
	float4 _Shadows;            // xyz: color, w: unused
	float4 _Midtones;           // xyz: color, w: unused
	float4 _Highlights;         // xyz: color, w: unused
	float4 _ShaHiLimits;        // xy: shadows min/max, zw: highlight min/max
	half4 _SplitShadows;        // xyz: color, w: balance
	half4 _SplitHighlights;     // xyz: color, w: unused
	TEXTURE2D(_CurveMaster);
	TEXTURE2D(_CurveRed);
	TEXTURE2D(_CurveGreen);
	TEXTURE2D(_CurveBlue);
	TEXTURE2D(_CurveHueVsHue);
	TEXTURE2D(_CurveHueVsSat);
	TEXTURE2D(_CurveSatVsSat);
	TEXTURE2D(_CurveLumVsSat);
	half EvaluateCurve(TEXTURE2D(curve), float t)
	{
		half x = SAMPLE_TEXTURE2D(curve, sampler_LinearClamp, float2(t, 0.0)).x;
		return saturate(x);
	}
	half4 Frag(Varyings input) : SV_Target
	{
		float3 colorLinear = GetLutStripValue(input.uv, _Lut_Params);
		// White balance in LMS space
		float3 colorLMS = LinearToLMS(colorLinear);
		colorLMS *= _ColorBalance.xyz;
		colorLinear = LMSToLinear(colorLMS);
		// Do contrast in log after white balance
		float3 colorLog = LinearToLogC(colorLinear);
		colorLog = (colorLog - ACEScc_MIDGRAY) * _HueSatCon.z + ACEScc_MIDGRAY;
		colorLinear = LogCToLinear(colorLog);
		// Color filter is just an unclipped multiplier
		colorLinear *= _ColorFilter.xyz;
		// Do NOT feed negative values to the following color ops
		colorLinear = max(0.0, colorLinear);
		// Split toning
		// As counter-intuitive as it is, to make split-toning work the same way it does in Adobe
		// products we have to do all the maths in gamma-space...
		float balance = _SplitShadows.w;
		float3 colorGamma = PositivePow(colorLinear, 1.0 / 2.2);
		float luma = saturate(GetLuminance(saturate(colorGamma)) + balance);
		float3 splitShadows = lerp((0.5).xxx, _SplitShadows.xyz, 1.0 - luma);
		float3 splitHighlights = lerp((0.5).xxx, _SplitHighlights.xyz, luma);
		colorGamma = SoftLight(colorGamma, splitShadows);
		colorGamma = SoftLight(colorGamma, splitHighlights);
		colorLinear = PositivePow(colorGamma, 2.2);
		// Channel mixing (Adobe style)
		colorLinear = float3(
			dot(colorLinear, _ChannelMixerRed.xyz),
			dot(colorLinear, _ChannelMixerGreen.xyz),
			dot(colorLinear, _ChannelMixerBlue.xyz)
		);
		// Shadows, midtones, highlights
		luma = GetLuminance(colorLinear);
		float shadowsFactor = 1.0 - smoothstep(_ShaHiLimits.x, _ShaHiLimits.y, luma);
		float highlightsFactor = smoothstep(_ShaHiLimits.z, _ShaHiLimits.w, luma);
		float midtonesFactor = 1.0 - shadowsFactor - highlightsFactor;
		colorLinear = colorLinear * _Shadows.xyz * shadowsFactor
			+ colorLinear * _Midtones.xyz * midtonesFactor
			+ colorLinear * _Highlights.xyz * highlightsFactor;
		// Lift, gamma, gain
		colorLinear = colorLinear * _Gain.xyz + _Lift.xyz;
		colorLinear = sign(colorLinear) * pow(abs(colorLinear), _Gamma.xyz);
		// HSV operations
		float satMult;
		float3 hsv = RgbToHsv(colorLinear);
		{
			// Hue Vs Sat
			satMult = EvaluateCurve(_CurveHueVsSat, hsv.x) * 2.0;
			// Sat Vs Sat
			satMult *= EvaluateCurve(_CurveSatVsSat, hsv.y) * 2.0;
			// Lum Vs Sat
			satMult *= EvaluateCurve(_CurveLumVsSat, Luminance(colorLinear)) * 2.0;
			// Hue Shift & Hue Vs Hue
			float hue = hsv.x + _HueSatCon.x;
			float offset = EvaluateCurve(_CurveHueVsHue, hue) - 0.5;
			hue += offset;
			hsv.x = RotateHue(hue, 0.0, 1.0);
		}
		colorLinear = HsvToRgb(hsv);
		// Global saturation
		luma = GetLuminance(colorLinear);
		colorLinear = luma.xxx + (_HueSatCon.yyy * satMult) * (colorLinear - luma.xxx);
		// YRGB curves
		{
			const float kHalfPixel = (1.0 / 128.0) / 2.0;
			float3 c = colorLinear;
			// Y (master)
			c += kHalfPixel.xxx;
			float mr = EvaluateCurve(_CurveMaster, c.r);
			float mg = EvaluateCurve(_CurveMaster, c.g);
			float mb = EvaluateCurve(_CurveMaster, c.b);
			c = float3(mr, mg, mb);
			// RGB
			c += kHalfPixel.xxx;
			float r = EvaluateCurve(_CurveRed, c.r);
			float g = EvaluateCurve(_CurveGreen, c.g);
			float b = EvaluateCurve(_CurveBlue, c.b);
			colorLinear = float3(r, g, b);
		}
		return half4(saturate(colorLinear), 1.0);
	}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "LutBuilderLdr"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
		ENDHLSL
	}
}
```



### PaniniProjection

```c
HLSLINCLUDE
#pragma exclude_renderers gles
#pragma multi_compile_local _GENERIC _UNIT_DISTANCE
#pragma multi_compile _ _USE_DRAW_PROCEDURAL
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
TEXTURE2D_X(_SourceTex);
float4 _Params;
// Back-ported & adapted from the work of the Stockholm demo team - thanks Lasse
float2 Panini_UnitDistance(float2 view_pos)
{
	// Given
	//    S----------- E--X-------
	//    |      ` .  /,´
	//    |-- ---    Q
	//  1 |       ,´/  `
	//    |     ,´ /    ´
	//    |   ,´  /      `
	//    | ,´   /       .
	//    O`    /        .
	//    |    /         `
	//    |   /         ´
	//  1 |  /         ´
	//    | /        ´
	//    |/_  .  ´
	//    P
	//
	// Have E
	// Want to find X
	//
	// First apply tangent-secant theorem to find Q
	//   PE*QE = SE*SE
	//   QE = PE-PQ
	//   PQ = PE-(SE*SE)/PE
	//   Q = E*(PQ/PE)
	// Then project Q to find X
	const float d = 1.0;
	const float view_dist = 2.0;
	const float view_dist_sq = 4.0;
	float view_hyp = sqrt(view_pos.x * view_pos.x + view_dist_sq);
	float cyl_hyp = view_hyp - (view_pos.x * view_pos.x) / view_hyp;
	float cyl_hyp_frac = cyl_hyp / view_hyp;
	float cyl_dist = view_dist * cyl_hyp_frac;
	float2 cyl_pos = view_pos * cyl_hyp_frac;
	return cyl_pos / (cyl_dist - d);
}
float2 Panini_Generic(float2 view_pos, float d)
{
	// Given
	//    S----------- E--X-------
	//    |    `  ~.  /,´
	//    |-- ---    Q
	//    |        ,/    `
	//  1 |      ,´/       `
	//    |    ,´ /         ´
	//    |  ,´  /           ´
	//    |,`   /             ,
	//    O    /
	//    |   /               ,
	//  d |  /
	//    | /                ,
	//    |/                .
	//    P
	//    |              ´
	//    |         , ´
	//    +-    ´
	//
	// Have E
	// Want to find X
	//
	// First compute line-circle intersection to find Q
	// Then project Q to find X
	float view_dist = 1.0 + d;
	float view_hyp_sq = view_pos.x * view_pos.x + view_dist * view_dist;
	float isect_D = view_pos.x * d;
	float isect_discrim = view_hyp_sq - isect_D * isect_D;
	float cyl_dist_minus_d = (-isect_D * view_pos.x + view_dist * sqrt(isect_discrim)) / view_hyp_sq;
	float cyl_dist = cyl_dist_minus_d + d;
	float2 cyl_pos = view_pos * (cyl_dist / view_dist);
	return cyl_pos / (cyl_dist - d);
}
half4 Frag(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	#if _GENERIC
	float2 proj_pos = Panini_Generic((2.0 * input.uv - 1.0) * _Params.xy * _Params.w, _Params.z);
	#else // _UNIT_DISTANCE
	float2 proj_pos = Panini_UnitDistance((2.0 * input.uv - 1.0) * _Params.xy * _Params.w);
	#endif
	float2 proj_ndc = proj_pos / _Params.xy;
	float2 coords = proj_ndc * 0.5 + 0.5;
	return SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, coords);
}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "Panini Projection"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
		ENDHLSL
	}
}
```



### ScalingSetup

```c
HLSLINCLUDE
#pragma multi_compile_local_fragment _ _FXAA
#pragma multi_compile_vertex _ _USE_DRAW_PROCEDURAL
#pragma multi_compile_local_fragment _ _GAMMA_20
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
TEXTURE2D_X(_SourceTex);
float4 _SourceSize;
half4 Frag(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	float2 positionNDC = uv;
	int2   positionSS = uv * _SourceSize.xy;
	half3 color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_PointClamp, uv).xyz;
	#if _FXAA
	color = ApplyFXAA(color, positionNDC, positionSS, _SourceSize, _SourceTex);
	#endif
	#if _GAMMA_20 && !UNITY_COLORSPACE_GAMMA
	// EASU expects perceptually encoded color data so either encode to gamma 2.0 here if the input
	// data is linear, or let it pass through unchanged if it's already gamma encoded.
	color = LinearToGamma20(color);
	#endif
	return half4(color, 1.0);
}
ENDHLSL
///
/// Scaling Setup Shader
///
/// This shader is used to perform any operations that need to place before image scaling occurs.
/// It is not expected to be executed unless image scaling is active.
///
/// Supported Operations:
///
/// FXAA
/// The FXAA shader does not support mismatched input and output dimensions so it must be run before any image
/// scaling takes place.
///
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "ScalingSetup"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
		ENDHLSL
	}
}
```



### StopNaN

```c
HLSLINCLUDE
#pragma exclude_renderers gles
#pragma multi_compile _ _USE_DRAW_PROCEDURAL
#pragma exclude_renderers gles
#pragma target 3.5
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
#define NAN_COLOR half3(0.0, 0.0, 0.0)
TEXTURE2D_X(_SourceTex);
half4 Frag(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	half3 color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_PointClamp, UnityStereoTransformScreenSpaceTex(input.uv)).xyz;
	if (AnyIsNaN(color) || AnyIsInf(color))
		color = NAN_COLOR;
	return half4(color, 1.0);
}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "Stop NaN"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
		ENDHLSL
	}
}
```



### SubpixelMorphologicalAntialiasing

```c
Properties
{
	[HideInInspector] _StencilRef ("_StencilRef", Int) = 64
	[HideInInspector] _StencilMask ("_StencilMask", Int) = 64
}
HLSLINCLUDE
	#pragma multi_compile_local _SMAA_PRESET_LOW _SMAA_PRESET_MEDIUM _SMAA_PRESET_HIGH
	#pragma multi_compile _ _USE_DRAW_PROCEDURAL
	#pragma exclude_renderers gles
ENDHLSL
SubShader
{
	Cull Off ZWrite Off ZTest Always
	// Edge detection
	Pass
	{
		Stencil
		{
			WriteMask [_StencilMask]
			Ref [_StencilRef]
			Comp Always
			Pass Replace
		}
		HLSLPROGRAM
			#pragma vertex VertEdge
			#pragma fragment FragEdge
			#include "SubpixelMorphologicalAntialiasingBridge.hlsl"
		ENDHLSL
	}
	// Blend Weights Calculation
	Pass
	{
		Stencil
		{
			WriteMask [_StencilMask]
			ReadMask [_StencilMask]
			Ref [_StencilRef]
			Comp Equal
			Pass Replace
		}
		HLSLPROGRAM
			#pragma vertex VertBlend
			#pragma fragment FragBlend
			#include "SubpixelMorphologicalAntialiasingBridge.hlsl"
		ENDHLSL
	}
	// Neighborhood Blending
	Pass
	{
		HLSLPROGRAM
			#pragma vertex VertNeighbor
			#pragma fragment FragNeighbor
			#include "SubpixelMorphologicalAntialiasingBridge.hlsl"
		ENDHLSL
	}
}
```



### UberPost

```c
HLSLINCLUDE
#pragma exclude_renderers gles
#pragma multi_compile_local_fragment _ _DISTORTION
#pragma multi_compile_local_fragment _ _CHROMATIC_ABERRATION
#pragma multi_compile_local_fragment _ _BLOOM_LQ _BLOOM_HQ _BLOOM_LQ_DIRT _BLOOM_HQ_DIRT
#pragma multi_compile_local_fragment _ _HDR_GRADING _TONEMAP_ACES _TONEMAP_NEUTRAL
#pragma multi_compile_local_fragment _ _FILM_GRAIN
#pragma multi_compile_local_fragment _ _DITHERING
#pragma multi_compile_local_fragment _ _GAMMA_20 _LINEAR_TO_SRGB_CONVERSION
#pragma multi_compile_local_fragment _ _USE_FAST_SRGB_LINEAR_CONVERSION
#pragma multi_compile _ _USE_DRAW_PROCEDURAL
#pragma multi_compile_fragment _ DEBUG_DISPLAY
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingFullscreen.hlsl"
// Hardcoded dependencies to reduce the number of variants
#if _BLOOM_LQ || _BLOOM_HQ || _BLOOM_LQ_DIRT || _BLOOM_HQ_DIRT
	#define BLOOM
	#if _BLOOM_LQ_DIRT || _BLOOM_HQ_DIRT
		#define BLOOM_DIRT
	#endif
#endif
TEXTURE2D_X(_SourceTex);
TEXTURE2D_X(_Bloom_Texture);
TEXTURE2D(_LensDirt_Texture);
TEXTURE2D(_Grain_Texture);
TEXTURE2D(_InternalLut);
TEXTURE2D(_UserLut);
TEXTURE2D(_BlueNoise_Texture);
float4 _Lut_Params;
float4 _UserLut_Params;
float4 _Bloom_Params;
float _Bloom_RGBM;
float4 _LensDirt_Params;
float _LensDirt_Intensity;
float4 _Distortion_Params1;
float4 _Distortion_Params2;
float _Chroma_Params;
half4 _Vignette_Params1;
float4 _Vignette_Params2;
float2 _Grain_Params;
float4 _Grain_TilingParams;
float4 _Bloom_Texture_TexelSize;
float4 _Dithering_Params;
#define DistCenter              _Distortion_Params1.xy
#define DistAxis                _Distortion_Params1.zw
#define DistTheta               _Distortion_Params2.x
#define DistSigma               _Distortion_Params2.y
#define DistScale               _Distortion_Params2.z
#define DistIntensity           _Distortion_Params2.w
#define ChromaAmount            _Chroma_Params.x
#define BloomIntensity          _Bloom_Params.x
#define BloomTint               _Bloom_Params.yzw
#define BloomRGBM               _Bloom_RGBM.x
#define LensDirtScale           _LensDirt_Params.xy
#define LensDirtOffset          _LensDirt_Params.zw
#define LensDirtIntensity       _LensDirt_Intensity.x
#define VignetteColor           _Vignette_Params1.xyz
#define VignetteCenter          _Vignette_Params2.xy
#define VignetteIntensity       _Vignette_Params2.z
#define VignetteSmoothness      _Vignette_Params2.w
#define VignetteRoundness       _Vignette_Params1.w
#define LutParams               _Lut_Params.xyz
#define PostExposure            _Lut_Params.w
#define UserLutParams           _UserLut_Params.xyz
#define UserLutContribution     _UserLut_Params.w
#define GrainIntensity          _Grain_Params.x
#define GrainResponse           _Grain_Params.y
#define GrainScale              _Grain_TilingParams.xy
#define GrainOffset             _Grain_TilingParams.zw
#define DitheringScale          _Dithering_Params.xy
#define DitheringOffset         _Dithering_Params.zw
float2 DistortUV(float2 uv)
{
	// Note: this variant should never be set with XR
	#if _DISTORTION
	{
		uv = (uv - 0.5) * DistScale + 0.5;
		float2 ruv = DistAxis * (uv - 0.5 - DistCenter);
		float ru = length(float2(ruv));
		UNITY_BRANCH
		if (DistIntensity > 0.0)
		{
			float wu = ru * DistTheta;
			ru = tan(wu) * (rcp(ru * DistSigma));
			uv = uv + ruv * (ru - 1.0);
		}
		else
		{
			ru = rcp(ru) * DistTheta * atan(ru * DistSigma);
			uv = uv + ruv * (ru - 1.0);
		}
	}
	#endif
	return uv;
}
half4 Frag(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
	float2 uvDistorted = DistortUV(uv);
	half3 color = (0.0).xxx;
	#if _CHROMATIC_ABERRATION
	{
		// Very fast version of chromatic aberration from HDRP using 3 samples and hardcoded
		// spectral lut. Performs significantly better on lower end GPUs.
		float2 coords = 2.0 * uv - 1.0;
		float2 end = uv - coords * dot(coords, coords) * ChromaAmount;
		float2 delta = (end - uv) / 3.0;
		half r = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uvDistorted                ).x;
		half g = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, DistortUV(delta + uv)      ).y;
		half b = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, DistortUV(delta * 2.0 + uv)).z;
		color = half3(r, g, b);
	}
	#else
	{
		color = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uvDistorted).xyz;
	}
	#endif
	// Gamma space... Just do the rest of Uber in linear and convert back to sRGB at the end
	#if UNITY_COLORSPACE_GAMMA
	{
		color = GetSRGBToLinear(color);
	}
	#endif
	#if defined(BLOOM)
	{
		#if _BLOOM_HQ && !defined(SHADER_API_GLES)
		half4 bloom = SampleTexture2DBicubic(TEXTURE2D_X_ARGS(_Bloom_Texture, sampler_LinearClamp), uvDistorted, _Bloom_Texture_TexelSize.zwxy, (1.0).xx, unity_StereoEyeIndex);
		#else
		half4 bloom = SAMPLE_TEXTURE2D_X(_Bloom_Texture, sampler_LinearClamp, uvDistorted);
		#endif
		#if UNITY_COLORSPACE_GAMMA
		bloom.xyz *= bloom.xyz; // γ to linear
		#endif
		UNITY_BRANCH
		if (BloomRGBM > 0)
		{
			bloom.xyz = DecodeRGBM(bloom);
		}
		bloom.xyz *= BloomIntensity;
		color += bloom.xyz * BloomTint;
		#if defined(BLOOM_DIRT)
		{
			// UVs for the dirt texture should be DistortUV(uv * DirtScale + DirtOffset) but
			// considering we use a cover-style scale on the dirt texture the difference
			// isn't massive so we chose to save a few ALUs here instead in case lens
			// distortion is active.
			half3 dirt = SAMPLE_TEXTURE2D(_LensDirt_Texture, sampler_LinearClamp, uvDistorted * LensDirtScale + LensDirtOffset).xyz;
			dirt *= LensDirtIntensity;
			color += dirt * bloom.xyz;
		}
		#endif
	}
	#endif
	// To save on variants we'll use an uniform branch for vignette. Lower end platforms
	// don't like these but if we're running Uber it means we're running more expensive
	// effects anyway. Lower-end devices would limit themselves to on-tile compatible effect
	// and thus this shouldn't too much of a problem (famous last words).
	UNITY_BRANCH
	if (VignetteIntensity > 0)
	{
		color = ApplyVignette(color, uvDistorted, VignetteCenter, VignetteIntensity, VignetteRoundness, VignetteSmoothness, VignetteColor);
	}
	// Color grading is always enabled when post-processing/uber is active
	{
		color = ApplyColorGrading(color, PostExposure, TEXTURE2D_ARGS(_InternalLut, sampler_LinearClamp), LutParams, TEXTURE2D_ARGS(_UserLut, sampler_LinearClamp), UserLutParams, UserLutContribution);
	}
	#if _FILM_GRAIN
	{
		color = ApplyGrain(color, uv, TEXTURE2D_ARGS(_Grain_Texture, sampler_LinearRepeat), GrainIntensity, GrainResponse, GrainScale, GrainOffset);
	}
	#endif
	// When Unity is configured to use gamma color encoding, we ignore the request to convert to gamma 2.0 and instead fall back to sRGB encoding
	#if _GAMMA_20 && !UNITY_COLORSPACE_GAMMA
	{
		color = LinearToGamma20(color);
	}
	// Back to sRGB
	#elif UNITY_COLORSPACE_GAMMA || _LINEAR_TO_SRGB_CONVERSION
	{
		color = GetLinearToSRGB(color);
	}
	#endif
	#if _DITHERING
	{
		color = ApplyDithering(color, uv, TEXTURE2D_ARGS(_BlueNoise_Texture, sampler_PointRepeat), DitheringScale, DitheringOffset);
		// Assume color > 0 and prevent 0 - ditherNoise.
		// Negative colors can cause problems if fed back to the postprocess via render to FP16 texture.
		color = max(color, 0);
	}
	#endif
	#if defined(DEBUG_DISPLAY)
	half4 debugColor = 0;
	if(CanDebugOverrideOutputColor(half4(color, 1), uv, debugColor))
	{
		return debugColor;
	}
	#endif
	return half4(color, 1.0);
}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	ZTest Always ZWrite Off Cull Off
	Pass
	{
		Name "UberPost"
		HLSLPROGRAM
			#pragma vertex FullscreenVert
			#pragma fragment Frag
		ENDHLSL
	}
}
```



## Terrain
### TerrainDetailLitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

TEXTURE2D(_MainTex);       SAMPLER(sampler_MainTex);
float4 _MainTex_ST;
```



### TerrainDetailLitPasses

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
struct Attributes
{
    float4  PositionOS  : POSITION;
    float2  UV0         : TEXCOORD0;
    float2  UV1         : TEXCOORD1;
    float3  NormalOS    : NORMAL;
    half4   Color       : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float2  UV01            : TEXCOORD0; // UV0
    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 1);
    half4   Color           : TEXCOORD2; // Vertex Color
    half4   LightingFog     : TEXCOORD3; // Vertex Lighting, Fog Factor
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4  ShadowCoords    : TEXCOORD4; // Shadow UVs
    #endif
    half4   NormalWS        : TEXCOORD5;
    float3  PositionWS      : TEXCOORD6;
    float4  PositionCS      : SV_POSITION; // Clip Position
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings input, out InputData inputData)
void InitializeSurfaceData(half3 albedo, half alpha, out SurfaceData surfaceData)
half4 UniversalTerrainLit(InputData inputData, SurfaceData surfaceData)
half4 UniversalTerrainLit(InputData inputData, half3 albedo, half alpha)
Varyings TerrainLitVertex(Attributes input)
half4 TerrainLitForwardFragment(Varyings input) : SV_Target
FragmentOutput TerrainLitGBufferFragment(Varyings input)
```



### TerrainLitDepthNormalsPass

```c
#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
// DepthNormal pass
struct AttributesDepthNormal
{
    float4 positionOS : POSITION;
    half3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct VaryingsDepthNormal
{
    float4 uvMainAndLM            : TEXCOORD0; // xy: control, zw: lightmap
    #ifndef TERRAIN_SPLAT_BASEPASS
	float4 uvSplat01              : TEXCOORD1; // xy: splat0, zw: splat1
	float4 uvSplat23              : TEXCOORD2; // xy: splat2, zw: splat3
    #endif
    #if defined(_NORMALMAP) && !defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
	half4 normal             : TEXCOORD3;    // xyz: normal, w: viewDir.x
	half4 tangent            : TEXCOORD4;    // xyz: tangent, w: viewDir.y
	half4 bitangent          : TEXCOORD5;    // xyz: bitangent, w: viewDir.z
    #else
	half3 normal             : TEXCOORD3;
    #endif
    float4 clipPos           : SV_POSITION;
    UNITY_VERTEX_OUTPUT_STEREO
};
VaryingsDepthNormal DepthNormalOnlyVertex(AttributesDepthNormal v)
half4 DepthNormalOnlyFragment(VaryingsDepthNormal IN) : SV_TARGET
```



### TerrainLitInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
CBUFFER_START(UnityPerMaterial)
    float4 _MainTex_ST;
    half4 _BaseColor;
    half _Cutoff;
CBUFFER_END
#define _Surface 0.0 // Terrain is always opaque
CBUFFER_START(_Terrain)
    half _NormalScale0, _NormalScale1, _NormalScale2, _NormalScale3;
    half _Metallic0, _Metallic1, _Metallic2, _Metallic3;
    half _Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3;
    half4 _DiffuseRemapScale0, _DiffuseRemapScale1, _DiffuseRemapScale2, _DiffuseRemapScale3;
    half4 _MaskMapRemapOffset0, _MaskMapRemapOffset1, _MaskMapRemapOffset2, _MaskMapRemapOffset3;
    half4 _MaskMapRemapScale0, _MaskMapRemapScale1, _MaskMapRemapScale2, _MaskMapRemapScale3;
    float4 _Control_ST;
    float4 _Control_TexelSize;
    half _DiffuseHasAlpha0, _DiffuseHasAlpha1, _DiffuseHasAlpha2, _DiffuseHasAlpha3;
    half _LayerHasMask0, _LayerHasMask1, _LayerHasMask2, _LayerHasMask3;
    half4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;
    half _HeightTransition;
    half _NumLayersCount;
    #ifdef UNITY_INSTANCING_ENABLED
    float4 _TerrainHeightmapRecipSize;   // float4(1.0f/width, 1.0f/height, 1.0f/(width-1), 1.0f/(height-1))
    float4 _TerrainHeightmapScale;       // float4(hmScale.x, hmScale.y / (float)(kMaxHeight), hmScale.z, 0.0f)
    #endif
    #ifdef SCENESELECTIONPASS
    int _ObjectId;
    int _PassValue;
    #endif
CBUFFER_END
TEXTURE2D(_Control);    SAMPLER(sampler_Control);
TEXTURE2D(_Splat0);     SAMPLER(sampler_Splat0);
TEXTURE2D(_Splat1);
TEXTURE2D(_Splat2);
TEXTURE2D(_Splat3);
#ifdef _NORMALMAP
TEXTURE2D(_Normal0);     SAMPLER(sampler_Normal0);
TEXTURE2D(_Normal1);
TEXTURE2D(_Normal2);
TEXTURE2D(_Normal3);
#endif
#ifdef _MASKMAP
TEXTURE2D(_Mask0);      SAMPLER(sampler_Mask0);
TEXTURE2D(_Mask1);
TEXTURE2D(_Mask2);
TEXTURE2D(_Mask3);
#endif
TEXTURE2D(_MainTex);       SAMPLER(sampler_MainTex);
TEXTURE2D(_SpecGlossMap);  SAMPLER(sampler_SpecGlossMap);
TEXTURE2D(_MetallicTex);   SAMPLER(sampler_MetallicTex);
#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
#define ENABLE_TERRAIN_PERPIXEL_NORMAL
#endif
#ifdef UNITY_INSTANCING_ENABLED
TEXTURE2D(_TerrainHeightmapTexture);
TEXTURE2D(_TerrainNormalmapTexture);
SAMPLER(sampler_TerrainNormalmapTexture);
#endif
UNITY_INSTANCING_BUFFER_START(Terrain)
UNITY_DEFINE_INSTANCED_PROP(float4, _TerrainPatchInstanceData)  // float4(xBase, yBase, skipScale, ~)
UNITY_INSTANCING_BUFFER_END(Terrain)
#ifdef _ALPHATEST_ON
TEXTURE2D(_TerrainHolesTexture);
SAMPLER(sampler_TerrainHolesTexture);
void ClipHoles(float2 uv)
#endif
half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
void TerrainInstancing(inout float4 positionOS, inout float3 normal, inout float2 uv)
void TerrainInstancing(inout float4 positionOS, inout float3 normal)
```



### TerrainLitMetaPass

```c
#include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"
Varyings TerrainVertexMeta(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);
    TerrainInstancing(input.positionOS, input.normalOS, input.uv0);
    // For some reason, uv1 and uv2 are not populated for instanced terrain. Use uv0.
    input.uv1 = input.uv2 = input.uv0;
    output = UniversalVertexMeta(input);
    return output;
}
half4 TerrainFragmentMeta(Varyings input) : SV_Target
{
	#ifdef _ALPHATEST_ON
    ClipHoles(input.uv);
	#endif
    return UniversalFragmentMetaLit(input);
}
```



### TerrainLitPasses

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float4 uvMainAndLM              : TEXCOORD0; // xy: control, zw: lightmap
    #ifndef TERRAIN_SPLAT_BASEPASS
        float4 uvSplat01                : TEXCOORD1; // xy: splat0, zw: splat1
        float4 uvSplat23                : TEXCOORD2; // xy: splat2, zw: splat3
    #endif
    #if defined(_NORMALMAP) && !defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
        half4 normal                    : TEXCOORD3;    // xyz: normal, w: viewDir.x
        half4 tangent                   : TEXCOORD4;    // xyz: tangent, w: viewDir.y
        half4 bitangent                 : TEXCOORD5;    // xyz: bitangent, w: viewDir.z
    #else
        half3 normal                    : TEXCOORD3;
        half3 vertexSH                  : TEXCOORD4; // SH
    #endif
    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light
    #else
        half  fogFactor                 : TEXCOORD6;
    #endif
    float3 positionWS               : TEXCOORD7;
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        float4 shadowCoord              : TEXCOORD8;
    #endif
	#if defined(DYNAMICLIGHTMAP_ON)
    float2 dynamicLightmapUV        : TEXCOORD9;
	#endif
    float4 clipPos                  : SV_POSITION;
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(Varyings IN, half3 normalTS, out InputData inputData)
void InitializeInputData(Varyings IN, half3 normalTS, out InputData inputData)
#ifndef TERRAIN_SPLAT_BASEPASS
void NormalMapMix(float4 uvSplat01, float4 uvSplat23, inout half4 splatControl, inout half3 mixedNormal)
void SplatmapMix(float4 uvMainAndLM, float4 uvSplat01, float4 uvSplat23, inout half4 splatControl, out half weight, out half4 mixedDiffuse, out half4 defaultSmoothness, inout half3 mixedNormal)
#endif
#ifdef _TERRAIN_BLEND_HEIGHT
void HeightBasedSplatModify(inout half4 splatControl, in half4 masks[4])
#endif
void SplatmapFinalColor(inout half4 color, half fogCoord)
Varyings SplatmapVert(Attributes v)
void ComputeMasks(out half4 masks[4], half4 hasMask, Varyings IN)
// Used in Standard Terrain shader
#ifdef TERRAIN_GBUFFER
FragmentOutput SplatmapFragment(Varyings IN)
#else
half4 SplatmapFragment(Varyings IN) : SV_TARGET
#endif
float3 _LightDirection;
float3 _LightPosition;
struct AttributesLean
{
    float4 position     : POSITION;
    float3 normalOS       : NORMAL;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct VaryingsLean
{
    float4 clipPos      : SV_POSITION;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};
VaryingsLean ShadowPassVertex(AttributesLean v)
half4 ShadowPassFragment(VaryingsLean IN) : SV_TARGET
// Depth pass
VaryingsLean DepthOnlyVertex(AttributesLean v)
half4 DepthOnlyFragment(VaryingsLean IN) : SV_TARGET
```



### WavingGrassDepthNormalsPass

```c
struct GrassVertexDepthNormalInput
{
    float4 vertex       : POSITION;
    float3 normal       : NORMAL;
    float4 tangent      : TANGENT;
    half4 color         : COLOR;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct GrassVertexDepthNormalOutput
{
    float2 uv           : TEXCOORD0;
    half3 normal        : TEXCOORD1;
    half4 color         : TEXCOORD2;
    float3 viewDirWS    : TEXCOORD3;
    float4 clipPos      : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
GrassVertexDepthNormalOutput DepthNormalOnlyVertex(GrassVertexDepthNormalInput v)
half4 DepthNormalOnlyFragment(GrassVertexDepthNormalOutput input) : SV_TARGET
```



### WavingGrassInput

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
CBUFFER_START(UnityPerMaterial)
    float4 _MainTex_ST;
    half4 _BaseColor;
    half4 _SpecColor;
    half4 _EmissionColor;
    half _Cutoff;
    half _Shininess;
CBUFFER_END
#define _Surface 0.0 // Grass is always opaque
// Terrain engine shader helpers
CBUFFER_START(TerrainGrass)
    half4 _WavingTint;
    float4 _WaveAndDistance;    // wind speed, wave size, wind amount, max sqr distance
    float4 _CameraPosition;     // .xyz = camera position, .w = 1 / (max sqr distance)
    float3 _CameraRight, _CameraUp;
CBUFFER_END
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;
float4 _MainTex_MipInfo;
// ---- Grass helpers
// Calculate a 4 fast sine-cosine pairs
// val:     the 4 input values - each must be in the range (0 to 1)
// s:       The sine of each of the 4 values
// c:       The cosine of each of the 4 values
void FastSinCos (float4 val, out float4 s, out float4 c) ;
    float4 cos8  = ;
    // sin
    s =  val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
    // cos
    c = 1 + r5 * cos8.x + r6 * cos8.y + r7 * cos8.z + r8 * cos8.w;
}
half4 TerrainWaveGrass (inout float4 vertex, float waveAmount, half4 color)
void TerrainBillboardGrass( inout float4 pos, float2 offset )
```



### WavingGrassPasses

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

struct GrassVertexInput
{
    float4 vertex       : POSITION;
    float3 normal       : NORMAL;
    float4 tangent      : TANGENT;
    half4 color         : COLOR;
    float2 texcoord     : TEXCOORD0;
    float2 lightmapUV   : TEXCOORD1;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct GrassVertexOutput
{
    float2 uv                       : TEXCOORD0;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

    float4 posWSShininess : TEXCOORD2;// xyz: posWS, w: Shininess * 128

    half3  normal         : TEXCOORD3;
    half3 viewDir         : TEXCOORD4;
	// x: fogFactor, yzw: vertex light
    half4 fogFactorAndVertexLight   : TEXCOORD5; 

	#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord              : TEXCOORD6;
	#endif
    half4 color                     : TEXCOORD7;

    float4 clipPos                  : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeInputData(GrassVertexOutput input, out InputData inputData)
void InitializeVertData(GrassVertexInput input, inout GrassVertexOutput vertData)
///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////
// Grass: appdata_full usage
// color        - .xyz = color, .w = wave scale
// normal       - normal
// tangent.xy   - billboard extrusion
// texcoord     - UV coords
// texcoord1    - 2nd UV coords
GrassVertexOutput WavingGrassVert(GrassVertexInput v)
GrassVertexOutput WavingGrassBillboardVert(GrassVertexInput v)
inline void InitializeSimpleLitSurfaceData(GrassVertexOutput input, out SurfaceData outSurfaceData)
// Used for StandardSimpleLighting shader
#ifdef TERRAIN_GBUFFER
FragmentOutput LitPassFragmentGrass(GrassVertexOutput input)
#else
half4 LitPassFragmentGrass(GrassVertexOutput input) : SV_Target
#endif
struct GrassVertexDepthOnlyInput
{
    float4 vertex       : POSITION;
    float4 tangent      : TANGENT;
    half4 color         : COLOR;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct GrassVertexDepthOnlyOutput
{
    float2 uv           : TEXCOORD0;
    half4 color         : TEXCOORD1;
    float4 clipPos      : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
void InitializeVertData(GrassVertexDepthOnlyInput input, inout GrassVertexDepthOnlyOutput vertData)
GrassVertexDepthOnlyOutput DepthOnlyVertex(GrassVertexDepthOnlyInput v)
half4 DepthOnlyFragment(GrassVertexDepthOnlyOutput input) : SV_TARGET
```



## Terrain(Shader)
### TerrainDetailLit

```c
Properties
{
	_MainTex ("Main Texture", 2D) = "white" {  }
}
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Unlit" "IgnoreProjector" = "True"}
	LOD 100
	ZWrite On
	// Lightmapped
	Pass
	{
		Name "TerrainDetailVertex"
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile _ DEBUG_DISPLAY
		#pragma vertex TerrainLitVertex
		#pragma fragment TerrainLitForwardFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainDetailLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainDetailLitPasses.hlsl"
		ENDHLSL
	}
	// GBuffer
	Pass
	{
		Name "TerrainDetailVertex - GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma target 2.0
		#pragma vertex Vert
		#pragma fragment Frag
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX //_ADDITIONAL_LIGHTS
		//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
		TEXTURE2D(_MainTex);       SAMPLER(sampler_MainTex);
		float4 _MainTex_ST;
		struct Attributes
		{
			float4  PositionOS  : POSITION;
			float2  UV0         : TEXCOORD0;
			float2  UV1         : TEXCOORD1;
			half3   NormalOS    : NORMAL;
			half4   Color       : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct Varyings
		{
			float2  UV01            : TEXCOORD0; // UV0
			DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 1);
			half4   Color           : TEXCOORD2; // Vertex Color
			half4   LightingFog     : TEXCOORD3; // Vetex Lighting, Fog Factor
			float4  ShadowCoords    : TEXCOORD4; // Shadow UVs
			half3   NormalWS        : TEXCOORD5; // World Space Normal
			float4  PositionCS      : SV_POSITION; // Clip Position
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};
		Varyings Vert(Attributes input)
		{
			Varyings output = (Varyings)0;
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_TRANSFER_INSTANCE_ID(input, output);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
			// Vertex attributes
			output.UV01 = TRANSFORM_TEX(input.UV0, _MainTex);
			OUTPUT_LIGHTMAP_UV(input.UV1, unity_LightmapST, output.staticLightmapUV);
			VertexPositionInputs vertexInput = GetVertexPositionInputs(input.PositionOS.xyz);
			output.Color = input.Color;
			output.PositionCS = vertexInput.positionCS;
			// Shadow Coords
			output.ShadowCoords = GetShadowCoord(vertexInput);
			// Vertex Lighting
			output.NormalWS = TransformObjectToWorldNormal(input.NormalOS).xyz;
			OUTPUT_SH(output.NormalWS, output.vertexSH);
			Light mainLight = GetMainLight();
			half3 attenuatedLightColor = mainLight.color * mainLight.distanceAttenuation;
			half3 diffuseColor = LightingLambert(attenuatedLightColor, mainLight.direction, output.NormalWS);
		#ifdef _ADDITIONAL_LIGHTS
			int pixelLightCount = GetAdditionalLightsCount();
			for (int i = 0; i < pixelLightCount; ++i)
			{
				Light light = GetAdditionalLight(i, vertexInput.positionWS);
				half3 attenuatedLightColor = light.color * light.distanceAttenuation;
				diffuseColor += LightingLambert(attenuatedLightColor, light.direction, output.NormalWS);
			}
		#endif
			output.LightingFog.xyz = diffuseColor;
			// Fog factor
			output.LightingFog.w = ComputeFogFactor(output.PositionCS.z);
			return output;
		}
		FragmentOutput Frag(Varyings input)
		{
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
			half3 bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, input.NormalWS);
			half3 lighting = input.LightingFog.rgb * MainLightRealtimeShadow(input.ShadowCoords) + bakedGI;
			half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.UV01);
			half4 color = 1.0;
			color.rgb = input.Color.rgb * tex.rgb * lighting;
			SurfaceData surfaceData = (SurfaceData)0;
			surfaceData.alpha = 1.0;
			surfaceData.occlusion = 1.0;
			InputData inputData = (InputData)0;
			inputData.normalWS = input.NormalWS;
			inputData.positionCS = input.PositionCS;
			return SurfaceDataToGbuffer(surfaceData, inputData, color.rgb, kLightingInvalid);
		}
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthNormalOnlyVertex
		#pragma fragment DepthNormalOnlyFragment
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "Meta"
		Tags{ "LightMode" = "Meta" }
		Cull Off
		HLSLPROGRAM
		#pragma vertex UniversalVertexMeta
		#pragma fragment UniversalFragmentMetaSimple
		#pragma shader_feature_local_fragment _SPECGLOSSMAP
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitMetaPass.hlsl"
		ENDHLSL
	}
}
//Fallback "VertexLit"
```



### TerrainLit

```c
Properties
{
	[HideInInspector] [ToggleUI] _EnableHeightBlend("EnableHeightBlend", Float) = 0.0
	_HeightTransition("Height Transition", Range(0, 1.0)) = 0.0
	// Layer count is passed down to guide height-blend enable/disable, due
	// to the fact that heigh-based blend will be broken with multipass.
	[HideInInspector] [PerRendererData] _NumLayersCount ("Total Layer Count", Float) = 1.0
	// set by terrain engine
	[HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}
	[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "grey" {}
	[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "grey" {}
	[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "grey" {}
	[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "grey" {}
	[HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
	[HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
	[HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
	[HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
	[HideInInspector] _Mask3("Mask 3 (A)", 2D) = "grey" {}
	[HideInInspector] _Mask2("Mask 2 (B)", 2D) = "grey" {}
	[HideInInspector] _Mask1("Mask 1 (G)", 2D) = "grey" {}
	[HideInInspector] _Mask0("Mask 0 (R)", 2D) = "grey" {}
	[HideInInspector][Gamma] _Metallic0("Metallic 0", Range(0.0, 1.0)) = 0.0
	[HideInInspector][Gamma] _Metallic1("Metallic 1", Range(0.0, 1.0)) = 0.0
	[HideInInspector][Gamma] _Metallic2("Metallic 2", Range(0.0, 1.0)) = 0.0
	[HideInInspector][Gamma] _Metallic3("Metallic 3", Range(0.0, 1.0)) = 0.0
	[HideInInspector] _Smoothness0("Smoothness 0", Range(0.0, 1.0)) = 0.5
	[HideInInspector] _Smoothness1("Smoothness 1", Range(0.0, 1.0)) = 0.5
	[HideInInspector] _Smoothness2("Smoothness 2", Range(0.0, 1.0)) = 0.5
	[HideInInspector] _Smoothness3("Smoothness 3", Range(0.0, 1.0)) = 0.5
	// used in fallback on old cards & base map
	[HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "grey" {}
	[HideInInspector] _BaseColor("Main Color", Color) = (1,1,1,1)
	[HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}
	[ToggleUI] _EnableInstancedPerPixelNormal("Enable Instanced per-pixel normal", Float) = 1.0
}
HLSLINCLUDE
#pragma multi_compile_fragment __ _ALPHATEST_ON
ENDHLSL
SubShader
{
	Tags { "Queue" = "Geometry-100" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "False" "TerrainCompatible" = "True"}
	Pass
	{
		Name "ForwardLit"
		Tags { "LightMode" = "UniversalForward" }
		HLSLPROGRAM
		#pragma target 3.0
		#pragma vertex SplatmapVert
		#pragma fragment SplatmapFragment
		#define _METALLICSPECGLOSSMAP 1
		#define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma multi_compile_instancing
		#pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma shader_feature_local_fragment _TERRAIN_BLEND_HEIGHT
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _MASKMAP
		// Sample normal in pixel shader when doing instancing
		#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		// -------------------------------------
		// Universal Pipeline keywords
		// This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
		#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma target 3.0
		#pragma vertex SplatmapVert
		#pragma fragment SplatmapFragment
		#define _METALLICSPECGLOSSMAP 1
		#define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		//#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
		#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		//#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma shader_feature_local _TERRAIN_BLEND_HEIGHT
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _MASKMAP
		// Sample normal in pixel shader when doing instancing
		#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
		#define TERRAIN_GBUFFER 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthNormalOnlyVertex
		#pragma fragment DepthNormalOnlyFragment
		#pragma shader_feature_local _NORMALMAP
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "SceneSelectionPass"
		Tags { "LightMode" = "SceneSelectionPass" }
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#define SCENESELECTIONPASS
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma vertex TerrainVertexMeta
		#pragma fragment TerrainFragmentMeta
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma shader_feature EDITOR_VISUALIZATION
		#define _METALLICSPECGLOSSMAP 1
		#define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitMetaPass.hlsl"
		ENDHLSL
	}
	UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
}
Dependency "AddPassShader" = "Hidden/Universal Render Pipeline/Terrain/Lit (Add Pass)"
Dependency "BaseMapShader" = "Hidden/Universal Render Pipeline/Terrain/Lit (Base Pass)"
Dependency "BaseMapGenShader" = "Hidden/Universal Render Pipeline/Terrain/Lit (Basemap Gen)"
CustomEditor "UnityEditor.Rendering.Universal.TerrainLitShaderGUI"
Fallback "Hidden/Universal Render Pipeline/FallbackError"
```



### TerrainLitAdd

```c
Properties
{
	// Layer count is passed down to guide height-blend enable/disable, due
	// to the fact that heigh-based blend will be broken with multipass.
	[HideInInspector] [PerRendererData] _NumLayersCount ("Total Layer Count", Float) = 1.0
	// set by terrain engine
	[HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}
	[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
	[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
	[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
	[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
	[HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
	[HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
	[HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
	[HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
	[HideInInspector][Gamma] _Metallic0("Metallic 0", Range(0.0, 1.0)) = 0.0
	[HideInInspector][Gamma] _Metallic1("Metallic 1", Range(0.0, 1.0)) = 0.0
	[HideInInspector][Gamma] _Metallic2("Metallic 2", Range(0.0, 1.0)) = 0.0
	[HideInInspector][Gamma] _Metallic3("Metallic 3", Range(0.0, 1.0)) = 0.0
	[HideInInspector] _Mask3("Mask 3 (A)", 2D) = "grey" {}
	[HideInInspector] _Mask2("Mask 2 (B)", 2D) = "grey" {}
	[HideInInspector] _Mask1("Mask 1 (G)", 2D) = "grey" {}
	[HideInInspector] _Mask0("Mask 0 (R)", 2D) = "grey" {}
	[HideInInspector] _Smoothness0("Smoothness 0", Range(0.0, 1.0)) = 1.0
	[HideInInspector] _Smoothness1("Smoothness 1", Range(0.0, 1.0)) = 1.0
	[HideInInspector] _Smoothness2("Smoothness 2", Range(0.0, 1.0)) = 1.0
	[HideInInspector] _Smoothness3("Smoothness 3", Range(0.0, 1.0)) = 1.0
	// used in fallback on old cards & base map
	[HideInInspector] _BaseMap("BaseMap (RGB)", 2D) = "white" {}
	[HideInInspector] _BaseColor("Main Color", Color) = (1,1,1,1)
	[HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}
}
HLSLINCLUDE
#pragma multi_compile_fragment __ _ALPHATEST_ON
ENDHLSL
SubShader
{
	Tags { "Queue" = "Geometry-99" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True"}
	Pass
	{
		Name "TerrainAddLit"
		Tags { "LightMode" = "UniversalForward" }
		Blend One One
		HLSLPROGRAM
		#pragma target 3.0
		#pragma vertex SplatmapVert
		#pragma fragment SplatmapFragment
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma shader_feature_local_fragment _TERRAIN_BLEND_HEIGHT
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local_fragment _MASKMAP
		// Sample normal in pixel shader when doing instancing
		#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
		#define TERRAIN_SPLAT_ADDPASS
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		Blend One One
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma target 3.0
		#pragma vertex SplatmapVert
		#pragma fragment SplatmapFragment
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		//#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma shader_feature_local _TERRAIN_BLEND_HEIGHT
		#pragma shader_feature_local _NORMALMAP
		#pragma shader_feature_local _MASKMAP
		// Sample normal in pixel shader when doing instancing
		#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
		#define TERRAIN_SPLAT_ADDPASS 1
		#define TERRAIN_GBUFFER 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
}
Fallback "Hidden/Universal Render Pipeline/FallbackError"

```



### TerrainLitBase

```c
Properties
{
	[MainColor] _BaseColor("Color", Color) = (1,1,1,1)
	_MainTex("Albedo(RGB), Smoothness(A)", 2D) = "white" {}
	_MetallicTex ("Metallic (R)", 2D) = "black" {}
	[HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}
}
HLSLINCLUDE
#pragma multi_compile_fragment __ _ALPHATEST_ON
ENDHLSL
SubShader
{
	Tags { "Queue" = "Geometry-100" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True"}
	LOD 200
	// ------------------------------------------------------------------
	//  Forward pass. Shades all light in a single pass. GI + emission + Fog
	Pass
	{
		Name "ForwardLit"
		// Lightmode matches the ShaderPassName set in UniversalPipeline.cs. SRPDefaultUnlit and passes with
		// no LightMode tag are also rendered by Universal Pipeline
		Tags{"LightMode" = "UniversalForward"}
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#define _METALLICSPECGLOSSMAP 1
		#define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_instancing
		#pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#pragma vertex SplatmapVert
		#pragma fragment SplatmapFragment
		#pragma shader_feature_local _NORMALMAP
		// Sample normal in pixel shader when doing instancing
		#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
		#define TERRAIN_SPLAT_BASEPASS 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma vertex ShadowPassVertex
		#pragma fragment ShadowPassFragment
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	// ------------------------------------------------------------------
	//  GBuffer pass. Does GI + emission. All additional lights are done deferred as well as fog
	Pass
	{
		Name "GBuffer"
		Tags{"LightMode" = "UniversalGBuffer"}
		HLSLPROGRAM
		#pragma exclude_renderers gles
		#pragma target 2.0
		// -------------------------------------
		// Material Keywords
		#define _METALLICSPECGLOSSMAP 1
		#define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		//#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		//#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile _ DYNAMICLIGHTMAP_ON
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma multi_compile_instancing
		#pragma instancing_options norenderinglayer assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma vertex SplatmapVert
		#pragma fragment SplatmapFragment
		#pragma shader_feature_local _NORMALMAP
		// Sample normal in pixel shader when doing instancing
		#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
		#define TERRAIN_SPLAT_BASEPASS 1
		#define TERRAIN_GBUFFER 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthNormalOnlyVertex
		#pragma fragment DepthNormalOnlyFragment
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#pragma shader_feature_local _NORMALMAP
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitDepthNormalsPass.hlsl"
		ENDHLSL
	}
	// This pass it not used during regular rendering, only for lightmap baking.
	Pass
	{
		Name "Meta"
		Tags{"LightMode" = "Meta"}
		Cull Off
		HLSLPROGRAM
		#pragma vertex TerrainVertexMeta
		#pragma fragment TerrainFragmentMeta
		#pragma shader_feature EDITOR_VISUALIZATION
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
		#define _METALLICSPECGLOSSMAP 1
		#define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitMetaPass.hlsl"
		ENDHLSL
	}
	UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Universal Render Pipeline/Terrain/Lit/SceneSelectionPass"
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
//CustomEditor "LitShaderGUI"
```



### TerrainLitBasemapGen

```c
Properties
{
	// Layer count is passed down to guide height-blend enable/disable, due
	// to the fact that heigh-based blend will be broken with multipass.
	[HideInInspector] [PerRendererData] _NumLayersCount ("Total Layer Count", Float) = 1.0
	[HideInInspector] _Control("AlphaMap", 2D) = "" {}
	[HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
	[HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
	[HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
	[HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
	[HideInInspector] _Mask3("Mask 3 (A)", 2D) = "grey" {}
	[HideInInspector] _Mask2("Mask 2 (B)", 2D) = "grey" {}
	[HideInInspector] _Mask1("Mask 1 (G)", 2D) = "grey" {}
	[HideInInspector] _Mask0("Mask 0 (R)", 2D) = "grey" {}
	[HideInInspector] [Gamma] _Metallic0 ("Metallic 0", Range(0.0, 1.0)) = 0.0
	[HideInInspector] [Gamma] _Metallic1 ("Metallic 1", Range(0.0, 1.0)) = 0.0
	[HideInInspector] [Gamma] _Metallic2 ("Metallic 2", Range(0.0, 1.0)) = 0.0
	[HideInInspector] [Gamma] _Metallic3 ("Metallic 3", Range(0.0, 1.0)) = 0.0
	[HideInInspector] _Smoothness0 ("Smoothness 0", Range(0.0, 1.0)) = 1.0
	[HideInInspector] _Smoothness1 ("Smoothness 1", Range(0.0, 1.0)) = 1.0
	[HideInInspector] _Smoothness2 ("Smoothness 2", Range(0.0, 1.0)) = 1.0
	[HideInInspector] _Smoothness3 ("Smoothness 3", Range(0.0, 1.0)) = 1.0
	[HideInInspector] _DstBlend("DstBlend", Float) = 0.0
}
Subshader
{
	HLSLINCLUDE
	#pragma target 3.0
	#define _METALLICSPECGLOSSMAP 1
	#define _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A 1
	#define _TERRAIN_BASEMAP_GEN
	#pragma shader_feature_local _TERRAIN_BLEND_HEIGHT
	#pragma shader_feature_local _MASKMAP
	#include "TerrainLitInput.hlsl"
	#include "TerrainLitPasses.hlsl"
	ENDHLSL
	Pass
	{
		Tags
		{
			"Name" = "_MainTex"
			"Format" = "ARGB32"
			"Size" = "1"
		}
		ZTest Always Cull Off ZWrite Off
		Blend One [_DstBlend]
		HLSLPROGRAM
		#pragma vertex Vert
		#pragma fragment Frag
		Varyings Vert(Attributes IN)
		{
			Varyings output = (Varyings) 0;
			output.clipPos = TransformWorldToHClip(IN.positionOS.xyz);
			// NOTE : This is basically coming from the vertex shader in TerrainLitPasses
			// There are other plenty of other values that the original version computes, but for this
			// pass, we are only interested in a few, so I'm just skipping the rest.
			output.uvMainAndLM.xy = IN.texcoord;
			output.uvSplat01.xy = TRANSFORM_TEX(IN.texcoord, _Splat0);
			output.uvSplat01.zw = TRANSFORM_TEX(IN.texcoord, _Splat1);
			output.uvSplat23.xy = TRANSFORM_TEX(IN.texcoord, _Splat2);
			output.uvSplat23.zw = TRANSFORM_TEX(IN.texcoord, _Splat3);
			return output;
		}
		half4 Frag(Varyings IN) : SV_Target
		{
			half3 normalTS = half3(0.0h, 0.0h, 1.0h);
			half4 splatControl;
			half weight;
			half4 mixedDiffuse = 0.0h;
			half4 defaultSmoothness = 0.0h;
			half4 masks[4];
			float2 splatUV = (IN.uvMainAndLM.xy * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
			splatControl = SAMPLE_TEXTURE2D(_Control, sampler_Control, splatUV);
			masks[0] = 1.0h;
			masks[1] = 1.0h;
			masks[2] = 1.0h;
			masks[3] = 1.0h;
		#ifdef _MASKMAP
			masks[0] = SAMPLE_TEXTURE2D(_Mask0, sampler_Mask0, IN.uvSplat01.xy);
			masks[1] = SAMPLE_TEXTURE2D(_Mask1, sampler_Mask0, IN.uvSplat01.zw);
			masks[2] = SAMPLE_TEXTURE2D(_Mask2, sampler_Mask0, IN.uvSplat23.xy);
			masks[3] = SAMPLE_TEXTURE2D(_Mask3, sampler_Mask0, IN.uvSplat23.zw);
		#ifdef _TERRAIN_BLEND_HEIGHT
			HeightBasedSplatModify(splatControl, masks);
		#endif
		#endif
			SplatmapMix(IN.uvMainAndLM, IN.uvSplat01, IN.uvSplat23, splatControl, weight, mixedDiffuse, defaultSmoothness, normalTS);
			half4 hasMask = half4(_LayerHasMask0, _LayerHasMask1, _LayerHasMask2, _LayerHasMask3);
			half4 maskSmoothness = half4(masks[0].a, masks[1].a, masks[2].a, masks[3].a);
			maskSmoothness *= half4(_MaskMapRemapScale0.a, _MaskMapRemapScale1.a, _MaskMapRemapScale2.a, _MaskMapRemapScale3.a);
			maskSmoothness += half4(_MaskMapRemapOffset0.a, _MaskMapRemapOffset1.a, _MaskMapRemapOffset2.a, _MaskMapRemapOffset3.a);
			defaultSmoothness = lerp(defaultSmoothness, maskSmoothness, hasMask);
			half smoothness = dot(splatControl, defaultSmoothness);
			return half4(mixedDiffuse.rgb, smoothness);
		}
		ENDHLSL
	}
	Pass
	{
		Tags
		{
			"Name" = "_MetallicTex"
			"Format" = "R8"
			"Size" = "1/4"
			"EmptyColor" = "FF000000"
		}
		ZTest Always Cull Off ZWrite Off
		Blend One [_DstBlend]
		HLSLPROGRAM
		#pragma vertex Vert
		#pragma fragment Frag
		Varyings Vert(Attributes IN)
		{
			Varyings output = (Varyings)0;
			output.clipPos = TransformWorldToHClip(IN.positionOS.xyz);
			// This is just like the other in that it is from TerrainLitPasses
			output.uvMainAndLM.xy = IN.texcoord;
			output.uvSplat01.xy = TRANSFORM_TEX(IN.texcoord, _Splat0);
			output.uvSplat01.zw = TRANSFORM_TEX(IN.texcoord, _Splat1);
			output.uvSplat23.xy = TRANSFORM_TEX(IN.texcoord, _Splat2);
			output.uvSplat23.zw = TRANSFORM_TEX(IN.texcoord, _Splat3);
			return output;
		}
		half4 Frag(Varyings IN) : SV_Target
		{
			half3 normalTS = half3(0.0h, 0.0h, 1.0h);
			half4 splatControl;
			half weight;
			half4 mixedDiffuse;
			half4 defaultSmoothness;
			half4 masks[4];
			float2 splatUV = (IN.uvMainAndLM.xy * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
			splatControl = SAMPLE_TEXTURE2D(_Control, sampler_Control, splatUV);
			masks[0] = 1.0h;
			masks[1] = 1.0h;
			masks[2] = 1.0h;
			masks[3] = 1.0h;
		#ifdef _MASKMAP
			masks[0] = SAMPLE_TEXTURE2D(_Mask0, sampler_Mask0, IN.uvSplat01.xy);
			masks[1] = SAMPLE_TEXTURE2D(_Mask1, sampler_Mask0, IN.uvSplat01.zw);
			masks[2] = SAMPLE_TEXTURE2D(_Mask2, sampler_Mask0, IN.uvSplat23.xy);
			masks[3] = SAMPLE_TEXTURE2D(_Mask3, sampler_Mask0, IN.uvSplat23.zw);
		#ifdef _TERRAIN_BLEND_HEIGHT
			HeightBasedSplatModify(splatControl, masks);
		#endif
		#endif
			SplatmapMix(IN.uvMainAndLM, IN.uvSplat01, IN.uvSplat23, splatControl, weight, mixedDiffuse, defaultSmoothness, normalTS);
			half4 hasMask = half4(_LayerHasMask0, _LayerHasMask1, _LayerHasMask2, _LayerHasMask3);
			half4 defaultMetallic = half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3);
			half4 maskMetallic = half4(masks[0].r, masks[1].r, masks[2].r, masks[3].r);
			maskMetallic *= half4(_MaskMapRemapScale0.r, _MaskMapRemapScale1.r, _MaskMapRemapScale3.r, _MaskMapRemapScale3.r);
			maskMetallic += half4(_MaskMapRemapOffset0.r, _MaskMapRemapOffset1.r, _MaskMapRemapOffset2.r, _MaskMapRemapOffset3.r);
			defaultMetallic = lerp(defaultMetallic, maskMetallic, hasMask);
			half metallic = dot(splatControl, defaultMetallic);
			return metallic;
		}
		ENDHLSL
	}
}
```



### WavingGrass

```c
// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
Properties
{
	_WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
	_Cutoff ("Cutoff", float) = 0.5
}
SubShader
{
	Tags {"Queue" = "Geometry+200" "RenderType" = "Grass" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit" }//"DisableBatching"="True"
	Cull Off
	LOD 200
	AlphaTest Greater [_Cutoff]
	ColorMask RGB
	Pass
	{
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		#pragma vertex WavingGrassVert
		#pragma fragment LitPassFragmentGrass
		#define _ALPHATEST_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull Off
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#define _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassPasses.hlsl"
		ENDHLSL
	}
	// This pass is used when drawing to a _CameraNormalsTexture texture with the forward renderer or the depthNormal prepass with the deferred renderer.
	Pass
	{
		Name "DepthNormalsOnly"
		Tags{"LightMode" = "DepthNormalsOnly"}
		ZWrite On
		Cull Off
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthNormalOnlyVertex
		#pragma fragment DepthNormalOnlyFragment
		// -------------------------------------
		// Material Keywords
		#define _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT // forward-only variant
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassDepthNormalsPass.hlsl"
		ENDHLSL
	}
}

```



### WavingGrassBillboard

```c
// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
Properties
{
	_WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
	_Cutoff ("Cutoff", float) = 0.5
}
SubShader
{
	Tags {"Queue" = "Geometry+200" "RenderType" = "GrassBillBoard" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "SimpleLit" }//"DisableBatching"="True"
	Cull Off
	LOD 200
	AlphaTest Greater [_Cutoff]
	ColorMask RGB
	Pass
	{
		HLSLPROGRAM
		#pragma target 2.0
		// -------------------------------------
		// Universal Pipeline keywords
		#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile _ SHADOWS_SHADOWMASK
		#pragma multi_compile _ _CLUSTERED_RENDERING
		// -------------------------------------
		// Unity defined keywords
		#pragma multi_compile _ DIRLIGHTMAP_COMBINED
		#pragma multi_compile _ LIGHTMAP_ON
		#pragma multi_compile_fog
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#pragma vertex WavingGrassBillboardVert
		#pragma fragment LitPassFragmentGrass
		#define _ALPHATEST_ON
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthOnly"
		Tags{"LightMode" = "DepthOnly"}
		ZWrite On
		ColorMask 0
		Cull Off
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthOnlyVertex
		#pragma fragment DepthOnlyFragment
		// -------------------------------------
		// Material Keywords
		#define _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassPasses.hlsl"
		ENDHLSL
	}
	Pass
	{
		Name "DepthNormals"
		Tags{"LightMode" = "DepthNormals"}
		ZWrite On
		Cull Off
		HLSLPROGRAM
		#pragma target 2.0
		#pragma vertex DepthNormalOnlyVertex
		#pragma fragment DepthNormalOnlyFragment
		// -------------------------------------
		// Material Keywords
		#define _ALPHATEST_ON
		#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
		//--------------------------------------
		// GPU Instancing
		#pragma multi_compile_instancing
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassInput.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrassDepthNormalsPass.hlsl"
		ENDHLSL
	}
}
```



## Utils
### CopyDepthPass

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#if defined(_DEPTH_MSAA_2)
    #define MSAA_SAMPLES 2
#elif defined(_DEPTH_MSAA_4)
    #define MSAA_SAMPLES 4
#elif defined(_DEPTH_MSAA_8)
    #define MSAA_SAMPLES 8
#else
    #define MSAA_SAMPLES 1
#endif
struct Attributes
{
	#if _USE_DRAW_PROCEDURAL
    uint vertexID     : SV_VertexID;
	#else
    float4 positionHCS: POSITION;
    float2 uv         : TEXCOORD0;
	#endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv         : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};
Varyings vert(Attributes input)
#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
#define DEPTH_TEXTURE_MS(name, samples) Texture2DMSArray<float, samples> name
#define DEPTH_TEXTURE(name) TEXTURE2D_ARRAY_FLOAT(name)
#define LOAD(uv, sampleIndex) LOAD_TEXTURE2D_ARRAY_MSAA(_CameraDepthAttachment, uv, unity_StereoEyeIndex, sampleIndex)
#define SAMPLE(uv) SAMPLE_TEXTURE2D_ARRAY(_CameraDepthAttachment, sampler_CameraDepthAttachment, uv, unity_StereoEyeIndex).r
#else
#define DEPTH_TEXTURE_MS(name, samples) Texture2DMS<float, samples> name
#define DEPTH_TEXTURE(name) TEXTURE2D_FLOAT(name)
#define LOAD(uv, sampleIndex) LOAD_TEXTURE2D_MSAA(_CameraDepthAttachment, uv, sampleIndex)
#define SAMPLE(uv) SAMPLE_DEPTH_TEXTURE(_CameraDepthAttachment, sampler_CameraDepthAttachment, uv)
#endif
#if MSAA_SAMPLES == 1
    DEPTH_TEXTURE(_CameraDepthAttachment);
    SAMPLER(sampler_CameraDepthAttachment);
#else
    DEPTH_TEXTURE_MS(_CameraDepthAttachment, MSAA_SAMPLES);
    float4 _CameraDepthAttachment_TexelSize;
#endif
#if UNITY_REVERSED_Z
    #define DEPTH_DEFAULT_VALUE 1.0
    #define DEPTH_OP min
#else
    #define DEPTH_DEFAULT_VALUE 0.0
    #define DEPTH_OP max
#endif
float SampleDepth(float2 uv)
float frag(Varyings input) : SV_Depth
```



### Deferred

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"

#define PREFERRED_CBUFFER_SIZE (64 * 1024)
#define SIZEOF_VEC4_TILEDATA 1 // uint4
#define SIZEOF_VEC4_PUNCTUALLIGHTDATA 6 // 6 * float4
// Should be ushort, but extra unpacking code is "too expensive"
#define MAX_DEPTHRANGE_PER_CBUFFER_BATCH (PREFERRED_CBUFFER_SIZE / 4) 
#define MAX_TILES_PER_CBUFFER_PATCH (PREFERRED_CBUFFER_SIZE / (16 * SIZEOF_VEC4_TILEDATA))
#define MAX_PUNCTUALLIGHT_PER_CBUFFER_BATCH (PREFERRED_CBUFFER_SIZE / (16 * SIZEOF_VEC4_PUNCTUALLIGHTDATA))
// Should be ushort, but extra unpacking code is "too expensive"
#define MAX_REL_LIGHT_INDICES_PER_CBUFFER_BATCH (PREFERRED_CBUFFER_SIZE / 4)
#if defined(SHADER_API_SWITCH)
#define USE_CBUFFER_FOR_DEPTHRANGE 0
#define USE_CBUFFER_FOR_TILELIST 0
#define USE_CBUFFER_FOR_LIGHTDATA 1
#define USE_CBUFFER_FOR_LIGHTLIST 0
#elif defined(SHADER_API_GLES) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)
#define USE_CBUFFER_FOR_DEPTHRANGE 1
#define USE_CBUFFER_FOR_TILELIST 1
#define USE_CBUFFER_FOR_LIGHTDATA 1
#define USE_CBUFFER_FOR_LIGHTLIST 1
#else
#define USE_CBUFFER_FOR_DEPTHRANGE 0
#define USE_CBUFFER_FOR_TILELIST 0
#define USE_CBUFFER_FOR_LIGHTDATA 1
#define USE_CBUFFER_FOR_LIGHTLIST 0
#endif
// This structure is used in StructuredBuffer.
// TODO move some of the properties to half storage (color, attenuation, spotDirection, flag to 16bits, occlusionProbeInfo)
struct PunctualLightData
{
    float3 posWS;
    float radius2;
    float4 color
    float4 attenuation;
    float3 spotDirection;
    int flags;
    float4 occlusionProbeInfo;
    uint layerMask; 
};
Light UnityLightFromPunctualLightDataAndWorldSpacePosition(PunctualLightData punctualLightData, float3 positionWS, half4 shadowMask, int shadowLightIndex, bool materialFlagReceiveShadowsOff)
```



### Fullscreen

```c
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#if _USE_DRAW_PROCEDURAL
void GetProceduralQuad(in uint vertexID, out float4 positionCS, out float2 uv)
#endif
struct Attributes
{
	#if _USE_DRAW_PROCEDURAL
    uint vertexID     : SV_VertexID;
	#else
    float4 positionOS : POSITION;
    float2 uv         : TEXCOORD0;
	#endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv         : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};
Varyings FullscreenVert(Attributes input)
Varyings Vert(Attributes input)
```



### Universal2D

```c
struct Attributes
{
    float4 positionOS       : POSITION;
    float2 uv               : TEXCOORD0;
};
struct Varyings
{
    float2 uv        : TEXCOORD0;
    float4 vertex : SV_POSITION;
};
Varyings vert(Attributes input)
half4 frag(Varyings input) : SV_Target
```



## Utils(Shader)
### Blit

```c
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	Pass
	{
		Name "Blit"
		ZTest Always
		ZWrite Off
		Cull Off
		HLSLPROGRAM
		#pragma vertex FullscreenVert
		#pragma fragment Fragment
		#pragma multi_compile_fragment _ _LINEAR_TO_SRGB_CONVERSION
		#pragma multi_compile _ _USE_DRAW_PROCEDURAL
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Fullscreen.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingFullscreen.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
		TEXTURE2D_X(_SourceTex);
		SAMPLER(sampler_SourceTex);
		half4 Fragment(Varyings input) : SV_Target
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
			float2 uv = input.uv;
			half4 col = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, uv);
			#ifdef _LINEAR_TO_SRGB_CONVERSION
			col = LinearToSRGB(col);
			#endif
			#if defined(DEBUG_DISPLAY)
			half4 debugColor = 0;
			if(CanDebugOverrideOutputColor(col, uv, debugColor))
			{
				return debugColor;
			}
			#endif
			return col;
		}
		ENDHLSL
	}
}
```



### CopyDepth

```c
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	Pass
	{
		Name "CopyDepth"
		ZTest Always ZWrite On ColorMask 0
		Cull Off
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile _ _DEPTH_MSAA_2 _DEPTH_MSAA_4 _DEPTH_MSAA_8
		#pragma multi_compile _ _USE_DRAW_PROCEDURAL
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/CopyDepthPass.hlsl"
		ENDHLSL
	}
}
```



### CoreBlit

```c
HLSLINCLUDE
#pragma target 2.0
#pragma editor_sync_compilation
#pragma multi_compile _ DISABLE_TEXTURE2D_X_ARRAY
#pragma multi_compile _ BLIT_SINGLE_SLICE
// Core.hlsl for XR dependencies
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
ENDHLSL
SubShader
{
	Tags{ "RenderPipeline" = "UniversalPipeline" }
	// 0: Nearest
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragNearest
		ENDHLSL
	}
	// 1: Bilinear
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragBilinear
		ENDHLSL
	}
	// 2: Nearest quad
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragNearest
		ENDHLSL
	}
	// 3: Bilinear quad
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragBilinear
		ENDHLSL
	}
	// 4: Nearest quad with padding
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragNearest
		ENDHLSL
	}
	// 5: Bilinear quad with padding
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragBilinear
		ENDHLSL
	}
	// 6: Nearest quad with padding
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragNearestRepeat
		ENDHLSL
	}
	// 7: Bilinear quad with padding
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragBilinearRepeat
		ENDHLSL
	}
	// 8: Bilinear quad with padding (for OctahedralTexture)
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragOctahedralBilinearRepeat
		ENDHLSL
	}
	/// Version 4, 5, 6, 7 with Alpha Blending 0.5
	// 9: Nearest quad with padding alpha blend (4 with alpha blend)
	Pass
	{
		ZWrite Off ZTest Always Blend DstColor Zero Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragNearest
			#define WITH_ALPHA_BLEND
		ENDHLSL
	}
	// 10: Bilinear quad with padding alpha blend (5 with alpha blend)
	Pass
	{
		ZWrite Off ZTest Always Blend DstColor Zero Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragBilinear
			#define WITH_ALPHA_BLEND
		ENDHLSL
	}
	// 11: Nearest quad with padding alpha blend (6 with alpha blend)
	Pass
	{
		ZWrite Off ZTest Always Blend DstColor Zero Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragNearestRepeat
			#define WITH_ALPHA_BLEND
		ENDHLSL
	}
	// 12: Bilinear quad with padding alpha blend (7 with alpha blend)
	Pass
	{
		ZWrite Off ZTest Always Blend DstColor Zero Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragBilinearRepeat
			#define WITH_ALPHA_BLEND
		ENDHLSL
	}
	// 13: Bilinear quad with padding alpha blend (for OctahedralTexture) (8 with alpha blend)
	Pass
	{
		ZWrite Off ZTest Always Blend DstColor Zero Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuadPadding
			#pragma fragment FragOctahedralBilinearRepeat
			#define WITH_ALPHA_BLEND
		ENDHLSL
	}
	// 14. Project Cube to Octahedral 2d quad
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragOctahedralProject
		ENDHLSL
	}
	// 15. Project Cube to Octahedral 2d quad with luminance (grayscale), RGBA to YYYY
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragOctahedralProjectLuminance
		ENDHLSL
	}
	// 16. Project Cube to Octahedral 2d quad with with A to RGBA (AAAA)
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragOctahedralProjectAlphaToRGBA
		ENDHLSL
	}
	// 17. Project Cube to Octahedral 2d quad with with R to RGBA (RRRR)
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragOctahedralProjectRedToRGBA
		ENDHLSL
	}
	// 18. Bilinear quad with luminance (grayscale), RGBA to YYYY
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragBilinearLuminance
		ENDHLSL
	}
	// 19. Bilinear quad with A to RGBA (AAAA)
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragBilinearAlphaToRGBA
		ENDHLSL
	}
	// 20. Bilinear quad with R to RGBA (RRRR)
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragBilinearRedToRGBA
		ENDHLSL
	}
}
Fallback Off
```



### CoreBlitColorAndDepth

```c
HLSLINCLUDE
	#pragma target 2.0
	#pragma editor_sync_compilation
	#pragma multi_compile _ DISABLE_TEXTURE2D_X_ARRAY
	#pragma multi_compile _ BLIT_SINGLE_SLICE
	// Core.hlsl for XR dependencies
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/BlitColorAndDepth.hlsl"
ENDHLSL
SubShader
{
	Tags{ "RenderPipeline" = "UniversalPipeline" }
	// 0: Color Only
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragColorOnly
		ENDHLSL
	}
	// 1:  Color Only and Depth
	Pass
	{
		ZWrite On ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment FragColorAndDepth
		ENDHLSL
	}
}
Fallback Off
```



### FallbackError

```c
SubShader
{
	Tags {"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" "ShaderModel" = "4.5"}
	Pass
	{
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 4.5
		#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#pragma editor_sync_compilation
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
		struct appdata_t
		{
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct v2f
		{
			float4 vertex : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO
		};
		v2f vert (appdata_t v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			return o;
		}
		float4 frag (v2f i) : SV_Target
		{
			return float4(1,0,1,1);
		}
		ENDHLSL
	}
}
Fallback "Hidden/Core/FallbackError"
```



### MaterialError

```c
SubShader
{
	Pass
	{
		// Hybrid Renderer compatible error shader, which is used by Hybrid Renderer
		// instead of the incompatible built-in error shader.
		// TODO: Ideally this would be combined with FallbackError.shader, but it seems
		// problematic because FallbackError needs to support SM2.0 and seems to use
		// built-in shader headers, whereas Hybrid support needs SM4.5 and SRP shader headers.
		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 4.5
		#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
		#pragma multi_compile _ DOTS_INSTANCING_ON
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
		struct appdata_t {
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		struct v2f {
			float4 vertex : SV_POSITION;
			UNITY_VERTEX_OUTPUT_STEREO
		};
		v2f vert (appdata_t v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			return o;
		}
		float4 frag (v2f i) : SV_Target
		{
			return float4(1,0,1,1);
		}
		ENDHLSL
	}
}
Fallback Off
```



### Sampling

```c
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	LOD 100
	// 0 - Downsample - Box filtering
	Pass
	{
		Name "BoxDownsample"
		ZTest Always
		ZWrite Off
		Cull Off
		HLSLPROGRAM
		#pragma vertex FullscreenVert
		#pragma fragment FragBoxDownsample
		#pragma multi_compile _ _USE_DRAW_PROCEDURAL
		#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Fullscreen.hlsl"
		TEXTURE2D_X(_SourceTex);
		SAMPLER(sampler_SourceTex);
		float4 _SourceTex_TexelSize;
		float _SampleOffset;
		half4 FragBoxDownsample(Varyings input) : SV_Target
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
			float4 d = _SourceTex_TexelSize.xyxy * float4(-_SampleOffset, -_SampleOffset, _SampleOffset, _SampleOffset);
			half4 s;
			s =  SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, input.uv + d.xy);
			s += SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, input.uv + d.zy);
			s += SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, input.uv + d.xw);
			s += SAMPLE_TEXTURE2D_X(_SourceTex, sampler_SourceTex, input.uv + d.zw);
			return s * 0.25h;
		}
		ENDHLSL
	}
}
```



### ScreenSpaceAmbientOcclusion

```c
HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	struct Attributes
	{
		float4 positionHCS   : POSITION;
		float2 uv           : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};
	struct Varyings
	{
		float4  positionCS  : SV_POSITION;
		float2  uv          : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};
	Varyings VertDefault(Attributes input)
	{
		Varyings output;
		UNITY_SETUP_INSTANCE_ID(input);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
		// Note: The pass is setup with a mesh already in CS
		// Therefore, we can just output vertex position
		output.positionCS = float4(input.positionHCS.xyz, 1.0);
		#if UNITY_UV_STARTS_AT_TOP
		output.positionCS.y *= _ScaleBiasRt.x;
		#endif
		output.uv = input.uv;
		// Add a small epsilon to avoid artifacts when reconstructing the normals
		output.uv += 1.0e-6;
		return output;
	}
ENDHLSL
SubShader
{
	Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	Cull Off ZWrite Off ZTest Always
	// ------------------------------------------------------------------
	// Depth only passes
	// ------------------------------------------------------------------
	// 0 - Occlusion estimation with CameraDepthTexture
	Pass
	{
		Name "SSAO_Occlusion"
		ZTest Always
		ZWrite Off
		Cull Off
		HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment SSAO
			#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
			#pragma multi_compile_local _SOURCE_DEPTH _SOURCE_DEPTH_NORMALS
			#pragma multi_compile_local _RECONSTRUCT_NORMAL_LOW _RECONSTRUCT_NORMAL_MEDIUM _RECONSTRUCT_NORMAL_HIGH
			#pragma multi_compile_local _ _ORTHOGRAPHIC
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SSAO.hlsl"
		ENDHLSL
	}
	// 1 - Horizontal Blur
	Pass
	{
		Name "SSAO_HorizontalBlur"
		HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment HorizontalBlur
			#define BLUR_SAMPLE_CENTER_NORMAL
			#pragma multi_compile_local _ _ORTHOGRAPHIC
			#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
			#pragma multi_compile_local _SOURCE_DEPTH _SOURCE_DEPTH_NORMALS
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SSAO.hlsl"
		ENDHLSL
	}
	// 2 - Vertical Blur
	Pass
	{
		Name "SSAO_VerticalBlur"
		HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment VerticalBlur
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SSAO.hlsl"
		ENDHLSL
	}
	// 3 - Final Blur
	Pass
	{
		Name "SSAO_FinalBlur"
		HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment FinalBlur
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SSAO.hlsl"
		ENDHLSL
	}
	// 4 - After Opaque
	Pass
	{
		Name "SSAO_AfterOpaque"
		ZTest NotEqual
		ZWrite Off
		Cull Off
		Blend One SrcAlpha, Zero One
		BlendOp Add, Add
		HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment FragAfterOpaque
			#define _SCREEN_SPACE_OCCLUSION
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			half4 FragAfterOpaque(Varyings input) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
				AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(input.uv);
				half occlusion = aoFactor.indirectAmbientOcclusion;
				return half4(0.0, 0.0, 0.0, occlusion);
			}
		ENDHLSL
	}
}
```



### ScreenSpaceShadows

```c
SubShader
{
	Tags{ "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
	HLSLINCLUDE
	//Keep compiler quiet about Shadows.hlsl.
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Fullscreen.hlsl"
	half4 Fragment(Varyings input) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
#if UNITY_REVERSED_Z
		float deviceDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, input.uv.xy).r;
#else
		float deviceDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, input.uv.xy).r;
		deviceDepth = deviceDepth * 2.0 - 1.0;
#endif
		float3 wpos = ComputeWorldSpacePosition(input.uv.xy, deviceDepth, unity_MatrixInvVP);
		//Fetch shadow coordinates for cascade.
		float4 coords = TransformWorldToShadowCoord(wpos);
		// Screenspace shadowmap is only used for directional lights which use orthogonal projection.
		ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
		half4 shadowParams = GetMainLightShadowParams();
		return SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), coords, shadowSamplingData, shadowParams, false);
	}
	ENDHLSL
	Pass
	{
		Name "ScreenSpaceShadows"
		ZTest Always
		ZWrite Off
		Cull Off
		HLSLPROGRAM
		#pragma multi_compile _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
		#pragma multi_compile_vertex _ _USE_DRAW_PROCEDURAL
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma vertex   FullscreenVert
		#pragma fragment Fragment
		ENDHLSL
	}
}
```



### StencilDeferred

```c
Properties {
	_StencilRef ("StencilRef", Int) = 0
	_StencilReadMask ("StencilReadMask", Int) = 0
	_StencilWriteMask ("StencilWriteMask", Int) = 0
	_LitPunctualStencilRef ("LitPunctualStencilWriteMask", Int) = 0
	_LitPunctualStencilReadMask ("LitPunctualStencilReadMask", Int) = 0
	_LitPunctualStencilWriteMask ("LitPunctualStencilWriteMask", Int) = 0
	_SimpleLitPunctualStencilRef ("SimpleLitPunctualStencilWriteMask", Int) = 0
	_SimpleLitPunctualStencilReadMask ("SimpleLitPunctualStencilReadMask", Int) = 0
	_SimpleLitPunctualStencilWriteMask ("SimpleLitPunctualStencilWriteMask", Int) = 0
	_LitDirStencilRef ("LitDirStencilRef", Int) = 0
	_LitDirStencilReadMask ("LitDirStencilReadMask", Int) = 0
	_LitDirStencilWriteMask ("LitDirStencilWriteMask", Int) = 0
	_SimpleLitDirStencilRef ("SimpleLitDirStencilRef", Int) = 0
	_SimpleLitDirStencilReadMask ("SimpleLitDirStencilReadMask", Int) = 0
	_SimpleLitDirStencilWriteMask ("SimpleLitDirStencilWriteMask", Int) = 0
	_ClearStencilRef ("ClearStencilRef", Int) = 0
	_ClearStencilReadMask ("ClearStencilReadMask", Int) = 0
	_ClearStencilWriteMask ("ClearStencilWriteMask", Int) = 0
}
HLSLINCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Deferred.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
struct Attributes
{
	float4 positionOS : POSITION;
	uint vertexID : SV_VertexID;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct Varyings
{
	float4 positionCS : SV_POSITION;
	float3 screenUV : TEXCOORD1;
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};
#if defined(_SPOT)
float4 _SpotLightScale;
float4 _SpotLightBias;
float4 _SpotLightGuard;
#endif
Varyings Vertex(Attributes input)
{
	Varyings output = (Varyings)0;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
	float3 positionOS = input.positionOS.xyz;
	#if defined(_SPOT)
	// Spot lights have an outer angle than can be up to 180 degrees, in which case the shape
	// becomes a capped hemisphere. There is no affine transforms to handle the particular cone shape,
	// so instead we will adjust the vertices positions in the vertex shader to get the tighest fit.
	[flatten] if (any(positionOS.xyz))
	{
		// The hemisphere becomes the rounded cap of the cone.
		positionOS.xyz = _SpotLightBias.xyz + _SpotLightScale.xyz * positionOS.xyz;
		positionOS.xyz = normalize(positionOS.xyz) * _SpotLightScale.w;
		// Slightly inflate the geometry to fit the analytic cone shape.
		// We want the outer rim to be expanded along xy axis only, while the rounded cap is extended along all axis.
		positionOS.xyz = (positionOS.xyz - float3(0, 0, _SpotLightGuard.w)) * _SpotLightGuard.xyz + float3(0, 0, _SpotLightGuard.w);
	}
	#endif
	#if defined(_DIRECTIONAL) || defined(_FOG) || defined(_CLEAR_STENCIL_PARTIAL) || (defined(_SSAO_ONLY) && defined(_SCREEN_SPACE_OCCLUSION))
		// Full screen render using a large triangle.
		output.positionCS = float4(positionOS.xy, UNITY_RAW_FAR_CLIP_VALUE, 1.0); // Force triangle to be on zfar
	#elif defined(_SSAO_ONLY) && !defined(_SCREEN_SPACE_OCCLUSION)
		// Deferred renderer does not know whether there is a SSAO feature or not at the C# scripting level.
		// However, this is known at the shader level because of the shader keyword SSAO feature enables.
		// If the keyword was not enabled, discard the SSAO_only pass by rendering the geometry outside the screen.
		output.positionCS = float4(positionOS.xy, -2, 1.0); // Force triangle to be discarded
	#else
		// Light shape geometry is projected as normal.
		VertexPositionInputs vertexInput = GetVertexPositionInputs(positionOS.xyz);
		output.positionCS = vertexInput.positionCS;
	#endif
	output.screenUV = output.positionCS.xyw;
	#if UNITY_UV_STARTS_AT_TOP
	output.screenUV.xy = output.screenUV.xy * float2(0.5, -0.5) + 0.5 * output.screenUV.z;
	#else
	output.screenUV.xy = output.screenUV.xy * 0.5 + 0.5 * output.screenUV.z;
	#endif
	return output;
}
TEXTURE2D_X(_CameraDepthTexture);
TEXTURE2D_X_HALF(_GBuffer0);
TEXTURE2D_X_HALF(_GBuffer1);
TEXTURE2D_X_HALF(_GBuffer2);
#if _RENDER_PASS_ENABLED
#define GBUFFER0 0
#define GBUFFER1 1
#define GBUFFER2 2
#define GBUFFER3 3
FRAMEBUFFER_INPUT_HALF(GBUFFER0);
FRAMEBUFFER_INPUT_HALF(GBUFFER1);
FRAMEBUFFER_INPUT_HALF(GBUFFER2);
FRAMEBUFFER_INPUT_FLOAT(GBUFFER3);
#else
#ifdef GBUFFER_OPTIONAL_SLOT_1
TEXTURE2D_X_HALF(_GBuffer4);
#endif
#endif
#if defined(GBUFFER_OPTIONAL_SLOT_2) && _RENDER_PASS_ENABLED
TEXTURE2D_X_HALF(_GBuffer5);
#elif defined(GBUFFER_OPTIONAL_SLOT_2)
TEXTURE2D_X(_GBuffer5);
#endif
#ifdef GBUFFER_OPTIONAL_SLOT_3
TEXTURE2D_X(_GBuffer6);
#endif
float4x4 _ScreenToWorld[2];
SamplerState my_point_clamp_sampler;
float3 _LightPosWS;
half3 _LightColor;
half4 _LightAttenuation; // .xy are used by DistanceAttenuation - .zw are used by AngleAttenuation *for SpotLights)
half3 _LightDirection;   // directional/spotLights support
half4 _LightOcclusionProbInfo;
int _LightFlags;
int _ShadowLightIndex;
uint _LightLayerMask;
int _CookieLightIndex;
half4 FragWhite(Varyings input) : SV_Target
{
	return half4(1.0, 1.0, 1.0, 1.0);
}
Light GetStencilLight(float3 posWS, float2 screen_uv, half4 shadowMask, uint materialFlags)
{
	Light unityLight;
	bool materialReceiveShadowsOff = (materialFlags & kMaterialFlagReceiveShadowsOff) != 0;
	#ifdef _LIGHT_LAYERS
	uint lightLayerMask =_LightLayerMask;
	#else
	uint lightLayerMask = DEFAULT_LIGHT_LAYERS;
	#endif
	#if defined(_DIRECTIONAL)
		#if defined(_DEFERRED_MAIN_LIGHT)
			unityLight = GetMainLight();
			// unity_LightData.z is set per mesh for forward renderer, we cannot cull lights in this fashion with deferred renderer.
			unityLight.distanceAttenuation = 1.0;
			if (!materialReceiveShadowsOff)
			{
				#if defined(_MAIN_LIGHT_SHADOWS_SCREEN) && !defined(_SURFACE_TYPE_TRANSPARENT)
					float4 shadowCoord = float4(screen_uv, 0.0, 1.0);
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					float4 shadowCoord = TransformWorldToShadowCoord(posWS.xyz);
				#else
					float4 shadowCoord = float4(0, 0, 0, 0);
				#endif
				unityLight.shadowAttenuation = MainLightShadow(shadowCoord, posWS.xyz, shadowMask, _MainLightOcclusionProbes);
			}
			#if defined(_LIGHT_COOKIES)
				real3 cookieColor = SampleMainLightCookie(posWS);
				unityLight.color *= float4(cookieColor, 1);
			#endif
		#else
			unityLight.direction = _LightDirection;
			unityLight.distanceAttenuation = 1.0;
			unityLight.shadowAttenuation = 1.0;
			unityLight.color = _LightColor.rgb;
			unityLight.layerMask = lightLayerMask;
			if (!materialReceiveShadowsOff)
			{
				#if defined(_ADDITIONAL_LIGHT_SHADOWS)
					unityLight.shadowAttenuation = AdditionalLightShadow(_ShadowLightIndex, posWS.xyz, _LightDirection, shadowMask, _LightOcclusionProbInfo);
				#endif
			}
		#endif
	#else
		PunctualLightData light;
		light.posWS = _LightPosWS;
		light.radius2 = 0.0; //  only used by tile-lights.
		light.color = float4(_LightColor, 0.0);
		light.attenuation = _LightAttenuation;
		light.spotDirection = _LightDirection;
		light.occlusionProbeInfo = _LightOcclusionProbInfo;
		light.flags = _LightFlags;
		light.layerMask = lightLayerMask;
		unityLight = UnityLightFromPunctualLightDataAndWorldSpacePosition(light, posWS.xyz, shadowMask, _ShadowLightIndex, materialReceiveShadowsOff);
		#ifdef _LIGHT_COOKIES
			// Enable/disable is done toggling the keyword _LIGHT_COOKIES, but we could do a "static if" instead if required.
			// if(_CookieLightIndex >= 0)
			{
				float4 cookieUvRect = GetLightCookieAtlasUVRect(_CookieLightIndex);
				float4x4 worldToLight = GetLightCookieWorldToLightMatrix(_CookieLightIndex);
				float2 cookieUv = float2(0,0);
				#if defined(_SPOT)
					cookieUv = ComputeLightCookieUVSpot(worldToLight, posWS, cookieUvRect);
				#endif
				#if defined(_POINT)
					cookieUv = ComputeLightCookieUVPoint(worldToLight, posWS, cookieUvRect);
				#endif
				half4 cookieColor = SampleAdditionalLightsCookieAtlasTexture(cookieUv);
				cookieColor = half4(IsAdditionalLightsCookieAtlasTextureRGBFormat() ? cookieColor.rgb
									: IsAdditionalLightsCookieAtlasTextureAlphaFormat() ? cookieColor.aaa
									: cookieColor.rrr, 1);
				unityLight.color *= cookieColor;
			}
		#endif
	#endif
	return unityLight;
}
half4 DeferredShading(Varyings input) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 screen_uv = (input.screenUV.xy / input.screenUV.z);
	#if _RENDER_PASS_ENABLED
	float d        = LOAD_FRAMEBUFFER_INPUT(GBUFFER3, input.positionCS.xy).x;
	half4 gbuffer0 = LOAD_FRAMEBUFFER_INPUT(GBUFFER0, input.positionCS.xy);
	half4 gbuffer1 = LOAD_FRAMEBUFFER_INPUT(GBUFFER1, input.positionCS.xy);
	half4 gbuffer2 = LOAD_FRAMEBUFFER_INPUT(GBUFFER2, input.positionCS.xy);
	#else
	// Using SAMPLE_TEXTURE2D is faster than using LOAD_TEXTURE2D on iOS platforms (5% faster shader).
	// Possible reason: HLSLcc upcasts Load() operation to float, which doesn't happen for Sample()?
	float d        = SAMPLE_TEXTURE2D_X_LOD(_CameraDepthTexture, my_point_clamp_sampler, screen_uv, 0).x; // raw depth value has UNITY_REVERSED_Z applied on most platforms.
	half4 gbuffer0 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer0, my_point_clamp_sampler, screen_uv, 0);
	half4 gbuffer1 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer1, my_point_clamp_sampler, screen_uv, 0);
	half4 gbuffer2 = SAMPLE_TEXTURE2D_X_LOD(_GBuffer2, my_point_clamp_sampler, screen_uv, 0);
	#endif
	#if defined(_DEFERRED_MIXED_LIGHTING)
	half4 shadowMask = SAMPLE_TEXTURE2D_X_LOD(MERGE_NAME(_, GBUFFER_SHADOWMASK), my_point_clamp_sampler, screen_uv, 0);
	#else
	half4 shadowMask = 1.0;
	#endif
	#ifdef _LIGHT_LAYERS
	float4 renderingLayers = SAMPLE_TEXTURE2D_X_LOD(MERGE_NAME(_, GBUFFER_LIGHT_LAYERS), my_point_clamp_sampler, screen_uv, 0);
	uint meshRenderingLayers = uint(renderingLayers.r * 255.5);
	#else
	uint meshRenderingLayers = DEFAULT_LIGHT_LAYERS;
	#endif
	half surfaceDataOcclusion = gbuffer1.a;
	uint materialFlags = UnpackMaterialFlags(gbuffer0.a);
	half3 color = 0.0.xxx;
	half alpha = 1.0;
	#if defined(_DEFERRED_MIXED_LIGHTING)
	// If both lights and geometry are static, then no realtime lighting to perform for this combination.
	[branch] if ((_LightFlags & materialFlags) == kMaterialFlagSubtractiveMixedLighting)
		return half4(color, alpha); // Cannot discard because stencil must be updated.
	#endif
	#if defined(USING_STEREO_MATRICES)
	int eyeIndex = unity_StereoEyeIndex;
	#else
	int eyeIndex = 0;
	#endif
	float4 posWS = mul(_ScreenToWorld[eyeIndex], float4(input.positionCS.xy, d, 1.0));
	posWS.xyz *= rcp(posWS.w);
	Light unityLight = GetStencilLight(posWS.xyz, screen_uv, shadowMask, materialFlags);
	[branch] if (!IsMatchingLightLayer(unityLight.layerMask, meshRenderingLayers))
		return half4(color, alpha); // Cannot discard because stencil must be updated.
	#if defined(_SCREEN_SPACE_OCCLUSION) && !defined(_SURFACE_TYPE_TRANSPARENT)
		AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(screen_uv);
		unityLight.color *= aoFactor.directAmbientOcclusion;
		#if defined(_DIRECTIONAL) && defined(_DEFERRED_FIRST_LIGHT)
		// What we want is really to apply the mininum occlusion value between the baked occlusion from surfaceDataOcclusion and real-time occlusion from SSAO.
		// But we already applied the baked occlusion during gbuffer pass, so we have to cancel it out here.
		// We must also avoid divide-by-0 that the reciprocal can generate.
		half occlusion = aoFactor.indirectAmbientOcclusion < surfaceDataOcclusion ? aoFactor.indirectAmbientOcclusion * rcp(surfaceDataOcclusion) : 1.0;
		alpha = occlusion;
		#endif
	#endif
	InputData inputData = InputDataFromGbufferAndWorldPosition(gbuffer2, posWS.xyz);
	#if defined(_LIT)
		#if SHADER_API_MOBILE || SHADER_API_SWITCH
		// Specular highlights are still silenced by setting specular to 0.0 during gbuffer pass and GPU timing is still reduced.
		bool materialSpecularHighlightsOff = false;
		#else
		bool materialSpecularHighlightsOff = (materialFlags & kMaterialFlagSpecularHighlightsOff);
		#endif
		BRDFData brdfData = BRDFDataFromGbuffer(gbuffer0, gbuffer1, gbuffer2);
		color = LightingPhysicallyBased(brdfData, unityLight, inputData.normalWS, inputData.viewDirectionWS, materialSpecularHighlightsOff);
	#elif defined(_SIMPLELIT)
		SurfaceData surfaceData = SurfaceDataFromGbuffer(gbuffer0, gbuffer1, gbuffer2, kLightingSimpleLit);
		half3 attenuatedLightColor = unityLight.color * (unityLight.distanceAttenuation * unityLight.shadowAttenuation);
		half3 diffuseColor = LightingLambert(attenuatedLightColor, unityLight.direction, inputData.normalWS);
		half smoothness = exp2(10 * surfaceData.smoothness + 1);
		half3 specularColor = LightingSpecular(attenuatedLightColor, unityLight.direction, inputData.normalWS, inputData.viewDirectionWS, half4(surfaceData.specular, 1), smoothness);
		// TODO: if !defined(_SPECGLOSSMAP) && !defined(_SPECULAR_COLOR), force specularColor to 0 in gbuffer code
		color = diffuseColor * surfaceData.albedo + specularColor;
	#endif
	return half4(color, alpha);
}
half4 FragFog(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	#if _RENDER_PASS_ENABLED
		float d = LOAD_FRAMEBUFFER_INPUT(GBUFFER3, input.positionCS.xy).x;
	#else
		float d = LOAD_TEXTURE2D_X(_CameraDepthTexture, input.positionCS.xy).x;
	#endif
	float eye_z = LinearEyeDepth(d, _ZBufferParams);
	float clip_z = UNITY_MATRIX_P[2][2] * -eye_z + UNITY_MATRIX_P[2][3];
	half fogFactor = ComputeFogFactor(clip_z);
	half fogIntensity = ComputeFogIntensity(fogFactor);
	return half4(unity_FogColor.rgb, fogIntensity);
}
half4 FragSSAOOnly(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
	float2 screen_uv = (input.screenUV.xy / input.screenUV.z);
	AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(screen_uv);
	half surfaceDataOcclusion = SAMPLE_TEXTURE2D_X_LOD(_GBuffer1, my_point_clamp_sampler, screen_uv, 0).a;
	// What we want is really to apply the mininum occlusion value between the baked occlusion from surfaceDataOcclusion and real-time occlusion from SSAO.
	// But we already applied the baked occlusion during gbuffer pass, so we have to cancel it out here.
	// We must also avoid divide-by-0 that the reciprocal can generate.
	half occlusion = aoFactor.indirectAmbientOcclusion < surfaceDataOcclusion ? aoFactor.indirectAmbientOcclusion * rcp(surfaceDataOcclusion) : 1.0;
	return half4(0.0, 0.0, 0.0, occlusion);
}
ENDHLSL
SubShader
{
	Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
	// 0 - Stencil pass
	Pass
	{
		Name "Stencil Volume"
		ZTest LEQual
		ZWrite Off
		ZClip false
		Cull Off
		ColorMask 0
		Stencil {
			Ref [_StencilRef]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront NotEqual
			PassFront Keep
			ZFailFront Invert
			CompBack NotEqual
			PassBack Keep
			ZFailBack Invert
		}
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile_vertex _ _SPOT
		#pragma vertex Vertex
		#pragma fragment FragWhite
		//#pragma enable_d3d11_debug_symbols
		ENDHLSL
	}
	// 1 - Deferred Punctual Light (Lit)
	Pass
	{
		Name "Deferred Punctual Light (Lit)"
		ZTest GEqual
		ZWrite Off
		ZClip false
		Cull Front
		Blend One One, Zero One
		BlendOp Add, Add
		Stencil {
			Ref [_LitPunctualStencilRef]
			ReadMask [_LitPunctualStencilReadMask]
			WriteMask [_LitPunctualStencilWriteMask]
			Comp Equal
			Pass Zero
			Fail Keep
			ZFail Keep
		}
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile_fragment _DEFERRED_STENCIL
		#pragma multi_compile _POINT _SPOT
		#pragma multi_compile_fragment _LIT
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _DEFERRED_MIXED_LIGHTING
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma vertex Vertex
		#pragma fragment DeferredShading
		//#pragma enable_d3d11_debug_symbols
		ENDHLSL
	}
	// 2 - Deferred Punctual Light (SimpleLit)
	Pass
	{
		Name "Deferred Punctual Light (SimpleLit)"
		ZTest GEqual
		ZWrite Off
		ZClip false
		Cull Front
		Blend One One, Zero One
		BlendOp Add, Add
		Stencil {
			Ref [_SimpleLitPunctualStencilRef]
			ReadMask [_SimpleLitPunctualStencilReadMask]
			WriteMask [_SimpleLitPunctualStencilWriteMask]
			CompBack Equal
			PassBack Zero
			FailBack Keep
			ZFailBack Keep
		}
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile_fragment _DEFERRED_STENCIL
		#pragma multi_compile _POINT _SPOT
		#pragma multi_compile_fragment _SIMPLELIT
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _DEFERRED_MIXED_LIGHTING
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma vertex Vertex
		#pragma fragment DeferredShading
		//#pragma enable_d3d11_debug_symbols
		ENDHLSL
	}
	// 3 - Deferred Directional Light (Lit)
	Pass
	{
		Name "Deferred Directional Light (Lit)"
		ZTest NotEqual
		ZWrite Off
		Cull Off
		Blend One SrcAlpha, Zero One
		BlendOp Add, Add
		Stencil {
			Ref [_LitDirStencilRef]
			ReadMask [_LitDirStencilReadMask]
			WriteMask [_LitDirStencilWriteMask]
			Comp Equal
			Pass Keep
			Fail Keep
			ZFail Keep
		}
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile_fragment _DEFERRED_STENCIL
		#pragma multi_compile _DIRECTIONAL
		#pragma multi_compile_fragment _LIT
		#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile_fragment _ _DEFERRED_MAIN_LIGHT
		#pragma multi_compile_fragment _ _DEFERRED_FIRST_LIGHT
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _DEFERRED_MIXED_LIGHTING
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma vertex Vertex
		#pragma fragment DeferredShading
		//#pragma enable_d3d11_debug_symbols
		ENDHLSL
	}
	// 4 - Deferred Directional Light (SimpleLit)
	Pass
	{
		Name "Deferred Directional Light (SimpleLit)"
		ZTest NotEqual
		ZWrite Off
		Cull Off
		Blend One SrcAlpha, Zero One
		BlendOp Add, Add
		Stencil {
			Ref [_SimpleLitDirStencilRef]
			ReadMask [_SimpleLitDirStencilReadMask]
			WriteMask [_SimpleLitDirStencilWriteMask]
			Comp Equal
			Pass Keep
			Fail Keep
			ZFail Keep
		}
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile_fragment _DEFERRED_STENCIL
		#pragma multi_compile _DIRECTIONAL
		#pragma multi_compile_fragment _SIMPLELIT
		#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
		#pragma multi_compile_fragment _ _DEFERRED_MAIN_LIGHT
		#pragma multi_compile_fragment _ _DEFERRED_FIRST_LIGHT
		#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
		#pragma multi_compile_fragment _ _SHADOWS_SOFT
		#pragma multi_compile_fragment _ LIGHTMAP_SHADOW_MIXING
		#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
		#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
		#pragma multi_compile_fragment _ _DEFERRED_MIXED_LIGHTING
		#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
		#pragma multi_compile_fragment _ _LIGHT_LAYERS
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma multi_compile_fragment _ _LIGHT_COOKIES
		#pragma vertex Vertex
		#pragma fragment DeferredShading
		//#pragma enable_d3d11_debug_symbols
		ENDHLSL
	}
	// 5 - Legacy fog
	Pass
	{
		Name "Fog"
		ZTest NotEqual
		ZWrite Off
		Cull Off
		Blend OneMinusSrcAlpha SrcAlpha, Zero One
		BlendOp Add, Add
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile _FOG
		#pragma multi_compile FOG_LINEAR FOG_EXP FOG_EXP2
		#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
		#pragma vertex Vertex
		#pragma fragment FragFog
		//#pragma enable_d3d11_debug_symbols
		ENDHLSL
	}
	// 6 - Clear stencil partial
	// This pass clears stencil between camera stacks rendering.
	// This is because deferred renderer encodes material properties in the 4 highest bits of the stencil buffer,
	// but we don't want to keep this information between camera stacks.
	Pass
	{
		Name "ClearStencilPartial"
		ColorMask 0
		ZTest NotEqual
		ZWrite Off
		Cull Off
		Stencil {
			Ref [_ClearStencilRef]
			ReadMask [_ClearStencilReadMask]
			WriteMask [_ClearStencilWriteMask]
			Comp NotEqual
			Pass Zero
			Fail Keep
			ZFail Keep
		}
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile _CLEAR_STENCIL_PARTIAL
		#pragma vertex Vertex
		#pragma fragment FragWhite
		ENDHLSL
	}
	// 7 - SSAO Only
	// This pass only runs when there is no fullscreen deferred light rendered (no directional light). It will adjust indirect/baked lighting with realtime occlusion
	// by rendering just before deferred shading pass.
	// This pass is also completely discarded from vertex shader when SSAO renderer feature is not enabled.
	Pass
	{
		Name "SSAOOnly"
		ZTest NotEqual
		ZWrite Off
		Cull Off
		Blend One SrcAlpha, Zero One
		BlendOp Add, Add
		HLSLPROGRAM
		#pragma exclude_renderers gles gles3 glcore
		#pragma target 4.5
		#pragma multi_compile_vertex _SSAO_ONLY
		#pragma multi_compile_vertex _ _SCREEN_SPACE_OCCLUSION
		#pragma vertex Vertex
		#pragma fragment FragSSAOOnly
		//#pragma enable_d3d11_debug_symbols
		ENDHLSL
	}
}
FallBack "Hidden/Universal Render Pipeline/FallbackError"
```



## XR(Shader)
### XRMirrorView

```c
SubShader
{
	Tags{ "RenderPipeline" = "UniversalPipeline" }
	HLSLINCLUDE
		#pragma exclude_renderers gles
	ENDHLSL
	// 0: TEXTURE2D
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragBilinear
			#define SRC_TEXTURE2D_X_ARRAY 0
			#include "Packages/com.unity.render-pipelines.universal/Shaders/XR/XRMirrorView.hlsl"
		ENDHLSL
	}
	// 1: TEXTURE2D_ARRAY
	Pass
	{
		ZWrite Off ZTest Always Blend Off Cull Off
		HLSLPROGRAM
			#pragma vertex VertQuad
			#pragma fragment FragBilinear
			#define SRC_TEXTURE2D_X_ARRAY 1
			#include "Packages/com.unity.render-pipelines.universal/Shaders/XR/XRMirrorView.hlsl"
		ENDHLSL
	}
}
Fallback Off
```



### XROcclusionMesh

```c
HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#pragma exclude_renderers d3d11_9x gles
	#pragma multi_compile _ XR_OCCLUSION_MESH_COMBINED
	// Not all platforms properly support SV_RenderTargetArrayIndex
	#if defined(SHADER_API_D3D11) || defined(SHADER_API_VULKAN) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3) || defined(SHADER_API_PSSL)
		#define USE_XR_OCCLUSION_MESH_COMBINED XR_OCCLUSION_MESH_COMBINED
	#endif
	struct Attributes
	{
		float4 vertex : POSITION;
	};
	struct Varyings
	{
		float4 vertex : SV_POSITION;
	#if USE_XR_OCCLUSION_MESH_COMBINED
		uint rtArrayIndex : SV_RenderTargetArrayIndex;
	#endif
	};
	Varyings Vert(Attributes input)
	{
		Varyings output;
		output.vertex = float4(input.vertex.xy * float2(2.0f, -2.0f) + float2(-1.0f, 1.0f), UNITY_NEAR_CLIP_VALUE, 1.0f);
	#if USE_XR_OCCLUSION_MESH_COMBINED
		output.rtArrayIndex = input.vertex.z;
	#endif
		return output;
	}
	float4 Frag() : SV_Target
	{
		return (0.0f).xxxx;
	}
ENDHLSL
SubShader
{
	Tags{ "RenderPipeline" = "UniversalPipeline" }
	Pass
	{
		ZWrite On ZTest Always Blend Off Cull Off
		ColorMask 0
		HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
		ENDHLSL
	}
}
Fallback Off
```




# 如何制作这份文档

## 如何列出文件名

建一个urp工程，到Library目录里把跟内置shader相关的两个文件夹拷出来。

cmd指令打印当前目录下所有hlsl文件名的两种方式（我选择了后者，即统一加“### ”前缀）

```bat
dir /B *.hlsl
```

```bat
for %f in (*.hlsl) do @echo ### %~nxf
#或者
for %f in (*.shader) do @echo ### %~nxf
#如果把nxf换成nf就是不含扩展名
```

## 如何统计代码中的函数

在notepad++中，通过以下几步 正则/通配符 替换完成代码精简：

- 设为“正则表达式”模式
- 在“查找目标”框中输入正则表达式，例如`(?s)\{.*?\}`。这里的`(?s)`是一个模式修饰符，它告诉正则表达式引擎让`.`匹配任何字符，包括换行符。在“替换为”框中输入空字符串，以删除匹配到的内容。
- 查找内容：`^\s*//.*$`（匹配以零个或多个空白字符开头，后跟`//`，然后是任意字符直到行尾的行）。在“替换为”框中输入空字符串，以删除匹配到的内容。
- 设为“扩展（\n, \r...）”模式
- 在“查找目标”框中 \n\n, 在”替换为"中输入 \n, 连续执行两次替换

最后，再手动整理到这篇文档中。

## 如何把源码整理为Obsidian的关系图谱

这个批处理脚本可以把当前目录文件复制一份加后缀的空文件到子目录A下，然后把生成的文件按原目录结构放到Obsidian工程里

```c
@echo off  
setlocal enabledelayedexpansion  
  
REM 遍历当前目录下的所有文件  
for %%f in (*) do (  
    REM 获取文件名（包括扩展名）和新的文件名  
    set "filename=%%~nxf"  
    set "newfilename=!filename!.md"  
      
    REM 在子目录A中创建新的空文件  
    if not exist "A" mkdir "A"  
    type nul > "A\!newfilename!"  
)  
  
endlocal
```

已整理好的Obsidian工程放到了gitee仓库：

https://gitee.com/wangbenchong/urpshader-relation-graph.git

网页：[Wangbenchong/URPShaderRelationGraph (gitee.com)](https://gitee.com/wangbenchong/urpshader-relation-graph)
