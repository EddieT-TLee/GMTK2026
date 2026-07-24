using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{
    [Header("itchi Refrence for Events")]
    [SerializeField] private Stats itchi;

    [Header("Stat Bars UI Elements")]
    [SerializeField] private SegmentedBar HappinessBar;
    [SerializeField] private SegmentedBar HygieneBar;
    [SerializeField] private SegmentedBar HungerBar;

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
        HappinessBar.SetFill(current/max);
    }

    private void UpdateHygieneBar(float current, float max)
    {
        HygieneBar.SetFill(current / max);
    }
    
    private void UpdateHungerBar(float current, float max)
    {
        HungerBar.SetFill(current / max);
    }

}