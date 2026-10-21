

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 9.8f;
    public float rayLength = 1.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(
            moveInput * speed,
            rb.linearVelocity.y
        );

        // Girar el personaje según la dirección
        if (moveInput > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < -0.1f)
        {
            spriteRenderer.flipX = true;
        }

        // Detectar si está tocando el suelo
        isGrounded = IsGrounded();

        // Saltar
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
            isGrounded = false;
        }

        // Controlar las animaciones
        animator.SetBool(
            "EstaCaminando",
            Mathf.Abs(moveInput) > 0.1f && isGrounded
        );

        animator.SetBool(
            "EstaSaltando",
            !isGrounded
        );
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            rayLength,
            groundLayer
        );

        Debug.DrawRay(
            transform.position,
            Vector2.down * rayLength,
            Color.red
        );

        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * rayLength
        );
    }
}
