using System.Collections.Generic;
using UnityEngine;

public class ToyManager : MonoBehaviour
{
    public static List<Toy> toys = new List<Toy>();

    public static void ToySelected(Toy selectedToy)
    {
        foreach (Toy toy in toys)
        {
            if (toy != selectedToy)
            {
                toy.DespawnDraggable();
            }
        }
    }
}
