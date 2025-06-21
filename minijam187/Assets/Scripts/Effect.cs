using System;

[Serializable]
public class Effect
{
    public int Amount;
    public CardEffect CardEffect;

    public void Apply(IEffected effected)
    {
        CardEffect.Apply(effected, Amount);
    }
}
