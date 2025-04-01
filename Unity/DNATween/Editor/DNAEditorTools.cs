using UnityEditor;
using UnityEngine;

/// <summary>
/// Tools for the editor
/// </summary>

public static class DNAEditorTools
{
    static bool minimalisticLook = false;
    /// <summary>
    /// Unity 4.3 changed the way LookLikeControls works.
    /// </summary>

    public static void SetLabelWidth(float width)
    {
        EditorGUIUtility.labelWidth = width;
    }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text) { return DrawHeader(text, text, false, minimalisticLook); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text, string key) { return DrawHeader(text, key, false, minimalisticLook); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text, bool detailed) { return DrawHeader(text, text, detailed, !detailed); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
    {
        bool state = EditorPrefs.GetBool(key, true);

        if (!minimalistic) GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUI.changed = false;

        if (minimalistic)
        {
            if (state) text = "\u25BC" + (char)0x200a + text;
            else text = "\u25BA" + (char)0x200a + text;

            GUILayout.BeginHorizontal();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        else
        {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
        }

        if (GUI.changed) EditorPrefs.SetBool(key, state);

        if (!minimalistic) GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    public static void BeginContents() { BeginContents(minimalisticLook); }

    static bool mEndHorizontal = false;

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    public static void BeginContents(bool minimalistic)
    {
        if (!minimalistic)
        {
            mEndHorizontal = true;
            GUILayout.BeginHorizontal();
            #if UNITY_2017_1_OR_NEWER
            EditorGUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(10f));
            #else
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
            #endif
        }
        else
        {
            mEndHorizontal = false;
            EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
            GUILayout.Space(10f);
        }
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }

    /// <summary>
    /// Create an undo point for the specified objects.
    /// </summary>

    public static void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            UnityEditor.Undo.RecordObjects(objects, name);

            foreach (Object obj in objects)
            {
                if (obj == null) continue;
                EditorUtility.SetDirty(obj);
            }
        }
    }

    /// <summary>
    /// End drawing the content area.
    /// </summary>

    public static void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (mEndHorizontal)
        {
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(3f);
    }

    public static void ShowPropertyField(SerializedProperty property)
    {
        if (null != property)
            EditorGUILayout.PropertyField(property, true);
    }

    /// <summary>
    /// Convenience function that marks the specified object as dirty in the Unity Editor.
    /// </summary>

    public static void SetDirty(UnityEngine.Object obj)
    {
        if (obj)
        {
            EditorUtility.SetDirty(obj);
        }
    }

    [MenuItem("DNATools/Copy Transform to Scene View Camera")]// %#c")] // Ctrl + Shift + C
    private static void CopyTransformToCamera()
    {
        // 获取当前选中的物体
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            // 获取场景视图的相机
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                // 将选中物体的位置和旋转赋值给场景相机
                sceneView.pivot = selectedObject.transform.position;
                sceneView.rotation = selectedObject.transform.rotation;
                sceneView.Repaint(); // 刷新场景视图
            }
            else
            {
                EditorUtility.DisplayDialog("Error","No active Scene View found.","OK");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Error","No object selected.","OK");
        }
    }
}
