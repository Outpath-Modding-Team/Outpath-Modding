using Outpath_Modding.API.Config;
using Outpath_Modding.API.Mod;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Outpath_Modding.MenuEditor.ModsPanel.Components
{
    public class ModElementItem : MonoBehaviour
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }

        public Image Image { get; private set; }
        public TMP_Text Text { get; private set; }

        public IMod<IConfig> SourceMod { get; private set; }

        public void Setup(string name, string description, Sprite sprite, IMod<IConfig> mod)
        {
            Name = name;
            Description = description;
            Icon = sprite;

            Image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            Text = transform.GetComponentInChildren<TMP_Text>();

            if (sprite)
                Image.sprite = sprite;

            Text.text = name;
            SourceMod = mod;
        }

        public void Setup(string name, string description, IMod<IConfig> mod)
        {
            Name = name;
            Description = description;

            Image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            Text = transform.GetComponentInChildren<TMP_Text>();

            Text.text = name;
            SourceMod = mod;
        }
    }
}
