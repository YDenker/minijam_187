using UnityEngine;

[CreateAssetMenu(fileName = "Team Rocket", menuName = "Scriptable Objects/Enemy Team")]
public class EnemyTeam : ScriptableObject
{
    public string Name = "Team Rocket";
    public EnemyData[] enemyData;
}
