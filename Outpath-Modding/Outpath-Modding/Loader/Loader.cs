using System;
using System.IO;
using System.Reflection;
using Outpath_Modding.API.Mod;
using System.Collections.Generic;
using Outpath_Modding.GameConsole;
using Outpath_Modding.GameConsole.Components;
using Outpath_Modding.GameConsole.Commands;

namespace Outpath_Modding.Loader
{
    public static class Loader
    {
        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static List<IOMod> Mods { get; } = new List<IOMod>();

        private static int loadedDependenciesCount;
        private static int loadedModsCount;

        public static void Load()
        {
            Paths.SetupModPaths();

            ConsoleManager.SetupConsole();

            LoadDependencies();
            LoadMods();

            CommandManager.AddCommand(new Ping());
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
                        if (!type.IsSubclassOf(typeof(OMod))) continue;

                        IOMod oMod = Activator.CreateInstance(type) as IOMod;
                        oMod.ModAssembly = modAssembly;
                        Mods.Add(oMod);

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
    }
}