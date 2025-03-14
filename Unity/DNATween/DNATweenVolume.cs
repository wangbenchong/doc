using UnityEngine;

/// <summary>
/// Tween the audio source's volume.
/// </summary>

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("DNA/Tween/DNATweenVolume")]
public class DNATweenVolume : DNATweener
{
#if UNITY_3_5
    [HideInInspector]
	public float from = 1f;
    [HideInInspector]
	public float to = 1f;
#else
    [HideInInspector]
    [Range(0f, 1f)]
    public float from = 1f;
    [HideInInspector]
    [Range(0f, 1f)]
    public float to = 1f;
#endif

    AudioSource mSource;

    /// <summary>
    /// Cached version of 'audio', as it's always faster to cache.
    /// </summary>

    public AudioSource audioSource
    {
        get
        {
            if (mSource == null)
            {
                mSource = GetComponent<AudioSource>();

                if (mSource == null)
                {
                    mSource = GetComponent<AudioSource>();

                    if (mSource == null)
                    {
                        DNACommonFunction.LogError("TweenVolume needs an AudioSource to work with" + this.name);
                        enabled = false;
                    }
                }
            }
            return mSource;
        }
    }

    [System.Obsolete("Use 'value' instead")]
    public float volume { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Audio source's current volume.
    /// </summary>

    public float value
    {
        get
        {
            return audioSource != null ? mSource.volume : 0f;
        }
        set
        {
            if (audioSource != null) mSource.volume = value;
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from * (1f - factor) + to * factor;
        mSource.enabled = (mSource.volume > 0.01f);
    }

    public static void Deactive(GameObject targetObj)
    {
        DeactiveTween<DNATweenVolume>(targetObj);
    }

    public override void SetStartByCurrentValue() { from = value; }
    public override void SetEndByCurrentValue() { to = value; }
}
