using UnityEngine;

[CreateAssetMenu(fileName = "Draw", menuName = "Scriptable Objects/Effect/Draw")]
public class DrawEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => DrawCards(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void DrawCards(IEffected origin, IEffected effected, int amount)
    {
        GameManager.Instance.Hand.DrawCard(amount);
        GameManager.Instance.Log.Log(origin.Name + " draws <b>" + amount + "</b> " + (amount > 1 ? "cards." : "card."));
    }
}
