using UnityEngine;

namespace Outpath_Modding.Unity.AssetBundleLoader
{
    public class CustomAssetBundle
    {
        public string Name { get; }

        public AssetBundle AssetBundle { get; set; }

        public CustomAssetBundle(string name)
        {
            Name = name;
        }
    }
}
