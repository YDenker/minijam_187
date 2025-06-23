using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAmount : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int amount;
    [SerializeField] private int duration;
    [SerializeField] private GameObject parent;
    [SerializeField] private TMP_Text info;

    public void Populate(int amount, int duration)
    {
        this.amount = amount;
        this.duration = duration;
        info.text = this.amount + " for " + this.duration + " rounds";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        parent.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.SetActive(false);
    }
}
