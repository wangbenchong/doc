using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenScale))]
public class DNATweenScaleEditor : DNATweenerEditor
{
    protected override void BeforeDrawCommonProperties()
    {
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenScale tw = target as DNATweenScale;
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
