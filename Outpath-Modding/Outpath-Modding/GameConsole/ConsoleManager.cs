using System;
using UnityEngine;
using Outpath_Modding.Unity.AssetBundleLoader;
using Console = Outpath_Modding.GameConsole.Components.Console;

namespace Outpath_Modding.GameConsole
{
    public static class ConsoleManager
    {
        public static readonly CustomAssetBundle CONSOLE_ASSETS = new CustomAssetBundle("console");

        public static GameObject ConsoleObj { get; private set; }
        public static Console Console { get; private set; }

        public static void SetupConsole()
        {
            Logger.Debug("Console loading...");

            if (Console != null)
            {
                Logger.Debug("Console is loaded!");
                return;
            }

            try
            {
                AssetBundleLoader.LoadAssetBundle(CONSOLE_ASSETS);

                var prefab = CONSOLE_ASSETS.AssetBundle.LoadAsset<GameObject>("ConsoleCanvas");

                if (prefab == null)
                {
                    Logger.Error("prefab is null");
                    return;
                }

                ConsoleObj = GameObject.Instantiate(prefab);
                GameObject.DontDestroyOnLoad(ConsoleObj);

                if (!ConsoleObj)
                {
                    Logger.Error($"Instantiated assetBundle ({CONSOLE_ASSETS.Name}) but GameObject is null.");
                    return;
                }

                CONSOLE_ASSETS.LoadedAssets = new UnityEngine.Object[] { ConsoleObj };

                Console = ConsoleObj.AddComponent<Console>();

                Logger.Debug("Console is loaded!");
            }
            catch (Exception e)
            {
                Logger.Error("Console loading error: " + e);
            }
        }
    }
}
