# 项目概览

## 项目结构

```
llamacpploader/
├── LlamaCppLoader.csproj          # 项目文件
├── App.xaml                        # 应用程序定义
├── App.xaml.cs                     # 应用程序代码
├── MainWindow.xaml                 # 主窗口界面
├── MainWindow.xaml.cs              # 主窗口逻辑
├── SaveProfileDialog.xaml          # 保存配置对话框界面
├── SaveProfileDialog.xaml.cs       # 保存配置对话框逻辑
├── build.bat                       # 构建脚本
├── publish.bat                     # 发布脚本
├── README.md                       # 项目说明
├── QUICKSTART.md                   # 快速入门指南
├── profiles.example.json           # 配置文件示例
└── .gitignore                      # Git 忽略文件
```

## 主要功能

### 1. 图形化配置界面
- 服务器路径选择
- 模型文件选择
- 所有 llama.cpp 参数的可视化配置

### 2. 配置文件管理
- 保存多个配置方案
- 快速切换配置
- 删除不需要的配置
- 配置文件存储在用户应用数据目录

### 3. 服务器控制
- 一键启动服务器
- 一键停止服务器
- 实时查看服务器输出
- 显示完整启动命令

### 4. 用户体验
- 友好的错误提示
- 文件浏览对话框
- 关闭程序时提示是否停止服务器
- 现代化的界面设计

## 技术实现

### WPF 界面
- 使用 XAML 构建现代化 UI
- Grid 布局自适应窗口大小
- ScrollViewer 支持长表单滚动
- 深色主题的控制台输出

### 进程管理
- 使用 Process 类启动 llama-server
- 重定向标准输出和错误输出
- 异步接收输出信息
- 安全的进程停止机制

### 数据持久化
- JSON 格式存储配置
- 使用 Newtonsoft.Json 序列化
- 自动创建配置目录
- 错误处理和恢复

### 命令行构建
- StringBuilder 构建复杂参数
- 路径引号处理
- 参数格式化（浮点数精度控制）
- 条件参数（如 Flash Attention）

## 支持的 llama.cpp 参数

### 基础参数
- `--model`: 模型文件路径
- `--port`: 服务器端口
- `--jinja`: Jinja 模板支持

### 上下文参数
- `--ctx-size`: 上下文大小
- `--batch-size`: 批处理大小
- `--ubatch-size`: 微批处理大小
- `--flash-attn`: Flash Attention
- `--cache-type-k`: KV Cache K 类型
- `--cache-type-v`: KV Cache V 类型

### 采样参数
- `--temp`: Temperature
- `--top-p`: Top P 采样
- `--top-k`: Top K 采样
- `--min-p`: Min P 采样
- `--repeat-penalty`: 重复惩罚
- `--repeat-last-n`: 重复检测窗口
- `--frequency-penalty`: 频率惩罚
- `--presence-penalty`: 存在惩罚

## 配置文件格式

配置文件使用 JSON 格式，存储在：
```
%APPDATA%\LlamaCppLoader\profiles.json
```

结构示例：
```json
{
  "配置名称": {
    "ServerPath": "服务器路径",
    "ModelPath": "模型路径",
    "CtxSize": 85000,
    ...其他参数
  }
}
```

## 构建流程

### Debug 构建
```bash
dotnet restore
dotnet build
```

生成位置：`bin\Debug\net8.0-windows\LlamaCppLoader.exe`

### Release 构建
```bash
dotnet build -c Release
```

生成位置：`bin\Release\net8.0-windows\LlamaCppLoader.exe`

### 单文件发布
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

生成位置：`bin\Release\net8.0-windows\win-x64\publish\LlamaCppLoader.exe`

优点：
- 包含所有依赖
- 无需安装 .NET Runtime
- 可以直接分发
- 文件大小约 100-150MB

## 依赖项

### NuGet 包
- `Newtonsoft.Json` 13.0.3 - JSON 序列化

### .NET 版本
- 目标框架：.NET 8.0
- UI 框架：WPF (Windows Presentation Foundation)

### 运行要求
- Windows 10 或更高版本
- .NET 8.0 Runtime（如果使用非 self-contained 版本）

## 扩展建议

### 可能的增强功能
1. **模型管理**
   - 模型列表管理
   - 模型信息显示
   - 最近使用的模型

2. **高级参数**
   - GPU 选择
   - 线程数配置
   - 更多 llama.cpp 参数

3. **监控功能**
   - CPU/GPU 使用率
   - 内存使用情况
   - 请求统计

4. **快捷操作**
   - 系统托盘图标
   - 开机自启动
   - 快捷键支持

5. **API 测试**
   - 内置 HTTP 客户端
   - 测试对话功能
   - 请求历史记录

## 常见问题

### Q: 如何添加新参数？
1. 在 `ServerConfig` 类中添加属性
2. 在 `MainWindow.xaml` 中添加对应的 UI 控件
3. 在 `GetCurrentConfig()` 中读取 UI 值
4. 在 `LoadProfileToUI()` 中设置 UI 值
5. 在 `BuildArguments()` 中添加命令行参数

### Q: 如何支持更多模型格式？
修改文件对话框的过滤器：
```csharp
Filter = "Model Files (*.gguf;*.bin)|*.gguf;*.bin|All Files (*.*)|*.*"
```

### Q: 如何更改默认配置？
修改各个 TextBox 的 Text 属性或在 `LoadDefaultSettings()` 中设置。

## 许可证

本项目使用 MIT 许可证，可以自由使用、修改和分发。

## 贡献指南

欢迎提交：
- Bug 报告
- 功能建议
- 代码改进
- 文档完善

提交 Pull Request 前请确保：
- 代码可以编译通过
- 遵循现有代码风格
- 添加必要的注释
- 更新相关文档
