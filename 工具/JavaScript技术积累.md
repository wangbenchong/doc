# 语法规则

本仓库唯一指定编辑工具 **Typora** 就是用 js 做的插件。所以有必要了解一些js语法（未列出的参考C语法）

- 行尾不要求有分号

- 等于：===

- 不等于：!==

- 变量类型：const 仅允许声明时赋值

- 变量类型：let 允许声明后再次赋值

- 字符串：'abc' 和 "abc" 都可以

- 正则替换：'abc@def@gh'.replace(/@/g, '你好') 结果是 'abc你好def你好gh'，其中/g表示匹配所有

- 模板字符串：使用反引号

  ```js
  const name = "Alice";
  const str = `Hello, ${name}!`;
  ```

- 其他

