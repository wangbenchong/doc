﻿
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenNumber))]
public class DNATweenNumberEditor : DNATweenerEditor
{
    protected override void BeforeDrawCommonProperties()
    {
        DNATweenNumber tw = target as DNATweenNumber;

        tw.from = EditorGUILayout.LongField("From", tw.from);
        tw.to = EditorGUILayout.LongField("To", tw.to);
        tw.InputFormat = EditorGUILayout.TextField("InputFormat", tw.InputFormat);
        tw.IsStringUS = EditorGUILayout.Toggle("IsStringUS", tw.IsStringUS);
    }
}
