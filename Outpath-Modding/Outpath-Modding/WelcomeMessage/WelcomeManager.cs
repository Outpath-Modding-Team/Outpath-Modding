using Outpath_Modding.Unity.AssetBundleLoader;
using Outpath_Modding.WelcomeMessage.Components;
using System;
using UnityEngine;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.WelcomeMessage
{
    public class WelcomeManager
    {
        public static readonly CustomAssetBundle WELCOME_ASSETS = new CustomAssetBundle("welcome");

        public static GameObject WelcomeObj { get; private set; }
        public static WelcomeComponent WelcomeComponent { get; private set; }

        public static void SetupWelcomePanel()
        {
            if (WelcomeComponent != null)
            {
                return;
            }

            try
            {
                var prefab = WELCOME_ASSETS.LoadAsset<GameObject>("WelcomeCanvas");

                if (prefab == null)
                {
                    Logger.Error("prefab is null");
                    return;
                }

                WelcomeObj = GameObject.Instantiate(prefab);

                if (!WelcomeObj)
                {
                    Logger.Error($"Instantiated assetBundle ({WELCOME_ASSETS.Name}) but GameObject is null.");
                    return;
                }

                WelcomeComponent = WelcomeObj.AddComponent<WelcomeComponent>();
            }
            catch (Exception e)
            {
                Logger.Error("Welcome panel loading error: " + e);
            }
        }
    }
}
