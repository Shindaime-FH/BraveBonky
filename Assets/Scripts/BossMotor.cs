using UnityEngine;

public class BossMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BossChase chase;
    [SerializeField] private BossHover hover;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (chase == null) chase = GetComponent<BossChase>();
        if (hover == null) hover = GetComponent<BossHover>();
    }

    private void FixedUpdate()
    {
        float nextX = rb.position.x;
        float nextY = rb.position.y;

        if (chase != null) nextX = chase.GetNextX(rb.position.x, Time.fixedDeltaTime);
        if (hover != null) nextY = hover.GetTargetY();

        rb.MovePosition(new Vector2(nextX, nextY));
    }
}