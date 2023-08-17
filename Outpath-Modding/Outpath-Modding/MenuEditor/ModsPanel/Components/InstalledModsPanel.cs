using Outpath_Modding.API.Config;
using Outpath_Modding.API.Mod;
using Outpath_Modding.Unity;
using Outpath_Modding.WelcomeMessage.Components;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Outpath_Modding.MenuEditor.ModsPanel.Components
{
    public class InstalledModsPanel : MonoBehaviour
    {
        public Transform ModElementListContent { get; private set; }
        public Transform ModInfoPanel { get; private set; }
        public List<ModElementItem> ModElements { get; private set; }

        public Image ModInfoImage { get; private set; }
        public TMP_Text ModInfoName { get; private set; }
        public TMP_Text ModInfoDescription { get; private set; }

        private void Awake()
        {
            ModElements = new List<ModElementItem>();
            ModElementListContent = GetComponentInChildren<ScrollRect>().content;
            ModInfoPanel = transform.GetChild(1);
            ModInfoImage = ModInfoPanel.GetChild(0).GetChild(0).GetComponent<Image>();
            ModInfoName = ModInfoPanel.GetChild(1).GetComponent<TMP_Text>();
            ModInfoDescription = ModInfoPanel.GetChild(2).GetComponentInChildren<TMP_Text>();

            ModInfoDescription.gameObject.AddComponent<LinkOpener>();

            ModInfoName.text = "";
            ModInfoDescription.text = "";
            ModInfoPanel.GetChild(0).gameObject.SetActive(false);
            //ModInfoPanel.gameObject.SetActive(false);
        }

        public void OnCklickModButton(GameObject gameObject)
        {
            ModInfoPanel.GetChild(0).gameObject.SetActive(true);
            ModElementItem modElement = gameObject.GetComponent<ModElementItem>();
            if (modElement.Icon)
                ModInfoImage.sprite = modElement.Icon;
            ModInfoName.text = modElement.Name;
            ModInfoDescription.text = modElement.Description;
        }

        public void AddModToList(IMod<IConfig> mod)
        {
            if (File.Exists(Path.Combine(mod.ResourcesPath, "modicon.png")))
            {
                GameObject modElementObj = Instantiate(ModsPanelManager.ModElementPrefab, ModElementListContent);
                modElementObj.AddComponent<ModElementItem>().Setup(mod.Name, mod.Description, ImageLoader.LoadSprite(Path.Combine(mod.ResourcesPath, "modicon.png")), mod);
                modElementObj.GetComponent<Button>().onClick.AddListener(() => OnCklickModButton(modElementObj));
            }
            else
            {
                GameObject modElementObj = Instantiate(ModsPanelManager.ModElementPrefab, ModElementListContent);
                modElementObj.AddComponent<ModElementItem>().Setup(mod.Name, mod.Description, mod);
                modElementObj.GetComponent<Button>().onClick.AddListener(() => OnCklickModButton(modElementObj));
            }
        }
    }
}
