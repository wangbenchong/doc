using UnityEngine;
namespace DNA.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class DnaSimpleBar : MonoBehaviour
    {
        public RectTransform.Axis mDirection = RectTransform.Axis.Horizontal;
        public bool mirror = false;//镜像
        public RectTransform mContent = null;
        [SerializeField]
        [Range(0f, 1f)]
        private float mValue = 1f;

        // 从当前位置涨到mTweenTarget，mTweenTarget > mValue
        private bool mTweening = false;
        private float mTweenTarget = 1;
        private float mDuration = 3;
        private float mTick = 0;
        private float mStartValue = 0;

        public float Value
        {
            get
            {
                return mValue;
            }
            set
            {
                if (value > 1f)
                {
                    mValue = 1f;
                }
                else if (value < 0f)
                {
                    mValue = 0f;
                }
                else
                {
                    mValue = value;
                }
                RefreshValue();
            }
        }
        private bool bRectTranCached = false;
        private RectTransform _mRectTran;
        private RectTransform mRectTran
        {
            get
            {
                if (!bRectTranCached)
                {
                    _mRectTran = GetComponent<RectTransform>();
                    bRectTranCached = true;
                }
                return _mRectTran;
            }
        }
        [ContextMenu("Refresh")]
        private void RefreshValue()
        {
            if (mContent == null)
            {
                return;
            }
            //运行中执行会出问题
            //if (mirror)
            //{
            //    mContent.pivot = Vector2.right;
            //}
            //else
            //{
            //    mContent.pivot = Vector2.zero;
            //}
            float size = (mDirection == RectTransform.Axis.Horizontal) ? mRectTran.rect.width : mRectTran.rect.height;
            mContent.SetSizeWithCurrentAnchors(mDirection, size * Value);
        }

        private void Update()
        {
            if (!mTweening)
                return;
            if (mTweenTarget <= mValue)
                return;
            mTick += Time.deltaTime;
            Value = Mathf.Lerp(mStartValue, mTweenTarget, mTick / mDuration);
            if (Value >= mTweenTarget)
            {
                Value = mTweenTarget;
                mTweening = false;
            }
        }

        public void TweenToValue(float target, float duration)
        {
            if (target <= mValue)
                return;
            mTweening = true;

            mTweenTarget = target;
            mDuration = duration;
            mTick = 0;
            mStartValue = Value;
        }

#if UNITY_EDITOR
        public void OnValidateEditor()
        {
            RefreshValue();
        }
#endif
    }
}