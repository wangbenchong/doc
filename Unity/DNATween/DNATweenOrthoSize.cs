using UnityEngine;

/// <summary>
/// Tween the camera's orthographic size.
/// </summary>

[RequireComponent(typeof(Camera))]
[AddComponentMenu("DNA/Tween/DNATween Orthographic Size")]
public class DNATweenOrthoSize : DNATweener
{
    [HideInInspector]
    public float from = 1f;
    [HideInInspector]
    public float to = 1f;

    Camera mCam;

    /// <summary>
    /// Camera that's being tweened.
    /// </summary>

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
	public Camera cachedCamera { get { if (mCam == null) mCam = camera; return mCam; } }
#else
    public Camera cachedCamera { get { if (mCam == null) mCam = GetComponent<Camera>(); return mCam; } }
#endif

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get { return cachedCamera.orthographicSize; }
        set { cachedCamera.orthographicSize = value; }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }


    public static void Deactive(GameObject targetObj)
    {
        DeactiveTween<DNATweenOrthoSize>(targetObj);
    }

    public override void SetStartByCurrentValue() { from = value; }
    public override void SetEndByCurrentValue() { to = value; }
}
