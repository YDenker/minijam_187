using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Enemy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public EnemyData data;

    // UI References
    [SerializeField] private GameObject effects_parent;
    [SerializeField] private TMP_Text health_text;
    [SerializeField] private Slider health_slider;
    [SerializeField] private RawImage sprite;

    // Internal
    private int currentHealth;
    private bool isHovered = false;

    public void Start()
    {
        currentHealth = data.maxHealth;
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        PaintHover();
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        health_slider.value = ToFloat(currentHealth) / ToFloat(data.maxHealth);
        health_text.text = currentHealth.ToString()+"/"+ data.maxHealth.ToString();
    }

    public void PaintHover()
    {
        sprite.texture = isHovered ? data.enemyHoveredSprite.texture : data.enemySprite.texture;
    }

    public void TakeDamage(int value)
    {
        currentHealth -= value;
        UpdateHealth();
    }

    public void Heal(int value)
    {
        currentHealth += value;
        UpdateHealth();
    }

    private float ToFloat(int value)
    {
        float result = value;
        return result;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        PaintHover();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        PaintHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("DEINE MUDDA");
    }
}
