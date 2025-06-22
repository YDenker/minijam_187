using System;

[Serializable]
public class EnemyTurn
{
    public enum Target { SELF, PLAYER } // maybe Ally in the future

    public int minAmount, maxAmount;
    public CardEffect effect;
    public Target target;

    public void Apply(IEffected origin, IEffected target)
    {
        int amount = UnityEngine.Random.Range(minAmount, maxAmount);
        effect.Apply(origin, target, amount);
    }
}
