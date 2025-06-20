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

    // Prefabs
    [SerializeField] private GameObject cardPrefab;

    // UI References
    [SerializeField] private HandFanLayout handFanLayout;

    // Card Curve
    public Color32 curveColor;
    public LineRenderer lineRenderer;
    public int curveResolution = 20;

    public GameState state = GameState.PAUSE;

    public Card Selected = null;

    public bool IsSelected => Selected != null;


    public void DrawCard()
    {
        GameObject newCard = Instantiate(cardPrefab);
        RectTransform cardRT = newCard.GetComponent<RectTransform>();
        handFanLayout.AddCard(cardRT);
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
            DrawCard();
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
            
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
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
