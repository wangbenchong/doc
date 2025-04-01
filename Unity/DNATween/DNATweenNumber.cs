using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
[AddComponentMenu("DNA/Tween/DNATweenNumber")]
public class DNATweenNumber : DNATweener
{
    [HideInInspector]
    public long from = 0;
    [HideInInspector]
    public long to = 100;
    [HideInInspector]
    public string InputFormat = string.Empty;
    [HideInInspector]
    public bool IsStringUS = true;

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
    long cachedValue = 0;


    public long value
    {
        get
        {
            return cachedValue;
        }
        set
        {
            cachedValue = value;

            if (IsStringUS)
            {
                if (string.IsNullOrEmpty(InputFormat))
                    text.text = cachedValue.ToStringUS();
                else
                    text.text = DNACommonFunction.Format(cachedValue.ToStringUS());
            }
            else
            {
                if (string.IsNullOrEmpty(InputFormat))
                    text.text = cachedValue.ToString();
                else
                    text.text = DNACommonFunction.Format(InputFormat, cachedValue.ToString());
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
        long result = from;
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
