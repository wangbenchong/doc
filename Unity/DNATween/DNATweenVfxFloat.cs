using UnityEngine;
using UnityEngine.VFX;
[RequireComponent(typeof(VisualEffect))]
[AddComponentMenu("DNA/Tween/DNATweenVfxFloat")]
public class DNATweenVfxFloat : DNATweener
{
    public float from = 0;
    public float to = 1;
    public string propertyName = "";
    private int propertyID = -1;
    private VisualEffect effect;
    #if UNITY_EDITOR
    [ContextMenu("Refresh VFX PropertyID")]
    #endif
    public void RefreshPropertyID()
    {
        propertyID = -1;
    }
    private void DoRefreshPropertyID()
    {
        if(propertyID < 0)
        {
            effect = GetComponent<VisualEffect>();
            propertyID = Shader.PropertyToID(propertyName);
        }
    }

    public float value 
    { 
        get 
        {
            DoRefreshPropertyID();
            if(propertyID >= 0)
            {
                return effect.GetFloat(propertyID);
            }
            return 0;
        } 
        set
        {
            DoRefreshPropertyID();
            if(propertyID >= 0)
            {
                effect.SetFloat(propertyID, value);
            }
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from * (1f - factor) + to * factor;
    }

    public override void SetStartByCurrentValue() {from = value; }

    public override void SetEndByCurrentValue() {to = value;}
}
