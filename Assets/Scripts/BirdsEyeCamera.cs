using UnityEngine;

public class BirdsEyeCamera : MonoBehaviour
{
    [SerializeField] private float height = 35f;
    [SerializeField] private float followSmoothing = 6f;
    [SerializeField] private float orthoLerpSpeed = 6f;
    [SerializeField] private float minOrthographicSize = 15f;

    private Vector3 targetCenter;
    private float targetOrthographicSize;
    private Camera cachedCamera;

    private void Awake()
    {
        cachedCamera = GetComponent<Camera>();
        if (cachedCamera != null)
        {
            cachedCamera.orthographic = true;
            if (targetOrthographicSize <= 0f)
            {
                targetOrthographicSize = cachedCamera.orthographicSize;
            }
        }
        targetCenter = transform.position;
    }

    private void OnEnable()
    {
        if (cachedCamera == null)
        {
            cachedCamera = GetComponent<Camera>();
        }
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(targetCenter.x, height, targetCenter.z);
        float followLerp = 1f - Mathf.Exp(-followSmoothing * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLerp);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        if (cachedCamera != null)
        {
            float lerp = 1f - Mathf.Exp(-orthoLerpSpeed * Time.deltaTime);
            cachedCamera.orthographicSize = Mathf.Lerp(cachedCamera.orthographicSize, targetOrthographicSize, lerp);
        }
    }

    public void SetCenter(Vector3 center)
    {
        targetCenter = new Vector3(center.x, 0f, center.z);
    }

    public void ConfigureBounds(float width, float depth)
    {
        float halfExtent = Mathf.Max(width, depth) * 0.5f;
        targetOrthographicSize = Mathf.Max(halfExtent, minOrthographicSize);
        if (cachedCamera != null)
        {
            cachedCamera.nearClipPlane = 0.3f;
            cachedCamera.farClipPlane = Mathf.Max(height * 2f, 100f);
        }
    }

    public void SnapToCenter()
    {
        transform.position = new Vector3(targetCenter.x, height, targetCenter.z);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        if (cachedCamera != null)
        {
            cachedCamera.orthographicSize = targetOrthographicSize;
        }
    }
}
