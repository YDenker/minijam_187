using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    // STATS
    [Space]
    public LightSide lightSide;
    public DarkSide darkSide;

    [System.Serializable]
    public class LightSide
    {
        public string cardName;
        public int cost;
        public string effectText;
        public Sprite art;
        // EFFECT
        public Effect[] effects;
        public LightSide(int cost, string effectText)
        {
            this.cost = cost;
            this.effectText = effectText;
        }
    }
    [System.Serializable]
    public class DarkSide
    {
        public string cardName;
        public int cost;
        public string effectText;
        public Sprite art;
        // EFFECT
        public Effect[] effects;
        public DarkSide(int cost, string effectText)
        {
            this.cost = cost;
            this.effectText = effectText;
        }
    }
}
