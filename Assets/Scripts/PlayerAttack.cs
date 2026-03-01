using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Hitboxes (attach the components from the child objects)")]
    [SerializeField] private PlayerAttackHitbox hitboxPierce;
    [SerializeField] private PlayerAttackHitbox hitboxDown;
    [SerializeField] private PlayerAttackHitbox hitboxBig;

    [Header("Damage")]
    [SerializeField] private int pierceDamage = 8;
    [SerializeField] private int downDamage = 12;
    [SerializeField] private int bigDamage = 20;

    [Header("Cooldowns (how fast you can attack again)")]
    [SerializeField] private float pierceCooldown = 0.15f;
    [SerializeField] private float downCooldown = 0.25f;
    [SerializeField] private float bigCooldown = 0.45f;

    AudioManager audioManager;

    private bool canAttack = true;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Make sure hitboxes start disabled
        if (hitboxPierce != null) hitboxPierce.SetActive(false);
        if (hitboxDown != null) hitboxDown.SetActive(false);
        if (hitboxBig != null) hitboxBig.SetActive(false);

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (animator == null) return;

        // no attacks if dead
        if (animator.GetBool("deathQueued") || animator.GetBool("isDead"))
            return;

        // keep hitboxes on correct side (left/right)
        UpdateHitboxesSide();

        if (!canAttack) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(AttackRoutine("attackPierce", pierceCooldown));
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(AttackRoutine("attackDown", downCooldown));
            audioManager.PlaySFX(audioManager.playerAttack);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(AttackRoutine("attackBig", bigCooldown));
            audioManager.PlaySFX(audioManager.playerWalk);
        }
    }

    private IEnumerator AttackRoutine(string triggerName, float cooldown)
    {
        canAttack = false;

        // Trigger animation. Hit timing is handled by Animation Events.
        animator.SetTrigger(triggerName);

        // wait for cooldown (you can tweak these in Inspector)
        yield return new WaitForSeconds(cooldown);

        canAttack = true;
    }

    private void UpdateHitboxesSide()
    {
        if (spriteRenderer == null) return;

        bool facingLeft = spriteRenderer.flipX;

        FlipHitboxLocalX(hitboxPierce, facingLeft);
        FlipHitboxLocalX(hitboxDown, facingLeft);
        FlipHitboxLocalX(hitboxBig, facingLeft);
    }

    private void FlipHitboxLocalX(PlayerAttackHitbox hb, bool facingLeft)
    {
        if (hb == null) return;

        Transform t = hb.transform;
        float absX = Mathf.Abs(t.localPosition.x);
        t.localPosition = new Vector3(facingLeft ? -absX : absX, t.localPosition.y, t.localPosition.z);
    }

    // ============================
    // ANIMATION EVENTS (CALL THESE)
    // ============================
    // In the Animation Window, add events that call these methods.

    // PIERCE
    public void EnableHitboxPierce()
    {
        Debug.Log("EVENT: EnableHitboxPierce");
        if (hitboxPierce == null) return;
        hitboxPierce.SetDamage(pierceDamage);
        hitboxPierce.SetActive(true);
    }

    public void DisableHitboxPierce()
    {
        Debug.Log("EVENT: EnableHitboxPierce");
        if (hitboxPierce == null) return;
        hitboxPierce.SetActive(false);
    }

    // SWING DOWN
    public void EnableHitboxDown()
    {
        if (hitboxDown == null) return;
        hitboxDown.SetDamage(downDamage);
        hitboxDown.SetActive(true);
    }

    public void DisableHitboxDown()
    {
        if (hitboxDown == null) return;
        hitboxDown.SetActive(false);
    }

    // BIG SWING
    public void EnableHitboxBig()
    {
        if (hitboxBig == null) return;
        hitboxBig.SetDamage(bigDamage);
        hitboxBig.SetActive(true);
    }

    public void DisableHitboxBig()
    {
        if (hitboxBig == null) return;
        hitboxBig.SetActive(false);
    }
}