using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private int damage;
    private bool active;

    private readonly HashSet<Collider2D> hitThisActivation = new HashSet<Collider2D>();

    public void SetDamage(int dmg) => damage = dmg;

    public void SetActive(bool value)
    {
        active = value;
        gameObject.SetActive(value);

        if (value)
            hitThisActivation.Clear();
    }

    private void TryHit(Collider2D other)
    {
        if (!active) return;
        if (!other.CompareTag("Enemy")) return;
        if (hitThisActivation.Contains(other)) return;

        // BossHealth sitzt am Root => InParent!
        BossHealth bossHp = other.GetComponentInParent<BossHealth>();
        if (bossHp != null)
        {
            bossHp.TakeDamage(damage);
            hitThisActivation.Add(other);
            return;
        }

        // (Optional) normale Gegner
        EnemyHealth enemyHp = other.GetComponentInParent<EnemyHealth>();
        if (enemyHp != null)
        {
            enemyHp.TakeDamage(damage);
            hitThisActivation.Add(other);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) => TryHit(other);
    private void OnTriggerStay2D(Collider2D other) => TryHit(other);
}