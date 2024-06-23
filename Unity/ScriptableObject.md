```c
using UnityEngine;
[CreateAssetMenu(menuName= "Script Objects/Sample")]
public class SamepleScriptableObject : ScriptableObject
{
    private void Awake(){}
    private void OnEnable(){}
    privaet void OnDisable(){}
    private void OnDestroy(){}
}
```

好处：

- 可在运行时修改
- 分拆数据防止prefab过大，减少同时编辑prefab的情况