using UnityEngine;

using BezierSolution;//Need install "Bezier Solution" from Unity Asset Store


[AddComponentMenu("DNA/Tween/DNATweenPositionBezier")]
public class DNATweenPositionBezier : DNATweener
{
	[Range(0f,1f)]
    public float from = 0f;
	[Range(0f,1f)]
    public float to = 1f;

    public BezierSpline spline;
    public LookAtMode lookAt = LookAtMode.Forward;

    public bool highQuality = false;
    private float m_normalizedT = 0f;

    public float value
    {
        get { return m_normalizedT; }
        set
        {
	        m_normalizedT = value;
	        float realFactor = highQuality ?  spline.evenlySpacedPoints.GetNormalizedTAtPercentage(m_normalizedT) : m_normalizedT;
	        TransTarget.position = spline.GetPoint(realFactor);

			if( lookAt == LookAtMode.Forward )
			{
				BezierSpline.Segment segment = spline.GetSegmentAt( realFactor );
				Quaternion targetRotation;
				if(IsForward)
					targetRotation = Quaternion.LookRotation( segment.GetTangent(), segment.GetNormal() );
				else
					targetRotation = Quaternion.LookRotation( -segment.GetTangent(), segment.GetNormal() );

				TransTarget.rotation = targetRotation;
			}
			else if (lookAt == LookAtMode.SplineExtraData)
			{
				TransTarget.rotation = spline.GetExtraData( realFactor, BezierWalker.extraDataLerpAsQuaternionFunction);
			}
        }
    }
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
        value = from * (1f - factor) + to * factor;
    }

    public override void SetStartByCurrentValue() { from = value; }

    public override void SetEndByCurrentValue() { to = value; }
}
