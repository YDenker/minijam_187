using System.Collections.Generic;
using UnityEngine;

public class HandFanLayout : MonoBehaviour
{
    public List<RectTransform> cardsInHand = new List<RectTransform>();

    public float maxFanAngle = 30f;
    public float cardSpacing = 100f;

    public void UpdateHandLayout()
    {
        int count = cardsInHand.Count;
        float totalWidth = (count - 1) * cardSpacing;

        for (int i = 0; i < count; i++)
        {
            float t = count > 1 ? i /(float)(count - 1) : 0.5f;
            float x = (i - (count - 1) / 2f) * cardSpacing;

            float angle = Mathf.Lerp(maxFanAngle, -maxFanAngle, t);

            RectTransform card = cardsInHand[i];
            card.anchoredPosition = new Vector2(x, 50f);
            card.localRotation = Quaternion.Euler(0, 0, angle);
            card.SetSiblingIndex(i);
        }
    }

    public void AddCard(RectTransform card)
    {
        cardsInHand.Add(card);
        card.SetParent(transform, false);
        UpdateHandLayout();
    }

    public void RemoveCard(RectTransform card)
    {
        cardsInHand.Remove(card);
        Destroy(card);
        UpdateHandLayout();
    }
}
