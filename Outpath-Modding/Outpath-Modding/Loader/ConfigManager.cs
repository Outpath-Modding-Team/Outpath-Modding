using Newtonsoft.Json;
using Outpath_Modding.API.Config;
using Outpath_Modding.API.Extensions;
using Outpath_Modding.API.Mod;
using Outpath_Modding.GameConsole;
using System;
using System.IO;

namespace Outpath_Modding.Loader
{
    public static class ConfigManager
    {
        public static IConfig GenereteDefoultConfig(IMod<IConfig> mod)
        {
            try
            {
                if (mod.Config != null)
                {
                    string config = JsonConvert.SerializeObject(mod.Config, Formatting.Indented);

                    if (!Directory.Exists(Path.Combine(Paths.Configs, mod.Name)))
                        Directory.CreateDirectory(Path.Combine(Paths.Configs, mod.Name));

                    File.WriteAllText(mod.ConfigPath, "");
                    File.WriteAllText(mod.ConfigPath, config);

                    return mod.Config;
                }
                Logger.Error("Config is null");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("Config error: " + ex.ToString());
                return null;
            }
        }

        public static void LoadConfig(IMod<IConfig> mod)
        {
            if (File.Exists(mod.ConfigPath))
            {
                try
                {
                    mod.Config.CopyProperties((IConfig)JsonConvert.DeserializeObject(File.ReadAllText(mod.ConfigPath), mod.Config.GetType()));
                }
                catch
                {
                    mod.Config.CopyProperties(GenereteDefoultConfig(mod));
                }
            }
            else
            {
                mod.Config.CopyProperties(GenereteDefoultConfig(mod));
            }
        }
    }
}
