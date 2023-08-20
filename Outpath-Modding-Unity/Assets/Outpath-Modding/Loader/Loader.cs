using System;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace Outpath_Modding.Loader
{
    public class Loader : MonoBehaviour
    {
        public static string ModErrorPath;
        public static string ModAssemblePath;
        public static string HarmonyAssemblePath;
        public static Assembly ModAssembly;
        public static Assembly HarmonyAssembly;
        public static bool IsLoaded;

        private void Awake()
        {
            ModErrorPath = Path.Combine(Directory.GetCurrentDirectory(), "LogError.txt");
            ModAssemblePath = Path.Combine(Directory.GetCurrentDirectory(), "Outpath-Modding.dll");
            HarmonyAssemblePath = Path.Combine(Application.dataPath, "Managed", "0Harmony.dll");
            Load();
        }

        public static void Load()
        {
            try
            {
                if (IsLoaded)
                    return;

                if (!File.Exists(ModAssemblePath) || !File.Exists(HarmonyAssemblePath))
                {
                    IsLoaded = false;
#if UNITY_EDITOR
                    if (!File.Exists(ModAssemblePath))
                        Debug.LogWarning("[Outpath-Modding] Mod Assembly not found");
                    if (!File.Exists(HarmonyAssemblePath))
                        Debug.LogWarning("[Outpath-Modding] Harmony Assembly not found");
#endif
                    return;
                }

                HarmonyAssembly = Assembly.LoadFile(HarmonyAssemblePath);

                if (HarmonyAssembly != null)
                {

                    ModAssembly = Assembly.LoadFile(ModAssemblePath);

                    if (ModAssembly != null)
                    {
                        IsLoaded = true;
                        LoadModding();
                    }
                    else
                    {
                        IsLoaded = false;
#if UNITY_EDITOR
                        Debug.LogWarning("[Outpath-Modding] Mod Assembly is null");
#endif
                    }
                }
                else
                {
                    IsLoaded = false;
#if UNITY_EDITOR
                    Debug.LogWarning("[Outpath-Modding] Harmony Assembly is null");
#endif
                }
            }
            catch (Exception ex)
            {
                IsLoaded = false;

                using (StreamWriter streamWriter = File.CreateText(ModErrorPath))
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
            ModAssembly.GetType("Outpath_Modding.Loader").GetMethod("Load").Invoke(null, new object[] { });
        }
    }
}
