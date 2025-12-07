using UnityEngine;

/// <summary>
/// Initialise la scène 3D avec tous les éléments nécessaires au jeu
/// S'exécute automatiquement au démarrage de la scène
/// </summary>
public class SceneSetup : MonoBehaviour
{
    [Header("Paramètres de Configuration")]
    [SerializeField] private bool generateLevelOnStart = true;
    [SerializeField] private bool setupCamera = true;
    [SerializeField] private bool setupGameManager = true;
    [SerializeField] private bool setupLighting = true;

    private void Start()
    {
        if (generateLevelOnStart)
        {
            SetupScene();
        }
    }

    /// <summary>
    /// Configure complètement la scène
    /// </summary>
    private void SetupScene()
    {
        Debug.Log("=== Initialisation de la Scène 3D ===");

        if (setupLighting)
            SetupLighting();

        if (setupCamera)
            SetupCamera();

        if (setupGameManager)
            SetupGameManager();

        Debug.Log("=== Configuration de la Scène Terminée ===");
    }

    /// <summary>
    /// Configure l'éclairage de la scène
    /// </summary>
    private void SetupLighting()
    {
        // Chercher ou créer une lumière directionnelle (soleil)
        Light directionalLight = FindObjectOfType<Light>();

        if (directionalLight == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            directionalLight = lightObj.AddComponent<Light>();
            directionalLight.type = LightType.Directional;
            directionalLight.intensity = 1f;
            lightObj.transform.eulerAngles = new Vector3(50, -30, 0);
        }

        Debug.Log("✓ Éclairage configuré");
    }

    /// <summary>
    /// Configure la caméra principale
    /// </summary>
    private void SetupCamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            mainCamera = cameraObj.AddComponent<Camera>();
            cameraObj.AddComponent<AudioListener>();
        }

        // Positionner la caméra pour une bonne vue du jeu
        mainCamera.transform.position = new Vector3(0, 20, -15);
        mainCamera.transform.eulerAngles = new Vector3(45, 0, 0);
        mainCamera.backgroundColor = new Color(0.2f, 0.3f, 0.4f); // Bleu ciel

        Debug.Log("✓ Caméra configurée");
    }

    /// <summary>
    /// Configure le GameManager s'il n'existe pas
    /// </summary>
    private void SetupGameManager()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            GameObject gmObj = new GameObject("GameManager");
            gameManager = gmObj.AddComponent<GameManager>();
            Debug.Log("✓ GameManager créé");
        }
        else
        {
            Debug.Log("✓ GameManager trouvé");
        }
    }
}
