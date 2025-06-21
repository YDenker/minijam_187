using UnityEngine;

[CreateAssetMenu(fileName = "Draw", menuName = "Scriptable Objects/Effect/Draw")]
public class DrawEffect : CardEffect
{
    public override void Apply(IEffected effected, int amount)
    {
        GameManager.Instance.Hand.DrawCard(amount);
        GameManager.Instance.Log.Log("Draw <b>" + amount + "</b> " + (amount > 1 ? "cards.": "card"));
    }
}
