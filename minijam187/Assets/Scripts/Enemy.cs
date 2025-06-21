using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Entity
{
    private EnemyTurn[] possibleTurns;

    private EnemyTurn current;

    public EnemyTurn PlannedTurn => current;

    public void Populate(EnemyData data)
    {
        entityName = data.Name;
        maxHealth = data.maxHealth;
        currentHealth = data.maxHealth;
        sprite.color = Color.white;
        entitySprite = data.enemySprite;
        entitySpriteHovered = data.enemyHoveredSprite;
        possibleTurns = data.possibleTurns;
        UpdateVisual();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (GameManager.Instance.IsSelected)
        {
            GameManager.Instance.PlaySelectedCard(this);
        }
        else
        {

        }
    }

    public void SelectTurn()
    {
        if (possibleTurns == null || possibleTurns.Length == 0) throw new System.Exception("Enemies need at least one possible turn option");

        // RANDOM SELECTOR
        int i = Random.Range(0, possibleTurns.Length);
        current = possibleTurns[i];
    }

    public void DoTurn()
    {
        switch (current.target)
        {
            case EnemyTurn.Target.SELF:
                current.Apply(this);
                break;
            case EnemyTurn.Target.PLAYER:
            default:
                current.Apply(GameManager.Instance.Player);
                break;
        }
    }
}
