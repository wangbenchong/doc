using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenColor))]
public class DNATweenColorEditor : DNATweenerEditor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenColor tw = target as DNATweenColor;
        GUI.changed = false;

        Color from = EditorGUILayout.ColorField("From", tw.from);
        Color to = EditorGUILayout.ColorField("To", tw.to);

        if (GUI.changed)
        {
            DNAEditorTools.RegisterUndo("Tween Change", tw);
            tw.from = from;
            tw.to = to;
            DNAEditorTools.SetDirty(tw);
        }

        DrawCommonProperties();
    }
}
