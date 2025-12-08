using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private float distance = 6f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float verticalSensitivity = 0.8f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 70f;
    [SerializeField] private float positionSmoothing = 10f;
    [SerializeField] private float rotationSmoothing = 12f;
    [SerializeField] private bool lockCursor = true;

    private float yaw;
    private float pitch;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (target == null)
        {
            GameObject candidate = GameObject.FindWithTag("Player");
            if (candidate != null)
            {
                target = candidate.transform;
            }
        }

        Vector3 euler = transform.rotation.eulerAngles;
        yaw = euler.y;
        pitch = ClampPitch(euler.x);
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            GameObject candidate = GameObject.FindWithTag("Player");
            if (candidate == null)
            {
                return;
            }

            target = candidate.transform;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * verticalSensitivity * Time.deltaTime;
        pitch = ClampPitch(pitch);

        Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position + targetOffset - desiredRotation * Vector3.forward * distance;

        float positionLerp = 1f - Mathf.Exp(-positionSmoothing * Time.deltaTime);
        float rotationLerp = 1f - Mathf.Exp(-rotationSmoothing * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionLerp);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationLerp);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private float ClampPitch(float rawPitch)
    {
        rawPitch = Mathf.Repeat(rawPitch + 180f, 360f) - 180f;
        return Mathf.Clamp(rawPitch, minPitch, maxPitch);
    }
}
