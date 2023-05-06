using Outpath_Modding.GameConsole;
using System;
using System.IO;
using System.Reflection;

namespace Outpath_Modding.API.Mod
{
    public abstract class OMod : IOMod
    {
        public virtual string Name { get; set; } = "None";
        public virtual string Author { get; set; } = "None";
        public virtual Version Version { get; set; } = new Version(1, 0, 0);
        public virtual Version OModdingVersion { get; set; } = Loader.Loader.Version;
        public Assembly ModAssembly { get; set; }
        public DirectoryInfo Directory { get; set; }

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
