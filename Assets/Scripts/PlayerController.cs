using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Paramètres de Déplacement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField] private float rotationSharpness = 12f;
    [SerializeField] private Transform referenceCamera;
    [SerializeField] private Transform character;
    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 desiredHorizontalVelocity;
    private Vector3 desiredLookDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CacheCamera();
    }

    void Update()
    {
        if (referenceCamera == null)
        {
            CacheCamera();
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(moveHorizontal, 0f, moveVertical);
        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        if (input.sqrMagnitude > 0f)
        {
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;
            if (referenceCamera != null)
            {
                forward = Vector3.ProjectOnPlane(referenceCamera.forward, Vector3.up).normalized;
                right = Vector3.ProjectOnPlane(referenceCamera.right, Vector3.up).normalized;
            }

            Vector3 moveDirection = (right * input.x + forward * input.z).normalized;
            desiredHorizontalVelocity = moveDirection * moveSpeed;
            desiredLookDirection = moveDirection;
        }
        else
        {
            desiredHorizontalVelocity = Vector3.zero;
            desiredLookDirection = Vector3.zero;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 velocity = new Vector3(desiredHorizontalVelocity.x, currentVelocity.y, desiredHorizontalVelocity.z);
        rb.linearVelocity = velocity;

        if (desiredLookDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredLookDirection, Vector3.up);
            Quaternion smoothed = Quaternion.Slerp(rb.rotation, targetRotation, 1f - Mathf.Exp(-rotationSharpness * Time.fixedDeltaTime));
            rb.MoveRotation(smoothed);

            if (character != null)
            {
                character.rotation = smoothed;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Joueur a atterri sur le sol");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Joueur a quitté le sol");
        }
    }

    private void CacheCamera()
    {
        if (referenceCamera != null)
        {
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            referenceCamera = mainCamera.transform;
        }
    }
}
