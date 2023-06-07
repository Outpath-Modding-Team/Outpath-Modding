using Outpath_Modding.API.Config;
using System;
using System.IO;
using System.Reflection;

namespace Outpath_Modding.API.Mod
{
    public interface IMod<out TConfig> where TConfig : IConfig
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public Version Version { get; set; }

        public Version OModdingVersion { get; set; }

        public TConfig Config { get; }

        public string ConfigPath { get; }

        public string ResourcesPath { get; }

        public Assembly ModAssembly { get; set; }

        public DirectoryInfo DirectoryInfo { get; set; }

        void OnLoaded();

        void OnUnloaded();
    }
}
