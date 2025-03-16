using UnityEngine;

/// <summary>
/// Tween the object's rotation.
/// </summary>

[AddComponentMenu("DNA/Tween/DNATweenRotation")]
public class DNATweenRotation : DNATweener
{
    [HideInInspector]
    public Vector3 from;
    [HideInInspector]
    public Vector3 to;
    public bool quaternionLerp { get; set; }


    [System.Obsolete("Use 'value' instead")]
    public Quaternion rotation { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Quaternion value { get { return TransTarget.localRotation; } set { TransTarget.localRotation = value; } }

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
        value = quaternionLerp ? 
        Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor)
        :
        Quaternion.Euler(new Vector3(
        Mathf.Lerp(from.x, to.x, factor),
        Mathf.Lerp(from.y, to.y, factor),
        Mathf.Lerp(from.z, to.z, factor)))
        ;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartByCurrentValue() { from = value.eulerAngles; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndByCurrentValue() { to = value.eulerAngles; }
}
