# 使用帮助 - Llama.cpp Server Loader

## 目录
1. [界面介绍](#界面介绍)
2. [基本操作](#基本操作)
3. [参数详解](#参数详解)
4. [配置管理](#配置管理)
5. [高级技巧](#高级技巧)
6. [常见问题](#常见问题)

---

## 界面介绍

### 主界面布局

程序界面分为四个主要区域：

#### 1. 服务器配置（Server Configuration）
- **llama-server.exe Path**: llama-server 可执行文件的路径
- **Model Path**: GGUF 格式模型文件的路径
- **Port**: 服务器监听端口（默认 8080）
- **Enable Jinja**: 是否启用 Jinja 模板支持

#### 2. 上下文配置（Context Configuration）
- **Context Size**: 模型上下文窗口大小
- **Batch Size**: 批处理大小
- **UBatch Size**: 微批处理大小
- **Cache Type K/V**: KV 缓存的量化类型
- **Flash Attention**: 是否启用 Flash Attention

#### 3. 采样参数（Sampling Parameters）
- **Temperature**: 控制输出随机性
- **Top P**: 核采样概率阈值
- **Top K**: 考虑的最高概率 token 数量
- **Min P**: 最小概率阈值
- **Repeat Penalty**: 重复惩罚系数
- **Repeat Last N**: 重复检测窗口大小
- **Frequency Penalty**: 频率惩罚
- **Presence Penalty**: 存在惩罚

#### 4. 配置管理（Profile Management）
- 配置文件下拉列表
- 保存配置按钮
- 删除配置按钮

#### 5. 控制台输出（Console Output）
- 显示服务器启动信息
- 显示运行日志
- 显示错误信息

#### 6. 控制按钮
- **Start Server**: 启动服务器
- **Stop Server**: 停止服务器
- **Clear Output**: 清空控制台输出

---

## 基本操作

### 首次使用

**步骤 1**: 设置 llama-server 路径
1. 点击 "llama-server.exe Path" 右侧的 **Browse...** 按钮
2. 浏览到 llama-server.exe 所在位置
3. 选择 llama-server.exe 并确认

**步骤 2**: 选择模型文件
1. 点击 "Model Path" 右侧的 **Browse...** 按钮
2. 浏览到 GGUF 模型文件位置
3. 选择模型文件并确认

**步骤 3**: 调整参数（可选）
- 大部分情况下可以使用默认值
- 根据你的硬件配置和需求调整

**步骤 4**: 启动服务器
1. 点击底部的 **Start Server** 按钮
2. 观察控制台输出
3. 看到 "Server started successfully!" 表示启动成功

**步骤 5**: 使用服务器
- 服务器启动后可通过 `http://localhost:8080` 访问
- 使用兼容 OpenAI API 的客户端连接

### 停止服务器

1. 点击 **Stop Server** 按钮
2. 等待服务器完全停止
3. 控制台会显示 "Server stopped."

---

## 参数详解

### 上下文参数

#### Context Size（上下文大小）
- **作用**: 决定模型能"记住"多少内容
- **影响**: 越大可以处理更长的对话，但消耗更多内存
- **推荐值**:
  - 短对话: 4096-8192
  - 一般对话: 16000-32000
  - 长文档处理: 65000-85000
  - 超长上下文: 100000+

#### Batch Size（批处理大小）
- **作用**: 一次处理的 token 数量
- **影响**: 影响处理速度和内存占用
- **推荐值**:
  - 低内存: 256-512
  - 一般配置: 512-1024
  - 高性能: 1024-2048

#### UBatch Size（微批处理大小）
- **作用**: 用于提示处理的批大小
- **影响**: 影响首 token 生成速度
- **推荐值**: 通常为 Batch Size 的 1/4 到 1/2

#### Flash Attention
- **作用**: 优化的注意力机制实现
- **影响**: 显著提升速度，降低显存占用
- **建议**: GPU 支持时建议启用

#### Cache Type K/V
- **作用**: KV 缓存的量化类型
- **选项**:
  - `f16`: 16位浮点，最高质量，最大内存占用
  - `q8_0`: 8位量化，质量与内存平衡（推荐）
  - `q4_0`: 4位量化，节省内存，略微降低质量
- **建议**: 大部分情况使用 `q8_0`

### 采样参数

#### Temperature（温度）
- **作用**: 控制输出的随机性和创造性
- **范围**: 0.0 - 2.0
- **效果**:
  - 0.0 - 0.3: 非常确定性，适合事实性任务
  - 0.4 - 0.7: 平衡创造性和准确性（推荐）
  - 0.8 - 1.2: 更有创造性
  - 1.3+: 非常随机，可能不连贯
- **应用场景**:
  - 代码生成: 0.1-0.3
  - 问答: 0.3-0.5
  - 对话: 0.4-0.7
  - 创意写作: 0.7-1.0

#### Top P（核采样）
- **作用**: 从累积概率达到 P 的最小 token 集合中采样
- **范围**: 0.0 - 1.0
- **推荐值**:
  - 精确任务: 0.85-0.90
  - 一般用途: 0.90-0.95
  - 创意任务: 0.95-0.98

#### Top K
- **作用**: 只考虑概率最高的 K 个 token
- **范围**: 1 - 100
- **推荐值**:
  - 严格: 10-20
  - 平衡: 25-40（推荐）
  - 宽松: 50-80

#### Min P
- **作用**: 过滤掉概率低于阈值的 token
- **范围**: 0.0 - 1.0
- **推荐值**: 0.05-0.10
- **说明**: 帮助避免生成低质量输出

#### Repeat Penalty（重复惩罚）
- **作用**: 惩罚重复出现的 token
- **范围**: 1.0 - 1.5
- **效果**:
  - 1.0: 无惩罚
  - 1.05-1.10: 轻度惩罚（推荐）
  - 1.15-1.30: 中度惩罚
  - 1.30+: 强烈惩罚，可能影响连贯性

#### Repeat Last N
- **作用**: 检查最后 N 个 token 的重复
- **推荐值**: 256-512
- **说明**: 值太小可能遗漏重复，太大消耗更多计算

#### Frequency Penalty（频率惩罚）
- **作用**: 根据 token 出现频率降低其概率
- **范围**: 0.0 - 2.0
- **推荐值**: 0.0-0.2
- **应用**: 减少重复内容

#### Presence Penalty（存在惩罚）
- **作用**: 鼓励使用新 token
- **范围**: 0.0 - 2.0
- **推荐值**: 0.0-0.2
- **应用**: 增加输出多样性

---

## 配置管理

### 保存配置

1. 配置好所有参数
2. 点击 **Save Profile** 按钮
3. 在弹出对话框中输入配置名称
   - 建议使用描述性名称，如 "Qwen3.6-35B-Chat"
4. 点击 **Save** 确认

### 加载配置

1. 点击配置文件下拉列表
2. 选择要加载的配置
3. 所有参数会自动填充到界面

### 删除配置

1. 在下拉列表中选择要删除的配置
2. 点击 **Delete Profile** 按钮
3. 在确认对话框中点击 **Yes**

### 配置文件位置

配置文件自动保存在：
```
C:\Users\<你的用户名>\AppData\Roaming\LlamaCppLoader\profiles.json
```

可以手动编辑此文件来批量导入配置。

---

## 高级技巧

### 1. 性能优化

#### 内存优化
- 减小 Context Size
- 减小 Batch Size
- 使用 q4_0 的 Cache Type

#### 速度优化
- 启用 Flash Attention
- 增加 Batch Size（如果内存足够）
- 使用 q8_0 或 f16 的 Cache Type

### 2. 质量优化

#### 提高输出质量
- 使用较低的 Temperature (0.3-0.5)
- 增加 Top K (30-50)
- 适当的 Repeat Penalty (1.05-1.10)

#### 增加创造性
- 提高 Temperature (0.7-0.9)
- 提高 Top P (0.92-0.95)
- 降低 Repeat Penalty (1.0-1.05)

### 3. 多模型管理

为不同模型创建专门的配置：
- **小模型** (7B): 较大的 Context Size, 中等 Batch Size
- **中等模型** (13-35B): 平衡的配置
- **大模型** (70B+): 较小的 Context 和 Batch Size

### 4. 任务特定配置

#### 代码生成配置
```
Temperature: 0.2
Top P: 0.95
Top K: 25
Min P: 0.05
Repeat Penalty: 1.05
```

#### 创意写作配置
```
Temperature: 0.8
Top P: 0.92
Top K: 40
Min P: 0.05
Repeat Penalty: 1.10
Frequency Penalty: 0.10
```

#### 问答配置
```
Temperature: 0.4
Top P: 0.88
Top K: 25
Min P: 0.05
Repeat Penalty: 1.05
```

### 5. 查看完整命令

启动服务器时，控制台会显示完整的命令行参数。你可以：
- 复制用于脚本
- 用于命令行直接启动
- 调试参数问题

---

## 常见问题

### Q1: 服务器启动失败
**检查项**:
1. llama-server.exe 路径是否正确
2. 模型文件是否存在
3. 端口是否被占用（尝试更换端口）
4. 是否有足够的内存
5. 查看控制台错误信息

### Q2: 服务器启动很慢
**可能原因**:
- Context Size 太大
- 模型文件很大
- 硬盘读取速度慢

**解决方法**:
- 耐心等待（首次加载需要时间）
- 减小 Context Size
- 将模型放在 SSD 上

### Q3: 内存不足错误
**解决方法**:
1. 减小 Context Size
2. 减小 Batch Size
3. 使用 q4_0 的 Cache Type
4. 使用更小的模型或更高量化的模型

### Q4: GPU 未被使用
**检查项**:
- llama-server.exe 是否为 CUDA/ROCm 版本
- GPU 驱动是否正确安装
- CUDA/ROCm 版本是否匹配

### Q5: 输出质量不好
**调整方法**:
1. 尝试不同的 Temperature 值
2. 调整 Top P 和 Top K
3. 修改 Repeat Penalty
4. 检查模型是否适合当前任务

### Q6: 无法连接到服务器
**检查项**:
1. 服务器是否真的启动了
2. 端口号是否正确
3. 防火墙是否阻止连接
4. 使用 `http://localhost:端口号` 而不是 IP

### Q7: 程序关闭后服务器还在运行
- 使用程序的 Stop Server 按钮停止
- 或者通过任务管理器结束 llama-server.exe 进程

### Q8: 配置文件丢失
- 配置文件在 %APPDATA%\LlamaCppLoader\profiles.json
- 如果丢失，程序会创建新的空配置文件
- 建议定期备份配置文件

---

## 获取更多帮助

- **llama.cpp 官方文档**: https://github.com/ggerganov/llama.cpp
- **参数详细说明**: https://github.com/ggerganov/llama.cpp/blob/master/examples/server/README.md
- **问题报告**: 在项目 GitHub 页面提交 Issue

---

## 快捷键

- **Ctrl+S**: （建议添加）保存配置
- **Enter**: 在保存配置对话框中确认
- **Esc**: 在对话框中取消

---

## 更新历史

### Version 1.0.0
- 初始版本发布
- 支持所有主要 llama.cpp 参数
- 配置文件管理
- 实时控制台输出

---

**提示**: 建议先用较小的 Context Size 和 Batch Size 测试，确认能正常工作后再逐步增加。
