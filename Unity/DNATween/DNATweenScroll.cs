using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the object's alpha. Works with both UI widgets as well as renderers.
/// </summary>

[AddComponentMenu("DNA/Tween/DNATweenScroll")]
public class DNATweenScroll : DNATweener
{
    [HideInInspector]
    public float fromX = 1f;
    [HideInInspector]
    public float toX = 1f;
    [HideInInspector]
    public float fromY = 1f;
    [HideInInspector]
    public float toY = 1f;

    bool mCached = false;
    ScrollRect mScrollRect;

    void Cache()
    {
        mCached = true;
        mScrollRect = GetComponent<ScrollRect>();
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float ScrollVectorX
    {
        get
        {
            float horizontalNormalizedPosition = 0;
            if (!mCached)
                Cache();
            if (mScrollRect != null)
            {
                horizontalNormalizedPosition = mScrollRect.horizontalNormalizedPosition;
            }
            return horizontalNormalizedPosition;
        }
        set
        {
            if (!mCached)
                Cache();
            //if (value == null)
            //{
            //    LogHelper.LogError("value is " + value);
            //}

            if (mScrollRect != null)
            {
                mScrollRect.horizontalNormalizedPosition = value;
            }
        }
    }

    public float ScrollVectorY
    {
        get
        {
            float verticalNormalizedPosition = 0;
            if (!mCached)
                Cache();
            if (mScrollRect != null)
            {
                verticalNormalizedPosition = mScrollRect.verticalNormalizedPosition;
            }
            return verticalNormalizedPosition;
        }
        set
        {
            if (!mCached)
                Cache();
            //if (value == null)
            //{
            //    LogHelper.LogError("value is " + value);
            //}

            if (mScrollRect != null)
            {
                mScrollRect.verticalNormalizedPosition = value;
            }
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        ScrollVectorX = Mathf.Lerp(fromX, toX, factor);
        ScrollVectorY = Mathf.Lerp(fromY, toY, factor);
    }

    public static void Deactive(GameObject targetObj)
    {
        DeactiveTween<DNATweenScroll>(targetObj);
    }

    public override void SetStartByCurrentValue()
    {
        fromX = ScrollVectorX;
        fromY = ScrollVectorY;
    }
    public override void SetEndByCurrentValue()
    {
        toX = ScrollVectorX;
        toY = ScrollVectorY;
    }
}
