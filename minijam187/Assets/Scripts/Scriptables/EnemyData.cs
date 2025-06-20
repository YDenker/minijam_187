using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public Sprite enemySprite;
    public Sprite enemyHoveredSprite;
    public int maxHealth;
    public int damage;
}
