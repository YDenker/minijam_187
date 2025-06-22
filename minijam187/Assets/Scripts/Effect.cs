using System;

[Serializable]
public class Effect
{
    public int Amount;
    public CardEffect CardEffect;

    public void Apply(IEffected origin, IEffected effected)
    {
        CardEffect.Apply(origin, effected, Amount);
    }
}
