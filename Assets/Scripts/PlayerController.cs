using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;

    [Header("Ground Check")]
    [SerializeField] private float groundDist = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if grounded using raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDist, groundLayer);

        // Handle horizontal movement input
        HandleMovement();

        // Handle jump input
        HandleJump();

        // Apply appropriate drag based on ground state
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    private void HandleMovement()
    {
        // Get horizontal input (A/D or Arrow Keys)
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Apply movement force
        Vector3 moveDirection = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0);
        rb.velocity = moveDirection;
    }

    private void HandleJump()
    {
        // Jump only when grounded and space is pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Reset vertical velocity and apply jump impulse
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
