using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Effect/Status")]
public class StatusEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => ApplyStatus(origin, effected, amount), GameManager.Instance.EndPlaySelectedCard));
    }

    private void ApplyStatus(IEffected origin, IEffected effected, int amount)
    {
        effected.GainStatus(this, amount);
        GameManager.Instance.Log.Log("Status Effects are not implemented!");
    }
}
