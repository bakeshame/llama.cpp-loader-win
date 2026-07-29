# 🚀 快速启动 - 3 步开始使用

## 第一步：运行程序

双击运行：
```
bin\Release\net8.0-windows\LlamaCppLoader.exe
```

或在命令行：
```bash
cd bin\Release\net8.0-windows
.\LlamaCppLoader.exe
```

---

## 第二步：配置路径（首次使用）

### 1. 设置 llama-server.exe 路径
- 点击第一个 **Browse...** 按钮
- 找到并选择你的 `llama-server.exe`
- 例如：`E:\llama.cpp\llama-server.exe`

### 2. 选择模型文件
- 点击第二个 **Browse...** 按钮
- 选择你的 GGUF 模型文件
- 例如：`E:\lmstudio_models\unsloth\Qwen3.6-35B-A3B-GGUF\Qwen3.6-35B-A3B-UD-Q4_K_S.gguf`

**提示**：其他参数已经预设好了，使用默认值即可！

---

## 第三步：启动服务器

点击底部的绿色 **Start Server** 按钮

✅ 启动成功后，你会在控制台看到：
```
[18:30:45] Starting llama.cpp server...
[18:30:47] Server started successfully!
[18:30:47] Server accessible at: http://localhost:8080
```

现在你可以访问 **http://localhost:8080** 使用服务器了！

---

## 💾 保存配置（推荐）

配置好后，保存起来下次直接用：

1. 点击 **Save Profile** 按钮
2. 输入名称，比如 **"Qwen3.6-35B"**
3. 点击 **Save**

下次使用时，从下拉菜单选择配置就能快速加载！

---

## 🎯 预设的参数

程序已经按你的需求预设了参数：

| 参数 | 值 |
|------|-----|
| Context Size | 85000 |
| Batch Size | 1024 |
| UBatch Size | 256 |
| Flash Attention | 启用 |
| Cache Type K | q8_0 |
| Cache Type V | q8_0 |
| Temperature | 0.40 |
| Top P | 0.88 |
| Top K | 25 |
| Min P | 0.05 |
| Repeat Penalty | 1.05 |
| Repeat Last N | 512 |
| Frequency Penalty | 0.10 |
| Presence Penalty | 0 |
| Port | 8080 |
| Jinja | 启用 |

与你原来的命令行参数完全一致！

---

## 🛑 停止服务器

点击红色的 **Stop Server** 按钮即可

---

## ❓ 常见问题

### Q: 提示找不到 .NET Runtime
**A**: 下载并安装 .NET 8.0 Runtime：https://dotnet.microsoft.com/download/dotnet/8.0

或者使用独立版本（运行 `publish.bat` 构建）

### Q: 服务器启动很慢
**A**: 首次加载大模型需要时间，请耐心等待

### Q: 端口被占用
**A**: 修改 Port 值为其他端口，比如 8081

### Q: 需要调整参数
**A**: 查看 [HELP.md](HELP.md) 了解每个参数的详细说明

---

## 📖 更多帮助

- **详细使用说明**：[HELP.md](HELP.md)
- **参数调优指南**：[QUICKSTART.md](QUICKSTART.md)
- **技术文档**：[PROJECT_OVERVIEW.md](PROJECT_OVERVIEW.md)

---

## 🎉 就这么简单！

1. ✅ 运行程序
2. ✅ 选择路径
3. ✅ 启动服务器

**享受图形化的 llama.cpp 使用体验！** 🚀

---

**提示**：关闭程序时会询问是否停止服务器，请选择 "Yes" 安全停止。
