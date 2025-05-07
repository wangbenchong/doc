using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEditor;

public abstract class DNATweener : DNABaseMono
{
    //单帧最长时间限制
    public const float MAX_DELTA_TIME = 0.034f;//极限情况，每秒29帧

    [System.Serializable]
    public class TimePointData
    {
        [Range(0f, 1f)]
        public float time = 0;
        public bool isPointDone { get; set; }
        public EDirection direction = EDirection.Both;
        public UnityEvent unityEvent = null;
    }

    public enum Style
    {
        Once,
        Loop,
        PingPong,
    }
    public enum UpdateType
    {
        Update,
        LateUpdate,
        FixedUpdate,
    }
    //事件触发的条件（动画正、反向触发）
    public enum EDirection
    {
        Both,
        Forward,
        Reverse,
    }

    //枚举值每个都写明数字，并且留足够间隔，这样后面追加新的类型就可以选择插入任何位置
    public enum ECurveType
    {
        Default = 0,  //直线 y = x;
        EaseIn = 10,
        EaseInSteeper = 11,
        EaseOut = 20,
        EaseOutSteeper = 21,
        EaseInOut = 30,
        EaseInOutSteeper = 31,
        BounceIn = 40,
        BounceOut = 41,
        Custom = 100,
    }

    [HideInInspector]
    public Style style = Style.Once;
    [HideInInspector]
    public UpdateType updateType = UpdateType.Update;
    [HideInInspector]
    public ECurveType curveType = ECurveType.Default;
    [HideInInspector]
    [SerializeField]
    AnimationCurveData curveData;

    [HideInInspector]
    public bool ignoreTimeScale = false;

    [HideInInspector]
    public float delay = 0f;

    [HideInInspector]
    public float duration = 1f;

    [HideInInspector]
    public int tweenGroup = 0;

    #region EventOnInspector
    public List<UnityEvent> OnFinishedList = null;
    public List<TimePointData> TimePointList = null;
    #endregion

    #region 非序列化事件，供代码调用
    private List<Action> onFinishedActions = null;
    /// <summary>
    /// delay大于0时有效，而且该回调被调用后会被置空
    /// </summary>
    private Action onDelayFinishedAction = null;
    #endregion
    //-----For Delay------
    bool mStarted = false;
    float mStartTime = 0f;
    //---------------------
    float mDuration = 0f;
    float mAmountPerDelta = 1000f;
    float mFactor = 0f;

    /// <summary>
    /// Amount advanced per delta time.
    /// </summary>
    public float AmountPerDelta
    {
        get
        {
            if (mDuration != duration)
            {
                mDuration = duration;
                mAmountPerDelta = Mathf.Abs((duration > 0f) ? 1f / duration : 1000f) * Mathf.Sign(mAmountPerDelta);
            }
            return mAmountPerDelta;
        }
    }

    /// <summary>
    /// Tween factor, 0-1 range.
    /// </summary>

    public float TweenFactor
    {
        get
        {
            return mFactor;
        }
        set
        {
            mFactor = Mathf.Clamp01(value);
        }
    }

    /// <summary>
    /// Direction that the tween is currently playing in.
    /// </summary>

    public bool IsForward
    {
        get
        {
            return AmountPerDelta >= 0f;
        }
    }
    
    /// <summary>
    /// Update as soon as it's started so that there is no delay.
    /// </summary>
    protected virtual void Start()
    {
        //空防护
        //if (this.curveType == ECurveType.Custom)
        //{
        //    if (this.curveData == null)
        //    {
        //        this.curveType = ECurveType.Default;
        //    }
        //}
        //Update();
    }
    /// <summary>
    /// Update the tweening factor and call the virtual update function.
    /// </summary>
    

