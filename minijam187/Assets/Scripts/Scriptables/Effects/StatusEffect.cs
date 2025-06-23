using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Effect/Status")]
public class StatusEffect : CardEffect
{
    public enum Status { BURN, POISON, HEALOVERTIME, HEALPREVENTION, STUN }
    public enum Target { SELF, ENEMY }

    public Status status;
    public Target target;
    public int duration;

    public override void Apply(IEffected origin, IEffected effected, int amount, bool fromCard)
    {
        GameManager.Instance.StartCoroutine(spellAnimation.Play(origin.GetEffectOrigin(), effected.GetEffectTarget(), null, () => ApplyStatus(origin, effected, amount), (fromCard ? GameManager.Instance.EndPlaySelectedCard : GameManager.Instance.DoEnemyTurn)));
    }

    private void ApplyStatus(IEffected origin, IEffected effected, int amount)
    {
        if (target == Target.SELF)
        {
            origin.GainStatus(this, amount);
            GameManager.Instance.Log.Log(origin.Name +" gained the status "+ status.ToString());
        }
        else
        {
            effected.GainStatus(this, amount);
            GameManager.Instance.Log.Log(effected.Name + " gained the status " + status.ToString());
        }
    }
}
