using System;

[Serializable]
public class EnemyTurn
{
    public enum Target { SELF, PLAYER } // maybe Ally in the future

    public int minAmount, maxAmount;
    public CardEffect effect;
    public Target target;

    public void Apply(IEffected target)
    {
        int amount = UnityEngine.Random.Range(minAmount, maxAmount);
        effect.Apply(target, amount);
    }
}
