using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DNATweenRectTranSize))]
public class DNATweenRectTranSizeEditor : DNATweenerEditor
{
    RectTransform tempTran;
    protected override void BeforeDrawCommonProperties()
    {
        RectTransform from = null, to = null;
        bool UseTransform;
        float f_from = 0f, f_to = 0f;
        RectTransform.Axis axis = RectTransform.Axis.Horizontal;

        DNAEditorTools.SetLabelWidth(120f);

        DNATweenRectTranSize tw = target as DNATweenRectTranSize;
        GUI.changed = false;

        UseTransform = EditorGUILayout.Toggle("Use Transform", tw.UseTransform);
        if (tw.UseTransform)
        {
            from = (RectTransform)EditorGUILayout.ObjectField("From", tw.from, typeof(RectTransform), true);
            to = (RectTransform)EditorGUILayout.ObjectField("To", tw.to, typeof(RectTransform), true);
        }
        else
        {
            f_from = EditorGUILayout.FloatField("From", tw.f_from);
            f_to = EditorGUILayout.FloatField("To", tw.f_to);
            axis = (RectTransform.Axis)EditorGUILayout.EnumPopup("Axis", tw.axis);
        }
        tempTran = (RectTransform)EditorGUILayout.ObjectField("Transform", tw.mTrans, typeof(RectTransform), true);

        if (GUI.changed)
        {
            DNAEditorTools.RegisterUndo("Tween Change", tw);
            tw.UseTransform = UseTransform;
            if (UseTransform)
            {
                tw.from = from;
                tw.to = to;
            }
            else
            {
                tw.f_from = f_from;
                tw.f_to = f_to;
                tw.axis = axis;
            }
            tw.mTrans = tempTran;
            DNAEditorTools.SetDirty(tw);
        }
    }
}
