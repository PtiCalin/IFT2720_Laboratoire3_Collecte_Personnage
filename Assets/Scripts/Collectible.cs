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
        // Rotate around Y axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Bob up and down
        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player touches the collectible
        if (other.CompareTag("Player") && !isCollected)
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        // Mark as collected
        isCollected = true;

        // Add points to game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddPoints(pointsValue, isTreasure);
        }

        // Destroy the object
        Destroy(gameObject);
    }
}
