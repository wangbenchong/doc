#  说明

这个 git 仓库主要存储 markdown 文章，对于作者来说是千金不换的永久知识笔记。

##  如何编辑本仓库

**【Typora】**本仓库**唯一指定**编辑工具，一款让你在**编辑文档舒适区越陷越深**的软件：

-  [Typora安装与使用技巧](./工具/Typora安装与使用技巧.md)



## 如何多端查看本仓库

- PC端：Typora软件

- 安卓端：[Obsidian和MGit](./工具/Obsidian和MGit.md)

- 网页端：[优先Gitee有插图，其次Github](#仓库地址)

  

## 目录规则

```c
|——功能分类1
|		|————无图文档1.md
|		|————无图文档2.md
|		|————有图文档3
|		|		|——————有图文档3.md
|		|		|——————img
|		|				|——————图片a.jpg
|		|				|——————图片b.jpg
|		|————有图文档4
|				|——————有图文档4.md
|				|——————img
|						|——————图片a.jpg
|						|——————图片b.jpg
|——功能分类2
|		|————无图文档5.md
|		|————无图文档6.md
|		|————有图文档7
|		|		|——————有图文档7.md
|		|		|——————img
|		|				|——————图片a.jpg
```

- 根目录文件夹是大分类，比如：工具、数学、渲染；子目录中存放md文档
- 如果需要给文档做插图，那就再做子文件夹把文档放进去，并在里面建img文件夹来存放图片
- 允许多篇md文档使用同一个img图片文件夹，前提是这些md文档应该属于**同一类型的知识**
- 插图必须是[极致压缩](./工具/图片压缩及markdown文档插图规范.md)过的，禁止浪费仓库空间
- 允许子文件夹集中存放代码文件，但这个文件夹根目录一定要有ReadMe.md，并符合[相关规范](./工具/C#工具集/C#_以markdown形式列出所有cs文件.md)



## 仓库地址

gitee网页：https://gitee.com/wangbenchong/doc

gitee仓库地址：https://gitee.com/wangbenchong/doc.git

svn仓库：svn://git.oschina.net/wangbenchong/doc

----------两者不定期手动镜像，优先国内---------------

手动镜像方法：[文件夹软链接+拷贝文件](./工具/批处理技巧.md)

github网页：https://github.com/wangbenchong/doc

github仓库地址：https://github.com/wangbenchong/doc.git

