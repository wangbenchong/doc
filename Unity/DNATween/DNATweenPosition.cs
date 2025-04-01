using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>

[AddComponentMenu("DNA/Tween/DNATweenPosition")]
public class DNATweenPosition : DNATweener
{
    [HideInInspector]
    public Vector3 from;
    [HideInInspector]
    public Vector3 to;

    [HideInInspector]
    public bool worldSpace = false;

    #region PathCurve
    [HideInInspector]
    public bool useNormalCurve = false;
    [HideInInspector]
    public AnimationCurveData normalCurve;
    private float mdistance = 0;
    private Vector3 mLeftNormalVector = Vector3.zero;
    private Vector3 tempVec;
    bool needSetLeftNormalVector = true;
    #endregion

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            return worldSpace ? TransTarget.position : TransTarget.localPosition;
        }
        set
        {
            if (worldSpace) TransTarget.position = value;
            else TransTarget.localPosition = value;
        }
    }


    private Transform m_TransTarget = null;
    /// <summary>
    /// 目标Transform;
    /// </summary>
    public Transform TransTarget
    {
        get { return m_TransTarget == null ? transform : m_TransTarget; }
        set { m_TransTarget = value; }
    }

    public void SetTarget(Transform trans)
    {
        TransTarget = trans;
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (!useNormalCurve)
            value = from * (1f - factor) + to * factor;
        else
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
                SetLeftNormalVector();
                if (isFinished)
                {
                    needSetLeftNormalVector = true;
                }
#if UNITY_EDITOR
            }
            else //编辑器预览情况下每帧计算
            {
                SetLeftNormalVector(true);
            }
#endif
            tempVec = from * (1f - factor) + to * factor;
            value = mdistance * normalCurve.Curve.Evaluate(this.TweenFactor) * mLeftNormalVector + tempVec;
        }
    }
    protected override void Start()
    {
        base.Start();
        SetLeftNormalVector(true);
    }
    private void SetLeftNormalVector(bool forceSet = false)
    {
        if (!useNormalCurve)
        {
            return;
        }
        if (!needSetLeftNormalVector && !forceSet)
        {
            return;
        }
        mdistance = Vector2.Distance(from, to);
        Vector3 temp = to - from;
        mLeftNormalVector.x = -1f * temp.y;
        mLeftNormalVector.y = temp.x;
        mLeftNormalVector.Normalize();
        needSetLeftNormalVector = false;
    }
    public override void Play(bool forward)
    {
        SetLeftNormalVector(true);
        base.Play(forward);
    }

    public override void SetStartByCurrentValue()
    {
        from = value;
    }

    public override void SetEndByCurrentValue() { to = value; }
    
}