    void Update()
    {
        if(updateType == UpdateType.Update || updateType == UpdateType.LateUpdate)
            LogicUpdate(UpdateType.Update);
    }
    // void LateUpdate()
    // {
    //     if(updateType == UpdateType.LateUpdate
    //     #if UNITY_EDITOR
    //     && Application.isPlaying
    //     #endif
    //     )
    //         LogicUpdate(UpdateType.LateUpdate);
    // }
    void FixedUpdate()
    {
        if(updateType == UpdateType.FixedUpdate
        #if UNITY_EDITOR
        && Application.isPlaying
        #endif
        )
            LogicUpdate(UpdateType.FixedUpdate);
    }
    private void LogicUpdate(UpdateType updateType)
    {
#if UNITY_EDITOR
        if (this == null)
        {
            DNACommonFunction.LogError("this is null, return;");
            RemoveUpdateEditor();
            return;
        }
#endif

        float delta = 0f;
        float time = 0f;
        switch(updateType)
        {
            case UpdateType.FixedUpdate:
                delta = ignoreTimeScale ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
                time = ignoreTimeScale ? Time.fixedUnscaledTime : Time.fixedTime;
                break;
            case UpdateType.Update:
            case UpdateType.LateUpdate:
            default:
                delta = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                time = ignoreTimeScale ? Time.unscaledTime : Time.time;
                break;
            
        }
        if (delta > MAX_DELTA_TIME)
        {
            delta = MAX_DELTA_TIME;
        }
        if(delay > 0)
        {
            if (!mStarted)
            {
                if(mFactor <= 0)
                {
                    Sample(0f, false);
                    mStartTime = time + delay;
                }
                mStarted = true;
            }

            if (time < mStartTime)
                return;

            if (
            #if UNITY_EDITOR
                Application.isPlaying &&
            #endif
                null != onDelayFinishedAction)
            {
                System.Action dele = onDelayFinishedAction;
                onDelayFinishedAction = null;
                dele.Invoke();
            }
        }

        // Advance the sampling factor
        mFactor += AmountPerDelta * delta;

        // Loop style simply resets the play factor after it exceeds 1.
        if (style == Style.Loop)
        {
            if (mFactor > 1f)
            {
                mFactor -= Mathf.Floor(mFactor);
                ResetTimePoint();
            }
        }
        else if (style == Style.PingPong)
        {
            // Ping-pong style reverses the direction
            if (mFactor > 1f)
            {
                mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
                mAmountPerDelta = -mAmountPerDelta;
                ResetTimePoint();
            }
            else if (mFactor < 0f)
            {
                mFactor = -mFactor;
                mFactor -= Mathf.Floor(mFactor);
                mAmountPerDelta = -mAmountPerDelta;
                ResetTimePoint();
            }
        }

        HandleTimePointEvent();

        if ((style == Style.Once) && (duration == 0f || mFactor > 1f || mFactor < 0f))
        {
            ResetTimePoint();
            mFactor = Mathf.Clamp01(mFactor);
            Sample(mFactor, true);

            //Tween just Finish Now, first step set enabled to False
            enabled = false;

            #if UNITY_EDITOR
            if (IsEditorUpdate)
            {
                RemoveUpdateEditor();
            }
            if(!Application.isPlaying)return;
            #endif

            //--------- UnityEvent ------------
            if (null != OnFinishedList)
            {
                for (int i = 0; i < OnFinishedList.Count; ++i)
                    OnFinishedList[i].Invoke();
            }
            //------------------------------

            if (onFinishedActions != null)
            {
                for (int i = 0; i < onFinishedActions.Count; ++i)
                {
                    Action ed = onFinishedActions[i];
                    ed.Invoke();
                }
            }
        }
        else
        {
            Sample(mFactor, false);
        }
    }

