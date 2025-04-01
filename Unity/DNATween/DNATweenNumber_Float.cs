using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the camera's field of view.
/// </summary>
[RequireComponent(typeof(Text))]
[AddComponentMenu("DNA/Tween/DNATweenNumber_Float")]
public class DNATweenNumber_Float : DNATweener
{
    [HideInInspector]
    public float from = 0;
    [HideInInspector]
    public float to = 100;
    [HideInInspector]
    public string InputFormat = string.Empty;
    [HideInInspector]
    public bool IsStringUS = true;
    /// <summary>
    /// 小数点后保留位数
    /// </summary>
    [HideInInspector]
    public int DotNum = 1;
    /// <summary>
    /// 是否四舍五入
    /// </summary>
    [HideInInspector]
    public bool RoundToInt = true;

    private Text m_text = null;
    /// <summary>
    /// 目标Transform;
    /// </summary>
    public Text text
    {
        get
        {
            if (m_text == null)
            {
                m_text = transform.GetComponent<Text>();
            }
            return m_text;
        }
    }


    /// <summary>
    /// Tween's current value.
    /// </summary>
    float cachedValue = 0;


    public float value
    {
        get
        {
            return cachedValue;
        }
        set
        {
            cachedValue = value;
            if (RoundToInt)
            {
                float _count = Mathf.Pow(10, DotNum);
                cachedValue = Mathf.RoundToInt(cachedValue * _count) / _count;
            }
            else
            {
                float _count = Mathf.Pow(10, DotNum);
                cachedValue = (int)(cachedValue * _count) / _count;
            }

            if (IsStringUS)
            {
                if (string.IsNullOrEmpty(InputFormat))
                    text.text = cachedValue.ToString("N" + DotNum).Replace(',', ' ');
                else
                    text.text = DNACommonFunction.Format(cachedValue.ToString("N" + DotNum).Replace(',', ' '));
            }
            else
            {
                if (string.IsNullOrEmpty(InputFormat))
                    text.text = cachedValue.ToString("F" + DotNum);
                else
                    text.text = DNACommonFunction.Format(InputFormat, cachedValue.ToString("F" + DotNum));
            }
            if (from == to && enabled)//优化：停止无意义的逐帧刷新
            {
                enabled = false;
            }
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        //value = (int)(from * (1f - factor) + to * factor);

        if (from == to)
        {
            value = from;
            return;
        }


        factor = Mathf.Clamp01(factor);
        float result = from;
        if (DNACommonFunction.IsApproximately(factor, 0))
        {

        }
        else if (DNACommonFunction.IsApproximately(factor, 1))
        {
            result = to;
        }
        else
        {
            int tempI = Mathf.RoundToInt(factor * 100);
            if (tempI < 0) tempI = 0;
            else if (tempI > 100) tempI = 100;

            result = from + (to - from) * tempI / 100;
        }

        value = result;
    }

    public static DNATweenNumber_Float Tween(GameObject targetObj, int toNum, Style eLoopType, float fDuration, System.Action cbFinish, bool reuse)
    {
        DNATweenNumber_Float tweenNum = CreateTween<DNATweenNumber_Float>(targetObj);
        tweenNum.from = tweenNum.value;
        tweenNum.to = toNum;
        tweenNum.DoTween(eLoopType, fDuration, cbFinish, reuse);

        return tweenNum;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartByCurrentValue()
    {
        from = value;
    }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndByCurrentValue()
    {
        to = value;
    }
}
