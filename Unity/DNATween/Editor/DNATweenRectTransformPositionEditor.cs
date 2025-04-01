using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenRectTransformPosition))]
public class DNATweenRectTransformPositionEditor : DNATweenerEditor
{
    protected override void BeforeDrawCommonProperties()
    {
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenRectTransformPosition tw = target as DNATweenRectTransformPosition;

        tw.from = EditorGUILayout.Vector3Field("From", tw.from);
        tw.to = EditorGUILayout.Vector3Field("To", tw.to);
    }
}