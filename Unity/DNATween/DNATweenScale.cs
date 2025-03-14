using UnityEngine;

[AddComponentMenu("DNA/Tween/DNATweenScale")]
public class DNATweenScale : DNATweener
{
    [HideInInspector]
    public Vector3 from = Vector3.one;
    [HideInInspector]
    public Vector3 to = Vector3.one;

    public Vector3 value { get { return TransTarget.localScale; } set { TransTarget.localScale = value; } }
    private Transform m_TransTarget = null;
    /// <summary>
    /// 目标Transform;
    /// </summary>
    public Transform TransTarget
    {
        get
        {
            if (m_TransTarget == null)
            {
                m_TransTarget = transform;
            }
            return m_TransTarget;
        }
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
        value = from * (1f - factor) + to * factor;
    }


    public static void Deactive(GameObject targetObj)
    {
        DeactiveTween<DNATweenScale>(targetObj);
    }

    public override void SetStartByCurrentValue() { from = value; }

    public override void SetEndByCurrentValue() { to = value; }
}
