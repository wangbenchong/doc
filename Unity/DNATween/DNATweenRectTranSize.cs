using UnityEngine;

public class DNATweenRectTranSize : DNATweener
{
    [HideInInspector]
    public RectTransform from;
    [HideInInspector]
    public RectTransform to;
    [HideInInspector]
    public bool UseTransform = true;
    #region 通过数值的from、to来控制动画
    [HideInInspector]
    public float f_from;
    [HideInInspector]
    public float f_to;
    [HideInInspector]
    public RectTransform.Axis axis = RectTransform.Axis.Horizontal;
    #endregion
    [HideInInspector]
    public RectTransform mTrans;
    public RectTransform cachedTransform { get { if (mTrans == null) mTrans = transform.GetComponent<RectTransform>(); return mTrans; } }
    public Vector2 value
    {
        get
        {
            return new Vector2(cachedTransform.rect.width, cachedTransform.rect.height);
        }
        set
        {
            if (UseTransform)
            {
                cachedTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value.x);
                cachedTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value.y);
            }
        }
    }

    public float f_value
    {
        get
        {
            if (axis == RectTransform.Axis.Horizontal)
            {
                return cachedTransform.rect.width;
            }
            else
            {
                return cachedTransform.rect.height;
            }
        }
        set
        {
            cachedTransform.SetSizeWithCurrentAnchors(axis, value);
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (UseTransform)
        {
            Vector2 fromVec = new Vector2(from.rect.width, from.rect.height);
            Vector2 toVec = new Vector2(to.rect.width, to.rect.height);
            value = fromVec * (1f - factor) + toVec * factor;
        }
        else
        {
            f_value = f_from * (1f - factor) + f_to * factor;
        }
    }
}
