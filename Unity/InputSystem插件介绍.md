参考文章：

- [【Unity InputSystem】基础教程（保姆级超详细超基础！！！） - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/64ce58b4edbc2a10dd1e49b8)
- [Quickstart Guide | Input System | 1.10.0 (unity3d.com)](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/QuickStartGuide.html)
- [更好上手的Unity Input System教程_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1VCNdeSER3/)

# 【Unity InputSystem】基础教程摘抄

随着Unity的不断发展，开发者们对Unity的项目输入系统要求也越来越高，经常会有项目在做多平台适配和跨平台移植时对变更输入系统而感到烦恼。而InputSystem这款插件正是Unity官方为了解决广大开发者而推出的一款新的输入方式。

## 基础概念

### 前言

相较于旧版的InputManager，能更好**应对跨平台项目**。  [附视频教程](https://blog.csdn.net/JavaD0g/article/details/131027152)

## 基础操作

### 插件安装

`Package Manager` 的 `Unity Registry` 分类下，搜索 `InputSystem` 插件并安装。

### 如何创建InputActions

安装插件后，如果发现  `Project右键 > Creater > InputActions`，证明插件安装成功，点击创建InputActions，双击可打开编辑。

### InputActions概念及结构关系

结构关系为 ：

- InputSystem：插件
  - InputActions：文件
    - ActionMaps：大类，区分键盘手柄
      - Actions：子项，区分跑跳
        - Action Properties：细则

### ActionProperties 细则说明

在Actions中也有许多参数，其中ActionType则是我们最常用到。其概念为我们该动作输入映射的类型，有以下三种类型:

- Button 默认设置，包括按钮或者按键触发的一次性动作，适合攻击或者点击UI
- Value 提供一种连续状态变化事件，适合角色移动。如果设置了多个输入，就会切换到最主要的一个。
- Pass Through 和 Value 很相似，但它不会像Value一样（如果有多个输入时，会同时参考这些输入值）

 ![img](https://u3d-connect-cdn-public-prd.cdn.unity.cn/h1/20230805/p/images/013f975b-0ba0-4c96-a9f4-36c68fa1d3fa_image.png)

在使用 Value 或者 Pass Through 时，你会看到一个额外的选项 Control Type 为该 Value 的返回值类型：

 ![img](https://u3d-connect-cdn-public-prd.cdn.unity.cn/h1/20230805/p/images/6474a595-71ab-41c0-a22c-611825256d39_image.png)

## 动作映射调用

### 官方PlayerInput组件调用

我们需要在场景中在我们需要的对象上添加PlayerInput组件，填入我们刚刚创建好的InputActions，选择想要使用的ActionMap输入映射集，再选择 Behavior 调用方式（有四种方式）

![](https://u3d-connect-cdn-public-prd.cdn.unity.cn/h1/20230805/p/images/9e7b10fb-869f-4429-9b84-5084ceea053f_image.png)

#### Send Message 方式

使用Send Message时，每次的触发会调用一个对应的函数（就是在对应的Actions名前面加个On-）正如下图所示在我们PlayerInput组件当中我们将BehaviorType选择Send Message后我们的输入参数将会通过Send Message方法发送到我们对应生成的函数中。比如Input Action 名为 Jump，那么对应的函数即为 OnJump

![img](https://u3d-connect-cdn-public-prd.cdn.unity.cn/h1/20230805/p/images/2704d399-a15e-4bc5-a0d7-745919379f81_image.png)

获取输入时的数据，我们可以写一个输入控制类，在该类中调用我们上述说到的Actions生成函数 可以通过isPressed获取设置了ActionType为Button类型的动作是否点击 可以通过Get 获取设置了ActionType为Value对应类型的数据

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

区别于上述两种BehaviorType不同的是，在我们选择该方法后会出现Events的选项，我们需要自己写好动作方法后将其挂载到我们对应的ActionMaps中对应的ActionEvents上才能触发对应的动作事件。

 ![img](https://u3d-connect-cdn-public-prd.cdn.unity.cn/h1/20230805/p/images/e1734733-f4a9-4756-86af-b7e4e50ded8f_image.png)

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

与Invoke Unity Events方式其实大致相同，需要我们自己先写好一个带有InputAction.CallbackContext类型入参的动作方法，不同的是我们挂载方式变成了脚本事件加载而不是在Unity界面上的可视化挂载

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

提示：在我自己尝试下发现上述四种的官方组件调用方式都只在输入发生时触发时发送一次输入返回，并不会持续发送，**所以不适合做移动，只适合做跳跃或UI点击**。

### 自定义脚本代替PlayerInput

基于上述提示，所以官方PlayerInput组件调用动作事件函数时并不能满足我们所有的场景需求（也可能是我在持续返回信号上没找到解决方案），所以我们还需要学习一下不借助官方PlayerInput组件的事件调用。我们直接在我们的脚本中调用InputSystem中的动作事件。

在我们使用脚本调用之前我们需要做一件事情，在我们创建好的InputActions属性面板中找到Generate C# Class并勾选,随后点击Apply生成对应的脚本，之后我们就可以在我们自己写的PlayerController 类中调用该脚本了

 ![img](https://u3d-connect-cdn-public-prd.cdn.unity.cn/h1/20230805/p/images/51f7753c-7a53-4257-8cda-2fdf8a9e4654_image.png)

在此就不做过多的说明了，直接上代码（自己看注释理解）

```csharp
using UnityEngine;
using UnityEngine.InputSystem;
public class CSharpEvent : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    //将对应的ActionMaps中对应的Action进行传址引用
    public Vector2 keyboardMoveAxes => playerInputActions.keyboard.moveControl.ReadValue<Vector2>();

    void Awake() {
        //实例化InputActions脚本
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable(){
        //将要使用的ActionMap开启
        playerInputActions.keyboard.Enable();
    }
    private void OnDisable()
    {
        //上述同理
        playerInputActions.keyboard.Disable();
    }
    private void Update()
    {
        //在帧更新方法中调用所写的动作方法
        movePlayer();
    }
    private void movePlayer(){
        //因为要在Update方法中使用，需要需要先判断是否有输入对应的Input操作
        if(keyboardMoveAxes != Vector2.zero){
            //判断有输入后便执行对应方法
            Debug.Log(keyboardMoveAxes);
        }
    }
}
```

# 3.引用文献

- [Unity InputSystem官方使用手册](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/Installation.html)
- [Unity input system 使用记录（实例版）- 作者 : xkxsxkx](https://blog.csdn.net/qq_38836770/article/details/125240140)
- [Unity New Input System 作者 : 小虫儿飞到花丛中](https://blog.csdn.net/weixin_44542069/article/details/124122792)
- [B站视频版教学](https://space.bilibili.com/347855339/channel/collectiondetail?sid=1434940)
- [InputSystem进阶：实现角色移动跳跃](https://blog.csdn.net/JavaD0g/article/details/131217718)