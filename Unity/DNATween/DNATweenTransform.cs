using UnityEngine;

[AddComponentMenu("DNA/Tween/DNATweenTransform")]
public class DNATweenTransform : DNATweener
{
    [HideInInspector]
    public Transform from;
    [HideInInspector]
    public Transform to;
    [HideInInspector]
    public bool parentWhenFinished = false;

    Transform mTrans;
    Vector3 mPos;
    Quaternion mRot;
    Vector3 mScale;

    /// <summary>
    /// Interpolate the position, scale, and rotation.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (to != null)
        {
            if (mTrans == null)
            {
                mTrans = transform;
                mPos = mTrans.position;
                mRot = mTrans.rotation;
                mScale = mTrans.localScale;
            }
            // Change the parent when finished, if requested
            if (parentWhenFinished && isFinished) mTrans.SetParent(to);
            if (from != null)
            {
                mTrans.position = from.position * (1f - factor) + to.position * factor;
                mTrans.localScale = from.localScale * (1f - factor) + to.localScale * factor;
                mTrans.rotation = Quaternion.Slerp(from.rotation, to.rotation, factor);
            }
            else
            {
                mTrans.position = mPos * (1f - factor) + to.position * factor;
                mTrans.localScale = mScale * (1f - factor) + to.localScale * factor;
                mTrans.rotation = Quaternion.Slerp(mRot, to.rotation, factor);
            }
        }
    }
}
