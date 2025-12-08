using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI treasuresText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int coinCount = 0;
    private int treasureCount = 0;
    private int totalScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddPoints(int points, bool isTreasure)
    {
        totalScore += points;
        if (isTreasure)
        {
            treasureCount++;
            Debug.Log($"Treasure collected! Count: {treasureCount}, Score: {totalScore}");
        }
        else
        {
            coinCount++;
            Debug.Log($"Coin collected! Count: {coinCount}, Score: {totalScore}");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinsText != null)
            coinsText.text = $"Coins: {coinCount}";

        if (treasuresText != null)
            treasuresText.text = $"Treasures: {treasureCount}";

        if (scoreText != null)
            scoreText.text = $"Score: {totalScore}";
    }

    public int GetTotalCoins() => coinCount;
    public int GetTotalTreasures() => treasureCount;
    public int GetTotalScore() => totalScore;
}
