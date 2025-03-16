using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("DNA/Tween/DNATweenColor")]
public class DNATweenColor : DNATweener
{
    [HideInInspector]
    public Color from = Color.white;
    [HideInInspector]
    public Color to = Color.white;

    bool mCached = false;
    Material mMat;
    Light mLight;
    SpriteRenderer mSr;
    Graphic mGraphic;

    private bool b_LightNotNull = false;
    private bool b_SpriteRenderNotNull = false;
    private bool b_GraphicNotNull = false;

    //=====================材质=====================
    private bool b_MatNotNull = false;
    private bool b_UseMatProperty = false;
    private int materialKeyNameID;
    //==============================================

    void Cache()
    {
        mCached = true;

        b_LightNotNull = false;
        b_MatNotNull = false;
        b_SpriteRenderNotNull = false;
        b_GraphicNotNull = false;

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

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
		Renderer ren = renderer;
#else
        Renderer ren = GetComponent<Renderer>();
#endif
        if (ren != null)
        {
            mMat = ren.material;
            b_MatNotNull = true;
            return;
        }

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
		mLight = light;
#else
        mLight = GetComponent<Light>();
#endif
        if (mLight != null)
        {
            b_LightNotNull = true;
            return;
        }
    }

    /// <summary>
    /// 设置自定义的材质
    /// </summary>
    /// <param name="material"></param>
    /// <param name="materialPropertyName"></param>
    public void SetCustomMaterial(Material material, string materialPropertyName)
    {
        mCached = true;
        b_MatNotNull = true;
        b_UseMatProperty = true;
        mMat = material;
        materialKeyNameID = Shader.PropertyToID(materialPropertyName);
    }

    [System.Obsolete("Use 'value' instead")]
    public Color color { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Color value
    {
        get
        {
            if (!mCached) Cache();
            if (b_GraphicNotNull)
            {
                return mGraphic.color;
            }
            if (b_MatNotNull)
            {
                if (b_UseMatProperty)
                {
                    return mMat.GetColor(materialKeyNameID);
                }
                else
                {
                    return mMat.color;
                }
            }
            if (b_SpriteRenderNotNull)
            {
                return mSr.color;
            }
            if (b_LightNotNull)
            {
                return mLight.color;
            }
            return Color.black;
        }
        set
        {
            if (!mCached) Cache();
            if (b_GraphicNotNull)
            {
                mGraphic.color = value;
            }
            else if (b_MatNotNull)
            {
                if (b_UseMatProperty)
                {
                    mMat.SetColor(materialKeyNameID, value);
                }
                else
                {
                    mMat.color = value;
                }
            }
            else if (b_SpriteRenderNotNull)
            {
                mSr.color = value;
            }
            else if (b_LightNotNull)
            {
                mLight.color = value;
                mLight.enabled = (value.r + value.g + value.b) > 0.01f;
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
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = Color.Lerp(from, to, factor); }

    public override void SetStartByCurrentValue() { from = value; }
    public override void SetEndByCurrentValue() { to = value; }
}
