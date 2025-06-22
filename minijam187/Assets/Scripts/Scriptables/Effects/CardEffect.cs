using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public SpellAnimation spellAnimation;
    public abstract void Apply(IEffected origin, IEffected effected, int amount, bool fromCard = true);
}

public enum CardPosition { LEFTMOST, RIGHTMOST, CENTER, SELF, All }

public static class CardPositionTypeExtensions
{
    public static string ToStringSentence(this CardPosition type)
    {
        return type switch
        {
            CardPosition.LEFTMOST => "leftmost",
            CardPosition.RIGHTMOST => "rightmost",
            CardPosition.SELF => "played",
            CardPosition.All => "whole hand of",
            _ => "unknown",
        };
    }
}