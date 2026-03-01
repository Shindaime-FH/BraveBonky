using UnityEngine;

public class BossAntiStomp : MonoBehaviour
{
    [SerializeField] private float pushStrength = 2.5f;
    [SerializeField] private float minHeightAboveBoss = 0.2f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Rigidbody2D prb = collision.collider.GetComponentInParent<Rigidbody2D>();
        if (prb == null) return;

        Vector2 bossPos = transform.position;
        Vector2 playerPos = prb.position;

        // Only if the player is clearly above the boss
        if (playerPos.y > bossPos.y + minHeightAboveBoss)
        {
            float dir = Mathf.Sign(playerPos.x - bossPos.x);
            if (dir == 0f) dir = 1f;

            // small sideways + tiny up push so the player can't "rest" on top
            prb.AddForce(new Vector2(dir * pushStrength, 0.5f), ForceMode2D.Impulse);
        }
    }
}