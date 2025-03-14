# 关于lua的表和usertable

Lua是一种轻量级的、嵌入式的脚本语言，广泛用于配置、脚本化以及快速开发等领域。Lua的表（table）是其最强大的特性之一，也是Lua中唯一的数据结构，用来表示数组、集合、记录以及其他数据结构。

## Lua的表（Table）

Lua的表是一种关联数组，可以用作数组（通过数字索引）或字典（通过字符串键）。表在Lua中是动态的，可以随时增长和缩减。

### 基本特性

1. 键和值：
   - 表中的每个元素都是一个键值对。
   - 键可以是任何类型，但通常是数字和字符串。
   - 值也可以是任何类型，包括nil、数字、字符串、表、函数等。
2. 数组功能：
   - 通过数字索引（从1开始）访问表元素，类似于数组。
   - Lua的表没有固定的大小限制，可以动态增长和缩减。
3. 字典功能：
   - 通过字符串键访问表元素，类似于字典或哈希表。
   - 可以使用任意字符串作为键。

### 示例

```lua
-- 创建一个表
myTable = {}
 
-- 作为数组使用
myTable[1] = "apple"
myTable[2] = "banana"
print(myTable[1])  -- 输出: apple
 
-- 作为字典使用
myTable["color"] = "red"
print(myTable["color"])  -- 输出: red
```

## Userdata和Usertable

在Lua中，userdata是一种特殊的类型，用于表示C语言中的任意类型的数据。userdata允许Lua和C代码之间共享数据。

### Userdata

- **定义**：Userdata是一个指向C内存中某块数据的指针。
- **用途**：通常用于将C语言的数据结构传递给Lua脚本，或者从Lua脚本中访问C语言的数据结构。
- **轻量级userdata**和**完整userdata**：轻量级userdata是一个简单的指针，没有元表（metatable）；完整userdata则包含一个指向数据的指针和一个指向元表的指针。

### Usertable（元表）

- **定义**：元表（metatable）是一个普通的Lua表，用于定义userdata的行为。
- **用途**：通过为userdata设置元表，可以定义userdata在特定操作（如加法、减法、索引访问等）时的行为。
- **示例**：可以为userdata定义`__add`元方法，从而自定义userdata相加时的行为。

### 示例

```lua
-- 创建一个userdata
local myUserdata = {}
setmetatable(myUserdata, {
    __add = function(a, b)
        -- 自定义加法行为
        return a.value + b.value
    end
})
 
-- 设置userdata的值
myUserdata.value = 10
 
-- 创建另一个userdata
local anotherUserdata = {}
setmetatable(anotherUserdata, getmetatable(myUserdata))  -- 共享元表
anotherUserdata.value = 20
 
-- 使用自定义的加法
print(myUserdata + anotherUserdata)  -- 输出: 30
```

在这个示例中，我们创建了两个userdata对象，并共享一个元表。通过定义`__add`元方法，我们自定义了这两个userdata相加时的行为。

## 总结

- Lua的表是一种灵活且强大的数据结构，可以用来表示数组、字典等。
- Userdata用于在Lua和C代码之间共享数据，通过元表可以定义userdata的行为。
- 通过合理使用表和userdata，Lua可以高效地处理各种复杂的数据结构和操作。

# 关于Lua的元表和面向对象

Lua是一种轻量级、高效的脚本语言，被广泛应用于游戏开发、嵌入式系统、网络应用等领域。在Lua中，元表和面向对象编程是两个重要的概念，下面分别进行介绍：

## Lua的元表（Metatable）

### **定义**

- 元表是Lua中一种用于定义或修改表（table）在特定操作下的行为的机制。

### **元方法（Metamethods）**

- 元表中的字段（通常以双下划线“__”开头）被称为元方法。
- 元方法允许开发者自定义表在Lua中的行为，例如算术运算、比较操作、字符串连接等。

### **常见元方法及其用途**

- **算术类元方法**：如`__add`、`__sub`、`__mul`、`__div`、`__mod`、`__pow`、`__unm`，用于定义两个表或数值之间的算术运算行为。
- **连接类元方法**：如`__concat`，用于定义连接两个表或字符串的操作。
- **长度类元方法**：如`__len`，用于定义获取表长度的操作。
- **比较类元方法**：如`__eq`、`__lt`、`__le`，用于定义等于、小于、小于等于等操作的行为。
- **索引类元方法**：如`__index`，当尝试访问表中不存在的键时，Lua会调用此方法。
- **新索引类元方法**：如`__newindex`，当尝试向表中不存在的键赋值时，Lua会调用此方法。
- **调用类元方法**：如`__call`，允许表像函数一样被调用。
- **字符串表示类元方法**：如`__tostring`，定义将表转换为字符串的行为。
- **元表保护类元方法**：如`__metatable`，用于保护元表，使其不能被外部访问或修改。

