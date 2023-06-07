using System.IO;

namespace Outpath_Modding.Loader
{
    public class Paths
    {
        public static string Main = Path.Combine(Directory.GetCurrentDirectory(), "OutpathModding");
        public static string Mods = Path.Combine(Main, "Mods");
        public static string Dependencies = Path.Combine(Main, "Dependencies");
        public static string Configs = Path.Combine(Main, "Configs");
        public static string Resources = Path.Combine(Main, "Resources");
        public static string AssetBundles = Path.Combine(Main, "AssetBundles");
        public static string Log = Path.Combine(Main, "Log");

        public static void SetupModPaths()
        {
            if (!Directory.Exists(Main)) Directory.CreateDirectory(Main);
            if (!Directory.Exists(Mods)) Directory.CreateDirectory(Mods);
            if (!Directory.Exists(Dependencies)) Directory.CreateDirectory(Dependencies);
            if (!Directory.Exists(Configs)) Directory.CreateDirectory(Configs);
            if (!Directory.Exists(Resources)) Directory.CreateDirectory(Resources);
            if (!Directory.Exists(AssetBundles)) Directory.CreateDirectory(AssetBundles);
            if (!Directory.Exists(Log)) Directory.CreateDirectory(Log);

            if (!File.Exists(Path.Combine(Log, "LogMessage.txt"))) File.Create(Path.Combine(Log, "LogMessage.txt")).Close();
            if (!File.Exists(Path.Combine(Log, "LogDebug.txt"))) File.Create(Path.Combine(Log, "LogDebug.txt")).Close();
            if (!File.Exists(Path.Combine(Log, "LogInfo.txt"))) File.Create(Path.Combine(Log, "LogInfo.txt")).Close();
            if (!File.Exists(Path.Combine(Log, "LogWarn.txt"))) File.Create(Path.Combine(Log, "LogWarn.txt")).Close();
            if (!File.Exists(Path.Combine(Log, "LogError.txt"))) File.Create(Path.Combine(Log, "LogError.txt")).Close();
        }
    }
}
