using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Entity
{
    [SerializeField] private Sprite Tombstone;
    private EnemyTurn[] possibleTurns;

    private EnemyTurn current;
    private bool dark;
    private bool isDead;

    public bool IsDead => isDead;

    public EnemyTurn PlannedTurn => current;

    public void Populate(EnemyData data)
    {
        entityName = data.Name;
        namePlate.text = data.Name;
        namePlate.gameObject.SetActive(false);
        maxHealth = data.maxHealth;
        currentHealth = data.maxHealth;
        sprite.color = Color.white;
        entitySprite = data.enemySprite;
        entitySpriteHovered = data.enemyHoveredSprite;
        possibleTurns = data.possibleTurns;
        dark = data.dark;
        isDead = false;
        UpdateVisual();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (eventData.button != PointerEventData.InputButton.Left || isDead)
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

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isDead) return;
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (isDead) return;
        base.OnPointerExit(eventData);
    }

    public override int Heal(HealEffect effect, int amount)
    {
        if (isDead) return 0;
        if (antihealStatus.IsAfflicted)
            return 0;
        float third = ((float)GameManager.Instance.Player.MaxSource) / 3f;
        float source = GameManager.Instance.Player.source;
        if (source < -third)
            amount *= 2;
        else if (source < -(third * 2))
            amount *= 3;
        else if (source == -GameManager.Instance.Player.MaxSource)
            amount *= 4;

        int tmp = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        UpdateHealth();
        return currentHealth - tmp;
    }

    public override int TakeDamage(DamageEffect effect, int amount)
    {
        if (isDead) return 0;
        float third = ((float)GameManager.Instance.Player.MaxSource) / 3f;
        float source = GameManager.Instance.Player.source;
        if (source > third)
            amount *= 2;
        else if (source > third * 2)
            amount *= 3;
        else if (source == GameManager.Instance.Player.MaxSource)
            amount *= 4;

        if ((effect.Type == DamageType.LIGHT && !dark) || (effect.Type == DamageType.DARK && dark))
            amount = Mathf.CeilToInt(((float)amount / 2f));
        int tmp = currentHealth;
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealth();

        if (currentHealth <= 0)
            Death();

        return tmp - currentHealth;
    }

    private void Death()
    {
        isDead = true;
        sprite.texture = Tombstone.texture;
        health_slider.gameObject.SetActive(false);
        effects_parent.SetActive(false);
        GameManager.Instance.Log.Log(this.Name + " died!");
        if (GameManager.Instance.Alive <= 0)
            GameManager.Instance.WinnerScreen();
    }

    public void SelectTurn()
    {
        if (isDead) return;
        if (possibleTurns == null || possibleTurns.Length == 0) throw new System.Exception("Enemies need at least one possible turn option");

        // RANDOM SELECTOR
        int i = Random.Range(0, possibleTurns.Length);
        current = possibleTurns[i];
    }

    public void DoTurn()
    {
        if (isDead)
        {
            GameManager.Instance.DeadTurnEffect.Apply(this,this);
            return;
        };
        HandleDebuffs();
        if (IsStunned)
        {
            GameManager.Instance.SkipTurnEffect.Apply(this, this);
            return;
        }
        switch (current.target)
        {
            case EnemyTurn.Target.SELF:
                current.Apply(this,this);
                break;
            case EnemyTurn.Target.PLAYER:
            default:
                current.Apply(this,GameManager.Instance.Player);
                break;
        }
    }
}
