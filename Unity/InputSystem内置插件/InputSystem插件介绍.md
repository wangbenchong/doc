# 参考了这些链接

- [【推荐先看】Unity Input System Step-By-Step 最简教程 | 三叔的数字花园](https://tuncle.blog/input_system_minimum_tutorial/index.html)
- [基础教程——Unity 官方开发者社区](https://developer.unity.cn/projects/64ce58b4edbc2a10dd1e49b8)
- [Quickstart Guide | Input System | 1.10.0 (unity3d.com)](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/QuickStartGuide.html)
- [更好上手的 Unity Input System 教程_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1VCNdeSER3/)

在阅读了以上链接的文章、视频后，结合我目前用的新版Unity6，我将文章编辑如下：

# 摘抄：Unity Input System Step-By-Step 最简教程

## Overview

完整工程见：[xuejiaW/InputSystemSample: A minimum unity project to illustrate how to use Unity new input system. (github.com)](https://github.com/xuejiaW/InputSystemSample)

Unity 有内建的 `Input Manager` 机制，这一套机制存在了非常久的时间。针对于传统的键盘，鼠标等输入，内置的 `Input Manager` 可以健壮的处理，但 `Input Manager` 的可拓展性不高。也因此随着输入设备种类的增多（如各类 XR 设备），`Input Manager` 无法优雅的解决这些输入。

`Input System` 是 Unity 为了解决 `Input Manager` 的上述问题，提供的高可拓展性，高自由配置的新输入解决方案。

对于新工程，Unity 官方都推荐使用 `Input System` 作为输入的解决方案，但 `Input Manager` 并不会短期内被废弃，因为历史包袱过重 。
Input System 依赖 Unity 2019.1 及以上版本，以下基于 Unity 2022.3.15f1 + Input System 1.7 编写**（我在摘抄的同时已按照新的Unity6版本做了校正）**



### 安装 Input System

首先通过 Unity Package Manager 安装 Input System。之后会自动弹出如下窗口，提示 `Input System` 启用后需要重启 Editor Backend 才能正常使用，点击 `Yes` 启用 Input System 并重启 Unity：

 ![启用 Input System](./img/2023-11-16-15-54-50.jpg)

**启用 Input System**

当 Unity 重启后，根据 Unity 版本的不同，可能内置的 `Input Manager` 会被关闭，如果要重新启用，可以在 `Edit` -> `Project Settings` -> `Player` -> `Other Settings` -> `Active Input Handling` 中选择 `Both` 来切换输入方式：
 ![Player Settings 中切换输出方式](./img/2023-11-16-15-59-49.jpg)

至此 Input System 已经被正确安装。

为方便后续的调试，Demo 工程中预先引入了 URP 和一个最简的测试场景，此时的工程的见（基于旧版Unity 2022，仅供参考）：
 [xuejiaW/InputSystemSample at 7d04... (github.com)](https://github.com/xuejiaW/InputSystemSample/tree/7d041a5604c71096b0147cd0ae672557eee517c8)



### 创建 Input System SettingAssets

推荐先创建全局 Settings Asset（非必需，有默认设置），你需要在 `Project Settings` -> `Input System Package` 中创建相关资源：

![Create Input System](./img/2023-11-16-16-01-18.jpg)

当点击创建后，会在工程的根目录创建出一个 `InputSystem.inputsettings` 文件，该文件即是 Input System 的总配置文件。同时原 `Input System Package` 页面也会包含有一系列的针对于 `Input System` 的配置项：
 ![Input System Settings](./img/2023-11-16-16-13-41.jpg)

↑↑ *Input System Settings* ↑↑



选择创建出来的 `InputSystem.inputsettings` 文件，在 Inspector 中会出现直接跳转到 `Input System Settings` 的按钮，点击该按钮，同样会跳转到上述 `Input System Settings` 的配置页面：
 ![Asset Inspector | 400](./img/2023-11-16-16-17-06.jpg)

> 你可以随意修改 `InputSystem.inputsettings` 的位置，并不要求该文件必须在工程根目录下。
>

此时的工程状态见（基于旧版Unity 2022，仅供参考）：
 [xuejiaW/InputSystemSample at 62a... (github.com)](https://github.com/xuejiaW/InputSystemSample/tree/62aff15fcf5c3479e5a3073af7646a6c2775e043)



### 查看 Input Debugger 窗口

你可以在 `Window -> Analysis -> Input Debugger` 中打开 Input Debugger 窗口，该窗口中可以显示当前连接的输入设备：
 ![Input Debugger 窗口](./img/2023-11-16-16-20-42.jpg)



## 使用 Input System

### 创建 Input Action Asset

如场景中有如下小球，为了让其移动，可以为它添加一个 `Player Input` 组件，在 `Player Input` 组件上，选择 `Create Action` 创建出一个 Input Action 资源：
 ![img](./img/2023-11-17-15-09-59.jpg)

点击后，会需要你选择保存的路径，选择后，会在该路径下创建出一个 `Input Action` 资源（ `Input System.inputactions`），并自动打开该资源的配置窗口，窗口如下所示（点击该资源上的 `Edit asset` 按钮或双击该资源，都将打开这个窗口）：
 ![Input Action Window](./img/2023-11-16-16-28-43.jpg)

具体查看 Input Action Asset 中的 Move Action，定义了键盘的 `WASD` 和 `上下左右` 触发：
 ![img](./img/2023-11-16-16-33-04.jpg)

此时按下 `WASD` 或 `上下左右`，会发现小球还 **不能** 移动，因为 PlayerInput 只负责获取输入信息，但还是没有 *处理* 这些输入信息。

此时工程状态见（基于旧版Unity 2022，仅供参考）：
 [xuejiaW/InputSystemSample at ed81... (github.com)](https://github.com/xuejiaW/InputSystemSample/tree/ed81be6f2efcf89c72f287240c9b56ea80a24094)



### 附插件原生 PlayerInput 脚本（简略版）

插件原生脚本不可编辑，仅供查看和使用，我翻译了一下代码开头的注释：

```csharp
/*
表示游戏中的一个独立玩家，包含一组该玩家独有的操作和一组配对的设备。
PlayerInput 是输入系统功能的高级封装，旨在帮助快速设置新的输入系统。它负责 InputAction 的管理，并提供了一个自定义的 UI（需要 "Unity UI" 包）来帮助设置输入。

该组件隐式支持本地多人游戏。每个 PlayerInput 实例代表一个独立的用户，拥有自己的设备和操作集。要协调玩家管理并促进诸如通过设备活动加入等机制，请使用 UnityEngine.InputSystem.PlayerInputManager。

PlayerInput 通知脚本代码事件的方式由 notificationBehavior 决定。默认情况下，它设置为 UnityEngine.InputSystem.PlayerNotifications.SendMessages，这将使用GameObject.SendMessage(string,object) 向 PlayerInput 所在的 GameObject 发送消息。

启用时，PlayerInput 将创建一个 InputUser 并将设备与该用户配对，这些设备随后专属于该玩家。设备集可以在实例化 PlayerInput 时通过 Instantiate(GameObject,int,string,int,InputDevice[]) 或 Instantiate(GameObject,int,string,int,InputDevice)显式控制。这也使得可以将同一设备分配给两个不同的玩家，例如用于分屏键盘游戏。
*/
//示例：
var p1 = PlayerInput.Instantiate(playerPrefab,
     controlScheme: "KeyboardLeft", device: Keyboard.current);
var p2 = PlayerInput.Instantiate(playerPrefab,
     controlScheme: "KeyboardRight", device: Keyboard.current);
/*
如果没有为 PlayerInput 指定特定设备，该组件将查找系统中存在的兼容设备并自动将它们与自己配对。如果 PlayerInput 的 actions 定义了控制方案，PlayerInput 将查找一个所有所需设备都可用且未与其他玩家配对的方案。它将首先尝试 defaultControlScheme（如果已设置），然后回退到按顺序尝试所有可用方案。一旦找到一个所有所需设备都可用的方案，PlayerInput 将把这些设备与自己配对并选择该方案。

如果没有定义控制方案，PlayerInput 将尝试将尽可能多的未配对设备与自己绑定，只要这些设备与 actions 中的绑定匹配。这意味着，例如，如果有一个键盘和两个游戏手柄可用，并且 PlayerInput 启用时存在键盘和游戏手柄的绑定，所有三个设备都将与该玩家配对。

请注意，当使用 PlayerInputManager 时，设备与玩家的配对由加入逻辑控制。在这种情况下，PlayerInput 将自动配对玩家加入时使用的设备。如果 actions 中存在控制方案，则选择与该设备兼容的第一个方案。如果需要其他设备，这些设备将从当前未配对的设备池中配对。

设备配对可以随时通过手动控制配对来更改，使用 PlayerInput 分配的 user 的 InputUser.PerformPairingWithDevice（及相关方法），或者通过切换控制方案（例如使用 SwitchCurrentControlScheme(string,InputDevice[])），如果 PlayerInput 的 actions 中存在控制方案。

当玩家失去与其配对的设备时（例如，当设备被拔出或断电时），InputUser 将发出 InputUserChange.DeviceLost 信号，该信号也会作为消息、deviceLostEvent 或 onDeviceLost 传递（取决于 notificationBehavior）。

当设备重新连接时，InputUser 将发出 InputUserChange.DeviceRegained 信号，该信号也会作为消息、deviceRegainedEvent 或 onDeviceRegained 传递（取决于 notificationBehavior）。

当游戏中只有一个活动的 PlayerInput 时，加入未启用（参见 PlayerInputManager.joiningEnabled），并且 neverAutoSwitchControlSchemes 未设置为true，玩家的设备配对也会根据设备使用情况自动更新。

如果 actions 中存在控制方案，那么如果使用了一个设备（不仅仅是插入设备，而是在非噪声、非合成控件上接收输入）且该设备与当前使用的方案不同的控制方案兼容，PlayerInput 将尝试切换到该控制方案。成功与否取决于该方案的所有设备要求是否从可用设备集中得到满足。如果控制方案发生变化，InputUser 会在 InputUser.onChange 上发出 InputUserChange.ControlSchemeChanged 信号。

如果 actions 中不存在控制方案，PlayerInput 将自动将任何新可用的设备与自己配对，只要该设备有任何可用的绑定。
如果多个 PlayerInput 处于活动状态，则上述两种行为将自动禁用。
*/
//示例：
using UnityEngine;
using UnityEngine.InputSystem;
// 与 PlayerInput 相邻的组件。
[RequireComponent(typeof(PlayerInput))]
public class MyPlayerLogic : MonoBehaviour
{
     public GameObject projectilePrefab;

     private Vector2 m_Look;
     private Vector2 m_Move;
     private bool m_Fire;

     // 'Fire' 输入操作已触发。对于 'Fire'，我们希望连续操作（即开火）在按住开火按钮时持续进行，以便在按钮按下时重复触发操作。我们可以通过在按钮上设置 "Press" 交互并将其设置为以固定间隔重复来轻松实现这一点。
     public void OnFire()
     {
         Instantiate(projectilePrefab);
     }

     // 'Move' 输入操作已触发。
     public void OnMove(InputValue value)
     {
         m_Move = value.Get<Vector2>();
     }

     // 'Look' 输入操作已触发。
     public void OnLook(InputValue value)
     {
         m_Look = value.Get<Vector2>();
     }

     public void OnUpdate()
     {
         // 根据 m_Move 和 m_Look 更新变换
     }
 }
/*
也可以使用 InputAction 的轮询 API（参见 InputAction.triggered 和 InputAction.ReadValue{TValue}）与 PlayerInput 结合使用。
*/
//示例：
using UnityEngine;
using UnityEngine.InputSystem;
// 与 PlayerInput 相邻的组件。
[RequireComponent(typeof(PlayerInput))]
public class MyPlayerLogic : MonoBehaviour
{
     public GameObject projectilePrefab;

     private PlayerInput m_PlayerInput;
     private InputAction m_LookAction;
     private InputAction m_MoveAction;
     private InputAction m_FireAction;

     public void OnUpdate()
     {
         // 在第一次更新时查找我们需要的所有数据。
         // 注意：我们不在 OnEnable 中执行此操作，因为 PlayerInput 本身在 OnEnable 中执行一些初始化工作。
         if (m_PlayerInput == null)
         {
             m_PlayerInput = GetComponent<PlayerInput>();
             m_FireAction = m_PlayerInput.actions["fire"];
             m_LookAction = m_PlayerInput.actions["look"];
             m_MoveAction = m_PlayerInput.actions["move"];
         }

         if (m_FireAction.triggered)
             /* 开火逻辑... */;

         var move = m_MoveAction.ReadValue<Vector2>();
         var look = m_LookAction.ReadValue<Vector2>();
         /* 根据 move和look 更新变换... */
     }
 }
// <seealso cref="UnityEngine.InputSystem.PlayerInputManager"/>
```

代码主体部分也做了翻译和缩略（为了压缩篇幅，部分逻辑这里就不列出了，不过还是太长，也可以先跳过这段代码不看）：

```csharp
public const string DeviceLostMessage = "OnDeviceLost";
public const string DeviceRegainedMessage = "OnDeviceRegained";
public const string ControlsChangedMessage = "OnControlsChanged";
/// <summary>
/// 玩家的输入是否处于激活状态。如果为 true，表示玩家正在接收输入。
/// 要激活或停用输入，请使用 ActivateInput 或 DeactivateInput。
/// </summary>
public bool inputIsActive => m_InputActive;
public int playerIndex => m_PlayerIndex;
public int splitScreenIndex => m_SplitScreenIndex;
public InputActionAsset actions{get;set;}//内部略
public string currentControlScheme{get;}//内部略
public string defaultControlScheme{get;set;}//内部略
public bool neverAutoSwitchControlSchemes{get;set;}//内部略
public InputActionMap currentActionMap{get;set;}//内部略
public string defaultActionMap{get;set;}//内部略
public PlayerNotifications notificationBehavior{get;set;}//内部略
public ReadOnlyArray<ActionEvent> actionEvents{get;set;}//内部略
public DeviceLostEvent deviceLostEvent{get;}//内部略
public DeviceRegainedEvent deviceRegainedEvent{get;}//内部略
public ControlsChangedEvent controlsChangedEvent{get;}//内部略
public event Action<InputAction.CallbackContext> onActionTriggered{add;remove}//内部略
public event Action<PlayerInput> onDeviceLost{add;remove}//内部略
public event Action<PlayerInput> onDeviceRegained{add;remove}//内部略
public event Action<PlayerInput> onControlsChanged{add;remove}//内部略
public new Camera camera{get => m_Camera; set => m_Camera = value}
public InputSystemUIInputModule uiInputModule{get;set;}//内部略
public InputUser user => m_InputUser;
public ReadOnlyArray<InputDevice> devices{get;}//内部略
public bool hasMissingRequiredDevices => user.valid && user.hasMissingRequiredDevices;
public static ReadOnlyArray<PlayerInput> all => new ReadOnlyArray<PlayerInput>(s_AllActivePlayers, 0, s_AllActivePlayersCount);
public static bool isSinglePlayer =>
    s_AllActivePlayersCount <= 1 &&
    (PlayerInputManager.instance == null || !PlayerInputManager.instance.joiningEnabled);
public TDevice GetDevice<TDevice>()
    where TDevice : InputDevice
{
    foreach (var device in devices)
        if (device is TDevice deviceOfType)
            return deviceOfType;
    return null;
}
/// <summary>
/// 通过启用当前操作映射来启用玩家的输入。
/// 当 PlayerInput 组件启用时，输入会自动激活。然而，可以通过调用此方法在使用 DeactivateInput 停用输入后重新激活输入。
/// 注意，激活输入只会激活当前操作映射（参见 currentActionMap）。
/// 可以通过 inputIsActive 检查当前状态。
/// </summary>
/// <example>
/// PlayerInput.all[0].ActivateInput();
/// </example>
public void ActivateInput()
{
    UpdateDelegates();
    m_InputActive = true;
    // 如果没有当前操作映射，但有默认操作映射，则将其设置为当前操作映射。
    if (m_CurrentActionMap == null && m_Actions != null && !string.IsNullOrEmpty(m_DefaultActionMap))
        SwitchCurrentActionMap(m_DefaultActionMap);
    else
        m_CurrentActionMap?.Enable();
}

/// <summary>
/// 通过禁用当前操作映射来禁用玩家的输入。
/// 当 PlayerInput 组件启用时，输入会自动激活。此方法可用于手动停用输入。
/// 注意，停用输入只会禁用当前操作映射（参见 "currentActionMap"）。
/// </summary>
/// <example>
/// PlayerInput.all[0].DeactivateInput();
/// </example>
/// <seealso cref="ActivateInput"/>
/// <seealso cref="inputIsActive"/>
public void DeactivateInput()
{
    m_CurrentActionMap?.Disable();
    m_InputActive = false;
}

/// <summary>
/// 将当前控制方案切换为适合给定设备集的方案。
/// 玩家当前配对的设备（参见 devices）将被取消配对。
/// </summary>
/// <param name="devices">输入设备列表。注意，如果任何设备已经与其他玩家配对，则该设备将同时与两个玩家配对。</param>
/// <returns>如果切换成功则返回 true，否则返回 false。例如，如果 actions 没有适合给定设备集的控制方案，则可能返回 false。</returns>
/// <exception cref="ArgumentNullException"><paramref name="devices"/> 为 null。</exception>
/// <exception cref="InvalidOperationException">actions 未分配。</exception>
/// <example>
/// // 将第一个玩家切换到键盘和鼠标。
/// PlayerInput.all[0].SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
/// </example>
/// <seealso cref="currentControlScheme"/>
/// <seealso cref="InputActionAsset.controlSchemes"/>
public bool SwitchCurrentControlScheme(params InputDevice[] devices)
{
    if (devices == null)
        throw new ArgumentNullException(nameof(devices));
    if (actions == null)
        throw new InvalidOperationException(
            "必须为 PlayerInput 设置 actions 才能切换控制方案");

    // 在关联的操作资源中查找适合给定设备的控制方案
    var scheme = InputControlScheme.FindControlSchemeForDevices(devices, actions.controlSchemes);
    if (!scheme.HasValue)
        return false;

    var controlScheme = scheme.Value;
    SwitchControlSchemeInternal(ref controlScheme, devices);
    return true;
}

/// <summary>
/// 将玩家切换到使用给定的控制方案和指定的设备集。
/// 此方法可用于显式强制组合控制方案和特定设备集。
/// 玩家当前配对的设备（参见 devices）将被取消配对。
/// </summary>
/// <example>
/// // 将玩家 1 切换到 "Gamepad" 控制方案，并使用第二个游戏手柄。
/// PlayerInput.all[0].SwitchCurrentControlScheme(
///     "Gamepad",
///     Gamepad.all[1]);
/// </example>
/// <seealso cref="InputActionAsset.controlSchemes"/>
/// <seealso cref="currentControlScheme"/>
public void SwitchCurrentControlScheme(string controlScheme, params InputDevice[] devices)
{
    if (string.IsNullOrEmpty(controlScheme))
        throw new ArgumentNullException(nameof(controlScheme));
    if (devices == null)
        throw new ArgumentNullException(nameof(devices));

    user.FindControlScheme(controlScheme, out InputControlScheme scheme); // 如果未找到则抛出异常
    SwitchControlSchemeInternal(ref scheme, devices);
}

/// <summary>
/// 将玩家切换到使用给定的操作映射。
/// </summary>
/// <param name="mapNameOrId">操作映射的名称或其 ID。</param>
/// <remarks>
/// 此方法可用于显式设置操作映射。
/// </remarks>
/// <example>
/// <code>
/// PlayerInput.all[0].SwitchCurrentActionMap("Player");
/// </code>
/// </example>
/// <seealso cref="InputActionMap"/>
public void SwitchCurrentActionMap(string mapNameOrId)
{
    // Must be enabled.
    if (!m_Enabled)
    {
        Debug.LogError($"Cannot switch to actions '{mapNameOrId}'; input is not enabled", this);
        return;
    }

    // Must have actions.
    if (m_Actions == null)
    {
        Debug.LogError($"Cannot switch to actions '{mapNameOrId}'; no actions set on PlayerInput", this);
        return;
    }

    // Must have map.
    var actionMap = m_Actions.FindActionMap(mapNameOrId);
    if (actionMap == null)
    {
        Debug.LogError($"Cannot find action map '{mapNameOrId}' in actions '{m_Actions}'", this);
        return;
    }

    currentActionMap = actionMap;
}

/// <summary>
/// 返回具有指定玩家索引的玩家。
/// </summary>
/// <param name="playerIndex">活动玩家列表中的索引。</param>
/// <returns>具有给定玩家索引的玩家，如果不存在则返回 null。</returns>
/// <example>
/// PlayerInput player = PlayerInput.GetPlayerByIndex(0);
/// </example>
/// <seealso cref="PlayerInput.playerIndex"/>
public static PlayerInput GetPlayerByIndex(int playerIndex)
{
    for (var i = 0; i < s_AllActivePlayersCount; ++i)
        if (s_AllActivePlayers[i].playerIndex == playerIndex)
            return s_AllActivePlayers[i];
    return null;
}

/// <summary>
/// 查找与给定设备配对的第一个 PlayerInput。
/// 可能有多个玩家与设备配对。此函数将返回找到的第一个玩家。
/// </summary>
/// <example>
/// // 查找与第一个游戏手柄配对的玩家。
/// var player = PlayerInput.FindFirstPairedToDevice(Gamepad.all[0]);
/// </example>
public static PlayerInput FindFirstPairedToDevice(InputDevice device)
{
    if (device == null)
        throw new ArgumentNullException(nameof(device));
    for (var i = 0; i < s_AllActivePlayersCount; ++i)
    {
        if (ReadOnlyArrayExtensions.ContainsReference(s_AllActivePlayers[i].devices, device))
            return s_AllActivePlayers[i];
    }
    return null;
}

/// <summary>
/// 实例化一个玩家对象，设置并启用其输入。
/// </summary>
/// <param name="prefab">要克隆的预制体。必须在层次结构中的某处包含一个 PlayerInput 组件。</param>
/// <param name="playerIndex">要分配给玩家的玩家索引。参见PlayerInput.playerIndex。
/// 默认情况下，将根据 all 中的玩家数量自动分配。</param>
/// <param name="controlScheme">要激活的控制方案。</param>
/// <param name="splitScreenIndex">要在哪个分屏上实例化。</param>
/// <param name="pairWithDevice">要与用户配对的设备。默认情况下，此值为 null，这意味着
/// PlayerInput 将根据控制方案（如果有）自动与可用且未配对的设备配对，或者根据 actions 中的绑定（如果没有控制方案）进行配对。</param>
/// <returns>新创建的 PlayerInput 组件。</returns>
/// <exception cref="ArgumentNullException"><paramref name="prefab"/> 为 null。</exception>
/// <example>
/// var p1 = PlayerInput.Instantiate(playerPrefab, controlScheme: "KeyboardLeft", device: Keyboard.current);
/// </example>
public static PlayerInput Instantiate(GameObject prefab, int playerIndex = -1, string controlScheme = null,
    int splitScreenIndex = -1, InputDevice pairWithDevice = null)
{
    if (prefab == null)
        throw new ArgumentNullException(nameof(prefab));
    // Set initialization data.
    s_InitPlayerIndex = playerIndex;
    s_InitSplitScreenIndex = splitScreenIndex;
    s_InitControlScheme = controlScheme;
    if (pairWithDevice != null)
        ArrayHelpers.AppendWithCapacity(ref s_InitPairWithDevices, ref s_InitPairWithDevicesCount, pairWithDevice);
    return DoInstantiate(prefab);
}

/// <summary>
/// Object.Instantiate(Object) 的封装方法，允许实例化一个玩家预制体并自动将一个或多个特定设备与新创建的玩家配对。
/// 注意，与 Object.Instantiate(Object) 不同，此方法将始终激活生成的 GameObject 及其组件。
/// </summary>
/// <param name="prefab">一个包含 PlayerInput 组件的玩家预制体。</param>
/// <param name="playerIndex">要实例化的玩家索引。</param>
/// <param name="controlScheme">要激活的控制方案。</param>
/// <param name="splitScreenIndex">要在哪个分屏上实例化。</param>
/// <param name="pairWithDevices">限制配对的设备。</param>
/// <returns>新创建的 PlayerInput 组件。</returns>
/// <example>
/// var devices = new InputDevice[] { Gamepad.all[0], Gamepad.all[1] };
/// var p1 = PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevices: devices);
/// </example>
public static PlayerInput Instantiate(GameObject prefab, int playerIndex = -1, string controlScheme = null,
    int splitScreenIndex = -1, params InputDevice[] pairWithDevices)
{
    //..略..
    return DoInstantiate(prefab);
}

private static PlayerInput DoInstantiate(GameObject prefab)
{
    //..省略各种Instantiate和Destroy..
    return playerInput;
}

[Tooltip("与玩家关联的输入操作。")]
[SerializeField] internal InputActionAsset m_Actions;

[Tooltip("确定当与玩家相关的输入事件发生时，应如何发送通知。")]
[SerializeField] internal PlayerNotifications m_NotificationBehavior;

[Tooltip("UI InputModule，其输入操作应与此 PlayerInput 的操作同步。")]

#if UNITY_INPUT_SYSTEM_ENABLE_UI
[SerializeField] internal InputSystemUIInputModule m_UIInputModule;

[Tooltip("当 PlayerInput 失去配对的设备时触发的事件（例如设备电量耗尽）。")]
#endif

[SerializeField] internal DeviceLostEvent m_DeviceLostEvent;
[SerializeField] internal DeviceRegainedEvent m_DeviceRegainedEvent;
[SerializeField] internal ControlsChangedEvent m_ControlsChangedEvent;
[SerializeField] internal ActionEvent[] m_ActionEvents;
[SerializeField] internal bool m_NeverAutoSwitchControlSchemes;
[SerializeField] internal string m_DefaultControlScheme; ////REVIEW: 是否应该为这些设置 ID 以便安全地重命名？
[SerializeField] internal string m_DefaultActionMap;
[SerializeField] internal int m_SplitScreenIndex = -1;

[Tooltip("玩家的视角摄像机引用。请注意，仅在使用分屏和/或每个玩家的 UI 时才需要此属性。否则可以安全地不初始化此属性。")]
[SerializeField] internal Camera m_Camera;

// 通过 SendMessage() 或 BroadcastMessage() 发送消息时使用的值对象。接收者可以忽略此对象。
// 我们重复使用同一个对象以避免分配垃圾。
[NonSerialized] private InputValue m_InputValueObject;

[NonSerialized] internal InputActionMap m_CurrentActionMap;

[NonSerialized] private int m_PlayerIndex = -1;
[NonSerialized] private bool m_InputActive;
[NonSerialized] private bool m_Enabled;
[NonSerialized] internal bool m_ActionsInitialized;
[NonSerialized] private Dictionary<string, string> m_ActionMessageNames;
[NonSerialized] private InputUser m_InputUser;
[NonSerialized] private Action<InputAction.CallbackContext> m_ActionTriggeredDelegate;
[NonSerialized] private CallbackArray<Action<PlayerInput>> m_DeviceLostCallbacks;
[NonSerialized] private CallbackArray<Action<PlayerInput>> m_DeviceRegainedCallbacks;
[NonSerialized] private CallbackArray<Action<PlayerInput>> m_ControlsChangedCallbacks;
[NonSerialized] private CallbackArray<Action<InputAction.CallbackContext>> m_ActionTriggeredCallbacks;
[NonSerialized] private Action<InputControl, InputEventPtr> m_UnpairedDeviceUsedDelegate;
[NonSerialized] private Func<InputDevice, InputEventPtr, bool> m_PreFilterUnpairedDeviceUsedDelegate;
[NonSerialized] private bool m_OnUnpairedDeviceUsedHooked;
[NonSerialized] private Action<InputDevice, InputDeviceChange> m_DeviceChangeDelegate;
[NonSerialized] private bool m_OnDeviceChangeHooked;

internal static int s_AllActivePlayersCount;
internal static PlayerInput[] s_AllActivePlayers;
private static Action<InputUser, InputUserChange, InputDevice> s_UserChangeDelegate;

// The following information is used when the next PlayerInput component is enabled.
private static int s_InitPairWithDevicesCount;
private static InputDevice[] s_InitPairWithDevices;
private static int s_InitPlayerIndex = -1;
private static int s_InitSplitScreenIndex = -1;
private static string s_InitControlScheme;
internal static bool s_DestroyIfDeviceSetupUnsuccessful;

private void InitializeActions(){}//略

private void UninitializeActions(){}//略

private void InstallOnActionTriggeredHook()
{
    if (m_ActionTriggeredDelegate == null)
        m_ActionTriggeredDelegate = OnActionTriggered;
    foreach (var actionMap in m_Actions.actionMaps)
        actionMap.actionTriggered += m_ActionTriggeredDelegate;
}

private void UninstallOnActionTriggeredHook()
{
    if (m_ActionTriggeredDelegate != null)
        foreach (var actionMap in m_Actions.actionMaps)
            actionMap.actionTriggered -= m_ActionTriggeredDelegate;
}

private void OnActionTriggered(InputAction.CallbackContext context){}//略

private void CacheMessageNames()
{
    if (m_Actions == null)
        return;

    if (m_ActionMessageNames != null)
        m_ActionMessageNames.Clear();
    else
        m_ActionMessageNames = new Dictionary<string, string>();

    foreach (var action in m_Actions)
    {
        action.MakeSureIdIsInPlace();

        var name = CSharpCodeHelpers.MakeTypeName(action.name);
        m_ActionMessageNames[action.m_Id] = "On" + name;
    }
}
/// <summary>
/// Initialize user and devices.
/// </summary>
private void AssignUserAndDevices(){}//内部略

private bool HaveBindingForDevice(InputDevice device)
{
    if (m_Actions == null)
        return false;
    var actionMaps = m_Actions.actionMaps;
    for (var i = 0; i < actionMaps.Count; ++i)
    {
        var actionMap = actionMaps[i];
        if (actionMap.IsUsableWithDevice(device))
            return true;
    }
    return false;
}

private bool TryToActivateControlScheme(InputControlScheme controlScheme){return true;}//略
private void AssignPlayerIndex(){}//内部略
#if UNITY_EDITOR && UNITY_INPUT_SYSTEM_PROJECT_WIDE_ACTIONS
void Reset()
{
    // 将默认操作设置为项目范围内的操作。
    m_Actions = InputSystem.actions;
    // TODO 需要监控更改吗？
}
#endif

private void OnEnable()
{
    m_Enabled = true;
    using (InputActionRebindingExtensions.DeferBindingResolution())
    {
        AssignPlayerIndex(); // 分配玩家索引
        InitializeActions(); // 初始化操作
        AssignUserAndDevices(); // 分配用户和设备
        ActivateInput(); // 激活输入
    }
    // 分屏索引默认为玩家索引。
    if (s_InitSplitScreenIndex >= 0)
        m_SplitScreenIndex = splitScreenIndex;
    else
        m_SplitScreenIndex = playerIndex;

    // 添加到全局列表并按玩家索引排序。
    ArrayHelpers.AppendWithCapacity(ref s_AllActivePlayers, ref s_AllActivePlayersCount, this);
    for (var i = 1; i < s_AllActivePlayersCount; ++i)
        for (var j = i; j > 0 && s_AllActivePlayers[j - 1].playerIndex > s_AllActivePlayers[j].playerIndex; --j)
            s_AllActivePlayers.SwapElements(j, j - 1);
    // 如果是第一个玩家，则挂钩用户更改通知。
    if (s_AllActivePlayersCount == 1)
    {
        if (s_UserChangeDelegate == null)
            s_UserChangeDelegate = OnUserChange;
        InputUser.onChange += s_UserChangeDelegate;
    }
    // 在单玩家模式下，设置自动设备切换。
    if (isSinglePlayer)
    {
        if (m_Actions != null && m_Actions.controlSchemes.Count == 0)
        {
            // 没有控制方案。我们选择与绑定兼容的任何设备。
            StartListeningForDeviceChanges();
        }
        else if (!neverAutoSwitchControlSchemes)
        {
            // 我们有控制方案，因此仅监听未配对设备的输入活动（即实际使用未配对设备，而不仅仅是插入设备）。
            StartListeningForUnpairedDeviceActivity();
        }
    }
    HandleControlsChanged(); // 处理控件更改
    // 触发加入事件。
    PlayerInputManager.instance?.NotifyPlayerJoined(this);
}

private void StartListeningForUnpairedDeviceActivity() { } // 内部实现略
private void StopListeningForUnpairedDeviceActivity() { } // 内部实现略
private void StartListeningForDeviceChanges() { } // 内部实现略
private void StopListeningForDeviceChanges() { } // 内部实现略

private void OnDisable()
{
    m_Enabled = false;

    // 从全局列表中移除。
    var index = ArrayHelpers.IndexOfReference(s_AllActivePlayers, this, s_AllActivePlayersCount);
    if (index != -1)
        ArrayHelpers.EraseAtWithCapacity(s_AllActivePlayers, ref s_AllActivePlayersCount, index);
    // 如果是最后一个玩家，则取消挂钩更改通知。
    if (s_AllActivePlayersCount == 0 && s_UserChangeDelegate != null)
        InputUser.onChange -= s_UserChangeDelegate;
    StopListeningForUnpairedDeviceActivity(); // 停止监听未配对设备活动
    StopListeningForDeviceChanges(); // 停止监听设备更改
    // 触发离开事件。
    PlayerInputManager.instance?.NotifyPlayerLeft(this);
    ////TODO: 理想情况下，这不应该需要立即解析绑定，而是等待需要更新设置的人。
    // 在拆卸配置时避免重复解析绑定。
    using (InputActionRebindingExtensions.DeferBindingResolution())
    {
        DeactivateInput(); // 停用输入
        UnassignUserAndDevices(); // 取消分配用户和设备
        UninitializeActions(); // 取消初始化操作
    }
    m_PlayerIndex = -1; // 重置玩家索引
}

/// <summary>
/// 调试辅助方法，可以挂接到输入操作上。
/// </summary>
/// <example>
/// using UnityEngine;
/// using UnityEngine.InputSystem;
/// // 与 PlayerInput 相邻的组件。
/// [RequireComponent(typeof(PlayerInput))]
/// public class MyPlayerLogic : MonoBehaviour
/// {
///     public void OnEnable()
///     {
///         var playerInput = GetComponent<PlayerInput>();
///         playerInput.onActionTriggered += DebugLogAction;
///     }
///     public void OnDisable()
///     {
///         playerInput.onActionTriggered -= DebugLogAction;
///     }
/// }
/// </example>
public void DebugLogAction(InputAction.CallbackContext context)
{
    Debug.Log(context.ToString());
}

private void HandleDeviceLost()
{
    switch (m_NotificationBehavior)
    {
        case PlayerNotifications.SendMessages:
            SendMessage(DeviceLostMessage, this, SendMessageOptions.DontRequireReceiver);
            break;
        case PlayerNotifications.BroadcastMessages:
            BroadcastMessage(DeviceLostMessage, this, SendMessageOptions.DontRequireReceiver);
            break;
        case PlayerNotifications.InvokeUnityEvents:
            m_DeviceLostEvent?.Invoke(this);
            break;
        case PlayerNotifications.InvokeCSharpEvents:
            DelegateHelpers.InvokeCallbacksSafe(ref m_DeviceLostCallbacks, this, "onDeviceLost");
            break;
    }
}
private void HandleDeviceRegained()
{
    switch (m_NotificationBehavior)
    {
        case PlayerNotifications.SendMessages:
            SendMessage(DeviceRegainedMessage, this, SendMessageOptions.DontRequireReceiver);
            break;
        case PlayerNotifications.BroadcastMessages:
            BroadcastMessage(DeviceRegainedMessage, this, SendMessageOptions.DontRequireReceiver);
            break;
        case PlayerNotifications.InvokeUnityEvents:
            m_DeviceRegainedEvent?.Invoke(this);
            break;
        case PlayerNotifications.InvokeCSharpEvents:
            DelegateHelpers.InvokeCallbacksSafe(ref m_DeviceRegainedCallbacks, this, "onDeviceRegained");
            break;
    }
}
private void HandleControlsChanged()
{
    switch (m_NotificationBehavior)
    {
        case PlayerNotifications.SendMessages:
            SendMessage(ControlsChangedMessage, this, SendMessageOptions.DontRequireReceiver);
            break;
        case PlayerNotifications.BroadcastMessages:
            BroadcastMessage(ControlsChangedMessage, this, SendMessageOptions.DontRequireReceiver);
            break;
        case PlayerNotifications.InvokeUnityEvents:
            m_ControlsChangedEvent?.Invoke(this);
            break;
        case PlayerNotifications.InvokeCSharpEvents:
            DelegateHelpers.InvokeCallbacksSafe(ref m_ControlsChangedCallbacks, this, "onControlsChanged");
            break;
    }
}
private static void OnUserChange(InputUser user, InputUserChange change, InputDevice device)
{
    switch (change)
    {
        case InputUserChange.DeviceLost:
        case InputUserChange.DeviceRegained:
            for (var i = 0; i < s_AllActivePlayersCount; ++i)
            {
                var player = s_AllActivePlayers[i];
                if (player.m_InputUser == user)
                {
                    if (change == InputUserChange.DeviceLost)
                        player.HandleDeviceLost();
                    else if (change == InputUserChange.DeviceRegained)
                        player.HandleDeviceRegained();
                }
            }
            break;

        case InputUserChange.ControlsChanged:
            for (var i = 0; i < s_AllActivePlayersCount; ++i)
            {
                var player = s_AllActivePlayers[i];
                if (player.m_InputUser == user)
                    player.HandleControlsChanged();
            }
            break;
    }
}

private static bool OnPreFilterUnpairedDeviceUsed(InputDevice device, InputEventPtr eventPtr)
{
    // 如果设备无法与任何控制方案一起使用，则提前退出。
    var actions = all[0].actions;
    // 如果有任何 OnScreenControl 处于活动状态，则跳过 Pointer 设备，因为它们将使用它来生成设备事件。
    return actions != null && (!OnScreenControl.HasAnyActive || !(device is Pointer)) && actions.IsUsableWithDevice(device);
}

private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr) { } // 内部实现略
private void OnDeviceChange(InputDevice device, InputDeviceChange change) { } // 内部实现略
private void SwitchControlSchemeInternal(ref InputControlScheme controlScheme, params InputDevice[] devices) { } // 内部实现略

/// <summary>
/// 与 PlayerInput 操作关联的事件。
/// </summary>
/// <remarks>
/// <example>
/// <code>
/// public class MyPlayerScript : MonoBehaviour
/// {
///     void OnFireEvent(InputAction.CallbackContext context)
///     {
///         // 处理开火事件
///     }
/// }
/// </code>
/// </example>
/// <seealso cref="PlayerInput.actionEvents"/>
/// <seealso cref="InputAction.CallbackContext"/>
[Serializable]
public class ActionEvent : UnityEvent<InputAction.CallbackContext>
{
    /// <summary>
    /// 触发事件的操作的 GUID 字符串。
    /// </summary>
    public string actionId => m_ActionId;

    /// <summary>
    /// 触发事件的操作的名称。
    /// </summary>
    public string actionName => m_ActionName;

    [SerializeField] private string m_ActionId;
    [SerializeField] private string m_ActionName;

    public ActionEvent() { }
    public ActionEvent(InputAction action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        if (action.isSingletonAction)
            throw new ArgumentException($"操作必须是资源的一部分（给定的操作 '{action}' 是单例）");
        if (action.actionMap.asset == null)
            throw new ArgumentException($"操作必须是资源的一部分（给定的操作 '{action}' 不是）");

        m_ActionId = action.id.ToString();
        m_ActionName = $"{action.actionMap.name}/{action.name}";
    }
    public ActionEvent(Guid actionGUID, string name = null)
    {
        m_ActionId = actionGUID.ToString();
        m_ActionName = name;
    }
}
/// <summary>
/// 当与 PlayerInput 配对的 InputDevice 断开连接时触发的事件。
/// 可以通过 deviceLostEvent 设置设备丢失事件。
/// </summary>
[Serializable]
public class DeviceLostEvent : UnityEvent<PlayerInput>{}

/// <summary>
/// 当 PlayerInput 重新获得之前丢失的 InputDevice 时触发的事件。
/// 可以通过 deviceRegainedEvent 设置设备重新获得事件。
/// </summary>
[Serializable]
public class DeviceRegainedEvent : UnityEvent<PlayerInput>{}

/// <summary>
/// 当 PlayerInput 使用的控件集发生变化时触发的事件。
/// 可以通过 controlsChangedEvent 设置控件变化事件。
/// </summary>
[Serializable]
public class ControlsChangedEvent : UnityEvent<PlayerInput>{}
```


可以看到代码量很大，所以尽量使用 PlayerInput 吧，功能强大不是轻易能用自定义代码取代的。



### 使用代码控制小球

为了处理 Input System 的输入信息，可以添加 `PlayerController` 脚本，其实现如下：

```c#
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_Rb;
    private float m_MovementX;
    private float m_MovementY;
    [SerializeField] private float m_Speed = 5;

    private void Start() { m_Rb = GetComponent<Rigidbody>(); }

    private void OnMove(InputValue value)
    {
        var inputVector = value.Get<Vector2>();
        m_MovementX = inputVector.x;
        m_MovementY = inputVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new(m_MovementX, 0.0f, m_MovementY);
        m_Rb.AddForce(movement * m_Speed);
    }
}
```

其中的 `OnMove` 对应 [创建 Input Action Asset](#创建 Input Action Asset) 后 Asset 中的 `Move` Action：
 ![img](./img/image-20231117102349.jpg)

对于 Asset 中任意名称的 Action，都可以通过 `On<ActionName>` 监听到。
 如果 Action 叫做 `AAA`，则可以定义 `OnAAA` 函数监听。

将该脚本挂载在 `Player` 上，如下所示：
 ![挂载 PlayerController | 500](./img/2023-11-17-16-10-20.jpg)

↑↑ *挂载 PlayerController* ↑↑

此时小球就可以通过键盘的 `WASD` 和 `上下左右` 移动。

此时的工程状态见（基于旧版Unity 2022，仅供参考）：
 [xuejiaW/InputSystemSample at 9cb75b9a8719cc8e695ac32ad786adcc007494d8 (github.com)](https://github.com/xuejiaW/InputSystemSample/tree/9cb75b9a8719cc8e695ac32ad786adcc007494d8)



## 自定义 Action Asset

在之前 [创建 Input Action Asset](#创建 Input Action Asset) 步骤中，创建出来的 Input Actions 是 Unity 默认实现的，即适合于 `PlayerInput` 组件的 Actions 资源。

`PlayerInput` 组件也是 Unity 内建的读取 Assets 资源的脚本

在这一节，会自定义 Actions 资源，并自定义使用该 Actions 资源的脚本。

### 创建自定义 Action Asset

在 Project 面板中，空白处右键选择 `Create -> Input Actions`，创建出一个新的 Input Actions 资源：

双击创建的资源（本例中为 `BallControls.inputactions` ） 后会打开空白的 Input Actions 窗口：
 ![Empty Input Assets](./img/2023-11-17-15-37-34.jpg)

此时点击画面左侧的 `+` 号可以创建出 Input Action Map，我们将新增的 Input Action Map 命名为 `BallPlayer`（图省略）

在窗口中间，可以为这个 Input Action Map 创建一些 Input Action，如下过程，创建了 `Buttons` 这个 Input Action（图省略）

在窗口的右侧，可以为 Input Action 创建一系列 Input Binding，如下步骤分别绑定了 `GamePad` 的 `East Button` 和 `West Button` （图省略）


> `GamePad` 的 `East` 和 `West` Button，在 Xbox 控制器上分别对应 `X` 键和 `B` 键

你也可以继续为 `Buttons` Action 绑定 Keyboard 的 `F1` 和 `F2` 按键，步骤如上，当绑定完成后，整个 `Buttons` Action 如下所示：
[![img](./img/image-20231117155554.jpg)

进一步创建 `Move` Input Action ，与 `Buttons` 不同是，`Move` 需要将 Action Type 设置为 `Value`，且 Control Type 为 `Vector2`，这表示 Action 会返回 `Vector2` 数据，即用于平面移动的上下左右数据：

如之前步骤一样，为该 `Move` Input Action 绑定 `Left Stick`，绑定后结果如下：
![左摇杆](./img/2023-11-17-16-05-20.jpg)

↑↑ *左摇杆* ↑↑



也可以将键盘上的按键通过 组合绑定（Composite Bindings） 至 `Move` Input Action，如下所示，其逻辑为使用四个按键分别表示 `Vector2` 四个方向（+x, −x,+y, −y+x,-x,+y,-y+x, −x,+y, −y）：
![四按键组合绑定](./img/2023-11-17-16-17-41.jpg)

↑↑ *四按键组合绑定* ↑↑



分别为上下左右四个方向设定四个按键 `K,J,H,L`，结果如下所示：
![绑定后的四个按键](./img/2023-11-17-16-26-24.jpg)

↑↑ *绑定后的四个按键* ↑↑



至此，自定义的 Action Asset 创建完成，其中定义了使用 `GamePad` 和 `Keyboard` 两种输入设备，分别控制 `Buttons` 和 `Move` 两个 Action。将新建的 `BallControls.inputactions` 替换掉 `PlayerInput` 组件中的 Actions，即可使用新的 Action Asset：
 ![使用新 Asset](./img/2023-11-17-16-43-31.jpg)

↑↑ *使用新 Asset* ↑↑



此时运行游戏，可以发现通过手柄的左摇杆和 `HJKL` 都可以控制小球的移动，而 `WASD` 则不行了。

这是因为 `PlayerController` 脚本监听的 `Motion` 事件在 `BallControls.inputactions` 中也存在，因此我们定义的左摇杆和 `HJKL` 四个按键都能响应，即使不修改 `PlayerController` 也可以正常运行。而原 `PlayerInput.inputactions` 中的 `WASD` 我们并没有绑定，所以无法响应。

此时的工程状态见（基于旧版Unity 2022，仅供参考）：
 [xuejiaW/InputSystemSample at 8d9... (github.com)](https://github.com/xuejiaW/InputSystemSample/tree/8d994e47fbf7c766c87aa62ce517e7e5bdda031b)



### PlayerInput也可以被替代

PlayerInput 是内置组件，只管解析 inputactions 文件以及接收输入信号，不管控制移动等行为。下面将演示如何一步一步用自定义脚本替代它。

#### 手动解析 Actions Asset

自定义一个 `BallController` 脚本，用于解析刚刚创建的 `BallControls.inputactions`，其实现如下：

```c#
public class BallController : MonoBehaviour
{
    [SerializeField] private float m_Speed = 10;
    [SerializeField] private InputActionAsset m_Asset = null;

    private Rigidbody m_Rb;
    private Vector2 m_Move;

    private InputActionMap m_ActionMap = null;
    private InputAction m_ButtonsAction = null;
    private InputAction m_MoveAction = null;

    private void Awake()
    {
        m_ActionMap = m_Asset.FindActionMap("BallPlayer");
        m_ButtonsAction = m_ActionMap.FindAction("Button");
        m_MoveAction = m_ActionMap.FindAction("Move");
        m_MoveAction.canceled += _ => OnMove(Vector2.zero);

        m_ButtonsAction.performed += _ => OnButton();
        m_MoveAction.performed += ctx => OnMove(ctx.ReadValue<Vector2>());

        m_Rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() { m_ActionMap.Enable(); }

    private void OnButton() { Debug.Log("On Buttons clicked triggered"); }

    private void OnMove(Vector2 coordinates)
    {
        Debug.Log($"On move clicked triggered {coordinates.ToString("f4")}");
        m_Move = coordinates;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new(m_Move.x, 0.0f, m_Move.y);
        m_Rb.AddForce(movement * m_Speed);
    }

    private void OnDisable() { m_ActionMap.Disable(); }
}
```

可以看到，该脚本直接引用了之前的 `InputActionAsset` ，并使用了 [InputActionAsset.FindActionMap](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/api/UnityEngine.InputSystem.InputActionAsset.html#UnityEngine_InputSystem_InputActionAsset_FindActionMap_System_String_System_Boolean_) 找寻之前创建的 `BallPlayer`（Input Action Map），并在 `OnEnable` 和 `OnDisable` 时启用和禁用该 Input Action Map。

另外脚本中通过 [InputActionMap.FindAction](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/api/UnityEngine.InputSystem.InputActionMap.html#UnityEngine_InputSystem_InputActionMap_FindAction_System_String_System_Boolean_) 找寻之前创建的 `Buttons` 和 `Move` Action，并监听了 Input Action 的 `performed` 事件，触发对应的回调函数 `OnButton` 和 `OnMove`。

**省略 PlayerInput**

至此 `BallController` 脚本已经基本的实现了之前 `PlayerInput` + `PlayerController` 的功能，因此在 `Player` 游戏物体上仅需要 `BallController` 脚本即可，注意要将之前创建的 `BallControls.inputactions` 挂载至脚本中：
![img](./img/2023-11-17-17-30-55.jpg)

此时小球可以如同之前一样的通过手柄和键盘控制移动。

此时工程状态见（基于旧版Unity 2022，仅供参考）：
 [xuejiaW/InputSystemSample at f070... (github.com)](https://github.com/xuejiaW/InputSystemSample/tree/f070c14f0671d7c1c702b270f3751aed8c003692)



### 基于 Actions Asset 自动生成对应类

在 [手动解析 Actions Asset](#手动解析 Actions Asset) 中需要手动管理 `Actions Asset` 并从中读取 Input Action Map 和 Input Action。

上述的读取过程，会随着 `Action Assets` 中的数据变更而出现潜在的失效（如命名错误），因此 Unity 提供了从 `Action Assets` 中自动创建相应脚本的能力，可以简化上述步骤。

可以选择 `Action Assets` 便选择，并勾选其中的 `Generate C# Class` ，选择需要创建的类名称，文件和命名空间，并点击 `Apply` 正式创建脚本：
![Create ](./img/image-20231119134430.jpg)

此时会创建出对应的脚本：
![Automated Create Script](./img/image-20231119134523.jpg)

↑↑ *Automated Create Script* ↑↑

#### 自动创建的 BallControls 代码（已基于新的Unity6版本）

```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.13.0
//     from Assets/Custom/InputSystem/BallControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

/// <summary>
/// Provides programmatic access to <see cref="InputActionAsset" />, <see cref="InputActionMap" />, <see cref="InputAction" /> and <see cref="InputControlScheme" /> instances defined in asset "Assets/Custom/InputSystem/BallControls.inputactions".
/// </summary>
/// <remarks>
/// This class is source generated and any manual edits will be discarded if the associated asset is reimported or modified.
/// </remarks>
/// <example>
/// <code>
/// using namespace UnityEngine;
/// using UnityEngine.InputSystem;
///
/// // Example of using an InputActionMap named "Player" from a UnityEngine.MonoBehaviour implementing callback interface.
/// public class Example : MonoBehaviour, MyActions.IPlayerActions
/// {
///     private MyActions_Actions m_Actions;                  // Source code representation of asset.
///     private MyActions_Actions.PlayerActions m_Player;     // Source code representation of action map.
///
///     void Awake()
///     {
///         m_Actions = new MyActions_Actions();              // Create asset object.
///         m_Player = m_Actions.Player;                      // Extract action map object.
///         m_Player.AddCallbacks(this);                      // Register callback interface IPlayerActions.
///     }
///
///     void OnDestroy()
///     {
///         m_Actions.Dispose();                              // Destroy asset object.
///     }
///
///     void OnEnable()
///     {
///         m_Player.Enable();                                // Enable all actions within map.
///     }
///
///     void OnDisable()
///     {
///         m_Player.Disable();                               // Disable all actions within map.
///     }
///
///     #region Interface implementation of MyActions.IPlayerActions
///
///     // Invoked when "Move" action is either started, performed or canceled.
///     public void OnMove(InputAction.CallbackContext context)
///     {
///         Debug.Log($"OnMove: {context.ReadValue&lt;Vector2&gt;()}");
///     }
///
///     // Invoked when "Attack" action is either started, performed or canceled.
///     public void OnAttack(InputAction.CallbackContext context)
///     {
///         Debug.Log($"OnAttack: {context.ReadValue&lt;float&gt;()}");
///     }
///
///     #endregion
/// }
/// </code>
/// </example>
public partial class @BallControls: IInputActionCollection2, IDisposable
{
    /// <summary>
    /// Provides access to the underlying asset instance.
    /// </summary>
    public InputActionAsset asset { get; }

    /// <summary>
    /// Constructs a new instance.
    /// </summary>
    public @BallControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BallControls"",
    ""maps"": [
        {
            ""name"": ""BallPlayer"",
            ""id"": ""a8e921ca-9664-4773-aa8a-e8f07b931f8e"",
            ""actions"": [
                {
                    ""name"": ""Button"",
                    ""type"": ""Button"",
                    ""id"": ""ed867724-3fa5-4d64-9dba-84a81bffa03f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""385d6c02-ab3e-4a1b-9eeb-1878593ceb97"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e208811a-e3a2-411d-8684-61ead5b6c5e4"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3338e937-cc54-4c94-ae64-38132cde5090"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""765eb4b6-e88c-4647-8d7d-8b7674dfa6bf"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e052ae9-5ad1-40b6-a0de-04de710af518"",
                    ""path"": ""<Keyboard>/f2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb485172-a5ab-4aaf-8a3c-f4ae189db478"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""5761fb56-855d-47e4-b52d-e482d97abf59"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""199f0d0a-6369-4086-840f-e9c3867cf66c"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b7330026-d83e-4982-8c2f-4c9f0def1451"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ccf97183-f41f-47a0-9acd-2b51fef21e51"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c100aaf8-fd10-4d7d-9d17-77d26c6b655a"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BallPlayer
        m_BallPlayer = asset.FindActionMap("BallPlayer", throwIfNotFound: true);
        m_BallPlayer_Button = m_BallPlayer.FindAction("Button", throwIfNotFound: true);
        m_BallPlayer_Move = m_BallPlayer.FindAction("Move", throwIfNotFound: true);
    }

    ~@BallControls()
    {
        UnityEngine.Debug.Assert(!m_BallPlayer.enabled, "This will cause a leak and performance issues, BallControls.BallPlayer.Disable() has not been called.");
    }

    /// <summary>
    /// Destroys this asset and all associated <see cref="InputAction"/> instances.
    /// </summary>
    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindingMask" />
    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.devices" />
    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.controlSchemes" />
    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Contains(InputAction)" />
    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.GetEnumerator()" />
    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable.GetEnumerator()" />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Enable()" />
    public void Enable()
    {
        asset.Enable();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Disable()" />
    public void Disable()
    {
        asset.Disable();
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindings" />
    public IEnumerable<InputBinding> bindings => asset.bindings;

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindAction(string, bool)" />
    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindBinding(InputBinding, out InputAction)" />
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // BallPlayer
    private readonly InputActionMap m_BallPlayer;
    private List<IBallPlayerActions> m_BallPlayerActionsCallbackInterfaces = new List<IBallPlayerActions>();
    private readonly InputAction m_BallPlayer_Button;
    private readonly InputAction m_BallPlayer_Move;
    /// <summary>
    /// Provides access to input actions defined in input action map "BallPlayer".
    /// </summary>
    public struct BallPlayerActions
    {
        private @BallControls m_Wrapper;

        /// <summary>
        /// Construct a new instance of the input action map wrapper class.
        /// </summary>
        public BallPlayerActions(@BallControls wrapper) { m_Wrapper = wrapper; }
        /// <summary>
        /// Provides access to the underlying input action "BallPlayer/Button".
        /// </summary>
        public InputAction @Button => m_Wrapper.m_BallPlayer_Button;
        /// <summary>
        /// Provides access to the underlying input action "BallPlayer/Move".
        /// </summary>
        public InputAction @Move => m_Wrapper.m_BallPlayer_Move;
        /// <summary>
        /// Provides access to the underlying input action map instance.
        /// </summary>
        public InputActionMap Get() { return m_Wrapper.m_BallPlayer; }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
        public void Enable() { Get().Enable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
        public void Disable() { Get().Disable(); }
        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
        public bool enabled => Get().enabled;
        /// <summary>
        /// Implicitly converts an <see ref="BallPlayerActions" /> to an <see ref="InputActionMap" /> instance.
        /// </summary>
        public static implicit operator InputActionMap(BallPlayerActions set) { return set.Get(); }
        /// <summary>
        /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <param name="instance">Callback instance.</param>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
        /// </remarks>
        /// <seealso cref="BallPlayerActions" />
        public void AddCallbacks(IBallPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_BallPlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BallPlayerActionsCallbackInterfaces.Add(instance);
            @Button.started += instance.OnButton;
            @Button.performed += instance.OnButton;
            @Button.canceled += instance.OnButton;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
        }

        /// <summary>
        /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
        /// </summary>
        /// <remarks>
        /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
        /// </remarks>
        /// <seealso cref="BallPlayerActions" />
        private void UnregisterCallbacks(IBallPlayerActions instance)
        {
            @Button.started -= instance.OnButton;
            @Button.performed -= instance.OnButton;
            @Button.canceled -= instance.OnButton;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
        }

        /// <summary>
        /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="BallPlayerActions.UnregisterCallbacks(IBallPlayerActions)" />.
        /// </summary>
        /// <seealso cref="BallPlayerActions.UnregisterCallbacks(IBallPlayerActions)" />
        public void RemoveCallbacks(IBallPlayerActions instance)
        {
            if (m_Wrapper.m_BallPlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        /// <summary>
        /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
        /// </summary>
        /// <remarks>
        /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
        /// </remarks>
        /// <seealso cref="BallPlayerActions.AddCallbacks(IBallPlayerActions)" />
        /// <seealso cref="BallPlayerActions.RemoveCallbacks(IBallPlayerActions)" />
        /// <seealso cref="BallPlayerActions.UnregisterCallbacks(IBallPlayerActions)" />
        public void SetCallbacks(IBallPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_BallPlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BallPlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    /// <summary>
    /// Provides a new <see cref="BallPlayerActions" /> instance referencing this action map.
    /// </summary>
    public BallPlayerActions @BallPlayer => new BallPlayerActions(this);
    /// <summary>
    /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "BallPlayer" which allows adding and removing callbacks.
    /// </summary>
    /// <seealso cref="BallPlayerActions.AddCallbacks(IBallPlayerActions)" />
    /// <seealso cref="BallPlayerActions.RemoveCallbacks(IBallPlayerActions)" />
    public interface IBallPlayerActions
    {
        /// <summary>
        /// Method invoked when associated input action "Button" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnButton(InputAction.CallbackContext context);
        /// <summary>
        /// Method invoked when associated input action "Move" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
        /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
        void OnMove(InputAction.CallbackContext context);
    }
}

```



重做之前的 `BallController` 脚本，新建一个空的mono脚本，继承自动生成脚本 `BallControls` 的一个接口：

```csharp
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour, BallControls.IBallPlayerActions
{
}
```

让IDE（VSCode、VS、Rider随便哪款）来自动填充接口实现：

```csharp
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour, BallControls.IBallPlayerActions
{
    public void OnButton(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}
```

再把之前的 `BallController` 逻辑补全一下

#### 最终BallController代码

```csharp
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour, BallControls.IBallPlayerActions
{
    [SerializeField] private float m_Speed = 10;
    private Rigidbody m_Rb;
    private Vector2 m_Move;
    //---以下都是固定写法---
    private BallControls m_Actions;
    private BallControls.BallPlayerActions m_Player;
    void Awake()
    {
        m_Actions = new BallControls();
        m_Player = m_Actions.BallPlayer;
        m_Player.AddCallbacks(this);
        //此时不再需要手动获取 Input Action Map 和 Input Action
        //只需要使用 <ActionControls>.<ActionMapName>.<Action>的风格直接访问即可。
        //如本代码中就是：m_Actions.BallPlayer.Move
        //-----以上都是固定写法------
        //定制内容
        m_Rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 movement = new(m_Move.x, 0.0f, m_Move.y);
        m_Rb.AddForce(movement * m_Speed);
    }
    //---以下都是固定写法---
    void Oestroy()
    {
        m_Actions.Dispose();
    }
    void OnEnable() { m_Player.Enable(); }
    void OnDisable() { m_Player.Disable(); }
    //-----以上都是固定写法------
    #region 接口实现
    public void OnButton(InputAction.CallbackContext context)
    {
/*因为事件绑的太全了，要自己根据以下状态进行过滤
Debug.Log($"phase:{context.phase}, started:{context.started}, performed:{context.performed}, canceled:{context.canceled}, time:{DateTime.Now.Millisecond}");
基本可以只靠context.phase来判断回调来源
对于键盘按键来说，【通常情况】是键盘按下立刻先后触发两个回调：
1.phase:Started, started:True, performed:False, canceled:False
2.phase:Performed, started:False, performed:True, canceled:False
键盘抬起触发一个回调：
3.phase:Canceled, started:False, performed:False, canceled:True
但也有【其常情况】，比如左shift键，ActionType用的不是Button而是Pass Through
没有Started和Canceled状态，按下和抬起都是触发的Performed
需要做以下设置：
在Input Action Editor面板里，右边Binding Properties中有个Interactions设置，默认是空的
给它添加了一个press，Trigger Behavior设为PressOnly就好了。
再次测试，三个回调已经是【通常情况】。
确定回调执行顺序正常之后，通常的过滤方案：
遇到Started：可以直接return
遇到Performed：处理按下逻辑
遇到Canceled：处理抬起逻辑
*/
        Debug.Log("我会执行三次");
        if(context.phase == InputActionPhase.Performed)
        {
            Debug.Log("按下");
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("抬起");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();       
    }
    #endregion 接口实现
}
```

用这个新的 `BallController` 脚本替换老的脚本，并运行，可以看到效果与之前的效果无差别。

此时工程状态见（代码没有本文的新，本文的已经是Unity6）：
 [xuejiaW/InputSystemSample at a92af1df6ccfb22f2747548d2abf1a08c43a3407 (github.com)](https://github.com/xuejiaW/InputSystemSample/tree/a92af1df6ccfb22f2747548d2abf1a08c43a3407)



# 摘抄：基础教程——Unity 官方开发者社区

随着 Unity 的不断发展，开发者们对 Unity 的项目输入系统要求也越来越高，经常会有项目在做多平台适配和跨平台移植时对变更输入系统而感到烦恼。而 InputSystem 这款插件正是 Unity 官方为了解决广大开发者而推出的一款新的输入方式。

## 基础概念

### 前言

相较于旧版的 InputManager，能更好 **应对跨平台项目**。  [附视频教程](https://blog.csdn.net/JavaD0g/article/details/131027152)

## 基础操作

### 插件安装

`Package Manager` 的 `Unity Registry` 分类下，搜索 `InputSystem` 插件并安装。

### 如何创建 InputActions

安装插件后，如果发现  `Project右键 > Creater > InputActions`，证明插件安装成功，点击创建扩展名为 inputactions 的文件，双击可打开编辑。

### InputActions 概念及结构关系

结构关系为 ：

- InputSystem：插件
  - InputActions：文件
    - ActionMaps：大类，区分键盘手柄
      - Actions：子项，区分跑跳
        - Action Properties：细则

### ActionProperties 细则说明

在 Actions 中也有许多参数，其中 ActionType 则是我们最常用到。其概念为我们该动作输入映射的类型，有以下三种类型:

- Button 默认设置，包括按钮或者按键触发的一次性动作，适合攻击或者点击 UI
- Value 提供一种连续状态变化事件，适合角色移动。如果设置了多个输入，就会切换到最主要的一个。
- Pass Through 和 Value 很相似，但它不会像 Value 一样（如果有多个输入时，会同时参考这些输入值）

 ![img](./img/013f975b-0ba0-4c96-a9f4-36c68fa1d3fa_image.jpg)

在使用 Value 或者 Pass Through 时，你会看到一个额外的选项 Control Type 为该 Value 的返回值类型：

 ![img](./img/6474a595-71ab-41c0-a22c-611825256d39_image.jpg)

## 动作映射调用

### 官方 PlayerInput 组件调用

我们需要在场景中在我们需要的对象上添加 PlayerInput 组件，填入我们刚刚创建好的 InputActions，选择想要使用的 ActionMap 输入映射集，再选择 Behavior 调用方式（有四种方式）

![](./img/9e7b10fb-869f-4429-9b84-5084ceea053f_image.jpg)

#### Send Message 方式

使用 Send Message 时，每次的触发会调用一个对应的函数（就是在对应的 Actions 名前面加个 On-）正如下图所示在我们 PlayerInput 组件当中我们将 BehaviorType 选择 Send Message 后我们的输入参数将会通过 Send Message 方法发送到我们对应生成的函数中。比如 Input Action 名为 Jump，那么对应的函数即为 OnJump

![img](./img/2704d399-a15e-4bc5-a0d7-745919379f81_image.jpg)

获取输入时的数据，我们可以写一个输入控制类，在该类中调用我们上述说到的 Actions 生成函数 可以通过 isPressed 获取设置了 ActionType 为 Button 类型的动作是否点击 可以通过 Get 获取设置了 ActionType 为 Value 对应类型的数据

```csharp
public class PlayerController : MonoBehaviour
{
    void OnAction1(InputValue value)
    {
        bool isAction1Pressd = value.isPressed;
        Debug.Log(isAction1Pressd);
    }
    void OnMove(InputValue value)
    {
        Vector2 moveVal = value.Get<Vector2>();
        Debug.Log(moveVal );
    }
    void OnJump(InputValue value) 
    { 
        float triggerVal = value.Get<float>(); 
        Debug.Log(triggerVal );
    }
}
```

#### Broadcast Messages 方式

Broadcast Messages 与 send Message 很相似（但目前我还没有搞懂具体区别）

#### Invoke Unity Events 方式

区别于上述两种 BehaviorType 不同的是，在我们选择该方法后会出现 Events 的选项，我们需要自己写好动作方法后将其挂载到我们对应的 ActionMaps 中对应的 ActionEvents 上才能触发对应的动作事件。

 ![img](./img/e1734733-f4a9-4756-86af-b7e4e50ded8f_image.jpg)

定义参数为 `InputAction.CallbackContext` 类型的方法，即可获取我们对应输入返回（且加入该参数后会将其方法置顶如上图所示）

```csharp
public class PlayerController : MonoBehaviour
{
    public void moveControl(InputAction.CallbackContext value)
    {
        Vector2 moveVal = value.ReadValue<Vector2>();
        Debug.Log(moveVal);
    }
}
```

#### Invoke CSharp Events 方式

与 Invoke Unity Events 方式其实大致相同，需要我们自己先写好一个带有 InputAction.CallbackContext 类型入参的动作方法，不同的是我们挂载方式变成了脚本事件加载而不是在 Unity 界面上的可视化挂载

```csharp
using UnityEngine;
using UnityEngine.InputSystem;
public class CSharpEvent : MonoBehaviour
{
    public PlayerInput playerInput;
    void OnEnable()
    {
        playerInput.onActionTriggered += MyEventFunction;
    }
    void OnDisable()
    {
        playerInput.onActionTriggered -= MyEventFunction;
    }
    void MyEventFunction(InputAction.CallbackContext value)
    {
        Debug.Log(value.action.name + (" was triggered"));
    }
}
```

提示：在我自己尝试下发现上述四种的官方组件调用方式都只在输入发生时触发时发送一次输入返回，并不会持续发送，**所以不适合做移动，只适合做跳跃或 UI 点击**。

### 自动化脚本

参见上文[基于 Actions Asset 自动生成对应类](#基于 Actions Asset 自动生成对应类)



# Unity 6 中 `.inputsettings` 文件的行为

我用 Unity6 新工程验证了一下，.inputsstings 文件依然有用，但不是初始自动生成的，初始就是一个 inputactions 文件。unity 会检测项目内所有的.inputsettings 文件，如果一个都没检测到，playerSettings 会显示一个按钮，点击按钮会创建第一个 inputsettings 文件。当我手动复制出多个 inputsettings 文件，那些后出现的 inputsettings 文件的 inspector 属性面板会提示当前未在使用，可以点击切换成当前使用的。

------

## 如何使用多个 `.inputsettings` 文件

根据验证结果，以下是如何使用多个 `.inputsettings` 文件的建议：

1. **创建多个配置文件**：

   - 复制现有的 `.inputsettings` 文件，重命名并根据需要修改配置。
   - 例如，可以创建一个 `PC.inputsettings` 和一个 `Mobile.inputsettings`。

2. **切换配置文件**：

   - 在 Unity Editor 中，选择需要使用的 `.inputsettings` 文件，然后在 Inspector 面板中点击“切换”按钮。

   - 在运行时，也可以通过代码动态切换：

     ```c#
     InputSystem.settings = customInputSettings; // customInputSettings 是你的 .inputsettings 文件
     ```

3. **为不同平台配置**：

   - 在 `Player Settings` 中，结合 `Platform Settings` 和不同的 `.inputsettings` 文件，为不同平台定制输入行为。

------

## 总结

- Unity 6 中，`.inputsettings` 文件仍然有用，但不会在初始状态下自动生成。
- 你可以手动创建多个 `.inputsettings` 文件，并通过 Inspector 面板或代码切换使用。
- 这种设计提供了更高的灵活性和可管理性，特别适合需要多套输入配置的项目。



# 关于我的 Dance Club 游戏，分阶段按键映射方案

> 关于Unity的InputSystem的使用，我想实现ESC键持续监听，WASD四个键只在角色行走模式监听，JKIL四个键只在角色跳舞模式监听，应该如何设计InputAction的map分布？

在Unity的Input System中，你可以通过设计多个`InputActionMap`来实现不同模式下按键的监听。以下是实现你需求的具体步骤：

------

##  创建Input Action Asset

- 在Unity中创建一个`Input Action Asset`（右键 -> Create -> Input Actions）。
- 打开该Asset，进入编辑界面。

------

##  设计InputActionMap

- 创建多个`InputActionMap`，分别对应不同的模式：
  - **GlobalMap**：用于监听全局按键（如ESC键）。
  - **WalkMap**：用于监听角色行走模式的按键（如WASD）。
  - **DanceMap**：用于监听角色跳舞模式的按键（如JKIL）。

------

## 定义InputAction

- 在每个`InputActionMap`中定义对应的`InputAction`：
  - **GlobalMap**：
    - 添加一个`InputAction`，命名为`Escape`，绑定到`ESC`键。
  - **WalkMap**：
    - 添加四个`InputAction`，分别命名为`MoveForward`、`MoveBackward`、`MoveLeft`、`MoveRight`，绑定到`W`、`S`、`A`、`D`键。
  - **DanceMap**：
    - 添加四个`InputAction`，分别命名为`Dance1`、`Dance2`、`Dance3`、`Dance4`，绑定到`J`、`K`、`I`、`L`键。

------

###  设置Action的触发方式

- 对于`GlobalMap`中的`Escape`，设置为 `Performed`（按下时触发，而不是Continuous按下时每帧连续触发）。
- 对于`WalkMap`和`DanceMap`中的按键，设置为 `Performed`（按下时触发）。

------

##  编写脚本控制InputActionMap的启用与禁用

- 在脚本中动态启用或禁用`InputActionMap`，以切换不同的模式。
- 示例代码：

```c#
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActionMap globalMap;
    private InputActionMap walkMap;
    private InputActionMap danceMap;

    private void Awake()
    {
        // 获取Input Action Asset
        var inputActions = new PlayerInputActions();
        globalMap = inputActions.GlobalMap;
        walkMap = inputActions.WalkMap;
        danceMap = inputActions.DanceMap;

        // 绑定事件
        globalMap["Escape"].performed += OnEscapePressed;
        walkMap["MoveForward"].performed += OnMoveForward;
        danceMap["Dance1"].performed += OnDance1;
    }

    private void OnEnable()
    {
        // 默认启用全局Map
        globalMap.Enable();
    }

    private void OnDisable()
    {
        globalMap.Disable();
        walkMap.Disable();
        danceMap.Disable();
    }

    // 切换模式
    public void SwitchToWalkMode()
    {
        walkMap.Enable();
        danceMap.Disable();
    }

    public void SwitchToDanceMode()
    {
        danceMap.Enable();
        walkMap.Disable();
    }

    // 事件处理
    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        Debug.Log("ESC键被按下");
    }

    private void OnMoveForward(InputAction.CallbackContext context)
    {
        Debug.Log("W键被按下，角色向前移动");
    }

    private void OnDance1(InputAction.CallbackContext context)
    {
        Debug.Log("J键被按下，角色跳舞动作1");
    }
}
```

------

## 切换模式

- 在游戏中通过调用`SwitchToWalkMode`或`SwitchToDanceMode`来切换不同的模式。
- 例如：
  - 当角色进入行走模式时，调用`SwitchToWalkMode`。
  - 当角色进入跳舞模式时，调用`SwitchToDanceMode`。

------

## 注意事项

- 确保在切换模式时，禁用不需要的`InputActionMap`，以避免按键冲突。
- 如果需要更复杂的逻辑（如组合键或长按检测），可以在`InputAction`的回调中进一步处理。

------

通过以上设计，你可以实现ESC键的持续监听，以及在不同模式下对WASD和JKIL按键的监听。