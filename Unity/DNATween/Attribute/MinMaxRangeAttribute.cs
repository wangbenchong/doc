using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MinMaxRangeAttribute : PropertyAttribute
{
    public readonly float Min;
    public readonly float Max;

    public MinMaxRangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeAttributeDrawer : PropertyDrawer
{
    private const float TextFieldWidth = 45f;
    private const float Spacing = 5f;
    private const float SliderPadding = 2f; // 防止滑块与输入框紧贴

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 验证属性类型
        if (property.propertyType != SerializedPropertyType.Vector2)
        {
            EditorGUI.HelpBox(position, $"{label.text} 必须使用 Vector2 类型", MessageType.Error);
            return;
        }

        MinMaxRangeAttribute range = (MinMaxRangeAttribute)attribute;
        
        // 获取属性值
        Vector2 value = property.vector2Value;
        float minValue = Mathf.Clamp(value.x, range.Min, range.Max);
        float maxValue = Mathf.Clamp(value.y, range.Min, range.Max);
        
        // 计算布局
        position = EditorGUI.PrefixLabel(position, label);
        
        Rect minRect = new Rect(position) { 
            width = TextFieldWidth 
        };
        
        Rect maxRect = new Rect(position) { 
            x = position.xMax - TextFieldWidth,
            width = TextFieldWidth 
        };
        
        Rect sliderRect = new Rect(position) {
            x = minRect.xMax + Spacing,
            xMax = maxRect.x - Spacing,
            y = position.y + SliderPadding,
            height = position.height - SliderPadding * 2
        };

        EditorGUI.BeginProperty(position, label, property);
        
        // 绘制最小值和最大值输入框
        EditorGUI.BeginChangeCheck();
        minValue = EditorGUI.FloatField(minRect, minValue);
        if (EditorGUI.EndChangeCheck())
        {
            minValue = Mathf.Clamp(minValue, range.Min, maxValue);
            property.vector2Value = new Vector2(minValue, maxValue);
        }

        // 绘制范围滑块
        EditorGUI.BeginChangeCheck();
        EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, range.Min, range.Max);
        if (EditorGUI.EndChangeCheck())
        {
            property.vector2Value = new Vector2(minValue, maxValue);
        }

        // 绘制最大值输入框
        EditorGUI.BeginChangeCheck();
        maxValue = EditorGUI.FloatField(maxRect, maxValue);
        if (EditorGUI.EndChangeCheck())
        {
            maxValue = Mathf.Clamp(maxValue, minValue, range.Max);
            property.vector2Value = new Vector2(minValue, maxValue);
        }

        EditorGUI.EndProperty();
    }
}
#endif