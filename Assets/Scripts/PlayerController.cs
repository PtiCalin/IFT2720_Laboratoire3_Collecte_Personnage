using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private Transform character;
    
    private Rigidbody rb;
    private Transform cam;
    private bool isGrounded;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main?.transform;
    }

    void Update()
    {
        // Movement is handled in OnMove callback
        // Jump input
        if (inputActions.Player.Attack.triggered && isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        
        // Calculate camera-relative movement direction
        Vector3 moveDir = Vector3.zero;
        if (input.sqrMagnitude > 0.01f && cam != null)
        {
            Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
            moveDir = (right * input.x + forward * input.y).normalized * moveSpeed;
        }

        // Apply horizontal velocity while preserving vertical
        rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

        // Rotate character to face movement direction
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            Quaternion smoothRot = Quaternion.Slerp(rb.rotation, targetRot, 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime));
            rb.MoveRotation(smoothRot);
            
            if (character != null)
                character.rotation = smoothRot;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
