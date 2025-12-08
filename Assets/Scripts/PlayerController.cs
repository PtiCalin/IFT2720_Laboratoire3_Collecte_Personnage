using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Paramètres de Déplacement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 desiredHorizontalVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(moveHorizontal, 0f, moveVertical);
        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }
        desiredHorizontalVelocity = input * moveSpeed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 velocity = new Vector3(desiredHorizontalVelocity.x, currentVelocity.y, desiredHorizontalVelocity.z);
        rb.velocity = velocity;
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
}
