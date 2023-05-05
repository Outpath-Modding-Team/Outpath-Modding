using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsoleEventTrigger : EventTrigger
{
    private void Awake()
    {
        if (this.triggers.Count(x => x.eventID == EventTriggerType.Drag) < 1)
            this.triggers.Add(new Entry() { eventID = EventTriggerType.Drag });
        this.triggers.First(x => x.eventID == EventTriggerType.Drag).callback.AddListener(OnDragThis);
    }

    public void OnDragThis(BaseEventData eventData)
    {
        PointerEventData pointerDate = (PointerEventData)eventData;
        ((RectTransform)Console.Instance.consoleObj.transform).anchoredPosition += pointerDate.delta / Console.Instance.consoleCanvas.scaleFactor;
    }
}
