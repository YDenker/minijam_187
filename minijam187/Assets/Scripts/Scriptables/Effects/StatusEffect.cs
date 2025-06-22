using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Effect/Status")]
public class StatusEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => ApplyStatus(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void ApplyStatus(IEffected origin, IEffected effected, int amount)
    {
        effected.GainStatus(this, amount);
        GameManager.Instance.Log.Log("Status Effects are not implemented!");
    }
}
