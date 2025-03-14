﻿using UnityEngine;
using UnityEngine.UI;
using DNA.UI;
[RequireComponent(typeof(UGUISimpleBar))]
public class DNATweenSimpleBar : DNATweener
{
    [HideInInspector]
    [Range(0f, 1f)]
    public float from = 1f;
    [HideInInspector]
    [Range(0f, 1f)]
    public float to = 1f;

    bool mCached = false;
    UGUISimpleBar simpleBar = null;

    void Cache()
    {
        mCached = true;
        simpleBar = GetComponent<UGUISimpleBar>();
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
            return simpleBar.Value;
        }
        set
        {
            if (!mCached)
                Cache();
            simpleBar.Value = value;
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
