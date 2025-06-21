using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DrawEffect", menuName = "Scriptable Objects/Effects/Draw")]
public class Draw : Effect
{
    public int Amount;
    public override void Apply(IEffected effected)
    {
        GameManager.Instance.Log.Log("Draw " + Amount + " Cards");
        GameManager.Instance.DrawCard(Amount);
    }
}