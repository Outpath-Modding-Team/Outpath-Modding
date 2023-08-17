using Outpath_Modding.Events;
using Outpath_Modding.MenuEditor.ModsPanel.Components;
using Outpath_Modding.Unity.AssetBundleLoader;
using UnityEngine;

namespace Outpath_Modding.MenuEditor.ModsPanel
{
    public static class ModsPanelManager
    {
        public static readonly CustomAssetBundle MODSPANEL_ASSETS = new CustomAssetBundle("modspanel");
        public static GameObject ModElementPrefab { get; private set; }
        public static GameObject ModsPanelObj { get; private set; }
        public static ModsPanelController ModsPanelController { get; private set; }

        public static void Setup()
        {
            ModElementPrefab = MODSPANEL_ASSETS.LoadAsset<GameObject>("ModElementItem");

            MenuManager.MainMenuCanvas.AddCustomOptionsButton(new MainMenuCanvas.CustomOptionButton("Mods", "Mods", OnCklick));
            EventsManager.MenuLoaded += OnLoadMenu;
        }

        private static void OnLoadMenu()
        {
            var prefab = MODSPANEL_ASSETS.AssetBundle.LoadAsset<GameObject>("ModsPanel");
            ModsPanelObj = GameObject.Instantiate(prefab, MenuManager.MainMenuCanvas.MainMenuCanvasObject.transform);
            ModsPanelController = ModsPanelObj.AddComponent<ModsPanelController>();
        }

        private static void OnCklick()
        {
            ModsPanelController.OnShowPanel();
        }
    }
}