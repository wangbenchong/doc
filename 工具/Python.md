# 基本语法

Python 是一种广泛使用的高级编程语言，以其简洁易读的语法和强大的功能而著称。以下是 Python 基本语法的简要介绍：

## 变量和数据类型

- **变量**：在 Python 中，变量不需要声明类型，直接赋值即可。

  ```python
  x = 10  
  name = "Alice"
  ```

- **数据类型**：Python 支持多种数据类型，包括整数（int）、浮点数（float）、字符串（str）、布尔值（bool）、列表（list）、元组（tuple）、字典（dict）和集合（set）。

  ```python
  a = 5          # 整数  
  b = 3.14       # 浮点数  
  c = "Hello"    # 字符串  
  d = True       # 布尔值  
  e = [1, 2, 3]  # 列表  
  f = (1, 2, 3)  # 元组  
  g = {"name": "Alice", "age": 25}  # 字典  
  h = {1, 2, 3}  # 集合
  ```

##  控制结构

- **条件语句**：使用 `if`、`elif` 和 `else`。

  ```python
  if x > 5:  
      print("x is greater than 5")  
  elif x == 5:  
      print("x is equal to 5")  
  else:  
      print("x is less than 5")
  ```

- **循环**：使用 `for` 和 `while`。

  ```python
  # for 循环  
  for i in range(5):  
      print(i)  
   
  # while 循环  
  count = 0  
  while count < 5:  
      print(count)  
      count += 1
  ```

## 函数

- **定义函数**：使用 `def` 关键字。

  ```python
  def greet(name):  
      print(f"Hello, {name}!")  
   
  greet("Alice")
  ```

- **返回值**：使用 `return` 语句。

  ```python
  def add(a, b):  
      return a + b  
   
  result = add(3, 4)  
  print(result)  # 输出 7
  ```

## 注释

### 单行注释

1. **使用井号（#）**
   - 在需要注释的行前面加上`#`，`#`后面的内容都会被Python解释器忽略，作为注释处理。
   - 单行注释通常用于解释代码的功能、目的或提供简短说明。

### 多行注释

