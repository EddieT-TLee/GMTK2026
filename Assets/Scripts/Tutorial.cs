using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tutorialSprite;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private List<Sprite> nextImages = new();
    [SerializeField] private float fadeDuration = 1.0f;

    private Queue<Sprite> imageStack = new();
    
    private bool changingScenes = false;
    private Coroutine coroutine = null;

    private void Awake()
    {
        foreach (Sprite sprite in nextImages)
        {
            imageStack.Enqueue(sprite);
        }    
    }

    public void FadeToNextSlide()
    {
        

        if (imageStack.Count > 0)
        {
            if (coroutine == null)
            {
                if (imageStack.Count == 1)
                {
                    buttonText.text = "START";
                }
                coroutine = StartCoroutine(FadeTransition());
            }
        }
        else
        {
            if (!changingScenes)
            {
                changingScenes = true;
                Manager.instance.StartCoroutine(Manager.instance.ChangeScene("RealHealthyPatientCare", "Tutorial"));
            }
        }
    }

    private IEnumerator FadeTransition()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            Debug.Log($"Fading:\tAlpha = {tutorialSprite.color.a}\tt = {t}");

            Color temp = tutorialSprite.color;
            temp.a = Mathf.Lerp(temp.a, 0, t);

            tutorialSprite.color = temp;
            yield return null;
        }

        Color temp1 = tutorialSprite.color;
        temp1.a = 0;

        tutorialSprite.color = temp1;

        tutorialSprite.sprite = imageStack.Dequeue();
            
        elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            Color temp = tutorialSprite.color;
            temp.a = Mathf.Lerp(temp.a, 1, t);

            tutorialSprite.color = temp;
            yield return null;
        }

        temp1 = tutorialSprite.color;
        temp1.a = 1;

        tutorialSprite.color = temp1;
        coroutine = null;
    }
}
