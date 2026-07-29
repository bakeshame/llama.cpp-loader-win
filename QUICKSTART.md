# 快速入门指南

## 第一步：安装 .NET SDK

1. 访问 [.NET 8.0 下载页面](https://dotnet.microsoft.com/download/dotnet/8.0)
2. 下载并安装 **.NET 8.0 SDK** (不是 Runtime)
3. 安装完成后，重启命令提示符以确保环境变量生效

验证安装：
```bash
dotnet --version
```
应该显示类似 `8.0.xxx` 的版本号

## 第二步：构建项目

### 方式 1：使用构建脚本（推荐）

双击运行 `build.bat` 文件，脚本会自动：
- 检查 .NET SDK
- 恢复 NuGet 包
- 编译项目

构建完成后，可执行文件位于：
```
bin\Release\net8.0-windows\LlamaCppLoader.exe
```

### 方式 2：手动构建

打开命令提示符，进入项目目录，执行：

```bash
# 恢复依赖包
dotnet restore

# 构建项目
dotnet build -c Release
```

## 第三步：发布单文件应用（可选）

如果你想要一个独立的可执行文件（包含所有依赖，无需安装 .NET Runtime），可以：

### 方式 1：使用发布脚本（推荐）

双击运行 `publish.bat`

发布完成后，独立可执行文件位于：
```
bin\Release\net8.0-windows\win-x64\publish\LlamaCppLoader.exe
```

### 方式 2：手动发布

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## 第四步：准备 llama.cpp

1. 下载 llama.cpp 的 Windows 版本
   - 官方仓库：https://github.com/ggerganov/llama.cpp
   - 预编译版本：https://github.com/ggerganov/llama.cpp/releases

2. 解压到任意目录，确保包含 `llama-server.exe`

3. 准备 GGUF 格式的模型文件
   - 可以从 Hugging Face 下载
   - 也可以使用 llama.cpp 工具转换

## 第五步：运行程序

1. 双击运行 `LlamaCppLoader.exe`

2. 首次使用配置：
   - 点击 "llama-server.exe Path" 右侧的 Browse，选择 llama-server.exe
   - 点击 "Model Path" 右侧的 Browse，选择你的 .gguf 模型文件
   - 调整参数（可以使用默认值）

3. 保存配置（可选）：
   - 点击 "Save Profile"
   - 输入配置名称，如 "Qwen3.6-35B"
   - 以后可以快速加载此配置

4. 启动服务器：
   - 点击 "Start Server" 按钮
   - 观察控制台输出，等待服务器启动
   - 看到启动成功消息后，可以通过浏览器访问 http://localhost:8080

## 参数说明

### 上下文大小 (Context Size)
- 控制模型可以"记住"多少文本
- 较大的值可以处理更长的对话，但消耗更多内存
- 推荐值：32000-85000

### 批处理大小 (Batch Size)
- 影响处理速度和内存使用
- 推荐值：512-2048

### Flash Attention
- 启用可以提高处理速度
- 需要 GPU 支持

### Temperature
- 控制输出的随机性
- 0.0 = 完全确定性
- 1.0 = 高随机性
- 推荐值：0.3-0.7

### Top P / Top K
- 控制采样策略
- Top P: 累积概率阈值 (推荐 0.88-0.95)
- Top K: 考虑的候选词数量 (推荐 20-40)

## 常见用途示例

### 配置 1: 高性能对话
```
Context Size: 85000
Batch Size: 1024
Temperature: 0.40
Top P: 0.88
Flash Attention: ON
```

### 配置 2: 创意写作
```
Context Size: 32000
Batch Size: 512
Temperature: 0.80
Top P: 0.92
Top K: 40
```

### 配置 3: 代码生成
```
Context Size: 16000
Batch Size: 512
Temperature: 0.20
Top P: 0.95
Top K: 25
```

## 故障排除

### 问题：程序无法启动
解决方案：
- 确认已安装 .NET 8.0 Runtime 或使用 self-contained 版本
- 检查 Windows 版本是否为 Windows 10 或更高

### 问题：服务器启动失败
解决方案：
- 验证 llama-server.exe 路径正确
- 确认模型文件存在且完整
- 检查端口是否被占用（尝试更换端口）
- 查看控制台输出中的错误信息

### 问题：内存不足
解决方案：
- 减小 Context Size
- 减小 Batch Size
- 使用量化程度更高的模型（如 Q4_K_S 而不是 Q8_0）

### 问题：GPU 不被识别
解决方案：
- 确认已安装正确的 CUDA 驱动（NVIDIA GPU）
- 或使用支持 ROCm 的版本（AMD GPU）
- llama.cpp 需要对应的 GPU 支持版本

## 高级技巧

### 1. 命令行查看完整参数
在控制台输出中，启动时会显示完整的命令行参数，你可以复制用于其他用途。

### 2. 多配置管理
为不同的模型和用途创建不同的配置文件，快速切换。

### 3. 性能优化
- 启用 Flash Attention（如果 GPU 支持）
- 调整 Batch Size 以平衡速度和内存
- 使用 q8_0 或 q4_0 的 KV Cache 以节省显存

### 4. API 使用
服务器启动后，可以通过 HTTP API 使用：
```bash
# 测试连接
curl http://localhost:8080/health

# 发送请求（示例）
curl http://localhost:8080/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{
    "messages": [{"role": "user", "content": "Hello!"}]
  }'
```

## 获取帮助

如果遇到问题：
1. 查看控制台输出中的错误信息
2. 检查 llama.cpp 官方文档
3. 提交 Issue 到项目仓库

## 更新程序

要更新到最新版本：
1. 下载新的源代码
2. 运行 `build.bat` 或 `publish.bat` 重新构建
3. 你的配置文件会自动保留（存储在 %APPDATA%\LlamaCppLoader）
