using UnityEngine;
using System.Collections;

public class DNATweenRectTransformPosition : DNATweener
{
    [HideInInspector]
    public Vector3 from;
    [HideInInspector]
    public Vector3 to;

    RectTransform mTrans;

    public RectTransform cachedTransform { get { if (mTrans == null) mTrans = transform as RectTransform; return mTrans; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            if (null == cachedTransform)
                return Vector3.one;
            return cachedTransform.anchoredPosition3D;
        }
        set
        {
            cachedTransform.anchoredPosition3D = value;
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }


    [ContextMenu("Set 'From' to current value")]
    public override void SetStartByCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndByCurrentValue() { to = value; }
}
