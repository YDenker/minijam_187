using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Effect/Heal")]
public class HealEffect : CardEffect
{
    public override void Apply(IEffected effected, int amount)
    {
        int actual = effected.Heal(this, amount);
        GameManager.Instance.Log.Log("Heal "+ effected.Name + " for <b>" + actual +"</b> health.");
    }
}
