using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Segment : MonoBehaviour
{
    [SerializeField]
    private Image image;

    public void SetVisible(bool visible)
    {
        Color temp = image.color;
        temp.a = visible ? 1f : 0f;

        image.color = temp;
    }
}
