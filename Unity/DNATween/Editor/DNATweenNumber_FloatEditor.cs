
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenNumber_Float))]
public class DNATweenNumber_FloatEditor : DNATweenerEditor
{
    public override void OnInspectorGUI()
    {
        DNATweenNumber_Float tw = target as DNATweenNumber_Float;

        tw.from = EditorGUILayout.FloatField("From", tw.from);
        tw.to = EditorGUILayout.FloatField("To", tw.to);
        tw.InputFormat = EditorGUILayout.TextField("InputFormat", tw.InputFormat);
        tw.IsStringUS = EditorGUILayout.Toggle("IsStringUS", tw.IsStringUS);
        tw.DotNum = EditorGUILayout.IntField("DotNum", tw.DotNum);
        tw.RoundToInt = EditorGUILayout.Toggle("RoundToInt", tw.RoundToInt);
     
        DrawCommonProperties();
    }
}
