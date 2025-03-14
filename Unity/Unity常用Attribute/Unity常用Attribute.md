# Unity常用特性Attribute介绍使用

## HelpURLAttribute

 从字面意思理解，是查看帮助时，跳转到指定的页面。

如下图：

![](./img/HelpURLAttribute.jpg)

对应着蓝色小书的图标，点击以后会跳转到配置的URL。

## RangeAttribute

限定int或float的取值范围。

Attribute used to make a float or int variable in a script be restricted to a specific range.
 When this attribute is used, the float or int will be shown as a slider in the Inspector instead of the default number field.

当在int或float上应用RangeAttribute特性时，在Inspector面板中，显示的将是一个slider滑动条，而不是默认的数值字段。

```c#
[Range(0.1f,0.9f)]
float ratio;
```

## RequireComponentAttribute

自动添加所要依赖的组件，如将一个Script做为一个GameObject的组件，而这个Script需要访问Rigidbody组件，
 通过应用该属性，可以自动的添加Rigidbody组件到当前的GameObject中，避免设置错误。

*前提是：如果当前的Script已经添加到了GameObject，这时候你再应用RequireComponent特性是无效的，
 你必须删除 再重新添加，才会检测。

```c#
using UnityEngine;

// PlayerScript requires the GameObject to have a Rigidbody component
[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up);
    }
}
```

## TooltipAttribute

在Inspector面板中，为一个字段Field指定一个提示。

![](./img/TooltipAttribute.jpg)

```c#
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
    [Tooltip("Health value between 0 and 100.")]
    public int health = 0;
}
```

## HideInInspectorAttribute

让一个可被序列化的字段，不要显示在Inspector面板中，防止修改。

```c#
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
    [HideInInspector]
    public int p = 5;
}

```

## ExecuteInEditMode

让MonoBehaviour脚本的所有实例，在编辑模式下可运行。
这些函数的调用并不会像在PlayMode下那样：

The functions are not called constantly like they are in play mode.

- [Update](MonoBehaviour.Update.html) is only called when something in the scene changed.
- [OnGUI](MonoBehaviour.OnGUI.html) is called when the Game View recieves an [Event](Event.html).
- [OnRenderObject](MonoBehaviour.OnRenderObject.html) and the other rendering callback functions are called on every repaint of the Scene View or Game View.

只有窗口在发生改变 ，接触新的事件，重绘后才会调用。

*默认MonoBehaviour的脚本只能在运行模式下执行。

```c#
using UnityEngine;

[ExecuteInEditMode]
public class PrintAwake : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Editor causes this Awake");
    }

    void Update()
    {
        Debug.Log("Editor causes this Update");
    }
}
```

## DisallowMultipleComponent

禁止一个组件被重复的添加多次。

*如果当前GameObject已经存在了多个相同的组件，是不会有影响的，但应用了特性以后，就无法再次添加。

```c#
[DisallowMultipleComponent]
public class testEdit : MonoBehaviour {...}
```

## DelayedAttribute

在运行时，我们修改Inspector面板中的字段，会即时返回新的值，应用Delayed特性，只有在用户按下回车Enter或是
 焦点离开时才会返回新值，通过用于调试阶段。

*只能应用于字段，不可在类或其它目标元素上使用。

## SpaceAttribute

 间隔距离，在Inspector中，可以设置元素与元素之间的间隔。

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEdit : MonoBehaviour {
    public int a = 10;
    [Space(50)]
    public int b = 11;
}
```

![](./img/SpaceAttribute.jpg)

## TextAreaAttribute

 可以在一个高自由度并且可滚动的文本区域里编辑string字符串，如果字符串比较长，比较适用。

参数:
 minLines:文本区域最小行数
 maxLines:文本区域最大行数，超过最大行数，会出现滚动条。



```csharp
[TextArea(1,5)]
public string abc;
```

## MultilineAttribute

 在一个支持多行的文本区域内编辑string字符串，他和TextAreaAttribute不同，MultilineAttribute的TextArea没有滚动条。



```csharp
[Multiline(1,5)]
public string abc;
```



## ContextMenu

 向Inspector面板中脚本Script的上下文菜单（快捷，右键），添加一条指令，当选择该命令时，函数会执行。
 *只能用于非静态函数



```csharp
public class ContextTesting : MonoBehaviour {
    /// Add a context menu named "Do Something" in the inspector
    /// of the attached script.
    [ContextMenu ("Do Something")]
    void DoSomething () {
        Debug.Log ("Perform operation");
    }
}
```

![](./img/ContextMenuAttribute.jpg)

预定义的一些方法，如Reset，是可以进行重载的。



## ContextMenuItemAttribute

 在Inspector面板中，为字段field添加一个快捷的菜单。

```csharp
[Multiline][ContextMenuItem("Reset", "ResetString")]
    public string abc;
    public void ResetString()
    {
        abc = "";
    }
