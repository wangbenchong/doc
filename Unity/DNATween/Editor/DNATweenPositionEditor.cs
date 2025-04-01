using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweenPosition))]
public class DNATweenPositionEditor : DNATweenerEditor
{
    protected override void BeforeDrawCommonProperties()
    {
        DNAEditorTools.SetLabelWidth(120f);

        DNATweenPosition tw = target as DNATweenPosition;
        GUI.changed = false;

        Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
        Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);
        bool useNormalCurve = EditorGUILayout.Toggle("Use Normal Curve", tw.useNormalCurve);
        AnimationCurveData curve = null;
        if (useNormalCurve)
            curve = EditorGUILayout.ObjectField("CurveData: ", tw.normalCurve, typeof(AnimationCurveData), true) as AnimationCurveData;
        bool isWorld = EditorGUILayout.Toggle("WorldSpace", tw.worldSpace);
        if (GUI.changed)
        {
            DNAEditorTools.RegisterUndo("Tween Change", tw);
            tw.from = from;
            tw.to = to;
            tw.useNormalCurve = useNormalCurve;
            if (useNormalCurve)
                tw.normalCurve = curve;
            tw.worldSpace = isWorld;
            DNAEditorTools.SetDirty(tw);
        }
    }
}
