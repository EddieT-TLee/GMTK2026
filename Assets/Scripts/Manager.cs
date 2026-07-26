using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    public static Manager instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Additive);
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut() => Fade(0f, 1f); // clear to black
    public IEnumerator FadeIn() => Fade(1f, 0f); // black to clear

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        SetAlpha(from);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, elapsed / fadeDuration));
            yield return null;
        }

        SetAlpha(to);
    }

    private void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }

    public void StartFadeIn()
    {
        StartCoroutine(DelayedFadeIn());
    }

    private IEnumerator DelayedFadeIn()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        yield return StartCoroutine(FadeIn());
    }
    
    public IEnumerator ChangeScene(string sceneToLoad, string sceneToUnload)
    {
        yield return StartCoroutine(FadeOut());

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        yield return loadOp;

        Scene newScene = SceneManager.GetSceneByName(sceneToLoad);
        if (newScene.IsValid())
            SceneManager.SetActiveScene(newScene);
        else
            Debug.LogError($"{sceneToLoad} scene not valid after load!");

        yield return SceneManager.UnloadSceneAsync(sceneToUnload);
        
        StartFadeIn();
    }
}
