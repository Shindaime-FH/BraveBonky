using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Animator animator;   // Visual Animator
    [SerializeField] private BossHealth health;

    [Header("Attack timing (phase 1)")]
    [SerializeField] private float minDelay = 0.8f;
    [SerializeField] private float maxDelay = 1.4f;

    [Header("Phase 2 speed up")]
    [SerializeField] private float phase2Multiplier = 0.65f;

    [Header("Attack Hitboxes")]
    [SerializeField] private BossAttackHitbox hitboxDown;
    [SerializeField] private BossAttackHitbox hitboxLeft;
    [SerializeField] private BossAttackHitbox hitboxRight;

    private bool started;
    private float nextActionTime;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (health == null) health = GetComponent<BossHealth>();

        if (hitboxDown == null) hitboxDown = transform.Find("BossHitbox_Down")?.GetComponent<BossAttackHitbox>();
        if (hitboxLeft == null) hitboxLeft = transform.Find("BossHitbox_Left")?.GetComponent<BossAttackHitbox>();
        if (hitboxRight == null) hitboxRight = transform.Find("BossHitbox_Right")?.GetComponent<BossAttackHitbox>();

        // sicherheitshalber aus
        hitboxDown?.SetActive(false);
        hitboxLeft?.SetActive(false);
        hitboxRight?.SetActive(false);
    }

    // Called via AnimationEvent (Relay) at end of intro
    public void OnIntroFinished()
    {
        started = true;
        animator.SetBool("introDone", true);
        animator.SetBool("isInvulnerable", false);
        ScheduleNext();
    }

    private void Update()
    {
        if (!started) return;
        if (health != null && health.IsDead) return;

        if (Time.time >= nextActionTime)
        {
            DoRandomAction();
            ScheduleNext();
        }
    }

    private void ScheduleNext()
    {
        float delay = Random.Range(minDelay, maxDelay);

        if (health != null && health.Phase2Active)
            delay *= phase2Multiplier;

        nextActionTime = Time.time + delay;
    }

    private void DoRandomAction()
    {
        // You can tweak these weights
        int r = Random.Range(0, 100);

        if (r < 30) animator.SetTrigger("block");      // 30% block
        else if (r < 60) animator.SetTrigger("atkLeft");
        else if (r < 85) animator.SetTrigger("atkRight");
        else animator.SetTrigger("atkDown");
    }

    // Called via AnimationEvents (Relay)
    public void IntroStartInvuln() => animator.SetBool("isInvulnerable", true);
    public void IntroEndInvuln() => animator.SetBool("isInvulnerable", false);

    // Animation Events (Attack windows)
    public void DownHitboxOn() => hitboxDown?.SetActive(true);
    public void DownHitboxOff() => hitboxDown?.SetActive(false);

    public void LeftHitboxOn() => hitboxLeft?.SetActive(true);
    public void LeftHitboxOff() => hitboxLeft?.SetActive(false);

    public void RightHitboxOn() => hitboxRight?.SetActive(true);
    public void RightHitboxOff() => hitboxRight?.SetActive(false);

    public void BlockStart()
    {
        animator.SetBool("isInvulnerable", true);
    }

    public void BlockEnd()
    {
        animator.SetBool("isInvulnerable", false);
    }

    public void Phase2Start()
    {
        animator.SetBool("isInvulnerable", true);
        animator.SetBool("phase2Active", true);
    }

    public void Phase2End()
    {
        animator.SetBool("isInvulnerable", false);
    }
}