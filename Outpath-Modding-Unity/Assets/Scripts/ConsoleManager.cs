using System.IO;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    void Start()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles", "console"));

        var welcomeAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles", "welcome"));

        if (myLoadedAssetBundle == null || welcomeAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("ConsoleCanvas");
        DontDestroyOnLoad(Instantiate(prefab).AddComponent<Console>().gameObject);

        if (PlayerPrefs.GetInt("WelcomeMSG", 0) == 0)
        {
            var prefabWelcome = welcomeAssetBundle.LoadAsset<GameObject>("WelcomeCanvas");
            Instantiate(prefabWelcome).AddComponent<WelcomeMessage>();
        }

        myLoadedAssetBundle.Unload(false);
        welcomeAssetBundle.Unload(false);
    }
}
