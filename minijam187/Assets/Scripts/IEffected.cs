using UnityEngine;

public interface IEffected
{
    public string Name { get; }
    public int Heal(HealEffect healthEffect, int amount);
    public int TakeDamage(DamageEffect damageEffect, int amount);
    public bool GainStatus(StatusEffect statusEffect, int amount);
}
