using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Paramètres de Déplacement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
        private Rigidbody rb;
        private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed;
        transform.Translate(move * Time.deltaTime, 0, 0);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
}
