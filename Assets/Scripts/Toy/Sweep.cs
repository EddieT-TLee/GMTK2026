using System.Collections;
using UnityEngine;

public class Sweep : MonoBehaviour
{
    [SerializeField] private GameObject mopPrefab;
    [SerializeField] private Vector2 startPoint; 
    [SerializeField] private Vector2 endPoint;  
    [SerializeField] private float moveSpeed = 8f;

    public void StartSweep()
    {
        StartCoroutine(CleanUpMop());
    }

    private IEnumerator CleanUpMop()
    {
        GameObject spawnedMop = Instantiate(mopPrefab, startPoint, Quaternion.identity);

        // Moving the mop towards end positon
        while (spawnedMop != null && Vector3.Distance(spawnedMop.transform.position, endPoint) > 0.01f)
        {
            spawnedMop.transform.position = Vector3.MoveTowards(
                spawnedMop.transform.position, 
                endPoint, 
                moveSpeed * Time.deltaTime
            );
            yield return null; 
        }

        
        if (spawnedMop != null && spawnedMop.transform.position == (Vector3)endPoint)
        {
            Destroy(spawnedMop);
        }
    }
}
