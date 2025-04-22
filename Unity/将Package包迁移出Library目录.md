> 参考文章：
>
> - [Unity - 如何修改一个 Package 或是如何将 Package Local 化_unity 修改 package 文件夹-CSDN 博客](https://blog.csdn.net/linjf520/article/details/125738218)
> - [一起成为偷库大盗吧，Unity Packages 的使用略谈 - 知乎](https://zhuanlan.zhihu.com/p/335580913)

我们平时从 PackageManager 下载的包默认都会存在于 Library 目录（具体来说在 PackageCache 子目录）下，这个目录通常是不被 git 仓库管理的（避免浪费仓库空间），并且这个目录下的文件不适合手动修改，因为 Unity 会在一些情况（如更新包）时自动覆盖掉这些修改。所以基于以上这些原因，团队开发过程中需要把某些第三方的包从 Library 中迁移出来，接受 git 仓库的管理，便于同步；而在个人开发过程中，也会遇到需要迁移的情况，来对这些包做私人定制。

迁移出来放在哪儿呢？Unity 为我们准备了现成的与 Library 同级的 **Packages** 目录。只需要把 Library 目录下的包文件夹剪切到 Packages 文件夹，甚至还可以重新修改文件夹的名字。然后 PackageManager 会显示这个包是带 `Custom` 标的（如果是通过 Git 链接安装的那么迁移之前显示的是 `Git` 标），并取消了 Update 按钮让这个包不会随外界版本更新，此时你可以随便修改这个包里的代码而不必担心被覆盖。整个过程不需要修改任何json配置文件。

当然，如果你是从头做起创建一个包的话，就得会改 Packages 目录下的 manifest.json 文件，以及包文件夹下的  package.json 文件了。

如果你从其他地方拷贝的包（不是从自己的Library而来），安装方式： PackageManager +号展开 > Install package from disk > 选择包里的 package.json
