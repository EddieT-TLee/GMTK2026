using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] [Range(0f, 1f)] private float wantThreshold = 0.25f;

    public enum ItchiWant
    {
        Satisfied,
        Comb,
        Sponge,
        Apple,
        ChickenNoodleSoup,
        Milk,
        Pills,
        Ball,
        CarrotOnAStick,
        Tamagotchi
    }

    public enum ItchiStats
    {
        None,
        Hygiene,
        Hunger,
        Happiness,
    }

    public ItchiWant currentWant;

    private static readonly List<ItchiWant> bathes = new() {
        ItchiWant.Comb,
        ItchiWant.Sponge
    };

    private static readonly List<ItchiWant> foods = new() {
        ItchiWant.Apple,
        ItchiWant.ChickenNoodleSoup,
        ItchiWant.Milk,
        ItchiWant.Pills,
    };

    private static readonly List<ItchiWant> games = new() {
        ItchiWant.Ball,
        ItchiWant.CarrotOnAStick,
        ItchiWant.Tamagotchi
    };

    private void OnEnable()
    {
        stats.OnHungerChanged += HandleStatChanged;
        stats.OnHappinessChanged += HandleStatChanged;
        stats.OnHygieneChanged += HandleStatChanged;
    }

    private void OnDisable()
    {
        stats.OnHungerChanged -= HandleStatChanged;
        stats.OnHappinessChanged -= HandleStatChanged;
        stats.OnHygieneChanged -= HandleStatChanged;
    }

    public void SatisfyCurrentWant()
    {
        switch (currentWant)
        {
            case ItchiWant.Satisfied:
                break;

            case ItchiWant.Comb:
            case ItchiWant.Sponge:
                stats.AddHygiene(0.10f);
                break;

            case ItchiWant.Apple:
            case ItchiWant.ChickenNoodleSoup:
            case ItchiWant.Milk:
            case ItchiWant.Pills:
                stats.AddHunger(0.10f);
                break;

            case ItchiWant.Ball:
            case ItchiWant.CarrotOnAStick:
            case ItchiWant.Tamagotchi:
                stats.AddHappiness(0.10f);
                break;
        }
        
        currentWant = ItchiWant.Satisfied;
    }

    private void HandleStatChanged(float current, float max)
    {
        if (currentWant != ItchiWant.Satisfied)
            return;

        ItchiStats neededStat = GetNeededStat();

        if (neededStat != ItchiStats.None)
            GenerateWant(neededStat);
    }

    private void GenerateWant(ItchiStats stat)
    {
        switch (stat)
        {
            case ItchiStats.None:
                break;
            case ItchiStats.Hygiene:
                currentWant = bathes[Random.Range(0, bathes.Count)];
                break;
            case ItchiStats.Hunger:
                currentWant = foods[Random.Range(0, foods.Count)];
                break;
            case ItchiStats.Happiness:
                currentWant = games[Random.Range(0, games.Count)];
                break;
        }
    }

    private ItchiStats GetNeededStat()
    {
        float hunger = stats.HungerPercentage;
        float happiness = stats.HappinessPercentage;
        float hygiene = stats.HygienePercentage;

        float mostWant = Mathf.Min(hunger, happiness, hygiene);

        if (mostWant >= wantThreshold) return ItchiStats.None;

        if (hunger == mostWant)
        {
            return ItchiStats.Hunger;
        }

        if (happiness == mostWant)
        {
            return ItchiStats.Happiness;
        }
        
        return ItchiStats.Hygiene;
    }
}
