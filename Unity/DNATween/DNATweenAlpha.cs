using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("DNA/Tween/DNATweenAlpha")]
public class DNATweenAlpha : DNATweener
{
    [HideInInspector]
    public float from = 1f;
    [HideInInspector]
    public float to = 1f;

    bool mCached = false;
    Material mMat;
    Renderer mRen;
    MaterialPropertyBlock mBlock;
    SpriteRenderer mSr;
    CanvasGroup canvasGroup;
    Graphic mGraphic;
    bool b_MatNotNull = false;
    bool b_SpriteRenderNotNull = false;
    bool b_CanvasGroupNotNull = false;
    bool b_GraphicNotNull = false;

    [System.Obsolete("Use 'value' instead")]
    public float alpha
    {
        get
        {
            return this.Value;
        }
        set
        {
            this.Value = value;
        }
    }

    bool bUseCachedColor = false;
    Color cachedColor;

    /// <summary>
    /// 将材质属性用ID存储下来
    /// </summary>
    private static bool bInit = false;
    private static int ms_TintColorProp;
    //private static int ms_CircleProp;

    public void SetColor(Color c, bool bUseCachedColor)
    {
        this.bUseCachedColor = bUseCachedColor;
        if (!mCached)
        {
            Cache();
        }

        if (mMat.HasProperty(ms_TintColorProp))
        {
            if (bUseCachedColor)
            {
                cachedColor = c;
            }
            mBlock.SetColor(ms_TintColorProp, c);
            mRen.SetPropertyBlock(mBlock);
        }
    }

    private void InitMatProperty()
    {
        if (bInit)
            return;
        bInit = true;

        ms_TintColorProp = Shader.PropertyToID("_TintColor");
    }

    public void Cache()
    {
        InitMatProperty();

        b_MatNotNull = false;
        b_SpriteRenderNotNull = false;
        b_CanvasGroupNotNull = false;
        b_GraphicNotNull = false;
        mCached = true;
        canvasGroup = TransTarget.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            b_CanvasGroupNotNull = true;
            return;
        }
        mGraphic = TransTarget.GetComponent<Graphic>();
        if (mGraphic != null)
        {
            b_GraphicNotNull = true;
            return;
        }
        mSr = TransTarget.GetComponent<SpriteRenderer>();
        if (mSr != null)
        {
            b_SpriteRenderNotNull = true;
            return;
        }

        mRen = TransTarget.GetComponent<Renderer>();
        if (mRen != null)
        {
            mMat = mRen.sharedMaterial;
            mBlock = new MaterialPropertyBlock();
            mRen.GetPropertyBlock(mBlock);
            b_MatNotNull = true;

            if (bUseCachedColor && mMat.HasProperty(ms_TintColorProp))
            {
                cachedColor = mMat.GetColor(ms_TintColorProp);
            }
        }
    }

    private Transform m_TransTarget = null;
    /// <summary>
    /// 目标Transform;
    /// </summary>
    public Transform TransTarget
    {
        get { return m_TransTarget == null ? this.transform : m_TransTarget; }
        set
        {
            m_TransTarget = value;
            Cache();
        }
    }

    public void SetTarget(Transform trans)
    {
        TransTarget = trans;
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float Value
    {
        get
        {
            if (!mCached)
                Cache();
            if (b_CanvasGroupNotNull)
                return canvasGroup.alpha;
            if (b_GraphicNotNull)
                return mGraphic.color.a;
            if (b_SpriteRenderNotNull)
                return mSr.color.a;

            if (b_MatNotNull)
            {
                if (mMat.HasProperty(ms_TintColorProp))
                    return mMat.GetColor(ms_TintColorProp).a;
                else
                    return mMat.color.a;

            }
            else
            {
                return 1f;
            }
        }
        set
        {
            if (!mCached)
                Cache();
            if (b_CanvasGroupNotNull)
            {
                canvasGroup.alpha = value;
            }
            else if (b_GraphicNotNull)
            {
                Color c = mGraphic.color;
                c.a = value;
                mGraphic.color = c;
            }
            else if (b_SpriteRenderNotNull)
            {
                Color c = mSr.color;
                c.a = value;
                mSr.color = c;
            }
            else if (b_MatNotNull)
            {
                if (mMat.HasProperty(ms_TintColorProp))
                {
                    if (bUseCachedColor)
                    {
                        cachedColor.a = value;
                        mBlock.SetColor(ms_TintColorProp, cachedColor);
                    }
                    else
                    {
                        Color c = mMat.GetColor(ms_TintColorProp);
                        c.a = value;
                        mBlock.SetColor(ms_TintColorProp, c);
                    }
                    mRen.SetPropertyBlock(mBlock);
                }
                else
                {
                    Color c = mMat.color;
                    c.a = value;
                    mMat.color = c;
                }
            }
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        Value = Mathf.Lerp(from, to, factor);
    }

    public static DNATweenAlpha Tween(GameObject targetObj, float toAlpha, Style eLoopType, float fDuration, System.Action cbFinish, bool reuse = false)
    {
        DNATweenAlpha tweenAlpha = CreateTween<DNATweenAlpha>(targetObj);
        tweenAlpha.from = tweenAlpha.Value;
        tweenAlpha.to = toAlpha;
        tweenAlpha.DoTween(eLoopType, fDuration, cbFinish, reuse);

        return tweenAlpha;
    }

    public static void Deactive(GameObject targetObj)
    {
        DeactiveTween<DNATweenAlpha>(targetObj);
    }

    public override void SetStartByCurrentValue()
    {
        from = Value;
    }
    public override void SetEndByCurrentValue()
    {
        to = Value;
    }
    
    public void SetMaterial(Material mMat)
    {
        this.mMat = mMat;
    }
}