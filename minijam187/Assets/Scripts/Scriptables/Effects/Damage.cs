using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DamageEffect", menuName = "Scriptable Objects/Effects/Damage")]
public class Damage : Effect
{
    [Serializable]
    public enum DamageType
    {
        LIGHT,
        DARK,
        PHYSICAL
    }
    public int Amount;
    public DamageType Type;
    public override void Apply(IEffected effected)
    {
        GameManager.Instance.Log.Log("Deal " + Amount + " " + Type.ToString() + " damage to " + effected.Name());
        effected.TakeDamage(Amount, Type);
    }
}
