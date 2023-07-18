using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Outpath_Modding.GameConsole.Components
{
    public class Console : MonoBehaviour
    {
        public static Console Instance { get; private set; }

        public Canvas consoleCanvas { get; private set; }
        public CanvasGroup consoleCanvasGroup { get; private set; }
        public Transform consoleObj { get; private set; }
        public Transform upPanel { get; private set; }
        public Transform downPanel { get; private set; }
        public Transform consoleDownPanel { get; private set; }
        public Transform settingsDownPanel { get; private set; }
        public Transform settingsButtonsObj { get; private set; }
        public Transform settingsPanelsObj { get; private set; }
        public Transform changeKeyPanel { get; private set; }
        public TMP_Text consoleText { get; private set; }
        public TMP_InputField messageInputField { get; private set; }
        public Button sendButton { get; private set; }
        public Button moveButton { get; private set; }
        public Button hideButton_Open { get; private set; }
        public Button hideButton_Close { get; private set; }
        public Button settingsButton { get; private set; }

        public List<ConsoleKey> ConsoleKeys { get; private set; }
        public string currentChangedKeyId { get; private set; }

        public bool isSelect;
        public bool isCloseWindow;
        public bool isOpenSettings;

        void Awake()
        {
            Instance = this;

            consoleCanvas = GetComponent<Canvas>();
            consoleCanvasGroup = GetComponent<CanvasGroup>();
            consoleObj = transform.GetChild(0);
            upPanel = consoleObj.GetChild(0);
            downPanel = consoleObj.GetChild(1);
            consoleDownPanel = downPanel.GetChild(0);
            settingsDownPanel = downPanel.GetChild(1);
            settingsButtonsObj = settingsDownPanel.GetChild(0);
            settingsPanelsObj = settingsDownPanel.GetChild(1);
            changeKeyPanel = settingsDownPanel.GetChild(2);
            consoleText = consoleDownPanel.GetComponentInChildren<ScrollRect>().content.GetComponentInChildren<TMP_Text>();
            messageInputField = consoleDownPanel.GetChild(0).gameObject.AddComponent<ConsoleInputField>();
            sendButton = consoleDownPanel.GetComponentInChildren<Button>();
            moveButton = upPanel.GetChild(2).GetComponent<Button>();
            hideButton_Open = upPanel.GetChild(3).GetComponent<Button>();
            hideButton_Close = upPanel.GetChild(4).GetComponent<Button>();
            settingsButton = upPanel.GetChild(5).GetComponent<Button>();

            moveButton.gameObject.AddComponent<ConsoleEventTrigger>();

            ConsoleKeys = new List<ConsoleKey>()
            {
                new ConsoleKey("ShowHideConsole", KeyCode.F1),
                new ConsoleKey("ResetConsole", KeyCode.F3)
            };

            SetupKeyBindingButtons();
            SetupInterfaceSettings();

            sendButton.onClick.AddListener(SendMessageOnInputField);
            hideButton_Open.onClick.AddListener(OnChangeWindowState);
            hideButton_Close.onClick.AddListener(OnChangeWindowState);
            settingsButton.onClick.AddListener(OnChangeSettingsState);

            SetupSettingsButtons();
            OnChangeSettingsGroup("Interface");

            if (!PlayerPrefs.HasKey("ConsoleTheme"))
            {
                OnChangeConsoleTheme(0);
            }
            else OnChangeConsoleTheme(PlayerPrefs.GetInt("ConsoleTheme"));

            hideButton_Open.gameObject.SetActive(false);
            settingsDownPanel.gameObject.SetActive(false);
            changeKeyPanel.gameObject.SetActive(false);
        }

        void Start()
        {
            ClearConsole();

            if (Logger.LogText.Count > 0)
            {
                foreach (var item in Logger.LogText)
                {
                    switch (item.Type)
                    {
                        case 0:
                            SendLog(item.Context);
                            break;
                        case 1:
                            SendLog(item.Context, item.Color);
                            break;
                        case 2:
                            SendDebug(item.Context);
                            break;
                        case 3:
                            SendInfo(item.Context);
                            break;
                        case 4:
                            SendWarn(item.Context);
                            break;
                        case 5:
                            SendError(item.Context);
                            break;
                    }
                }
            }
        }

        void Update()
        {
            if (isSelect) if (Input.GetKeyUp(KeyCode.Return)) SendMessageOnInputField();

            if (changeKeyPanel.gameObject.activeSelf && Input.anyKeyDown)
            {
                foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
                {
                    bool isKeyExist = false;
                    if (ConsoleKeys.Count(x => x.CurKeyCode == value) > 0)
                    {
                        isKeyExist = true;
                    }
                    if (Input.GetKey(value) && !isKeyExist)
                    {
                        ConsoleKeys.First(x => x.KeyId == currentChangedKeyId).CurKeyCode = value;
                        GetKeyBindingOptionById(currentChangedKeyId).GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = $"{value}";
                        changeKeyPanel.gameObject.SetActive(false);
                        currentChangedKeyId = "";
                    }
                }
            }

            if (changeKeyPanel.gameObject.activeSelf || isSelect)
                return;

            if (Input.GetKeyUp(ConsoleKeys.First(x => x.KeyId == "ShowHideConsole").CurKeyCode))
                OnChangeConsoleState();

            if (Input.GetKeyUp(ConsoleKeys.First(x => x.KeyId == "ResetConsole").CurKeyCode))
                OnResetConsole();
        }

        public void OnChangeConsoleState()
        {
            if (consoleCanvasGroup.alpha == 1)
            {
                consoleCanvasGroup.alpha = 0;
                consoleCanvasGroup.blocksRaycasts = false;
            }
            else
            {
                consoleCanvasGroup.alpha = 1;
                consoleCanvasGroup.blocksRaycasts = true;
            }
        }

        public void OnChangeConsoleTheme(int themeId)
        {
            PlayerPrefs.SetInt("ConsoleTheme", themeId);
            switch (themeId)
            {
                case 0:
                    {
                        SetTheme("#4B4B4B", "#404040");
                        break;
                    }
                case 1:
                    {
                        SetTheme("#000000", "#404040");
                        break;
                    }
                case 2:
                    {
                        SetTheme("#FFFFFF", "#e6e6e6");
                        break;
                    }
                case 3:
                    {
                        SetTheme("#F5F5DC", "#eeeec3");
                        break;
                    }
                case 4:
                    {
                        SetTheme("#0000FF", "#000099");
                        break;
                    }
                case 5:
                    {
                        SetTheme("#082567", "#07215f");
                        break;
                    }
                default:
                    {
                        SetTheme("#4B4B4B", "#404040");
                        break;
                    }

            }
        }

        public void SetTheme(string firstHex, string secondHex)
        {
            ColorUtility.TryParseHtmlString(firstHex, out Color firstColor);
            ColorUtility.TryParseHtmlString(secondHex, out Color secondColor);
            upPanel.GetComponent<Image>().color = firstColor;
            downPanel.GetComponent<Image>().color = firstColor;
            consoleDownPanel.GetComponentInChildren<ScrollRect>().GetComponent<Image>().color = secondColor;
            sendButton.GetComponent<Image>().color = secondColor;
            messageInputField.GetComponent<Image>().color = secondColor;
            foreach (Button button in settingsButtonsObj.GetComponentsInChildren<Button>())
            {
                button.GetComponent<Image>().color = secondColor;
            }
            foreach (Transform panel in settingsPanelsObj)
            {
                panel.GetComponent<Image>().color = secondColor;

                foreach (Transform option in panel.GetComponent<ScrollRect>().content.transform)
                {
                    foreach (Image image in option.GetComponentsInChildren<Image>())
                    {
                        image.color = secondColor;
                    }
                }
            }

        }

        public void OnResetConsole()
        {
            consoleObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            if (isCloseWindow) OnOpenWindow();
        }

        public void OnChangeKey(string keyId)
        {
            currentChangedKeyId = keyId;
            changeKeyPanel.gameObject.SetActive(true);
        }

        public void SetupSettingsButtons()
        {
            foreach (Button button in settingsButtonsObj.GetComponentsInChildren<Button>())
            {
                button.onClick.AddListener(() => OnChangeSettingsGroup(button.name.Replace("Button", "")));
            }
        }

        public void SetupKeyBindingButtons()
        {
            foreach (Transform option in GetSettingsPanelById("KeyBinding").GetComponent<ScrollRect>().content.transform)
            {
                Button button = option.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => OnChangeKey(option.name.Replace("Option", "")));
                button.GetComponentInChildren<TMP_Text>().text = $"{(KeyCode)PlayerPrefs.GetInt(option.name.Replace("Option", ""))}";
            }
        }

        public void SetupInterfaceSettings()
        {
            foreach (Transform option in GetSettingsPanelById("Interface").GetComponent<ScrollRect>().content.transform)
            {
                if (option.name.Contains("Theme"))
                {
                    if (!PlayerPrefs.HasKey("ConsoleTheme"))
                    {
                        option.GetComponentInChildren<TMP_Dropdown>().value = 0;
                    }
                    else option.GetComponentInChildren<TMP_Dropdown>().value = PlayerPrefs.GetInt("ConsoleTheme");
                    option.GetComponentInChildren<TMP_Dropdown>().onValueChanged.AddListener(OnChangeConsoleTheme);
                }
            }
        }

        public Transform GetKeyBindingOptionById(string id)
        {
            foreach (Transform option in GetSettingsPanelById("KeyBinding").GetComponent<ScrollRect>().content.transform)
            {
                if (option.name.Contains(id))
                    return option;
            }
            return null;
        }

        public Transform GetSettingsPanelById(string id)
        {
            foreach (Transform panel in settingsPanelsObj)
            {
                if (panel.name.Contains(id))
                    return panel;
            }
            return null;
        }

        public void OnChangeSettingsState()
        {
            if (isOpenSettings)
            {
                consoleDownPanel.gameObject.SetActive(true);
                settingsDownPanel.gameObject.SetActive(false);
                isOpenSettings = false;
            }
            else
            {
                consoleDownPanel.gameObject.SetActive(false);
                settingsDownPanel.gameObject.SetActive(true);
                isOpenSettings = true;
            }
        }

        public void OnChangeSettingsGroup(string id)
        {
            foreach (Transform panel in settingsPanelsObj)
            {
                if (panel.name.Contains(id))
                    panel.gameObject.SetActive(true);
                else panel.gameObject.SetActive(false);
            }
        }

        public void ClearConsole()
        {
            consoleText.text = "";
        }

        public void OnChangeWindowState()
        {
            if (!isCloseWindow) OnCloseWindow();
            else OnOpenWindow();
        }

        public void OnCloseWindow()
        {
            downPanel.gameObject.SetActive(false);

            upPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 25);
            hideButton_Open.gameObject.SetActive(true);
            hideButton_Close.gameObject.SetActive(false);
            isCloseWindow = true;
        }

        public void OnOpenWindow()
        {
            downPanel.gameObject.SetActive(true);

            upPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 25);
            hideButton_Open.gameObject.SetActive(false);
            hideButton_Close.gameObject.SetActive(true);
            isCloseWindow = false;
        }

        public void OnChangeSelectState(string text)
        {
            isSelect = !isSelect;
        }

        public void SendMessageOnInputField()
        {
            if (string.IsNullOrEmpty(messageInputField.text)) return;

            string text = messageInputField.text;
            messageInputField.text = "";
            consoleText.text = consoleText.text + "\n" + $"[{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}] " + text;
            CommandManager.OnSendCommand(text);
        }

        public void SendError(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            consoleText.text = consoleText.text + "\n" + $"<color=red>[{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}] [Error] " + text + "</color>";
        }

        public void SendWarn(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            consoleText.text = consoleText.text + "\n" + $"<color=yellow>[{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}] [Warn] " + text + "</color>";
        }

        public void SendInfo(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            consoleText.text = consoleText.text + "\n" + $"<color=green>[{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}] [Info] " + text + "</color>";
        }

        public void SendDebug(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            consoleText.text = consoleText.text + "\n" + $"[{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}] [Debug] " + text;
        }

        public void SendLog(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            consoleText.text = consoleText.text + "\n" + $"[{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}] " + text;
        }

        public void SendLog(string text, Color color)
        {
            if (string.IsNullOrEmpty(text)) return;

            consoleText.text = consoleText.text + "\n" + $"<color=#{color.ToHexString()}>[{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}] " + text + "</color>";
        }

        [System.Serializable]
        public class ConsoleKey
        {
            public string KeyId;

            public KeyCode CurKeyCode
            {
                get
                {
                    return _curKeyCode;
                }
                set
                {
                    _curKeyCode = value;
                    PlayerPrefs.SetInt(KeyId, (int)_curKeyCode);
                }
            }

            private KeyCode _curKeyCode;

            public KeyCode DefaultKeyCode;

            public ConsoleKey(string keyId, KeyCode defoultKeyCode)
            {
                KeyId = keyId;
                if (!PlayerPrefs.HasKey(KeyId))
                {
                    CurKeyCode = defoultKeyCode;
                }
                else CurKeyCode = (KeyCode)PlayerPrefs.GetInt(KeyId);
                DefaultKeyCode = defoultKeyCode;
            }
        }
    }
}
