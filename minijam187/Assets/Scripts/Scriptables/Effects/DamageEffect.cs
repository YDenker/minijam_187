using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "Scriptable Objects/Effect/Damage")]
public class DamageEffect : CardEffect
{
    public DamageType Type = DamageType.LIGHT;

    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => LooseHealth(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    public void ApplyRoundBased(IEffected target, int amount, Action endCallback)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(target.GetEffectOrigin(), target.GetEffectTarget(), null, () => LooseHealth(target, target, amount), endCallback));
    }
    private void LooseHealth(IEffected origin, IEffected effected, int amount)
    {
        if (Type == DamageType.SELF)
        {
            int actual = origin.TakeDamage(this, amount);
            GameManager.Instance.Log.Log(origin.Name + " took <b>" + actual + "</b> damage.");
        }
        else
        {
            int actual = effected.TakeDamage(this, amount);
            if (actual != amount)
                GameManager.Instance.Log.Log(origin.Name + " deals <b>" + actual + "</b>(" + amount + ") <b>" + Type.ToColoredString() + "</b> damage to " + effected.Name + ".");
            else
                GameManager.Instance.Log.Log(origin.Name + " deals <b>" + amount + " " + Type.ToColoredString() + "</b> damage to " + effected.Name + ".");
        }
    }
}
public enum DamageType { LIGHT, DARK, SELF, PHYSICAL, FIRE, POISON }

public static class DamageEffectTypeExtensions
{
    public static string ToColoredString(this DamageType type)
    {
        return type switch
        {
            DamageType.LIGHT => "<color=yellow>light</color>",
            DamageType.DARK => "<color=#595857>dark</color>",
            DamageType.PHYSICAL => "<color=brown>physical</color>",
            DamageType.FIRE => "<color=red>burn</color>",
            DamageType.POISON => "<color=green>poison</color>",
            _ => "unknown",
        };
    }
}