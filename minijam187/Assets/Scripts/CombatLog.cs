using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatLog : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private GameObject turnLogPrefab;

    public void LogTurnSwitch(string side, int turn)
    {
        GameObject newEntry = Instantiate(turnLogPrefab, content);
        LogEntry logEntry = newEntry.GetComponent<LogEntry>();
        logEntry.SetText(side + " TURN "+ turn.ToString());
        StartCoroutine(ScrollToBottom());
    }

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