using UnityEngine;

public class BossHover : MonoBehaviour
{
    [SerializeField] private Animator animator; // Visual Animator
    [SerializeField] private float amplitude = 0.6f;
    [SerializeField] private float speed = 2f;

    private float baseY;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        baseY = transform.position.y;
    }

    public float GetTargetY()
    {
        if (animator != null && animator.GetBool("introDone") == false)
            return baseY;

        if (animator != null && animator.GetBool("isDead") == true)
            return baseY;

        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        return baseY + yOffset;
    }

    public void SetBaseY(float y) => baseY = y;
}