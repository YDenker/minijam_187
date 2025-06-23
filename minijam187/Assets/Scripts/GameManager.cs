using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


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
    public EnemyTeam[] EnemyTeamComp;

    public EnemyTurn SkipTurnEffect;

    // UI References
    [SerializeField] private Player player;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private HandFanLayout handFanLayout;
    [SerializeField] private CombatLog combatLog;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private RectTransform animationLayer;
    [SerializeField] private GameObject winnerScreen;
    [SerializeField] private GameObject looserScreen;

    public HandFanLayout Hand => handFanLayout;
    public CombatLog Log => combatLog;
    public RectTransform AnimationLayer => animationLayer;

    // Card Curve
    public Color32 curveColor;
    public LineRenderer lineRenderer;
    public int curveResolution = 20;

    public GameState state = GameState.PLAYER;

    public Card Selected = null;
    public Player Player => player;

    public bool IsSelected => Selected != null;

    // Internal
    private bool animInProgress = false;
    private Enemy[] activeEnemies;
    private int playerturn = 0;
    private int enemyturn = 0;

    private List<Enemy> deadEnemies = new();

    private int enemyFinishedCount = 0;
    private int effect_finished = 0;

    private int teamIndex = 0;

    public void StartGame()
    {
        StartCoroutine(handFanLayout.DrawCard(3, () => SwitchState(GameState.PLAYER)));
    }

    private void WinnerScreen()
    {
        winnerScreen.SetActive(true);
    }

    public void LooserScreen()
    {
        looserScreen.SetActive(true);
    }

    private void CleanUpDead()
    {
        List<int> indices = new();
        int i = 0;
        foreach (Enemy enemy in deadEnemies)
        {
            if (activeEnemies.Contains<Enemy>(enemy))
            {
                indices.Add(i);
            }
            i++;
        }
        Enemy[] tmp = new Enemy[activeEnemies.Length - indices.Count];
        for (int j = 0, k = 0; j < activeEnemies.Length; j++, k++)
        {
            if (indices.Contains(j))
                k--;
            else
                tmp[k] = activeEnemies[j];
        }
        activeEnemies = tmp;

        if (activeEnemies.Length == 0)
            WinnerScreen();
    }

    public void StartTurn()
    {
        foreach (Enemy enemy in activeEnemies)
        {
            enemy.SelectTurn();
        }
        Player.HandleDebuffs();
        if (Player.IsStunned)
        {
            SkipTurnEffect.effect.Apply(Player, Player, 0);
            return;
        }
        handFanLayout.DrawCard(2);
        Player.SetMana(Player.MaxMana);
    }

    public void EndTurn()
    {
        SwitchState(GameState.ENEMY);
    }

    public void DoEnemyTurn()
    {
        if (enemyFinishedCount >= activeEnemies.Length)
        {
            enemyFinishedCount = 0;
            SwitchState(GameState.PLAYER);
        }
        else
        {
            activeEnemies[enemyFinishedCount].DoTurn();
            enemyFinishedCount += 1;
        }
    }

    public void SwitchState(GameState state)
    {
        CleanUpDead();
        this.state = state;
        switch (state)
        {
            case GameState.PLAYER:
                playerturn++;
                Log.LogTurnSwitch(state.ToString(), playerturn);
                StartTurn();
                endTurnButton.interactable = true;
                break;
            case GameState.ENEMY:
                enemyturn++;
                Log.LogTurnSwitch(state.ToString(), enemyturn);
                endTurnButton.interactable = false;
                DoEnemyTurn();
                break;
            case GameState.PAUSE:
            default:
                endTurnButton.interactable = false;
                break;
        }
    }

    public void SelectCard(Card card)
    {
        Selected = card;
        handFanLayout.SelectCard(card);
        lineRenderer.enabled = true;
    }

    public void Death(Enemy enemy)
    {
        Log.Log(enemy.Name +" died!");
        deadEnemies.Add(enemy);
        enemy.gameObject.SetActive(false);
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
        if (animInProgress)
            return;

        bool isLight = Selected.data.isLightSide;
        int cost = isLight ? Selected.data.lightSide.cost : Selected.data.darkSide.cost;

        if (!Player.TrySubtractMana(cost))
        {
            combatLog.Log("Not enough mana!");
            return;
        }

        Hand.cardsInHand.Remove(Selected);
        lineRenderer.enabled = false;
        animInProgress = true;
        effect_finished = 0;
        if (isLight)
        {
            foreach (var effect in Selected.data.lightSide.effects)
            {
                effect_finished++;
                effect.Apply(Player,target);
            }
        }
        else
        {
            foreach (var effect in Selected.data.darkSide.effects)
            {
                effect_finished++;
                effect.Apply(Player,target);
            }
        }
    }

    public void EndPlaySelectedCard()
    {
        effect_finished--;
        if (effect_finished > 0)
            return;
        Player.ChangeSource(Selected.data.isLightSide ? Selected.data.lightSide.sourceImpact : Selected.data.darkSide.sourceImpact);
        handFanLayout.FlipCard(Selected, () => handFanLayout.RemoveCard(Selected));
        handFanLayout.UnGreyCards();
        animInProgress = false;
    }

    public void Awake()
    {
        Instance = this;
        Hand.CardDeck.Populate(playerStats.DeckData);
        player.Populate(playerStats);
        teamIndex = Random.Range(0, EnemyTeamComp.Length);
        activeEnemies = new Enemy[EnemyTeamComp[teamIndex].enemyData.Length];
        int i = 0;
        foreach (EnemyData enemy in EnemyTeamComp[teamIndex].enemyData)
        {
            activeEnemies[i] = enemies[i];
            activeEnemies[i].gameObject.SetActive(true);
            activeEnemies[i].Populate(enemy);
            i++;
        }
        SwitchState(GameState.PAUSE);
        endTurnButton.onClick.AddListener(() => EndTurn());
        lineRenderer.startColor = curveColor;
        lineRenderer.endColor = curveColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    public void Start()
    {
        Log.Log(EnemyTeamComp[teamIndex].Name + " appears.");
        StartGame();
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