### **设置和获取元表**

- 使用`setmetatable(table, metatable)`函数为表设置元表。
- 使用`getmetatable(table)`函数获取表的元表。

### 元方法__concat示例

在Lua中，`__concat` 元方法允许你自定义两个表（或userdata，如果它们有元表且元表中定义了`__concat`）使用连接操作符（`..`）进行连接时的行为。以下是一个简单的示例，展示了如何为Lua中的表定义自定义的`__concat`元方法。

```lua
-- 定义两个表，它们将用于连接操作
local table1 = { value = "Hello, " }
local table2 = { value = "World!" }
 
-- 为这两个表设置相同的元表
local metatable = {
    __concat = function(a, b)
        -- 这里我们假设表a和b都有一个名为'value'的字段
        -- 并且我们将这两个字段的值连接起来
        return a.value .. b.value
    end
}
 
setmetatable(table1, metatable)
setmetatable(table2, metatable)
 
-- 使用自定义的连接操作
local result = table1 .. table2
 
-- 输出结果
print(result)  -- 输出: Hello, World!
```

然而，需要注意的是，上面的代码实际上并不是Lua中`__concat`元方法的典型用法，因为通常我们不会直接对表使用连接操作符。`__concat`元方法更常见于userdata或与字符串操作紧密相关的自定义类型上。

下面是一个更贴近实际用途的示例，展示了如何为userdata定义`__concat`元方法：

```lua
-- 创建一个简单的userdata类型（在Lua中，我们通常通过C语言扩展来创建userdata，但这里为了简化，我们使用一个表来模拟）
local userdata1 = { str = "Hello, " }
local userdata2 = { str = "Lua!" }
 
-- 定义一个元表，其中包含__concat元方法
local metatable = {
    __concat = function(a, b)
        -- 假设a和b都是包含'str'字段的表（模拟userdata）
        return a.str .. b.str
    end
}
 
-- 为这两个表设置元表（在真实的userdata场景中，这一步通常由C代码完成）
setmetatable(userdata1, metatable)
setmetatable(userdata2, metatable)
 
-- 由于Lua不允许直接对表使用__concat（除非它们被当作userdata处理，这通常通过C扩展实现），
-- 我们这里通过一个技巧来调用元方法：使用getmetatable和rawget来直接访问元方法并调用它。
-- 但在实际使用中，如果你正在处理真正的userdata，并且这些userdata的元表中定义了__concat，
-- 那么当你使用..操作符连接它们时，Lua会自动调用这个元方法。
-- 下面的代码仅用于演示如何手动调用元方法；在实际应用中，你应该直接使用..操作符。
local result = rawget(metatable, "__concat")(userdata1, userdata2)
 
-- 输出结果
print(result)  -- 输出: Hello, Lua!
```

然而，需要强调的是，在纯Lua代码中，你通常不会直接对表使用`__concat`元方法，因为表不是用来表示字符串的。`__concat`元方法主要用于自定义类型的userdata，这些类型在C扩展中定义，并且与Lua的字符串类型进行交互。在纯Lua代码中，你更可能会直接使用字符串连接操作符`..`来处理字符串。

如果你正在编写一个Lua C扩展，并且想要为你的自定义userdata类型定义`__concat`行为，你需要在C代码中设置该userdata的元表，并在元表中定义`__concat`函数。这样，当Lua脚本尝试使用`..`操作符连接你的userdata对象时，Lua解释器会自动调用你定义的`__concat`函数。

## Lua的面向对象编程

1. **面向对象编程概述**：

   - 面向对象编程（OOP）是一种编程范式，它使用“对象”来设计软件。对象是类的实例，每个对象都有独立的内存区域，并包含状态（成员变量）和行为（方法）。

2. **Lua中的面向对象实现**：

   - Lua本身不是一种纯粹的面向对象语言，但它支持面向对象编程的特性。
   - 在Lua中，可以使用表（table）来模拟对象，并通过表的方法来实现面向对象的编程。
   - 通过给表添加字段和方法，可以创建对象并调用其方法。

