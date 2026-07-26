using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSuds : MonoBehaviour
{
    public static SpawnSuds Instance { get; private set; }

    [SerializeField] private BoxCollider2D itchi;
    [SerializeField] private GameObject sudsPrefab;
    [SerializeField] private int MinSudsAmount = 10;
    [SerializeField] private int MaxSudsAmount = 40;

    [SerializeField] private Status status;
    [SerializeField] private Animator animator;

    private readonly List<GameObject> activeSuds = new List<GameObject>();

    public event Action sudsCleared;

    private void Awake()
    {
        Instance = this;
    }

    public void AttachSudsToItchi()
    {
        int sudsAmount = Random.Range(MinSudsAmount, MaxSudsAmount);

        for (int i = 0; i < sudsAmount; i++)
        {
            Vector3 sudsPosition = new Vector3(
                Random.Range(itchi.bounds.min.x, itchi.bounds.max.x),
                Random.Range(itchi.bounds.min.y, itchi.bounds.max.y),
                0);
            GameObject suds = Instantiate(sudsPrefab, sudsPosition, Quaternion.identity);
            suds.transform.parent = itchi.transform;

            Suds sudsComponent = suds.GetComponent<Suds>();
            if (sudsComponent != null)
            {
                sudsComponent.Init(this);
            }

            activeSuds.Add(suds);
        }
    }

    public void RemoveSuds(GameObject suds)
    {
        if (!activeSuds.Remove(suds)) return;

        Destroy(suds);

        if (activeSuds.Count == 0)
        {
            if (!status.SatisfyWant(Status.HygieneWant.Sponge))
            {
                animator.Play("HeadShake");
            }
            sudsCleared?.Invoke();
        }
    }
}
