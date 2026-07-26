using System;
using UnityEngine;

public class Poop : MonoBehaviour
{
    private Stats stats;

    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Itchi").GetComponent<Stats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Mop")
        {
            stats.AddHygiene(0.035f);
            Destroy(gameObject);
        }
    }
}