3. **封装、继承和多态**：

   - **封装**：通过将数据和相关的方法封装在表中，实现对数据的保护和访问控制。
   - **继承**：通过设置元表的`__index`字段，实现对父类的继承。这样，子类可以访问父类中的方法和变量。
   - **多态**：通过设置元表的元方法，可以实现对不同对象的不同行为。多态允许使用统一的接口来调用不同对象的方法，而这些方法的具体实现可能因对象而异。

4. **示例**：

   - 定义一个基类`Shape`，包含`area`属性和`new`、`printArea`方法。

   - 从`Shape`类扩展出`Rectangle`类，并添加`length`和`breadth`属性以及相应的计算方法。

   - 通过设置元表和元方法，实现`Rectangle`类对`Shape`类的继承和多态。

 ```lua
 -- 定义一个基类 Shape
 Shape = {}

 -- Shape 的构造函数
 function Shape:new(area)
     local obj = {}    -- 创建一个新表作为对象
     self.__index = self -- 设置元表的 __index 字段指向自己，用于继承
     setmetatable(obj, self) -- 将 Shape 设置为 obj 的元表
     obj.area = area or 0 -- 初始化 area 属性
     return obj
 end

 -- Shape 的方法
 function Shape:printArea()
     print("Area:", self.area)
 end

 -- 定义一个子类 Rectangle，继承自 Shape
 Rectangle = {}

 -- Rectangle 的构造函数
 function Rectangle:new(length, breadth)
     local obj = Shape:new() -- 调用 Shape 的构造函数来初始化 obj
     self.__index = self -- 设置 Rectangle 的 __index 字段指向自己
     setmetatable(obj, self) -- 将 Rectangle 设置为 obj 的元表

     -- 由于 Rectangle 继承了 Shape，我们需要在 Rectangle 的元表的 __index 字段中
     -- 加入对 Shape 的引用，以便访问 Shape 的方法和属性。但在这个例子中，由于我们
     -- 在 Rectangle 的构造函数中直接调用了 Shape:new()，并且 setmetatable(obj, Shape)
     -- 会被 Shape:new() 内部的 setmetatable(obj, self) 覆盖，所以我们只需要确保
     -- Rectangle 的元表的 __index 字段最终指向 Rectangle 自身即可（如果 Rectangle
     -- 有自己独有的方法或属性需要隐藏的话）。然而，由于我们打算通过 Rectangle 的
     -- 实例直接访问 Shape 的方法（如 printArea），并且 Rectangle 本身没有重写这些方法，
     -- 所以我们实际上不需要显式设置 __index 指向 Shape，因为 Lua 会自动沿着元表链查找。
     -- 但为了清晰起见，这里我们还是手动设置一下（虽然在这个特定例子中是多余的）。
     -- 正确的做法是在 Rectangle 的外部设置一个元表，其 __index 指向 Shape，但这样
     -- 做的话，我们需要在 Rectangle:new() 外部调用 setmetatable，而不是在内部。
     -- 为了简化，我们在这里省略这一步，并依靠 Shape:new() 设置的元表。

     -- 初始化 Rectangle 独有的属性
     obj.length = length or 0
     obj.breadth = breadth or 0

     -- 计算并设置 area 属性
     obj.area = length * breadth

     return obj
 end

 -- 注意：在这个例子中，我们没有为 Rectangle 重写 printArea 方法，
 -- 所以它会继承并使用 Shape 的 printArea 方法。

 -- 创建 Shape 和 Rectangle 的实例，并调用它们的方法
 shapeInstance = Shape:new(100)
 shapeInstance:printArea() -- 输出: Area: 100

 rectInstance = Rectangle:new(10, 5)
 rectInstance:printArea() -- 输出: Area: 50
 ```

​     然而，上面的代码有一个问题：`Rectangle`的元表设置其实是不必要的，因为我们已经在`Rectangle:new`方法中调用了`Shape:new`，后者已经设置了对象的元表。此外，由于`Rectangle`没有重写`Shape`的任何方法，所以`__index`字段的设置在这个例子中是多余的（Lua会沿着元表链自动查找方法）。

为了更清晰地展示继承，我们可以稍微修改代码，使`Rectangle`明确地从`Shape`继承，而不需要在`Rectangle:new`内部调用`Shape:new`并重新设置元表。这通常是通过在`Rectangle`外部设置元表来实现的，如下所示：

