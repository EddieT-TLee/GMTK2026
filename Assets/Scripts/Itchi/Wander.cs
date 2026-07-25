using UnityEngine;

public class Wander : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    [SerializeField] private float maxWaitTime = 5;
    [SerializeField] private GameObject poopPrefab;
    
    [Header("Itchi Refrence for Events")] [SerializeField]
    private Stats itchi;

    private Vector3 wanderTarget;
    private float WalkTimer;
    private float WalkTimeInterval;
    private Camera cam;
    private bool dirty;
    private Animator animator;
    private bool isChasing;
    private float poopTimer;
    private float poopTimeInterval;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        cam = Camera.main;
        PickTargetPosition();
        WalkTimeInterval = Random.Range(3, maxWaitTime);
        animator = GetComponent<Animator>();

        poopTimeInterval = Random.Range(5, 10);

    }

    void Update()
    {
        Draggable activeDraggable = ToyManager.CurrentDraggable;
        // Will probably add some for things to this for toys
        bool shouldChase = activeDraggable != null && activeDraggable.CompareTag("Food");
 
        if (shouldChase)
        {
            // Keeps chasing the object if it has correct tag
            wanderTarget = new Vector3(activeDraggable.transform.position.x, transform.position.y, transform.position.z);
            isChasing = true;
        }
        else if (isChasing)
        {
            // Food was just picked up/despawned - go back to normal wandering
            isChasing = false;
            PickTargetPosition();
            WalkTimer = 0;
        }
 
        transform.position = Vector3.MoveTowards(transform.position, wanderTarget, speed * Time.deltaTime);
 
        // only wander if not chasing
        if (!isChasing && WalkTimer > WalkTimeInterval)
        {
            WalkTimeInterval = Random.Range(3, maxWaitTime);
            PickTargetPosition();
            WalkTimer = 0;
        }
 
        WalkTimer += Time.deltaTime;
        
        // Poop interval
        if (poopTimer > poopTimeInterval)
        {
            Instantiate(poopPrefab, new Vector3(transform.position.x, -1.3f, 0), transform.rotation);
            poopTimeInterval = Random.Range(5, 10);
            poopTimer = 0;
        }
        
        poopTimer += Time.deltaTime;
        
        UpdateAnimator();
    }
    
    void OnEnable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Wander Scirpt");
            return;
        }

        itchi.OnHygieneChanged += SetHygieneStatus;
    }

    void OnDisable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Wander Scirpt");
            return;
        }

        itchi.OnHygieneChanged -= SetHygieneStatus;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        // Walking is true Ithci hasn't reached their target yet
        float deltaX = wanderTarget.x - transform.position.x;
        bool isWalking = Mathf.Abs(deltaX) > 0.01f;

        // Face the direction we're moving 
        if (isWalking)
        {
            transform.rotation = deltaX > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsDirty", dirty);
    }
    
    private void PickTargetPosition()
    {
        float randomX = Random.Range(-0.9f, 0.9f);

        // Change the positon to a world position
        float halfWidth = cam.orthographicSize * cam.aspect;

        float worldX = cam.transform.position.x + randomX * halfWidth;

        wanderTarget = new Vector3(worldX, transform.position.y, transform.position.z);
        // Debug.Log(wanderTarget.x);
        // Debug.Log(randomX);
    }
    
    private void SetHygieneStatus(float current, float max) => dirty = (current / max < 0.25);
    
    
    
}
