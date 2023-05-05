using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    public static string compilatePath = Application.dataPath + "/AssetBundles";

    [MenuItem("Outpath-Modding/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        try
        {
            CheckDirectory(compilatePath);

            BuildPipeline.BuildAssetBundles(compilatePath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

            Debug.Log("Building AssetBundles completed");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private static void CheckDirectory(string path)
    {
        if (!Directory.Exists(compilatePath))
        {
            Directory.CreateDirectory(compilatePath);
        }

        if (Directory.GetFiles(compilatePath).Length > 0)
        {
            foreach (var item in Directory.GetFiles(compilatePath))
            {
                File.Delete(item);
            }
        }
    }
}
