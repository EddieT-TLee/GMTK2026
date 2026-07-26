using System;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    [SerializeField] private string sudsTag = "Suds";
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sudsCleaningClip;

    public event Action ScrubbingCompleted;

    private void OnEnable()
    {
        if (SpawnSuds.Instance != null)
        {
            SpawnSuds.Instance.sudsCleared += HandleAllSudsCleared;
        }
    }

    private void OnDisable()
    {
        if (SpawnSuds.Instance != null)
        {
            SpawnSuds.Instance.sudsCleared -= HandleAllSudsCleared;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(sudsTag)) return;

        Suds suds = other.GetComponent<Suds>();
        if (suds != null)
        {
            suds.Scrub();
            TryPlaySudsCleaningSound();
        }
    }

    private void HandleAllSudsCleared()
    {
        Debug.Log("Suds Cleared");
        ScrubbingCompleted?.Invoke();
        Destroy(gameObject);
    }

    private void TryPlaySudsCleaningSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sudsCleaningClip);
        }
    }
}