```

![](./img/ContextMenuItemAttribute.jpg)

## CreateAssetMenuAttribute

快速的创建ScriptableObject派生类的实例，并存储成以“.asset"结尾的文件，ScriptableObject的派生类可以存储为外部的文件，图形化编辑对象数据，一些静态的数据，动态的加载，ScriptableObject是一种解决方案，具体见另一篇文章的说明：
 https://www.jianshu.com/p/da578e55ca47



```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "xxxx",menuName = "xxx/xxx")]
public class testEdit : ScriptableObject {
    public int a = 10;
    public int b = 11;
    public int c = 12;
    [Multiline][ContextMenuItem("Reset", "ResetString")]
    public string abc;

}
```

说明：
 fileName:生成asset文件的文件名。
 menuName:在Assets/Create上子菜单的名字。



## ColorUsageAttribute

颜色选择器，color picker，只能应用在Color字段上。
 默认参数为是否显示alpha，具体使用看下官方文档的参数描述，这里不加代码了

![](./img/ColorUsageAttribute.jpg)

## AddComponentMenu

 把添加Script的操作放在Component菜单下，来替代Component/Scripts，因为里面的脚本可能非常多，基本上没有实用价值
 AddComponentMenu的方便之处在于如果你当前场景内，有多个GameObjects需要添加同一个脚本，那么使用同时选中
 这些地象，并打开Component菜单选中要添加的脚本就可以一次性添加了。



```csharp
[AddComponentMenu("Example/testEdits")]
public class testEdit : MonoBehaviour {
```

## AssemblyIsEditorAssembly

添加该特性的任意类，都会被视为Editor编辑器类。只有用于Editor模式下。

## PreferBinarySerialization

 只能用在ScriptableObject的派生类上（使用在其它类型上面会被忽略），修改序列化的模式为二进制，而不是YAML，  当你的资源比较大的时候，保存成二进制，生成的数据会更加的紧凑，从而提高程序的读写性能。



```csharp
[CreateAssetMenu]
[PreferBinarySerialization]
public class testEdit : ScriptableObject {
    
    public Color a;
    public int b = 11;
    public int c = 12;  
    [Multiline][ContextMenuItem("Reset", "ResetString")]
    public string abc;

}
```

用记事本打开生成后的asset，会发现都是二进制的数据。

## RuntimeInitializeOnLoadMethodAttribute

 在运行时，当前类初始化完成，自动调用被该特性应用的静态函数,这和static静态构造函数还不一样，static静态构造函数是在所有的方法之前运行的，而RuntimeInitializeOnLoadMethod特性的方法是Awake方法之后执行的(如果是MonoBehaviour派生类)。

如果一个类中有多个静态方法使用了RuntimeInitializeOnLoadMethod特性，执行顺序是不固定的。



```csharp
[RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("After scene is loaded and game is running");
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnSecondRuntimeMethodLoad()
    {
        Debug.Log("SecondMethod After scene is loaded and game is running.");
    }
```

## SelectionBaseAttribute

 设置基础选中对象，应用该标识一个对象为选中对象，当我们在scene view中选择一个objects的时候，u3d会返回给我们是适合的对象，比如你选中的对象是prefab的一部分，默认会返回节点的根对象，默认根对象被设置成了基础选中对象，你可以修改他，让其它的对象成为基础选中对象，比如根对象可能就是一个空的GameObject，而我们要实际查看编辑的对象是子节点，这样我们可以将子节点中添加的脚本应用SelectionBase特性。

![](./img/SelectionBaseAttribute_1.jpg)

![](./img/SelectionBaseAttribute_2.jpg)

我将脚本加到Camera_Offset后，成为了默认的选中对象，这样每次我在场景中选中时，Camera_Offset会被选择，并高亮显示。

## SerializeField

 强制去序列化一个私有的字段field.默认情况下，当u3d在序列化脚本的时候，只会序列化public的字段，这是u3d内部的实现的序列化，并不是 .NET's serialization的实现。
 另一点，私有字段，你不希望派生类访问，但你希望在Inspector中可以进行配置，也可以应用SerializeField来解决。



```csharp
using UnityEngine;

public class SomePerson : MonoBehaviour
{
    //This field gets serialized because it is public.
    public string name = "John";

    //This field does not get serialized because it is private.
    private int age = 40;

    //This field gets serialized even though it is private
    //because it has the SerializeField attribute applied.
    [SerializeField]
    private bool hasHealthPotion = true;

    void Update()
    {
    }
}
```



## SharedBetweenAnimatorsAttribute

 状态机组件，没有在项目中使用过，mark下，在简书里新建一篇叫StateMachineBehaviour的文章，实际测试案例后，回来补充。



## HeaderAttribute

 在Inspector面板中，为field字段添加头信息，增强描述。
 The header is done using a DecoratorDrawer.



```csharp
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
    [Header("Health Settings")]
    public int health = 0;
    public int maxHealth = 100;
    [Header("Shield Settings")]
    public int shield = 0;
    public int maxShield = 0;
}
```



##  GUITargetAttribute

 设置OnGUI方法在哪一个Display下显示，默认是所有的Display均显示.

```cpp
[GUITarget(0,1,new int[]{2,3,4})]
void OnGUI()
{
    if (GUI.Button (new Rect (0, 0, 128, 128), "Test")) {
        Debug.Log ("blahblahblah....");
    }
}
```

![](./img/GUITargetAttribute.jpg)

说明：
 提供了如下参数：
 displayIndex    Display index.display 索引
 displayIndex1   Display index. display索引
 displayIndexList  Display index list.display索引列表

# 经典自定义Attribute

## 在Transform变量下面加三个按钮

其中第一个按钮支持自定义功能

```csharp
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;
#endif

namespace DNATools
{
    /// <summary>
    /// 使用方法示例：[DNATools.TransformButton("Test Model", nameof(TestModel))]
    /// </summary>
    public class TransformButtonAttribute : PropertyAttribute
    {
        public string ButtonLabel { get; private set; } // 按钮的显示文本
        public string MethodName { get; private set; } // 回调函数的方法名

