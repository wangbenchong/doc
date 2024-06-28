# URP项目的特征性体现在以下三个方面：

## Project Settings

### Graphics

- Scriptable Render Pipeline Settings 填入***URP-HighFidelity.assets***  (Universal Render Pipeline Asset)

  创建方式（会连带创建Universal Renderer Data并自动填入）：

  ```mermaid
  graph LR
  Assets --> Create --> Rendering --> U[URP Asset（with 2D Render）<BR>（属于assets文件）]
  Rendering --> U2[URP Asset（with Universal Render）<BR>（属于assets文件）]
  ```

  

- URP Global Settings 填入***UniversalRenderPipelineGlobalSettings.assets*** (Universal Render Pipeline Global Setttings)

  创建方式：

  ```mermaid
  graph LR
  Assets --> Create --> Rendering --> U[URP Global Settings Asset<BR>（属于assets文件）]
  ```

  

### Quality

- Render Pipeline Asset 填入***URP-HighFidelity.assets*** (Universal Render Pipeline Asset)

## Assets

### 根目录

- ***UniversalRenderPipelineGlobalSettings.assets***

### Settings

- ***SampleSceneProfile.assets*** (Volume Profile)

   创建以及引用方式：

   ```mermaid
   graph LR
   Assets --> Create --> V[Volume Profile<BR>（属于assets文件）]
   GameObject --> Volume --> GV[Global Volume<BR>（属于Volume组件）]
   V --填入--> GV
   ```

   

- ***URP-HighFidelity-Renderer.assets*** (Universal Renderer Data)
  
     创建方式（单独创建才需要，如果先创建了Pipeline Asset，这个会自动生成）：
  
  ```mermaid
  graph LR
  Assets --> Create --> Rendering --> U2D[URP 2D Render<BR>（属于assets文件）]
  U2D -.填入.-> UA2[URP Asset 2D]
  Rendering --> UR[URP Universal Render<BR>（属于assets文件）]
  UR -.填入.-> UA[URP Asset]
  ```
  
     可填入以下：
  
  - Post Process Data
  
  - SSAO (Screen Space Ambient Occlusion)
  - Render Objects
  - Decal
  - Screen Space Shadows
  - 自定义RenerFeature
  
- ***URP-HighFidelity.assets***  (Universal Render Pipeline Asset) 可填入若干 Universal Renderer Data

   类似的其他品质还有

- ***URP-Balanced-Renderer.assets*** (Universal Renderer Data)
  
- ***URP-Balanced.assets***  (Universal Render Pipeline Asset)

- ***URP-Performant-Render.assets*** (Universal Renderer Data)

- ***URP-Performant.assets***  (Universal Render Pipeline Asset)

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