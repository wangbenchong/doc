/////////////////////////////////////////////////////////////////////////////////
//
//  author: wangbenchong
//
//	description:	UGUI的ScrollRect派生类
//					
//
//					
//					
//
/////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace DNA.UI
{
    public class DnaScrollRect : ScrollRect
    {
        private bool mIsDragging = false;
        /// <summary>
        /// 当前是否正在拖拽
        /// </summary>
        public bool IsDragging
        {
            get
            {
                return mIsDragging;
            }
        }
        PointerEventData beginDragEvent;
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            mIsDragging = true;
            this.beginDragEvent = eventData;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            mIsDragging = false;
        }
#if UNITY_EDITOR
        void OnApplicationFocus(bool isPause)
        {
            OnApplicationPause(!isPause);
        }
#endif
        void OnApplicationPause(bool isPause)
        {
            if (!isPause)
            {
                if (mIsDragging && beginDragEvent != null)
                {
                    OnEndDrag(beginDragEvent);
                    beginDragEvent = null;
                }
            }
        }
    }
}
