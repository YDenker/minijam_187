using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

[CreateAssetMenu(fileName = "Dead", menuName = "Scriptable Objects/Effect/Dead")]
public class DeadTurnEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null,  null, (fromCard ? GameManager.Instance.EndTurn : GameManager.Instance.DoEnemyTurn)));
    }
}
