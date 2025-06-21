using UnityEngine;
using static CardData;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        PLAYER,
        ENEMY,
        PAUSE
    }

    // Prefabs
    [SerializeField] private GameObject cardPrefab;

    // UI References
    [SerializeField] private HandFanLayout handFanLayout;
    [SerializeField] private CombatLog combatLog;

    public CombatLog Log => combatLog;

    // Card Curve
    public Color32 curveColor;
    public LineRenderer lineRenderer;
    public int curveResolution = 20;

    public GameState state = GameState.PAUSE;

    public Card Selected = null;

    public bool IsSelected => Selected != null;


    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (handFanLayout.cardsInHand.Count >= handFanLayout.maxHandCapacity)
                return;
            GameObject newCard = Instantiate(cardPrefab);
            Card card = newCard.GetComponent<Card>();
            if(!handFanLayout.AddCard(card)) Destroy(card.gameObject);
        }
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
        if (Selected.isLightSide)
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
        StartCoroutine(Selected.FlipCard(() => handFanLayout.RemoveCard(Selected)));
        lineRenderer.enabled = false;
    }

    void Awake()
    {
        Instance = this;
        lineRenderer.startColor = curveColor;
        lineRenderer.endColor = curveColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    public void Update()
    {
        // INPUT
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawCard(1);
        }
        
        if (IsSelected)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Selected.HandleSelection();
            }
            Vector3 cardPosition = Selected.transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            Vector3 midPoint = (cardPosition + mousePosition) * 0.5f + Vector3.up * 2f;

            for (int i = 0; i < curveResolution; i++)
            {
                float t = i / (float)(curveResolution - 1);
                Vector3 bezierPoint = CalculateQuadraticBezierPoint(t, cardPosition, midPoint, mousePosition);
                lineRenderer.SetPosition(i, bezierPoint);
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
