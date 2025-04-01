# 自定义Extension组件，实现目标过近时让目标消失

编写一个 **Extension 组件**，用于动态修改 `CinemachineCamera` 的近裁面（Near Clip Plane）以及相机的LayerMask。这个组件会在相机由于避障功能过于贴近角色时增大近裁面或者干脆把角色整体从相机中剔除，确保角色被完全裁剪，避免玩家看到被裁剪一半的角色。

参考文章：

- [Can Cinemachine Virtual Cameras have their own culling masks? - Unity Engine - Unity Discussions](https://discussions.unity.com/t/can-cinemachine-virtual-cameras-have-their-own-culling-masks/750325)
- [Unity社区（需要翻墙否则会重定向到国内）](https://discussions.unity.com)

以下是完整的代码实现：

------

## DynamicNearClipExtension.cs

```c#
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

[SaveDuringPlay]
[AddComponentMenu("")] // 隐藏菜单项
[ExecuteAlways]
[DisallowMultipleComponent]
public class DynamicNearClipExtension : CinemachineExtension
{
    [Header("近裁面动态调整设置")]
    [Tooltip("是否启用近裁面动态调整")]
    public bool editNearClip = true;

    [Tooltip("最小近裁面距离")]
    public float MinNearClip = 0.1f;

    [Tooltip("最大近裁面距离")]
    public float MaxNearClip = 1.6f;

    [Tooltip("近裁面调整的平滑速度")]
    public float SmoothSpeed = 30f;

    [Tooltip("角色被裁剪的阈值距离。当相机与角色的距离小于此值时，近裁面会增大")]
    public float ClipThreshold = 0.9f;

    private float m_TargetNearClip; // 目标近裁面值
    private float m_CurrentNearClip; // 当前近裁面值


    [Header("相机剔除动态调整设置")]
    [Tooltip("是否启用相机剔除动态调整")]
    public bool editCullMask = false;
    [Tooltip("角色被剔除的阈值距离。当相机与角色的距离小于此值时，角色会直接从相机剔除")]
    public float cullThreshold = 0.8f;
    [Tooltip("Player 层的名称")]
    public string playerLayerName = "Player";

    private int _playerLayerMask; // Player 层的掩码
    
    /// <summary>
    /// 在相机管道的 Finalize 阶段调用，用于修改相机状态。
    /// </summary>
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // 只在 Finalize 阶段处理
        if (stage == CinemachineCore.Stage.Finalize)
        {
            // 获取 Follow 目标
            var followTarget = vcam.Follow;
            if (followTarget == null)
                return;

            // 计算相机与目标的距离
            float distanceToTarget = Vector3.Distance(state.RawPosition, followTarget.position);

            if(editNearClip)
            {
                // 如果距离小于阈值，增大近裁面
                if (distanceToTarget < ClipThreshold)
                {
                    // 根据距离动态调整目标近裁面
                    m_TargetNearClip = Mathf.Lerp(MaxNearClip, MinNearClip, distanceToTarget / ClipThreshold);
                }
                else
                {
                    // 否则恢复最小近裁面
                    m_TargetNearClip = MinNearClip;
                }
                // 平滑过渡近裁面值
                m_CurrentNearClip = Mathf.Lerp(m_CurrentNearClip, m_TargetNearClip, SmoothSpeed * deltaTime);
                // 修改相机状态的近裁面
                state.Lens.NearClipPlane = m_CurrentNearClip;
            }
            if(editCullMask && _playerLayerMask > 0)
            {
                var brain = CinemachineCore.FindPotentialTargetBrain(vcam);
                Camera cam = brain?.OutputCamera;
                if (cam == null) return;
                // 动态调整 cullingMask
                if (distanceToTarget < cullThreshold)
                {
                    // 如果距离过近，移除 Player 层
                    cam.cullingMask &= ~(1 << _playerLayerMask);
                }
                else
                {
                    // 如果距离足够远，恢复 Player 层
                    cam.cullingMask |= (1 << _playerLayerMask);
                }
            }
        }
    }

    /// <summary>
    /// 当组件启用时调用，初始化近裁面值。
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        m_CurrentNearClip = MinNearClip; // 初始化为最小近裁面
        m_TargetNearClip = MinNearClip;  // 初始目标值

        if(editCullMask)
        {
            // 初始化 Player 层的掩码
            _playerLayerMask = LayerMask.NameToLayer(playerLayerName);
            if (_playerLayerMask == -1)
            {
                Debug.LogError($"Layer '{playerLayerName}' 不存在，请检查 Layer 设置！");
            }
        }
    }
}
```

------

## **代码解析**

### 核心功能

- **动态调整近裁面**：
  - 当相机与目标的距离小于 `ClipThreshold` 时，近裁面会动态增大，确保角色被完全裁剪。
  - 当相机与目标的距离大于 `ClipThreshold` 时，近裁面恢复为最小值。
- **平滑过渡**：
  - 使用 `Mathf.Lerp` 实现近裁面的平滑过渡，避免近裁面值的突变。
- **动态调整相机Layer剔除**
  - 当相机与目标的距离小于 `cullThreshold` 时，直接把角色整体剔除
  - 当相机与目标的距离大于 `cullThreshold` 时，恢复相机对角色的接收


### 关键参数

- **`MinNearClip`**：近裁面的最小值。
- **`MaxNearClip`**：近裁面的最大值。
- **`ClipThreshold`**：触发近裁面调整的阈值距离。当相机与目标的距离小于此值时，近裁面会增大。
- **`SmoothSpeed`**：近裁面调整的平滑速度。
- **`cullThreshold`**：角色被剔除的阈值距离。当相机与角色的距离小于此值时，角色会直接从相机剔除。

### 核心方法

- **`PostPipelineStageCallback`**：
  - 每帧都在管线阶段（Pipeline Stages）处理完成后执行
  - 在 `Finalize` 阶段调用，用于修改相机状态。
  - 计算相机与目标的距离，并根据距离动态调整近裁面。
  - 使用 `Mathf.Lerp` 实现近裁面的平滑过渡。
- **`OnEnable`**：
  - 初始化近裁面值，确保组件启用时近裁面从最小值开始。
  - 初始化角色layer层，确保执行相机剔除时能找到对应的层。

### 继承选择

继承设计中，选择继承 `CinemachineExtension` 而不是 `CinemachineComponentBase` 的原因主要与功能和职责的划分有关。选择继承 `CinemachineExtension` 是因为它更适合扩展和修改现有功能，同时保持了代码的灵活性和可维护性。如果你的需求是扩展而非完全重写核心逻辑，`CinemachineExtension` 是更优的选择。以下是具体原因：

1. **职责与功能**

   - **CinemachineComponentBase**：这是 Cinemachine 中核心组件的基础类，通常用于直接控制相机的行为（如跟随、瞄准等）。它的职责较为底层，直接参与相机的计算和更新。

   - **CinemachineExtension**：这是 Cinemachine 的扩展机制，允许在不修改核心组件的情况下扩展功能。它的职责是提供额外的功能或修改现有行为，而不直接参与核心逻辑。

2. **扩展性**

   - 如果你需要添加自定义行为或修改现有行为，继承 `CinemachineExtension` 更为合适。它允许你在不破坏原有逻辑的情况下，通过扩展机制实现功能。

   - 继承 `CinemachineComponentBase` 则需要直接修改核心逻辑，可能会引入不必要的复杂性或风险。

3. **灵活性**

   - `CinemachineExtension` 提供了更大的灵活性，允许你在多个虚拟相机上复用扩展逻辑，而不需要为每个相机单独编写组件。

   - `CinemachineComponentBase` 的灵活性较低，通常需要为每个相机单独配置组件。

4. **维护性**

   - 使用 `CinemachineExtension` 可以更好地分离关注点，使代码更易于维护。扩展逻辑与核心逻辑分离，便于独立测试和修改。

   - 直接继承 `CinemachineComponentBase` 可能会导致代码耦合度增加，增加维护难度。

5. **适用场景**

   - 如果你的需求是扩展或修改 Cinemachine 的行为（如添加自定义的相机抖动、动态调整视野等），`CinemachineExtension` 是更合适的选择。

   - 如果你需要完全自定义相机的核心行为（如实现全新的跟随或瞄准逻辑），则可能需要继承 `CinemachineComponentBase`。

------

## 使用方法

1. 将 `DynamicNearClipExtension` 脚本保存为 `DynamicNearClipExtension.cs`。
2. 在 Unity 编辑器中，将 `DynamicNearClipExtension` 组件添加到 `CinemachineCamera` 的 GameObject 上。
3. 自行选择开启动态近裁、动态剔除两大功能，并调整其中的具体参数。

------

## 示例场景

假设你有一个第三人称相机，角色可能会因为避障功能过于贴近相机。通过添加 `DynamicNearClipExtension` 组件，当相机与角色的距离小于 `ClipThreshold` 时，近裁面会自动增大，确保角色被完全裁剪，避免玩家看到被裁剪一半的角色。当相机与角色贴近到小于 `cullThreshold` 时，相机会直接把角色整体剔除。

------

## 总结

- 这个 `DynamicNearClipExtension` 组件通过动态调整近裁面和相机剔除，解决了相机因避障功能过于贴近角色时，角色被部分裁剪的问题。
- 代码符合 **Cinemachine 3.x** 的扩展组件规范，易于集成到现有项目中。

# 源码翻译

## CinemachineComponentBase 代码注释翻译

```csharp
using UnityEngine;

namespace Unity.Cinemachine
{
    /// <summary>
    /// 表示 CinemachineCamera 的抽象基类，用于对相机状态进行修改。
    /// </summary>
    [ExecuteAlways]
    public abstract class CinemachineComponentBase : MonoBehaviour
    {
        /// <summary>用于表示非常小的浮点数的常量</summary>
        protected const float Epsilon = UnityVectorExtensions.Epsilon;

        CinemachineVirtualCameraBase m_VcamOwner;

        /// <summary>获取关联的 CinemachineVirtualCameraBase</summary>
        public CinemachineVirtualCameraBase VirtualCamera
        {
            get
            {
                if (m_VcamOwner == null)
                    TryGetComponent(out m_VcamOwner);
#if !CINEMACHINE_NO_CM2_SUPPORT
                if (m_VcamOwner == null && transform.parent != null)
                    transform.parent.TryGetComponent(out m_VcamOwner);
#endif
                return m_VcamOwner;
            }
        }

        /// <summary>
        /// 标准的 OnEnable 调用。派生类应调用基类实现。
        /// 处理管道的验证。
        /// </summary>
        protected virtual void OnEnable()
        {
            var vcam = VirtualCamera as CinemachineCamera;
            if (vcam != null)
                vcam.InvalidatePipelineCache();
        }

        /// <summary>
        /// 标准的 OnDisable 调用。派生类应调用基类实现。
        /// 处理管道的验证。
        /// </summary>
        protected virtual void OnDisable()
        {
            var vcam = VirtualCamera as CinemachineCamera;
            if (vcam != null)
                vcam.InvalidatePipelineCache();
        }

        /// <summary>返回所有者 vcam 的 Follow 目标。</summary>
        public Transform FollowTarget
        {
            get
            {
                var vcam = VirtualCamera;
                return vcam == null ? null : vcam.ResolveFollow(vcam.Follow);
            }
        }

        /// <summary>返回所有者 vcam 的 LookAt 目标。</summary>
        public Transform LookAtTarget
        {
            get
            {
                CinemachineVirtualCameraBase vcam = VirtualCamera;
                return vcam == null ? null : vcam.ResolveLookAt(vcam.LookAt);
            }
        }

        /// <summary>将 Follow 目标作为 ICinemachineTargetGroup 返回，如果目标不是组则返回 null</summary>
        public ICinemachineTargetGroup FollowTargetAsGroup 
        {
            get
            {
                CinemachineVirtualCameraBase vcam = VirtualCamera;
                return vcam == null ? null : vcam.FollowTargetAsGroup;
            }
        }

        /// <summary>获取 Follow 目标的位置。特殊处理：如果 Follow 目标是 VirtualCamera，则返回 vcam State 的位置，而不是 Transform 的位置。</summary>
        public Vector3 FollowTargetPosition
        {
            get
            {
                var vcam = VirtualCamera.FollowTargetAsVcam;
                if (vcam != null)
                    return vcam.State.GetFinalPosition();
                Transform target = FollowTarget;
                if (target != null)
                    return TargetPositionCache.GetTargetPosition(target);
                return Vector3.zero;
            }
        }

        /// <summary>获取 Follow 目标的旋转。特殊处理：如果 Follow 目标是 VirtualCamera，则返回 vcam State 的旋转，而不是 Transform 的旋转。</summary>
        public Quaternion FollowTargetRotation
        {
            get
            {
                var vcam = VirtualCamera.FollowTargetAsVcam;
                if (vcam != null)
                    return vcam.State.GetFinalOrientation();
                Transform target = FollowTarget;
                if (target != null)
                    return TargetPositionCache.GetTargetRotation(target);
                return Quaternion.identity;
            }
        }

        /// <summary>将 LookAt 目标作为 ICinemachineTargetGroup 返回，如果目标不是组则返回 null</summary>
        public ICinemachineTargetGroup LookAtTargetAsGroup => VirtualCamera.LookAtTargetAsGroup;

        /// <summary>获取 LookAt 目标的位置。特殊处理：如果 LookAt 目标是 VirtualCamera，则返回 vcam State 的位置，而不是 Transform 的位置。</summary>
        public Vector3 LookAtTargetPosition
        {
            get
            {
                var vcam = VirtualCamera.LookAtTargetAsVcam;
                if (vcam != null)
                    return vcam.State.GetFinalPosition();
                Transform target = LookAtTarget;
                if (target != null)
                    return TargetPositionCache.GetTargetPosition(target);
                return Vector3.zero;
            }
        }

        /// <summary>获取 LookAt 目标的旋转。特殊处理：如果 LookAt 目标是 VirtualCamera，则返回 vcam State 的旋转，而不是 Transform 的旋转。</summary>
        public Quaternion LookAtTargetRotation
        {
            get
            {
                var vcam = VirtualCamera.LookAtTargetAsVcam;
                if (vcam != null)
                    return vcam.State.GetFinalOrientation();
                Transform target = LookAtTarget;
                if (target != null)
                    return TargetPositionCache.GetTargetRotation(target);
                return Quaternion.identity;
            }
        }

        /// <summary>返回所有者 vcam 的 CameraState。</summary>
        public CameraState VcamState
        {
            get
            {
                CinemachineVirtualCameraBase vcam = VirtualCamera;
                return vcam == null ? CameraState.Default : vcam.State;
            }
        }

        /// <summary>如果此对象已启用并设置为生成结果，则返回 true。</summary>
        public abstract bool IsValid { get; }

        /// <summary>重写此方法以执行诸如偏移 ReferenceLookAt 之类的操作。基类实现不执行任何操作。</summary>
        /// <param name="curState">必须被修改的输入状态</param>
        /// <param name="deltaTime">当前有效的 deltaTime</param>
        public virtual void PrePipelineMutateCameraState(ref CameraState curState, float deltaTime) {}

        /// <summary>此组件在管道中的哪个阶段执行</summary>
        public abstract CinemachineCore.Stage Stage { get; }

        /// <summary>对于希望在 Aim 阶段之后应用的 Body 阶段组件，因为它们使用 Aim 作为程序化放置的输入</summary>
        public virtual bool BodyAppliesAfterAim => false;

        /// <summary>修改相机状态。此状态稍后将应用于相机。</summary>
        /// <param name="curState">必须被修改的输入状态</param>
        /// <param name="deltaTime">用于基于时间的效果的 Delta 时间（如果小于 0 则忽略）</param>
        public abstract void MutateCameraState(ref CameraState curState, float deltaTime);

        /// <summary>通知此虚拟相机即将激活。基类实现不执行任何操作。</summary>
        /// <param name="fromCam">被停用的相机。可能为 null。</param>
        /// <param name="worldUp">默认的世界 Up，由 CinemachineBrain 设置</param>
        /// <param name="deltaTime">用于基于时间的效果的 Delta 时间（如果小于或等于 0 则忽略）</param>
        /// <returns>如果 vcam 应因此调用而执行内部更新，则返回 true</returns>
        public virtual bool OnTransitionFromCamera(
            ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime) => false;

        /// <summary>当目标发生瞬移时调用此方法，以便组件可以更新其内部状态，使相机也能无缝瞬移。基类实现不执行任何操作。</summary>
        /// <param name="target">被瞬移的对象</param>
        /// <param name="positionDelta">目标位置的变化量</param>
        public virtual void OnTargetObjectWarped(Transform target, Vector3 positionDelta) {}

        /// <summary>
        /// 强制虚拟相机假定给定的位置和方向。程序化放置随后接管。
        /// 基类实现不执行任何操作。</summary>
        /// <param name="pos">要采用的世界空间位置</param>
        /// <param name="rot">要采用的世界空间方向</param>
        public virtual void ForceCameraPosition(Vector3 pos, Quaternion rot) {}

        /// <summary>
        /// 报告此组件所需的最大阻尼时间。
        /// 仅在编辑器中用于时间轴擦除。
        /// </summary>
        /// <returns>此组件中的最高阻尼设置</returns>
        public virtual float GetMaxDampTime() => 0;

        /// <summary>
        /// 如果此组件尝试使相机看向跟踪目标，则返回 true。
        /// 由检查器用于警告用户潜在的设置不当。
        /// </summary>
        internal virtual bool CameraLooksAtTarget { get => false; }
    }
}
```



## CinemachineVirtualCameraBase 代码注释翻译

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Cinemachine
{
    /// <summary>
    /// 表示 Unity 场景中虚拟相机的 MonoBehaviour 基类。
    ///
    /// 此类旨在附加到空的 Transform GameObject。
    /// 继承类可以是独立的虚拟相机（如 CinemachineCamera），
    /// 也可以是元相机（如 CinemachineClearShot 或 CinemachineBlendListCamera）。
    ///
    /// CinemachineVirtualCameraBase 暴露了 OutputChannel 属性。当行为在游戏中启用时，
    /// 虚拟相机会自动放置在由静态 CinemachineCore 单例维护的队列中。
    /// 队列按优先级排序。当 Unity 相机配备了 CinemachineBrain 行为时，
    /// 大脑将选择队列中的第一个相机。如果你有多个带有 CinemachineBrain 行为的 Unity 相机
    /// （例如在分屏上下文中），则可以通过设置虚拟相机的剔除标志来过滤队列。
    /// Unity 相机的剔除掩码将充当大脑的过滤器。除此之外，
    /// 没有什么可以阻止虚拟相机同时控制多个 Unity 相机。
    /// </summary>
    public abstract class CinemachineVirtualCameraBase : MonoBehaviour, ICinemachineCamera
    {
        /// <summary>
        /// 优先级可用于在多个 CM 相机同时激活时控制哪个 CM 相机是活动的。
        /// 最近激活的 CinemachineCamera 将接管控制，除非有另一个具有更高优先级的 CM 相机处于活动状态。
        /// 通常，最近激活的最高优先级 CinemachineCamera 将控制主相机。
        /// 
        /// 默认优先级值为 0。通常保留默认设置即可。
        /// 在特殊情况下，如果你希望 CinemachineCamera 的优先级值高于或低于 0，可以在此处设置。
        /// </summary>
        [NoSaveDuringPlay]
        [Tooltip("优先级可用于在多个 CM 相机同时激活时控制哪个 CM 相机是活动的。"
            + "最近激活的 CinemachineCamera 将接管控制，除非有另一个具有更高优先级的 CM 相机处于活动状态。"
            + "通常，最近激活的最高优先级 CinemachineCamera 将控制主相机。\n\n"
            + "默认优先级值为 0。通常保留默认设置即可。"
            + "在特殊情况下，如果你希望 CinemachineCamera 的优先级值高于或低于 0，可以在此处设置。")]
        [EnabledProperty(toggleText: "(使用默认值)")]
        public PrioritySettings Priority = new ();

        /// <summary>
        /// 输出通道的功能类似于 Unity 层。使用它可以将 CinemachineCameras 的输出过滤到不同的 CinemachineBrains，
        /// 例如在多屏幕环境中。
        /// </summary>
        [NoSaveDuringPlay]
        [Tooltip("输出通道的功能类似于 Unity 层。使用它可以将 CinemachineCameras 的输出过滤到不同的 CinemachineBrains，"
            + "例如在多屏幕环境中。")]
        public OutputChannels OutputChannel = OutputChannels.Default;

        /// <summary>用于从 CM2 升级的辅助方法</summary>
        internal protected virtual bool IsDprecated => false;

        /// <summary>表示对象激活顺序的序列号。用于优先级排序。</summary>
        internal int ActivationId;

        int m_QueuePriority = int.MaxValue;

        /// <summary>
        /// 必须在管道的每一帧开始时设置此值，以放松虚拟相机对目标的附着。范围为 0...1。
        /// 1 表示完全附着，是正常状态。
        /// 0 表示无附着，虚拟相机的行为将像未设置 Follow 目标一样。
        /// </summary>
        [NonSerialized]
        public float FollowTargetAttachment;

        /// <summary>
        /// 必须在管道的每一帧开始时设置此值，以放松虚拟相机对目标的附着。范围为 0...1。
        /// 1 表示完全附着，是正常状态。
        /// 0 表示无附着，虚拟相机的行为将像未设置 LookAt 目标一样。
        /// </summary>
        [NonSerialized]
        public float LookAtTargetAttachment;

        /// <summary>
        /// 当虚拟相机处于待机模式时，更新的频率
        /// </summary>
        public enum StandbyUpdateMode
        {
            /// <summary>仅在虚拟相机处于活动状态时更新</summary>
            Never,
            /// <summary>即使虚拟相机不处于活动状态，也每帧更新</summary>
            Always,
            /// <summary>偶尔更新虚拟相机，具体频率取决于其他待机虚拟相机的数量</summary>
            RoundRobin
        };

        /// <summary>当虚拟相机不处于活动状态时，更新的频率。设置此值以调整性能。大多数情况下 Never 即可，除非虚拟相机正在执行镜头评估。</summary>
        [Tooltip("当虚拟相机不处于活动状态时，更新的频率。设置此值以调整性能。大多数情况下 Never 即可，除非虚拟相机正在执行镜头评估。")]
        [FormerlySerializedAs("m_StandbyUpdate")]
        public StandbyUpdateMode StandbyUpdate = StandbyUpdateMode.RoundRobin;

        // 用于 GameObject 名称的缓存，以避免 GC 分配
        string m_CachedName;
        bool m_WasStarted;
        bool m_ChildStatusUpdated = false;
        CinemachineVirtualCameraBase m_ParentVcam = null;

        Transform m_CachedFollowTarget;
        CinemachineVirtualCameraBase m_CachedFollowTargetVcam;
        ICinemachineTargetGroup m_CachedFollowTargetGroup;

        Transform m_CachedLookAtTarget;
        CinemachineVirtualCameraBase m_CachedLookAtTargetVcam;
        ICinemachineTargetGroup m_CachedLookAtTargetGroup;

        // 旧版流式支持
        [HideInInspector, SerializeField, NoSaveDuringPlay]
        int m_StreamingVersion;

        /// <summary>
        /// 重写此方法以处理流式版本更改所需的任何升级。
        /// 请注意，由于此方法不是从主线程调用的，因此它不能执行许多操作，包括检查 Unity 对象是否为 null。
        /// </summary>
        /// <param name="streamedVersion">流式传输的版本</param>
        internal protected virtual void PerformLegacyUpgrade(int streamedVersion)
        {
            if (streamedVersion < 20220601)
            {
                if (m_LegacyPriority != 0)
                {
                    Priority.Value = m_LegacyPriority;
                    m_LegacyPriority = 0;
                }
            }
        }

        [HideInInspector, SerializeField, NoSaveDuringPlay, FormerlySerializedAs("m_Priority")]
        int m_LegacyPriority = 0;

        /// <summary>
        /// 查询组件和扩展以获取最大阻尼时间。
        /// 基类实现查询扩展。
        /// 仅在编辑器中用于时间轴擦除。
        /// </summary>
        /// <returns>此 vcam 中的最高阻尼设置</returns>
        public virtual float GetMaxDampTime()
        {
            float maxDamp = 0;
            if (Extensions != null)
                for (int i = 0; i < Extensions.Count; ++i)
                    maxDamp = Mathf.Max(maxDamp, Extensions[i].GetMaxDampTime());
            return maxDamp;
        }

        /// <summary>获取数量的阻尼版本。这是将在给定时间内生效的数量部分。
        /// 此方法考虑了目标附着。对于不考虑目标附着的一般阻尼，请使用 Damper.Damp()</summary>
        /// <param name="initial">将被阻尼的数量</param>
        /// <param name="dampTime">阻尼速率。这是将原始数量减少到可忽略百分比所需的时间</param>
        /// <param name="deltaTime">阻尼的时间</param>
        /// <returns>阻尼后的数量。这将是原始数量按 0 到 1 之间的值缩放的结果。</returns>
        public float DetachedFollowTargetDamp(float initial, float dampTime, float deltaTime)
        {
            dampTime = Mathf.Lerp(Mathf.Max(1, dampTime), dampTime, FollowTargetAttachment);
            deltaTime = Mathf.Lerp(0, deltaTime, FollowTargetAttachment);
            return Damper.Damp(initial, dampTime, deltaTime);
        }

        /// <summary>获取数量的阻尼版本。这是将在给定时间内生效的数量部分。
        /// 此方法考虑了目标附着。对于不考虑目标附着的一般阻尼，请使用 Damper.Damp()</summary>
        /// <param name="initial">将被阻尼的数量</param>
        /// <param name="dampTime">阻尼速率。这是将原始数量减少到可忽略百分比所需的时间</param>
        /// <param name="deltaTime">阻尼的时间</param>
        /// <returns>阻尼后的数量。这将是原始数量按 0 到 1 之间的值缩放的结果。</returns>
        public Vector3 DetachedFollowTargetDamp(Vector3 initial, Vector3 dampTime, float deltaTime)
        {
            dampTime = Vector3.Lerp(Vector3.Max(Vector3.one, dampTime), dampTime, FollowTargetAttachment);
            deltaTime = Mathf.Lerp(0, deltaTime, FollowTargetAttachment);
            return Damper.Damp(initial, dampTime, deltaTime);
        }

        /// <summary>获取数量的阻尼版本。这是将在给定时间内生效的数量部分。
        /// 此方法考虑了目标附着。对于不考虑目标附着的一般阻尼，请使用 Damper.Damp()</summary>
        /// <param name="initial">将被阻尼的数量</param>
        /// <param name="dampTime">阻尼速率。这是将原始数量减少到可忽略百分比所需的时间</param>
        /// <param name="deltaTime">阻尼的时间</param>
        /// <returns>阻尼后的数量。这将是原始数量按 0 到 1 之间的值缩放的结果。</returns>
        public Vector3 DetachedFollowTargetDamp(Vector3 initial, float dampTime, float deltaTime)
        {
            dampTime = Mathf.Lerp(Mathf.Max(1, dampTime), dampTime, FollowTargetAttachment);
            deltaTime = Mathf.Lerp(0, deltaTime, FollowTargetAttachment);
            return Damper.Damp(initial, dampTime, deltaTime);
        }

        /// <summary>获取数量的阻尼版本。这是将在给定时间内生效的数量部分。
        /// 此方法考虑了目标附着。对于不考虑目标附着的一般阻尼，请使用 Damper.Damp()</summary>
        /// <param name="initial">将被阻尼的数量</param>
        /// <param name="dampTime">阻尼速率。这是将原始数量减少到可忽略百分比所需的时间</param>
        /// <param name="deltaTime">阻尼的时间</param>
        /// <returns>阻尼后的数量。这将是原始数量按 0 到 1 之间的值缩放的结果。</returns>
        public float DetachedLookAtTargetDamp(float initial, float dampTime, float deltaTime)
        {
            dampTime = Mathf.Lerp(Mathf.Max(1, dampTime), dampTime, LookAtTargetAttachment);
            deltaTime = Mathf.Lerp(0, deltaTime, LookAtTargetAttachment);
            return Damper.Damp(initial, dampTime, deltaTime);
        }

        /// <summary>获取数量的阻尼版本。这是将在给定时间内生效的数量部分。
        /// 此方法考虑了目标附着。对于不考虑目标附着的一般阻尼，请使用 Damper.Damp()</summary>
        /// <param name="initial">将被阻尼的数量</param>
        /// <param name="dampTime">阻尼速率。这是将原始数量减少到可忽略百分比所需的时间</param>
        /// <param name="deltaTime">阻尼的时间</param>
        /// <returns>阻尼后的数量。这将是原始数量按 0 到 1 之间的值缩放的结果。</returns>
        public Vector3 DetachedLookAtTargetDamp(Vector3 initial, Vector3 dampTime, float deltaTime)
        {
            dampTime = Vector3.Lerp(Vector3.Max(Vector3.one, dampTime), dampTime, LookAtTargetAttachment);
            deltaTime = Mathf.Lerp(0, deltaTime, LookAtTargetAttachment);
            return Damper.Damp(initial, dampTime, deltaTime);
        }

        /// <summary>获取数量的阻尼版本。这是将在给定时间内生效的数量部分。
        /// 此方法考虑了目标附着。对于不考虑目标附着的一般阻尼，请使用 Damper.Damp()</summary>
        /// <param name="initial">将被阻尼的数量</param>
        /// <param name="dampTime">阻尼速率。这是将原始数量减少到可忽略百分比所需的时间</param>
        /// <param name="deltaTime">阻尼的时间</param>
        /// <returns>阻尼后的数量。这将是原始数量按 0 到 1 之间的值缩放的结果。</returns>
        public Vector3 DetachedLookAtTargetDamp(Vector3 initial, float dampTime, float deltaTime)
        {
            dampTime = Mathf.Lerp(Mathf.Max(1, dampTime), dampTime, LookAtTargetAttachment);
            deltaTime = Mathf.Lerp(0, deltaTime, LookAtTargetAttachment);
            return Damper.Damp(initial, dampTime, deltaTime);
        }

        /// <summary>
        /// 添加一个 Pipeline 阶段钩子回调。
        /// 这将在每个管道阶段之后调用，以允许其他服务钩入管道。
        /// 参见 CinemachineCore.Stage。
        /// </summary>
        /// <param name="extension">要添加的扩展</param>
        internal void AddExtension(CinemachineExtension extension)
        {
            if (Extensions == null)
                Extensions = new List<CinemachineExtension>();
            else
                Extensions.Remove(extension);
            Extensions.Add(extension);
        }

        /// <summary>移除 Pipeline 阶段钩子回调。</summary>
        /// <param name="extension">要移除的扩展</param>
        internal void RemoveExtension(CinemachineExtension extension)
        {
            if (Extensions != null)
                Extensions.Remove(extension);
        }

        /// <summary> 连接到此 vcam 的扩展</summary>
        internal List<CinemachineExtension> Extensions { get; private set; }

        /// <summary>
        /// 为此相机调用 PostPipelineStageDelegate，并为所有父相机（如果有）调用。
        /// 实现必须确保在每个管道阶段之后调用此方法，以允许其他服务钩入管道。
        /// 参见 CinemachineCore.Stage。
        /// </summary>
        /// <param name="vcam">正在处理的虚拟相机</param>
        /// <param name="stage">当前管道阶段</param>
        /// <param name="newState">当前虚拟相机状态</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        protected void InvokePostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage,
            ref CameraState newState, float deltaTime)
        {
            if (Extensions != null)
            {
                for (int i = 0; i < Extensions.Count; ++i)
                {
                    var e = Extensions[i];
                    if (e == null)
                    {
                        // 对象被删除（可能是由于编辑器中的撤销操作）
                        Extensions.RemoveAt(i);
                        --i;
                    }
                    else if (e.enabled)
                        e.InvokePostPipelineStageCallback(vcam, stage, ref newState, deltaTime);
                }
            }
            if (ParentCamera is CinemachineVirtualCameraBase vcamParent)
                vcamParent.InvokePostPipelineStageCallback(vcam, stage, ref newState, deltaTime);
        }
        
        /// <summary>
        /// 为此相机调用 PrePipelineMutateCameraStateCallback，并为所有父相机（如果有）调用。
        /// 实现必须确保在每个管道阶段之后调用此方法，以允许其他服务钩入管道。
        /// 参见 CinemachineCore.Stage。
        /// </summary>
        /// <param name="vcam">正在处理的虚拟相机</param>
        /// <param name="newState">当前虚拟相机状态</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        protected void InvokePrePipelineMutateCameraStateCallback(
            CinemachineVirtualCameraBase vcam, ref CameraState newState, float deltaTime)
        {
            if (Extensions != null)
            {
                for (int i = 0; i < Extensions.Count; ++i)
                {
                    var e = Extensions[i];
                    if (e == null)
                    {
                        // 对象被删除（可能是由于编辑器中的撤销操作）
                        Extensions.RemoveAt(i);
                        --i;
                    }
                    else if (e.enabled)
                        e.PrePipelineMutateCameraStateCallback(vcam, ref newState, deltaTime);
                }
            }
            if (ParentCamera is CinemachineVirtualCameraBase vcamParent)
                vcamParent.InvokePrePipelineMutateCameraStateCallback(vcam, ref newState, deltaTime);
        }

        /// <summary>
        /// 为此相机的所有扩展调用 OnTransitionFromCamera
        /// </summary>
        /// <param name="fromCam">被停用的相机。可能为 null。</param>
        /// <param name="worldUp">默认的世界 Up，由 CinemachineBrain 设置</param>
        /// <param name="deltaTime">用于基于时间的效果的 Delta 时间（如果小于或等于 0 则忽略）</param>
        /// <returns>如果请求 vcam 更新内部状态，则返回 true</returns>
        protected bool InvokeOnTransitionInExtensions(
            ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
        {
            bool forceUpdate = false;
            if (Extensions != null)
            {
                for (int i = 0; i < Extensions.Count; ++i)
                {
                    var e = Extensions[i];
                    if (e == null)
                    {
                        // 对象被删除（可能是由于编辑器中的撤销操作）
                        Extensions.RemoveAt(i);
                        --i;
                    }
                    else if (e.enabled && e.OnTransitionFromCamera(fromCam, worldUp, deltaTime))
                        forceUpdate = true;
                }
            }
            return forceUpdate;
        }

        /// <summary>获取虚拟相机的名称。基类实现返回所有者 GameObject 名称的缓存。</summary>
        public string Name 
        {
            get 
            {
#if UNITY_EDITOR
                // 允许在不播放时更改 vcam 名称
                if (!Application.isPlaying)
                    m_CachedName = null;
#endif
                m_CachedName ??= IsValid ? name : "(已删除)";
                return m_CachedName;
            }
        }

        /// <summary>获取此虚拟相机的简要调试描述，用于显示调试信息时使用</summary>
        public virtual string Description => "";
        
        /// <summary>如果对象已被删除，则返回 false</summary>
        public bool IsValid => !(this == null);

        /// <summary>CameraState 对象包含定位 Unity 相机所需的所有信息。它是此类的输出。</summary>
        public abstract CameraState State { get; }

        /// <summary>支持元虚拟相机。这是虚拟相机实际上是私有虚拟相机军队的公共面孔的情况，
        /// 它自行管理这些虚拟相机。此方法获取虚拟相机所有者（如果有）。
        /// 私有军队实现为父 vcam 的 Transform 子级。</summary>
        public ICinemachineMixer ParentCamera
        {
            get
            {
                if (!m_ChildStatusUpdated || !Application.isPlaying)
                    UpdateStatusAsChild();
                return m_ParentVcam as ICinemachineMixer;
            }
        }

        /// <summary>获取 Cinemachine 管道中 Aim 组件的 LookAt 目标。</summary>
        public abstract Transform LookAt { get; set; }

        /// <summary>获取 Cinemachine 管道中 Body 组件的 Follow 目标。</summary>
        public abstract Transform Follow { get; set; }

        /// <summary>将此设置为强制下一帧忽略前一帧的状态。
        /// 这很有用，例如，如果你想取消阻尼或其他基于时间的处理。</summary>
        public virtual bool PreviousStateIsValid { get; set; }

        /// <summary>
        /// 更新相机的状态。
        /// 实现必须保证每帧不会多次调用，并且应使用
        /// CinemachineCore.UpdateVirtualCamera(ICinemachineCamera, Vector3, float)，它具有防止每帧多次调用的保护。
        /// </summary>
        /// <param name="worldUp">默认的世界 Up，由 CinemachineBrain 设置</param>
        /// <param name="deltaTime">用于基于时间的效果的 Delta 时间（如果小于 0 则忽略）</param>
        public void UpdateCameraState(Vector3 worldUp, float deltaTime)
        {
            CameraUpdateManager.UpdateVirtualCamera(this, worldUp, deltaTime);
        }

        /// <summary>仅供内部使用。
        /// 由 CinemachineCore 在指定的更新时间调用，以便 vcam 可以定位自身并跟踪其目标。
        /// 不要调用此方法。让框架在适当的时间调用它。</summary>
        /// <param name="worldUp">默认的世界 Up，由 CinemachineBrain 设置</param>
        /// <param name="deltaTime">用于基于时间的效果的 Delta 时间（如果小于 0 则忽略）</param>
        public abstract void InternalUpdateCameraState(Vector3 worldUp, float deltaTime);

        /// <inheritdoc />
        public virtual void OnCameraActivated(ICinemachineCamera.ActivationEventParams evt) 
        {
            if (evt.IncomingCamera == (ICinemachineCamera)this)
                OnTransitionFromCamera(evt.OutgoingCamera, evt.WorldUp, evt.DeltaTime);
        }

        // GML todo: 去掉 OnTransitionFromCamera
        /// <summary>通知此虚拟相机即将激活。基类实现必须由任何重写的方法调用。</summary>
        /// <param name="fromCam">被停用的相机。可能为 null。</param>
        /// <param name="worldUp">默认的世界 Up，由 CinemachineBrain 设置</param>
        /// <param name="deltaTime">用于基于时间的效果的 Delta 时间（如果小于或等于 0 则忽略）</param>
        public virtual void OnTransitionFromCamera(
            ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
        {
            if (!gameObject.activeInHierarchy)
                PreviousStateIsValid = false;
        }

        /// <summary>
        /// 当对象被时间轴人为激活时，在不活动的对象上调用。
        /// 这是必要的，因为 Awake() 不会在不活动的游戏对象上调用。
        /// </summary>
        internal void EnsureStarted()
        {
            if (!m_WasStarted)
            {
                m_WasStarted = true;

                // 如果需要，执行旧版升级
                if (m_StreamingVersion < CinemachineCore.kStreamingVersion)
                    PerformLegacyUpgrade(m_StreamingVersion);
                m_StreamingVersion = CinemachineCore.kStreamingVersion;

                var extensions = GetComponentsInChildren<CinemachineExtension>();
                for (int i = 0; i < extensions.Length; ++i)
                    extensions[i].EnsureStarted();
            }
        }

#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnScriptReload()
        {
            var vcams = Resources.FindObjectsOfTypeAll(
                typeof(CinemachineVirtualCameraBase)) as CinemachineVirtualCameraBase[];
            for (int i = 0; i < vcams.Length; ++i)
                vcams[i].LookAtTargetChanged = vcams[i].FollowTargetChanged = true;
        }
#endif

        /// <summary>基类实现确保优先级队列保持最新。</summary>
        protected virtual void OnTransformParentChanged()
        {
            CameraUpdateManager.CameraDisabled(this);
            CameraUpdateManager.CameraEnabled(this);
            UpdateStatusAsChild();
            UpdateVcamPoolStatus();
        }

        /// <summary>维护全局 vcam 注册表。始终调用基类实现。</summary>
        protected virtual void OnDestroy()
        {
            CameraUpdateManager.CameraDestroyed(this);
        }

        /// <summary>派生类应调用基类实现。</summary>
        protected virtual void Start()
        {
            m_WasStarted = true;

            // 如果需要，执行旧版升级
            if (m_StreamingVersion < CinemachineCore.kStreamingVersion)
                PerformLegacyUpgrade(m_StreamingVersion);
            m_StreamingVersion = CinemachineCore.kStreamingVersion;
        }
        
        /// <summary>基类实现将虚拟相机添加到优先级队列中。</summary>
        protected virtual void OnEnable()
        {
            UpdateStatusAsChild();
            UpdateVcamPoolStatus();    // 添加到队列
            if (!CinemachineCore.IsLive(this))
                PreviousStateIsValid = false;
            CameraUpdateManager.CameraEnabled(this);
            InvalidateCachedTargets();

            // 健全性检查 - 如果另一个 vcam 组件已启用，则关闭
            var vcamComponents = GetComponents<CinemachineVirtualCameraBase>();
            for (int i = 0; i < vcamComponents.Length; ++i)
            {
                if (vcamComponents[i].enabled && vcamComponents[i] != this)
                {
                    var toDeprecate = vcamComponents[i].IsDprecated ? vcamComponents[i] : this;
                    if (!toDeprecate.IsDprecated)
                        Debug.LogWarning(Name
                            + " 有多个 CinemachineVirtualCameraBase 派生组件。禁用 "
                            + toDeprecate.GetType().Name);
                    toDeprecate.enabled = false;
                }
            }
        }

        /// <summary>基类实现确保优先级队列保持最新。</summary>
        protected virtual void OnDisable()
        {
            UpdateVcamPoolStatus();    // 从队列中移除
            CameraUpdateManager.CameraDisabled(this);
        }

        /// <summary>基类实现确保优先级队列保持最新。</summary>
        protected virtual void Update()
        {
            if (Priority.Value != m_QueuePriority)
                UpdateVcamPoolStatus(); // 强制重新排序
        }

        void UpdateStatusAsChild()
        {
            m_ChildStatusUpdated = true;
            m_ParentVcam = null;
            Transform p = transform.parent;
            if (p != null)
                p.TryGetComponent(out m_ParentVcam);
        }

        /// <summary>返回此 vcam 的 LookAt 目标，如果为 null，则返回父 vcam 的 LookAt 目标。</summary>
        /// <param name="localLookAt">此 vcam 的 LookAt 值。</param>
        /// <returns>相同的值，如果存在父级则返回父级的值。</returns>
        public Transform ResolveLookAt(Transform localLookAt)
        {
            Transform lookAt = localLookAt;
            if (lookAt == null && ParentCamera is CinemachineVirtualCameraBase vcamParent)
                lookAt = vcamParent.LookAt; // 父级提供默认值
            return lookAt;
        }

        /// <summary>返回此 vcam 的 Follow 目标，如果为 null，则返回父 vcam 的 Follow 目标。</summary>
        /// <param name="localFollow">此 vcam 的 Follow 值。</param>
        /// <returns>相同的值，如果存在父级则返回父级的值。</returns>
        public Transform ResolveFollow(Transform localFollow)
        {
            Transform follow = localFollow;
            if (follow == null && ParentCamera is CinemachineVirtualCameraBase vcamParent)
                follow = vcamParent.Follow; // 父级提供默认值
            return follow;
        }

        void UpdateVcamPoolStatus()
        {
            CameraUpdateManager.RemoveActiveCamera(this);
            if (m_ParentVcam == null && isActiveAndEnabled)
                CameraUpdateManager.AddActiveCamera(this);
            m_QueuePriority = Priority.Value;
        }

        /// <summary>当多个虚拟相机具有最高优先级时，有时需要将一个推到顶部，
        /// 如果它与同级共享最高优先级，则使其成为当前的活动相机。
        ///
        /// 当新的 vcam 启用时，这会自动发生：最近的一个会进入优先级子队列的顶部。
        /// 使用此方法将 vcam 推到其优先级同级的顶部。
        /// 如果它与同级共享最高优先级，则此 vcam 将成为活动相机。</summary>
        [Obsolete("请使用 Prioritize()")]
        public void MoveToTopOfPrioritySubqueue() => Prioritize();

        /// <summary>当多个 CM 相机具有最高优先级时，有时需要将一个推到顶部，
        /// 如果它与同级共享最高优先级，则使其成为当前的活动相机。
        ///
        /// 当新的 CinemachineCamera 启用时，这会自动发生：最近的一个会进入优先级子队列的顶部。
        /// 使用此方法将相机推到其优先级同级的顶部。
        /// 如果它与同级共享最高优先级，则此 vcam 将成为活动相机。</summary>
        public void Prioritize() => UpdateVcamPoolStatus(); // 强制重新排序
        
        /// <summary>当目标发生瞬移时调用此方法，以便组件可以更新其内部状态，使相机也能无缝瞬移。</summary>
        /// <param name="target">被瞬移的对象</param>
        /// <param name="positionDelta">目标位置的变化量</param>
        public virtual void OnTargetObjectWarped(Transform target, Vector3 positionDelta) 
            => OnTargetObjectWarped(this, target, positionDelta);

        void OnTargetObjectWarped(CinemachineVirtualCameraBase vcam, Transform target, Vector3 positionDelta)
        {
            // 通知扩展
            if (Extensions != null)
            {
                for (int i = 0; i < Extensions.Count; ++i)
                    Extensions[i].OnTargetObjectWarped(vcam, target, positionDelta);
            }
            if (ParentCamera is CinemachineVirtualCameraBase vcamParent)
                vcamParent.OnTargetObjectWarped(vcam, target, positionDelta);
        }

        /// <summary>
        /// 强制虚拟相机假定给定的位置和方向
        /// </summary>
        /// <param name="pos">要采用的世界空间位置</param>
        /// <param name="rot">要采用的世界空间方向</param>
        public virtual void ForceCameraPosition(Vector3 pos, Quaternion rot)
        {
            // 通知扩展
            if (Extensions != null)
            {
                for (int i = 0; i < Extensions.Count; ++i)
                    Extensions[i].ForceCameraPosition(pos, rot);
            }
        }

        /// <summary>
        /// 基于此 vcam 的当前 Transform 创建相机状态
        /// </summary>
        /// <param name="worldUp">当前的世界 Up 方向，由大脑提供</param>
        /// <param name="lens">镜头设置，将作为基础，如果存在大脑的镜头设置，则会与其结合</param>
        /// <returns>基于此 vcam 当前 Transform 的 CameraState。</returns>
        protected CameraState PullStateFromVirtualCamera(Vector3 worldUp, ref LensSettings lens)
        {
            CameraState state = CameraState.Default;
            state.RawPosition = TargetPositionCache.GetTargetPosition(transform);
            state.RawOrientation = TargetPositionCache.GetTargetRotation(transform);
            state.ReferenceUp = worldUp;

            CinemachineBrain brain = CinemachineCore.FindPotentialTargetBrain(this);
            if (brain != null && brain.OutputCamera != null)
                lens.PullInheritedPropertiesFromCamera(brain.OutputCamera);

            state.Lens = lens;
            return state;
        }

        void InvalidateCachedTargets()
        {
            m_CachedFollowTarget = null;
            m_CachedFollowTargetVcam = null;
            m_CachedFollowTargetGroup = null;
            m_CachedLookAtTarget = null;
            m_CachedLookAtTargetVcam = null;
            m_CachedLookAtTargetGroup = null;
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoad]
        class OnDomainReload 
        { 
            static OnDomainReload() 
            {
#if UNITY_2023_1_OR_NEWER
                var vcams = FindObjectsByType<CinemachineVirtualCameraBase>
                    (FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
                var vcams = FindObjectsOfType<CinemachineVirtualCameraBase>(true);
#endif
                for (int i = 0; i < vcams.Length; ++i)
                    vcams[i].InvalidateCachedTargets();
            }
        }
#endif

        /// <summary>
        /// 如果此帧 Follow 目标已更改，则此属性为 true。
        /// </summary>
        public bool FollowTargetChanged { get; private set; }

        /// <summary>
        /// 如果此帧 LookAt 目标已更改，则此属性为 true。
        /// </summary>
        public bool LookAtTargetChanged { get; private set; }

        /// <summary>
        /// 从 InternalUpdateCameraState() 调用此方法以检查目标是否已更改并更新目标缓存。
        /// 这是跟踪目标对象更改时所需的。
        /// </summary>
        public void UpdateTargetCache()
        {
            var target = ResolveFollow(Follow);
            FollowTargetChanged = target != m_CachedFollowTarget;
            if (FollowTargetChanged)
            {
                m_CachedFollowTarget = target;
                m_CachedFollowTargetVcam = null;
                m_CachedFollowTargetGroup = null;
                if (m_CachedFollowTarget != null)
                {
                    target.TryGetComponent(out m_CachedFollowTargetVcam);
                    target.TryGetComponent(out m_CachedFollowTargetGroup);
                }
            }
            target = ResolveLookAt(LookAt);
            LookAtTargetChanged = target != m_CachedLookAtTarget;
            if (LookAtTargetChanged)
            {
                m_CachedLookAtTarget = target;
                m_CachedLookAtTargetVcam = null;
                m_CachedLookAtTargetGroup = null;
                if (target != null)
                {
                    target.TryGetComponent(out m_CachedLookAtTargetVcam);
                    target.TryGetComponent(out m_CachedLookAtTargetGroup);
                }
            }
        }

        /// <summary>将 Follow 目标作为 ICinemachineTargetGroup 返回，如果目标不是 ICinemachineTargetGroup 则返回 null</summary>
        public ICinemachineTargetGroup FollowTargetAsGroup => m_CachedFollowTargetGroup;

        /// <summary>将 Follow 目标作为 CinemachineVirtualCameraBase 返回，如果目标不是 CinemachineVirtualCameraBase 则返回 null</summary>
        public CinemachineVirtualCameraBase FollowTargetAsVcam => m_CachedFollowTargetVcam;

        /// <summary>将 LookAt 目标作为 ICinemachineTargetGroup 返回，如果目标不是 ICinemachineTargetGroup 则返回 null</summary>
        public ICinemachineTargetGroup LookAtTargetAsGroup => m_CachedLookAtTargetGroup;

        /// <summary>将 LookAt 目标作为 CinemachineVirtualCameraBase 返回，如果目标不是 CinemachineVirtualCameraBase 则返回 null</summary>
        public CinemachineVirtualCameraBase LookAtTargetAsVcam => m_CachedLookAtTargetVcam;

        /// <summary>获取管道中特定阶段的组件集。</summary>
        /// <param name="stage">我们想要组件的阶段</param>
        /// <returns>该阶段的 Cinemachine 组件，如果不存在则返回 null。</returns>
        public virtual CinemachineComponentBase GetCinemachineComponent(CinemachineCore.Stage stage) => null;

        /// <summary>如果此相机当前对某个 CinemachineBrain 处于活动状态，则返回 true。</summary>
        public bool IsLive => CinemachineCore.IsLive(this);

        /// <summary>检查此相机当前是否在其父管理器或 CinemacineBrain 中参与混合</summary>
        /// <returns>如果相机参与混合，则返回 true</returns>
        public bool IsParticipatingInBlend()
        {
            if (IsLive)
            {
                var parent = ParentCamera as CinemachineCameraManagerBase;
                if (parent != null)
                    return (parent.ActiveBlend != null && parent.ActiveBlend.Uses(this)) || parent.IsParticipatingInBlend();
                var brain = CinemachineCore.FindPotentialTargetBrain(this);
                if (brain != null)
                    return brain.ActiveBlend != null && brain.ActiveBlend.Uses(this);
            }
            return false;
        }

        /// <summary>
        /// 暂时取消此帧的阻尼。相机将在更新时捕捉到其目标位置。
        /// </summary>
        /// <param name="updateNow">如果为 true，则立即将相机捕捉到其目标，否则等待到帧结束时相机通常更新的时间。</param>
        public void CancelDamping(bool updateNow = false)
        {
            PreviousStateIsValid = false;
            if (updateNow)
            {
                var up = State.ReferenceUp;
                var brain = CinemachineCore.FindPotentialTargetBrain(this);
                if (brain != null)
                    up = brain.DefaultWorldUp;
                InternalUpdateCameraState(up, -1);
            }
        }
    }
}
```





## CinemachineThirdPersonFollow代码注释翻译

是一个用于第三人称相机的组件，支持复杂的相机旋转和避障功能。

```csharp
/// <summary>
/// 第三人称跟随相机组件，支持复杂的旋转：水平围绕原点旋转，垂直围绕肩部旋转。
/// </summary>
[AddComponentMenu("Cinemachine/Procedural/Position Control/Cinemachine Third Person Follow")]
[SaveDuringPlay]
[DisallowMultipleComponent]
[CameraPipeline(CinemachineCore.Stage.Body)]
[HelpURL(Documentation.BaseURL + "manual/CinemachineThirdPersonFollow.html")]
public class CinemachineThirdPersonFollow : CinemachineComponentBase
    , CinemachineFreeLookModifier.IModifierValueSource
    , CinemachineFreeLookModifier.IModifiablePositionDamping
    , CinemachineFreeLookModifier.IModifiableDistance
{
    /// <summary>相机对目标位置变化的响应速度。每个轴（相机局部坐标系）可以单独设置。
    /// 值表示相机追上目标新位置所需的大致时间。较小的值会产生更刚性的效果，较大的值会产生更柔软的效果。</summary>
    [Tooltip("相机对目标位置变化的响应速度。每个轴（相机局部坐标系）可以单独设置。"
       + "值表示相机追上目标新位置所需的大致时间。较小的值会产生更刚性的效果，较大的值会产生更柔软的效果")]
    public Vector3 Damping;

    /// <summary>肩部枢轴点相对于目标原点的位置。此偏移是在目标局部空间中定义的。</summary>
    [Header("Rig")]
    [Tooltip("肩部枢轴点相对于目标原点的位置。此偏移是在目标局部空间中定义的")]
    public Vector3 ShoulderOffset;

    /// <summary>手部相对于肩部的垂直偏移。臂长会影响相机垂直旋转时目标在屏幕上的位置。</summary>
    [Tooltip("手部相对于肩部的垂直偏移。臂长会影响相机垂直旋转时目标在屏幕上的位置")]
    public float VerticalArmLength;

    /// <summary>指定相机位于目标的哪一侧（左、右或中间）。</summary>
    [Tooltip("指定相机位于目标的哪一侧（左、右或中间）")]
    [Range(0, 1)]
    public float CameraSide;

    /// <summary>相机与手部之间的距离。</summary>
    [Tooltip("相机与手部之间的距离")]
    public float CameraDistance;

#if CINEMACHINE_PHYSICS
    /// <summary>
    /// 碰撞分辨率设置。
    /// </summary>
    [Serializable]
    public struct ObstacleSettings
    {
        /// <summary>启用或禁用障碍物处理。如果启用，相机将被拉近以避免遮挡。</summary>
        [Tooltip("如果启用，相机将被拉近以避免遮挡")]
        public bool Enabled;
        
        /// <summary>相机将避开这些层上的障碍物。</summary>
        [Tooltip("相机将避开这些层上的障碍物")]
        public LayerMask CollisionFilter;

        /// <summary>
        /// 具有此标签的障碍物将被忽略。建议将此字段设置为目标的标签。
        /// </summary>
        [TagField]
        [Tooltip("具有此标签的障碍物将被忽略。建议将此字段设置为目标的标签")]
        public string IgnoreTag;

        /// <summary>
        /// 指定相机可以接近障碍物的距离。
        /// </summary>
        [Tooltip("指定相机可以接近障碍物的距离")]
        [Range(0, 1)]
        public float CameraRadius;
    
        /// <summary>
        /// 相机逐渐移动到避免遮挡的位置的速度。较高的值会使相机移动得更缓慢。
        /// </summary>
        [Range(0, 10)]
        [Tooltip("相机逐渐移动到避免遮挡的位置的速度。较高的值会使相机移动得更缓慢")]
        public float DampingIntoCollision;

        /// <summary>
        /// 相机在碰撞解决后逐渐恢复到正常位置的速度。较高的值会使相机恢复得更缓慢。
        /// </summary>
        [Range(0, 10)]
        [Tooltip("相机在碰撞解决后逐渐恢复到正常位置的速度。较高的值会使相机恢复得更缓慢")]
        public float DampingFromCollision;

        internal static ObstacleSettings Default => new()
        {
            Enabled = false,
            CollisionFilter = 1,
            IgnoreTag = string.Empty,
            CameraRadius = 0.2f,
            DampingIntoCollision = 0,
            DampingFromCollision = 0.5f
        };
    }
    
    /// <summary>如果启用，相机将被拉近以避免遮挡。</summary>
    [FoldoutWithEnabledButton]
    public ObstacleSettings AvoidObstacles = ObstacleSettings.Default;

    /// <summary>当前正在避开的障碍物。</summary>
    public Collider CurrentObstacle { get; set; }
#endif

    // 状态信息
    Vector3 m_PreviousFollowTargetPosition;
    Vector3 m_DampingCorrection; // 这是在局部 rig 空间中的
#if CINEMACHINE_PHYSICS
    float m_CamPosCollisionCorrection;
#endif

    void OnValidate()
    {
        CameraSide = Mathf.Clamp(CameraSide, -1.0f, 1.0f);
        Damping.x = Mathf.Max(0, Damping.x);
        Damping.y = Mathf.Max(0, Damping.y);
        Damping.z = Mathf.Max(0, Damping.z);
#if CINEMACHINE_PHYSICS
        AvoidObstacles.CameraRadius = Mathf.Max(0.001f, AvoidObstacles.CameraRadius);
        AvoidObstacles.DampingIntoCollision = Mathf.Max(0, AvoidObstacles.DampingIntoCollision);
        AvoidObstacles.DampingFromCollision = Mathf.Max(0, AvoidObstacles.DampingFromCollision);
#endif
    }

    void Reset()
    {
        ShoulderOffset = new Vector3(0.5f, -0.4f, 0.0f);
        VerticalArmLength = 0.4f;
        CameraSide = 1.0f;
        CameraDistance = 2.0f;
        Damping = new Vector3(0.1f, 0.5f, 0.3f);
#if CINEMACHINE_PHYSICS
        AvoidObstacles = ObstacleSettings.Default;
#endif
    }

    float CinemachineFreeLookModifier.IModifierValueSource.NormalizedModifierValue
    {
        get
        {
            var up = VirtualCamera.State.ReferenceUp;
            var rot = FollowTargetRotation;
            var a = Vector3.SignedAngle(rot * Vector3.up, up, rot * Vector3.right);
            return Mathf.Clamp(a, -90, 90) / -90;
        }
    }

    Vector3 CinemachineFreeLookModifier.IModifiablePositionDamping.PositionDamping
    {
        get => Damping;
        set => Damping = value;
    }

    float CinemachineFreeLookModifier.IModifiableDistance.Distance
    {
        get => CameraDistance;
        set => CameraDistance = value;
    }
    
    /// <summary>如果组件已启用并且定义了 Follow 目标，则返回 true。</summary>
    public override bool IsValid => enabled && FollowTarget != null;

    /// <summary>获取此组件实现的 Cinemachine 管道阶段。始终返回 Aim 阶段。</summary>
    public override CinemachineCore.Stage Stage { get => CinemachineCore.Stage.Body; }

    /// <summary>
    /// 报告此组件所需的最大阻尼时间。
    /// </summary>
    /// <returns>此组件中的最高阻尼设置</returns>
    public override float GetMaxDampTime() 
    { 
        return Mathf.Max(
#if CINEMACHINE_PHYSICS
            AvoidObstacles.Enabled ? Mathf.Max(
                AvoidObstacles.DampingIntoCollision, AvoidObstacles.DampingFromCollision) : 0,
#else
            0,
#endif
            Mathf.Max(Damping.x, Mathf.Max(Damping.y, Damping.z)));
    }

    /// <summary>将相机定向以匹配 Follow 目标的方向。</summary>
    /// <param name="curState">当前相机状态</param>
    /// <param name="deltaTime">自上一帧以来的经过时间，用于阻尼计算。如果为负，则重置前一状态。</param>
    public override void MutateCameraState(ref CameraState curState, float deltaTime)
    {
        if (IsValid)
        {
            if (!VirtualCamera.PreviousStateIsValid)
                deltaTime = -1;
            PositionCamera(ref curState, deltaTime);
        }
    }

    /// <summary>当目标发生瞬移时调用此方法，以便更新内部状态，使相机也能无缝瞬移。</summary>
    /// <param name="target">被瞬移的对象</param>
    /// <param name="positionDelta">目标位置的变化量</param>
    public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
    {
        base.OnTargetObjectWarped(target, positionDelta);
        if (target == FollowTarget)
            m_PreviousFollowTargetPosition += positionDelta;
    }
    
    void PositionCamera(ref CameraState curState, float deltaTime)
    {
        var up = curState.ReferenceUp;
        var targetPos = FollowTargetPosition;
        var targetRot = FollowTargetRotation;
        var targetForward = targetRot * Vector3.forward;
        var heading = GetHeading(targetRot, up);

        if (deltaTime < 0)
        {
            // 无阻尼 - 重置阻尼状态信息
            m_DampingCorrection = Vector3.zero;
#if CINEMACHINE_PHYSICS
            m_CamPosCollisionCorrection = 0;
#endif
        }
        else
        {
            // 阻尼校正应用于肩部偏移 - 拉伸 rig
            m_DampingCorrection += Quaternion.Inverse(heading) * (m_PreviousFollowTargetPosition - targetPos);
            m_DampingCorrection -= VirtualCamera.DetachedFollowTargetDamp(m_DampingCorrection, Damping, deltaTime);
        }

        m_PreviousFollowTargetPosition = targetPos;
        var root = targetPos;
        GetRawRigPositions(root, targetRot, heading, out _, out Vector3 hand);

        // 将相机放置在距离手部的正确距离处
        var camPos = hand - (targetForward * (CameraDistance - m_DampingCorrection.z));

#if CINEMACHINE_PHYSICS
        CurrentObstacle = null;
        if (AvoidObstacles.Enabled)
        {
            // 检查手部是否与某物碰撞，如果是，则将手部拉近玩家。半径略微放大，以避免靠近墙壁时出现问题
            float dummy = 0;
            var collidedHand = ResolveCollisions(root, hand, -1, AvoidObstacles.CameraRadius * 1.05f, ref dummy);
            camPos = ResolveCollisions(
                collidedHand, camPos, deltaTime, AvoidObstacles.CameraRadius, ref m_CamPosCollisionCorrection);
        }
#endif
        // 设置状态
        curState.RawPosition = camPos;
        curState.RawOrientation = targetRot; // 不是必需的，但保留以避免破坏依赖此的场景

        // 修正默认情况下我们看向 Follow 目标的情况
        if (!curState.HasLookAt() || curState.ReferenceLookAt.Equals(targetPos))
            curState.ReferenceLookAt = camPos + targetRot * new Vector3(0, 0, 3); // 以便有东西可看
    }
    
    /// <summary>
    /// 仅供内部使用。公开用于检查器中的 Gizmo。
    /// </summary>
    /// <param name="root">Rig 的根。</param>
    /// <param name="shoulder">Rig 的肩部。</param>
    /// <param name="hand">Rig 的手部。</param>
    public void GetRigPositions(out Vector3 root, out Vector3 shoulder, out Vector3 hand)
    {
        var up = VirtualCamera.State.ReferenceUp;
        var targetRot = FollowTargetRotation;
        var heading = GetHeading(targetRot, up);
        root = m_PreviousFollowTargetPosition;
        GetRawRigPositions(root, targetRot, heading, out shoulder, out hand);
#if CINEMACHINE_PHYSICS
        if (AvoidObstacles.Enabled)
        {
            float dummy = 0;
            hand = ResolveCollisions(root, hand, -1, AvoidObstacles.CameraRadius * 1.05f, ref dummy);
        }
#endif
    }

    internal static Quaternion GetHeading(Quaternion targetRot, Vector3 up)
    {
        var targetForward = targetRot * Vector3.forward;
        var planeForward = Vector3.Cross(up, Vector3.Cross(targetForward.ProjectOntoPlane(up), up));
        if (planeForward.AlmostZero())
            planeForward = Vector3.Cross(targetRot * Vector3.right, up);
        return Quaternion.LookRotation(planeForward, up);
    }

    void GetRawRigPositions(
        Vector3 root, Quaternion targetRot, Quaternion heading, 
        out Vector3 shoulder, out Vector3 hand)
    {
        var shoulderOffset = ShoulderOffset;
        shoulderOffset.x = Mathf.Lerp(-shoulderOffset.x, shoulderOffset.x, CameraSide);
        shoulderOffset.x += m_DampingCorrection.x;
        shoulderOffset.y += m_DampingCorrection.y;
        shoulder = root + heading * shoulderOffset;
        hand = shoulder + targetRot * new Vector3(0, VerticalArmLength, 0);   
    }

#if CINEMACHINE_PHYSICS
    Vector3 ResolveCollisions(
        Vector3 root, Vector3 tip, float deltaTime, 
        float cameraRadius, ref float collisionCorrection)
    {
        if (AvoidObstacles.CollisionFilter.value == 0)
            return tip;
        
        var dir = tip - root;
        var len = dir.magnitude;
        if (len < Epsilon)
            return tip;
        dir /= len;

        var result = tip;
        float desiredCorrection = 0;

        if (RuntimeUtility.SphereCastIgnoreTag(
            new Ray(root, dir), cameraRadius, out RaycastHit hitInfo, 
            len, AvoidObstacles.CollisionFilter, AvoidObstacles.IgnoreTag))
        {
            CurrentObstacle = hitInfo.collider;
            var desiredResult = hitInfo.point + hitInfo.normal * cameraRadius;
            desiredCorrection = (desiredResult - tip).magnitude;
        }

        collisionCorrection += deltaTime < 0 ? desiredCorrection - collisionCorrection : Damper.Damp(
            desiredCorrection - collisionCorrection, 
            desiredCorrection > collisionCorrection ? AvoidObstacles.DampingIntoCollision : AvoidObstacles.DampingFromCollision, 
            deltaTime);

        // 应用校正
        if (collisionCorrection > Epsilon)
            result -= dir * collisionCorrection;

        return result;
    }
#endif
}
```

## CinemachineFreeLookModifier代码注释翻译

是一个用于 FreeLook 相机的扩展组件，支持根据垂直轴的值动态修改相机参数（如倾斜、镜头设置、位置阻尼等）。

```csharp
/// <summary>
/// 这是 CinemachineCameras 的附加组件，包含 OrbitalFollow 组件。
/// 它根据垂直角度修改相机距离。
/// </summary>
[SaveDuringPlay] 
[AddComponentMenu("Cinemachine/Procedural/Extensions/Cinemachine FreeLook Modifier")] // 在菜单中隐藏
[ExecuteAlways]
[DisallowMultipleComponent]
[HelpURL(Documentation.BaseURL + "manual/CinemachineFreeLookModifier.html")]
public class CinemachineFreeLookModifier : CinemachineExtension
{
    /// <summary>
    /// 用于 CinemachineComponentBase 派生类的接口，以暴露一个标准化值，
    /// 该值可由 CinemachineFreeLookModifier 使用以驱动 rig 选择。
    /// </summary>
    public interface IModifierValueSource
    {
        /// <summary>
        /// 此值对于中间 rig 为 0，对于底部 rig 为 -1，对于顶部 rig 为 1。
        /// 介于两者之间的值表示 rig 之间的混合。
        /// </summary>
        float NormalizedModifierValue { get; }
    }

    /// <summary>
    /// 用于 CinemachineComponentBase 派生类的接口，以允许其位置阻尼被驱动。
    /// </summary>
    public interface IModifiablePositionDamping
    {
        /// <summary>获取/设置位置阻尼值</summary>
        Vector3 PositionDamping { get; set; }
    }

    /// <summary>
    /// 用于 CinemachineComponentBase 派生类的接口，以允许其屏幕构图被驱动。
    /// </summary>
    public interface IModifiableComposition
    {
        /// <summary>获取/设置屏幕位置</summary>
        ScreenComposerSettings Composition { get; set; }
    }

    /// <summary>
    /// 用于 CinemachineComponentBase 派生类的接口，以允许相机距离被修改。
    /// </summary>
    public interface IModifiableDistance
    {
        /// <summary>获取/设置相机距离</summary>
        float Distance { get; set; }
    }

    /// <summary>
    /// 用于 CinemachineComponentBase 派生类的接口，以允许噪声振幅和频率被修改。
    /// </summary>
    public interface IModifiableNoise
    {
        /// <summary>获取/设置噪声振幅和频率</summary>
        (float, float) NoiseAmplitudeFrequency { get; set; }
    }

    /// <summary>
    /// 用于根据垂直轴值修改 FreeLook 相机某些方面的对象的接口。
    /// </summary>
    [Serializable]
    public abstract class Modifier
    {
        /// <summary>在编辑器中从 OnValidate 调用。验证并清理字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public virtual void Validate(CinemachineVirtualCameraBase vcam) {}

        /// <summary>当修改器创建时调用。使用适当的值初始化字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public virtual void Reset(CinemachineVirtualCameraBase vcam) {}

        /// <summary>
        /// 缓存组件的类型（如果没有缓存组件则为 null）。如果此修改器针对特定组件，
        /// 此值表示该组件的类型。修改器应缓存该组件以提高性能。
        /// <see cref="ComponentModifier"/> 是实现此功能的基类。
        /// </summary>
        public virtual Type CachedComponentType => null;

        /// <summary>如果缓存了 vcam 组件或不需要，则返回 true</summary>
        public virtual bool HasRequiredComponent => true;

        /// <summary>从 OnEnable 和检查器中调用。刷新任何性能敏感的内容。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public virtual void RefreshCache(CinemachineVirtualCameraBase vcam) {}
        
        /// <summary>
        /// 从扩展的 PrePipelineMutateCameraState() 调用。执行任何必要的操作以修改相关相机设置。
        /// 原始相机设置应在 <see cref="AfterPipeline"/> 中恢复。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public virtual void BeforePipeline(
            CinemachineVirtualCameraBase vcam, 
            ref CameraState state, float deltaTime, 
            float modifierValue) {}

        /// <summary>
        /// 从扩展的 PostPipelineStageCallback(Finalize) 调用。执行任何必要的操作以修改状态，
        /// 并恢复在 <see cref="BeforePipeline"/> 中更改的任何相机参数。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public virtual void AfterPipeline(
            CinemachineVirtualCameraBase vcam,
            ref CameraState state, float deltaTime,
            float modifierValue) {}
    }

    /// <summary>
    /// 用于修改单个 CinemachineComponentBase 内部内容的修改器。
    /// </summary>
    /// <typeparam name="T">要修改的内容的类型。</typeparam>
    public abstract class ComponentModifier<T> : Modifier
    {
        /// <summary>将被修改的 CinemachineComponentBase。在此缓存以提高效率。</summary>
        protected T CachedComponent;

        /// <summary>如果 CinemachineCamera 具有我们要修改的组件，则返回 true。</summary>
        public override bool HasRequiredComponent => CachedComponent != null;

        /// <summary>被修改的组件的类型</summary>
        public override Type CachedComponentType => typeof(T);

        /// <summary>从 OnEnable 和检查器中调用。刷新 CachedComponent。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void RefreshCache(CinemachineVirtualCameraBase vcam) => TryGetVcamComponent(vcam, out CachedComponent);
    }

    /// <summary>
    /// 内置的 FreeLook 修改器，用于相机倾斜。在相机管道的末尾应用垂直旋转。
    /// </summary>
    public class TiltModifier : Modifier
    {
        /// <summary>顶部和底部 rig 的值</summary>
        [HideFoldout]
        public TopBottomRigs<float> Tilt;

        /// <summary>从 OnValidate 调用以验证此组件</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Validate(CinemachineVirtualCameraBase vcam)
        {
            Tilt.Top = Mathf.Clamp(Tilt.Top, -30, 30);
            Tilt.Bottom = Mathf.Clamp(Tilt.Bottom, -30, 30);
        }

        /// <summary>当修改器创建时调用。使用适当的值初始化字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Reset(CinemachineVirtualCameraBase vcam) 
            => Tilt = new TopBottomRigs<float>() { Top = -5, Bottom = 5 };

        /// <summary>
        /// 从扩展的 PostPipelineStageCallback(Finalize) 调用。执行任何必要的操作以修改状态，
        /// 并恢复在 <see cref="BeforePipeline"/> 中更改的任何相机参数。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void AfterPipeline(
            CinemachineVirtualCameraBase vcam,
            ref CameraState state, float deltaTime,
            float modifierValue)
        {
            float tilt = modifierValue > 0 
                ? Mathf.Lerp(0, Tilt.Top, modifierValue) 
                : Mathf.Lerp(Tilt.Bottom, 0, modifierValue + 1);

            // 在局部 X 轴上倾斜
            var qTilted = state.RawOrientation * Quaternion.AngleAxis(tilt, Vector3.right);
            state.OrientationCorrection = Quaternion.Inverse(state.GetCorrectedOrientation()) * qTilted;
        }
    }

    /// <summary>
    /// 内置的修改器，用于相机镜头。在相机管道的开头应用镜头。
    /// </summary>
    public class LensModifier : Modifier
    {
        /// <summary>顶部轨道的设置</summary>
        [Tooltip("在轴范围顶部采用的值")]
        [LensSettingsHideModeOverrideProperty]
        public LensSettings Top;

        /// <summary>底部轨道的设置</summary>
        [Tooltip("在轴范围底部采用的值")]
        [LensSettingsHideModeOverrideProperty]
        public LensSettings Bottom;
    
        /// <summary>从 OnValidate 调用以验证此组件</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Validate(CinemachineVirtualCameraBase vcam) 
        {
            Top.Validate();
            Bottom.Validate();
        }

        /// <summary>当修改器创建时调用。使用适当的值初始化字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Reset(CinemachineVirtualCameraBase vcam) 
        {
            if (vcam == null)
                Top = Bottom = LensSettings.Default;
            else 
            {
                var state = vcam.State;
                Top = Bottom = state.Lens;
                Top.CopyCameraMode(ref state.Lens);
                Bottom.CopyCameraMode(ref state.Lens);
            }
        }

        /// <summary>
        /// 从扩展的 PrePipelineMutateCameraState() 调用。执行任何必要的操作以修改相关相机设置。
        /// 原始相机设置应在 <see cref="AfterPipeline"/> 中恢复。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void BeforePipeline(
            CinemachineVirtualCameraBase vcam, 
            ref CameraState state, float deltaTime, float modifierValue) 
        {
            Top.CopyCameraMode(ref state.Lens);
            Bottom.CopyCameraMode(ref state.Lens);
            if (modifierValue >= 0)
                state.Lens.Lerp(Top, modifierValue);
            else
                state.Lens.Lerp(Bottom, -modifierValue);
        }
    }

    /// <summary>
    /// 内置的 FreeLook 修改器，用于位置阻尼。在相机管道的开头修改位置阻尼。
    /// </summary>
    public class PositionDampingModifier : ComponentModifier<IModifiablePositionDamping>
    {
        /// <summary>顶部和底部 rig 的值</summary>
        [HideFoldout]
        public TopBottomRigs<Vector3> Damping;

        /// <summary>从 OnValidate 调用以验证此组件</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Validate(CinemachineVirtualCameraBase vcam)
        {
            Damping.Top = new Vector3(Mathf.Max(0, Damping.Top.x), Mathf.Max(0, Damping.Top.y), Mathf.Max(0, Damping.Top.z));
            Damping.Bottom = new Vector3(Mathf.Max(0, Damping.Bottom.x), Mathf.Max(0, Damping.Bottom.y), Mathf.Max(0, Damping.Bottom.z));
        }

        /// <summary>当修改器创建时调用。使用适当的值初始化字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Reset(CinemachineVirtualCameraBase vcam) 
        {
            if (CachedComponent != null)
                Damping.Top = Damping.Bottom = CachedComponent.PositionDamping;
        }

        Vector3 m_CenterDamping;

        /// <summary>
        /// 从扩展的 PrePipelineMutateCameraState() 调用。执行任何必要的操作以修改相关相机设置。
        /// 原始相机设置应在 <see cref="AfterPipeline"/> 中恢复。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void BeforePipeline(
            CinemachineVirtualCameraBase vcam, 
            ref CameraState state, float deltaTime, float modifierValue) 
        {
            if (CachedComponent != null)
            {
                m_CenterDamping = CachedComponent.PositionDamping;
                CachedComponent.PositionDamping = modifierValue >= 0 
                    ? Vector3.Lerp(m_CenterDamping, Damping.Top, modifierValue)
                    : Vector3.Lerp(Damping.Bottom, m_CenterDamping, modifierValue + 1);
            }
        }

        /// <summary>
        /// 从扩展的 PostPipelineStageCallback(Finalize) 调用。执行任何必要的操作以修改状态，
        /// 并恢复在 <see cref="BeforePipeline"/> 中更改的任何相机参数。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void AfterPipeline(
            CinemachineVirtualCameraBase vcam,
            ref CameraState state, float deltaTime,
            float modifierValue)
        {
            // 恢复设置
            if (CachedComponent != null)
                CachedComponent.PositionDamping = m_CenterDamping;
        }
    }

    /// <summary>
    /// 内置的 Freelook 修改器，用于屏幕构图。在相机管道的开头修改构图。
    /// </summary>
    public class CompositionModifier : ComponentModifier<IModifiableComposition>
    {
        /// <summary>顶部和底部 rig 的值</summary>
        [HideFoldout]
        public TopBottomRigs<ScreenComposerSettings> Composition;

        /// <summary>从 OnValidate 调用以验证此组件</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Validate(CinemachineVirtualCameraBase vcam)
        {
            Composition.Top.Validate();
            Composition.Bottom.Validate();
        }

        /// <summary>当修改器创建时调用。使用适当的值初始化字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Reset(CinemachineVirtualCameraBase vcam) 
        {
            if (CachedComponent != null)
                Composition.Top = Composition.Bottom = CachedComponent.Composition;
        }

        ScreenComposerSettings m_SavedComposition;

        /// <summary>
        /// 从扩展的 PrePipelineMutateCameraState() 调用。执行任何必要的操作以修改相关相机设置。
        /// 原始相机设置应在 <see cref="AfterPipeline"/> 中恢复。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void BeforePipeline(
            CinemachineVirtualCameraBase vcam, 
            ref CameraState state, float deltaTime, float modifierValue) 
        {
            if (CachedComponent != null)
            {
                m_SavedComposition = CachedComponent.Composition;
                CachedComponent.Composition = modifierValue >= 0
                    ? ScreenComposerSettings.Lerp(m_SavedComposition, Composition.Top, modifierValue)
                    : ScreenComposerSettings.Lerp(Composition.Bottom, m_SavedComposition, modifierValue + 1);
            }
        }

        /// <summary>
        /// 从扩展的 PostPipelineStageCallback(Finalize) 调用。执行任何必要的操作以修改状态，
        /// 并恢复在 <see cref="BeforePipeline"/> 中更改的任何相机参数。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void AfterPipeline(
            CinemachineVirtualCameraBase vcam,
            ref CameraState state, float deltaTime,
            float modifierValue)
        {
            // 恢复设置
            if (CachedComponent != null)
            {
                CachedComponent.Composition = m_SavedComposition;
            }
        }
    }

    /// <summary>
    /// 内置的 FreeLook 修改器，用于相机距离。在相机管道的开头应用距离。
    /// </summary>
    public class DistanceModifier : ComponentModifier<IModifiableDistance>
    {
        /// <summary>顶部和底部 rig 的值</summary>
        [HideFoldout]
        public TopBottomRigs<float> Distance;

        /// <summary>从 OnValidate 调用以验证此组件</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Validate(CinemachineVirtualCameraBase vcam)
        {
            Distance.Top = Mathf.Max(0, Distance.Top);
            Distance.Bottom = Mathf.Max(0, Distance.Bottom);
        }

        /// <summary>当修改器创建时调用。使用适当的值初始化字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Reset(CinemachineVirtualCameraBase vcam) 
        {
            if (CachedComponent != null)
                Distance.Top = Distance.Bottom = CachedComponent.Distance;
        }

        float m_CenterDistance;

        /// <summary>
        /// 从扩展的 PrePipelineMutateCameraState() 调用。执行任何必要的操作以修改相关相机设置。
        /// 原始相机设置应在 <see cref="AfterPipeline"/> 中恢复。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void BeforePipeline(
            CinemachineVirtualCameraBase vcam, 
            ref CameraState state, float deltaTime, float modifierValue) 
        {
            if (CachedComponent != null)
            {
                m_CenterDistance = CachedComponent.Distance;
                CachedComponent.Distance = modifierValue >= 0 
                    ? Mathf.Lerp(m_CenterDistance, Distance.Top, modifierValue)
                    : Mathf.Lerp(Distance.Bottom, m_CenterDistance, modifierValue + 1);
            }
        }

        /// <summary>
        /// 从扩展的 PostPipelineStageCallback(Finalize) 调用。执行任何必要的操作以修改状态，
        /// 并恢复在 <see cref="BeforePipeline"/> 中更改的任何相机参数。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void AfterPipeline(
            CinemachineVirtualCameraBase vcam,
            ref CameraState state, float deltaTime,
            float modifierValue)
        {
            // 恢复设置
            if (CachedComponent != null)
                CachedComponent.Distance = m_CenterDistance;
        }
    }

    /// <summary>
    /// 内置的修改器，用于噪声组件，例如 <see cref="CinemachineBasicMultiChannelPerlin"/>。
    /// 应用振幅和频率的缩放。
    /// </summary>
    public class NoiseModifier : ComponentModifier<IModifiableNoise>
    {
        /// <summary>
        /// 应用于 IModifiableNoise 组件的设置
        /// </summary>
        [Serializable]
        public struct NoiseSettings
        {
            /// <summary>噪声振幅的乘数</summary>
            [Tooltip("噪声振幅的乘数")]
            public float Amplitude;

            /// <summary>噪声频率的乘数</summary>
            [Tooltip("噪声频率的乘数")]
            public float Frequency;
        }

        /// <summary>顶部和底部 rig 的值</summary>
        [HideFoldout]
        public TopBottomRigs<NoiseSettings> Noise;

        (float, float) m_CenterNoise;

        /// <summary>当修改器创建时调用。使用适当的值初始化字段。</summary>
        /// <param name="vcam">虚拟相机所有者</param>
        public override void Reset(CinemachineVirtualCameraBase vcam) 
        {
            if (CachedComponent != null)
            {
                var value = CachedComponent.NoiseAmplitudeFrequency;
                Noise.Top = Noise.Bottom = new NoiseSettings { Amplitude = value.Item1, Frequency = value.Item2 };
            }
        }

        /// <summary>
        /// 从扩展的 PrePipelineMutateCameraState() 调用。执行任何必要的操作以修改相关相机设置。
        /// 原始相机设置应在 <see cref="AfterPipeline"/> 中恢复。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void BeforePipeline(
            CinemachineVirtualCameraBase vcam, 
            ref CameraState state, float deltaTime, float modifierValue) 
        {
            if (CachedComponent != null)
            {
                m_CenterNoise = CachedComponent.NoiseAmplitudeFrequency;
                if (modifierValue >= 0)
                    CachedComponent.NoiseAmplitudeFrequency = (
                        Mathf.Lerp(m_CenterNoise.Item1, Noise.Top.Amplitude, modifierValue),
                        Mathf.Lerp(m_CenterNoise.Item2, Noise.Top.Frequency, modifierValue));
                else
                    CachedComponent.NoiseAmplitudeFrequency = (
                        Mathf.Lerp(Noise.Bottom.Amplitude, m_CenterNoise.Item1, modifierValue + 1),
                        Mathf.Lerp(Noise.Bottom.Frequency, m_CenterNoise.Item2, modifierValue + 1));
            }
        }

        /// <summary>
        /// 从扩展的 PostPipelineStageCallback(Finalize) 调用。执行任何必要的操作以修改状态，
        /// 并恢复在 <see cref="BeforePipeline"/> 中更改的任何相机参数。
        /// </summary>
        /// <param name="vcam">vcam 所有者</param>
        /// <param name="state">当前 vcam 状态。可以在此函数中修改</param>
        /// <param name="deltaTime">当前适用的 deltaTime</param>
        /// <param name="modifierValue">修改器变量的标准化值。这是 FreeLook 的垂直轴。
        /// 范围从 -1 到 1，其中 0 是中间 rig。</param>
        public override void AfterPipeline(
            CinemachineVirtualCameraBase vcam,
            ref CameraState state, float deltaTime,
            float modifierValue) 
        {
            // 恢复设置
            if (CachedComponent != null)
                CachedComponent.NoiseAmplitudeFrequency = m_CenterNoise;
        }
    }

    /// <summary>
    /// 用于保存顶部、中间和底部轨道设置的辅助结构。
    /// </summary>
    /// <typeparam name="T">保存值的对象的类型。</typeparam>
    [Serializable]
    public struct TopBottomRigs<T>
    {
        /// <summary>顶部轨道的设置</summary>
        [Tooltip("在轴范围顶部采用的值")]
        public T Top;

        /// <summary>底部轨道的设置</summary>
        [Tooltip("在轴范围底部采用的值")]
        public T Bottom;
    }

    /// <summary>
    /// 将每帧应用于相机的修改器集合。
    /// 这些将根据 FreeLook 的垂直轴值修改设置。
    /// </summary>
    [Tooltip("这些将根据 FreeLook 的垂直轴值修改设置")]
    [SerializeReference] [NoSaveDuringPlay] public List<Modifier> Modifiers = new ();

    IModifierValueSource m_ValueSource;
    float m_CurrentValue;
    static AnimationCurve s_EasingCurve;

    void OnValidate()
    {
        var vcam = ComponentOwner;
        for (int i = 0; i < Modifiers.Count; ++i)
            Modifiers[i]?.Validate(vcam);
    }

    /// <summary>当组件启用时调用</summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshComponentCache();
    }

    // GML todo: 清理此代码
    static void TryGetVcamComponent<T>(CinemachineVirtualCameraBase vcam, out T component)
    {
        if (vcam == null || !vcam.TryGetComponent(out component))
            component = default;
    }

    void RefreshComponentCache()
    {
        var vcam = ComponentOwner;
        TryGetVcamComponent(vcam, out m_ValueSource);
        for (int i = 0; i < Modifiers.Count; ++i)
            Modifiers[i]?.RefreshCache(vcam);
    }

    // 检查器需要
    internal bool HasValueSource() { RefreshComponentCache(); return m_ValueSource != null; }

    /// <summary>重写此方法以执行诸如偏移 ReferenceLookAt 之类的操作。
    /// 基类实现不执行任何操作。</summary>
    /// <param name="vcam">正在处理的虚拟相机</param>
    /// <param name="curState">必须被修改的输入状态</param>
    /// <param name="deltaTime">当前适用的 deltaTime</param>
    public override void PrePipelineMutateCameraStateCallback(
        CinemachineVirtualCameraBase vcam, ref CameraState curState, float deltaTime) 
    {
        if (m_ValueSource != null && vcam == ComponentOwner)
        {
            // 应用缓动
            if (s_EasingCurve == null)
            {
                s_EasingCurve = AnimationCurve.Linear(0f, 0f, 1, 1f);
#if false // 不，它看起来不太好
// GML todo: 找到一种制作平滑曲线的好方法。也许是贝塞尔曲线？
                // 缓出，硬入
                var keys = s_EasingCurve.keys;
                keys[0].outTangent = 0;
                keys[1].inTangent = 1.4f;
                s_EasingCurve.keys = keys;
#endif
            }
            var v = m_ValueSource.NormalizedModifierValue;
            var sign = Mathf.Sign(v);
            m_CurrentValue = sign * s_EasingCurve.Evaluate(sign * v);
            for (int i = 0; i < Modifiers.Count; ++i)
                Modifiers[i]?.BeforePipeline(vcam, ref curState, deltaTime, m_CurrentValue);
        }
    }
        
    /// <summary>
    /// 回调以执行请求的 rig 修改。
    /// </summary>
    /// <param name="vcam">正在处理的虚拟相机</param>
    /// <param name="stage">当前管道阶段</param>
    /// <param name="state">当前虚拟相机状态</param>
    /// <param name="deltaTime">当前适用的 deltaTime</param>
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (m_ValueSource != null && stage == CinemachineCore.Stage.Finalize && vcam == ComponentOwner)
        {
            for (int i = 0; i < Modifiers.Count; ++i)
                Modifiers[i]?.AfterPipeline(vcam, ref state, deltaTime, m_CurrentValue);
        }
    }
}
```

## CinemachineExtension代码注释翻译

```csharp
/// <summary>
/// CinemachineCamera 扩展模块的基类。
/// 钩入 Cinemachine 管道。使用此功能为 vcam 添加额外的处理，
/// 修改其生成的状态。
/// </summary>
public abstract class CinemachineExtension : MonoBehaviour
{
    /// <summary>
    /// 需要保存每个 vcam 状态的扩展应从此类继承并添加
    /// 适当的成员变量。使用 GetExtraState() 访问。
    /// </summary>
    protected class VcamExtraStateBase
    {
        /// <summary>被扩展修改的虚拟相机</summary>
        public CinemachineVirtualCameraBase Vcam;
    }

