# 🔧 重要修复：Flash Attention 参数

## ⚠️ 发现的问题

你在运行时遇到的错误：
```
error while handling argument "--flash-attn": error: unknown value for --flash-attn: '--cache-type-k'
```

**原因**：`--flash-attn` 参数需要一个值（`on`、`off` 或 `auto`），但原代码只是添加了 `--flash-attn` 而没有指定值。

---

## ✅ 已修复

我已经修复了代码，将：
```csharp
args.Append("--flash-attn ");
```

改为：
```csharp
args.Append("--flash-attn on ");
```

---

## 🔄 如何应用修复

### 方法 1：关闭程序后重新构建（推荐）

1. **关闭正在运行的 LlamaCppLoader.exe**
2. **重新构建**：
   ```bash
   dotnet build -c Release
   ```
   或双击 `build.bat`

3. **重新运行程序**

### 方法 2：手动修改源代码（如果你想验证）

打开 `MainWindow.xaml.cs`，找到第 358-361 行：

**修改前**：
```csharp
if (config.FlashAttn)
{
    args.Append("--flash-attn ");
}
```

**修改后**：
```csharp
if (config.FlashAttn)
{
    args.Append("--flash-attn on ");
}
```

然后关闭程序，重新构建。

---

## 🎯 修复后的命令

修复后，当启用 Flash Attention 时，生成的命令将是：
```bash
--flash-attn on --cache-type-k q8_0 --cache-type-v q8_0 ...
```

而不是之前的：
```bash
--flash-attn --cache-type-k q8_0 --cache-type-v q8_0 ...
```

---

## 📋 完整的正确命令示例

修复后，程序会生成如下命令：
```bash
llama-server.exe 
  --model "E:\lmstudio_models\...\model.gguf" 
  --ctx-size 85000 
  --batch-size 1024 
  --ubatch-size 256 
  --flash-attn on 
  --cache-type-k q8_0 
  --cache-type-v q8_0 
  --temp 0.40 
  --top-p 0.88 
  --top-k 25 
  --min-p 0.05 
  --repeat-penalty 1.05 
  --repeat-last-n 512 
  --frequency-penalty 0.10 
  --presence-penalty 0 
  --port 8080 
  --jinja
```

---

## ✨ 测试验证

重新构建后，启动程序并点击 "Start Server"，你应该看到：

✅ **成功的输出**：
```
[18:30:00] Starting llama.cpp server...
[18:30:00] Command: E:\llama.cpp\llama-server.exe
[18:30:00] Arguments: --model "..." --ctx-size 85000 ... --flash-attn on ...
[18:30:00] --------------------------------------------------------------------------------
[18:30:00] Server started successfully!
[18:30:00] Server should be accessible at: http://localhost:8080
[18:30:02] llm_load_tensors: ...
[18:30:03] ...正常的加载信息...
```

而不再看到错误：
```
[ERROR] error while handling argument "--flash-attn"
```

---

## 💡 为什么会有这个问题

llama.cpp 的新版本中，`--flash-attn` 参数从开关型改为了值型参数，需要明确指定 `on`、`off` 或 `auto`。

旧版本可能接受 `--flash-attn` 作为开关，但新版本需要明确的值。

---

## 🔍 其他可能的调整

如果你想让用户可以选择 Flash Attention 的模式，可以：

### 选项 1：三选一下拉框（更灵活）

将 UI 中的 CheckBox 改为 ComboBox：
```xml
<ComboBox x:Name="FlashAttnComboBox" SelectedIndex="1">
    <ComboBoxItem Content="off"/>
    <ComboBoxItem Content="on"/>
    <ComboBoxItem Content="auto"/>
</ComboBox>
```

代码中：
```csharp
var flashMode = ((ComboBoxItem)FlashAttnComboBox.SelectedItem)?.Content.ToString() ?? "auto";
args.Append($"--flash-attn {flashMode} ");
```

### 选项 2：保持当前设计（简单）

当前的修复（CheckBox + `on` 值）是最简单的方案：
- 勾选 = `--flash-attn on`
- 不勾选 = 不添加参数（使用 llama.cpp 的默认值 `auto`）

---

## 📝 更新记录

- **2026-07-29 18:30** - 发现问题
- **2026-07-29 18:31** - 代码已修复
- **待执行** - 关闭程序后重新构建

---

## 🚀 快速操作步骤

1. ✅ **在程序中点击 "Stop Server"** （如果服务器在运行）
2. ✅ **关闭 LlamaCppLoader.exe 程序**
3. ✅ **运行 `build.bat` 或执行 `dotnet build -c Release`**
4. ✅ **重新运行程序**
5. ✅ **测试启动服务器**

---

**修复已完成，只需重新构建即可！** 🎉
