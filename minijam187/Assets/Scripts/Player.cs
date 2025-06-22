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
        float value = (((float)source + (float)maxSource) / ((float)maxSource * 2f)) - (float)maxSource;
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
