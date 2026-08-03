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
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LlamaCppLoader
{
    public class ServerConfig
    {
        public string ServerPath { get; set; } = string.Empty;
        public string ModelPath { get; set; } = string.Empty;
        public int CtxSize { get; set; } = 65536;
        public int BatchSize { get; set; } = 1024;
        public int UBatchSize { get; set; } = 256;
        public bool FlashAttn { get; set; } = true;
        public string CacheTypeK { get; set; } = "q8_0";
        public string CacheTypeV { get; set; } = "q8_0";
        public double Temp { get; set; } = 0.40;
        public double TopP { get; set; } = 0.88;
        public int TopK { get; set; } = 30;
        public double MinP { get; set; } = 0.05;
        public double RepeatPenalty { get; set; } = 1.08;
        public int RepeatLastN { get; set; } = 1024;
        public double FrequencyPenalty { get; set; } = 0.00;
        public double PresencePenalty { get; set; } = 0;
        public int Port { get; set; } = 8080;
        public bool Jinja { get; set; } = true;
        public int Parallel { get; set; } = 1;
        public bool ReasoningPreserve { get; set; } = true;
        public bool ApiKeyEnabled { get; set; } = false;
        public string ApiKey { get; set; } = string.Empty;
        public int NGpuLayers { get; set; } = 999;
    }
}
