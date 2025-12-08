using System.Collections.Generic;
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
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2Int playerStartCell = Vector2Int.zero;
    
    [Header("Configuration des Collectibles")]
    [SerializeField] private int numberOfCoins = 10;
    [SerializeField] private int numberOfTreasures = 3;
    [SerializeField] private int coinPointsValue = 10;
    [SerializeField] private int treasurePointsValue = 50;
    
    [Header("Configuration du Labyrinthe")]
    [SerializeField, Min(2)] private int mazeRows = 12;
    [SerializeField, Min(2)] private int mazeColumns = 12;
    [SerializeField, Min(1f)] private float cellSize = 4f;
    [SerializeField] private float wallHeight = 2f;
    [SerializeField] private float wallThickness = 0.5f;
    
    [Header("Matériaux (Optionnel)")]
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material coinMaterial;
    [SerializeField] private Material treasureMaterial;

    private GameObject player;
    private GameObject levelParent;
    private bool[,,] mazeLayout;
    private int cachedRows;
    private int cachedColumns;
    private float cachedSpacing;
    private float cachedCellHalf;
    private float cachedOffsetX;
    private float cachedOffsetZ;

    private enum MazeDirection
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3
    }

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
        int rows = Mathf.Max(mazeRows, 2);
        int columns = Mathf.Max(mazeColumns, 2);
        float spacing = Mathf.Max(cellSize, 1f);

        GameObject mazeParent = new GameObject("Maze");
        mazeParent.transform.SetParent(levelParent.transform, true);

        cachedRows = rows;
        cachedColumns = columns;
        cachedSpacing = spacing;
        mazeLayout = GenerateMazeLayout(rows, columns);
        BuildMazeGeometry(mazeParent, mazeLayout, rows, columns, spacing);

        Debug.Log($"Labyrinthe généré ({rows}x{columns}) avec un tracé aléatoire.");
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

    private bool[,,] GenerateMazeLayout(int rows, int columns)
    {
        bool[,,] layout = new bool[rows, columns, 4];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    layout[row, col, dir] = true;
                }
            }
        }

        bool[,] visited = new bool[rows, columns];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        Vector2Int startCell = new Vector2Int(0, 0);
        visited[startCell.x, startCell.y] = true;
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            Vector2Int currentCell = stack.Peek();
            List<(MazeDirection direction, Vector2Int cell)> neighbors = GetUnvisitedNeighbors(currentCell, visited, rows, columns);

            if (neighbors.Count > 0)
            {
                int randomIndex = Random.Range(0, neighbors.Count);
                MazeDirection direction = neighbors[randomIndex].direction;
                Vector2Int nextCell = neighbors[randomIndex].cell;

                RemoveWallBetween(layout, currentCell, nextCell, direction);
                visited[nextCell.x, nextCell.y] = true;
                stack.Push(nextCell);
            }
            else
            {
                stack.Pop();
            }
        }

        return layout;
    }

    private List<(MazeDirection direction, Vector2Int cell)> GetUnvisitedNeighbors(Vector2Int cell, bool[,] visited, int rows, int columns)
    {
        var neighbors = new List<(MazeDirection direction, Vector2Int cell)>();
        int row = cell.x;
        int col = cell.y;

        if (row + 1 < rows && !visited[row + 1, col])
        {
            neighbors.Add((MazeDirection.North, new Vector2Int(row + 1, col)));
        }

        if (col + 1 < columns && !visited[row, col + 1])
        {
            neighbors.Add((MazeDirection.East, new Vector2Int(row, col + 1)));
        }

        if (row - 1 >= 0 && !visited[row - 1, col])
        {
            neighbors.Add((MazeDirection.South, new Vector2Int(row - 1, col)));
        }

        if (col - 1 >= 0 && !visited[row, col - 1])
        {
            neighbors.Add((MazeDirection.West, new Vector2Int(row, col - 1)));
        }

        return neighbors;
    }

    private void RemoveWallBetween(bool[,,] layout, Vector2Int current, Vector2Int next, MazeDirection direction)
    {
        layout[current.x, current.y, (int)direction] = false;
        MazeDirection opposite = GetOppositeDirection(direction);
        layout[next.x, next.y, (int)opposite] = false;
    }

    private MazeDirection GetOppositeDirection(MazeDirection direction)
    {
        switch (direction)
        {
            case MazeDirection.North:
                return MazeDirection.South;
            case MazeDirection.South:
                return MazeDirection.North;
            case MazeDirection.East:
                return MazeDirection.West;
            case MazeDirection.West:
                return MazeDirection.East;
            default:
                return MazeDirection.North;
        }
    }

    private void BuildMazeGeometry(GameObject mazeParent, bool[,,] layout, int rows, int columns, float spacing)
    {
        float cellHalf = spacing * 0.5f;
        float offsetX = -columns * spacing * 0.5f;
        float offsetZ = -rows * spacing * 0.5f;

        cachedCellHalf = cellHalf;
        cachedOffsetX = offsetX;
        cachedOffsetZ = offsetZ;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 cellCenter = new Vector3(
                    offsetX + col * spacing + cellHalf,
                    wallHeight * 0.5f,
                    offsetZ + row * spacing + cellHalf);

                if (layout[row, col, (int)MazeDirection.North])
                {
                    Vector3 position = cellCenter + new Vector3(0f, 0f, cellHalf);
                    CreateWall(position, new Vector3(spacing, wallHeight, wallThickness), mazeParent);
                }

                if (layout[row, col, (int)MazeDirection.East])
                {
                    Vector3 position = cellCenter + new Vector3(cellHalf, 0f, 0f);
                    CreateWall(position, new Vector3(wallThickness, wallHeight, spacing), mazeParent);
                }

                if (row == 0 && layout[row, col, (int)MazeDirection.South])
                {
                    Vector3 position = cellCenter - new Vector3(0f, 0f, cellHalf);
                    CreateWall(position, new Vector3(spacing, wallHeight, wallThickness), mazeParent);
                }

                if (col == 0 && layout[row, col, (int)MazeDirection.West])
                {
                    Vector3 position = cellCenter - new Vector3(cellHalf, 0f, 0f);
                    CreateWall(position, new Vector3(wallThickness, wallHeight, spacing), mazeParent);
                }
            }
        }
    }

    private Vector3 GetCellCenterPosition(int row, int column)
    {
        if (cachedSpacing <= 0f || cachedRows <= 0 || cachedColumns <= 0)
        {
            return Vector3.zero;
        }

        row = Mathf.Clamp(row, 0, cachedRows - 1);
        column = Mathf.Clamp(column, 0, cachedColumns - 1);

        float x = cachedOffsetX + column * cachedSpacing + cachedCellHalf;
        float z = cachedOffsetZ + row * cachedSpacing + cachedCellHalf;
        return new Vector3(x, 0f, z);
    }

    private Vector3 GetPlayerSpawnPosition()
    {
        if (mazeLayout == null || cachedRows <= 0 || cachedColumns <= 0)
        {
            return playerStartPosition;
        }

        int column = Mathf.Clamp(playerStartCell.x, 0, cachedColumns - 1);
        int row = Mathf.Clamp(playerStartCell.y, 0, cachedRows - 1);
        Vector3 cellCenter = GetCellCenterPosition(row, column);
        cellCenter.y = playerStartPosition.y;
        return cellCenter;
    }

    /// <summary>
    /// Crée le personnage joueur avec tous ses composants
    /// </summary>
    private void CreatePlayer()
    {
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity, levelParent.transform);
            player.name = "Player";
        }
        else
        {
            player = new GameObject("Player");
            player.transform.SetParent(levelParent.transform, true);
        }

        player.tag = "Player";
        Vector3 spawnPosition = GetPlayerSpawnPosition();
        player.transform.position = spawnPosition;
        if (player.transform.parent != levelParent.transform)
        {
            player.transform.SetParent(levelParent.transform, true);
        }

        // S'assurer qu'un Rigidbody est présent et configuré
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = player.AddComponent<Rigidbody>();
            rb.mass = 1f;
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Garantir la présence d'un collider utilisable pour la physique
        Collider existingCollider = player.GetComponentInChildren<Collider>();
        if (existingCollider == null)
        {
            player.AddComponent<CapsuleCollider>();
        }
        else if (existingCollider.isTrigger)
        {
            existingCollider.isTrigger = false;
        }

        // Ajouter ou configurer le script PlayerController
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = player.AddComponent<PlayerController>();
        }
        playerController.moveSpeed = playerMoveSpeed;
        playerController.jumpForce = playerJumpForce;

        ThirdPersonCamera followCamera = FindFirstObjectByType<ThirdPersonCamera>();
        if (followCamera != null)
        {
            followCamera.SetTarget(player.transform);
        }

        Debug.Log("Joueur créé à la position: " + spawnPosition + (playerPrefab != null ? " avec le modèle fourni." : " via un objet placeholder."));
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
        float spacing = Mathf.Max(cellSize, 1f);
        float width = Mathf.Max(mazeColumns, 2) * spacing;
        float depth = Mathf.Max(mazeRows, 2) * spacing;

        float halfWidth = width * 0.5f;
        float halfDepth = depth * 0.5f;
        float marginUpperBound = Mathf.Max(Mathf.Min(halfWidth, halfDepth) - 0.1f, 0.1f);
        float margin = Mathf.Clamp(spacing * 0.5f, 0.1f, marginUpperBound);

        float x = Random.Range(-halfWidth + margin, halfWidth - margin);
        float z = Random.Range(-halfDepth + margin, halfDepth - margin);
        return new Vector3(x, height, z);
    }
}
