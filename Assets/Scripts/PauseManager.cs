using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused { get; private set; } = false;

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
