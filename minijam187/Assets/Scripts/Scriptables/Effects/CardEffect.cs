using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public SpellAnimation spellAnimation;
    public abstract void Apply(IEffected origin, IEffected effected, int amount, bool fromCard = true);
}
