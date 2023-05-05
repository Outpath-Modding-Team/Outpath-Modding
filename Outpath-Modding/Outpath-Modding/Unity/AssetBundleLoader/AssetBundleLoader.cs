using System.IO;
using UnityEngine;
using Outpath_Modding.Loader;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.Unity.AssetBundleLoader
{
    public static class AssetBundleLoader
    {
        public static void LoadAssetBundle(CustomAssetBundle customAssetBundle)
        {
            Logger.Debug("Loading AssetBundle has been started!");

            if (customAssetBundle.LoadedAssets != null && customAssetBundle.LoadedAssets.Length > 0) return;

            Logger.Debug("Loading AssetBundle continue!");

            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
            if (myLoadedAssetBundle == null)
            {
                Logger.Debug("Failed to load AssetBundle! Path: " + Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
                return;
            }

            customAssetBundle.AssetBundle = myLoadedAssetBundle;

            Logger.Debug("Loading AssetBundle seccsses!");
        }
    }
}
