using UnityEngine;

[CreateAssetMenu(fileName = "ManaDamage", menuName = "Scriptable Objects/Effect/ManaDamage")]
public class ManaDamageEffect : DamageEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        base.Apply(origin, effected, amount  * GameManager.Instance.Player.mana, fromCard);
    }
}
