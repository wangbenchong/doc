using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenFillAmount))]
public class DNATweenFillAmountEditor : DNATweenerEditor
{
    protected override void BeforeDrawCommonProperties()
    {
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenFillAmount tw = target as DNATweenFillAmount;
        GUI.changed = false;

        float from = EditorGUILayout.Slider("From", tw.from, 0f, 1f);
        float to = EditorGUILayout.Slider("To", tw.to, 0f, 1f);

        if (GUI.changed)
        {
            DNAEditorTools.RegisterUndo("Tween Change", tw);
            tw.from = from;
            tw.to = to;
            DNAEditorTools.SetDirty(tw);
        }
    }
}
