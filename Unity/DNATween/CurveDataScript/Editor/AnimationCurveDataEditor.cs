using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AnimationCurveData)), CanEditMultipleObjects]
public class AnimationCurveDataEditor : Editor
{
    AnimationCurveData _target = null;
    AnimationCurve curve = new AnimationCurve();
    void OnEnable()
    {
        _target = target as AnimationCurveData;
    }
    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 50f;
        curve = EditorGUILayout.CurveField("Curve:", _target.Curve, GUILayout.Width(230f), GUILayout.Height(180f));
        if (GUI.changed)
        {
            _target.Curve = curve;
            EditorUtility.SetDirty(target);
        }
        if(GUILayout.Button("Save"))
        {
            AssetDatabase.SaveAssets();
        }
    }
}
