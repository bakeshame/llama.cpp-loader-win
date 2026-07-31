namespace LlamaCppLoader
{
    public static class ConfigPresets
    {
        public static ServerConfig GetCTFPreset() => new()
        {
            ServerPath = string.Empty,
            ModelPath = string.Empty,
            CtxSize = 65536,
            BatchSize = 1024,
            UBatchSize = 256,
            FlashAttn = true,
            CacheTypeK = "q8_0",
            CacheTypeV = "q8_0",
            Temp = 0.40,
            TopP = 0.88,
            TopK = 30,
            MinP = 0.05,
            RepeatPenalty = 1.08,
            RepeatLastN = 1024,
            FrequencyPenalty = 0.00,
            PresencePenalty = 0,
            Port = 8080,
            Jinja = true,
            Parallel = 1,
            ReasoningPreserve = true,
            ApiKeyEnabled = false,
            ApiKey = string.Empty
        };

        public static ServerConfig GetConversationPreset() => new()
        {
            ServerPath = string.Empty,
            ModelPath = string.Empty,
            CtxSize = 32768,
            BatchSize = 512,
            UBatchSize = 256,
            FlashAttn = true,
            CacheTypeK = "q8_0",
            CacheTypeV = "q8_0",
            Temp = 0.70,
            TopP = 0.95,
            TopK = 40,
            MinP = 0.05,
            RepeatPenalty = 1.10,
            RepeatLastN = 512,
            FrequencyPenalty = 0.00,
            PresencePenalty = 0,
            Port = 8080,
            Jinja = true,
            Parallel = 1,
            ReasoningPreserve = false,
            ApiKeyEnabled = false,
            ApiKey = string.Empty
        };

        public static ServerConfig GetCodeGenerationPreset() => new()
        {
            ServerPath = string.Empty,
            ModelPath = string.Empty,
            CtxSize = 32768,
            BatchSize = 1024,
            UBatchSize = 256,
            FlashAttn = true,
            CacheTypeK = "q8_0",
            CacheTypeV = "q8_0",
            Temp = 0.20,
            TopP = 0.85,
            TopK = 20,
            MinP = 0.05,
            RepeatPenalty = 1.05,
            RepeatLastN = 512,
            FrequencyPenalty = 0.00,
            PresencePenalty = 0,
            Port = 8080,
            Jinja = true,
            Parallel = 1,
            ReasoningPreserve = false,
            ApiKeyEnabled = false,
            ApiKey = string.Empty
        };

        public static ServerConfig GetCreativeWritingPreset() => new()
        {
            ServerPath = string.Empty,
            ModelPath = string.Empty,
            CtxSize = 32768,
            BatchSize = 512,
            UBatchSize = 256,
            FlashAttn = true,
            CacheTypeK = "q8_0",
            CacheTypeV = "q8_0",
            Temp = 0.90,
            TopP = 0.95,
            TopK = 50,
            MinP = 0.05,
            RepeatPenalty = 1.15,
            RepeatLastN = 1024,
            FrequencyPenalty = 0.10,
            PresencePenalty = 0.10,
            Port = 8080,
            Jinja = true,
            Parallel = 1,
            ReasoningPreserve = false,
            ApiKeyEnabled = false,
            ApiKey = string.Empty
        };

        public static ServerConfig GetLargeContextPreset() => new()
        {
            ServerPath = string.Empty,
            ModelPath = string.Empty,
            CtxSize = 131072,
            BatchSize = 2048,
            UBatchSize = 512,
            FlashAttn = true,
            CacheTypeK = "q4_0",
            CacheTypeV = "q4_0",
            Temp = 0.40,
            TopP = 0.88,
            TopK = 30,
            MinP = 0.05,
            RepeatPenalty = 1.08,
            RepeatLastN = 2048,
            FrequencyPenalty = 0.00,
            PresencePenalty = 0,
            Port = 8080,
            Jinja = true,
            Parallel = 1,
            ReasoningPreserve = true,
            ApiKeyEnabled = false,
            ApiKey = string.Empty
        };
    }
}
