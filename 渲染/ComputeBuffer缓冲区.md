# Compute Buffer

SM4.5/Compute Shader

Shader Model不低于4.5，硬件版本要求相对新

功能：代替数组，长度不受限(而数组长度上限为1024)

Shader代码：

```c
struct Example
{
    float3 A;//shader里没有关键字public，默认就是公有
    float B;
};
StructuredBuffer<Example> _BufferExample;//类似于C#里的 List<>长度可以很大
void GetBufferValue(float Index, out float3 OutF)
{
    OutF = _BufferExample[Index].A;
}
Example GetBufferElement(float Index)
{
    return _BufferExample[Index];
}
```

C#传参：

```c#
[ExecuteAlways]
public class BufferTest : MonoBehaviour
{
    private ComputeBuffer buffer;
    private struct Test//和Shader的struct结构保持一致（包括成员变量名）
    {
        public Vector3 A;//保持公有，这样shader那边才能访问到
        public float B;
    }
    private void OnEnable()
    {
        Test test = new Test
        {
            //如果需要生成随机Vector3，可以用Random.insideUnitSphere,即返回半径为 1 的球体内的随机点
            A = new Vector3(0, 0.5f, 0.5f),
            B = 0.1f
        };
        Test test2 = new Test
        {
            A = new Vector3(0.5f, 0.5f, 0),
            B = 0.1f
        };
        //Test[] 对应 shader中的 StructuredBuffer<Example>
        Test[] data = new Test[] {test, test2};
        buffer = new ComputeBuffer(data.Length, sizeof(float)*4);
        buffer.SetData(data);
        GetComponent<MeshRenderer>().shareMaterial.SetBuffer("_BufferExample", buffer);
    }
    private void OnDisable()
    {
        buffer.Dispose();
    }
}
```

