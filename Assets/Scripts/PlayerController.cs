using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;

    [Header("Advanced Movement")]
    [SerializeField] private float airControlFactor = 0.5f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    [Header("Ground Check")]
    [SerializeField] private float groundDist = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;

    [Header("Physics Constraints")]
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float gravityMultiplier = 1.5f;

    [Header("Visual Feedback")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private bool isGrounded;
    private int jumpsRemaining;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float horizontalInput;
    private Vector3 lastGroundedPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Configure rigidbody for better physics
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;
        
        // Set default ground check point if not assigned
        if (groundCheckPoint == null)
            groundCheckPoint = transform;
            
        lastGroundedPosition = transform.position;
    }

    private void Update()
    {
        // Check if grounded using raycast
        CheckGrounded();

        // Get input
        horizontalInput = Input.GetAxis("Horizontal");

        // Handle jump input with buffer
        HandleJumpInput();

        // Apply appropriate drag based on ground state
        rb.drag = isGrounded ? groundDrag : airDrag;
        
        // Update visual rotation
        UpdateVisualRotation();
    }

    private void FixedUpdate()
    {
        // Handle horizontal movement in FixedUpdate for physics
        HandleMovement();
        
        // Apply additional gravity for better jump feel
        ApplyGravity();
        
        // Limit fall speed
        LimitFallSpeed();
    }

    private void CheckGrounded()
    {
        // Check if grounded using raycast from ground check point
        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, groundDist, groundLayer);

        // Coyote time - allows jump shortly after leaving ground
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpsRemaining = maxJumps;
            lastGroundedPosition = transform.position;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        // Calculate target velocity
        float targetSpeed = horizontalInput * moveSpeed;
        float currentSpeed = rb.velocity.x;
        
        // Apply acceleration for smoother movement
        float speedDiff = targetSpeed - currentSpeed;
        float accelRate = acceleration;
        
        // Reduce control in air
        if (!isGrounded)
        {
            accelRate *= airControlFactor;
        }
        
        float movement = speedDiff * accelRate;
        rb.AddForce(movement * Vector3.right, ForceMode.Force);
        
        // Clamp horizontal speed
        Vector3 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = velocity;
    }

    private void HandleJumpInput()
    {
        // Jump buffer - remembers jump input for a short time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Execute jump if conditions are met
        if (jumpBufferCounter > 0)
        {
            // Can jump if grounded or within coyote time, or if jumps remaining
            if (coyoteTimeCounter > 0 || jumpsRemaining > 0)
            {
                Jump();
                jumpBufferCounter = 0;
            }
        }
        
        // Variable jump height - release space early for shorter jump
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
        }
    }

    private void Jump()
    {
        // Reset vertical velocity and apply jump impulse
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        // Decrease jump count
        jumpsRemaining--;
        coyoteTimeCounter = 0;
        
        Debug.Log($"Jump! Jumps remaining: {jumpsRemaining}");
    }

    private void ApplyGravity()
    {
        // Apply additional gravity when falling for better feel
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void LimitFallSpeed()
    {
        // Prevent falling too fast
        if (rb.velocity.y < -maxFallSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxFallSpeed, rb.velocity.z);
        }
    }

    private void UpdateVisualRotation()
    {
        // Rotate visual model based on movement direction
        if (visualModel != null && Mathf.Abs(horizontalInput) > 0.1f)
        {
            float targetRotation = horizontalInput > 0 ? 0 : 180;
            Quaternion target = Quaternion.Euler(0, targetRotation, 0);
            visualModel.rotation = Quaternion.Lerp(visualModel.rotation, target, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize ground check
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundDist);
            Gizmos.DrawWireSphere(groundCheckPoint.position + Vector3.down * groundDist, 0.1f);
        }
    }

    // Public getters for other scripts
    public bool IsGrounded => isGrounded;
    public Vector3 GetVelocity() => rb.velocity;
    public Vector3 GetLastGroundedPosition() => lastGroundedPosition;
}
