using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenFOV))]
public class DNATweenFOVEditor : DNATweenerEditor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenFOV tw = target as DNATweenFOV;
        GUI.changed = false;

        float from = EditorGUILayout.Slider("From", tw.from, 1f, 180f);
        float to = EditorGUILayout.Slider("To", tw.to, 1f, 180f);

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
