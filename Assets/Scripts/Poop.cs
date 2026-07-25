using System;
using UnityEngine;

public class Poop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Mop")
        {
            Destroy(gameObject);
        }
    }
}
