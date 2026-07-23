using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{
    [Header("itchi Refrence for Events")] [SerializeField]
    private Stats itchi;

    [Header("Stat Bars UI Elements")] [SerializeField]
    private Image HappinessBar;

    [SerializeField] private Image HygieneBar;
    [SerializeField] private Image HungerBar;

    void OnEnable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Status Panel");
            return;
        }

        itchi.OnHappinessChanged += UpdateHappinessBar;
        itchi.OnHygieneChanged += UpdateHygieneBar;
        itchi.OnHungerChanged += UpdateHungerBar;
    }

    void OnDisable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Status Panel");
            return;
        }

        itchi.OnHappinessChanged -= UpdateHappinessBar;
        itchi.OnHygieneChanged -= UpdateHygieneBar;
        itchi.OnHungerChanged -= UpdateHungerBar;
    }

    private void UpdateHappinessBar(float current, float max)
    {
        HappinessBar.fillAmount = Mathf.Clamp(current/max, 0, 1);
    }

    private void UpdateHygieneBar(float current, float max)
    {
        HygieneBar.fillAmount = Mathf.Clamp(current/max, 0, 1);
    }
    
    
    private void UpdateHungerBar(float current, float max)
    {
        HungerBar.fillAmount = Mathf.Clamp(current/max, 0, 1);
    }

}