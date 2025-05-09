﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenRotation))]
public class DNATweenRotationEditor : DNATweenerEditor
{
    protected override void BeforeDrawCommonProperties()
    {
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenRotation tw = target as DNATweenRotation;
        GUI.changed = false;

        Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
        Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);

        if (GUI.changed)
        {
            DNAEditorTools.RegisterUndo("Tween Change", tw);
            tw.from = from;
            tw.to = to;
            DNAEditorTools.SetDirty(tw);
        }
    }
}
