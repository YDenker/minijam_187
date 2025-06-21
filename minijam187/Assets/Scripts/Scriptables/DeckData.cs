using UnityEngine;

[CreateAssetMenu(fileName = "newDeck", menuName = "Scriptable Objects/Deck")]
public class DeckData : ScriptableObject
{
    public CardData[] Cards;
}
