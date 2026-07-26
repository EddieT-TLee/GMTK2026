using System.Collections;
using UnityEngine;

public class Wander : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float maxWaitTime = 5;
    [SerializeField] private GameObject poopPrefab;
    [SerializeField] private float eatTime = 2f;
    
    [Header("Itchi Refrence for Events")]
    [SerializeField] private Stats itchi;
    [SerializeField] private Status status;

    [Header("Thought bubble references to disable upon death")]
    [SerializeField] private WantDisplay hungerDisplay;
    [SerializeField] private WantDisplay happinessDisplay;
    [SerializeField] private WantDisplay hygieneDisplay;

    private Camera cam;
    private Animator animator;

    private Vector3 wanderTarget;
    private float WalkTimer;
    private float WalkTimeInterval;

    private float poopTimer;
    private float poopTimeInterval;

    private float eatTimer;

    private bool gotFood;
    private bool isFoodGood;

    private bool isDirty;
    private bool isDead;
    private bool isChasing;
    private bool isEating;

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
        if (isDead) return;

        if (!isDead && itchi.HealthPercentage <= 0)
        {
            isDead = true;
            StartCoroutine(Death());
            return;
        }

        if (isEating)
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
        Food.foodAte += EatFood;
    }

    void OnDisable()
    {
        if (itchi == null)
        {
            Debug.LogWarning("Itchi was never assigned in Wander Scirpt");
            return;
        }

        itchi.OnHygieneChanged -= SetHygieneStatus;
        Food.foodAte -= EatFood;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;
        
        // Don't Walk if eating
        if (gotFood && isEating)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsEating", true);
            animator.SetBool("IsDirty", isDirty);
            gotFood = false;
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
        animator.SetBool("IsEating", isEating);
        animator.SetBool("IsDirty", isDirty);
    }
    
    private void PickTargetPosition()
    {
        float randomX = Random.Range(-0.6f, 0.6f);

        // Change the positon to a world position
        float halfWidth = cam.orthographicSize * cam.aspect;

        float worldX = cam.transform.position.x + randomX * halfWidth;

        wanderTarget = new Vector3(worldX, transform.position.y, transform.position.z);
        // Debug.Log(wanderTarget.x);
        // Debug.Log(randomX);
    }
    
    private void SetHygieneStatus(float current, float max) => isDirty = (current / max < 0.4);

    private void EatFood(Status.HungerWant foodEaten)
    {
        isEating = true;
        eatTimer = 0f;
        isChasing = false;
        gotFood = true;
        isFoodGood = status.SatisfyWant(foodEaten);

        Debug.Log("Started Eating");
    }

    private void StopEating()
    {
        Debug.Log("Stopped Eating");
        isEating = false;
        eatTimer = 0f;

        StartCoroutine(ProcessFood());
    }

    private IEnumerator Death()
    {
        if (isDirty)
        {
            animator.Play("Death");
        } else
        {
            animator.Play("DirtyDeath");
        }

        hungerDisplay.Hide();
        happinessDisplay.Hide();
        hygieneDisplay.Hide();

        yield return null;

        yield return new WaitUntil(() =>
            (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("DirtyDeath")) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        );

        yield return null;

        PauseManager.Pause();
    }

    private IEnumerator ProcessFood()
    {
        if (!isFoodGood)
        {
            animator.Play("HeadShake");

            yield return null;

            yield return new WaitUntil(() =>
                (animator.GetCurrentAnimatorStateInfo(0).IsName("HeadShake") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("DirtyHeadShake")) &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            );
        }

        PickTargetPosition();
        WalkTimer = 0;
    }
}
