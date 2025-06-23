using System;
using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Effect/Heal")]
public class HealEffect : CardEffect
{
    public bool self = false;
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        if (self)
        {
            effected = origin;
        }
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => GainHealth(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    public void ApplyRoundBased(IEffected target, int amount, Action endCallback)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(target.GetEffectOrigin(), target.GetEffectTarget(), null, () => GainHealth(target, target, amount), endCallback));
    }

    private void GainHealth(IEffected origin, IEffected effected, int amount)
    {
        int actual = effected.Heal(this, amount);
        if (actual != amount)
            GameManager.Instance.Log.Log(origin.Name + " heals " + effected.Name + " by <color=green><b>" + actual + "</b>(" + amount + ")</color> health.");
        else
            GameManager.Instance.Log.Log(origin.Name + " heals " + effected.Name + " by <color=green><b>" + amount + "</b></color> health.");
    }
}