1. **使用三个单引号（'''）或三个双引号（"""）**
   - 将需要注释的多行代码放在三个单引号或三个双引号之间，Python解释器会忽略这部分内容，作为多行注释处理。
   - 多行注释通常用于提供更详细的说明、文档或临时禁用一段代码。
2. **每行前面加井号（#）**
   - 虽然这不是一种严格意义上的“多行注释”方式，但你也可以通过在每行需要注释的代码前面加上`#`来实现多行注释的效果。
   - 这种方式比较繁琐，因为每行都需要单独添加`#`。

## 类和对象

- 定义类：使用 class关键字。

  ```python
  class Person:  
      def __init__(self, name, age):  
          self.name = name  
          self.age = age  
   
      def greet(self):  
          print(f"Hello, my name is {self.name} and I am {self.age} years old.")  
   
  # 创建对象  
  person = Person("Alice", 25)  
  person.greet()
  ```

## 模块和包

- **导入模块**：使用 `import` 语句。

  ```python
  import math  
  print(math.sqrt(16))  # 输出 4.0
  ```

- **导入特定函数**：

  ```python
  from math import sqrt  
  print(sqrt(16))  # 输出 4.0
  ```

- **自定义模块**：将 Python 文件作为模块导入。

  ```python
  # 在 mymodule.py 中  
  def my_function():  
      print("This is my function.")  
   
  # 在另一个文件中  
  import mymodule  
  mymodule.my_function()  # 输出 "This is my function."
  ```

## 异常处理

- try-except 语句

  ：用于捕获和处理异常。

  ```python
  try:  
      result = 10 / 0  
  except ZeroDivisionError:  
      print("Cannot divide by zero!")
  ```

## 文件操作

- **打开和读取文件**：

  ```python
  with open("example.txt", "r") as file:  
      content = file.read()  
      print(content)
  ```

- **写入文件**：

  ```python
  with open("example.txt", "w") as file:  
      file.write("Hello, World!")
  ```

## 列表推导式和生成器

- **列表推导式**：

  ```python
  squares = [x**2 for x in range(10)]  
  print(squares)  # 输出 [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
  ```

- **生成器**：使用 `yield` 关键字。

  ```python
  def generate_squares(n):  
      for i in range(n):  
          yield i**2  
   
  gen = generate_squares(10)  
  for square in gen:  
      print(square)
  ```

这些是 Python 编程的基本语法和概念。掌握这些基础知识后，你可以进一步学习更高级的特性和库，如面向对象编程、多线程、网络编程等。



# 配置环境

1. **安装 Python**：从 Python 官网下载并安装与操作系统相匹配的 Python 解释器版本。在安装过程中，建议勾选“Add Python to PATH”选项，以便在命令行中直接运行 Python。
2. **验证安装**：安装完成后，可以通过在命令行中输入“python”或“python --version”来验证 Python 是否安装成功。
3. **配置环境变量**：如果在安装过程中没有勾选“Add Python to PATH”选项，则需要手动配置环境变量。这可以通过系统属性中的环境变量设置来完成。
4. **安装 IDE**：选择并安装一个你喜欢的 IDE，如 PyCharm、Visual Studio Code 等。在 IDE 中配置 Python 解释器路径，以便能够运行和调试 Python 代码。

# 运行环境

运行Python代码并不局限于只能通过创建扩展名为`.py`的文件。实际上，Python提供了多种方式来执行代码，包括但不限于以下几种：

1. **Python脚本文件（.py）**：
   这是最常见的方式，通过编写一个包含Python代码的`.py`文件，然后在命令行或终端中运行`python script.py`来执行。
2. **Python交互式解释器**：
   Python自带一个交互式解释器（REPL，Read-Eval-Print Loop），允许你直接在命令行或终端中输入Python代码并立即看到结果。只需在命令行中输入`python`或`python3`（取决于你的系统配置），然后按回车键即可进入。
3. **Jupyter Notebook**：
   Jupyter Notebook是一个基于Web的应用程序，允许你创建和共享包含代码、方程、可视化和文本的文档。你可以在Notebook中逐行运行Python代码，非常适合数据分析和机器学习项目。
4. **集成开发环境（IDE）**：
   像PyCharm、Visual Studio Code等IDE提供了强大的代码编辑、调试和运行功能。你可以在IDE中编写Python代码，并使用IDE提供的工具来运行和调试它。
5. **从其他编程语言中调用Python**：
   你可以使用像C、C++、Java等编程语言中的库或接口来调用Python代码。例如，Python的C API允许C程序嵌入Python解释器并调用Python代码。
6. **内联脚本（在某些应用程序中）**：
   一些应用程序允许你在其内部直接编写和运行Python代码。例如，一些科学计算软件、图像处理软件或文本编辑器可能支持内联Python脚本。
7. **使用Shebang（#!）**：
   在Unix-like系统中，你可以在脚本文件的开头添加一行`#!/usr/bin/env python3`（或指向你系统中Python解释器的具体路径），然后赋予该脚本可执行权限（使用`chmod +x script.py`），之后就可以直接运行该脚本而不需要显式调用`python`命令。
8. **从命令行中直接运行单行Python代码**：
   在某些系统中，你可以使用`-c`选项从命令行中直接运行单行Python代码，例如`python -c "print('Hello, World!')"`。

因此，运行Python代码的方式是多种多样的，并不局限于创建`.py`文件。选择哪种方式取决于你的具体需求和偏好。



# 案例一：计算器

以下是一个简单的Python功能案例，它实现了一个基本的计算器，可以进行加、减、乘、除四种基本运算。

```python
def add(x, y):  
    """返回两个数的和"""  
    return x + y  
  
def subtract(x, y):  
    """返回两个数的差"""  
    return x - y  
  
def multiply(x, y):  
    """返回两个数的积"""  
    return x * y  
  
def divide(x, y):  
    """返回两个数的商，如果除数为0，则返回错误信息"""  
    if y == 0:  
        return "Error! Division by zero."  
    return x / y  
  
def calculator():  
    print("选择运算：")  
    print("1. 加")  
    print("2. 减")  
    print("3. 乘")  
    print("4. 除")  
  
    while True:  
        choice = input("输入你的选择(1/2/3/4): ")  
  
        if choice in ['1', '2', '3', '4']:  
            try:  
                num1 = float(input("输入第一个数字: "))  
                num2 = float(input("输入第二个数字: "))  
            except ValueError:  
                print("输入无效，请输入一个数字。")  
                continue  
  
            if choice == '1':  
                print(f"{num1} + {num2} = {add(num1, num2)}")  
            elif choice == '2':  
                print(f"{num1} - {num2} = {subtract(num1, num2)}")  
            elif choice == '3':  
                print(f"{num1} * {num2} = {multiply(num1, num2)}")  
            elif choice == '4':  
                print(f"{num1} / {num2} = {divide(num1, num2)}")  
        else:  
            print("输入无效，请选择1、2、3或4。")  
  
        next_calculation = input("继续计算吗？(yes/no): ")  
        if next_calculation.lower() != 'yes':  
            break  
  
if __name__ == "__main__":  
    calculator()
```

这个脚本定义了几个函数来执行基本的数学运算，并在一个`calculator`函数中提供了一个简单的命令行界面，让用户可以选择要执行的运算并输入数字。用户可以重复进行运算，直到他们选择不再继续。

要运行这个脚本，只需将其保存为一个`.py`文件（例如`calculator.py`），然后在命令行或终端中运行`python calculator.py`即可。