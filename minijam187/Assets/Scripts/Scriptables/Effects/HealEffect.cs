using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Effect/Heal")]
public class HealEffect : CardEffect
{
    public override void Apply(IEffected effected, int amount)
    {
        int actual = effected.Heal(this, amount);
        if (actual != amount)
            GameManager.Instance.Log.Log("Heal "+ effected.Name + " for <color=green><b>" + actual +"</b>("+amount+")</color> health.");
        else
            GameManager.Instance.Log.Log("Heal " + effected.Name + " for <color=green><b>" + amount + "</b></color> health.");
    }
}
