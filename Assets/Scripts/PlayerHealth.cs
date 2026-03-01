using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private Collider2D playerBodyCollider;
    [SerializeField] private LayerMask bossBodyLayer; // optional, wenn man mit Layer arbeitest
    private Collider2D[] ignoredBossColliders;

    private int hp;

    private Animator animator;
    private DamageFlash flash;
    private Rigidbody2D rb;
    private PlayerMovement movement;

    private bool deathQueued;  // HP=0, warten auf Landung f�r Death-Anim
    private bool isDead;       // Death-Anim gestartet

    private void Awake()
    {
        hp = maxHP;
        animator = GetComponent<Animator>();
        flash = GetComponent<DamageFlash>();
        rb = GetComponent<Rigidbody2D>();
        if (playerBodyCollider == null)
            playerBodyCollider = GetComponent<Collider2D>();
        movement = GetComponent<PlayerMovement>();

        animator.SetBool("isDead", false);
        animator.SetBool("deathQueued", false);
    }

    private void Update()
    {
        // Wenn wir schon "tot" sind, aber noch nicht am Boden -> warten bis grounded
        if (deathQueued && !isDead)
        {
            // Grounded-Flag muss von deinem GroundCheck gesetzt werden
            if (animator.GetBool("isGrounded"))
            {
                StartDeathAnimation();
            }
        }
    }

    /*public void TakeDamage(int amount)
    {
        // Wenn Death bereits gestartet oder queued ist, ignorieren
        if (isDead || deathQueued) return;

        hp -= amount;

        if (hp > 0)
        {
            animator.SetTrigger("hurt");
            flash?.Flash();
            return;
        }

        // HP <= 0 => sofort "gameplay-dead"
        hp = 0;
        deathQueued = true;

        // Sofort Eingaben/Attacks blockieren (�ber Animator)
        animator.SetBool("deathQueued", true);

        // Stoppe Attack-Trigger
        animator.ResetTrigger("hurt");
        animator.ResetTrigger("attackPierce");
        animator.ResetTrigger("attackDown");
        animator.ResetTrigger("attackBig");

        // Optional: horizontal velocity killen, damit du nicht "weiterrutschst"
        if (rb != null)

        {
            rb.gravityScale = 4f;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }


        Debug.Log("PLAYER: Death queued (inputs locked), waiting to land");
    }*/
    public void TakeDamage(int amount, Vector2 knockDir, float knockForce)
    {
        if (isDead || deathQueued) return;

        hp -= amount;

        // ---------- KNOCKBACK ----------
        if (rb != null)
        {
            Vector2 dir = knockDir.normalized;

            // Horizontal stoppen bevor wir Impuls geben
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            // Impuls hinzufügen
            rb.AddForce(new Vector2(dir.x * knockForce, knockForce * 0.1f), ForceMode2D.Impulse);
            movement?.LockMovement(0.15f);
        }

        // ---------- NICHT TÖDLICH ----------
        if (hp > 0)
        {
            animator.SetTrigger("hurt");
            flash?.Flash();
            return;
        }

        // ---------- TÖDLICH ----------
        hp = 0;
        deathQueued = true;

        animator.SetBool("deathQueued", true);

        animator.ResetTrigger("hurt");
        animator.ResetTrigger("attackPierce");
        animator.ResetTrigger("attackDown");
        animator.ResetTrigger("attackBig");

        // Damit du beim Sterben nicht auf dem Boss "liegen bleibst": Boss SOLID colliders ignorieren
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null && playerBodyCollider != null)
        {
            ignoredBossColliders = boss.GetComponentsInChildren<Collider2D>();
            Debug.Log("Death: found boss colliders = " + ignoredBossColliders.Length);

            foreach (var c in ignoredBossColliders)
            {
                if (c == null) continue;
                if (c.isTrigger) continue; // Trigger behalten (TouchDamage etc.)
                Physics2D.IgnoreCollision(playerBodyCollider, c, true);
            }
        }
        else
        {
            Debug.LogWarning("Death: Boss not found or playerBodyCollider missing!");
        }

        // Schnell runterfallen
        if (rb != null)
        {
            rb.gravityScale = 4f; // dein gewünschter Fallboost
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        Debug.Log("PLAYER: Death queued (inputs locked), waiting to land");
    }

    public void TakeDamage(int amount)
    {
        Vector2 defaultDir = Vector2.left;
        TakeDamage(amount, defaultDir, 5f);
    }

    private void StartDeathAnimation()
    {
        isDead = true;
        deathQueued = false;

        // Gravity wieder normal setzen
        if (rb != null)
            rb.gravityScale = 1f;

        if (ignoredBossColliders != null && playerBodyCollider != null)
        {
            foreach (var c in ignoredBossColliders)
            {
                if (c == null) continue;
                if (c.isTrigger) continue;
                Physics2D.IgnoreCollision(playerBodyCollider, c, false);
            }
        }

        animator.SetBool("isDead", true);
        flash?.Flash();

        int playerLayer = gameObject.layer;
        int bossLayer = LayerMask.NameToLayer("BossBody");
        if (bossLayer != -1)
        Physics2D.IgnoreLayerCollision(playerLayer, bossLayer, false);

        Debug.Log("PLAYER: Landed -> Death animation started");
    }

    public int CurrentHP => hp;
}