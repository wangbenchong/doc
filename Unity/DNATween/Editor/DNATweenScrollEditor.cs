using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenScroll))]
public class DNATweenScrollEditor : DNATweenerEditor
{
    protected override void BeforeDrawCommonProperties()
    {
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenScroll tw = target as DNATweenScroll;
        GUI.changed = false;

        float fromX = EditorGUILayout.Slider("fromX", tw.fromX, 0f, 1f);
        float toX = EditorGUILayout.Slider("toX", tw.toX, 0f, 1f);
        float fromY = EditorGUILayout.Slider("fromY", tw.fromY, 0f, 1f);
        float toY = EditorGUILayout.Slider("toY", tw.toY, 0f, 1f);

        if (GUI.changed)
        {
            DNAEditorTools.RegisterUndo("Tween Change", tw);
            tw.fromX = fromX;
            tw.toX = toX;
            tw.fromY = fromY;
            tw.toY = toY;
            DNAEditorTools.SetDirty(tw);
        }
    }
}