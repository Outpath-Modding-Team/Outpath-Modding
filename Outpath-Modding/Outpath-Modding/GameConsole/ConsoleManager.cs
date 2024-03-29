﻿using System;
using UnityEngine;
using Console = Outpath_Modding.GameConsole.Components.Console;

namespace Outpath_Modding.GameConsole
{
    public static class ConsoleManager
    {
        public static GameObject ConsoleObj { get; private set; }
        public static Console Console { get; private set; }

        public static void SetupConsole()
        {
            Logger.Info("Console loading...");

            if (Console != null)
            {
                Logger.Info("Console is loaded!");
                return;
            }

            try
            {
                var prefab = Resources.Load<GameObject>("OutpathModding/ConsoleCanvas");

                if (prefab == null)
                {
                    Logger.Error("prefab is null");
                    return;
                }

                ConsoleObj = GameObject.Instantiate(prefab);
                GameObject.DontDestroyOnLoad(ConsoleObj);

                if (!ConsoleObj)
                {
                    Logger.Error($"Console gameObject is null.");
                    return;
                }

                Console = ConsoleObj.AddComponent<Console>();

                Logger.Info("Console is loaded!");
            }
            catch (Exception e)
            {
                Logger.Error("Console loading error: " + e);
            }
        }
    }
}
