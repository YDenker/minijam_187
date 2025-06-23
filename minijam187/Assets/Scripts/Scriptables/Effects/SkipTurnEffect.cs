using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

[CreateAssetMenu(fileName = "Skip", menuName = "Scriptable Objects/Effect/Skip")]
public class SkipTurnEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => SkipTurn(origin), (fromCard ? GameManager.Instance.EndTurn : GameManager.Instance.DoEnemyTurn)));
    }

    private void SkipTurn(IEffected origin)
    {
        GameManager.Instance.Log.Log(origin.Name + " is stunned.");
    }
}
