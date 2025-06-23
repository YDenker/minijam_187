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
    [SerializeField] protected TMP_Text namePlate;
    [SerializeField] protected GameObject effects_parent;
    [SerializeField] protected TMP_Text health_text;
    [SerializeField] protected Slider health_slider;
    [SerializeField] protected RawImage sprite;

    [SerializeField] protected Burn burnStatus;
    [SerializeField] protected Poison poisonStatus;
    [SerializeField] protected HealOverTime healOverTimeStatus;
    [SerializeField] protected AntiHeal antihealStatus;
    [SerializeField] protected Stun stunStatus;

    public Vector2 AnimationOrigin;
    public Vector2 AnimationTarget;

    // Art
    protected Sprite entitySprite;
    protected Sprite entitySpriteHovered;

    // Internal
    private bool isHovered = false;
    protected bool nameplateOn = false;
    public bool IsStunned => stunStatus.IsStunned;
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

    public void HandleDebuffs()
    {
        if (burnStatus.IsBurning)
            burnStatus.Tick(this);
        if(poisonStatus.IsPoisioned)
            poisonStatus.Tick(this);
        if (healOverTimeStatus.IsAffected)
            healOverTimeStatus.Tick(this);
        if(antihealStatus.IsAfflicted)
            antihealStatus.Tick(this);
        if (stunStatus.IsStunned)
            stunStatus.Tick();
    }

    public bool GainStatus(StatusEffect effect, int amount)
    {
        switch (effect.status)
        {
            case StatusEffect.Status.BURN:
                burnStatus.Ignite(amount,effect.duration);
                break;
            case StatusEffect.Status.POISON:
                poisonStatus.Intoxicate(amount);
                break;
            case StatusEffect.Status.HEALOVERTIME:
                healOverTimeStatus.Mend(amount, effect.duration);
                break;
            case StatusEffect.Status.HEALPREVENTION:
                healOverTimeStatus.Remove();
                antihealStatus.Infest(amount);
                break;
            case StatusEffect.Status.STUN:
                stunStatus.Bonk(effect.duration);
                break;
            default:
                break;
        }
        return true;
    }
    public bool RemoveDebuffs()
    {
        burnStatus.Cleanse();
        poisonStatus.Cleanse();
        antihealStatus.Cleanse();
        stunStatus.Cleanse();
        return true;
    }

    public virtual int Heal(HealEffect effect, int amount)
    {
        if (antihealStatus.IsAfflicted)
            return 0;
        int tmp = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        UpdateHealth();
        return currentHealth - tmp;
    }

    public virtual int TakeDamage(DamageEffect effect, int amount)
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
