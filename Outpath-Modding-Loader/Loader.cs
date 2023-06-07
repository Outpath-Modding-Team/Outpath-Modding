using System;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace Outpath_Modding
{
	public class LoaderManager
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

                if (!File.Exists(LoaderManager.ModAssemblePath) || !File.Exists(LoaderManager.HarmonyAssemblePath))
                    LoaderManager.IsLoaded = false;

				LoaderManager.HarmonyAssembly = Assembly.LoadFile(LoaderManager.HarmonyAssemblePath);

				if (HarmonyAssembly != null)
				{
					LoaderManager.ModAssembly = Assembly.LoadFile(LoaderManager.ModAssemblePath);

					if (LoaderManager.ModAssembly != null)
					{
						LoaderManager.IsLoaded = true;
						LoaderManager.LoadModding();
					}
					else LoaderManager.IsLoaded = false;
				}
				else LoaderManager.IsLoaded = false;
            }
			catch (Exception ex)
			{
                LoaderManager.IsLoaded = false;
                using (StreamWriter streamWriter = File.CreateText(LoaderManager.ModErrorPath))
				{
					streamWriter.WriteLine(ex.ToString());
				}
			}
		}

		public static void LoadModding()
		{
			LoaderManager.ModAssembly.GetType("Outpath_Modding.Loader.Loader").GetMethod("Load").Invoke(null, new object[] { });
		}
	}
}