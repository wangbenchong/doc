//#define CINEMACHINE
using UnityEngine;

#if CINEMACHINE
using UnityEngine.Splines;
using Unity.Cinemachine;
#endif

[AddComponentMenu("DNA/Tween/DNATweenSplineCart")]
public class DNATweenSplineCart : DNATweener
{
#if UNITY_EDITOR
    public override string DEFINE_SYMBOL =>
        #if CINEMACHINE
        string.Empty;
        #else
        "CINEMACHINE";
        #endif
    public override string DEFINE_DEPEND_PACKAGE => "Cinemachine";
#endif

#if CINEMACHINE
    [Range(0, 1f)]
    public float from = 0;
    [Range(0, 1f)]
    public float to = 1f;
    [Header("曲线路径(必填)")]
    public SplineContainer Spline;
    [Header("朝向配置(非必填), 仅 RotateType 为 3D 时有效(但也可留空)")]
    public CinemachineSplineRoll m_Roll;
    [Header("旋转相关")]
    [Tooltip(@"None: 无旋转，只控制位移

以下基于3D物体默认Z轴朝前：
3D: 3D空间，自由旋转
3D_Water: 3D空间，一碗水端平

以下基于2D物体默认姿态朝右：
2D_Bullet: 2D子弹模式，始终面向前方
2D_FlyBird: 2D飞鸟模式，始终面向前方，但肚皮不会朝上
2D_Land: 前方被限制为水平方向，非左即右，不会倾斜")]
    public RotateType rotateType = RotateType._3D;
    public enum RotateType
    {
        //-----以下基于3D物体默认Z轴朝前-------
        /// <summary>
        /// 无旋转，只控制位移
        /// </summary>
        None,
        /// <summary>
        /// 3D空间，自由旋转
        /// </summary>
        _3D,
        /// <summary>
        /// 3D空间，一碗水端平
        /// </summary>
        _3D_Water,
        //--------3D和2D分割线----------------
        //-----以下基于2D物体默认姿态朝右-------
        /// <summary>
        /// 2D子弹模式，始终面向前方
        /// </summary>
        _2D_Bullet = 10,
        /// <summary>
        /// 2D飞鸟模式，始终面向前方，但肚皮不会朝上。
        /// </summary>
        _2D_FlyBird,
        /// <summary>
        /// 2D陆地模式，前方被限制为水平方向，非左即右，不会倾斜
        /// </summary>
        _2D_Land,
    }
    [Tooltip("当反向播放时，是否调转朝向")]
    public bool reverseFlip = false;
   
    
    private Transform m_TransTarget = null;
    /// <summary>
    /// 目标Transform;
    /// </summary>
    public Transform TransTarget
    {
        get
        {
            if (m_TransTarget == null)
            {
                m_TransTarget = transform;
            }
            return m_TransTarget;
        }
        set { m_TransTarget = value; }
    }

    public void SetTarget(Transform trans)
    {
        TransTarget = trans;
    }

    /// <summary>
    /// Tween the value.
    /// </summary>
    protected override void OnUpdate(float factor, bool isFinished)
    {
        if(Spline == null || Spline.Splines.Count < 1) return;
        float realFactor = from * (1f - factor) + to * factor;
        var splinePath = Spline.Splines[0];
        EvaluateSplineWithRoll(splinePath, Spline.transform, realFactor, m_Roll , out var pos, out var rot);
        ConservativeSetPositionAndRotation(TransTarget, pos, rot);
    }

    public override void SetStartByCurrentValue() 
    {
        DNACommonFunction.WarningDialog("不支持回溯，请直接编辑Cinimachine Spline曲线");
    }

    public override void SetEndByCurrentValue() 
    {
        SetStartByCurrentValue();
    }
    
    private void ConservativeSetPositionAndRotation(Transform tran, Vector3 pos, Quaternion rot)
    {
        if(rotateType == RotateType.None)
        {
            if (!tran.position.Equals(pos))
                tran.position = pos;
        }
        else
        {
            if (!tran.position.Equals(pos) || !tran.rotation.Equals(rot))
                tran.SetPositionAndRotation(pos, rot);
        }
    }
    private static readonly Quaternion TO_2D = Quaternion.Euler(90, 0, 90);
    /// <summary>
    /// Apply to a <see cref="ISpline"/>additional roll from <see cref="CinemachineSplineRoll"/>
    /// </summary>
    /// <param name="spline">The spline in question</param>
    /// <param name="tran">The transform of the spline</param>
    /// <param name="tNormalized">The normalized position on the spline</param>
    /// <param name="roll">The additional roll to apply, or null</param>
    /// <param name="position">returned point on the spline, in world coords</param>
    /// <param name="rotation">returned rotation at the point on the spline, in world coords</param>
    /// <returns>True if the spline position is valid</returns>
    private bool EvaluateSplineWithRoll(
        ISpline spline,
        Transform tran,
        float tNormalized, 
        CinemachineSplineRoll roll,
        out Vector3 position, out Quaternion rotation)
    {
        var result = LocalEvaluateSplineWithRoll(spline, tNormalized, roll, out position, out rotation);
        position = Matrix4x4.TRS(tran.position, tran.rotation, Vector3.one).MultiplyPoint3x4(position);
        if(rotateType >= RotateType._2D_Bullet)
        {
            rotation *= TO_2D;
        }
        else if(rotateType == RotateType._3D)
        {
            rotation = tran.rotation * rotation;
        }
        return result;
    }
    /// <summary>
    /// Apply to a <see cref="ISpline"/>additional roll from <see cref="CinemachineSplineRoll"/>
    /// </summary>
    /// <param name="spline">The spline in question</param>
    /// <param name="tNormalized">The normalized position on the spline</param>
    /// <param name="roll">The additional roll to apply, or null</param>
    /// <param name="position">returned point on the spline, in spline-local coords</param>
    /// <param name="rotation">returned rotation at the point on the spline, in spline-local coords</param>
    /// <returns>True if the spline position is valid</returns>
    private bool LocalEvaluateSplineWithRoll(
        ISpline spline,
        float tNormalized, 
        CinemachineSplineRoll roll,
        out Vector3 position, out Quaternion rotation)
    {
        if (spline == null || !SplineUtility.Evaluate(
            spline, tNormalized, out var splinePosition, out var fwd, out var up))
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            return false;
        }

        position = splinePosition;
        if(rotateType == RotateType.None)
        {
            rotation = Quaternion.identity;
            return true;
        }

        // Use defaults if spline rotation is undefined
        var cross = Vector3.Cross(fwd, up);
        if (cross.AlmostZero() || cross.IsNaN())
        {
            fwd = IsForward || !reverseFlip ? Vector3.forward : Vector3.back;
            up = Vector3.up;
        }
        else 
        {
            if(!IsForward && reverseFlip)
            {
                fwd = -fwd;
            }
        }
        if(rotateType >= RotateType._2D_FlyBird)
        {
            if(fwd.x < 0)
            {
                up = -up;
            }
            if(rotateType == RotateType._2D_Land)
            {
                if(fwd.x >= 0)
                {
                    fwd = Vector3.right;
                }
                else
                {
                    fwd = Vector3.left;
                }
            }
        }
        if(rotateType == RotateType._3D_Water)
        {
            fwd.y = 0.00001f;//防止fwd为Vector3.zero导致旋转出错
            up = Vector3.up;
        }
        // Apply extra roll if present
        if (rotateType != RotateType._3D || roll == null || !roll.enabled)
            rotation = Quaternion.LookRotation(fwd, up);
        else
        {
            float rollValue = roll.Roll.Evaluate(spline, tNormalized, PathIndexUnit.Normalized, roll.GetInterpolator());
            rotation = Quaternion.LookRotation(fwd, up) * RollAroundForward(rollValue);

            // same as Quaternion.AngleAxis(roll, Vector3.forward), just simplified
            static Quaternion RollAroundForward(float angle)
            {
                float halfAngle = angle * 0.5F * Mathf.Deg2Rad;
                return new Quaternion(0, 0, Mathf.Sin(halfAngle), Mathf.Cos(halfAngle));
            }
        }
        return true;
    }
#else
protected override void OnUpdate(float factor, bool isFinished){}
#endif
}