    private void HandleTimePointEvent()
    {
        if (
            #if UNITY_EDITOR
            Application.isPlaying &&
            #endif
            null != TimePointList)
        {
            for (int i = 0; i < TimePointList.Count; ++i)
            {
                TimePointData data = TimePointList[i];
                if (!data.isPointDone && (mAmountPerDelta > 0 && data.time <= mFactor || mAmountPerDelta < 0 && data.time >= mFactor))
                {
                    data.isPointDone = true;
                    UnityEvent onPointed = data.unityEvent;
                    if (null != onPointed)
                    {
                        switch (data.direction)
                        {
                            case EDirection.Both:
                                onPointed.Invoke();
                                break;
                            case EDirection.Forward:
                                if (mAmountPerDelta > 0)
                                    onPointed.Invoke();
                                break;
                            case EDirection.Reverse:
                                if (mAmountPerDelta < 0)
                                    onPointed.Invoke();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Convenience function -- add a new OnFinished event delegate (here for to be consistent with RemoveOnFinished).
    /// </summary>

    public void AddOnFinishedAction(Action action)
    {
        if (onFinishedActions == null)
        {
            onFinishedActions = new List<Action>();
        }

        if (!onFinishedActions.Contains(action))
        {
            onFinishedActions.Add(action);    
        }
    }
    
    public void ClearOnFinishedAction()
    {
        if (onFinishedActions != null)
            onFinishedActions.Clear();
    }

    /// <summary>
    /// Remove an OnFinished delegate. Will work even while iterating through the list when the tweener has finished its operation.
    /// </summary>

    public void RemoveOnFinishedAction(Action action)
    {
        if (onFinishedActions != null)
            onFinishedActions.Remove(action);
    }

    public void SetDelayFinishedAction(Action action)
    {
        if(Application.isPlaying)
        {
            onDelayFinishedAction = action;
        }
    }

    /// <summary>
    /// Mark as not started when finished to enable delay on next play.
    /// </summary>

    void OnDisable()
    {
        mStarted = false;
    }

    /// <summary>
    /// Sample the tween at the specified factor.
    /// </summary>
    public void Sample(float factor, bool isFinished)
    {
        if (this == null)
        {
            DNACommonFunction.LogError("this is null");
            return;
        }
        this.TweenFactor = factor;

        // Call the virtual update
        OnUpdate(GetFactor(curveType, factor), isFinished);
    }

    float GetFactor(ECurveType curveType, float factor)
    {
        switch (curveType)
        {
            case ECurveType.Default:
                return factor;
            case ECurveType.EaseIn:
                return EaseIn(factor, false);
            case ECurveType.EaseInSteeper:
                return EaseIn(factor, true);
            case ECurveType.EaseOut:
                return EaseOut(factor, false);
            case ECurveType.EaseOutSteeper:
                return EaseOut(factor, true);
            case ECurveType.EaseInOut:
                return EaseInOut(factor, false);
            case ECurveType.EaseInOutSteeper:
                return EaseInOut(factor, true);
            case ECurveType.BounceIn:
                return BounceLogic(factor);
            case ECurveType.BounceOut:
                return BounceLogic(1 - factor);
            case ECurveType.Custom:
                if (curveData != null && curveData.Curve != null)
                {
                    return curveData.Curve.Evaluate(factor);
                }
                else
                {
                    return AmountPerDelta > 0 ? 0 : 1;
                    //return factor;
                }
        }
        return factor;
    }

    private static float BounceLogic(float val)
    {
        if (val < 0.363636f) // 0.363636 = (1/2.75)
        {
            val = 7.5685f * val * val;
        }
        else if (val < 0.727272f) // 0.727272 = (2/2.75)
        {
            val = 7.5625f*(val -= 0.545454f)*val + 0.75f; // 0.545454f = (1.5/2.75)
        } 
        else if (val < 0.909090f) //0.909090 = (2.5/2.75)
        {
            val = 7.5625f*(val -= 0.818181f)*val + 0.9375f; //0.818181 = (2.25/2.75)
        }
        else
        {
            val = 7.5625f*(val -= 0.9545454f)*val + 0.984375f; //0.9545454 = (2.625 / 2.75)
        }
        return val;
    }

    private static float EaseIn(float val, bool steeper)
    {
        val = 1f - Mathf.Sin(0.5f*Mathf.PI*(1f - val));
        if (steeper)
        {
            val *= val;
        }
        return val;
    }

    private static float EaseOut(float val, bool steeper)
    {
        val = Mathf.Sin(0.5f*Mathf.PI*val);
        if (steeper)
        {
            val = 1f - val;
            val = 1f - val * val;
        }
        return val;
    }

    private static float EaseInOut(float val, bool steeper)
    {
        const float pi2 = Mathf.PI*2f;
        val = val - Mathf.Sin(val * pi2) / pi2;
        if (steeper)
        {
            val = val*2f - 1f;
            float sign = Mathf.Sign(val);
            val = 1f - Mathf.Abs(val);
            val = 1f - val * val;
            val = sign*val*0.5f + 0.5f;
        }
        return val;
    }

    public void SetStyle(int style)
    {
        this.style = (Style)style;
    }

    public virtual void Play(bool forward)
    {
        mAmountPerDelta = Mathf.Abs(AmountPerDelta);
        if (!forward)
            mAmountPerDelta = -mAmountPerDelta;
        enabled = true;
        LogicUpdate(updateType);
    }

    /// <summary>
    /// Play the tween forward.
    /// </summary>

    public void PlayForward()
    {
        Play(true);
    }

    /// <summary>
    /// Play the tween in reverse.
    /// </summary>

    public void PlayReverse()
    {
        Play(false);
    }

    /// <summary>
    /// 强制正向播放tween，可指定进度值，默认是0
    /// </summary>
    /// <param name="spcifyFactor"></param>
    public void PlayForwardForce(float spcifyFactor)
    {
        mFactor = spcifyFactor;
        if(mFactor <= 0)
        {
            mStarted = false;
        }
        ResetTimePoint();
        PlayForward();
    }

    /// <summary>
    /// 强制正向播放tween，从0开始
    /// </summary>
    public void PlayForwardForce()
    {
        PlayForwardForce(0);
    }

    /// <summary>
    /// 强制反向播放tween，可指定进度值，默认是1
    /// </summary>
    /// <param name="spcifyFactor"></param>
    public void PlayReverseForce(float spcifyFactor)
    {
        mFactor = spcifyFactor;
        ResetTimePoint();
        PlayReverse();
    }
 
    /// <summary>
    /// 强制反向播放tween，从1开始
    /// </summary>
    public void PlayReverseForce()
    {
        PlayReverseForce(1);
    }

    /// <summary>
    /// 重置timePoint的设置
    /// </summary>
    public void ResetTimePoint()
    {
        if (
            #if UNITY_EDITOR
            !Application.isPlaying ||
            #endif
            null == TimePointList) return;
        for (int i = 0; i < TimePointList.Count; ++i)
            TimePointList[i].isPointDone = false;
    }

    
    /// <summary>
    /// 重置到初始位置（会根据进度曲线做变换）
    /// </summary>
    public void ResetToBeginning()
    {
        ResetToBeginningForce(true);
    }

    /// <summary>
    /// 重置到初始位置（会根据进度曲线做变换）
    /// </summary>
    /// <param name="isForward">true是正方向</param>
    public void ResetToBeginningForce(bool isForward)
    {
        mFactor = isForward ? 0f : 1f;
        Sample(mFactor, false);
        enabled = false;
        mStarted = false;
    }
    
    /// <summary>
    /// 重置到临界位置（不考虑进度曲线）
    /// </summary>
    /// <param name="boundIsStart"></param>

    public void ResetToBound(bool boundIsStart)
    {
        mFactor = boundIsStart ? 0f : 1f;
        if (boundIsStart)
        {
            OnUpdate(0f, false);
        }
        else
        {
            OnUpdate(1f, false);
        }
        enabled = false;
        mStarted = false;
    }

    /// <summary>
    /// Manually start the tweening process, reversing its direction.
    /// </summary>
    public void Toggle()
    {
        if (mFactor > 0f)
        {
            mAmountPerDelta = -AmountPerDelta;
        }
        else
        {
            mAmountPerDelta = Mathf.Abs(AmountPerDelta);
        }
        enabled = true;
    }
    /// <summary>
    /// just set its direction without playing it.
    /// </summary>
    public void ToggleForce(bool isForward)
    {
        if (isForward)
        {
            mAmountPerDelta = Mathf.Abs(AmountPerDelta);
        }
        else
        {
            mAmountPerDelta = -Mathf.Abs(AmountPerDelta);
        }
    }

    /// <summary>
    /// Actual tweening logic should go here.
    /// </summary>

    abstract protected void OnUpdate(float factor, bool isFinished);
    //for son class : TweenTweener
    public void DoOnUpdate(float factor, bool isFinished)
    {
        OnUpdate(factor, isFinished);
    }

    /// <summary>
    /// Set the 'from' value by the current one.
    /// </summary>
    public virtual void SetStartByCurrentValue()
    {
    }

    /// <summary>
    /// Set the 'to' value by the current one.
    /// </summary>
    public virtual void SetEndByCurrentValue()
    {
    }

    /// <summary>
    /// 动态创建Tween组件之后会调用，不是常用的做法，除非你知道你在做什么。
    /// </summary>
    /// <param name="eLoopType"></param>
    /// <param name="fDuration"></param>
    /// <param name="cbFinish"></param>
    /// <param name="reuse">设为false会在动画结束时销毁Tween组件，慎用</param>
    protected void DoTween(Style eLoopType, float fDuration, Action cbFinish, bool reuse)
    {
        style = eLoopType;
        duration = fDuration;
        if (cbFinish != null)
        {
            AddOnFinishedAction(cbFinish);
        }
        if(!reuse)
        {
            AddOnFinishedAction(()=>{Destroy(this);});
        }
        PlayForward();
    }

    /// <summary>
    /// 动态创建Tween组件，不是常用的做法，除非你知道你在做什么。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetObj"></param>
    /// <returns></returns>
    protected static T CreateTween<T>(GameObject targetObj) where T : DNATweener
    {
        T tweenCOM = targetObj.GetComponent<T>();
        if (tweenCOM == null)
        {
            tweenCOM = (T)targetObj.AddComponent<T>();
        }

        return tweenCOM;
    }

    /// <summary>
    /// For Common UnityEvent to call
    /// </summary>
    /// <param name="go"></param>

    public void DestroyGameobject(GameObject go)
    {
        if (go != null)
        {
            Destroy(go);
        }
    }


#if UNITY_EDITOR//编辑器函数
    public void ResetToBeginEditor()
    {
        RemoveUpdateEditor();
        ResetToBeginningForce(true);
    }

    public void ResetToEndEditor()
    {
        RemoveUpdateEditor();
        ResetToBeginningForce(false);
    }

    public void PlayEditor(bool forward)
    {
        RemoveUpdateEditor();
        if (Application.isEditor && !Application.isPlaying)
        {
            AddUpdateEditor();
        }

        if (forward)
        {
            PlayForwardForce();
        }
        else
        {
            PlayReverseForce();
        }
    }
    public bool IsEditorUpdate { get; private set; }
    public void AddUpdateEditor()
    {
        if (!IsEditorUpdate)
        {
            IsEditorUpdate = true;
            EditorApplication.update += Update;
        }
    }

    public void RemoveUpdateEditor()
    {
        IsEditorUpdate = false;
        EditorApplication.update -= Update;
    }

    private static string _animCurveFolder = "";
    /// <summary>
    /// 曲线存储路径——根目录
    /// </summary>
    public static string AnimCurveFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_animCurveFolder))
            {
                _animCurveFolder = EditorPrefs.GetString("DNATween_AnimCurveFolder", "Assets/AnimationCurveData");
            }
            return _animCurveFolder;
        }
        set
        {
            if(_animCurveFolder != value)
            {
                EditorPrefs.SetString("DNATween_AnimCurveFolder", value);
                _animCurveFolder = value;
            }
        }
    }

    [ContextMenu("新建曲线")]
    public void _CreateUIAnimationCurveData()
    {
        CreateUIAnimationCurveData();
    }
    
    private static void CreateUIAnimationCurveData()
    {
        string path = AnimCurveFolder + "/Curve_XXX.asset";
        AnimationCurveData asset = ScriptableObject.CreateInstance<AnimationCurveData>();
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        asset = AssetDatabase.LoadMainAssetAtPath(path) as AnimationCurveData;
        asset.Curve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = asset;
    }
    [ContextMenu("曲线存储路径")]
    public void _SelectCurveFolder()
    {
        SelectCurveFolder();
    }
    private static void SelectCurveFolder()
    {
        string fullpath = Application.dataPath.Replace("Assets", "") + AnimCurveFolder;
        fullpath = fullpath.Replace("/", "\\");
        System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(fullpath);
        if (dinfo.Exists)
        {
            System.IO.FileInfo[] finfo = dinfo.GetFiles("*.asset");
            if (finfo != null && finfo.Length > 0)
            {
                var obj = AssetDatabase.LoadMainAssetAtPath(AnimCurveFolder + "/" + finfo[0].Name);
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
                return;
            }
        }
        EditorUtility.DisplayDialog("Error", "Not Find Path " + AnimCurveFolder, "ok");
    }
#endif//编辑器函数
}