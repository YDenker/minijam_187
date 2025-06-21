using UnityEngine;

public class LogEntry : MonoBehaviour
{
    [SerializeField] private RectTransform parentEntry;
    [SerializeField] private TMPro.TMP_Text childEntry;

    public void SetText(string message)
    {
        childEntry.text = message;
        Canvas.ForceUpdateCanvases();

        parentEntry.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((RectTransform)childEntry.transform).rect.height);
    }
}
