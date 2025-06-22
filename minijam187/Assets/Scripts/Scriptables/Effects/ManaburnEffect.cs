using UnityEngine;


[CreateAssetMenu(fileName = "ManaBurn", menuName = "Scriptable Objects/Effect/ManaBurn")]
public class ManaburnEffect : CardEffect
{
    public enum Type { ALL, SPECIFIED }
    public Type type;
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        if (type == Type.ALL)
            amount = GameManager.Instance.Player.mana;
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => LooseMana(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void LooseMana(IEffected origin, IEffected effected, int amount)
    {
        GameManager.Instance.Player.ForceSubtractMana(amount);
        GameManager.Instance.Log.Log(origin.Name + " lost <b>" + amount + "</b> mana.");
    }
}