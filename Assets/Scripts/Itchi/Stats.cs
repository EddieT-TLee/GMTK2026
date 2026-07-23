using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Stats : MonoBehaviour
{
    [Header("Stat Variables")] [SerializeField]
    private float MaxHunger;
    [SerializeField] private float MaxHappiness;
    [SerializeField] private float MaxHygiene;
    [SerializeField] private float CurrentHunger;
    [SerializeField] private float CurrentHappiness;
    [SerializeField] private float CurrentHygiene;

    [Header("Stat Decay Rates")] [SerializeField]
    private float HappinessDecayRate;
    [SerializeField] private float HygieneDecayRate;
    [SerializeField] private float HungerDecayRate;
    
    // Event Listeners for other scripts
    public event Action<float, float> OnHungerChanged;
    public event Action<float, float> OnHappinessChanged;
    public event Action<float, float> OnHygieneChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Change the stats to whatever is the start
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
    }
}