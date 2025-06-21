using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Enemy : Entity
{
    public void Populate(EnemyData data)
    {
        entityName = data.Name;
        maxHealth = data.maxHealth;
        currentHealth = data.maxHealth;
        sprite.color = Color.white;
        entitySprite = data.enemySprite;
        entitySpriteHovered = data.enemyHoveredSprite;
        UpdateVisual();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (GameManager.Instance.IsSelected)
        {
            GameManager.Instance.PlaySelectedCard(this);
        }
        else
        {

        }
    }
}
