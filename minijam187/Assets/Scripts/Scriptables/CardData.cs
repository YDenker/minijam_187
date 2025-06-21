using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [Serializable]
    public class CardRuntimeData
    {
        // Has to be unique
        public int ID = 0;
        // Starting State
        public bool isLightSide = true;
        // STATS
        public LightSide lightSide;
        public DarkSide darkSide;

        // Hidden
        private bool isTemporary = false;

        public bool IsTemporary => isTemporary;

        public CardRuntimeData(int ID, bool isLightSide, LightSide lightSide, DarkSide darkSide, bool isTemporary)
        {
            this.ID = ID;
            this.isLightSide = isLightSide;
            this.lightSide = lightSide;
            this.darkSide = darkSide;
            this.isTemporary = isTemporary;
        }
    }

    // Has to be unique
    public int ID = 0;
    // Starting State
    public bool isLightSide = true;
    // STATS
    [Space]
    public LightSide lightSide;
    public DarkSide darkSide;

    // Hidden
    private bool isTemporary = false;

    public bool IsTemporary => isTemporary;

    [System.Serializable]
    public class LightSide
    {
        public string cardName;
        public int cost;
        [SerializeField] private string effectText;
        public Sprite art;
        // EFFECT
        public Effect[] effects;

        public string EffectText
        {
            get
            {
                string tmp = effectText;
                for (int i = 1; i <= effects.Length; i++)
                {
                    tmp = tmp.Replace("$" + i.ToString(), effects[i - 1].Amount.ToString());
                }
                return tmp;
            }
        }
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
        [SerializeField] private string effectText;
        public Sprite art;
        // EFFECT
        public Effect[] effects;

        public string EffectText
        {
            get
            {
                string tmp = effectText;
                for (int i = 1; i <= effects.Length; i++)
                {
                    tmp = tmp.Replace("$" + i.ToString(), effects[i-1].Amount.ToString());
                }
                return tmp;
            }
        }
        public DarkSide(int cost, string effectText)
        {
            this.cost = cost;
            this.effectText = effectText;
        }
    }

    public CardRuntimeData GetRuntimeData()
    {
        return new CardRuntimeData(ID, isLightSide, lightSide, darkSide, isTemporary);
    }
}
