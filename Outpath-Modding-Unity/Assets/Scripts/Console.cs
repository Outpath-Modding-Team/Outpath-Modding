using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public static Console Instance { get; private set; }

    public Canvas consoleCanvas { get; private set; }
    public Transform consoleObj { get; private set; }
    public Transform upPanel { get; private set; }
    public Transform downPanel { get; private set; }
    public TMP_Text consoleText { get; private set; }
    public TMP_InputField messageInputField { get; private set; }
    public Button sendButton { get; private set; }
    public Button moveButton { get; private set; }
    public Button hideButton_Open { get; private set; }
    public Button hideButton_Close { get; private set; }
    
    public bool isSelect;
    public bool isCloseWindow;

    void Awake()
    {
        Instance = this;

        consoleCanvas = GetComponent<Canvas>();
        consoleObj = transform.GetChild(0);
        upPanel = consoleObj.GetChild(0);
        downPanel = consoleObj.GetChild(1);
        consoleText = downPanel.GetComponentInChildren<ScrollRect>().content.GetComponentInChildren<TMP_Text>();
        messageInputField = downPanel.GetChild(0).gameObject.AddComponent<ConsoleInputField>();
        sendButton = downPanel.GetComponentInChildren<Button>();
        moveButton = upPanel.GetChild(1).GetComponent<Button>();
        hideButton_Open = upPanel.GetChild(2).GetComponent<Button>();
        hideButton_Close = upPanel.GetChild(3).GetComponent<Button>();

        moveButton.gameObject.AddComponent<ConsoleEventTrigger>();

        sendButton.onClick.AddListener(SendMessageOnInputField);
        hideButton_Open.onClick.AddListener(OnChangeWindowState);
        hideButton_Close.onClick.AddListener(OnChangeWindowState);

        hideButton_Open.gameObject.SetActive(false);
    }

    void Start()
    {
        ClearConsole();

        SendError("Test Error");
        SendWarn("Test Warn");
        SendInfo("Test Info");
        SendDebug("Test Debug");
        SendLog("Test Log");
        SendLog("Test Log Color", Color.blue);
    }

    void Update()
    {
        if (isSelect) if (Input.GetKeyUp(KeyCode.Return)) SendMessageOnInputField();
    }

    public void ClearConsole()
    {
        consoleText.text = "";
    }

    public void OnChangeWindowState()
    {
        isCloseWindow = !isCloseWindow;

        if (isCloseWindow) OnCloseWindow();
        else OnOpenWindow();
    }

    public void OnCloseWindow()
    {
        downPanel.gameObject.SetActive(false);

        hideButton_Open.gameObject.SetActive(true);
        hideButton_Close.gameObject.SetActive(false);
    }

    public void OnOpenWindow()
    {
        downPanel.gameObject.SetActive(true);

        hideButton_Open.gameObject.SetActive(false);
        hideButton_Close.gameObject.SetActive(true);
    }

    public void OnChangeSelectState(string text)
    {
        isSelect = !isSelect;
    }

    public void SendMessageOnInputField()
    {
        if (string.IsNullOrEmpty(messageInputField.text)) return;

        consoleText.text = consoleText.text + "\n" + $"[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}] " + messageInputField.text;
        messageInputField.text = "";
    }

    public void SendError(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        consoleText.text = consoleText.text + "\n" + $"<color=red>[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}] [Error] " + text + "</color>";
    }

    public void SendWarn(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        consoleText.text = consoleText.text + "\n" + $"<color=yellow>[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}] [Warn] " + text + "</color>";
    }

    public void SendInfo(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        consoleText.text = consoleText.text + "\n" + $"<color=green>[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}] [Info] " + text + "</color>";
    }

    public void SendDebug(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        consoleText.text = consoleText.text + "\n" + $"[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}] [Debug] " + text;
    }

    public void SendLog(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        consoleText.text = consoleText.text + "\n" + $"[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}] " + text;
    }

    public void SendLog(string text, Color color)
    {
        if (string.IsNullOrEmpty(text)) return;

        consoleText.text = consoleText.text + "\n" + $"<color=#{color.ToHexString()}>[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}] " + text + "</color>";
    }
}
