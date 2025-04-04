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

