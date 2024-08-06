Burst通常是DOTS技术中的一环，但是也可独立使用，用来将部分代码编译为令CPU更高效执行的本地机器码。

Burst1.5以后支持Direct Call

```c#
[BurstCompile]
public class MyClass
{
  [BurstCompile]
  public static float DoSomething(float f) => math.sin(f);
 
  public static float SomeManagedCode(float f)
  {
    return DoSomething(f);
  }
}
```

不使用Direct Call的写法也是可以的

```c#
[BurstCompile]
public class MyClass
{
  [BurstCompile]
  public static float DoSomething(float f) => math.sin(f);
    
  public static float SomeManagedCode(float f)
  {
    var funcPtr = BurstCompiler.CompileFunctionPointer(DoSomething);
    return funcPtr.Invoke(f);
  }
}
```

使用Job的例子

```c#
using UnityEngine;
using Unity.Collections;
public class JobsBurstTest : MonoBehaviour
{
    [SerializeField]private int count = 10000;
    [SerializeField]private bool UseJob;
    void Update()
    {
        //使用Job，count为1000时，耗时32毫秒，count为10000时，耗时1951毫秒，开了[BurstCompile]耗时13毫秒
        if(UseJob)
        {
            NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(count, Allocator.Temp);
            for(int i=0;i<count;i++)
            {
                var job = new ComputerJob();
                job.c = this.count;
                var handler = job.Schedule();
                jobHandles[i] = handler;
            }
            jobHandles.Dispose();
        }
        //不使用Job，count为1000时耗时96毫秒
        else
        {
            for(int i=0;i<count;i++)
            {
                for(int j=0;j<count;j++)
                {
                    ComputerTast();
                }
            }
        }
    }
    public static void ComputerTask（）
    {
        var a = Mathf.Sqrt(Mathf.Pow(Mathf.PI, Mathf.PI));//多线程里不能用Random
    }
    [BurstCompile]//写了这句会有性能大提升（使用job的基础上又使用了Burst）,需要同时在unity菜单栏jobs/Burst/Enable Burst勾选
    public struct ComputerJob: IJob
    {
        public int c;
        public void Execute()
        {
            for(int i=0;i<c;i++)
            {
                ComputerTask();
            }
        }
    }
}

```

