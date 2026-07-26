using UnityEngine;

public class Wander : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    [SerializeField] private float maxWaitTime = 5;
    [SerializeField] private GameObject poopPrefab;
    [SerializeField] private float eatTime = 2f;
    
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
    private bool IsEating;
    private float eatTimer;
    
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

        if (IsEating)
        {
            eatTimer += Time.deltaTime;
            Debug.Log($"eatTimer: {eatTimer} / {eatTime}");
            if (eatTimer > eatTime)
            {
                Debug.Log("Stopped");
                StopEating();
            }

            UpdateAnimator();
            return;
        }
        
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
        Food.foodAte += eatFood;
    }

    void OnDisable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Wander Scirpt");
            return;
        }

        itchi.OnHygieneChanged -= SetHygieneStatus;
        Food.foodAte -= eatFood;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;
        
        // Don't Walk if eating
        if (IsEating)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsEating", IsEating);
            animator.SetBool("IsDirty", dirty);
            return;
        }
        // Walking is true Ithci hasn't reached their target yet
        float deltaX = wanderTarget.x - transform.position.x;
        bool isWalking = Mathf.Abs(deltaX) > 0.01f;

        // Face the direction we're moving 
        if (isWalking)
        {
            transform.rotation = deltaX > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsEating", IsEating);
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

    private void eatFood()
    {
        IsEating = true;
        eatTimer = 0f;
        isChasing = false;
    
        Debug.Log("Started Eating");
    }

    private void StopEating()
    {
        Debug.Log("Stopped Eating");
        IsEating = false;
        eatTimer = 0f;

        PickTargetPosition();
        WalkTimer = 0;
    }
}
