using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenOrthoSize))]
public class DNATweenOrthoSizeEditor : DNATweenerEditor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenOrthoSize tw = target as DNATweenOrthoSize;
        GUI.changed = false;

        float from = EditorGUILayout.FloatField("From", tw.from);
        float to = EditorGUILayout.FloatField("To", tw.to);

        if (from < 0f) from = 0f;
        if (to < 0f) to = 0f;

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
