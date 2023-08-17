using Outpath_Modding.Loader;
using System;
using System.IO;
using UnityEngine;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.Unity.AssetBundleLoader
{
    public static class AssetBundleLoader
    {
        public static void LoadAssetBundle(CustomAssetBundle customAssetBundle)
        {
            if (customAssetBundle.AssetBundle != null)
                return;

            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
            if (myLoadedAssetBundle == null)
            {
                Logger.Error("Failed to load AssetBundle! Path: " + Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
                return;
            }

            customAssetBundle.AssetBundle = myLoadedAssetBundle;
        }

        public static void ReLoadAssetBundle(CustomAssetBundle customAssetBundle)
        {
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
            if (myLoadedAssetBundle == null)
            {
                Logger.Error("Failed to reload AssetBundle! Path: " + Path.Combine(Paths.AssetBundles, customAssetBundle.Name));
                return;
            }

            customAssetBundle.AssetBundle = myLoadedAssetBundle;
        }

        public static T LoadAsset<T>(this CustomAssetBundle customAssetBundle, string objectName) where T : UnityEngine.Object
        {
            try
            {
                if (customAssetBundle.AssetBundle == null)
                    LoadAssetBundle(customAssetBundle);

                return customAssetBundle.AssetBundle.LoadAsset<T>(objectName);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while loading the asset:\n" + ex);
                return null;
            }
        }

        public static GameObject LoadGameObject(this CustomAssetBundle customAssetBundle, string objectName)
        {
            try
            {
                if (customAssetBundle.AssetBundle == null)
                    LoadAssetBundle(customAssetBundle);

                return customAssetBundle.AssetBundle.LoadAsset<GameObject>(objectName);
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while loading the GameObject:\n" + ex);
                return null;
            }
        }
    }
}
