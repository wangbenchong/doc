//编辑器下推荐配合一个脚本（支持编辑器运行时实例化临时材质，如MatFixInEditor脚本）使用
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("DNA/Tween/DNATweenMatFloat")]
public class DNATweenMatFloat : DNATweener
{
    public float from = 0;
    public float to = 1;
    public string propertyName = "";
    private int propertyID = -1;
    public Material mat{get;private set;}
    [SerializeField]
    [Tooltip("If true, will use shared material to tween property.\nIf false, will use propertyBlock to tween property.\nIf UGUI, force be true.")]
    private bool isShareMat = true;
    //if isShareMat = false, will use propertyBlock to tween material property
    private MaterialPropertyBlock propertyBlock;
    private Renderer rd;
    private bool isInitedMat = false;
    //Only need call after changing mat or propertyName(Typically there's no need to do so) during runtime.
    #if UNITY_EDITOR
    [ContextMenu("InitMat")]
    #endif
    public void InitMat()
    {
        propertyID = -1;
        isInitedMat = false;
    }
    private void DoInitMat()
    {
        if(isInitedMat || !Application.isPlaying)
        {
            return;
        }
        isInitedMat = true;
        propertyID = Shader.PropertyToID(propertyName);
        var graphic = GetComponent<Graphic>();
        if(propertyID >= 0)
        {
            if (graphic != null && graphic.material != graphic.defaultMaterial)
            {
                isShareMat = true;
                if(graphic.material != graphic.defaultMaterial)
                {
                    mat = graphic.material;
                }
            }
            else
            {
                rd = GetComponent<Renderer>();
                if(rd != null)
                {
                    if(!isShareMat)
                    {
                        if(propertyBlock == null)
                        {
                            propertyBlock = new MaterialPropertyBlock();
                        }
                        rd.GetPropertyBlock(propertyBlock);
                    }
                    mat = rd.sharedMaterial;
                }
            }
            if(mat == null)
            {
                propertyID = -1;
                enabled = false;
            }
        }
    }

    public float value 
    { 
        get 
        { 
            if (propertyID >= 0 && isInitedMat)
            {
                if(!isShareMat)
                {
                    return propertyBlock.GetFloat(propertyID);
                }
                else
                {
                    return mat.GetFloat(propertyID);
                }
            }
            return 0;
        } 
        set
        {
            if (propertyID >= 0 && isInitedMat)
            {
                if(!isShareMat)
                {
                    propertyBlock.SetFloat(propertyID, value);
                    rd.SetPropertyBlock(propertyBlock);
                }
                else
                {
                    mat.SetFloat(propertyID, value);
                }
            }
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        DoInitMat();
        if(propertyID >= 0 && isInitedMat)
        {
            value = from * (1f - factor) + to * factor;
        }
    }

    public override void SetStartByCurrentValue() {}

    public override void SetEndByCurrentValue() {}
}
