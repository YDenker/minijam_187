using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public int ressource = 4;

    public void Populate(PlayerStats stats)
    {
        entityName = stats.Name;
        maxHealth = stats.MaxHealth;
        currentHealth = stats.CurrentHealth;
        entitySprite = stats.PlayerSprite;
        entitySpriteHovered = stats.PlayerHoveredSprite;
        sprite.color = Color.white;
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
}
