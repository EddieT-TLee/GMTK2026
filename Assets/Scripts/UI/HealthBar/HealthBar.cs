using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Animator faceAnimator;
    [SerializeField] private Stats stats;
    [SerializeField] private Image fill;

    private void OnEnable()
    {
        stats.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        stats.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(float current, float max)
    {
        fill.fillAmount = current/max;
        faceAnimator.SetFloat("Health Percentage", current/max, 0.1f, Time.deltaTime);
    }
}