        // 构造函数，接受按钮文本和回调函数的方法名
        public TransformButtonAttribute(string buttonLabel, string methodName)
        {
            ButtonLabel = buttonLabel;
            MethodName = methodName;
        }
    }
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TransformButtonAttribute))]
    public class TransformButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 获取 TransformButtonAttribute
            TransformButtonAttribute transformButton = attribute as TransformButtonAttribute;

            // 确保字段类型是 Transform
            if (property.propertyType == SerializedPropertyType.ObjectReference &&
                (property.objectReferenceValue == null || property.objectReferenceValue is Transform))
            {
                // 绘制 Transform 字段
                EditorGUI.PropertyField(position, property, label);

                // 计算按钮的位置
                //Rect buttonRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);

                // 绘制按钮
                //if (GUI.Button(buttonRect, transformButton.ButtonLabel))
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button(transformButton?.ButtonLabel))
                {
                    // 获取 Transform 值
                    Transform transform = (Transform)property.objectReferenceValue;

                    // 获取脚本实例
                    UnityEngine.Object targetObject = property.serializedObject.targetObject;
                    Type targetType = targetObject.GetType();

                    // 通过反射获取方法
                    MethodInfo method = targetType.GetMethod(transformButton.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                    if (method != null)
                    {
                        // 调用方法
                        if (method.IsStatic)
                        {
                            method.Invoke(null, new object[] { transform });
                        }
                        else
                        {
                            method.Invoke(targetObject, new object[] { transform });
                        }
                    }
                    else
                    {
                        Debug.LogError($"Method '{transformButton.MethodName}' not found in {targetType.Name}.");
                    }
                }

                if (GUILayout.Button("Select", GUILayout.Width(50f)))
                {
                    var tran = (Transform)property.objectReferenceValue;
                    var go = tran?.gameObject;
                    Selection.activeGameObject = go;
                }
                if (GUILayout.Button("Ping",GUILayout.Width(50f)))
                {
                    var tran = (Transform)property.objectReferenceValue;
                    var go = tran?.gameObject;
                    EditorGUIUtility.PingObject(go);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                // 如果字段类型不是 Transform，显示错误提示
                EditorGUI.LabelField(position, label.text, "Use TransformButton with Transform fields only.");
            }
        }

        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     // 增加高度以容纳按钮
        //     return EditorGUIUtility.singleLineHeight * 2 + 4; // 字段高度 + 按钮高度 + 间距
        // }
    }
    #endif
}
```



# 第三方扩展：Odin插件

[Unity使用Odin完成编辑器开发 【基础知识篇 第一节】中文 分组 颜色 按钮 条件_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1kwAkepEwp/?spm_id_from=333.1387.favlist.content.click&vd_source=563d44869c3ecebb1867233573d16b7b)

百度网盘分享的文件：[Odin Inspector and Serializer v3.3.1.4.unitypackage](https://pan.baidu.com/s/1fUa2UK6TkuL-z51in6uWbg?pwd=0000)

Odin官网：https://odininspector.com/tutorials，可以看到，自定义Attribute只是其功能之一，非常强大

