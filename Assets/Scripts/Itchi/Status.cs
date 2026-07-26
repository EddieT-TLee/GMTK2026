using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Reference to Stats component")]
    [SerializeField] private Stats stats;

    [Header("Reference to thought bubbles")]
    [SerializeField] private WantDisplay hungerDisplay;
    [SerializeField] private WantDisplay happinessDisplay;
    [SerializeField] private WantDisplay hygieneDisplay;

    [SerializeField] private List<WantDisplayData> wantSprites;
    
    private Dictionary<string, Sprite> wantSpriteDictionary = new();

    [Header("Want Parameters")]
    [SerializeField] [Range(0f, 1f)] private float wantThreshold = 0.90f;
    [SerializeField] [Range(0f, 1f)] private float wantSatisfyIncrease = 0.1f;
    [SerializeField] private float minWantTime = 0.2f;
    [SerializeField] private float maxWantTime = 1.0f;

    public enum HungerWant
    {
        Satisfied,
        Apple,
        ChickenNoodleSoup,
        Milk,
        Pills
    }

    public enum HappinessWant
    {
        Satisfied,
        Ball,
        CarrotOnAStick,
        Tamagotchi
    }

    public enum HygieneWant
    {
        Satisfied,
        Comb,
        Sponge
    }

    public enum ItchiStats
    {
        None,
        Hunger,
        Happiness,
        Hygiene
    }

    public HungerWant hungerWant;
    public HappinessWant happinessWant;
    public HygieneWant hygieneWant;

    private void Awake()
    {
        foreach (WantDisplayData item in wantSprites)
        {
            if (!wantSpriteDictionary.ContainsKey(item.wantName))
            {
                wantSpriteDictionary.Add(item.wantName.ToLower(), item.sprite);
            }
        }

        StartCoroutine(HandleHungerStat());
        StartCoroutine(HandleHappinessStat());
        StartCoroutine(HandleHygieneStat());
    }

    public bool SatisfyWant(HungerWant want)
    {
        if (hungerWant != want)
            return false;

        stats.AddHunger(wantSatisfyIncrease);
        hungerWant = HungerWant.Satisfied;
        hungerDisplay.Hide();
        return true;
    }

    public bool SatisfyWant(HappinessWant want)
    {
        if (happinessWant != want)
            return false;

        stats.AddHappiness(wantSatisfyIncrease);
        happinessWant = HappinessWant.Satisfied;
        happinessDisplay.Hide();
        return true;
    }

    public bool SatisfyWant(HygieneWant want)
    {
        if (hygieneWant != want)
            return false;

        stats.AddHygiene(wantSatisfyIncrease);
        hygieneWant = HygieneWant.Satisfied;
        hygieneDisplay.Hide();
        return true;
    }

    private T GetRandomWantEnum<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(1, values.Length));
    }

    private static float Map(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) * (max2 - min2) / (max1 - min1) + min2;
    }

    private IEnumerator HandleHungerStat()
    {
        while (true)
        {
            if (stats.HungerPercentage <= wantThreshold && hungerWant == HungerWant.Satisfied)
            {
                float waitTime = Map(stats.HungerPercentage, 0, wantThreshold, minWantTime, maxWantTime);

                yield return new WaitForSeconds(waitTime);
                GenerateWant(ItchiStats.Hunger);
                hungerDisplay.Show();
            }

            yield return null;
        }
    }

    private IEnumerator HandleHappinessStat()
    {
        while (true)
        {
            if (stats.HappinessPercentage <= wantThreshold && happinessWant == HappinessWant.Satisfied)
            {
                float waitTime = Map(stats.HappinessPercentage, 0, wantThreshold, minWantTime, maxWantTime);

                yield return new WaitForSeconds(waitTime);
                GenerateWant(ItchiStats.Happiness);
                happinessDisplay.Show();
            }

            yield return null;
        }
    }

    private IEnumerator HandleHygieneStat()
    {
        while (true)
        {
            if (stats.HygienePercentage <= wantThreshold && hygieneWant != HygieneWant.Comb)
            {
                float waitTime = Map(stats.HygienePercentage, 0, wantThreshold, minWantTime, maxWantTime);

                yield return new WaitForSeconds(waitTime);
                GenerateWant(ItchiStats.Hygiene);
                if (hygieneWant == HygieneWant.Comb)
                {
                    hygieneDisplay.Show();
                }
            }

            yield return null;
        }
    }

    private void GenerateWant(ItchiStats stat)
    {
        switch (stat)
        {
            case ItchiStats.None:
                break;
            case ItchiStats.Hunger:
                hungerWant = GetRandomWantEnum<HungerWant>();
                while (hungerWant == HungerWant.Pills)
                {
                    hungerWant = GetRandomWantEnum<HungerWant>();
                }
                hungerDisplay.ChangeSprite(wantSpriteDictionary[hungerWant.ToString().ToLower()]);
                break;
            case ItchiStats.Happiness:
                happinessWant = GetRandomWantEnum<HappinessWant>();
                happinessDisplay.ChangeSprite(wantSpriteDictionary[happinessWant.ToString().ToLower()]);
                break;
            case ItchiStats.Hygiene:
                hygieneWant = GetRandomWantEnum<HygieneWant>();
                if (hygieneWant == HygieneWant.Comb)
                {
                    hygieneDisplay.ChangeSprite(wantSpriteDictionary["comb"]);
                }
                break;
        }
    }


}

[Serializable]
public struct WantDisplayData
{
    public string wantName;
    public Sprite sprite;
} 