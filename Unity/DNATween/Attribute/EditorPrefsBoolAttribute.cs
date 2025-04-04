using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorPrefsBoolAttribute : PropertyAttribute
{
    public string editorPrefsKey;
    public string label;
    
    public EditorPrefsBoolAttribute(string editorPrefsKey, string label)
    {
        this.editorPrefsKey = editorPrefsKey;
        this.label = label;
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EditorPrefsBoolAttribute))]
public class EditorPrefsBoolAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorPrefsBoolAttribute attr = (EditorPrefsBoolAttribute)attribute;
        bool currentValue = EditorPrefs.GetBool(attr.editorPrefsKey, true);
        EditorGUI.BeginChangeCheck();
        bool newValue = 
        EditorGUI.Toggle(position, attr.label, currentValue);
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetBool(attr.editorPrefsKey, newValue);
            // Force scene views to repaint
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }
}
#endif