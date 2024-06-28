# Runtime Inspector & Hierarchy

可以在Scene窗口下，运行时，显示Inspector & Hierarchy。

使用时放到场景里的一个不销毁Canvas下，彼此关联一下，锚到屏幕左上，搞个按钮切换显隐，然后写个C#脚本控制显隐，像这样：

```c#
private void SetActive(bool active)
{
    RectTransform rectTransform = GetComponent<RectTransform>();
    if (active)
    {
        rectTransform.anchoredPosition = new Vector2(Show_X, -50);
    }
    else
    {
        rectTransform.anchoredPosition = new Vector2(Hide_X, -50);
    }
}
```



相关链接：

- Forum Thread (https://forum.unity.com/threads/runtime-inspector-and-hierarchy-open-source.501220/) 
- GitHub Page (https://github.com/yasirkula/UnityRuntimeInspector) 
- Documentation (https://github.com/yasirkula/UnityRuntimeInspector) 
- Demo (https://yasirkula.net/DynamicPanelsDemo/)
- Discord (https://discord.gg/UJJt549AaV)