using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModElementItem : MonoBehaviour
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sprite Icon { get; private set; }

    public Image Image { get; private set; }
    public TMP_Text Text { get; private set; }

    public ModElementItem(string name, string description, Sprite sprite)
    { 
        Name = name;
        Description = description;
        Icon = sprite;

        Image = transform.GetChild(0).GetComponentInChildren<Image>();
        Text = transform.GetComponentInChildren<TMP_Text>();

        Image.sprite = sprite;
    }
}
