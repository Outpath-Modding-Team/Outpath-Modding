using HarmonyLib;
using Outpath_Modding.API.Features;
using Outpath_Modding.API.Features.Item;
using Outpath_Modding.Events.EventArguments;
using Outpath_Modding.MenuEditor;
using System;
using UnityEngine.SceneManagement;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.Events
{
    public static class EventsManager
    {
        private static readonly Harmony Harmony = new("com.outpath.moddingframework");

        public delegate void OutputhEventHandler<in T>(T ev) where T : EventArgs;

        public delegate void OutputhEventHandler();

        public static event OutputhEventHandler<ChangedSceneEventArgs> ChangedScene;

        public static event OutputhEventHandler<PickupedItemEventArgs> PickupedItem;

        public static event OutputhEventHandler<TakeOutResourceEventArgs> TakeOutResource;

        public static event OutputhEventHandler<SetItemToCraftEventArgs> SetItemToCraft;

        public static event OutputhEventHandler<SetItemToInfiniteCraftEventArgs> SetItemToInfiniteCraft;

        public static event OutputhEventHandler GameLoaded;

        public static event OutputhEventHandler MenuLoaded;

        public static void InvokeEvent<T>(this OutputhEventHandler<T> eventHandler, T args)
        where T : EventArgs
        {
            if (eventHandler is null)
                return;

            foreach (Delegate sub in eventHandler.GetInvocationList())
            {
                try
                {
                    sub.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    Logger.Error("An error occurred while handling the event " + eventHandler.GetType().Name + " : \n" + e.ToString());
                    throw;
                }
            }
        }

        public static void InvokeEvent(this OutputhEventHandler eventHandler)
        {
            if (eventHandler is null)
                return;

            foreach (Delegate sub in eventHandler.GetInvocationList())
            {
                try
                {
                    sub.DynamicInvoke();
                }
                catch (Exception e)
                {
                    Logger.Error("An error occurred while handling the event " + eventHandler.GetType().Name + " : \n" + e.ToString());
                    throw;
                }
            }
        }

        public static void OnChangedScene(ChangedSceneEventArgs eventArgs)
        {
            ChangedScene.InvokeEvent(eventArgs);
        }

        public static void OnPickupedItem(PickupedItemEventArgs eventArgs)
        {
            PickupedItem.InvokeEvent(eventArgs);
        }

        public static void OnTakeOutResource(TakeOutResourceEventArgs eventArgs)
        {
            TakeOutResource.InvokeEvent(eventArgs);
        }

        public static void OnSetItemToCraft(SetItemToCraftEventArgs eventArgs)
        {
            SetItemToCraft.InvokeEvent(eventArgs);
        }

        public static void OnSetItemToInfiniteCraft(SetItemToInfiniteCraftEventArgs eventArgs)
        {
            SetItemToInfiniteCraft.InvokeEvent(eventArgs);
        }

        public static void OnGameLoaded()
        {
            GameLoaded.InvokeEvent();
        }

        public static void OnMenuLoaded()
        {
            MenuLoaded.InvokeEvent();
        }

        public static void OnPatch()
        {
            try
            {
                Logger.Info("Patching events...");

                Harmony.PatchAll();

                Logger.Info("Patch completed successfully!");

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred during patching: \n" + ex.ToString());
            }
        }

        public static void OnUnpatch()
        {
            try
            {
                Logger.Info("Unpatching events...");

                Harmony.UnpatchAll();

                Logger.Info("Unpatching completed successfully!");
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred during unpatching: \n" + ex.ToString());
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            OnChangedScene(new ChangedSceneEventArgs(scene, loadSceneMode));

            if (scene.name == "Scene_Game")
            {
                CustomItemInfo.RegisterAllItems();
                IslandBlock.AddAllPropsToSpawn();
                OnGameLoaded();
            }
            else if (scene.name == "Scene_MainMenu")
            {
                MenuManager.MainMenuCanvas.Setup();
                OnMenuLoaded();
            }
        }
    }
}