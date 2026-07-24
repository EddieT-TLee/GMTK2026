using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Stats stats;
    [SerializeField] private Image fill;

    private void OnEnable()
    {
        stats.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        stats.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        fill.fillAmount = current/max;
    }
}
