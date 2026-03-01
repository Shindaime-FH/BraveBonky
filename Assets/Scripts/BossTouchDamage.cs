using UnityEngine;

/*public class BossTouchDamage : MonoBehaviour
{
    [SerializeField] private int touchDamage = 10;
    [SerializeField] private float cooldown = 0.6f;

    private float nextTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time < nextTime) return;

        // PlayerHealth sitzt meist am Root => InParent
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(touchDamage);
            nextTime = Time.time + cooldown;
        }
    }

    // Optional: zum Testen (kannst du später löschen)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("BossTouchDamage: Player entered");
    }
}*/

public class BossTouchDamage : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockForce = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
        if (ph != null)
        {
            Vector2 dir = (other.transform.position - transform.position);
            dir.y = 0f;

            if (dir.sqrMagnitude < 0.001f)
                dir = Vector2.right;

            ph.TakeDamage(damage, dir, knockForce);
        }
    }
}