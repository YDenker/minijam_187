using UnityEngine;

[CreateAssetMenu(fileName = "Source", menuName = "Scriptable Objects/Effect/Source")]
public class SourceEffect : CardEffect
{
    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null,  () =>SetSource(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void SetSource(IEffected origin, IEffected effected, int amount)
    {
        int actual = GameManager.Instance.Player.ChangeSource(amount);
        if ( actual != amount)
            GameManager.Instance.Log.Log(origin.Name + "s source was set to <b>" + actual + "</b>(" + amount + ").");
        else
            GameManager.Instance.Log.Log(origin.Name + "s source was set to <b>" + amount + "</b>.");
    }
}