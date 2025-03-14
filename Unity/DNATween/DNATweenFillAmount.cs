using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DNATweenFillAmount : DNATweener
{
    [HideInInspector]
    [Range(0f, 1f)]
    public float from = 1f;

    [HideInInspector]
    [Range(0f, 1f)]
    public float to = 1f;

    bool mCached = false;
    Image mImage = null;

    void Cache()
    {
        mCached = true;
        mImage = GetComponent<Image>();
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get
        {
            if (!mCached)
                Cache();
            return mImage.fillAmount;
        }
        set
        {
            if (!mCached)
                Cache();
            mImage.fillAmount = value;
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Mathf.Lerp(from, to, factor);
    }

    public override void SetStartByCurrentValue()
    {
        from = value;
    }
    public override void SetEndByCurrentValue()
    {
        to = value;
    }
}
