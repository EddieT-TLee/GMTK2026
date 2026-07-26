using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stat Variables")]
    [SerializeField] private float MaxHealth;
    [SerializeField] private float MaxHunger;
    [SerializeField] private float MaxHappiness;
    [SerializeField] private float MaxHygiene;
    [SerializeField] private float CurrentHealth;
    [SerializeField] private float CurrentHunger;
    [SerializeField] private float CurrentHappiness;
    [SerializeField] private float CurrentHygiene;

    [Header("Stat Decay Rates")]
    [SerializeField] private float HungerDecayRate;
    [SerializeField] private float HappinessDecayRate;
    [SerializeField] private float HygieneDecayRate;

    [Header("Health Decay Weights")]
    [SerializeField] private float HungerWeight;
    [SerializeField] private float HappinessWeight;
    [SerializeField] private float HygieneWeight;

    public float HealthPercentage => CurrentHealth / MaxHealth;
    public float HungerPercentage => CurrentHunger / MaxHunger;
    public float HappinessPercentage => CurrentHappiness / MaxHappiness;
    public float HygienePercentage => CurrentHygiene / MaxHygiene;

    // Event Listeners for other scripts
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnHungerChanged;
    public event Action<float, float> OnHappinessChanged;
    public event Action<float, float> OnHygieneChanged;
    public event Action OnDeath;

public enum Background { Hospital, Garden, FastFood }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Change the stats to whatever is the start
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnHungerChanged?.Invoke(CurrentHunger, MaxHunger);
        OnHappinessChanged?.Invoke(CurrentHappiness, MaxHappiness);
        OnHygieneChanged?.Invoke(CurrentHygiene, MaxHygiene);
    }

    void Update()
    {
        DecayStats();
    }

    // Decays the Stats of Itchi over time and also updates listners their value has changed
    private void DecayStats()
    {
        CurrentHunger = Mathf.Clamp(CurrentHunger - HungerDecayRate * Time.deltaTime, 0, MaxHunger);
        CurrentHappiness = Mathf.Clamp(CurrentHappiness - HappinessDecayRate * Time.deltaTime, 0, MaxHappiness);
        CurrentHygiene = Mathf.Clamp(CurrentHygiene - HygieneDecayRate * Time.deltaTime, 0, MaxHygiene);

        OnHungerChanged?.Invoke(CurrentHunger, MaxHunger);
        OnHappinessChanged?.Invoke(CurrentHappiness, MaxHappiness);
        OnHygieneChanged?.Invoke(CurrentHygiene, MaxHygiene);

        float healthDecay = MaxHealth *
            HungerWeight * (1 - HungerPercentage) +
            HappinessWeight * (1 - HappinessPercentage) +
            HygieneWeight * (1 - HygienePercentage);

        CurrentHealth = Mathf.Clamp(CurrentHealth - healthDecay * Time.deltaTime, 0, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth == 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void AddHunger(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        CurrentHunger = Mathf.Clamp(CurrentHunger + percentage * MaxHunger, 0, MaxHunger);
        OnHungerChanged?.Invoke(CurrentHunger, MaxHunger);
    }

    public void AddHappiness(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        CurrentHappiness = Mathf.Clamp(CurrentHappiness + percentage * MaxHunger, 0, MaxHappiness);
        OnHappinessChanged?.Invoke(CurrentHappiness, MaxHappiness);
    }

    public void AddHygiene(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        CurrentHygiene = Mathf.Clamp(CurrentHygiene + percentage * MaxHunger, 0, MaxHygiene);
        OnHygieneChanged?.Invoke(CurrentHygiene, MaxHygiene);
    }

    public void ChangeDecayRates(Background bg)
    {
        switch (bg)
        {
            case Background.Hospital:
                HungerDecayRate = 1.5f;
                HappinessDecayRate = 1.5f;
                HygieneDecayRate = 1f;
                break;
            case Background.Garden:
                HungerDecayRate = 1.5f;
                HappinessDecayRate = 1f;
                HygieneDecayRate = 1.5f;
                break;
            case Background.FastFood:
                HungerDecayRate = 1f;
                HappinessDecayRate = 1.5f;
                HygieneDecayRate = 1.5f;
                break;
        }
    }
}