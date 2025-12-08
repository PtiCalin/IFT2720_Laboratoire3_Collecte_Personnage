using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Camera birdsEyeCamera;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private bool startWithThirdPerson = true;
    [SerializeField] private bool unlockCursorInBirdView = true;

    private bool usingThirdPerson;
    private AudioListener thirdPersonListener;
    private AudioListener birdsEyeListener;
    private ThirdPersonCamera thirdPersonController;
    private BirdsEyeCamera birdsEyeController;

    private void Awake()
    {
        CacheReferences();
    }

    private void Start()
    {
        if (thirdPersonCamera == null)
        {
            thirdPersonCamera = Camera.main;
        }

        if (birdsEyeCamera == null)
        {
            BirdsEyeCamera foundBird = FindFirstObjectByType<BirdsEyeCamera>(FindObjectsInactive.Include);
            if (foundBird != null)
            {
                birdsEyeCamera = foundBird.GetComponent<Camera>();
            }
        }

        CacheReferences();
        SetActiveCamera(startWithThirdPerson);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            SetActiveCamera(!usingThirdPerson);
        }
    }

    public void Initialize(Camera thirdCamera, Camera birdCamera)
    {
        thirdPersonCamera = thirdCamera;
        birdsEyeCamera = birdCamera;
        CacheReferences();
        SetActiveCamera(startWithThirdPerson);
    }

    private void CacheReferences()
    {
        if (thirdPersonCamera != null)
        {
            thirdPersonListener = thirdPersonCamera.GetComponent<AudioListener>();
            thirdPersonController = thirdPersonCamera.GetComponent<ThirdPersonCamera>();
        }

        if (birdsEyeCamera != null)
        {
            birdsEyeListener = birdsEyeCamera.GetComponent<AudioListener>();
            if (birdsEyeListener != null)
            {
                birdsEyeListener.enabled = false;
            }
            birdsEyeController = birdsEyeCamera.GetComponent<BirdsEyeCamera>();
        }
    }

    private void SetActiveCamera(bool enableThirdPerson)
    {
        usingThirdPerson = enableThirdPerson;

        if (thirdPersonCamera != null)
        {
            thirdPersonCamera.enabled = enableThirdPerson;
            if (thirdPersonListener != null)
            {
                thirdPersonListener.enabled = enableThirdPerson;
            }
            if (thirdPersonController != null)
            {
                thirdPersonController.enabled = enableThirdPerson;
                if (enableThirdPerson)
                {
                    thirdPersonController.SnapToTarget();
                }
            }
        }

        if (birdsEyeCamera != null)
        {
            bool enableBird = !enableThirdPerson;
            birdsEyeCamera.enabled = enableBird;
            if (birdsEyeListener != null)
            {
                birdsEyeListener.enabled = enableBird;
            }
            if (birdsEyeController != null)
            {
                birdsEyeController.enabled = enableBird;
                if (enableBird)
                {
                    birdsEyeController.SnapToCenter();
                }
            }
        }

        if (unlockCursorInBirdView)
        {
            if (enableThirdPerson)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
