using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DNATweener), true)]
public class DNATweenerEditor : DNABaseEditor
{
    /// <summary>
    /// 子类通常不应重写该方法，而是在BeforeDrawCommonProperties中扩展绘制内容。
    /// </summary>
    protected override void AfterOnInspectorGUI()
    {
        BeforeDrawCommonProperties();
        DrawCommonProperties();
    }
    protected virtual void BeforeDrawCommonProperties()
    {
        DNAEditorTools.SetLabelWidth(110f);
        // 留给子类扩展
    }

    DNATweener tw;
    SerializedProperty curveData;

    protected override void OnAfterEnable()
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
        if (GUILayout.Button("初始状态", GUILayout.MinWidth(50)))
        {
            tw.ResetToBeginEditor();
        }
        if (GUILayout.Button("结束状态", GUILayout.MinWidth(50)))
        {
            tw.ResetToEndEditor();
        }
        EditorGUILayout.EndHorizontal();

        // 设置当前的tween的位置;
        GUI.changed = false;
        EditorGUILayout.BeginHorizontal();
        bool isPlaying = tw.IsEditorUpdate || (EditorApplication.isPlaying && tw.enabled);
        GUILayout.Label("预览",GUILayout.Width(30));
        GUI.enabled = !isPlaying;
        ScrollValue = EditorGUILayout.Slider(tw.TweenFactor, 0, 1);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        if(!isPlaying)
        {
            if (GUI.changed)
            {
                tw.RemoveUpdateEditor();
                tw.ToggleForce(true);
                tw.Sample(ScrollValue, false);
            }
        }
        else
        {
            //下面这两句可以让Inspector强制逐帧刷新，否则预览进度条只在鼠标晃到上面的时候才会刷新，导致进度条不流畅。
            EditorApplication.QueuePlayerLoopUpdate();
            Repaint();
        }
        GUI.changed = false;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("设置初始状态为当前"))
        {
            tw.SetStartByCurrentValue();
        }
        if (GUILayout.Button("设置结束状态为当前"))
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
            DNATweener.UpdateType updateType = (DNATweener.UpdateType)EditorGUILayout.EnumPopup("Update Type", tw.updateType);

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
            GUIContent durationContent = new GUIContent("Duration", "执行动画总时长（不含延迟时间）");
            float dur = EditorGUILayout.FloatField(durationContent, tw.duration, GUILayout.Width(170f));
            GUILayout.Label("seconds");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUIContent delayContent = new GUIContent("Start Delay", "执行前的延迟时间，仅用于正向播放");
            float del = EditorGUILayout.FloatField(delayContent, tw.delay, GUILayout.Width(170f));
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
                tw.updateType = updateType;
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
