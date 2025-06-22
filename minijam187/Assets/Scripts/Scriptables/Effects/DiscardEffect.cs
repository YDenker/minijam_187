using UnityEngine;

[CreateAssetMenu(fileName = "Discard", menuName = "Scriptable Objects/Effect/Discard")]
public class DiscardEffect : CardEffect
{
    public CardPosition CardsToDiscard = CardPosition.SELF;

    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), () => DiscardCard(origin, effected, amount), null, (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void DiscardCard(IEffected origin, IEffected effected, int amount)
    {
        Card[] cards;
        switch (CardsToDiscard)
        {
            case CardPosition.All:
                cards = GameManager.Instance.Hand.GetAllCards();
                GameManager.Instance.Log.Log(origin.Name + " discards the " + CardsToDiscard.ToStringSentence() + " cards.");
                break;
            case CardPosition.RIGHTMOST:
                cards = GameManager.Instance.Hand.GetRightMostCards(amount);
                if (amount != cards.Length)
                    GameManager.Instance.Log.Log(origin.Name + " discards the <b>" + cards.Length + "</b>(" + amount + ") " + CardsToDiscard.ToStringSentence() + " cards.");
                else
                    GameManager.Instance.Log.Log(origin.Name + " discards the <b>" + amount + "</b> " + CardsToDiscard.ToStringSentence() + " cards.");
                break;
            case CardPosition.LEFTMOST:
                cards = GameManager.Instance.Hand.GetLeftMostCards(amount);
                if (amount != cards.Length)
                    GameManager.Instance.Log.Log(origin.Name + " discards the <b>" + cards.Length + "</b>(" + amount + ") " + CardsToDiscard.ToStringSentence() + " cards.");
                else
                    GameManager.Instance.Log.Log(origin.Name + " discards the <b>" + amount + "</b> " + CardsToDiscard.ToStringSentence() + " cards.");
                break;
            default:
                cards = new Card[0];
                break;
        }
        DiscardCards(cards);
    }

    private void DiscardCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            GameManager.Instance.Hand.RemoveCard(card);
        }
    }
}
