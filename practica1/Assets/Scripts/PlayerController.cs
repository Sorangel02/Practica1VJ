

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 9.8f;
    public float rayLength = 1.3f;
    public LayerMask groundLayer;

    public float flySpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool isFlying = false;
    private float originalGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Guardar la gravedad original
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        // Activar o desactivar el vuelo con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlying = !isFlying;

            if (isFlying)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = originalGravity;
            }
        }

        float moveInput = Input.GetAxis("Horizontal");

        // Girar el personaje
        if (moveInput > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < -0.1f)
        {
            spriteRenderer.flipX = true;
        }

        // Movimiento durante el vuelo
        if (isFlying)
        {
            float verticalInput = Input.GetAxis("Vertical");

            rb.linearVelocity = new Vector2(
                moveInput * flySpeed,
                verticalInput * flySpeed
            );

            animator.SetBool("EstaVolando", true);
            animator.SetBool("EstaCaminando", false);
            animator.SetBool("EstaSaltando", false);
        }
        else
        {
            // Movimiento normal
            rb.linearVelocity = new Vector2(
                moveInput * speed,
                rb.linearVelocity.y
            );

            isGrounded = IsGrounded();

            // Saltar
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                Jump();
                isGrounded = false;
            }

            // Animaciones normales
            animator.SetBool("EstaVolando", false);

            animator.SetBool(
                "EstaCaminando",
                Mathf.Abs(moveInput) > 0.1f && isGrounded
            );

            animator.SetBool(
                "EstaSaltando",
                !isGrounded
            );
        }
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