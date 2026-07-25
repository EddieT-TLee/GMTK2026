using System;
using System.Collections.Generic;
using UnityEngine;

public class ToyManager : MonoBehaviour
{
    public static List<Toy> toys = new List<Toy>();

    // The Draggable that is currently spawned/active in the scene
    public static Draggable CurrentDraggable { get; private set; }

    // Fired when a toy is spawned / despawned so other systems 
    public static event Action<Draggable> DraggableSpawned;
    public static event Action DraggableDespawned;

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

    public static void SetCurrentDraggable(Draggable draggable)
    {
        CurrentDraggable = draggable;
        DraggableSpawned?.Invoke(draggable);
    }

    public static void ClearCurrentDraggable(Draggable draggable)
    {
        if (CurrentDraggable == draggable)
        {
            CurrentDraggable = null;
            DraggableDespawned?.Invoke();
        }
    }
}
