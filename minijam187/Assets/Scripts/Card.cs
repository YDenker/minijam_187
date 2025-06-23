using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CardData;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // UI References
    [SerializeField] private TMP_Text cost_text;
    [SerializeField] private TMP_Text name_text;
    [SerializeField] private TMP_Text effect_text;
    [SerializeField] private Image image;
    [SerializeField] private RawImage background;
    [SerializeField] private RawImage foreground;

    // Art
    [SerializeField] private Sprite light_background;
    [SerializeField] private Sprite dark_background;
    [SerializeField] private Sprite light_background_highlight;
    [SerializeField] private Sprite dark_background_highlight;

    public CardRuntimeData data;

    public RectTransform RectTransform;

    public RawImage Tint => foreground;

    // Internal
    private bool isHovered = false;
    private bool isSelected = false;

    public void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Populate(CardRuntimeData data)
    {
        this.data = data;
        UpdateVisual();
    }

    public void FlipCard()
    {
        data.isLightSide = !data.isLightSide;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (data == null) return;
        name_text.text = data.isLightSide ? data.lightSide.cardName : data.darkSide.cardName;
        cost_text.text = data.isLightSide ? data.lightSide.cost.ToString() : data.darkSide.cost.ToString();
        effect_text.text = data.isLightSide ? data.lightSide.EffectText : data.darkSide.EffectText;
        Sprite tmp = data.isLightSide ? data.lightSide.art : data.darkSide.art;
        if (tmp != null)
            image.sprite = tmp;
        foreground.enabled = false;
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
        background.texture = data.isLightSide ? (isHovered ? light_background_highlight.texture : light_background.texture) : (isHovered ? dark_background_highlight.texture : dark_background.texture);
    }

    public void HandleSelection()
    {
        if (GameManager.Instance.IsSelected && !isSelected) return;
        isSelected = !isSelected;

        if (isSelected)
        {
            transform.localScale = Vector3.one * 1.2f;
            GameManager.Instance.SelectCard(this);
        }
        else
        {
            GameManager.Instance.UnselectCard(this);
            transform.localScale = Vector3.one;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        HandleSelection();
    }

}
