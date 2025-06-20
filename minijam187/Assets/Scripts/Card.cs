using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private static Color32 NotHover = Color.grey;
    private static Color32 Hover = Color.yellow;

    // UI References
    [SerializeField] private TMP_Text cost_text;
    [SerializeField] private TMP_Text name_text;
    [SerializeField] private TMP_Text effect_text;
    [SerializeField] private RawImage image;
    [SerializeField] private RawImage hower;

    public CardData data;

    // Internal
    private bool isHovered = false;
    private bool isSelected = false;
    public bool isLightSide = true;

    public void Start()
    {
        UpdateVisual();
    }

    public void PlayCard()
    {
        if (isLightSide)
        {
            Debug.Log("Playing light side: " + data.lightSide.effectText);
        }
        else
        {
            Debug.Log("Playing dark side: " + data.darkSide.effectText);
        }
    }

    public void FlipCard()
    {
        isLightSide = !isLightSide;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        name_text.text = data.cardName;
        cost_text.text = isLightSide ? data.lightSide.cost.ToString() : data.darkSide.cost.ToString();
        effect_text.text = isLightSide ? data.lightSide.effectText : data.darkSide.effectText;
        PaintHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.IsSelected)
            return;
        isHovered = false;
        PaintHover();
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.IsSelected)
            return;
        isHovered = true;
        PaintHover();
        transform.localScale = Vector3.one * 1.1f;
    }

    private void PaintHover()
    {
        hower.color = isHovered ? Hover : NotHover;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.IsSelected && !isSelected) return;
        isSelected = !isSelected;

        if (isSelected)
        {
            transform.localScale = Vector3.one * 1.2f;
            GameManager.Instance.Selected = this;
        }
        else
        {
            GameManager.Instance.Selected = null;
            transform.localScale = Vector3.one;
        }
    }
}
