using System.Collections.Generic;
using UnityEngine;

public class BossAttackHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 15;
    [SerializeField] private float knockForce = 7f;

    private bool active;
    private readonly HashSet<Collider2D> hitThisActivation = new HashSet<Collider2D>();

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
        if (!other.CompareTag("Player")) return;
        if (hitThisActivation.Contains(other)) return;

        PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
        if (ph != null)
        {
            Vector2 dir = (other.transform.position - transform.position);
            dir.y = 0f;

            if (dir.sqrMagnitude < 0.001f)
                dir = Vector2.right;

            ph.TakeDamage(damage, dir, knockForce);
            hitThisActivation.Add(other);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) => TryHit(other);
    private void OnTriggerStay2D(Collider2D other) => TryHit(other);
}