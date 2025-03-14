using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenTransform))]
public class DNATweenTransformEditor : DNATweenerEditor
{
    SerializedProperty from;
    SerializedProperty to;
    SerializedProperty parentWhenFinished;
    public override void OnInspectorGUI()
    {
        if (null == from)
            from = serializedObject.FindProperty("from");
        if (null == to)
            to = serializedObject.FindProperty("to");
        if (null == parentWhenFinished)
            parentWhenFinished = serializedObject.FindProperty("parentWhenFinished");

        GUILayout.Space(6f);
        DNAEditorTools.SetLabelWidth(130f);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        DNAEditorTools.ShowPropertyField(from);
        DNAEditorTools.ShowPropertyField(to);
        DNAEditorTools.ShowPropertyField(parentWhenFinished);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        DrawCommonProperties();
    }
}
