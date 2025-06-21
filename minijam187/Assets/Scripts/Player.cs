using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : Entity, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int maxHealth = 100;
    public int ressource = 4;
    public Sprite playerSprite;
    public Sprite playerSpriteHovered;

    // UI References
    [SerializeField] private GameObject effects_parent;
    [SerializeField] private TMP_Text health_text;
    [SerializeField] private Slider health_slider;
    [SerializeField] private RawImage sprite;

    // Internal
    private int currentHealth = 100;
    private bool isHovered = false;

    public void Start()
    {
        currentHealth = maxHealth;
        sprite.color = Color.white;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        health_slider.value = ToFloat(currentHealth) / ToFloat(maxHealth);
        health_text.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void PaintHover()
    {
        sprite.texture = isHovered ? playerSpriteHovered.texture : playerSprite.texture;
    }

    public override string Name()
    {
        return "Mona";
    }

    public override void TakeDamage(int value, Damage.DamageType type)
    {
        currentHealth -= value;
        UpdateHealth();
    }

    public override void Heal(int value)
    {
        currentHealth += value;
        UpdateHealth();
    }

    private float ToFloat(int value)
    {
        float result = value;
        return result;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        PaintHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        PaintHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.IsSelected)
        {
            GameManager.Instance.PlaySelectedCard(this);
        }
        else
        {

        }
    }

    public override void GainStatus(object status)
    {
        throw new System.NotImplementedException();
    }
}
