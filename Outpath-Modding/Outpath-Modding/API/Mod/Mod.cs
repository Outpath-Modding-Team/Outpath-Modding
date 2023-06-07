using Outpath_Modding.API.Config;
using Outpath_Modding.GameConsole;
using Outpath_Modding.Loader;
using System;
using System.IO;
using System.Reflection;

namespace Outpath_Modding.API.Mod
{
    public abstract class Mod<TConfig> : IMod<TConfig> where TConfig : IConfig, new()
    {
        public Mod()
        {
            ConfigPath = Path.Combine(Paths.Configs, Name, "config.json");
            ResourcesPath = Path.Combine(Paths.Resources, Name);
            if (!Directory.Exists(ResourcesPath))
                Directory.CreateDirectory(ResourcesPath);
        }

        public virtual string Name { get; set; } = "None";
        public virtual string Author { get; set; } = "None";
        public virtual Version Version { get; set; } = new Version(1, 0, 0);
        public virtual Version OModdingVersion { get; set; } = Loader.Loader.Version;
        public TConfig Config { get; } = new();
        public string ConfigPath { get; }
        public string ResourcesPath { get; }
        public Assembly ModAssembly { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }

        public virtual void OnLoaded()
        {
            Logger.Info($"Loaded mod {Name} v{Version} by {Author}!");
        }

        public virtual void OnUnloaded()
        {
            Logger.Info($"Unloaded mod {Name} v{Version} by {Author}!");
        }
    }
}
