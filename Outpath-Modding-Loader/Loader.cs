using System;
using System.IO;
using System.Reflection;

namespace Outpath_Modding
{
	public class Loader
	{
		public static string ModErrorPath = Path.Combine(Directory.GetCurrentDirectory(), "LogError.txt");
		public static string ModAssemblePath = Path.Combine(Directory.GetCurrentDirectory(), "Outpath-Modding.dll");
		public static Assembly ModAssembly;
		public static bool IsLoaded;

		public static void Load()
		{
			try
			{
				if (!File.Exists(Loader.ModAssemblePath))
				{
					Loader.IsLoaded = false;
				}

				Loader.ModAssembly = Assembly.LoadFile(Loader.ModAssemblePath);

				if (Loader.ModAssembly != null)
				{
					Loader.IsLoaded = true;
					Loader.LoadModding();
				}
				else
				{
					Loader.IsLoaded = false;
				}
			}
			catch (Exception ex)
			{
				using (StreamWriter streamWriter = File.CreateText(Loader.ModErrorPath))
				{
					streamWriter.WriteLine(ex.ToString());
				}
				Loader.IsLoaded = false;
			}
		}

		public static void LoadModding()
		{
			Loader.ModAssembly.GetType("Outpath_Modding.Loader.Loader").GetMethod("Load").Invoke(null, new object[] { });
		}
	}
}