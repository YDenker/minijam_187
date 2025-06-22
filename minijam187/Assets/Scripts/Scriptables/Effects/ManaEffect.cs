using UnityEngine;

[CreateAssetMenu(fileName = "Mana", menuName = "Scriptable Objects/Effect/Mana")]
public class ManaEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => GainMana(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void GainMana(IEffected origin, IEffected effected, int amount)
    {
        GameManager.Instance.Player.GainMana(amount);
        GameManager.Instance.Log.Log(origin.Name + " gained <b>" + amount + "</b> " + "Mana.");
    }
}