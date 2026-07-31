using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LlamaCppLoader
{
    public partial class MainWindow : Window
    {
        private Process? serverProcess;
        private readonly string configDirectory;
        private readonly string profilesFile;
        private readonly string recentPathsFile;
        private Dictionary<string, ServerConfig> profiles = new();
        private RecentPaths recentPaths = new();
        private static readonly HttpClient httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        private string? localVersion;
        private string? latestVersion;
        private string? latestReleaseUrl;

        public MainWindow()
        {
            InitializeComponent();

            configDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LlamaCppLoader"
            );
            profilesFile = Path.Combine(configDirectory, "profiles.json");
            recentPathsFile = Path.Combine(configDirectory, "recent.json");

            Directory.CreateDirectory(configDirectory);
            LoadRecentPaths();
            LoadProfiles();
            LoadDefaultSettings();

            // 设置 User-Agent
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("LlamaCppLoader/1.4.0");

            // 异步检查版本
            _ = CheckVersionsAsync();
        }

        private void LoadDefaultSettings()
        {
            // Set default server path if llama-server.exe is in common locations
            var possiblePaths = new[]
            {
                @"E:\llama.cpp\llama-server.exe",
                @"C:\llama.cpp\llama-server.exe",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "llama-server.exe")
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    ServerPathTextBox.Text = path;
                    break;
                }
            }
        }

        private void LoadProfiles()
        {
            profiles = new Dictionary<string, ServerConfig>();

            if (File.Exists(profilesFile))
            {
                try
                {
                    var json = File.ReadAllText(profilesFile);
                    profiles = JsonConvert.DeserializeObject<Dictionary<string, ServerConfig>>(json)
                        ?? new Dictionary<string, ServerConfig>();
                }
                catch (Exception ex)
                {
                    LogToConsole($"Error loading profiles: {ex.Message}");
                }
            }

            ProfileComboBox.Items.Clear();
            foreach (var profile in profiles.Keys)
            {
                ProfileComboBox.Items.Add(profile);
            }

            if (ProfileComboBox.Items.Count > 0)
            {
                ProfileComboBox.SelectedIndex = 0;
                LoadProfileToUI(profiles[(string)ProfileComboBox.Items[0]]);
            }
        }

        private void SaveProfiles()
        {
            try
            {
                var json = JsonConvert.SerializeObject(profiles, Formatting.Indented);
                File.WriteAllText(profilesFile, json);
            }
            catch (Exception ex)
            {
                LogToConsole($"Error saving profiles: {ex.Message}");
            }
        }

        private void BrowseServerPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*",
                Title = "Select llama-server.exe"
            };

            if (dialog.ShowDialog() == true)
            {
                ServerPathTextBox.Text = dialog.FileName;
                AddToRecentServerPath(dialog.FileName);
                // 重新检测版本
                _ = CheckVersionsAsync();
            }
        }

        private void BrowseModelPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "GGUF Models (*.gguf)|*.gguf|All Files (*.*)|*.*",
                Title = "Select Model File"
            };

            if (dialog.ShowDialog() == true)
            {
                ModelPathTextBox.Text = dialog.FileName;
                AddToRecentModelPath(dialog.FileName);
            }
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveProfileDialog();
            if (dialog.ShowDialog() == true)
            {
                var profileName = dialog.ProfileName;
                var config = GetCurrentConfig();
                profiles[profileName] = config;
                SaveProfiles();
                LoadProfiles();
                ProfileComboBox.SelectedItem = profileName;
                LogToConsole($"Profile '{profileName}' saved successfully.");
            }
        }

        private void ProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProfileComboBox.SelectedItem != null && profiles.Count > 0)
            {
                var profileName = (string)ProfileComboBox.SelectedItem;
                if (profiles.ContainsKey(profileName))
                {
                    LoadProfileToUI(profiles[profileName]);
                }
            }
        }

        private void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ProfileComboBox.SelectedItem == null) return;

            var profileName = (string)ProfileComboBox.SelectedItem;
            var result = MessageBox.Show(
                $"Are you sure you want to delete profile '{profileName}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                profiles.Remove(profileName);
                SaveProfiles();
                LoadProfiles();
                LogToConsole($"Profile '{profileName}' deleted.");
            }
        }

        private ServerConfig GetCurrentConfig()
        {
            return new ServerConfig
            {
                ServerPath = ServerPathTextBox.Text,
                ModelPath = ModelPathTextBox.Text,
                CtxSize = int.TryParse(CtxSizeTextBox.Text, out var ctx) ? ctx : 65536,
                BatchSize = int.TryParse(BatchSizeTextBox.Text, out var batch) ? batch : 1024,
                UBatchSize = int.TryParse(UBatchSizeTextBox.Text, out var ubatch) ? ubatch : 256,
                FlashAttn = FlashAttnCheckBox.IsChecked == true,
                CacheTypeK = ((ComboBoxItem)CacheTypeKComboBox.SelectedItem)?.Content.ToString() ?? "q8_0",
                CacheTypeV = ((ComboBoxItem)CacheTypeVComboBox.SelectedItem)?.Content.ToString() ?? "q8_0",
                Temp = double.TryParse(TempTextBox.Text, out var temp) ? temp : 0.40,
                TopP = double.TryParse(TopPTextBox.Text, out var topP) ? topP : 0.88,
                TopK = int.TryParse(TopKTextBox.Text, out var topK) ? topK : 30,
                MinP = double.TryParse(MinPTextBox.Text, out var minP) ? minP : 0.05,
                RepeatPenalty = double.TryParse(RepeatPenaltyTextBox.Text, out var rp) ? rp : 1.08,
                RepeatLastN = int.TryParse(RepeatLastNTextBox.Text, out var rln) ? rln : 1024,
                FrequencyPenalty = double.TryParse(FrequencyPenaltyTextBox.Text, out var fp) ? fp : 0.00,
                PresencePenalty = double.TryParse(PresencePenaltyTextBox.Text, out var pp) ? pp : 0,
                Port = int.TryParse(PortTextBox.Text, out var port) ? port : 8080,
                Jinja = JinjaCheckBox.IsChecked == true,
                Parallel = int.TryParse(ParallelTextBox.Text, out var parallel) ? parallel : 1,
                ReasoningPreserve = ReasoningPreserveCheckBox.IsChecked == true,
                ApiKeyEnabled = ApiKeyEnabledCheckBox.IsChecked == true,
                ApiKey = ApiKeyTextBox.Text
            };
        }

        private void LoadProfileToUI(ServerConfig config)
        {
            ServerPathTextBox.Text = config.ServerPath;
            ModelPathTextBox.Text = config.ModelPath;
            CtxSizeTextBox.Text = config.CtxSize.ToString();
            BatchSizeTextBox.Text = config.BatchSize.ToString();
            UBatchSizeTextBox.Text = config.UBatchSize.ToString();
            FlashAttnCheckBox.IsChecked = config.FlashAttn;

            SelectComboBoxItem(CacheTypeKComboBox, config.CacheTypeK);
            SelectComboBoxItem(CacheTypeVComboBox, config.CacheTypeV);

            TempTextBox.Text = config.Temp.ToString("F2");
            TopPTextBox.Text = config.TopP.ToString("F2");
            TopKTextBox.Text = config.TopK.ToString();
            MinPTextBox.Text = config.MinP.ToString("F2");
            RepeatPenaltyTextBox.Text = config.RepeatPenalty.ToString("F2");
            RepeatLastNTextBox.Text = config.RepeatLastN.ToString();
            FrequencyPenaltyTextBox.Text = config.FrequencyPenalty.ToString("F2");
            PresencePenaltyTextBox.Text = config.PresencePenalty.ToString();
            PortTextBox.Text = config.Port.ToString();
            JinjaCheckBox.IsChecked = config.Jinja;

            // 新增参数
            ParallelTextBox.Text = config.Parallel.ToString();
            ReasoningPreserveCheckBox.IsChecked = config.ReasoningPreserve;
            ApiKeyEnabledCheckBox.IsChecked = config.ApiKeyEnabled;
            ApiKeyTextBox.Text = config.ApiKey;
            ApiKeyTextBox.IsEnabled = config.ApiKeyEnabled;
        }

        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (((ComboBoxItem)comboBox.Items[i]).Content.ToString() == value)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServerPathTextBox.Text))
            {
                MessageBox.Show("Please specify llama-server.exe path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!File.Exists(ServerPathTextBox.Text))
            {
                MessageBox.Show("llama-server.exe not found at specified path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(ModelPathTextBox.Text))
            {
                MessageBox.Show("Please specify model path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!File.Exists(ModelPathTextBox.Text))
            {
                MessageBox.Show("Model file not found at specified path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var config = GetCurrentConfig();
            var arguments = BuildArguments(config);

            try
            {
                LogToConsole("Starting llama.cpp server...");
                LogToConsole($"Command: {config.ServerPath}");
                LogToConsole($"Arguments: {arguments}");
                LogToConsole(new string('-', 80));

                serverProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = config.ServerPath,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Path.GetDirectoryName(config.ServerPath)
                    }
                };

                serverProcess.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Dispatcher.Invoke(() => LogToConsole(args.Data));
                    }
                };

                serverProcess.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Dispatcher.Invoke(() => LogToConsole($"[ERROR] {args.Data}"));
                    }
                };

                serverProcess.Start();
                serverProcess.BeginOutputReadLine();
                serverProcess.BeginErrorReadLine();

                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
                TestButton.IsEnabled = true;

                LogToConsole("Server started successfully!");
                LogToConsole($"Server should be accessible at: http://localhost:{config.Port}");
            }
            catch (Exception ex)
            {
                LogToConsole($"Error starting server: {ex.Message}");
                MessageBox.Show($"Failed to start server: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopServer_Click(object sender, RoutedEventArgs e)
        {
            if (serverProcess != null && !serverProcess.HasExited)
            {
                try
                {
                    LogToConsole("Stopping server...");
                    serverProcess.Kill();
                    serverProcess.WaitForExit(5000);
                    LogToConsole("Server stopped.");
                }
                catch (Exception ex)
                {
                    LogToConsole($"Error stopping server: {ex.Message}");
                }
                finally
                {
                    serverProcess?.Dispose();
                    serverProcess = null;
                    StartButton.IsEnabled = true;
                    StopButton.IsEnabled = false;
                    TestButton.IsEnabled = false;
                }
            }
        }

        private void ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            ConsoleOutput.Clear();
        }

        private string BuildArguments(ServerConfig config)
        {
            var args = new StringBuilder();

            args.Append($"--model \"{config.ModelPath}\" ");
            args.Append($"--ctx-size {config.CtxSize} ");
            args.Append($"--batch-size {config.BatchSize} ");
            args.Append($"--ubatch-size {config.UBatchSize} ");

            // 新增：并发槽位
            if (config.Parallel > 0)
            {
                args.Append($"--parallel {config.Parallel} ");
            }

            if (config.FlashAttn)
            {
                args.Append("--flash-attn on ");
            }

            args.Append($"--cache-type-k {config.CacheTypeK} ");
            args.Append($"--cache-type-v {config.CacheTypeV} ");
            args.Append($"--temp {config.Temp:F2} ");
            args.Append($"--top-p {config.TopP:F2} ");
            args.Append($"--top-k {config.TopK} ");
            args.Append($"--min-p {config.MinP:F2} ");
            args.Append($"--repeat-penalty {config.RepeatPenalty:F2} ");
            args.Append($"--repeat-last-n {config.RepeatLastN} ");
            args.Append($"--frequency-penalty {config.FrequencyPenalty:F2} ");
            args.Append($"--presence-penalty {config.PresencePenalty} ");
            args.Append($"--port {config.Port} ");

            // 新增：Reasoning Preserve
            if (config.ReasoningPreserve)
            {
                args.Append("--reasoning-preserve ");
            }

            // 新增：API Key
            if (config.ApiKeyEnabled && !string.IsNullOrWhiteSpace(config.ApiKey))
            {
                args.Append($"--api-key \"{config.ApiKey}\" ");
            }

            if (config.Jinja)
            {
                args.Append("--jinja ");
            }

            return args.ToString().Trim();
        }

        private void ApiKeyEnabled_Changed(object sender, RoutedEventArgs e)
        {
            if (ApiKeyTextBox != null)
            {
                ApiKeyTextBox.IsEnabled = ApiKeyEnabledCheckBox.IsChecked == true;
            }
        }

        private void LogToConsole(string message)
        {
            ConsoleOutput.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            ConsoleOutput.ScrollToEnd();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (serverProcess != null && !serverProcess.HasExited)
            {
                var result = MessageBox.Show(
                    "Server is still running. Do you want to stop it before closing?",
                    "Server Running",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    StopServer_Click(this, new RoutedEventArgs());
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnClosing(e);
        }

        private async Task CheckVersionsAsync()
        {
            try
            {
                // 检测本地版本
                await DetectLocalVersionAsync();

                // 获取最新版本
                await FetchLatestVersionAsync();

                // 更新UI
                UpdateVersionDisplay();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    VersionInfoTextBlock.Text = $"版本检测失败: {ex.Message}";
                });
            }
        }

        private async Task DetectLocalVersionAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    var serverPath = Dispatcher.Invoke(() => ServerPathTextBox.Text);

                    if (string.IsNullOrWhiteSpace(serverPath) || !File.Exists(serverPath))
                    {
                        localVersion = null;
                        return;
                    }

                    // 运行 llama-server --version 来获取版本
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = serverPath,
                            Arguments = "--version",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true,
                            WorkingDirectory = Path.GetDirectoryName(serverPath)
                        }
                    };

                    var output = new StringBuilder();
                    process.OutputDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            output.AppendLine(e.Data);
                    };
                    process.ErrorDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            output.AppendLine(e.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit(5000);

                    var versionText = output.ToString();

                    // 尝试从输出中提取版本号
                    // 格式示例: "version: 1234 (a1b2c3d4)" 或 "build: 1234"
                    var buildMatch = Regex.Match(versionText, @"(?:version|build):\s*(\d+)", RegexOptions.IgnoreCase);
                    var commitMatch = Regex.Match(versionText, @"\(([a-f0-9]{7,})\)", RegexOptions.IgnoreCase);

                    if (buildMatch.Success)
                    {
                        localVersion = $"build {buildMatch.Groups[1].Value}";
                        if (commitMatch.Success)
                        {
                            localVersion += $" ({commitMatch.Groups[1].Value})";
                        }
                    }
                    else
                    {
                        // 如果没有找到版本号，尝试从文件信息获取
                        var fileVersion = FileVersionInfo.GetVersionInfo(serverPath);
                        if (!string.IsNullOrEmpty(fileVersion.FileVersion))
                        {
                            localVersion = $"v{fileVersion.FileVersion}";
                        }
                        else
                        {
                            localVersion = "未知版本";
                        }
                    }
                }
                catch (Exception ex)
                {
                    localVersion = $"检测失败: {ex.Message}";
                }
            });
        }

        private async Task FetchLatestVersionAsync()
        {
            try
            {
                // GitHub API: 获取最新 release
                var apiUrl = "https://api.github.com/repos/ggerganov/llama.cpp/releases/latest";

                var response = await httpClient.GetStringAsync(apiUrl);
                var json = JObject.Parse(response);

                var tagName = json["tag_name"]?.ToString();
                latestReleaseUrl = json["html_url"]?.ToString();

                if (!string.IsNullOrEmpty(tagName))
                {
                    latestVersion = tagName;
                }
                else
                {
                    latestVersion = "无法获取";
                }
            }
            catch (HttpRequestException ex)
            {
                latestVersion = $"网络错误: {ex.Message}";
            }
            catch (Exception ex)
            {
                latestVersion = $"解析失败: {ex.Message}";
            }
        }

        private void UpdateVersionDisplay()
        {
            Dispatcher.Invoke(() =>
            {
                var localInfo = string.IsNullOrEmpty(localVersion) ? "未选择服务器" : $"本地: {localVersion}";
                var remoteInfo = string.IsNullOrEmpty(latestVersion) ? "检测中..." : $"最新: {latestVersion}";

                VersionInfoTextBlock.Text = $"{localInfo}  |  {remoteInfo}";

                // 检查是否需要更新
                if (!string.IsNullOrEmpty(localVersion) && !string.IsNullOrEmpty(latestVersion) &&
                    !localVersion.Contains("检测失败") && !latestVersion.Contains("错误") &&
                    !latestVersion.Contains("失败"))
                {
                    // 简单的版本比较
                    if (ShouldUpdate(localVersion, latestVersion))
                    {
                        UpdateAvailableTextBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        UpdateAvailableTextBlock.Visibility = Visibility.Collapsed;
                    }
                }
            });
        }

        private bool ShouldUpdate(string local, string latest)
        {
            // 提取构建号进行比较
            var localBuildMatch = Regex.Match(local, @"build\s+(\d+)");
            var latestBuildMatch = Regex.Match(latest, @"b(\d+)");

            if (localBuildMatch.Success && latestBuildMatch.Success)
            {
                if (int.TryParse(localBuildMatch.Groups[1].Value, out int localBuild) &&
                    int.TryParse(latestBuildMatch.Groups[1].Value, out int latestBuild))
                {
                    return latestBuild > localBuild;
                }
            }

            // 如果无法比较构建号，比较标签名
            return local != latest && !local.Contains(latest) && !latest.Contains("无法获取");
        }

        private void RefreshVersion_Click(object sender, RoutedEventArgs e)
        {
            VersionInfoTextBlock.Text = "刷新版本信息中...";
            UpdateAvailableTextBlock.Visibility = Visibility.Collapsed;
            _ = CheckVersionsAsync();
        }

        private void UpdateLink_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(latestReleaseUrl))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = latestReleaseUrl,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法打开浏览器: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("GitHub Release 页面地址不可用", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // ===== 最近使用路径功能 =====
        private void LoadRecentPaths()
        {
            if (File.Exists(recentPathsFile))
            {
                try
                {
                    var json = File.ReadAllText(recentPathsFile);
                    recentPaths = JsonConvert.DeserializeObject<RecentPaths>(json) ?? new RecentPaths();
                }
                catch (Exception ex)
                {
                    LogToConsole($"Error loading recent paths: {ex.Message}");
                    recentPaths = new RecentPaths();
                }
            }
        }

        private void SaveRecentPaths()
        {
            try
            {
                var json = JsonConvert.SerializeObject(recentPaths, Formatting.Indented);
                File.WriteAllText(recentPathsFile, json);
            }
            catch (Exception ex)
            {
                LogToConsole($"Error saving recent paths: {ex.Message}");
            }
        }

        private void AddToRecentServerPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                recentPaths.AddServerPath(path);
                SaveRecentPaths();
            }
        }

        private void AddToRecentModelPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                recentPaths.AddModelPath(path);
                SaveRecentPaths();
            }
        }

        // ===== 拖放功能 =====
        private void ServerPath_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    var file = files[0];
                    e.Effects = file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                        ? DragDropEffects.Copy
                        : DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void ServerPath_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    var file = files[0];
                    if (file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        ServerPathTextBox.Text = file;
                        AddToRecentServerPath(file);
                        _ = CheckVersionsAsync();
                        LogToConsole($"Server path set to: {file}");
                    }
                    else
                    {
                        MessageBox.Show("请拖放 .exe 文件", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void ModelPath_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    var file = files[0];
                    e.Effects = file.EndsWith(".gguf", StringComparison.OrdinalIgnoreCase)
                        ? DragDropEffects.Copy
                        : DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void ModelPath_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    var file = files[0];
                    if (file.EndsWith(".gguf", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelPathTextBox.Text = file;
                        AddToRecentModelPath(file);
                        LogToConsole($"Model path set to: {file}");
                    }
                    else
                    {
                        MessageBox.Show("请拖放 .gguf 文件", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        // ===== 参数预设功能 =====
        private void ApplyPreset_CTF(object sender, RoutedEventArgs e)
        {
            ApplyPreset(ConfigPresets.GetCTFPreset(), "CTF/代码审计");
        }

        private void ApplyPreset_Conversation(object sender, RoutedEventArgs e)
        {
            ApplyPreset(ConfigPresets.GetConversationPreset(), "普通对话");
        }

        private void ApplyPreset_CodeGen(object sender, RoutedEventArgs e)
        {
            ApplyPreset(ConfigPresets.GetCodeGenerationPreset(), "代码生成");
        }

        private void ApplyPreset_Creative(object sender, RoutedEventArgs e)
        {
            ApplyPreset(ConfigPresets.GetCreativeWritingPreset(), "创意写作");
        }

        private void ApplyPreset_LargeContext(object sender, RoutedEventArgs e)
        {
            ApplyPreset(ConfigPresets.GetLargeContextPreset(), "大上下文");
        }

        private void ApplyPreset(ServerConfig preset, string presetName)
        {
            // 保存当前的服务器和模型路径
            var currentServerPath = ServerPathTextBox.Text;
            var currentModelPath = ModelPathTextBox.Text;

            // 应用预设
            LoadProfileToUI(preset);

            // 恢复服务器和模型路径
            ServerPathTextBox.Text = currentServerPath;
            ModelPathTextBox.Text = currentModelPath;

            LogToConsole($"已应用预设：{presetName}");
        }

        // ===== 快捷键支持 =====
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+S: Save Profile
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                SaveProfile_Click(this, new RoutedEventArgs());
            }
            // F5: Start Server
            else if (e.Key == Key.F5 && Keyboard.Modifiers == ModifierKeys.None)
            {
                e.Handled = true;
                if (StartButton.IsEnabled)
                {
                    StartServer_Click(this, new RoutedEventArgs());
                }
            }
            // Ctrl+F5: Stop Server
            else if (e.Key == Key.F5 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                if (StopButton.IsEnabled)
                {
                    StopServer_Click(this, new RoutedEventArgs());
                }
            }
            // Ctrl+L: Clear Console
            else if (e.Key == Key.L && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                ClearOutput_Click(this, new RoutedEventArgs());
            }
        }

        // ===== 配置导入/导出 =====
        private void ExportProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = GetCurrentConfig();
                var dialog = new SaveFileDialog
                {
                    Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                    Title = "Export Configuration",
                    FileName = $"llamacpp-config-{DateTime.Now:yyyyMMdd-HHmmss}.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(dialog.FileName, json);
                    LogToConsole($"配置已导出到: {dialog.FileName}");
                    MessageBox.Show($"配置已成功导出到:\n{dialog.FileName}", "导出成功",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                LogToConsole($"导出配置失败: {ex.Message}");
                MessageBox.Show($"导出配置失败:\n{ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                    Title = "Import Configuration"
                };

                if (dialog.ShowDialog() == true)
                {
                    var json = File.ReadAllText(dialog.FileName);
                    var config = JsonConvert.DeserializeObject<ServerConfig>(json);

                    if (config != null)
                    {
                        LoadProfileToUI(config);
                        LogToConsole($"配置已从 {Path.GetFileName(dialog.FileName)} 导入");
                        MessageBox.Show($"配置已成功导入！\n\n提示：使用 'Save Profile' 保存到配置列表中",
                            "导入成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("无法解析配置文件", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogToConsole($"导入配置失败: {ex.Message}");
                MessageBox.Show($"导入配置失败:\n{ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===== 测试连接 =====
        private async void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            var port = int.TryParse(PortTextBox.Text, out var p) ? p : 8080;
            var testUrl = $"http://localhost:{port}/health";

            LogToConsole($"正在测试连接: {testUrl}");
            TestButton.IsEnabled = false;
            TestButton.Content = "⏳ Testing...";

            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var response = await httpClient.GetAsync(testUrl);
                sw.Stop();

                if (response.IsSuccessStatusCode)
                {
                    LogToConsole($"✓ 连接成功! 响应时间: {sw.ElapsedMilliseconds}ms");
                    LogToConsole($"  状态码: {(int)response.StatusCode} {response.StatusCode}");

                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(content) && content.Length < 500)
                    {
                        LogToConsole($"  响应: {content}");
                    }

                    MessageBox.Show(
                        $"✓ 服务器连接成功!\n\n" +
                        $"端口: {port}\n" +
                        $"响应时间: {sw.ElapsedMilliseconds}ms\n" +
                        $"状态: {response.StatusCode}",
                        "测试成功",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    LogToConsole($"✗ 服务器响应异常: {(int)response.StatusCode} {response.StatusCode}");
                    MessageBox.Show(
                        $"服务器响应异常\n\n状态码: {(int)response.StatusCode} {response.StatusCode}",
                        "测试失败",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (HttpRequestException ex)
            {
                LogToConsole($"✗ 连接失败: {ex.Message}");
                MessageBox.Show(
                    $"无法连接到服务器\n\n" +
                    $"端口: {port}\n" +
                    $"错误: {ex.Message}\n\n" +
                    $"请确认：\n" +
                    $"1. 服务器已启动\n" +
                    $"2. 端口号正确\n" +
                    $"3. 防火墙未阻止",
                    "连接失败",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (TaskCanceledException)
            {
                LogToConsole("✗ 连接超时（10秒）");
                MessageBox.Show(
                    "连接超时\n\n服务器可能正在启动中或未响应",
                    "超时",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            finally
            {
                TestButton.IsEnabled = true;
                TestButton.Content = "🧪 Test Connection";
            }
        }
    }
}
