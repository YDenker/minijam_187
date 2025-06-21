using UnityEngine;

public abstract class Entity : MonoBehaviour, IEffected
{
    public virtual string Name() { return "Unknown"; }
    public virtual void GainStatus(object status)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Heal(int value)
    {
        throw new System.NotImplementedException();
    }

    public virtual void TakeDamage(int value, Damage.DamageType type)
    {
        throw new System.NotImplementedException();
    }
}
