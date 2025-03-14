using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DNATweenTweener))]
public class DNATweenTweenerEditor : DNATweenerEditor
{
    DNATweenTweener tt = null;

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            if (tt == null)
                tt = target as DNATweenTweener;

            if (GUILayout.Button("播放所有动画"))
                tt.PlayAll();
        }
        DrawCommonProperties();
    }
}
