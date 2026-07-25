using UnityEngine;

public class Suds : MonoBehaviour
{
    private SpawnSuds spawner;

    public void Init(SpawnSuds owningSpawner)
    {
        spawner = owningSpawner;
    }

    public void Scrub()
    {
        if (spawner != null)
        {
            spawner.RemoveSuds(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
