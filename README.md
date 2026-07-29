# Llama.cpp Server Loader

一个用于加载和配置 llama.cpp 服务器的 Windows 桌面应用程序，提供图形化界面来管理 llama.cpp 的各项参数。
<img width="1507" height="990" alt="image" src="https://github.com/user-attachments/assets/ebedced3-bac9-4f79-b1bd-8ada25500639" />

## 功能特性

- 🖥️ **图形化界面**：直观的 WPF 界面，无需命令行操作
- ⚙️ **完整参数配置**：支持 llama.cpp 的所有主要参数
- 💾 **配置文件管理**：保存和加载多个配置文件
- 📊 **实时控制台输出**：查看服务器运行状态和日志
- 🚀 **快速启动**：一键启动/停止服务器
- 🔔 **版本检测**：自动检测本地版本并提示更新（v1.1.0 新增）
- 🌐 **GitHub 集成**：一键跳转到最新版本下载页面（v1.1.0 新增）

## 支持的参数

### 服务器配置
- llama-server.exe 路径
- 模型文件路径（.gguf）
- 端口号
- Jinja 模板支持

### 上下文配置
- Context Size（上下文大小）
- Batch Size（批处理大小）
- UBatch Size（微批处理大小）
- Flash Attention（闪存注意力机制）
- Cache Type K/V（KV 缓存类型：f16, q8_0, q4_0）

### 采样参数
- Temperature（温度）
- Top P（核采样）
- Top K（K采样）
- Min P（最小P采样）
- Repeat Penalty（重复惩罚）
- Repeat Last N（重复检测窗口）
- Frequency Penalty（频率惩罚）
- Presence Penalty（存在惩罚）

## 系统要求

- Windows 10/11
- .NET 8.0 Runtime
- llama.cpp 服务器程序（llama-server.exe）

## 安装

### 从源码构建

1. 确保已安装 .NET 8.0 SDK
2. 克隆或下载本项目
3. 在项目目录打开命令行，运行：

```bash
dotnet build -c Release
```

4. 生成的可执行文件位于：`bin\Release\net8.0-windows\LlamaCppLoader.exe`

### 发布为单文件应用

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

发布后的文件位于：`bin\Release\net8.0-windows\win-x64\publish\`

## 使用方法

### 1. 首次配置

1. 启动程序
2. 点击 **llama-server.exe Path** 旁的 **Browse...** 按钮，选择 llama-server.exe 的位置
3. 点击 **Model Path** 旁的 **Browse...** 按钮，选择要加载的 .gguf 模型文件
4. 根据需要调整其他参数

### 2. 保存配置文件

1. 配置好所有参数后，在 **Profile Management** 区域点击 **Save Profile**
2. 输入配置文件名称（例如：Qwen3.6-35B）
3. 点击 Save

### 3. 启动服务器

1. 确认所有配置正确
2. 点击底部的 **Start Server** 按钮
3. 在控制台输出区域查看服务器启动状态
4. 启动成功后，可以通过 `http://localhost:8080`（或你设置的端口）访问服务器

### 4. 停止服务器

点击 **Stop Server** 按钮即可停止运行中的服务器

### 5. 加载已保存的配置

1. 在 **Profile Management** 区域的下拉菜单中选择已保存的配置文件
2. 配置会自动加载到界面

### 6. 删除配置文件

1. 在下拉菜单中选择要删除的配置
2. 点击 **Delete Profile**
3. 确认删除

## 默认配置示例

程序预设了一组推荐配置：

```
Context Size: 85000
Batch Size: 1024
UBatch Size: 256
Flash Attention: 启用
Cache Type K: q8_0
Cache Type V: q8_0
Temperature: 0.40
Top P: 0.88
Top K: 25
Min P: 0.05
Repeat Penalty: 1.05
Repeat Last N: 512
Frequency Penalty: 0.10
Presence Penalty: 0
Port: 8080
Jinja: 启用
```

## 配置文件存储位置

配置文件保存在：
```
%APPDATA%\LlamaCppLoader\profiles.json
```

## 常见问题

### Q: 启动失败显示 "llama-server.exe not found"
A: 请确保 llama-server.exe 路径正确，并且文件确实存在。

### Q: 服务器启动但无法访问
A: 检查防火墙设置，确保端口未被占用，可以尝试更换端口号。

### Q: 模型加载失败
A: 确认模型文件是 .gguf 格式，路径中没有特殊字符，并且模型文件完整未损坏。

### Q: 如何查看详细错误信息
A: 所有输出和错误信息都会显示在程序下方的控制台输出区域。

## 技术栈

- .NET 8.0
- WPF (Windows Presentation Foundation)
- C#
- Newtonsoft.Json

## 更新日志

### Version 1.0.0
- 初始版本发布
- 支持所有主要 llama.cpp 参数
- 配置文件管理功能
- 实时控制台输出

## 许可证

本项目采用 MIT 许可证。

## 贡献

欢迎提交 Issue 和 Pull Request！

## 相关链接

- [llama.cpp 项目](https://github.com/ggerganov/llama.cpp)
- [.NET 下载](https://dotnet.microsoft.com/download)
