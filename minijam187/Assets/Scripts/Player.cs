using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Player : Entity
{
    [SerializeField] private Sprite moreYing;
    [SerializeField] private Sprite moreYang;
    [SerializeField] private Sprite equalYingYang;

    [SerializeField] private Slider source_slider;
    [SerializeField] private Image source_handle;
    [SerializeField] private TMP_Text mana_text;

    public int mana;
    public int source;

    private int maxMana;
    private int maxSource;

    public int MaxMana => maxMana;
    public int MaxSource => maxSource;

    public int ChangeSource(int amount)
    {
        int tmp = source;
        source = Mathf.Min(maxSource,Mathf.Max(source+ amount,-maxSource));
        UpdateSource();
        return tmp - source;
    }

    public bool TrySubtractMana(int amount)
    {
        if (mana < amount) return false;
        mana -= amount;
        UpdateMana();
        return true;
    }

    public int ForceSubtractMana(int amount)
    {
        int tmp = mana;
        mana -= amount;
        mana = Mathf.Max(0, mana);
        UpdateMana();
        return tmp - mana;
    }

    public void GainMana(int amount)
    {
        mana += amount;
        UpdateMana();
    }

    public void SetMana(int amount)
    {
        mana = amount;
        UpdateMana();
    }

    public void Populate(PlayerStats stats)
    {
        entityName = stats.Name;
        namePlate.text = stats.Name;
        namePlate.gameObject.SetActive(false);
        maxHealth = stats.MaxHealth;
        maxMana = stats.maxMana;
        mana = maxMana;
        maxSource = stats.maxSource;
        source = 0; // MIDDLE
        currentHealth = stats.CurrentHealth;
        entitySprite = stats.PlayerSprite;
        entitySpriteHovered = stats.PlayerHoveredSprite;
        sprite.color = Color.white;
        UpdateVisual();
    }

    public override void UpdateVisual()
    {
        base.UpdateVisual();
        UpdateSource();
        UpdateMana();
    }

    private void UpdateSource()
    {
        float t = Mathf.InverseLerp(-maxSource, maxSource, source);
        float value = t * 2f - 1f;
        source_slider.value = value;
        if (value > 0.5)
            source_handle.sprite = moreYang;
        else if (value < -0.5)
            source_handle.sprite = moreYing;
        else
            source_handle.sprite = equalYingYang;
    }

    private void UpdateMana()
    {
        mana_text.text = mana.ToString();
    }

    public override int Heal(HealEffect effect, int amount)
    {
        if (antihealStatus.IsAfflicted)
            return 0;
        float third = ((float)maxSource) / 3f;
        if (source > third)
            amount = Mathf.CeilToInt(((float)amount) / 2f);
        else if (source > third * 2)
            amount = Mathf.CeilToInt(((float)amount) / 3f);
        else if (source == maxSource)
            amount = Mathf.CeilToInt(((float)amount) / 4f);

        if (source < -third)
            amount *= 2;
        else if (source < -(third * 2))
            amount *= 3;
        else if (source == -maxSource)
            amount *= 4;
        int tmp = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        UpdateHealth();
        return currentHealth - tmp;
    }

    public override int TakeDamage(DamageEffect effect, int amount)
    {
        float third = ((float)maxSource) / 3f;
        if (source > third)
            amount *= 2;
        else if (source > third * 2)
            amount *= 3;
        else if (source == maxSource)
            amount *= 4;

        if (effect.Type == DamageType.LIGHT && source < -(third*2))
            amount = Mathf.CeilToInt(((float)amount) / 3f);
        else if (effect.Type == DamageType.LIGHT && source == -maxSource)
            amount = Mathf.CeilToInt(((float)amount) / 4f);

        int tmp = currentHealth;
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealth();

        if (currentHealth <= 0)
            GameManager.Instance.LooserScreen();

        return tmp - currentHealth;
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
            nameplateOn = !nameplateOn;
            namePlate.gameObject.SetActive(nameplateOn);
        }
    }
}
