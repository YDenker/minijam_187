using UnityEngine;

public interface IEffected
{
    public string Name();
    public void Heal(int value);
    public void TakeDamage(int value, Damage.DamageType type);
    public void GainStatus(object status);
}
