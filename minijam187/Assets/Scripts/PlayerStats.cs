using System;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public string Name;
    public Sprite PlayerSprite;
    public Sprite PlayerHoveredSprite;
    public int MaxHealth;
    public int CurrentHealth;
    public int maxMana;
    public int maxSource;
    public DeckData DeckData;
}
