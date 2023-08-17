using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.MenuEditor
{
    public class MainMenuCanvas
    {
        public GameObject MainMenuCanvasObject { get; private set; }
        public GameObject OptionsPanel { get; private set; }
        public Dictionary<int, GameObject> OptionsPanelButtons { get; private set; } = new Dictionary<int, GameObject>();

        public List<CustomOptionButton> CustomOptionsPanelButtons { get; private set; } = new List<CustomOptionButton>();

        public void Setup()
        {
            try
            {
                MainMenuCanvasObject = GameObject.FindObjectsOfType<Canvas>().First(x => x.name == "MainMenu Canvas").gameObject;
                OptionsPanelButtons = new Dictionary<int, GameObject>();

                foreach (Transform child in MainMenuCanvasObject.transform)
                    if (child.name == "Options Panel")
                        OptionsPanel = child.gameObject;

                if (OptionsPanel == null)
                {
                    Logger.Error("Option Panel not found!");
                    return;
                }
                else
                {
                    foreach (Transform child in OptionsPanel.transform)
                        OptionsPanelButtons.Add(OptionsPanelButtons.Count, child.gameObject);

                    LoadCustomOptionsButtons();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("[MainMenuCanvas] [Setup] " + ex.ToString());
            }
        }

        private void LoadCustomOptionsButtons()
        {
            if (CustomOptionsPanelButtons.Count <= 0)
                return;

            int buttonCount = 0;

            foreach (var customButton in CustomOptionsPanelButtons)
            {
                GameObject button = GameObject.Instantiate(OptionsPanelButtons[0], OptionsPanel.transform);

                button.transform.SetSiblingIndex(3 + buttonCount);
                button.name = customButton.Name;
                button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                button.GetComponent<Button>().onClick.AddListener(customButton.UnityAction);
                button.GetComponentInChildren<TMP_Text>().text = customButton.Text;
                buttonCount++;
            }
        }

        public void AddCustomOptionsButton(string name, string text, UnityAction unityAction)
            => CustomOptionsPanelButtons.Add(new CustomOptionButton(name, text, unityAction));

        public void AddCustomOptionsButton(CustomOptionButton customOptionButton)
            => CustomOptionsPanelButtons.Add(customOptionButton);

        public class CustomOptionButton
        {
            public string Name;
            public string Text;
            public UnityAction UnityAction;

            public CustomOptionButton(string name, string text, UnityAction unityAction)
            {
                Name = name;
                Text = text;
                UnityAction = unityAction;
            }
        }
    }
}
