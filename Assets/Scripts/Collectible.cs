using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Type")]
    [SerializeField] private bool isTreasure = false;

    [Header("Points")]
    [SerializeField] private int pointsValue = 10;

    [Header("Animation")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.3f;

    private Vector3 startPosition;
    private bool isCollected = false;

    /// <summary>
    /// Configure collectible parameters at runtime (used by level generator).
    /// </summary>
    public void Configure(bool treasure, int points, float rotation, float bobSpeedValue, float bobHeightValue)
    {
        isTreasure = treasure;
        pointsValue = points;
        rotationSpeed = rotation;
        bobSpeed = bobSpeedValue;
        bobHeight = bobHeightValue;
        startPosition = transform.position;
        isCollected = false;
    }

    private void Start()
    {
        // Store initial position for bobbing animation
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isCollected) 
            return;

        // Animate the collectible
        AnimateCollectible();
    }

    private void AnimateCollectible()
    {
        // Rotate around local Y axis (self) to keep the pivot centered
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

        // Bob up and down
        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = newPosition;
    }

    /// <summary>
    /// Déclenché quand le joueur traverse le trigger du collectible.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si le joueur touche l'objet à collecter
        if (other.CompareTag("Player") && !isCollected)
        {
            Debug.Log($"Le joueur est entré dans la zone de {gameObject.name}");
            CollectItem();
        }
    }
    
    private void CollectItem()
    {
        // Mark as collected
        isCollected = true;

        // Add points to UI/game state manager
        if (UI.Instance != null)
        {
            UI.Instance.AddPoints(pointsValue, isTreasure);
        }

        // Destroy the object
        Destroy(gameObject);
    }
}
