using UnityEngine;

public class DNABaseMono : MonoBehaviour
{
    /* 子类继承范例
    #if UNITY_EDITOR
    public override string DEFINE_SYMBOL =>
        #if CINEMACHINE
        string.Empty;
        #else
        "CINEMACHINE";
        #endif
    public override string DEFINE_DEPEND_PACKAGE => "Cinemachine";
    #endif
    */
    #if UNITY_EDITOR
    /// <summary>
    /// 当返回非空字符串时，如果没有开启这个宏会有文字提示用户开启宏。
    /// 子类如果覆写，需按宏分支返回不同值。
    /// </summary>
    public virtual string DEFINE_SYMBOL => string.Empty;
    /// <summary>
    /// 依赖于插件的名字。
    /// </summary>
    public virtual string DEFINE_DEPEND_PACKAGE => "某插件名";
    #endif
}
