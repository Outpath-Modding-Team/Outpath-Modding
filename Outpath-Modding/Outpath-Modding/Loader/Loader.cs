using System;
using System.IO;
using System.Reflection;
using Outpath_Modding.API.Mod;
using System.Collections.Generic;
using Outpath_Modding.GameConsole;
using Outpath_Modding.GameConsole.Components;
using Outpath_Modding.GameConsole.Commands;
using Outpath_Modding.Events;
using Outpath_Modding.API.Config;

namespace Outpath_Modding.Loader
{
    public static class Loader
    {
        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static List<IMod<IConfig>> Mods { get; } = new List<IMod<IConfig>>();

        private static int loadedDependenciesCount;
        private static int loadedModsCount;

        public static void Load()
        {
            Paths.SetupModPaths();
            ConsoleManager.SetupConsole();

            LoadDependencies();
            LoadMods();

            EventsManager.OnPatch();

            CommandManager.AddCommand(new Ping());
            CommandManager.AddCommand(new Clear());
        }

        public static void LoadMods()
        {
            Logger.Info("Loading modules has started!");

            loadedModsCount = 0;

            foreach (string FilePath in Directory.GetFiles(Paths.Mods, "*.dll"))
            {
                try
                {
                    Assembly modAssembly = Assembly.LoadFile(FilePath);

                    foreach (Type type in modAssembly.GetTypes())
                    {
                        if (!IsDerivedFromPlugin(type))
                            continue;

                        IMod<IConfig> oMod = Activator.CreateInstance(type) as IMod<IConfig>;
                        oMod.ModAssembly = modAssembly;
                        Mods.Add(oMod);
                        ConfigManager.LoadConfig(oMod);

                        oMod.OnLoaded();
                    }

                    loadedModsCount++;
                }
                catch (Exception e)
                {
                    loadedModsCount--;
                    Logger.Error($"Mod not loaded in path: {FilePath}");
                    Logger.Error(e.ToString());
                }
            }

            Logger.Info($"Loaded {loadedModsCount} mods!");
        }

        public static void LoadDependencies()
        {
            Logger.Info("Loading dependencies has started!");

            loadedDependenciesCount = 0;

            foreach (string FilePath in Directory.GetFiles(Paths.Dependencies, "*.dll"))
            {
                try
                {
                    string dependencyName = Assembly.LoadFile(FilePath).FullName;
                    Logger.Info($"Loaded {dependencyName} dependence!");
                    loadedDependenciesCount++;
                }
                catch (Exception e)
                {
                    loadedDependenciesCount--;
                    Logger.Error($"Dependence not loaded in path: {FilePath}");
                    Logger.Error(e.ToString());
                }
            }

            Logger.Info($"Loaded {loadedDependenciesCount} dependencies!");
        }

        private static bool IsDerivedFromPlugin(Type type)
        {
            while (type is not null)
            {
                type = type.BaseType;

                if (type is { IsGenericType: true })
                {
                    Type genericTypeDef = type.GetGenericTypeDefinition();

                    if (genericTypeDef == typeof(Mod<>))
                        return true;
                }
            }

            return false;
        }
    }
}