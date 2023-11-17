# TXT单词表转换成Json词典

## 程序说明

### 程序依赖

1. 程序依赖词典数据库`stardict.db`, 可在 `https://github.com/skywind3000/ECDICT` release里面， 下载 ecdict-sqlite-28.zip 解压得到。请写绝对路径。

2. txt单词表文件， 请写绝对路径。 txt文件里面， 一行一个单词。

3. 所有配置都写在 `appsettings.json` , 直接双击exe文件运行就好。

### 程序功能

1. txt单词文件转换Json词典，给 https://qwerty.kaiyi.cool 使用 。
2. 从txt单词文件(COCA单词表)里面，筛选出所有动词。 然后就可以手动导入欧路词典生词本。


## 一些想法 💡💡💡

### 1. 把Json词典里面的内容，从原来的 单词-翻译， 换成 例句-单词。 这样可以从 单词用法 的角度来学习词汇。 （键盘打字，输入句子 比 输入单词 更考验手速些 ）

问题： 例句从哪里来？
解决方案： 从 mdx词典 里面读取
	

+ 解决方案衍生的问题： 

	- mdx词典里面查出来的内容包含html， 那里面的html标签，简直就是牛鬼蛇神、五花八门。
	- 要针对每一部mdx词典写专门的html解析代码，实操解析起来应该问题不大，就是需要多花时间动手调试。 

+ 使用的编程语言：由于要解析html， 使用python比较合适，有很多的解析库。 

	- mdx词典查询单个词汇： https://github.com/mmjang/mdict-query.git    这个库可以使用，见 test_lzo.py 文件   bd.mdx_lookup('disturb')
	- 吐槽： 头一次见用 Visual Studio 来写python代码的，***是我孤陋寡闻了***（虽然每次装 vs 都顺手勾选了 python开发 这个组件，但是从来没在 vs 写过 python ）。
	- 吐槽： 他喵的，连虚拟环境都不用 ， requirements.txt 文件也没有。



## 待办列表（to do list）

- [ ] 实现想法1