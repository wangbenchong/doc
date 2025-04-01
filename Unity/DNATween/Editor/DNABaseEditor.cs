using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DNABaseMono),true)]

public class DNABaseEditor : Editor
{
    private DNABaseMono mono;
    void OnEnable()
    {
        mono = target as DNABaseMono;
        OnAfterEnable();
    }
    protected virtual void OnAfterEnable(){}
    /// <summary>
    /// 子类不应重写此方法，而是在AfterOnInspectorGUI中实现自定义逻辑。
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        if(!string.IsNullOrEmpty(mono.DEFINE_SYMBOL))
        {
            EditorGUILayout.TextArea($"⚠️该脚本当前被禁用（未编译）！！！\n依赖的插件：{mono.DEFINE_DEPEND_PACKAGE}\n请确保安装插件后，手动开启宏：\n{mono.DEFINE_SYMBOL}\n🚨在此之前请勿保存此组件，否则数据可能丢失！");
        }
        AfterOnInspectorGUI();
    }
    protected virtual void AfterOnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}