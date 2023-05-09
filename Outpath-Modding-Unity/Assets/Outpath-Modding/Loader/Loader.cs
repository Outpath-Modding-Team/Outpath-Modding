using System;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace Outpath_Modding.Loader
{
    public class Loader : MonoBehaviour
    {
        public static string ModErrorPath = Path.Combine(Directory.GetCurrentDirectory(), "LogError.txt");
        public static string ModAssemblePath = Path.Combine(Directory.GetCurrentDirectory(), "Outpath-Modding.dll");
        public static string HarmonyAssemblePath = Path.Combine(Application.dataPath, "Managed", "0Harmony.dll");
        public static Assembly ModAssembly;
        public static Assembly HarmonyAssembly;
        public static bool IsLoaded;

        public static void Load()
        {
            try
            {
                if (IsLoaded)
                    return;

                if (!File.Exists(Loader.ModAssemblePath) || !File.Exists(Loader.HarmonyAssemblePath))
                {
                    Loader.IsLoaded = false;
#if UNITY_EDITOR
                    Debug.LogWarning(!File.Exists(Loader.ModAssemblePath) ? "[Outpath-Modding] Mod Assembly not found" : "[Outpath-Modding] Harmony Assembly not found");
#endif
                }

                Loader.HarmonyAssembly = Assembly.LoadFile(Loader.HarmonyAssemblePath);

                if (HarmonyAssembly != null)
                {

                    Loader.ModAssembly = Assembly.LoadFile(Loader.ModAssemblePath);

                    if (Loader.ModAssembly != null)
                    {
                        Loader.IsLoaded = true;
                        Loader.LoadModding();
                    }
                    else
                    {
                        Loader.IsLoaded = false;
#if UNITY_EDITOR
                        Debug.LogWarning("[Outpath-Modding] Mod Assembly is null");
#endif
                    }
                }
                else
                {
                    Loader.IsLoaded = false;
#if UNITY_EDITOR
                    Debug.LogWarning("[Outpath-Modding] Harmony Assembly is null");
#endif
                }
            }
            catch (Exception ex)
            {
                Loader.IsLoaded = false;

                using (StreamWriter streamWriter = File.CreateText(Loader.ModErrorPath))
                {
                    streamWriter.WriteLine(ex.ToString());
                }
#if UNITY_EDITOR
                Debug.LogException(ex);
#endif
            }
        }

        public static void LoadModding()
        {
            Loader.ModAssembly.GetType("Outpath_Modding.Loader.Loader").GetMethod("Load").Invoke(null, new object[] { });
        }
    }
}
