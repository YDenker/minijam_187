using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CardData;

public class HandFanLayout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<Card> cardsInHand = new();

    public Deck CardDeck;

    public int maxHandCapacity = 15;

    public float minFanAngle = 10f;
    public float maxFanAngle = 30f;
    public float cardSpacing = 200f;
    public float curveHeight = 80f;
    public float parentPadding = 20f;
    public float minheight = 80f;
    public float maxheight = 120f;

    [Space]
    public Vector3 CardSpawnLocation = Vector3.zero;
    public Vector3 CardDespawnLocation = Vector3.zero;
    [Space]
    [SerializeField] private float drawDelay = 0.2f;
    public CardFlipper flipper = new();
    public CardMover selectMove = new();
    public CardMover fanCardsMove = new();
    public CardMover destroyCardsMove = new();

    //Prefabs
    [Space]
    [SerializeField] private GameObject cardPrefab;

    private int previousIndex = 0;

    private bool cursorOver = false;

    public void MoveCard(Card card, CardMover mover, Vector2 pos, float angle, Action endCallback)
    {
        StartCoroutine(mover.MoveCoroutine(card.RectTransform, pos, angle,endCallback));
    }

    public void FlipCard(Card card, Action endCallback)
    {
        StartCoroutine(flipper.FlipCoroutine(card.RectTransform, card.FlipCard, endCallback));
    }

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

            float fanT = (float)count / (float)maxHandCapacity;
            float fanAngle = Mathf.Lerp(minFanAngle, maxFanAngle, fanT);

            float angle = Mathf.Lerp(fanAngle, -fanAngle, t);

            float y = (cursorOver ?  maxheight : minheight) + (1- Mathf.Pow(2*t -1,2f)* curveHeight);

            Card card = cardsInHand[i];
            MoveCard(card,fanCardsMove,new Vector2(x,y),angle,null);
            card.RectTransform.SetSiblingIndex(i);
        }
    }

    public void SelectCard(Card card)
    {
        previousIndex = cardsInHand.IndexOf(card);
        MoveCard(card,selectMove,new Vector2(0,200f),0f,null);

        card.RectTransform.SetSiblingIndex(transform.childCount - 1);
        GreyOutCards(card);
    }

    public void UnselectCard(Card card)
    {
        card.RectTransform.SetSiblingIndex(previousIndex);
        UpdateHandLayout();
        UnGreyCards();
    }

    public void GreyOutCards(Card exception)
    {
        foreach(Card card in cardsInHand)
        {
            if (card != exception)
            {
                card.Tint.enabled = true;
            }
        }
    }

    public void UnGreyCards()
    {
        foreach(Card card in cardsInHand)
        {
            card.Tint.enabled = false;
        }
    }

    public void DrawCard(int amount)
    {
        StartCoroutine(DrawCard(amount, null));
    }

    public IEnumerator DrawCard(int amount, Action endCallback)
    {
        for (int i = 0; i < amount; i++)
        {
            if (cardsInHand.Count >= maxHandCapacity)
            {
                GameManager.Instance.Log.Log("Maximum hand size reached!");
                break;
            }

            CardRuntimeData data = CardDeck.DrawFromTop();
            if (data == null)
            {
                // TODO Animation maybe??
                yield return new WaitForSeconds(drawDelay*2);
                GameManager.Instance.Log.Log("Deck is empty!");
                ShuffleDiscardIntoDeck();
                data = CardDeck.DrawFromTop();
            }

            if (data == null)
            {
                GameManager.Instance.Log.Log("There are no more cards to draw!");
                break;
            }

            GameObject newCard = Instantiate(cardPrefab, CardDeck.PilePosition, Quaternion.Euler(0,0,90));
            Card card = newCard.GetComponent<Card>();
            card.Populate(data);
            if (!AddCard(card)) Destroy(card.gameObject);
            yield return new WaitForSeconds(drawDelay);
        }
        UpdateHandLayout();
        endCallback?.Invoke();
    }

    public void ShuffleDiscardIntoDeck()
    {
        GameManager.Instance.Log.Log("Shuffeling discard pile into deck.");
        CardDeck.ShuffleDiscardedIntoDeck();
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

    public void RemoveCard(Card card, bool exile = false)
    {
        if (cardsInHand.Contains(card))
            cardsInHand.Remove(card);
        if (exile)
            CardDeck.ExileCard(card.data);
        else
            CardDeck.DiscardCard(card.data);
        MoveCard(card, destroyCardsMove, CardDespawnLocation, 0f, () => Destroy(card.gameObject));
        UpdateHandLayout();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursorOver = false;
        if (!GameManager.Instance.IsSelected)
            UpdateHandLayout();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorOver = true;
        if(!GameManager.Instance.IsSelected)
            UpdateHandLayout();
    }
}
