using UnityEngine;
using CameraComponent = UnityEngine.Camera;

/// <summary>
/// Initialise la scène 3D avec tous les éléments nécessaires au jeu.
/// S'exécute automatiquement au démarrage de la scène.
/// </summary>
public class Scene : MonoBehaviour
{
    [Header("Paramètres de Configuration")]
    [SerializeField] private bool generateLevelOnStart = true;
    [SerializeField] private bool setupCamera = true;
    [SerializeField] private bool setupUI = true;
    [SerializeField] private bool setupLighting = true;

    private void Start()
    {
        if (generateLevelOnStart)
        {
            SetupScene();
        }
    }

    private void SetupScene()
    {
        if (setupLighting)
            SetupLighting();

        if (setupCamera)
            SetupCamera();

        if (setupUI)
            SetupUI();
    }

    private void SetupLighting()
    {
        Light directionalLight = FindFirstObjectByType<Light>();

        if (directionalLight == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            directionalLight = lightObj.AddComponent<Light>();
            directionalLight.type = LightType.Directional;
            directionalLight.intensity = 1f;
            lightObj.transform.eulerAngles = new Vector3(50, -30, 0);
        }
    }

    private void SetupCamera()
    {
        CameraComponent mainCamera = CameraComponent.main;

        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            mainCamera = cameraObj.AddComponent<CameraComponent>();
            cameraObj.AddComponent<AudioListener>();
        }

        mainCamera.transform.position = new Vector3(0f, 20f, -15f);
        mainCamera.transform.eulerAngles = new Vector3(45f, 0f, 0f);
        mainCamera.backgroundColor = new Color(0.2f, 0.3f, 0.4f);

        Camera rig = mainCamera.GetComponent<Camera>();
        if (rig == null)
        {
            rig = mainCamera.gameObject.AddComponent<Camera>();
        }
        rig.SetMode(Camera.CameraMode.ThirdPerson, true);
    }

    private void SetupUI()
    {
        UI ui = FindFirstObjectByType<UI>();

        if (ui == null)
        {
            GameObject gmObj = new GameObject("UI");
            gmObj.AddComponent<UI>();
        }
    }
}
