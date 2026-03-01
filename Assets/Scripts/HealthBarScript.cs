using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private BossHealth boss;
    [SerializeField] private Slider slider;

    private void OnEnable()
    {
        boss.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        boss.OnHealthChanged -= UpdateHealthBar;
    }

    void UpdateHealthBar(int currentHP, int maxHP)
    {
        slider.maxValue = maxHP;
        slider.value = currentHP;
    }
}
