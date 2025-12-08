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
        if (cam == null)
            cam = Camera.main?.transform;

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
        if (input.sqrMagnitude > 0.01f)
        {
            if (cam != null)
            {
                Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
                Vector3 right = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
                moveDir = (right * input.x + forward * input.y).normalized * moveSpeed;
            }
            else
            {
                moveDir = new Vector3(input.x, 0f, input.y).normalized * moveSpeed;
            }
        }

        // Apply horizontal velocity while preserving vertical
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDir.x;
        velocity.z = moveDir.z;
        rb.linearVelocity = velocity;

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
        if (IsGroundContact(collision))
            isGrounded = true;
    }

    void OnCollisionStay(Collision collision)
    {
        if (IsGroundContact(collision))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsGroundContact(collision))
            isGrounded = false;
    }

    private bool IsGroundContact(Collision collision)
    {
        // Treat collisions with an upward normal as ground, independent of tag
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
                return true;
        }
        return false;
    }
}
