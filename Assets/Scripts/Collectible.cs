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

    // ==========================================
    // MÉTHODES DE DÉCLENCHEUR (TRIGGER)
    // ==========================================
    // Les événements de déclencheur sont utilisés lorsque vous voulez savoir qu'un objet
    // est entré dans une zone sans provoquer de réponse physique.
    // Le Collider doit avoir "Is Trigger" activé dans l'Inspector Unity.
    
    /// <summary>
    /// OnTriggerEnter : Appelée lorsqu'un autre objet entre dans le volume d'un déclencheur
    /// (un collider avec "Is Trigger" activé).
    /// Utilisé ici pour détecter quand le joueur touche un objet à collecter.
    /// Contrairement à OnCollisionEnter, cela ne provoque pas de collision physique.
    /// </summary>
    /// <param name="other">Le Collider de l'objet qui est entré dans la zone de déclenchement</param>
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
    private void OnTriggerStay(Collider other)
    {
        // Exemple : Cette méthode pourrait être utilisée pour un effet continu
        // Par exemple, une zone de pouvoir qui donne des bonus tant que le joueur y reste
        
        if (other.CompareTag("Player") && !isCollected)
        {
            // Exemple d'utilisation : afficher un indicateur visuel tant que le joueur est proche
            // ou appliquer un effet de particules continu
            // Note : Pour un objet collectible, on collecte généralement immédiatement
            // dans OnTriggerEnter, donc cette méthode ne sera pas souvent utilisée ici.
        }
    }
    
    /// <summary>
    /// OnTriggerExit : Appelée lorsqu'un objet quitte le volume d'un déclencheur.
    /// Utile pour nettoyer des effets ou notifier que le joueur a quitté une zone.
    /// Par exemple, désactiver un bonus quand le joueur quitte une zone de pouvoir.
    /// </summary>
    /// <param name="other">Le Collider de l'objet qui a quitté la zone de déclenchement</param>
    private void OnTriggerExit(Collider other)
    {
        // Exemple : Cette méthode pourrait être utilisée pour notifier qu'un joueur
        // a quitté une zone sans collecter l'objet
        
        if (other.CompareTag("Player") && !isCollected)
        {
            Debug.Log($"Le joueur a quitté la zone de {gameObject.name} sans le collecter");
            // Exemple : Vous pourriez faire clignoter l'objet pour attirer l'attention
            // ou réinitialiser un indicateur visuel
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
