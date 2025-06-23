using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static CardData;
using UnityEngine.UI;
public class Deck : MonoBehaviour
{
    // UI References
    [SerializeField] private TMP_Text drawpile_count;
    [SerializeField] private TMP_Text exilepile_count;
    [SerializeField] private TMP_Text discardpile_count;
    [SerializeField] private Image discardpile_image;

    // Art
    [SerializeField] private Sprite discardpile_light;
    [SerializeField] private Sprite discardpile_dark;

    public Vector3 PilePosition => ((RectTransform)transform).anchoredPosition;

    public List<CardRuntimeData> ExilePile = new();
    public List<CardRuntimeData> DrawPile = new();
    public List<CardRuntimeData> DiscardPile = new();

    public void Populate(DeckData deck)
    {
        foreach (CardData card in deck.Cards)
        {
            card.isLightSide = 0.5f > Random.Range(0f, 1f);
            DrawPile.Add(card.GetRuntimeData());
        }
        DrawCounts();
        Shuffle();
    }

    public void Shuffle()
    {
        DrawPile.Shuffle();
        DrawPile.Shuffle();
    }

    public CardRuntimeData DrawFromTop()
    {
        var data = DrawPile.Pop();
        DrawCounts();
        return data;
    }

    public void ShuffleCardIntoDeck(CardRuntimeData card)
    {
        int index = Random.Range(0, DrawPile.Count);
        DrawPile.Insert(index, card);
        DrawCounts();
    }

    public void ShuffleDiscardedIntoDeck()
    {
        foreach (CardRuntimeData card in DiscardPile)
        {
            DrawPile.Add(card);
        }
        DiscardPile.Clear();
        DrawCounts();
        Shuffle();
    }

    public void DiscardCard(CardRuntimeData card)
    {
        DiscardPile.Add(card);
        discardpile_image.sprite = card.isLightSide ? discardpile_light : discardpile_dark;
        DrawCounts();
    }

    public void ExileCard(CardRuntimeData card)
    {
        ExilePile.Add(card);
        DrawCounts();
    }

    private void DrawCounts()
    {
        drawpile_count.text = DrawPile.Count.ToString();
        discardpile_count.text = DiscardPile.Count.ToString();
        exilepile_count.text = ExilePile.Count.ToString();
    }
}
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static T Pop<T>(this IList<T> list)
    {
        if (list.Count == 0) return default;

        int topindex = list.Count - 1;
        T top = list[topindex];
        list.RemoveAt(topindex);
        return top;
    }
}
