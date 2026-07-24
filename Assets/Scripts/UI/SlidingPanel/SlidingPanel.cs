using System.Collections;
using UnityEngine;

public class SlidingPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private RectTransform moveToTransform;

    private Vector3 startPosition;
    private Vector3 endPosition;

    [SerializeField] private float duration = 1.0f;

    private bool atStart = true;
    private Coroutine coroutine;

    private void Start()
    {
        startPosition = panelTransform.position;
        endPosition = moveToTransform.position;
    }

    public void TogglePanel()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        if (atStart)
        {
            coroutine = StartCoroutine(toEnd());
        }
        else
        {
            coroutine = StartCoroutine(toStart());
        }

        atStart = !atStart;
    }

    private IEnumerator toEnd()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            panelTransform.position = Vector3.Lerp(panelTransform.position, endPosition, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        panelTransform.position = endPosition;
        yield break;
    }

    private IEnumerator toStart()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            panelTransform.position = Vector3.Lerp(panelTransform.position, startPosition, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        panelTransform.position = startPosition;
        yield break;
    }
}
