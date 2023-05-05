using Outpath_Modding.GameConsole;
using System;
using System.IO;
using System.Reflection;

namespace Outpath_Modding.Loader
{
    public static class Loader
    {
        private static int loadedDependenciesCount;

        public static void Load()
        {
            Paths.SetupModPaths();

            ConsoleManager.SetupConsole();

            LoadDependencies();
            LoadMods();
        }

        public static void LoadMods()
        {
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