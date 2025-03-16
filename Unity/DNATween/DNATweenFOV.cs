using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("DNA/Tween/DNATween Field of View")]
public class DNATweenFOV : DNATweener
{
    [HideInInspector]
    public float from = 45f;
    [HideInInspector]
    public float to = 45f;

    Camera mCam;

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
	public Camera cachedCamera { get { if (mCam == null) mCam = camera; return mCam; } }
#else
    public Camera cachedCamera { get { if (mCam == null) mCam = GetComponent<Camera>(); return mCam; } }
#endif

    [System.Obsolete("Use 'value' instead")]
    public float fov { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value { get { return cachedCamera.fieldOfView; } set { cachedCamera.fieldOfView = value; } }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartByCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndByCurrentValue() { to = value; }
}
