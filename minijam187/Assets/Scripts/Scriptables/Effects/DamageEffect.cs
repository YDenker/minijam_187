using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "Damage", menuName = "Scriptable Objects/Effect/Damage")]
public class DamageEffect : CardEffect
{
    public DamageType Type = DamageType.LIGHT;

    public override void Apply(IEffected origin, IEffected effected, int amount)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => LooseHealth(origin, effected, amount), GameManager.Instance.EndPlaySelectedCard));
    }

    private void LooseHealth(IEffected origin, IEffected effected, int amount)
    {
        int actual = effected.TakeDamage(this, amount);
        if (actual != amount)
            GameManager.Instance.Log.Log(origin.Name + " deals <b>" + actual + "</b>(" + amount + ") <b>" + Type.ToColoredString() + "</b> damage to " + effected.Name + ".");
        else
            GameManager.Instance.Log.Log(origin.Name + " deals <b>" + amount + " " + Type.ToColoredString() + "</b> damage to " + effected.Name + ".");
    }
}
public enum DamageType { LIGHT, DARK, PHYSICAL }

public static class DamageEffectTypeExtensions
{
    public static string ToColoredString(this DamageType type)
    {
        return type switch
        {
            DamageType.LIGHT => "<color=yellow>light</color>",
            DamageType.DARK => "<color=black>dark</color>",
            DamageType.PHYSICAL => "<color=brown>physical</color>",
            _ => "unknown",
        };
    }
}