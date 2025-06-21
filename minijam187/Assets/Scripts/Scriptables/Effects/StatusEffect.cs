using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Effect/Status")]
public class StatusEffect : CardEffect
{
    public override void Apply(IEffected effected, int amount)
    {
        effected.GainStatus(this,amount);
        GameManager.Instance.Log.Log("Status Effects are not implemented!");
    }
}
