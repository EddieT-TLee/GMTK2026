using UnityEngine;

public class HeartMonitor : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip heartMonitorBeep;
    [SerializeField] private AudioClip flatlineTone;

    [SerializeField] private Stats stats;

    private float timeTillNextBeep = 0f;

    private float minTime = 60f/55f;
    private float maxTime = 60f/20f;
    
    private bool flatlining = false;

    private static float Map(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) * (max2 - min2) / (max1 - min1) + min2;
    }

    private void Update()
    {
        if (flatlining) return;

        if (stats.HealthPercentage <= 0 && !flatlining)
        {
            flatlining = true;
            audioSource.Play();
            return;
        }

        timeTillNextBeep -= Time.deltaTime;

        if (timeTillNextBeep <= 0)
        {
            audioSource.PlayOneShot(heartMonitorBeep);
            timeTillNextBeep = Map(stats.HealthPercentage, 0, 1, maxTime, minTime);
        }
    }
}
