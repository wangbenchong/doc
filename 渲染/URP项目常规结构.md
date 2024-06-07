# URP项目的特征性体现在以下三个方面：

## Project Settings

### Graphics

- Scriptable Render Pipeline Settings 填入**URP-HighFidelity (Universal Render Pipeline Asset)**
- URP Global Settings 填入**UniversalRenderPipelineGlobalSettings (Universal Render Pipeline Global Setttings)**

### Quality

- Render Pipeline Asset 填入**URP-HighFidelity (Universal Render Pipeline Asset)**

## Assets

**UniversalRenderPipelineGlobalSettings (Universal Render Pipeline Global Setttings)**

### Settings

- **SampleSceneProfile (Volume Profile)**
- **URP-HighFidelity-Renderer (Universal Renderer Data)** 可填入 Post Process Data
  
  - SSAO (Screen Space Ambient Occlusion)
  - Render Objects
  - Decal
  - Screen Space Shadows
- **URP-HighFidelity  (Universal Render Pipeline Asset)** 可填入 Universal Renderer Data

   类似的其他品质还有

- **URP-Balanced-Renderer (Universal Renderer Data)**
  
  - SSAO (Screen Space Ambient Occlusion)
- **URP-Balanced  (Universal Render Pipeline Asset)**
- **URP-Performant-Render (Universal Renderer Data)**
- **URP-Performant  (Universal Render Pipeline Asset)**

## Component

### Camera

- Projection
- Rendering (可指定Universal Renderer Data)
- Stack
- Environment
- Output

### Light

- General
- Emission
- Rendering
- Shadows
- Light Cookie