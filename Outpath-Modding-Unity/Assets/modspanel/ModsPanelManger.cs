using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModsPanelManger : MonoBehaviour
{
    public CanvasGroup CanvasGroup { get; private set; }
    public Transform UpPanel { get; private set; }
    public Transform DownPanel { get; private set; }
    public Transform WindowButtonsPanel { get; private set; }

    public Button CloseButton { get; private set; }
    public List<Button> WindowButtons { get; private set; }
    public List<Transform> Windows { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        UpPanel = transform.GetChild(0);
        DownPanel = transform.GetChild(1);
        WindowButtonsPanel = UpPanel.GetChild(0);

        WindowButtons = new List<Button>();
        Windows = new List<Transform>();

        CloseButton = UpPanel.GetChild(1).GetComponentInChildren<Button>();

        foreach (Transform buttonObj in WindowButtonsPanel)
        {
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OpenWindow(buttonObj.name.Replace("Button", "")));
            WindowButtons.Add(button);
        }

        foreach (Transform window in DownPanel)
        {
            Windows.Add(window);
        }

        CloseButton.onClick.AddListener(OnHidePanel);

        OpenWindow("InstalledMods");

        ChangeModsPanelState();
    }

    public void ChangeModsPanelState()
    {
        if (CanvasGroup.alpha == 1)
            OnHidePanel();
        else
            OnShowPanel();
    }

    public void OnHidePanel()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.blocksRaycasts = false;
        UpPanel.gameObject.SetActive(false);
        DownPanel.gameObject.SetActive(false);
    }

    public void OnShowPanel()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.blocksRaycasts = true;
        UpPanel.gameObject.SetActive(true);
        DownPanel.gameObject.SetActive(true);
    }

    public void OpenWindow(string id)
    {
        foreach (Transform window in Windows)
        {
            if (window.name.Contains(id))
                window.gameObject.SetActive(true);
            else
                window.gameObject.SetActive(false);
        }
    }
}
