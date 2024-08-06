# internal关键字与程序集

类似于public， 可以修饰类或成员，区别在于它所修饰的类只能在同一个程序集(即项目)中被访问。不过其他程序集依然可以通过子类继承访问。

```mermaid
graph TB
subgraph 解决方案N
程序集A --> dll
程序集B --> exe
end
```

