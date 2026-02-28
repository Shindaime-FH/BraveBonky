using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 200;
    [SerializeField] private float phase2Threshold = 0.35f;

    private int hp;
    private bool phase2Triggered;
    private bool dead;

    private Animator animator;     // Visual Animator
    private DamageFlash flash;     // Visual DamageFlash

    public bool IsDead => dead;
    public bool Phase2Active => animator != null && animator.GetBool("phase2Active");

    private void Awake()
    {
        hp = maxHP;
        animator = GetComponentInChildren<Animator>();
        flash = GetComponentInChildren<DamageFlash>();
    }

    public void TakeDamage(int amount)
    {
        if (dead) return;

        if (animator != null && animator.GetBool("isInvulnerable"))
        {
            // NUR wenn du ihn wirklich triffst während er blockt
            if (flash != null) flash.FlashColor(Color.white);
            return;
        }

        hp -= amount;

        if (animator != null)
            animator.SetTrigger("hurt");

        if (flash != null)
            flash.Flash(); // rot

        if (!phase2Triggered && hp <= Mathf.CeilToInt(maxHP * phase2Threshold))
        {
            phase2Triggered = true;
            if (animator != null) animator.SetTrigger("phase2");
        }

        if (hp <= 0)
        {
            hp = 0;
            dead = true;

            if (animator != null)
            {
                animator.SetBool("isDead", true);
                animator.SetTrigger("die");
            }
        }
    }
}