using System;
using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        PLAYER,
        ENEMY,
        PAUSE
    }

    // Starting
    public PlayerStats playerStats;
    public EnemyData[] enemyData;

    // UI References
    [SerializeField] private Player player;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private HandFanLayout handFanLayout;
    [SerializeField] private CombatLog combatLog;

    public HandFanLayout Hand => handFanLayout;
    public CombatLog Log => combatLog;

    // Card Curve
    public Color32 curveColor;
    public LineRenderer lineRenderer;
    public int curveResolution = 20;

    public GameState state = GameState.PLAYER;

    public Card Selected = null;

    public bool IsSelected => Selected != null;


    public void StartTurn()
    {
        StartCoroutine(handFanLayout.DrawCard(5, () => state = GameState.PLAYER));
        
    }

    public void EndTurn()
    {
        state = GameState.ENEMY;
    }

    public void SelectCard(Card card)
    {
        Selected = card;
        handFanLayout.SelectCard(card);
        lineRenderer.enabled = true;
    }

    public void UnselectCard(Card card)
    {
        if (card != Selected)
            Debug.LogError("THIS SHOULD NOT BE HAPPENING!");
        lineRenderer.enabled = false;
        handFanLayout.UnselectCard(card);
        Selected = null;
    }

    public void PlaySelectedCard(Entity target)
    {
        if (state != GameState.PLAYER)
        {
            combatLog.Log("Can only play cards during your turn!");
            return;
        }
        Hand.cardsInHand.Remove(Selected);
        if (Selected.data.isLightSide)
        {
            foreach (var effect in Selected.data.lightSide.effects)
            {
                effect.Apply(target);
            }
        }
        else
        {
            foreach (var effect in Selected.data.darkSide.effects)
            {
                effect.Apply(target);
            }
        }
        handFanLayout.FlipCard(Selected,() => handFanLayout.RemoveCard(Selected));
        lineRenderer.enabled = false;
        handFanLayout.UnGreyCards();
    }

    void Awake()
    {
        Instance = this;
        state = GameState.PAUSE;
        lineRenderer.startColor = curveColor;
        lineRenderer.endColor = curveColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    public void Start()
    {
        Hand.CardDeck.Populate(playerStats.DeckData);
        player.Populate(playerStats);
        int i = 0;
        foreach (EnemyData enemy in enemyData)
        {
            enemies[i].gameObject.SetActive(true);
            enemies[i].Populate(enemy);
            i++;
        }
        StartTurn();
    }

    public void Update()
    {
        // INPUT
        if (Input.GetKeyDown(KeyCode.D))
        {
            handFanLayout.DrawCard(1);
        }
        
        if (IsSelected)
        {
            
            Vector3 cardPosition = Selected.transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            Vector3 midPoint = (cardPosition + mousePosition) * 0.5f + Vector3.up * 2f;

            for (int i = 0; i < curveResolution; i++)
            {
                float t = i / (float)(curveResolution - 1);
                Vector3 bezierPoint = CalculateQuadraticBezierPoint(t, cardPosition, midPoint, mousePosition);
                lineRenderer.SetPosition(i, bezierPoint);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Selected.HandleSelection();
            }
        }
        else
        {
            
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}
