using System.IO;

namespace Outpath_Modding.Loader
{
    public class Paths
    {
        public static string Main = Path.Combine(Directory.GetCurrentDirectory(), "OutpathModding");
        public static string Mods = Path.Combine(Main, "Mods");
        public static string Dependencies = Path.Combine(Main, "Dependencies");
        public static string Configs = Path.Combine(Main, "Configs");
        public static string AssetBundles = Path.Combine(Main, "AssetBundles");
        public static string Log = Path.Combine(Main, "Log");

        public static void SetupModPaths()
        {
            if (!Directory.Exists(Main)) Directory.CreateDirectory(Main);
            if (!Directory.Exists(Mods)) Directory.CreateDirectory(Mods);
            if (!Directory.Exists(Dependencies)) Directory.CreateDirectory(Dependencies);
            if (!Directory.Exists(Configs)) Directory.CreateDirectory(Configs);
            if (!Directory.Exists(AssetBundles)) Directory.CreateDirectory(AssetBundles);
            if (!Directory.Exists(Log)) Directory.CreateDirectory(Log);

            if (!File.Exists(Path.Combine(Paths.Log, "LogMessage.txt"))) File.Create(Path.Combine(Paths.Log, "LogMessage.txt"));
            if (!File.Exists(Path.Combine(Paths.Log, "LogDebug.txt"))) File.Create(Path.Combine(Paths.Log, "LogDebug.txt"));
            if (!File.Exists(Path.Combine(Paths.Log, "LogInfo.txt"))) File.Create(Path.Combine(Paths.Log, "LogInfo.txt"));
            if (!File.Exists(Path.Combine(Paths.Log, "LogWarn.txt"))) File.Create(Path.Combine(Paths.Log, "LogWarn.txt"));
            if (!File.Exists(Path.Combine(Paths.Log, "LogError.txt"))) File.Create(Path.Combine(Paths.Log, "LogError.txt"));
        }
    }
}