```lua
-- ...（Shape 的定义和构造函数与之前相同）
 
-- Rectangle 继承自 Shape
setmetatable(Rectangle, {__index = Shape})
 
-- Rectangle 的构造函数（简化版）
function Rectangle:new(length, breadth)
    local obj = {} -- 创建一个新表作为对象
    setmetatable(obj, self) -- 设置 Rectangle 为 obj 的元表
    
    -- 初始化 Rectangle 独有的属性
    obj.length = length or 0
    obj.breadth = breadth or 0
    
    -- 调用 Shape 的构造函数来设置 area（这里需要稍微修改 Shape:new 以接受 self 作为参数）
    -- 但由于 Lua 不支持真正的类构造函数重载，我们通常会在 Rectangle 中直接计算 area
    obj.area = length * breadth
    
    return obj
end
 
-- 注意：在这个修改后的例子中，Shape:new 并没有设计为被子类直接调用，
-- 所以我们需要在 Rectangle 中直接设置 area。如果 Shape:new 需要被重用，
-- 我们可以将其重构为接受一个额外的 self 参数（这通常不是 Lua 中推荐的做法）。
-- 更好的做法是使用一个工厂函数或者一个单独的初始化函数来处理这种情况。
 
-- ...（创建实例和调用方法的代码与之前相同）
```

但是，上面的修改有一个问题：它打破了`Shape:new`的封装，因为它假设`Shape:new`可以被直接以这种方式调用，而这在原始的`Shape`定义中并不是这样的。在Lua中，更常见的做法是使用工厂函数或者初始化函数来避免直接调用构造函数的复杂性。然而，为了保持示例的简单性，并且展示如何通过元表实现继承，我们可以回到第一个示例，并接受`Rectangle:new`内部调用`Shape:new`的事实。在实际应用中，你可能需要根据你的具体需求来调整这些设计决策。

综上所述，Lua的元表和面向对象编程为开发者提供了强大的工具来组织和管理代码，提高代码的可重用性和可维护性。通过合理使用这些特性，可以创建出功能更加丰富和灵活的Lua程序。

# Lua里or 0的意思

在Lua中，`or` 是一个逻辑运算符，用于在两个值之间进行逻辑或（OR）操作。然而，Lua中的`or`运算符有一个特别有用的特性：当它的第一个操作数为`false`或`nil`时，它会返回第二个操作数，否则返回第一个操作数。

因此，表达式 `x or 0` 的意思是：如果`x`是`nil`或`false`（在Lua中，`false`和`nil`被视为假值，而其他所有值都被视为真值），则表达式的结果为`0`；否则，结果为`x`的值。

这种写法经常用于为变量提供默认值。例如：

```lua
lua复制代码

local length = someFunctionThatMightReturnNil() or 10
```

在这个例子中，如果`someFunctionThatMightReturnNil()`返回`nil`，则`length`将被设置为`10`。如果函数返回了其他任何值（比如一个数字、字符串、表等），则`length`将被设置为该返回值。

这种技术非常有用，因为它允许开发者在不确定一个变量是否已被赋值的情况下，为其提供一个合理的默认值，从而避免在后续的代码中出现`nil`值错误。



# Lua代码优化

本文总结一下编写lua代码时能够有效提高代码运行效率的要点

## 不要频繁插入小表

```lua
local a={}
for i=1,100 do
	a[i]=true
end
```

上述代码会进行8次rehash过程。由于lua中table和字符串表容量都以2的指数扩张，所以即便是100万元素的表，只会进行20次rehash过程（因为(1«20)>1,000,000），所以消耗比较大的是那些小表的频繁操作。通过预先填充解决这个问题。将

```lua
a=os.clock()
for i=1,200000 do
    local a={}
    a[1]=1;a[2]=1;a[3]=1;
end
b=os.clock()
print(b-a)
```

修改为

```lua
a=os.clock()
for i=1,200000 do
	local a={0,0,0}
	a[1]=1;a[2]=1;a[3]=1;
end
b=os.clock()
print(b-a)
```

比如在构建Vec3时，通过local v= {0,0,0}预先给v三个大小的容量，后面依次设置值的时候不会发生rehash过程

## 不要混用表的数组和散列桶部分

lua的table既可以作为字典，也可以作为数组使用，为了保证访问的效率和代码正确率，不要在一个表中混用这两种存储方式。要么以数组保存，要么以哈希表保存。

 

# 面试题库

[折了无数选手的腾讯lua笔试题，你来试试？ - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5e33a4daedbc2a7a7ccef8af)

[腾讯最难Lua笔试题参考答案 - 技术专栏 - Unity官方开发者社区](https://developer.unity.cn/projects/5e3d0d9aedbc2a001feed27e)



# XLua

