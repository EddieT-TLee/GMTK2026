using UnityEngine;

public class Wander : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    [SerializeField] private float maxWaitTime = 5;
    
    [Header("Itchi Refrence for Events")] [SerializeField]
    private Stats itchi;

    [Header("Itchi Needs Indicator")]
    [SerializeField] private GameObject hungerIndicator;
    [SerializeField] private GameObject happinessIndicator;

    private Vector3 wanderTarget;
    private float WalkTimer;
    private float WalkTimeInterval;
    private Camera cam;
    private bool dirty;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        cam = Camera.main;
        PickTargetPosition();
        WalkTimeInterval = Random.Range(3, maxWaitTime);
        
        // Set Indicator inactive by default
        hungerIndicator.SetActive(false);
        happinessIndicator.SetActive(false);
    }

    void Update()
    {
        // Choosing a random position to wander to
        transform.position = Vector3.MoveTowards(transform.position, wanderTarget, speed * Time.deltaTime);

        if (WalkTimer > WalkTimeInterval)
        {
            WalkTimeInterval = Random.Range(3, maxWaitTime);
            PickTargetPosition();
            WalkTimer = 0;
        }

        WalkTimer += Time.deltaTime;

    }
    
    
    void OnEnable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Wander Scirpt");
            return;
        }

        itchi.OnHappinessChanged += SetHappinessIndicator;
        itchi.OnHungerChanged += SetHungerIndicator;
        itchi.OnHygieneChanged += SetHygieneStatus;
    }

    void OnDisable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Wander Scirpt");
            return;
        }

        itchi.OnHappinessChanged -= SetHappinessIndicator;
        itchi.OnHungerChanged -= SetHungerIndicator;
        itchi.OnHygieneChanged -= SetHygieneStatus;
    }
    
    private void PickTargetPosition()
    {
        float randomX = Random.Range(-0.9f, 0.9f);

        transform.rotation = randomX > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

        // Change the positon to a world position
        float halfWidth = cam.orthographicSize * cam.aspect;

        float worldX = cam.transform.position.x + randomX * halfWidth;

        wanderTarget = new Vector3(worldX, transform.position.y, transform.position.z);
        // Debug.Log(wanderTarget.x);
        // Debug.Log(randomX);
    }
    
    private void SetHappinessIndicator(float current, float max)
    {
        happinessIndicator.SetActive(current / max < 0.25);
    }


    private void SetHungerIndicator(float current, float max)
    {
        hungerIndicator.SetActive(current / max < 0.25);
    }

    private void SetHygieneStatus(float current, float max)
    {
        dirty = (current / max < 0.25);
    }
    
    
}
