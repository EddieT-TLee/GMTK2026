using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentedBar : MonoBehaviour
{
    [SerializeField] private Transform segmentedBarParent;
    [SerializeField] private Segment segmentPrefab;

    [SerializeField] private int numSegments = 10;
    [SerializeField] private float blinkVisibleDuration = 0.25f;
    [SerializeField] private float blinkHiddenDuration = 0.25f;

    [SerializeField, Range(0f, 1f)] private float amount = 1f;

    private readonly List<Segment> segments = new();
    private readonly List<Segment> blinkingSegments = new();

    private bool blinkVisible = true;

    private void OnValidate()
    {
        amount = Mathf.Clamp01(amount);

        CacheSegments();

        if (segments.Count == numSegments)
            SetFill(amount);
    }

    private void Awake()
    {
        CacheSegments();

        if (segments.Count != numSegments)
            CreateSegments(numSegments);

        StartCoroutine(Blinking());
    }

    private void CacheSegments()
    {
        segments.Clear();

        // Grab existing child segments
        foreach (Transform child in segmentedBarParent)
        {
            if (child.TryGetComponent(out Segment segment))
                segments.Add(segment);
        }
    }

    private void CreateSegments(int count)
    {
        // Destroy existing child objects
        while (segmentedBarParent.childCount > 0)
        {
        #if UNITY_EDITOR
            DestroyImmediate(segmentedBarParent.GetChild(0).gameObject);
        #else
            Destroy(segmentedBarParent.GetChild(0).gameObject);
        #endif
        }

        segments.Clear();

        // Create new segments and attach to parent
        for (int i = 0; i < count; i++)
        {
            Segment segment = Instantiate(segmentPrefab, segmentedBarParent);
            segments.Add(segment);
        }

        SetFill(amount);
    }

    public void RebuildSegments()
    {
        CreateSegments(numSegments);
    }

    /// <summary>
    /// Sets the amount to fill the whole segmented health bar to.
    /// </summary>
    /// <param name="amount">float value between 0 and 1 inclusive</param>
    public void SetFill(float fillAmount)
    {
        amount = Mathf.Clamp01(fillAmount);
        blinkingSegments.Clear();

        float totalFill = amount * segments.Count;

        for (int i = 0; i < segments.Count; i++)
        {
            // Get the percentage that a single segment is filled
            float segmentFill = Mathf.Clamp01(totalFill - i);

            if (segmentFill >= 0.5f)
            {
                segments[i].SetVisible(true);
            } else if (segmentFill > 0f)
            {
                segments[i].SetVisible(blinkVisible);
                blinkingSegments.Add(segments[i]);
            } else
            {
                segments[i].SetVisible(false);
            }
        }
    }

    private IEnumerator Blinking()
    {
        while (true)
        {
            blinkVisible = !blinkVisible;

            foreach (Segment segment in blinkingSegments)
                segment.SetVisible(blinkVisible);

            yield return new WaitForSeconds(blinkVisible ? blinkVisibleDuration : blinkHiddenDuration);
        }
    }
}
