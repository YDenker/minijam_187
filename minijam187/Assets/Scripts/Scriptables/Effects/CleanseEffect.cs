using UnityEngine;

[CreateAssetMenu(fileName = "Cleanse", menuName = "Scriptable Objects/Effect/Cleanse")]
public class CleanseEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard = true)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => Cleanse(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void Cleanse(IEffected origin, IEffected effected, int amount)
    {
        origin.RemoveDebuffs();
    }
}
