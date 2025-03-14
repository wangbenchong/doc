using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweener), true)]
public class DNATweenerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        DNAEditorTools.SetLabelWidth(110f);
        DrawCommonProperties();
    }

    DNATweener tw;
    SerializedProperty curveData;

    void OnEnable()
    {
        tw = target as DNATweener;
        curveData = serializedObject.FindProperty("curveData");
        OnDisable();
    }

    public void OnDisable()
    {
        if (tw != null)
        {
            if (tw.IsEditorUpdate)
            {
                tw.ResetToBeginEditor();
            }
        }
    }

    private float ScrollValue = 0f;
    protected void DrawCommonProperties(bool isDrawDefaultInspector = true)
    {
        // 添加编辑器下的播放功能;
        GUI.changed = false;
        EditorGUILayout.BeginHorizontal();
        if (!EditorApplication.isPlaying)
        {
            if (!tw.IsEditorUpdate)
            {
                if (GUILayout.Button("正向播放(非运行)", GUILayout.MinWidth(65)))
                {
                    tw.PlayEditor(true);
                }
                if (GUILayout.Button("反向播放(非运行)", GUILayout.MinWidth(65)))
                {
                    tw.PlayEditor(false);
                }
            }
            else
            {
                if (GUILayout.Button("停止（非运行）"))
                {
                    tw.ResetToBeginEditor();
                    ScrollValue = 0f;
                }
            }
        }
        else
        {
            if (GUILayout.Button("正向播放", GUILayout.MinWidth(50)))
            {
                tw.PlayForwardForce();
            }
            if (GUILayout.Button("反向播放", GUILayout.MinWidth(50)))
            {
                tw.PlayReverseForce();
            }
        }
        if (GUILayout.Button("初始位置", GUILayout.MinWidth(50)))
        {
            tw.ResetToBeginEditor();
            ScrollValue = 0f;
        }
        if (GUILayout.Button("结束位置", GUILayout.MinWidth(50)))
        {
            tw.ResetToEndEditor();
            ScrollValue = 1f;
        }
        EditorGUILayout.EndHorizontal();

        // 设置当前的tween的位置;
        GUI.changed = false;
        EditorGUILayout.BeginHorizontal();
        bool isPlaying = tw.IsEditorUpdate || (EditorApplication.isPlaying && tw.enabled);
        GUILayout.Label("预览",GUILayout.Width(30));
        GUI.enabled = !isPlaying;
        ScrollValue = EditorGUILayout.Slider(ScrollValue, 0, 1);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        if(!isPlaying)
        {
            if (GUI.changed)
            {
                tw.RemoveUpdateEditor();
                tw.ToggleByBool(true);
                tw.Sample(ScrollValue, false);
            }
        }
        else
        {
            ScrollValue = tw.TweenFactor;
            //下面这两句可以让Inspector强制逐帧刷新，否则预览进度条只在鼠标晃到上面的时候才会刷新，导致进度条不流畅。
            EditorApplication.QueuePlayerLoopUpdate();
            Repaint();
        }
        GUI.changed = false;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("设置From值为当前"))
        {
            tw.SetStartByCurrentValue();
        }
        if (GUILayout.Button("设置To值为当前"))
        {
            tw.SetEndByCurrentValue();
        }
        EditorGUILayout.EndHorizontal();
        if (DNAEditorTools.DrawHeader("Tweener"))
        {
            DNAEditorTools.BeginContents();
            DNAEditorTools.SetLabelWidth(110f);

            GUI.changed = false;

            DNATweener.Style style = (DNATweener.Style)EditorGUILayout.EnumPopup("Play Style", tw.style);

            DNATweener.ECurveType curveType = (DNATweener.ECurveType)EditorGUILayout.EnumPopup("Curve Type", tw.curveType);

            if (curveType == DNATweener.ECurveType.Custom)
            {
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(curveData, true);
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    tw._CreateUIAnimationCurveData();
                }
                if (GUILayout.Button("@", GUILayout.Width(20)))
                {
                    tw._SelectCurveFolder();
                }
                EditorGUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            GUILayout.BeginHorizontal();

            float dur = EditorGUILayout.FloatField("Duration", tw.duration, GUILayout.Width(170f));
            GUILayout.Label("seconds");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            float del = EditorGUILayout.FloatField("Start Delay", tw.delay, GUILayout.Width(170f));
            GUILayout.Label("seconds");
            GUILayout.EndHorizontal();

            int tg = EditorGUILayout.IntField("Tween Group", tw.tweenGroup, GUILayout.Width(170f));
            GUILayout.BeginHorizontal();
            bool ts = EditorGUILayout.Toggle("Ignore TimeScale", tw.ignoreTimeScale, GUILayout.Width(170f));
            GUILayout.EndHorizontal();
            if (GUI.changed)
            {
                DNAEditorTools.RegisterUndo("Tween Change", tw);
                tw.curveType = curveType;
                tw.style = style;
                tw.ignoreTimeScale = ts;
                tw.tweenGroup = tg;
                tw.duration = dur;
                tw.delay = del;
                DNAEditorTools.SetDirty(tw);
            }
            DNAEditorTools.EndContents();
        }

        DNAEditorTools.SetLabelWidth(130f);

        if (isDrawDefaultInspector)
        {
            if (DNAEditorTools.DrawHeader("Other Default"))
            {
                DNAEditorTools.BeginContents();
                int originalIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;//默认绘制如果需要增加缩进，只能通过这种方式
                DrawDefaultInspector();
                EditorGUI.indentLevel = originalIndentLevel;    
                DNAEditorTools.EndContents();
            }
        }
    }
}
