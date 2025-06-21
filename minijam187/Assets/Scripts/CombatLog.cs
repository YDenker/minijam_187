using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatLog : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject logPrefab;

    public void Log(string message)
    {
        GameObject newEntry = Instantiate(logPrefab, content);
        LogEntry logEntry = newEntry.GetComponent<LogEntry>();
        logEntry.SetText(message);
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}