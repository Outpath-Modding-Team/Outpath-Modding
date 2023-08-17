using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstalledModsPanel : MonoBehaviour
{
    public Transform ModElementListContent { get; private set; }
    public Transform ModInfoPanel { get; private set; }
    public List<ModElementItem> ModElements { get; private set; }
    public GameObject ModElementPrefab { get; private set; }

    public Image ModInfoImage { get; private set; }
    public TMP_Text ModInfoName { get; private set; }
    public TMP_Text ModInfoDescription { get; private set; }

    private void Awake()
    {
        ModElements = new List<ModElementItem>();
        ModElementListContent = GetComponentInChildren<ScrollRect>().content;
        ModInfoPanel = transform.GetChild(1);
        ModInfoImage = ModInfoPanel.GetChild(0).GetComponentInChildren<Image>();
        ModInfoName = ModInfoPanel.GetChild(1).GetComponent<TMP_Text>();
        ModInfoDescription = ModInfoPanel.GetChild(2).GetComponentInChildren<TMP_Text>();

        ModInfoDescription.gameObject.AddComponent<LinkOpener>();
    }

    public void OnCklickModButton(GameObject gameObject)
    {
        ModElementItem modElement = gameObject.GetComponent<ModElementItem>();
        ModInfoImage.sprite = modElement.Icon;
        ModInfoName.text = modElement.Name;
        ModInfoDescription.text = modElement.Description;
    }
}