# 语法规则

本仓库唯一指定编辑工具 **Typora** 就是用 js 做的插件。所以有必要了解一些js语法（未列出的参考C语法）

- 行尾不要求有分号

- 等于：===

- 不等于：!==

- 变量类型：const 仅允许声明时赋值

- 变量类型：let 允许声明后再次赋值

- 字符串：'abc' 和 "abc" 都可以

- 正则替换：'abc@def@gh'.replace(/@/g, '你好') 结果是 'abc你好def你好gh'，其中/g表示匹配所有

  ```js
  //方法1：隐式正则指令
  const str = 'abc@def@gh'
  let newStr1 = str.replace(/@/g, '你好')
  console.log(newStr1); // 输出: 'abc你好def你好gh'
  
  //方法2：创建正则指令变量
  let regex = new RegExp('@', 'g') // 创建全局匹配反斜杠的正则表达式
  let newStr2 = str.replace(regex, '你好')
  console.log(newStr2); // 输出: 'abc你好def你好gh'
  ```

  

- 模板字符串：使用反引号

  ```js
  const name = "Alice";
  const str = `Hello, ${name}!`;
  ```

- 反斜杠相关：

  ```js
  //将反斜杠替换成斜杠的N种方法
  const str = 'E:\\Folder\\file.txt' //反斜杠的转义效果和C语言一样
  const str1 = str.split('\\').join('/')
  const str2 = str.split(String.fromCharCode(92)).join('/')
  const str3 = str.replace(/\\/g, '/')
  //最终都变成：'E:/Folder/file.txt'
  ```

  

- 待续...

