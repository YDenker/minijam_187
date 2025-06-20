using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    // STATS
    [Space]
    public string cardName;
    public LightSide lightSide;
    public DarkSide darkSide;

    [System.Serializable]
    public class LightSide
    {
        public int cost;
        public string effectText;
        public Sprite art;
        // EFFECT
        // TODO
        public LightSide(int cost, string effectText)
        {
            this.cost = cost;
            this.effectText = effectText;
        }
    }
    [System.Serializable]
    public class DarkSide
    {
        public int cost;
        public string effectText;
        public Sprite art;
        // EFFECT
        // TODO
        public DarkSide(int cost, string effectText)
        {
            this.cost = cost;
            this.effectText = effectText;
        }
    }
}
