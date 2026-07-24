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

    // Event Listeners for other scripts
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnHungerChanged;
    public event Action<float, float> OnHappinessChanged;
    public event Action<float, float> OnHygieneChanged;
    public event Action OnDeath;

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

        float healthDecay =
            HungerWeight * (MaxHunger - CurrentHunger) +
            HappinessWeight * (MaxHappiness - CurrentHappiness) +
            HygieneWeight * (MaxHygiene - CurrentHygiene);

        CurrentHealth = Mathf.Clamp(CurrentHealth - healthDecay * Time.deltaTime, 0, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth == 0)
        {
            OnDeath?.Invoke();
        }
    }
}