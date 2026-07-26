using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Animator faceAnimator;
    [SerializeField] private Stats stats;
    [SerializeField] private Image fill;

    //private void OnEnable()
    //{
    //    stats.OnHealthChanged += UpdateHealthUI;
    //}

    //private void OnDisable()
    //{
    //    stats.OnHealthChanged -= UpdateHealthUI;
    //}

    private void Update()
    {
        fill.fillAmount = stats.HealthPercentage;
        faceAnimator.SetFloat("Health Percentage", stats.HealthPercentage, 0f, Time.unscaledDeltaTime);
    }
}
