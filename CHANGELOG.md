# 版本历史

## Version 1.2.1 (2026-07-29) - UI 优化和单文件发布

### ✨ 新功能
- **真正的单文件发布**
  - 从 148 MB + 7个DLL → 69 MB 单文件
  - 启用 `IncludeNativeLibrariesForSelfExtract`
  - 启用 `EnableCompressionInSingleFile`
  - 无需额外 DLL，直接运行
  - 文件：`LlamaCppLoader.csproj`

### 🎨 界面改进
- **优化按钮尺寸**
  - Browse 按钮：统一 `MinWidth="90"`, `Padding="8,5"`
  - Profile 管理按钮：`MinWidth="100-110"`, `Padding="8,5"`
  - 版本刷新按钮：`MinWidth="90"`
  - 底部控制按钮：统一 `Height="40"`, `FontSize="14"`
  
- **改进文字颜色**
  - Start Server 按钮：白色文字 → 黑色文字
  - Stop Server 按钮：白色文字 → 黑色文字
  - 黑色文字在彩色背景上对比度更好

### 🔧 技术改进
- 更新项目版本号到 1.2.1
- 简化 publish.bat 脚本
- 配置持久化的单文件发布参数

### 📦 分发
- GitHub Release: https://github.com/bakeshame/llama.cpp-loader-win/releases/tag/v1.2.1
- 单文件 exe: 69 MB
- 支持 Windows 10/11 (x64)

---

### ✨ 新功能
- **全面的参数说明系统**
  - 所有参数添加中英文双语标签
  - 鼠标悬停显示详细 ToolTip 说明
  - 包含推荐值、使用场景、影响说明
  - 多行格式，信息更丰富
  - 文件：`MainWindow.xaml` 全面更新

- **优化默认参数值**
  - Context Size: 85000 → 65536 (2的幂次，性能更好)
  - Top K: 25 → 30 (更多候选，减少卡住)
  - Repeat Penalty: 1.05 → 1.08 (更好防止死循环)
  - Repeat Last N: 512 → 1024 (配合64k上下文)
  - Frequency Penalty: 0.10 → 0.00 (代码任务，不影响变量)
  - 文件：`MainWindow.xaml.cs` ServerConfig 类

- **专业预设配置方案**
  - 新增 7 种配置模板
  - CTF/代码审计、代码生成、大型源码分析等
  - 涵盖不同显存和场景
  - 文件：`profiles.recommended.json`

### 📚 文档
- 新增 `参数详解.md` - 完整的参数说明文档（约100KB）
  - 每个参数的详细解释
  - 推荐值对照表
  - 使用场景说明
  - 常见问题解决方案
  - 显存占用估算
  - 调试技巧
- 新增 `profiles.recommended.json` - 7种专业预设配置
- 新增 `版本更新说明-v1.2.0.md` - 本次更新详细说明

### 🎨 界面改进
- 参数标签全部改为中英文双语
- 所有输入框和选项都有详细 ToolTip
- 提示信息包含建议值和多种场景
- 更专业的参数说明格式

### 🔧 技术改进
- 优化默认值以适配 RTX 5090 24G 显存
- 防止死循环的参数调整
- 更好的代码生成参数配置
- 基于2的幂次的上下文大小

### 💡 专业建议
- 推荐使用 65536 上下文（而不是 85000）
- CTF/代码审计使用更高的 Repeat Penalty
- 代码任务不使用 Frequency Penalty
- 更大的 Repeat Last N 防止远距离重复

---

## Version 1.1.0 (2026-07-29) - 版本管理功能

### ✨ 新功能
- **自动版本检测**
  - 程序启动时自动检测本地 llama-server.exe 版本
  - 通过 `--version` 参数获取版本信息
  - 支持多种版本格式识别
  - 文件：`MainWindow.xaml.cs` 新增 `DetectLocalVersionAsync()` 方法

- **GitHub 最新版本获取**
  - 连接 GitHub API 获取 llama.cpp 最新 release
  - 使用官方 API: `api.github.com/repos/ggerganov/llama.cpp/releases/latest`
  - 智能解析版本号和下载链接
  - 文件：`MainWindow.xaml.cs` 新增 `FetchLatestVersionAsync()` 方法

- **版本对比和更新提示**
  - 智能比较本地和远程版本
  - 发现新版本时显示醒目通知
  - 一键跳转 GitHub Release 页面
  - 文件：`MainWindow.xaml.cs` 新增 `ShouldUpdate()` 方法

- **版本信息显示栏**
  - 顶部新增蓝色版本信息栏
  - 实时显示本地和最新版本
  - 提供手动刷新按钮
  - 文件：`MainWindow.xaml` 新增版本信息 UI 组件

- **自动触发检测**
  - 选择新服务器路径时自动检测版本
  - 手动刷新功能
  - 异步操作不阻塞界面

### 🎨 界面改进
- 窗口高度从 750 增加到 800
- 新增 4 行布局（版本栏/配置/控制台/按钮）
- 版本信息栏使用蓝色主题
- 更新提示使用 Hyperlink 控件

### 🔧 技术更新
- 新增 `System.Net.Http` 依赖
- 新增 `System.Text.RegularExpressions` 依赖
- 新增 `Newtonsoft.Json.Linq` 依赖
- 添加 HttpClient 单例（10秒超时）
- 完善的异常处理和错误提示

### 📝 文档
- 新增 `新功能-版本检测.md` - 详细功能说明
- 新增 `版本更新说明-v1.1.0.md` - 版本更新总结

