using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static bool trackingTime = false;
    public static float secondsPassed { get; private set; } = 0f;
    public static bool isPaused { get; private set; } = false;

    private void Update()
    {
        if (trackingTime)
        {
            secondsPassed += Time.deltaTime;
        }
    }

    public static void StartTime()
    {
        trackingTime = true;
    }

    public static void StopTime()
    {
        trackingTime = false;
    }

    public static void ClearTime()
    {
        secondsPassed = 0f;
    }

    public static void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
}