    CinemachineVirtualCameraBase m_VcamOwner;
    Dictionary<CinemachineVirtualCameraBase, VcamExtraStateBase> m_ExtraState;

    /// <summary>用于非常小的浮点数的有用常量</summary>
    protected const float Epsilon = UnityVectorExtensions.Epsilon;

    /// <summary>获取此扩展附加到的 CinemachineVirtualCamera。
    /// 这与扩展将修改的 CinemachineCameras 不同，
    /// 因为管理器相机拥有的扩展将应用于所有 CinemachineCamera 子级。</summary>
    public CinemachineVirtualCameraBase ComponentOwner
    {
        get
        {
            if (m_VcamOwner == null)
                TryGetComponent(out m_VcamOwner);
            return m_VcamOwner;
        }
    }

    /// <summary>连接到虚拟相机管道。
    /// 重写实现必须调用此基实现</summary>
    protected virtual void Awake() => ConnectToVcam(true);

    /// <summary>从虚拟相机管道断开连接。
    /// 重写实现必须调用此基实现</summary>
    protected virtual void OnDestroy() => ConnectToVcam(false);

    /// <summary>不执行任何操作。它在这里用于检查器中的小复选框。</summary>
    protected virtual void OnEnable() {}

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    static void OnScriptReload()
    {
        var extensions = Resources.FindObjectsOfTypeAll<CinemachineExtension>();
        // 按执行顺序排序
        System.Array.Sort(extensions, (x, y) => 
            UnityEditor.MonoImporter.GetExecutionOrder(UnityEditor.MonoScript.FromMonoBehaviour(y)) 
                - UnityEditor.MonoImporter.GetExecutionOrder(UnityEditor.MonoScript.FromMonoBehaviour(x)));
        for (int i = 0; i < extensions.Length; ++i)
            extensions[i].ConnectToVcam(true);
    }
#endif
    internal void EnsureStarted() => ConnectToVcam(true);

