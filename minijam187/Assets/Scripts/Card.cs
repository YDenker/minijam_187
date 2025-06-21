using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    // UI References
    [SerializeField] private TMP_Text cost_text;
    [SerializeField] private TMP_Text name_text;
    [SerializeField] private TMP_Text effect_text;
    [SerializeField] private RawImage image;
    [SerializeField] private RawImage background;

    // Art
    [SerializeField] private Sprite light_background;
    [SerializeField] private Sprite dark_background;
    [SerializeField] private Sprite light_background_highlight;
    [SerializeField] private Sprite dark_background_highlight;

    public CardData data;

    public RectTransform RectTransform;

    // Internal
    private bool isHovered = false;
    private bool isSelected = false;
    public bool isLightSide = true;

    public void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        UpdateVisual();
    }

    public IEnumerator FlipCard(Action callback)
    {
        isLightSide = !isLightSide;
        UpdateVisual();
        // DO THE ACTION FLIP ANIMATION HERE
        yield return new WaitForSeconds(2f);
        callback?.Invoke();
    }

    public void UpdateVisual()
    {
        name_text.text = isLightSide ? data.lightSide.cardName : data.darkSide.cardName;
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
        background.texture = isLightSide ? (isHovered ? light_background_highlight.texture : light_background.texture) : (isHovered ? dark_background_highlight.texture : dark_background.texture);
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
        HandleSelection();
    }

}
