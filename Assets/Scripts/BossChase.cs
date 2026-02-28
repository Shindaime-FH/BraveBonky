using UnityEngine;

public class BossChase : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator; // Visual Animator
    [SerializeField] private float speed = 2.2f;
    [SerializeField] private float stopDistance = 0.5f;

    private float currentX;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        currentX = transform.position.x;
    }

    public float GetNextX(float rbX, float dt)
    {
        currentX = rbX;

        if (player == null) return currentX;
        if (animator != null && animator.GetBool("introDone") == false) return currentX;
        if (animator != null && animator.GetBool("isDead") == true) return currentX;

        float dx = player.position.x - currentX;
        if (Mathf.Abs(dx) <= stopDistance) return currentX;

        float dir = Mathf.Sign(dx);
        return currentX + dir * speed * dt;
    }
}