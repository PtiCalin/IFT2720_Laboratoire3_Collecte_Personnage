using UnityEngine;

/// <summary>
/// Génère automatiquement le niveau de jeu incluant :
/// - Le sol (Plane)
/// - Le labyrinthe (murs)
/// - Le joueur
/// - Les pièces et trésors collectibles
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [Header("Configuration du Sol")]
    [SerializeField] private Vector3 groundScale = new Vector3(5, 1, 5);
    
    [Header("Configuration du Joueur")]
    [SerializeField] private Vector3 playerStartPosition = new Vector3(0, 2, 0);
    [SerializeField] private float playerMoveSpeed = 5f;
    [SerializeField] private float playerJumpForce = 5f;
    
    [Header("Configuration des Collectibles")]
    [SerializeField] private int numberOfCoins = 10;
    [SerializeField] private int numberOfTreasures = 3;
    [SerializeField] private int coinPointsValue = 10;
    [SerializeField] private int treasurePointsValue = 50;
    
    [Header("Configuration du Labyrinthe")]
    [SerializeField] private float wallHeight = 2f;
    [SerializeField] private float wallThickness = 0.5f;
    
    [Header("Matériaux (Optionnel)")]
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material coinMaterial;
    [SerializeField] private Material treasureMaterial;
    [SerializeField] private Material playerMaterial;

    private GameObject player;
    private GameObject levelParent;

    void Start()
    {
        // Créer un GameObject parent pour organiser la scène
        levelParent = new GameObject("Generated Level");
        
        // Générer tous les éléments du niveau
        CreateGround();
        CreateMaze();
        CreatePlayer();
        CreateCollectibles();
        
        Debug.Log("Niveau généré avec succès!");
    }

    /// <summary>
    /// Crée le sol (Plane) du niveau
    /// </summary>
    private void CreateGround()
    {
        // Créer un plan comme sol
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.tag = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = groundScale;
        ground.transform.parent = levelParent.transform;
        
        // Appliquer le matériau si disponible
        if (groundMaterial != null)
        {
            ground.GetComponent<Renderer>().material = groundMaterial;
        }
        
        Debug.Log("Sol créé");
    }

    /// <summary>
    /// Crée un labyrinthe simple avec des murs
    /// </summary>
    private void CreateMaze()
    {
        GameObject mazeParent = new GameObject("Maze");
        mazeParent.transform.parent = levelParent.transform;
        
        // Créer les murs extérieurs (bordures)
        CreateWall(new Vector3(0, wallHeight/2, 25), new Vector3(50, wallHeight, wallThickness), mazeParent); // Mur Nord
        CreateWall(new Vector3(0, wallHeight/2, -25), new Vector3(50, wallHeight, wallThickness), mazeParent); // Mur Sud
        CreateWall(new Vector3(25, wallHeight/2, 0), new Vector3(wallThickness, wallHeight, 50), mazeParent); // Mur Est
        CreateWall(new Vector3(-25, wallHeight/2, 0), new Vector3(wallThickness, wallHeight, 50), mazeParent); // Mur Ouest
        
        // Créer quelques murs intérieurs pour former un labyrinthe simple
        CreateWall(new Vector3(-10, wallHeight/2, 0), new Vector3(wallThickness, wallHeight, 30), mazeParent);
        CreateWall(new Vector3(10, wallHeight/2, 5), new Vector3(wallThickness, wallHeight, 20), mazeParent);
        CreateWall(new Vector3(0, wallHeight/2, -10), new Vector3(20, wallHeight, wallThickness), mazeParent);
        CreateWall(new Vector3(15, wallHeight/2, 15), new Vector3(15, wallHeight, wallThickness), mazeParent);
        CreateWall(new Vector3(-15, wallHeight/2, -15), new Vector3(10, wallHeight, wallThickness), mazeParent);
        
        Debug.Log("Labyrinthe créé");
    }

    /// <summary>
    /// Crée un mur individuel
    /// </summary>
    private void CreateWall(Vector3 position, Vector3 scale, GameObject parent)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall";
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.transform.parent = parent.transform;
        
        // Appliquer le matériau si disponible
        if (wallMaterial != null)
        {
            wall.GetComponent<Renderer>().material = wallMaterial;
        }
    }

    /// <summary>
    /// Crée le personnage joueur avec tous ses composants
    /// </summary>
    private void CreatePlayer()
    {
        // Créer une capsule pour le joueur
        player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = playerStartPosition;
        player.transform.parent = levelParent.transform;
        
        // Ajouter un Rigidbody pour la physique
        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.useGravity = true;
        
        // Geler les rotations pour éviter que le joueur bascule
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        // Ajouter le script PlayerController
        PlayerController playerController = player.AddComponent<PlayerController>();
        playerController.moveSpeed = playerMoveSpeed;
        playerController.jumpForce = playerJumpForce;
        
        // Appliquer le matériau si disponible
        if (playerMaterial != null)
        {
            player.GetComponent<Renderer>().material = playerMaterial;
        }
        
        Debug.Log("Joueur créé à la position: " + playerStartPosition);
    }

    /// <summary>
    /// Crée les pièces et trésors collectibles à des positions aléatoires
    /// </summary>
    private void CreateCollectibles()
    {
        GameObject collectiblesParent = new GameObject("Collectibles");
        collectiblesParent.transform.parent = levelParent.transform;
        
        // Créer les pièces
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector3 randomPosition = GetRandomPositionInMaze(1f);
            CreateCoin(randomPosition, collectiblesParent);
        }
        
        // Créer les trésors
        for (int i = 0; i < numberOfTreasures; i++)
        {
            Vector3 randomPosition = GetRandomPositionInMaze(1.5f);
            CreateTreasure(randomPosition, collectiblesParent);
        }
        
        Debug.Log($"{numberOfCoins} pièces et {numberOfTreasures} trésors créés");
    }

    /// <summary>
    /// Crée une pièce collectible
    /// </summary>
    private void CreateCoin(Vector3 position, GameObject parent)
    {
        GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        coin.name = "Coin";
        coin.transform.position = position;
        coin.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        coin.transform.parent = parent.transform;
        
        // Configurer le collider comme trigger
        Collider collider = coin.GetComponent<Collider>();
        collider.isTrigger = true;
        
        // Ajouter le script Collectible
        Collectible collectible = coin.AddComponent<Collectible>();
        // Utiliser la réflexion pour définir les champs privés sérialisés
        var type = typeof(Collectible);
        type.GetField("isTreasure", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, false);
        type.GetField("pointsValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, coinPointsValue);
        type.GetField("rotationSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, 100f);
        type.GetField("bobSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, 2f);
        type.GetField("bobHeight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, 0.3f);
        
        // Appliquer le matériau si disponible (jaune pour les pièces)
        Renderer renderer = coin.GetComponent<Renderer>();
        if (coinMaterial != null)
        {
            renderer.material = coinMaterial;
        }
        else
        {
            renderer.material.color = Color.yellow;
        }
    }

    /// <summary>
    /// Crée un trésor collectible
    /// </summary>
    private void CreateTreasure(Vector3 position, GameObject parent)
    {
        GameObject treasure = GameObject.CreatePrimitive(PrimitiveType.Cube);
        treasure.name = "Treasure";
        treasure.transform.position = position;
        treasure.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        treasure.transform.parent = parent.transform;
        
        // Configurer le collider comme trigger
        Collider collider = treasure.GetComponent<Collider>();
        collider.isTrigger = true;
        
        // Ajouter le script Collectible
        Collectible collectible = treasure.AddComponent<Collectible>();
        // Utiliser la réflexion pour définir les champs privés sérialisés
        var type = typeof(Collectible);
        type.GetField("isTreasure", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, true);
        type.GetField("pointsValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, treasurePointsValue);
        type.GetField("rotationSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, 80f);
        type.GetField("bobSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, 1.5f);
        type.GetField("bobHeight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(collectible, 0.4f);
        
        // Appliquer le matériau si disponible (doré/orange pour les trésors)
        Renderer renderer = treasure.GetComponent<Renderer>();
        if (treasureMaterial != null)
        {
            renderer.material = treasureMaterial;
        }
        else
        {
            renderer.material.color = new Color(1f, 0.5f, 0f); // Orange/doré
        }
    }

    /// <summary>
    /// Génère une position aléatoire dans les limites du labyrinthe
    /// </summary>
    private Vector3 GetRandomPositionInMaze(float height)
    {
        float x = Random.Range(-20f, 20f);
        float z = Random.Range(-20f, 20f);
        return new Vector3(x, height, z);
    }
}
