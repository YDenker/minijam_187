using UnityEngine;

public interface IEffected
{
    public string Name { get; }
    public int Heal(HealEffect healthEffect, int amount);
    public int TakeDamage(DamageEffect damageEffect, int amount);
    public bool GainStatus(StatusEffect statusEffect, int amount);
    public bool RemoveDebuffs();

    public Vector2 GetEffectOrigin();
    public Vector2 GetEffectTarget();

    public static Vector2 GetPositionInLayer(RectTransform source, RectTransform targetLayer)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, source.position);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetLayer, screenPos, null, out localPoint);
        return localPoint;
    }
}
