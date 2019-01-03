# CommonLib.NET

常用的C#类库整理

## File

文件操作帮助类 

### FileHelper 类
通用文件操作类
#### 方法
* `GetSuffix(string path)` 根据文件路径获取后缀名

	| 参数       | 描述               |
	| ---------- | ------------------ |
	| path| 文件路径 |
* `CreateDirectory(string path, bool isDirectory = false)` 根据文件路径创建目录 

	| 参数       | 描述               |
	| ---------- | ------------------ |
	| path| 文件路径 |
	| isDirectory | 是否目录,默认否|

### TxtHelper 类
Txt文件操作类
#### 方法
* `Read(string path)` 读取 Txt 数据

	| 参数       | 描述               |
	| ---------- | ------------------ |
	| path| 文件路径 |
* `Read(string path, System.Text.Encoding encoding)` 读取 Txt 数据

	| 参数       | 描述               |
	| ---------- | ------------------ |
	| path| 文件路径 |
	| encoding| 编码格式 |
* `Write(string path, string content, bool append = false)` 数据写入 Txt 文件 

	| 参数       | 描述               |
	| ---------- | ------------------ |
	| path| 文件路径 |
	| content | 要写入的数据|
	| append | 是否追加|
* `Write(string path, string content, bool append, System.Text.Encoding encoding)` 数据写入 Txt 文件 

	| 参数       | 描述               |
	| ---------- | ------------------ |
	| path| 文件路径 |
	| content | 要写入的数据|
	| append | 是否追加|
	| encoding | 编码格式|
