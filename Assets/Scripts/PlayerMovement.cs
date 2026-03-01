using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2;   // 2 = double jump

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
    private bool wasGrounded;

    private int jumpCount;

    // Knockback / hitstun lock (prevents movement overriding knockback)
    private float knockLockTimer;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Call this from PlayerHealth when you apply knockback
    public void LockMovement(float seconds)
    {
        knockLockTimer = Mathf.Max(knockLockTimer, seconds);
    }

    private void Update()
    {
        // timer runterzählen
        if (knockLockTimer > 0f)
            knockLockTimer -= Time.deltaTime;

        // Ground check MUSS immer laufen (auch bei deathQueued), damit Death nach Landung starten kann
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        // JumpCount nur beim echten "Landen" resetten (false -> true)
        if (!wasGrounded && isGrounded)
            jumpCount = 0;

        wasGrounded = isGrounded;

        // Dead / deathQueued -> Input blocken, aber grounded weiter updaten
        if (animator.GetBool("isDead") || animator.GetBool("deathQueued"))
        {
            moveInput = 0f;
            animator.SetBool("isRunning", false);
            return;
        }

        // Knockback lock -> Input blocken
        if (knockLockTimer > 0f)
        {
            moveInput = 0f;
            animator.SetBool("isRunning", false);
            return;
        }

        // Input
        moveInput = Input.GetAxisRaw("Horizontal");

        // DOUBLE JUMP
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }

        // Flip
        if (moveInput > 0.01f) spriteRenderer.flipX = false;
        else if (moveInput < -0.01f) spriteRenderer.flipX = true;

        // Animator
        animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f);
    }

    private void FixedUpdate()
    {
        if (animator.GetBool("isDead") || animator.GetBool("deathQueued"))
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (knockLockTimer > 0f)
            return;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}