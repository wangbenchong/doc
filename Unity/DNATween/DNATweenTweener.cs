using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DNATweenTweener : DNATweener
{
    public List<DNATweener> tweenList = null;
    [Tooltip("是否使用子动画曲线")]

    public bool UseChildAnimCurve = false;
    protected override void OnUpdate(float factor, bool isFinished)
    {
        //UnityEngine.Profiling.Profiler.BeginSample("DNATweenTweener-OnUpdate", gameObject);

        if (null != tweenList)
        {
            for (int i = 0; i < tweenList.Count; i++)
            {
                var tween = tweenList[i];
                if (tween == null || tween == this) continue;
                if (UseChildAnimCurve)
                {
                    tween.Sample(factor, isFinished);
                }
                else
                {
                    tween.DoOnUpdate(factor, isFinished);
                }
            }
        }

        //UnityEngine.Profiling.Profiler.EndSample();
    }
    protected override void Start()
    {
        if (null == tweenList) return;
        for (int i = 0; i < tweenList.Count; i++)
        {
            var tw = tweenList[i];
            if (tw == null) continue;
            if (tw.enabled)
                tw.enabled = false;
        }
    }

    public override void SetStartByCurrentValue()
    {
        if (null == tweenList) return;
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i] == null) continue;
            tweenList[i].SetStartByCurrentValue();
        }
    }

    public override void SetEndByCurrentValue()
    {
        if (null == tweenList) return;
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i] == null) continue;
            tweenList[i].SetEndByCurrentValue();
        }
    }

    /// <summary>
    /// 播放所有的动画;
    /// </summary>
    public void PlayAll(bool bForward = true)
    {
        // 播放的时候会重置所有动画;
        ResetAll();
        // 播放动画;
        if (null != tweenList)
        {
            for (int i = 0; i < tweenList.Count; i++)
            {
                if (tweenList[i] != null)
                {
                    if (bForward)
                    {
                        tweenList[i].PlayForwardForce();
                    }
                    else
                    {
                        tweenList[i].PlayReverseForce();
                    }
                }
            }
        }
    }
    /// <summary>
    /// 重置所有的动画;
    /// </summary>
    public void ResetAll(bool bForward = true)
    {
        // 设置到当前位置;
        if (null == tweenList) return;
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i] != null)
            {
                tweenList[i].ResetToBound(bForward);
            }
        }
        // 勾掉enable;
        enabled = false;
    }

#if UNITY_EDITOR
    [ContextMenu("设置DNATweenList")]
    private void SetTweenList()
    {
        if (null == tweenList) return;
        tweenList.Clear();
        DNATweener[] uiTweener = GetComponentsInChildren<DNATweener>(true);
        for (int i = 0; i < uiTweener.Length; i++)
        {
            DNATweener tw = uiTweener[i];
            if (tw == this)
            {
                continue;
            }
            if (this.tweenGroup == tw.tweenGroup)
            {
                tweenList.Add(tw);
            }
        }
    }
#endif
}
