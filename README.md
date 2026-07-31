# Llama.cpp Server Loader

一个用于加载和配置 llama.cpp 服务器的 Windows 桌面应用程序，提供图形化界面来管理 llama.cpp 的各项参数。
<img width="1507" height="990" alt="image" src="https://github.com/user-attachments/assets/ebedced3-bac9-4f79-b1bd-8ada25500639" />

[![Release](https://img.shields.io/github/v/release/bakeshame/llama.cpp-loader-win)](https://github.com/bakeshame/llama.cpp-loader-win/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/bakeshame/llama.cpp-loader-win/total)](https://github.com/bakeshame/llama.cpp-loader-win/releases)
[![License](https://img.shields.io/github/license/bakeshame/llama.cpp-loader-win)](LICENSE)

## 功能特性

- 🖥️ **图形化界面**：直观的 WPF 界面，无需命令行操作
- ⚙️ **完整参数配置**：支持 llama.cpp 的所有主要参数
- 💾 **配置文件管理**：保存和加载多个配置文件
- 📊 **实时控制台输出**：查看服务器运行状态和日志
- 🚀 **快速启动**：一键启动/停止服务器
- 🔔 **版本检测**：自动检测本地版本并提示更新
- 🌐 **GitHub 集成**：一键跳转到最新版本下载页面
- 🔐 **API Key 认证**：支持配置 API 密钥验证
- ⚡ **并发支持**：可配置并发槽位数量
- 🧠 **推理保留**：支持保留模型的内部推理过程
- 🖱️ **拖放支持** ⭐新：直接拖放文件到输入框
- 📂 **最近使用** ⭐新：自动记录最近使用的文件
- 🎯 **参数预设** ⭐新：5种场景化预设，一键应用

## 快速开始

### 下载使用

1. 前往 [Releases 页面](https://github.com/bakeshame/llama.cpp-loader-win/releases/latest)
2. 下载 `LlamaCppLoader.exe`（单文件，约 69 MB）
3. 直接运行，无需安装任何依赖

### 基本使用

#### 方法 1：拖放文件（推荐） 🆕
1. **拖放服务器**：从文件管理器拖动 `llama-server.exe` 到服务器路径框
2. **拖放模型**：拖动 `.gguf` 模型文件到模型路径框
3. **应用预设**：选择合适的场景预设（CTF/对话/代码生成等）
4. **启动服务器**：点击 "Start Server" 按钮

#### 方法 2：传统方式
1. **选择服务器路径**：点击 Browse 选择 `llama-server.exe`
2. **选择模型文件**：点击 Browse 选择 `.gguf` 模型文件
3. **配置参数**：根据需要调整上下文大小、采样参数等
4. **启动服务器**：点击 "Start Server" 按钮
5. **访问服务器**：浏览器打开 `http://localhost:8080`

💡 **提示**：程序会自动记住最近使用的 5 个文件路径，方便快速切换。

详细使用说明请查看 [HELP.md](HELP.md) | 新功能说明 [FEATURES_v1.3.0.md](FEATURES_v1.3.0.md)

## 支持的参数

### 服务器配置
- llama-server.exe 路径
- 模型文件路径（.gguf）
- 端口号
- Jinja 模板支持

### 上下文配置
- Context Size（上下文大小）：支持超长上下文（最高 128k+）
- Batch Size（批处理大小）：控制 Prompt 处理速度
- UBatch Size（微批处理大小）：控制显存峰值
- Flash Attention（闪存注意力机制）：大幅降低显存占用
- Cache Type K/V（KV 缓存类型）：f16, q8_0, q4_0

### 采样参数
- Temperature（温度）：0.0-2.0
- Top P（核采样）：0.0-1.0
- Top K（K采样）：1-100
- Min P（最小P采样）：0.0-1.0
- Repeat Penalty（重复惩罚）：1.0-1.5
- Repeat Last N（重复检测窗口）：64-2048
- Frequency Penalty（频率惩罚）：0.0-2.0
- Presence Penalty（存在惩罚）：0.0-2.0

### 高级配置
- Parallel Slots（并发槽位数）：1-16
- Reasoning Preserve（保留推理）：保留 `<think>` 标签
- API Key（接口密钥）：可选的访问认证

## 系统要求

- Windows 10/11 (x64)
- 无需安装 .NET Runtime（独立可执行文件）
- llama.cpp 服务器程序（[llama-server.exe](https://github.com/ggerganov/llama.cpp/releases)）

## 从源码构建

### 前置要求
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### 构建步骤

```bash
# 克隆仓库
git clone https://github.com/bakeshame/llama.cpp-loader-win.git
cd llama.cpp-loader-win

# 方法 1：使用构建脚本
.\build.bat

# 方法 2：使用 dotnet 命令
dotnet build -c Release
```

生成的可执行文件位于：`bin\Release\net8.0-windows\LlamaCppLoader.exe`

### 发布为单文件应用

```bash
# 方法 1：使用发布脚本
.\publish.bat

# 方法 2：使用 dotnet 命令
dotnet publish -c Release
```

发布后的单文件位于：`bin\Release\net8.0-windows\win-x64\publish\LlamaCppLoader.exe`

## 配置文件

### 存储位置
```
%APPDATA%\LlamaCppLoader\profiles.json
```

### 示例配置
查看 [profiles.example.json](profiles.example.json) 和 [profiles.recommended.json](profiles.recommended.json) 了解推荐配置。

## 常见问题

### Q: 启动失败显示 "llama-server.exe not found"
**A:** 请确保 llama-server.exe 路径正确，并且文件确实存在。可以从 [llama.cpp Releases](https://github.com/ggerganov/llama.cpp/releases) 下载最新版本。

### Q: 服务器启动但无法访问
**A:** 检查：
- 防火墙设置是否阻止了端口
- 端口是否被其他程序占用（尝试更换端口号）
- 查看控制台输出中的错误信息

### Q: 模型加载失败
**A:** 确认：
- 模型文件是 `.gguf` 格式
- 路径中没有特殊字符或中文（建议使用英文路径）
- 模型文件完整未损坏
- 显存足够加载模型

### Q: 如何查看详细错误信息
**A:** 所有输出和错误信息都会显示在程序下方的控制台输出区域。可以点击 "Clear Output" 清空日志。

### Q: 什么是 Jinja 模板？为什么必须开启？
**A:** Jinja 是模板引擎，用于正确格式化聊天消息。Qwen、Llama 等现代模型必须开启，否则会导致：
- 不听 system 指令
- 输出格式异常
- 可能陷入无限循环

### Q: Flash Attention 是什么？
**A:** Flash Attention 是一种优化的注意力机制实现，可以：
- 速度提升 2-3 倍
- 显存占用减少 40-60%
- 支持更长的上下文

强烈建议始终开启（如果显卡支持）。

### Q: 如何使用参数预设？ 🆕
**A:** 在"参数预设"区域点击对应的场景按钮：
- 🎯 **CTF/代码审计**：65k上下文，推理保留，适合代码分析
- 💬 **普通对话**：32k上下文，温度0.7，日常聊天
- 💻 **代码生成**：32k上下文，温度0.2，精确代码
- ✍️ **创意写作**：32k上下文，温度0.9，发散思维
- 📚 **大上下文**：128k上下文，q4_0缓存，超长文档

预设会保留你的服务器和模型路径，只更新参数。应用后可以继续微调。

## 推荐配置

使用内置预设快速开始，或参考下面的配置手动调整：

### CTF/代码审计（推荐） - 点击 🎯 预设
```
Context Size: 65536
Batch Size: 1024
Temperature: 0.40
Top P: 0.88
Top K: 30
Repeat Penalty: 1.08
Flash Attention: 启用
Jinja: 启用
Reasoning Preserve: 启用
```

### 普通对话 - 点击 💬 预设
```
Context Size: 32768
Batch Size: 512
Temperature: 0.70
Top P: 0.95
Top K: 40
Repeat Penalty: 1.10
```

### 代码生成 - 点击 💻 预设
```
Context Size: 32768
Batch Size: 1024
Temperature: 0.20
Top P: 0.85
Top K: 20
Repeat Penalty: 1.05
```

## 技术栈

- .NET 8.0
- WPF (Windows Presentation Foundation)
- C#
- Newtonsoft.Json

## 更新日志

查看 [CHANGELOG.md](CHANGELOG.md) 了解完整的版本历史。

### v1.3.0 (2026-07-29) 🆕
- 🖱️ 新增拖放文件支持（.exe 和 .gguf）
- 📂 自动记录最近使用的 5 个文件路径
- 🎯 新增 5 种参数预设（CTF/对话/代码生成/创意写作/大上下文）
- ⚡ 一键应用预设，快速切换场景
- 💡 改进的用户体验和工作流程

### v1.2.1 (2026-07-29)
- ✨ 优化单文件发布（69 MB，包含所有依赖）
- 🎨 改进按钮尺寸和 UI 一致性
- 🎨 优化底部按钮文字颜色（黑色更清晰）

### v1.2.0
- ➕ 新增并发槽位配置
- ➕ 新增推理保留开关
- ➕ 新增 API Key 认证支持
- 📝 优化所有参数的工具提示说明

### v1.1.0
- 🔔 新增版本检测功能
- 🌐 新增 GitHub 更新链接
- 🎨 新增版本信息栏

### v1.0.0
- 🎉 初始版本发布

## 截图

![主界面](screenshots/main.png)
*主界面 - 完整的参数配置*

## 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 贡献

欢迎贡献！请随时提交 Issue 或 Pull Request。

## 相关链接

- [llama.cpp 项目](https://github.com/ggerganov/llama.cpp)
- [llama.cpp 下载](https://github.com/ggerganov/llama.cpp/releases)
- [.NET 下载](https://dotnet.microsoft.com/download/dotnet/8.0)

## Star History

如果这个项目对你有帮助，请给个 ⭐ Star！

---

**Made with ❤️ for the llama.cpp community**
