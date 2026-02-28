using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 50;
    private int hp;

    private void Awake()
    {
        hp = maxHP;
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log($"{name} took {amount} damage. HP: {hp}");

        if (hp <= 0)
        {
            Debug.Log($"{name} died!");
            Destroy(gameObject);
        }
    }
}