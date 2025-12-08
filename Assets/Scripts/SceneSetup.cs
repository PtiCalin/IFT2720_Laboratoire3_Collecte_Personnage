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
        Light directionalLight = FindFirstObjectByType<Light>();

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
            cameraObj.tag = "MainCamera";
        }

        if (mainCamera.GetComponent<AudioListener>() == null)
        {
            mainCamera.gameObject.AddComponent<AudioListener>();
        }

        mainCamera.transform.position = new Vector3(0f, 2f, -6f);
        mainCamera.transform.rotation = Quaternion.identity;
        mainCamera.backgroundColor = new Color(0.2f, 0.3f, 0.4f);

        ThirdPersonCamera thirdPerson = mainCamera.GetComponent<ThirdPersonCamera>();
        if (thirdPerson == null)
        {
            thirdPerson = mainCamera.gameObject.AddComponent<ThirdPersonCamera>();
        }

        GameObject birdsEyeObj = GameObject.Find("BirdsEyeCamera");
        if (birdsEyeObj == null)
        {
            birdsEyeObj = new GameObject("BirdsEyeCamera");
        }

        Camera birdsEyeCamera = birdsEyeObj.GetComponent<Camera>();
        if (birdsEyeCamera == null)
        {
            birdsEyeCamera = birdsEyeObj.AddComponent<Camera>();
        }
        birdsEyeCamera.orthographic = true;
        birdsEyeCamera.enabled = false;
        birdsEyeObj.transform.position = new Vector3(0f, 35f, 0f);
        birdsEyeObj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        AudioListener birdsListener = birdsEyeObj.GetComponent<AudioListener>();
        if (birdsListener != null)
        {
            birdsListener.enabled = false;
        }

        BirdsEyeCamera birdsEyeController = birdsEyeObj.GetComponent<BirdsEyeCamera>();
        if (birdsEyeController == null)
        {
            birdsEyeController = birdsEyeObj.AddComponent<BirdsEyeCamera>();
        }
        birdsEyeController.enabled = false;

        GameObject cameraManager = GameObject.Find("CameraManager");
        if (cameraManager == null)
        {
            cameraManager = new GameObject("CameraManager");
        }

        CameraSwitcher switcher = cameraManager.GetComponent<CameraSwitcher>();
        if (switcher == null)
        {
            switcher = cameraManager.AddComponent<CameraSwitcher>();
        }
        switcher.Initialize(mainCamera, birdsEyeCamera);

        Debug.Log("✓ Caméra configurée");
    }

    /// <summary>
    /// Configure le GameManager s'il n'existe pas
    /// </summary>
    private void SetupGameManager()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

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
