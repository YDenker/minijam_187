using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "Scriptable Objects/Effect/Damage")]
public class DamageEffect : CardEffect
{
    public DamageType Type = DamageType.LIGHT;

    public override void Apply(IEffected effected, int amount)
    {
        int actual = effected.TakeDamage(this, amount);
        GameManager.Instance.Log.Log("Deal <b>" + actual + " "+Type.ToColoredString() + "</b> damage to " + effected.Name);
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