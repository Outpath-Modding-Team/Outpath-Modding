using System.IO;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    void Start()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles", "console"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("ConsoleCanvas");
        DontDestroyOnLoad(Instantiate(prefab).AddComponent<Console>().gameObject);

        myLoadedAssetBundle.Unload(false);
    }
}
