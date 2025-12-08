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
    
    /// <summary>
    /// OnTriggerStay : Appelée chaque frame où un objet reste à l'intérieur du volume
    /// d'un déclencheur. Utile pour des effets continus dans une zone, comme une zone
    /// de guérison, une zone de ralentissement, ou pour détecter la présence prolongée.
    /// </summary>
    /// <param name="other">Le Collider de l'objet qui reste dans la zone de déclenchement</param>
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
