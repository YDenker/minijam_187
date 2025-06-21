using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "HealEffect", menuName = "Scriptable Objects/Effects/Heal")]
public class Heal : Effect
{
    public int Amount;
    public override void Apply(IEffected effected)
    {
        GameManager.Instance.Log.Log("Heal " + effected.Name() + " by " + Amount);
        effected.Heal(Amount);
    }
}
