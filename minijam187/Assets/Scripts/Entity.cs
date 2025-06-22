using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEffected
{
    protected string entityName;
    protected int currentHealth;
    protected int maxHealth;

    // UI References
    [SerializeField] protected GameObject effects_parent;
    [SerializeField] protected TMP_Text health_text;
    [SerializeField] protected Slider health_slider;
    [SerializeField] protected RawImage sprite;

    public Vector2 AnimationOrigin;
    public Vector2 AnimationTarget;

    // Art
    protected Sprite entitySprite;
    protected Sprite entitySpriteHovered;

    // Internal
    private bool isHovered = false;

    public string Name => entityName;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public virtual void UpdateVisual()
    {
        PaintHover();
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        health_slider.value = (float)currentHealth / (float)maxHealth;
        health_text.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public bool GainStatus(StatusEffect effect, int amount)
    {
        throw new System.NotImplementedException();
    }

    public int Heal(HealEffect effect, int amount)
    {
        int tmp = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        UpdateHealth();
        return currentHealth - tmp;
    }

    public int TakeDamage(DamageEffect effect, int amount)
    {
        int tmp = currentHealth;
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealth();
        return tmp - currentHealth;
    }

    public void PaintHover()
    {
        sprite.texture = isHovered ? entitySpriteHovered.texture : entitySprite.texture;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        PaintHover();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        PaintHover();
    }

    public Vector2 GetEffectOrigin()
    {
        Vector2 origin = IEffected.GetPositionInLayer((RectTransform)transform, GameManager.Instance.AnimationLayer);

        return (origin + AnimationOrigin);
    }

    public Vector2 GetEffectTarget()
    {
        Vector2 origin = IEffected.GetPositionInLayer((RectTransform)transform, GameManager.Instance.AnimationLayer);
        return (origin + AnimationTarget);
    }
}