    /// <summary>连接到虚拟相机。实现必须安全地重复调用。
    /// 重写实现必须调用此基实现</summary>
    /// <param name="connect">如果连接则为 true，如果断开连接则为 false</param>
    protected virtual void ConnectToVcam(bool connect)
    {
        if (ComponentOwner != null)
        {
            if (connect)
                ComponentOwner.AddExtension(this);
            else
                ComponentOwner.RemoveExtension(this);
        }
        m_ExtraState = null;
    }

    /// <summary>重写此方法以执行诸如偏移 ReferenceLookAt 之类的操作。
    /// 基类实现不执行任何操作。</summary>
    /// <param name="vcam">正在处理的虚拟相机</param>
    /// <param name="curState">必须被修改的输入状态</param>
    /// <param name="deltaTime">当前适用的 deltaTime</param>
    public virtual void PrePipelineMutateCameraStateCallback(
        CinemachineVirtualCameraBase vcam, ref CameraState curState, float deltaTime) {}

    /// <summary>旧版支持。这仅在此处以避免更改 API
    /// 以使 PostPipelineStageCallback() 公开</summary>
    /// <param name="vcam">正在处理的虚拟相机</param>
    /// <param name="stage">当前管道阶段</param>
    /// <param name="state">当前虚拟相机状态</param>
    /// <param name="deltaTime">当前适用的 deltaTime</param>
    public void InvokePostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        PostPipelineStageCallback(vcam, stage, ref state, deltaTime);
    }

    /// <summary>
    /// 此回调将在虚拟相机实现管道的每个阶段后调用。
    /// 此方法可以修改引用的状态。
    /// 如果 deltaTime 小于 0，则重置所有状态信息并不执行阻尼。
    /// </summary>
    /// <param name="vcam">正在处理的虚拟相机</param>
    /// <param name="stage">当前管道阶段</param>
    /// <param name="state">当前虚拟相机状态</param>
    /// <param name="deltaTime">当前适用的 deltaTime</param>
    protected abstract void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime);

    /// <summary>当目标发生瞬移时调用此方法，以便扩展可以更新其内部状态，使相机
    /// 也能无缝瞬移。基类实现不执行任何操作。</summary>
    /// <param name="vcam">要瞬移的虚拟相机</param>
    /// <param name="target">被瞬移的对象</param>
    /// <param name="positionDelta">目标位置的变化量</param>
    public virtual void OnTargetObjectWarped(
        CinemachineVirtualCameraBase vcam, Transform target, Vector3 positionDelta) {}

    /// <summary>
    /// 强制虚拟相机假定给定的位置和方向
    /// </summary>
    /// <param name="pos">要采用的世界空间位置</param>
    /// <param name="rot">要采用的世界空间方向</param>
    public virtual void ForceCameraPosition(Vector3 pos, Quaternion rot) {}

    /// <summary>通知此虚拟相机即将激活。
    /// 基类实现必须由任何重写的方法调用。</summary>
    /// <param name="fromCam">被停用的相机。可能为 null。</param>
    /// <param name="worldUp">默认的世界 Up，由 CinemachineBrain 设置</param>
    /// <param name="deltaTime">用于基于时间的效果的 Delta 时间（如果小于或等于 0 则忽略）</param>
    /// <returns>如果请求 vcam 更新内部状态，则返回 true</returns>
    public virtual bool OnTransitionFromCamera(
        ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime) => false;

    /// <summary>
    /// 报告此扩展所需的最大阻尼时间。
    /// 仅在编辑器中用于时间轴擦除。
    /// </summary>
    /// <returns>此扩展中的最高阻尼设置</returns>
    public virtual float GetMaxDampTime() => 0;
    
    /// <summary>因为扩展可以放置在管理器相机上，并且在这种情况下
    /// 将被调用以处理所有 vcam 子级，所以 vcam 特定的状态信息
    /// 应存储在此处。只需定义一个类来保存您的状态信息
    /// 并在调用此方法时专门使用它。</summary>
    /// <typeparam name="T">额外状态类的类型</typeparam>
    /// <param name="vcam">正在处理的虚拟相机</param>
    /// <returns>额外状态，转换为类型 T</returns>
    protected T GetExtraState<T>(CinemachineVirtualCameraBase vcam) where T : VcamExtraStateBase, new()
    {
        if (m_ExtraState == null)
            m_ExtraState = new ();
        if (!m_ExtraState.TryGetValue(vcam, out var extra))
            extra = m_ExtraState[vcam] = new T { Vcam = vcam};
        return extra as T;
    }

    /// <summary>获取所有 vcam 的所有额外状态信息。</summary>
    /// <typeparam name="T">额外状态类型</typeparam>
    /// <param name="list">将填充额外状态的列表。</param>
    protected void GetAllExtraStates<T>(List<T> list) where T : VcamExtraStateBase, new()
    {
        list.Clear();
        if (m_ExtraState != null)
        {
            var iter = m_ExtraState.GetEnumerator();
            while (iter.MoveNext())
                list.Add(iter.Current.Value as T);
        }
    }
}
```



## Cinemachine中的属性Attribute

```csharp
using UnityEngine;

namespace Unity.Cinemachine
{
    /// <summary>
    /// 为类启用游戏模式保存功能。退出游戏模式时，将扫描该类的字段，并将其属性值应用到场景对象。
    /// 这是临时解决方案，待Unity实现更通用的游戏模式保存功能后将会淘汰。
    /// </summary>
    public sealed class SaveDuringPlayAttribute : System.Attribute {}

    /// <summary>
    /// 禁止字段的游戏模式保存功能。当类标记了[SaveDuringPlay]特性，但其中某些字段不需要保存时使用。
    /// </summary>
    public sealed class NoSaveDuringPlayAttribute : PropertyAttribute {}
}
```



# 如何设计左右分屏游戏

1. 两个Camera各自挂Cinemachine Brain，分配不同的Channel Mask（比如左半屏为Channel 1，右半屏Channel 2）。
2. 所有Cinemachine Camera，也要分配不同的Channel与Brain对应。
3. 左半屏Camera的Output中，Viewport Rect设置为：X(0) Y(0) W(0.5) H(1)；右半屏X(0.5) Y(0) W(0.5) H(1)。

