using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject defaultPlayerPrefab;
    
    [Header("Configuration du Labyrinthe")]
    [SerializeField, Min(2)] private int mazeRows = 12;
    [SerializeField, Min(2)] private int mazeColumns = 12;
    [SerializeField, Min(1f)] private float cellSize = 4f;
    [SerializeField] private float wallHeight = 2f;
    [SerializeField] private float wallThickness = 0.5f;
    
    [Header("Matériaux (Optionnel)")]
    [SerializeField] private Texture2D groundTexture;
    [SerializeField] private Texture2D wallTexture;
    [SerializeField] private Texture2D coinTexture;
    [SerializeField] private string groundTextureSearchTerm = "Poliigon_WoodFloorAsh_4186";
    [SerializeField] private string wallTextureSearchTerm = "Poliigon_BrickWallReclaimed_8320";
    [SerializeField] private string coinTextureSearchTerm = "Poliigon_MetalBronzeWorn_7248";

    private GameObject player;
    private GameObject levelParent;
    private bool[,,] mazeLayout;
    private int cachedRows;
    private int cachedColumns;
    private float cachedSpacing;
    private float cachedCellHalf;
    private float cachedOffsetX;
    private float cachedOffsetZ;
    private Vector3 cachedMazeCenter;
    private float cachedMazeWidth;
    private float cachedMazeDepth;
    private bool[,] occupiedCells;
    private List<Vector2Int> availableSpawnCells;

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

        cachedMazeWidth = columns * spacing;
        cachedMazeDepth = rows * spacing;
        cachedMazeCenter = GetCellCenterPosition(rows / 2, columns / 2);

        CameraRigController cameraRig = FindFirstObjectByType<CameraRigController>(FindObjectsInactive.Include);
        if (cameraRig != null)
        {
            cameraRig.SetCenter(cachedMazeCenter);
            cameraRig.ConfigureBounds(cachedMazeWidth, cachedMazeDepth);
            if (cameraRig.CurrentMode == CameraRigController.CameraMode.BirdsEye)
            {
                cameraRig.SnapToCenter();
            }
        }

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

        occupiedCells = new bool[rows, columns];
        availableSpawnCells = null;
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
        GameObject prefabToUse = playerPrefab != null ? playerPrefab : defaultPlayerPrefab;

        if (prefabToUse != null)
        {
            player = Instantiate(prefabToUse, playerStartPosition, Quaternion.identity, levelParent.transform);
            player.name = prefabToUse.name;
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

        int spawnColumn = Mathf.Clamp(playerStartCell.x, 0, cachedColumns - 1);
        int spawnRow = Mathf.Clamp(playerStartCell.y, 0, cachedRows - 1);
        ReserveCell(spawnRow, spawnColumn);

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

        CameraRigController cameraRig = FindFirstObjectByType<CameraRigController>(FindObjectsInactive.Include);
        if (cameraRig != null)
        {
            cameraRig.SetTarget(player.transform);
            if (cameraRig.CurrentMode == CameraRigController.CameraMode.ThirdPerson)
            {
                cameraRig.SnapToTarget();
            }
        }

        Debug.Log("Joueur créé à la position: " + spawnPosition + (prefabToUse != null ? " avec le modèle fourni." : " via un objet placeholder."));
    }

    /// <summary>
    /// Crée les pièces et trésors collectibles à des positions aléatoires
    /// </summary>
    private void CreateCollectibles()
    {
        GameObject collectiblesParent = new GameObject("Collectibles");
        collectiblesParent.transform.parent = levelParent.transform;

        EnsureSpawnCellPool();
        int coinsPlaced = 0;
        int treasuresPlaced = 0;
        
        // Créer les pièces
        for (int i = 0; i < numberOfCoins; i++)
        {
            if (!TryReserveSpawnPosition(1f, 0.35f, out Vector3 position))
            {
                Debug.LogWarning("Impossible de placer toutes les pièces : plus de cellules disponibles sans collision.");
                break;
            }

            CreateCoin(position, collectiblesParent);
            coinsPlaced++;
        }
        
        // Créer les trésors
        for (int i = 0; i < numberOfTreasures; i++)
        {
            if (!TryReserveSpawnPosition(1.5f, 0.45f, out Vector3 position))
            {
                Debug.LogWarning("Impossible de placer tous les trésors : plus de cellules disponibles sans collision.");
                break;
            }

            CreateTreasure(position, collectiblesParent);
            treasuresPlaced++;
        }
        
        Debug.Log($"{coinsPlaced} pièces et {treasuresPlaced} trésors créés");
    }

    /// <summary>
    /// Crée une pièce collectible
    /// </summary>
    private void CreateCoin(Vector3 position, GameObject parent)
    {
        GameObject coin;
        bool usedFallback = coinPrefab == null;

        if (!usedFallback)
        {
            coin = Instantiate(coinPrefab, position, Quaternion.identity, parent.transform);
            coin.name = coinPrefab.name;
        }
        else
        {
            coin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            coin.name = "Coin";
            coin.transform.SetParent(parent.transform, true);
            coin.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (coin.transform.parent != parent.transform)
        {
            coin.transform.SetParent(parent.transform, true);
        }
        coin.transform.position = position;

        // Configurer les colliders comme triggers
        Collider[] colliders = coin.GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            Collider generated = coin.AddComponent<SphereCollider>();
            generated.isTrigger = true;
        }
        else
        {
            foreach (Collider col in colliders)
            {
                col.isTrigger = true;
            }
        }

        // Ajouter le script Collectible le cas échéant
        Collectible collectible = coin.GetComponent<Collectible>();
        if (collectible == null)
        {
            collectible = coin.AddComponent<Collectible>();
        }

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

        // Appliquer le matériau si disponible
        if (coinMaterial != null)
        {
            foreach (Renderer renderer in coin.GetComponentsInChildren<Renderer>())
            {
                renderer.material = coinMaterial;
            }
        }
        else if (usedFallback)
        {
            Renderer renderer = coin.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow;
            }
        }
    }

    /// <summary>
    /// Crée un trésor collectible
    /// </summary>
    private void CreateTreasure(Vector3 position, GameObject parent)
    {
        GameObject treasure;
        bool usedFallback = treasurePrefab == null;

        if (!usedFallback)
        {
            treasure = Instantiate(treasurePrefab, position, Quaternion.identity, parent.transform);
            treasure.name = treasurePrefab.name;
        }
        else
        {
            treasure = GameObject.CreatePrimitive(PrimitiveType.Cube);
            treasure.name = "Treasure";
            treasure.transform.SetParent(parent.transform, true);
            treasure.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        if (treasure.transform.parent != parent.transform)
        {
            treasure.transform.SetParent(parent.transform, true);
        }
        treasure.transform.position = position;

        // Configurer les colliders comme triggers
        Collider[] colliders = treasure.GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            Collider generated = treasure.AddComponent<BoxCollider>();
            generated.isTrigger = true;
        }
        else
        {
            foreach (Collider col in colliders)
            {
                col.isTrigger = true;
            }
        }

        // Ajouter le script Collectible le cas échéant
        Collectible collectible = treasure.GetComponent<Collectible>();
        if (collectible == null)
        {
            collectible = treasure.AddComponent<Collectible>();
        }

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

        // Appliquer le matériau si disponible
        if (treasureMaterial != null)
        {
            foreach (Renderer renderer in treasure.GetComponentsInChildren<Renderer>())
            {
                renderer.material = treasureMaterial;
            }
        }
        else if (usedFallback)
        {
            Renderer renderer = treasure.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.5f, 0f);
            }
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

    private void ReserveCell(int row, int column)
    {
        if (occupiedCells == null)
        {
            return;
        }

        if (row < 0 || row >= occupiedCells.GetLength(0) || column < 0 || column >= occupiedCells.GetLength(1))
        {
            return;
        }

        occupiedCells[row, column] = true;
    }

    private void EnsureSpawnCellPool()
    {
        if (cachedRows <= 0 || cachedColumns <= 0)
        {
            availableSpawnCells = null;
            return;
        }

        if (occupiedCells == null || occupiedCells.GetLength(0) != cachedRows || occupiedCells.GetLength(1) != cachedColumns)
        {
            occupiedCells = new bool[cachedRows, cachedColumns];
        }

        availableSpawnCells ??= new List<Vector2Int>();
        availableSpawnCells.Clear();

        for (int row = 0; row < cachedRows; row++)
        {
            for (int column = 0; column < cachedColumns; column++)
            {
                if (!occupiedCells[row, column])
                {
                    availableSpawnCells.Add(new Vector2Int(row, column));
                }
            }
        }
    }

    private bool TryReserveSpawnPosition(float height, float clearance, out Vector3 position)
    {
        position = Vector3.zero;

        if (availableSpawnCells == null || availableSpawnCells.Count == 0)
        {
            return false;
        }

        int index = Random.Range(0, availableSpawnCells.Count);
        Vector2Int cell = availableSpawnCells[index];
        availableSpawnCells.RemoveAt(index);

        ReserveCell(cell.x, cell.y);

        Vector3 cellCenter = GetCellCenterPosition(cell.x, cell.y);
        cellCenter.y = height;

        float maxOffset = Mathf.Max(cachedCellHalf - clearance, 0f);
        float offsetX = maxOffset > 0f ? Random.Range(-maxOffset, maxOffset) : 0f;
        float offsetZ = maxOffset > 0f ? Random.Range(-maxOffset, maxOffset) : 0f;

        position = cellCenter + new Vector3(offsetX, 0f, offsetZ);
        return true;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        bool changed = false;

        changed |= EnsureMaterialsFolder();
        changed |= AutoAssignDefaultAsset(ref coinPrefab, "Assets/Models/Collectibles/coins/coin-4.fbx");
        changed |= AutoAssignDefaultAsset(ref treasurePrefab, "Assets/Models/Collectibles/chests/chest-1.fbx");
        changed |= AutoAssignDefaultAsset(ref defaultPlayerPrefab, "Assets/Models/Characters/character_root.fbx");

        changed |= EnsureMaterial(ref groundMaterial, "Ground", new Color(0.65f, 0.6f, 0.55f), "Ground", groundTexture, groundTextureSearchTerm);
        changed |= EnsureMaterial(ref wallMaterial, "Wall", new Color(0.6f, 0.6f, 0.6f), "Wall", wallTexture, wallTextureSearchTerm);
        changed |= EnsureMaterial(ref coinMaterial, "Coin", Color.yellow, "Coin", coinTexture, coinTextureSearchTerm);
        changed |= EnsureMaterial(ref treasureMaterial, "Treasure", new Color(1f, 0.5f, 0f), "Treasure");

        if (changed)
        {
            EditorUtility.SetDirty(this);
        }
    }

    private bool AutoAssignDefaultAsset(ref GameObject targetField, string assetPath)
    {
        if (targetField != null)
        {
            return false;
        }

        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if (asset == null)
        {
            return false;
        }

        targetField = asset;
        return true;
    }

    private bool EnsureMaterialsFolder()
    {
        if (string.IsNullOrEmpty(materialsFolderPath))
        {
            materialsFolderPath = "Assets/Materials";
        }

        materialsFolderPath = materialsFolderPath.Replace("\\", "/");

        if (AssetDatabase.IsValidFolder(materialsFolderPath))
        {
            return false;
        }

        string[] parts = materialsFolderPath.Split('/');
        if (parts.Length == 0)
        {
            return false;
        }

        string currentPath = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string nextPath = currentPath + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(nextPath))
            {
                AssetDatabase.CreateFolder(currentPath, parts[i]);
            }
            currentPath = nextPath;
        }

        return true;
    }

    private bool EnsureMaterial(
        ref Material targetField,
        string defaultName,
        Color fallbackColor,
        string searchTerm,
        Texture2D preferredTexture = null,
        string textureSearchTerm = null)
    {
        bool modified = false;

        if (targetField != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(targetField);
            if (string.IsNullOrEmpty(assetPath))
            {
                string newPath = GetUniqueMaterialPath(defaultName);
                Material clone = Object.Instantiate(targetField);
                clone.name = defaultName;
                ApplyMaterialColor(clone, fallbackColor);
                ApplyMaterialTexture(clone, preferredTexture, textureSearchTerm ?? searchTerm);
                AssetDatabase.CreateAsset(clone, newPath);
                targetField = clone;
                return true;
            }

            assetPath = assetPath.Replace("\\", "/");
            if (!assetPath.StartsWith(materialsFolderPath))
            {
                string fileName = Path.GetFileName(assetPath);
                string targetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(materialsFolderPath, fileName).Replace("\\", "/"));
                string error = AssetDatabase.MoveAsset(assetPath, targetPath);
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogWarning($"Impossible de déplacer {assetPath} vers {targetPath}. Création d'une copie: {error}");
                    Material clone = Object.Instantiate(targetField);
                    clone.name = defaultName;
                    ApplyMaterialColor(clone, fallbackColor);
                    ApplyMaterialTexture(clone, preferredTexture, textureSearchTerm ?? searchTerm);
                    targetPath = GetUniqueMaterialPath(defaultName);
                    AssetDatabase.CreateAsset(clone, targetPath);
                    targetField = clone;
                }
                modified = true;
            }

            if (ApplyMaterialTexture(targetField, preferredTexture, textureSearchTerm ?? searchTerm))
            {
                modified = true;
            }

            return modified;
        }

        string materialPath = FindMaterialPath(searchTerm);
        if (!string.IsNullOrEmpty(materialPath))
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material != null)
            {
                targetField = material;
                if (ApplyMaterialTexture(targetField, preferredTexture, textureSearchTerm ?? searchTerm))
                {
                    modified = true;
                }
                return true;
            }
        }

        string newAssetPath = GetUniqueMaterialPath(defaultName);
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material created = new Material(shader)
        {
            name = defaultName
        };
        ApplyMaterialColor(created, fallbackColor);
        ApplyMaterialTexture(created, preferredTexture, textureSearchTerm ?? searchTerm);
        AssetDatabase.CreateAsset(created, newAssetPath);
        targetField = created;
        return true;
    }

    private string FindMaterialPath(string searchTerm)
    {
        if (string.IsNullOrEmpty(materialsFolderPath) || !AssetDatabase.IsValidFolder(materialsFolderPath))
        {
            return null;
        }

        string filter = string.IsNullOrWhiteSpace(searchTerm) ? "t:Material" : $"t:Material {searchTerm}";
        string[] guids = AssetDatabase.FindAssets(filter, new[] { materialsFolderPath });
        if (guids.Length == 0 && filter != "t:Material")
        {
            guids = AssetDatabase.FindAssets("t:Material", new[] { materialsFolderPath });
        }

        if (guids.Length == 0)
        {
            return null;
        }

        return AssetDatabase.GUIDToAssetPath(guids[0]);
    }

    private string FindTextureAsset(string searchTerm)
    {
        if (string.IsNullOrEmpty(materialsFolderPath) || !AssetDatabase.IsValidFolder(materialsFolderPath))
        {
            return null;
        }

        string filter = string.IsNullOrWhiteSpace(searchTerm) ? "t:Texture2D" : $"t:Texture2D {searchTerm}";
        string[] guids = AssetDatabase.FindAssets(filter, new[] { materialsFolderPath });
        if (guids.Length == 0 && filter != "t:Texture2D")
        {
            guids = AssetDatabase.FindAssets("t:Texture2D", new[] { materialsFolderPath });
        }

        if (guids.Length == 0)
        {
            return null;
        }

        string bestPath = null;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path)?.ToLowerInvariant();
            if (fileName != null && (fileName.Contains("basecolor") || fileName.Contains("albedo")))
            {
                bestPath = path;
                break;
            }

            bestPath ??= path;
        }

        return bestPath;
    }

    private string GetUniqueMaterialPath(string defaultName)
    {
        string fileName = string.IsNullOrWhiteSpace(defaultName) ? "Material" : defaultName;
        string combined = Path.Combine(materialsFolderPath, fileName + ".mat");
        combined = combined.Replace("\\", "/");
        return AssetDatabase.GenerateUniqueAssetPath(combined);
    }

    private void ApplyMaterialColor(Material material, Color color)
    {
        if (material == null)
        {
            return;
        }

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }
        else if (material.HasProperty("_Color"))
        {
            material.color = color;
        }
    }

    private bool ApplyMaterialTexture(Material material, Texture2D preferredTexture, string searchTerm)
    {
        if (material == null)
        {
            return false;
        }

        Texture2D texture = preferredTexture;
        if (texture == null && !string.IsNullOrWhiteSpace(searchTerm))
        {
            string texturePath = FindTextureAsset(searchTerm);
            if (!string.IsNullOrEmpty(texturePath))
            {
                texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            }
        }

        if (texture == null)
        {
            return false;
        }

        bool changed = false;
        if (material.HasProperty("_BaseMap") && material.GetTexture("_BaseMap") != texture)
        {
            material.SetTexture("_BaseMap", texture);
            changed = true;
        }
        else if (material.HasProperty("_MainTex") && material.mainTexture != texture)
        {
            material.mainTexture = texture;
            changed = true;
        }

        return changed;
    }
#endif
}
