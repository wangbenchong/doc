NavMesh是Unity中自带的一种强大的寻路系统，它允许开发者为游戏中的角色或物体创建智能的移动路径。以下是对Unity寻路NavMesh的详细介绍：

# 基本概念

1. **NavMesh**：导航网格（Navigation Mesh）的缩写，是一种数据结构，用于描述游戏世界的可行走表面，并允许在游戏世界中寻找从一个可行走位置到另一个可行走位置的路径。
2. **NavMesh Agent**：需要自己移动到目标，并自动寻路的游戏物体。它是NavMesh系统中的核心组件，负责根据目标位置计算并移动角色。
3. **Off-Mesh Link**：用来控制当出现了不连续的地图（类似于断点）时，可以将两个断点连接起来，从而允许角色在这些不连续区域之间移动。
4. **NavMesh Obstacle**：地图上的障碍物，当角色移动时，需要避开这些障碍物。

# 使用步骤

1. 创建NavMesh
   - 将游戏场景中的地面或其他可行走区域设置为Navigation Static。
   - 打开Unity的Navigation窗口（Window->AI->Navigation），然后点击Bake按钮，Unity会根据设置为Navigation Static的对象生成NavMesh。
2. 添加NavMesh Agent
   - 选择需要自动寻路的角色或物体，为其添加NavMesh Agent组件。
   - 在NavMesh Agent组件中，可以设置角色的移动速度、转角速度、停止距离等参数。
3. 设置目标位置
   - 可以通过脚本为NavMesh Agent设置目标位置。当目标位置改变时，NavMesh Agent会自动重新计算并移动到新的目标位置。
4. 处理障碍物
   - 对于静态障碍物，NavMesh在烘焙时会自动避开它们。
   - 对于动态障碍物，可以使用NavMesh Obstacle组件来表示，并设置其大小和位置。当动态障碍物移动时，NavMesh Agent会自动避开它们。
5. 使用Off-Mesh Link
   - 当需要角色在不连续的区域之间移动时，可以使用Off-Mesh Link组件来创建连接。
   - 设置Off-Mesh Link的起点和终点，并指定角色在通过这些连接时需要执行的动作（如跳跃、开门等）。

# 高级功能

1. 多层NavMesh
   - Unity支持创建多层NavMesh，这样可以在不同的高度上创建不同的寻路路径。
   - 通过为NavMesh Agent设置不同的Layer Mask，可以控制角色在不同层之间移动。
2. 动态烘焙
   - 使用NavMeshComponents等扩展工具，可以实现动态烘焙NavMesh的功能。
   - 这样可以在游戏运行时根据需要实时更新NavMesh，从而适应游戏世界的动态变化。
3. 路径平滑处理
   - Unity的NavMesh系统提供了路径平滑处理的功能，可以使角色在移动时更加自然和流畅。
   - 可以通过调整NavMesh Agent的Path Smoothing参数来控制路径的平滑程度。

# 注意事项

1. 性能考虑
   - 在使用NavMesh时，需要注意其对游戏性能的影响。特别是在大型游戏世界中，NavMesh的烘焙和更新可能会消耗较多的计算资源。
   - 因此，在设计和实现寻路系统时，需要充分考虑性能优化的问题。
2. 障碍物处理
   - 对于动态障碍物，需要确保NavMesh Agent能够及时感知并避开它们。
   - 同时，需要注意处理障碍物与NavMesh之间的交互逻辑，以避免出现寻路错误或角色卡住的问题。
3. 地图设计
   - 在设计游戏地图时，需要充分考虑NavMesh的可行性。确保地图中的可行走区域是连续的、无断点的，并且符合角色的移动需求。

综上所述，Unity的NavMesh寻路系统是一种强大而灵活的工具，可以帮助开发者为游戏中的角色或物体创建智能的移动路径。通过合理使用NavMesh及其相关组件和扩展工具，可以实现高效、自然和流畅的寻路效果。



# Unity6新版寻路

[Unity6AI自动寻路|AI Navigation2.0-NavMesh links and obstacles【Unity教程】_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1CMkEYBEQf/)



# 异步加载NavMesh

应用领域：在 navMeshSurface 中制作程序化无限地形

[官方文档：AI.NavMeshBuilder-UpdateNavMeshDataAsync - Unity 脚本 API](https://docs.unity.cn/cn/current/ScriptReference/AI.NavMeshBuilder.UpdateNavMeshDataAsync.html)

[异步更新防阻塞社区讨论](https://discussions.unity.com/t/updatenavmeshdataasync-locking-main-thread/911940/12)

[有救啦！Unity Navmesh竟然是能异步更新的？_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1YwkwYBEbv)

论坛代码摘抄如下：

```c#
bool m_buildInProgress = false;
void FixedUpdate()
{
    if (!m_initialized) return;
    float dist = (m_lastPos - m_playercar.transform.position).magnitude;
    if (dist >= m_updateDistance)
    {
        UnityEngine.Debug.Log("Already building:" + m_buildInProgress);
        if (m_buildInProgress) return;
        UnityEngine.Debug.Log("Starting another build");
        Stopwatch stopwatch = new();

        m_navBounds.center = m_playercar.transform.position;
        stopwatch.Start();

        StartCoroutine(UpdateSurfaces());
        stopwatch.Stop();

        m_lastPos = m_playercar.transform.position;
    }
}

public IEnumerator UpdateSurfaces()
{
    NavMeshBuildSettings set = m_surfaces[0].GetBuildSettings();
    NavMeshData data = m_surfaces[0].navMeshData;
    set.maxJobWorkers = 1;
    AsyncOperation op = NavMeshBuilder.UpdateNavMeshDataAsync(data, set, m_navSources, m_navBounds);
    m_buildInProgress = true;

    float timer = Time.unscaledTime;
    while (!op.isDone)
    {
        yield return null;
    }
    m_buildInProgress = false;
    UnityEngine.Debug.Log("Nav Mesh Time: " + (Time.unscaledTime - timer));
}
```

B站up代码摘抄如下：

```c#
//UpdateNavmeshSurface.cs
//...
bool m_buildInProgress = false;
private void Update()
{
    //...
    UpdateNavmesh();
}
public void UpdateNavmesh()
{
    if(m_buildInProgress)return;
    StartCoroutine(UpdateNavmeshIEnumerator());
}
private IEnumerator UpdateNavmeshIEnumerator()
{
    //TODO: update tile by tile, only update those tiles that are changed,not the whole map
    NavMeshBuildSettings set = surface.GetBuildSettings();
    set.maxJobWorkers = 1;
    
    var sources = new List<NavMeshBuildSource>();
    var markups = new List<NavMeshBuildSource>();
    NavMeshBuilder.CollectSources(transform, suface.layerMask, surface.useGeometry, surface.defaultArea, markups, sources);
    AsyncOperation op = NavMeshBuilder.UpdateNavMeshDataAsync(surface.navMeshData, set, sources, mmo.Bounds);
    m_buildInProgress = true;
    float timer = Time.unscaledTime;
    while(!op.isDone)
    {
        yield return null;
    }
    m_buildInProgress = false;
    Debug.Log("Nav Mesh Time "+(Time.unscaledTime - timer));
}
//...
```

