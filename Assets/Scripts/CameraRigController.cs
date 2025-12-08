using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles both third-person orbit and bird's-eye orthographic camera modes.
/// </summary>
public class CameraRigController : MonoBehaviour
{
    public enum CameraMode
    {
        ThirdPerson,
        BirdsEye
    }

    [Header("Mode Settings")]
    [SerializeField] private CameraMode startMode = CameraMode.ThirdPerson;
    [SerializeField] private bool lockCursorInThirdPerson = true;
    [SerializeField] private bool unlockCursorInBirdsEye = true;

    [Header("Third-Person Orbit")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField, Min(0.1f)] private float distance = 6f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float verticalSensitivity = 0.8f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;
    [SerializeField] private float thirdPersonPositionSmoothing = 10f;
    [SerializeField] private float thirdPersonRotationSmoothing = 12f;

    [Header("Bird's-Eye View")]
    [SerializeField] private float birdsEyeHeight = 35f;
    [SerializeField] private float birdsEyeFollowSmoothing = 6f;
    [SerializeField] private float birdsEyeOrthoLerpSpeed = 6f;
    [SerializeField] private float birdsEyeMinOrthographicSize = 15f;

    private Camera cachedCamera;
    private CameraMode currentMode;
    private float yaw;
    private float pitch;
    private Vector3 birdsEyeCenter;
    private bool hasBirdsEyeCenter;
    private float birdsEyeTargetOrthographicSize;
    private CameraInputActions camInput;

    private void Awake()
    {
        cachedCamera = GetComponent<Camera>();
        birdsEyeCenter = Vector3.zero;
        hasBirdsEyeCenter = false;
        if (cachedCamera != null && cachedCamera.orthographic)
        {
            birdsEyeTargetOrthographicSize = Mathf.Max(cachedCamera.orthographicSize, birdsEyeMinOrthographicSize);
        }
        else
        {
            birdsEyeTargetOrthographicSize = birdsEyeMinOrthographicSize;
        }
        camInput = new CameraInputActions();
        camInput.Camera.Toggle.performed += ctx => ToggleCameraMode();
    }

    private void Start()
    {
        EnsureTarget();

        Vector3 euler = transform.rotation.eulerAngles;
        yaw = euler.y;
        pitch = ClampPitch(euler.x);

        SetMode(startMode, true);
        camInput.Enable();
    }

    private void Update()
    {
        // Camera toggle handled by Input System callback
    }

    private void LateUpdate()
    {
        switch (currentMode)
        {
            case CameraMode.ThirdPerson:
                UpdateThirdPerson();
                break;
            case CameraMode.BirdsEye:
                UpdateBirdsEye();
                break;
        }
    }

    public CameraMode CurrentMode => currentMode;

    public void SetMode(CameraMode mode, bool instant = false)
    {
        currentMode = mode;
        ApplyCameraMode(instant);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (currentMode == CameraMode.BirdsEye && newTarget != null)
        {
            SetCenter(new Vector3(newTarget.position.x, 0f, newTarget.position.z));
        }
    }

    public void SnapToTarget()
    {
        if (target == null)
        {
            return;
        }

        Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.rotation = desiredRotation;
        transform.position = target.position + targetOffset - desiredRotation * Vector3.forward * distance;
    }

    public void SetCenter(Vector3 center)
    {
        birdsEyeCenter = new Vector3(center.x, 0f, center.z);
        hasBirdsEyeCenter = true;
    }

    public void ConfigureBounds(float width, float depth)
    {
        float halfExtent = Mathf.Max(width, depth) * 0.5f;
        birdsEyeTargetOrthographicSize = Mathf.Max(halfExtent, birdsEyeMinOrthographicSize);
        if (cachedCamera != null && currentMode == CameraMode.BirdsEye)
        {
            cachedCamera.orthographicSize = birdsEyeTargetOrthographicSize;
        }
    }

    public void SnapToCenter()
    {
        Vector3 desiredPosition = new Vector3(birdsEyeCenter.x, birdsEyeHeight, birdsEyeCenter.z);
        transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        if (cachedCamera != null)
        {
            cachedCamera.orthographicSize = birdsEyeTargetOrthographicSize;
        }
    }

    private void ApplyCameraMode(bool instant)
    {
        if (cachedCamera == null)
        {
            cachedCamera = GetComponent<Camera>();
        }

        if (currentMode == CameraMode.ThirdPerson)
        {
            if (cachedCamera != null)
            {
                cachedCamera.orthographic = false;
            }
            if (lockCursorInThirdPerson)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (instant)
            {
                SnapToTarget();
            }
        }
        else
        {
            if (cachedCamera != null)
            {
                cachedCamera.orthographic = true;
                cachedCamera.orthographicSize = birdsEyeTargetOrthographicSize;
            }
            if (unlockCursorInBirdsEye)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (instant)
            {
                SnapToCenter();
            }
        }
    }

    private void UpdateThirdPerson()
    {
        EnsureTarget();
        if (target == null)
        {
            return;
        }

        Vector2 mouseDelta = camInput.Camera.Look.ReadValue<Vector2>();
        yaw += mouseDelta.x * rotationSpeed * Time.deltaTime;
        pitch -= mouseDelta.y * rotationSpeed * verticalSensitivity * Time.deltaTime;
        pitch = ClampPitch(pitch);

        Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position + targetOffset - desiredRotation * Vector3.forward * distance;

        float positionLerp = 1f - Mathf.Exp(-thirdPersonPositionSmoothing * Time.deltaTime);
        float rotationLerp = 1f - Mathf.Exp(-thirdPersonRotationSmoothing * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionLerp);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationLerp);
    }

    private void UpdateBirdsEye()
    {
        if (!hasBirdsEyeCenter && target != null)
        {
            SetCenter(target.position);
        }

        Vector3 desiredPosition = new Vector3(birdsEyeCenter.x, birdsEyeHeight, birdsEyeCenter.z);
        float followLerp = 1f - Mathf.Exp(-birdsEyeFollowSmoothing * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLerp);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        if (cachedCamera != null)
        {
            float orthoLerp = 1f - Mathf.Exp(-birdsEyeOrthoLerpSpeed * Time.deltaTime);
            cachedCamera.orthographicSize = Mathf.Lerp(cachedCamera.orthographicSize, birdsEyeTargetOrthographicSize, orthoLerp);
        }
    }

    private float ClampPitch(float rawPitch)
    {
        rawPitch = Mathf.Repeat(rawPitch + 180f, 360f) - 180f;
        return Mathf.Clamp(rawPitch, minPitch, maxPitch);
    }

    private void EnsureTarget()
    {
        if (target != null)
        {
            return;
        }

        GameObject candidate = GameObject.FindWithTag("Player");
        if (candidate != null)
        {
            target = candidate.transform;
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        camInput.Disable();
    }

    private void ToggleCameraMode()
    {
        CameraMode next = currentMode == CameraMode.ThirdPerson ? CameraMode.BirdsEye : CameraMode.ThirdPerson;
        SetMode(next, false);
    }
}
