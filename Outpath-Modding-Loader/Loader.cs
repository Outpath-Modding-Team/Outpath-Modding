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

                if (!File.Exists(ModAssemblePath) || !File.Exists(HarmonyAssemblePath))
				{
					IsLoaded = false;
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
					else IsLoaded = false;
				}
				else IsLoaded = false;
            }
			catch (Exception ex)
			{
                IsLoaded = false;
                using (StreamWriter streamWriter = File.CreateText(ModErrorPath))
				{
					streamWriter.WriteLine(ex.ToString());
				}
			}
		}

		public static void LoadModding()
		{
			ModAssembly.GetType("Outpath_Modding.Loader.Loader").GetMethod("Load").Invoke(null, new object[] { });
		}
	}
}