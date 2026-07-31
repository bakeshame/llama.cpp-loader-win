using System.Collections.Generic;
using System.Linq;

namespace LlamaCppLoader
{
    public class RecentPaths
    {
        public List<string> ServerPaths { get; set; } = new();
        public List<string> ModelPaths { get; set; } = new();
        public string? LastUsedProfile { get; set; }

        public void AddServerPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            ServerPaths.Remove(path); // Remove if exists
            ServerPaths.Insert(0, path); // Add to front

            // Keep only last 5
            if (ServerPaths.Count > 5)
                ServerPaths = ServerPaths.Take(5).ToList();
        }

        public void AddModelPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            ModelPaths.Remove(path);
            ModelPaths.Insert(0, path);

            if (ModelPaths.Count > 5)
                ModelPaths = ModelPaths.Take(5).ToList();
        }
    }
}