---

## Version 1.0.1 (2026-07-29) - 修复版本

### 🐛 Bug 修复
- **修复 Flash Attention 参数错误**
  - 问题：`--flash-attn` 参数缺少值，导致 llama.cpp 报错
  - 修复：将 `--flash-attn` 改为 `--flash-attn on`
  - 影响：所有启用 Flash Attention 的配置
  - 文件：`MainWindow.xaml.cs` 第 360 行

### 📝 更新说明
如果你已经构建了 v1.0.0，需要：
1. 关闭正在运行的程序
2. 重新构建：`dotnet build -c Release`
3. 重新运行程序

详见：[修复说明-FlashAttn.md](修复说明-FlashAttn.md)

---

## Version 1.0.0 (2026-07-29) - 初始版本

### ✨ 新功能

#### 核心功能
- ✅ 图形化界面配置 llama.cpp 服务器
- ✅ 支持所有主要 llama.cpp 参数
- ✅ 配置文件管理（保存/加载/删除）
- ✅ 一键启动/停止服务器
- ✅ 实时控制台输出

#### 支持的参数
**服务器配置**：
- llama-server.exe 路径选择
- 模型文件路径选择（GGUF 格式）
- 端口配置（默认 8080）
- Jinja 模板支持开关

**上下文配置**：
- Context Size（默认 85000）
- Batch Size（默认 1024）
- UBatch Size（默认 256）
- Flash Attention 开关（默认启用）
- Cache Type K/V（f16/q8_0/q4_0，默认 q8_0）

**采样参数**：
- Temperature（默认 0.40）
- Top P（默认 0.88）
- Top K（默认 25）
- Min P（默认 0.05）
- Repeat Penalty（默认 1.05）
- Repeat Last N（默认 512）
- Frequency Penalty（默认 0.10）
- Presence Penalty（默认 0）

#### 用户体验
- ✅ 文件浏览对话框（服务器和模型）
- ✅ 智能路径检测
- ✅ 友好的错误提示
- ✅ 路径和端口验证
- ✅ 关闭时提示停止服务器
- ✅ 显示完整启动命令

#### 配置管理
- ✅ 保存配置到 JSON 文件
- ✅ 从下拉菜单快速加载配置
- ✅ 删除不需要的配置
- ✅ 配置持久化到用户应用数据目录

### 📦 文件清单

**核心代码（7个文件）**：
- `LlamaCppLoader.csproj` - 项目文件
- `App.xaml` / `App.xaml.cs` - 应用程序入口
- `MainWindow.xaml` / `MainWindow.xaml.cs` - 主窗口（470+行）
- `SaveProfileDialog.xaml` / `SaveProfileDialog.xaml.cs` - 配置对话框

**文档（11个文件）**：
- `README.md` - 项目主文档
- `QUICKSTART.md` - 快速入门指南
- `HELP.md` - 详细使用帮助
- `PROJECT_OVERVIEW.md` - 技术文档
- `START_HERE.md` - 3步快速启动
- `构建成功说明.md` - 构建后指南
- `文档导航.md` - 文档索引
- `目录结构.md` - 目录说明
- `项目完成说明.md` - 完成总结
- `.gitignore` - Git 配置

**工具（3个文件）**：
- `build.bat` - 构建脚本
- `publish.bat` - 发布脚本
- `profiles.example.json` - 配置示例

### 🎯 技术栈
- **框架**：.NET 8.0
- **界面**：WPF (Windows Presentation Foundation)
- **语言**：C#
- **依赖**：Newtonsoft.Json 13.0.3
- **平台**：Windows 10/11

### 📊 代码统计
- **总文件数**：21 个
- **代码行数**：~1900 行
- **C# 代码**：~500 行
- **XAML 界面**：~250 行
- **文档**：~1150 行

### 🚀 构建方式
```bash
# 标准构建
dotnet build -c Release

# 独立发布
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### 📄 许可证
MIT License

---

## 即将推出（计划功能）

### Version 1.1.0（计划中）
- [ ] 系统托盘支持
- [ ] 多个服务器实例管理
- [ ] 性能监控（CPU/GPU/内存）
- [ ] 配置导入/导出功能
- [ ] 主题切换（亮色/暗色）
- [ ] 多语言支持（中文/英文）
- [ ] 快捷键支持

### Version 1.2.0（计划中）
- [ ] 内置 API 测试器
- [ ] 模型管理器
- [ ] 自动更新检查
- [ ] 使用统计和日志
- [ ] 预设配置模板库

---

## 已知问题

### Version 1.0.0
- ~~Flash Attention 参数缺少值~~ ✅ 已在 v1.0.1 修复

---

## 升级说明

### 从 v1.0.0 升级到 v1.0.1
1. 备份你的配置文件（可选）：
   ```
   %APPDATA%\LlamaCppLoader\profiles.json
   ```
2. 关闭程序
3. 下载新版本或重新构建
4. 运行新版本
5. 配置会自动保留

---

## 贡献者
- 初始开发：Claude (Opus 5)
- 需求提供：ChenLin

---

## 反馈和支持
- 报告问题：提交 Issue
- 功能建议：提交 Feature Request
- 文档问题：查看各个 `.md` 文件

---

**最新版本**：v1.0.1 (2026-07-29)
**下载**：构建后位于 `bin\Release\net8.0-windows\LlamaCppLoader.exe`
