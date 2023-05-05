using TMPro;
using UnityEngine.EventSystems;

public class ConsoleInputField : TMP_InputField
{
    protected override void Awake()
    {
        this.textComponent = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        this.placeholder = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        base.Awake();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        Console.Instance.isSelect = true;
        base.OnSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        Console.Instance.isSelect = false;
        base.OnDeselect(eventData);
    }
}
