using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Objects/Effects/Status")]
public class Status : Effect
{
    public object StatusEffect;
    public override void Apply(IEffected effected)
    {
        GameManager.Instance.Log.Log("Apply Status to "+ effected.Name());
        effected.GainStatus(StatusEffect);
    }
}
