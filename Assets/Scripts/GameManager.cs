using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI treasuresText;

    private int totalCoins = 0;
    private int totalTreasures = 0;

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Initialize UI display
        UpdateUI();
    }

    public void AddPoints(int points, bool isTreasure)
    {
        // Add points based on collectible type
        if (isTreasure)
        {
            totalTreasures += points;
            Debug.Log($"Treasure collected! Total: {totalTreasures}");
        }
        else
        {
            totalCoins += points;
            Debug.Log($"Coin collected! Total: {totalCoins}");
        }

        // Update the UI display
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update coin counter
        if (coinsText != null)
        {
            coinsText.text = $"Coins: {totalCoins}";
        }

        // Update treasure counter
        if (treasuresText != null)
        {
            treasuresText.text = $"Treasures: {totalTreasures}";
        }
    }

    public int GetTotalCoins() => totalCoins;
    public int GetTotalTreasures() => totalTreasures;
}
