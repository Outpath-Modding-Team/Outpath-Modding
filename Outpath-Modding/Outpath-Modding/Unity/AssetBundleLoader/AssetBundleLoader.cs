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
            Logger.Info("AssetBundle loading started!");

            if (customAssetBundle.LoadedAssets != null && customAssetBundle.LoadedAssets.Length > 0) return;

            Logger.Info("AssetBundle loading continue!");

            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
            if (myLoadedAssetBundle == null)
            {
                Logger.Info("Failed to load AssetBundle! Path: " + Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
                return;
            }

            customAssetBundle.AssetBundle = myLoadedAssetBundle;

            Logger.Info("AssetBundle loading seccsses!");
        }
    }
}
