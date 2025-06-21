using System.Collections.Generic;
using UnityEngine;

public class HandFanLayout : MonoBehaviour
{
    public List<Card> cardsInHand = new();

    public int maxHandCapacity = 15;

    public float maxFanAngle = 30f;
    public float cardSpacing = 200f;
    public float curveHeight = 80f;
    public float parentPadding = 20f;

    private int previousIndex = 0;

    public void UpdateHandLayout()
    {
        int count = cardsInHand.Count;
        float parentWidth = ((RectTransform)transform).rect.width - parentPadding;
        float totalWidth = (count - 1) * cardSpacing;
        float spacing = (totalWidth > parentWidth) ? parentWidth / Mathf.Max(count - 1, 1) : cardSpacing;

        for (int i = 0; i < count; i++)
        {
            float t = count > 1 ? i /(float)(count - 1) : 0.5f;
            float x = (i - (count - 1) / 2f) * spacing;

            float angle = Mathf.Lerp(maxFanAngle, -maxFanAngle, t);

            float y = 80f + (1- Mathf.Pow(2*t -1,2f)* curveHeight);

            RectTransform card = cardsInHand[i].RectTransform;
            card.anchoredPosition = new Vector2(x, y);
            card.localRotation = Quaternion.Euler(0, 0, angle);
            card.SetSiblingIndex(i);
        }
    }

    public void SelectCard(Card card)
    {
        previousIndex = cardsInHand.IndexOf(card);
        card.RectTransform.localRotation = Quaternion.identity;
        card.RectTransform.anchoredPosition = new Vector2(0, 200f);
        card.RectTransform.SetSiblingIndex(transform.childCount - 1);
    }

    public void UnselectCard(Card card)
    {
        card.RectTransform.SetSiblingIndex(previousIndex);
        UpdateHandLayout();
    }

    public bool AddCard(Card card)
    {
        if (cardsInHand.Count >= maxHandCapacity)
            return false;
        cardsInHand.Add(card);
        card.RectTransform.SetParent(transform, false);
        UpdateHandLayout();
        return true;
    }

    public void RemoveCard(Card card)
    {
        cardsInHand.Remove(card);
        Destroy(card.gameObject);
        UpdateHandLayout();
    }
}
