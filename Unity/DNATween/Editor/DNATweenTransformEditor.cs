using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenTransform))]
public class DNATweenTransformEditor : DNATweenerEditor
{
    SerializedProperty from;
    SerializedProperty to;
    SerializedProperty parentWhenFinished;
    protected override void BeforeDrawCommonProperties()
    {
        if (null == from)
            from = serializedObject.FindProperty("from");
        if (null == to)
            to = serializedObject.FindProperty("to");
        if (null == parentWhenFinished)
            parentWhenFinished = serializedObject.FindProperty("parentWhenFinished");

        DNAEditorTools.SetLabelWidth(130f);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        DNAEditorTools.ShowPropertyField(from);
        DNAEditorTools.ShowPropertyField(to);
        DNAEditorTools.ShowPropertyField(parentWhenFinished);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
