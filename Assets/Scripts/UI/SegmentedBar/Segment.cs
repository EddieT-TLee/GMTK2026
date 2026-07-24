using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Segment : MonoBehaviour
{
    [SerializeField]
    private Image image;

    public void SetVisible(bool visible)
    {
        image.enabled = visible;
    }
}
