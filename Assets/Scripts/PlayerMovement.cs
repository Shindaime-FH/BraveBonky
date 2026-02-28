using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.18f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool isGrounded;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (rb.linearVelocity.x > 0.1f || rb.linearVelocity.x < -0.1f)
        {
            // optional kleine Sperre
        }
        // Block input if dead or deathQueued
        if (animator != null && (animator.GetBool("isDead") || animator.GetBool("deathQueued")))
        {
            moveInput = 0f;
            animator.SetBool("isRunning", false);
            return;
        }

        // Input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Flip
        if (moveInput > 0.01f) spriteRenderer.flipX = false;
        else if (moveInput < -0.01f) spriteRenderer.flipX = true;

        // Animator
        animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        if (animator != null && (animator.GetBool("isDead") || animator.GetBool("deathQueued")))
        {
            // Stop horizontal movement, keep falling
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // APPLY MOVEMENT HERE ? (das hat gefehlt)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}