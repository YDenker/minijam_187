using UnityEngine;

[CreateAssetMenu(fileName = "Turn", menuName = "Scriptable Objects/Effect/Turn")]
public class TurnEffect : CardEffect
{
    public CardPosition CardsToTurn = CardPosition.SELF;

    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), () => TurnCard(origin, effected, amount), null, (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void TurnCard(IEffected origin, IEffected effected, int amount)
    {
        Card[] cards;
        switch (CardsToTurn)
        {
            case CardPosition.All:
                cards = GameManager.Instance.Hand.GetAllCards();
                GameManager.Instance.Log.Log(origin.Name + " turns the " + CardsToTurn.ToStringSentence() + " cards around.");
                break;
            case CardPosition.RIGHTMOST:
                cards = GameManager.Instance.Hand.GetRightMostCards(amount);
                if (amount != cards.Length)
                    GameManager.Instance.Log.Log(origin.Name + " turns the <b>" + cards.Length + "</b>("+amount+") " + CardsToTurn.ToStringSentence() + " cards around.");
                else
                    GameManager.Instance.Log.Log(origin.Name + " turns the <b>" + amount + "</b> " + CardsToTurn.ToStringSentence() + " cards around.");
                break;
            case CardPosition.LEFTMOST:
                cards = GameManager.Instance.Hand.GetLeftMostCards(amount);
                if (amount != cards.Length)
                    GameManager.Instance.Log.Log(origin.Name + " turns the <b>" + cards.Length + "</b>(" + amount + ") " + CardsToTurn.ToStringSentence() + " cards around.");
                else
                    GameManager.Instance.Log.Log(origin.Name + " turns the <b>" + amount + "</b> " + CardsToTurn.ToStringSentence() + " cards around.");
                break;
            default:
                cards = new Card[0];
                break;
        }
        TurnCards(cards);
    }

    private void TurnCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            GameManager.Instance.Hand.FlipCard(card, null);
        }
    }
}
