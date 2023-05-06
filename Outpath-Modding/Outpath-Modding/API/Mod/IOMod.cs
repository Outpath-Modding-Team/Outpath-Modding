using System;
using System.IO;
using System.Reflection;

namespace Outpath_Modding.API.Mod
{
    public interface IOMod
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public Version Version { get; set; }

        public Version OModdingVersion { get; set; }

        public Assembly ModAssembly { get; set; }

        public DirectoryInfo Directory { get; set; }

        void OnLoaded();

        void OnUnloaded();
    }
}
