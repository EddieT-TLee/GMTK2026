using UnityEngine;
using UnityEngine.EventSystems;

public class Ball : MonoBehaviour
{
    [SerializeField] private Draggable draggable;
    private Transform itchiHead;
    private Status status;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private float launchSpeed = 8f;
    [SerializeField] private float bounceSpeed = 6f;
    [SerializeField] private float bounceAngle = 45f;
    [SerializeField] private float rotationSpeed = 1080f;

    [Header("Want to satisfy")]
    [SerializeField] private bool ballWant = false;
    [SerializeField] private bool carrotWant = false;
    [SerializeField] private bool tamagotchiWant = false;

    private enum State
    {
        Idle,
        Launching,
        Bouncing
    }

    private State state = State.Idle;
    private Vector2 moveDirection;
    private float rotationDirection;

    private void Start()
    {
        itchiHead = GameObject.FindGameObjectWithTag("ItchiHead").GetComponent<Transform>();
        status = GameObject.FindGameObjectWithTag("Itchi").GetComponent<Status>();
        animator = status.GetComponent<Animator>();
        draggable.transform.position = transform.position;
        LaunchBall();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Launching:
                MoveTowardsHead();
                break;

            case State.Bouncing:
                BounceAway();
                break;
        }
    }
    
    private void LaunchBall()
    {
        if (state != State.Idle)
            return;

        state = State.Launching;
        draggable.enabled = false;
    }

    private void MoveTowardsHead()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            itchiHead.position,
            launchSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, itchiHead.position) < 0.05f)
        {
            float angle = Random.Range(-bounceAngle, bounceAngle);
            moveDirection = Quaternion.Euler(0, 0, angle) * Vector2.up;

            rotationDirection = -Mathf.Sign(moveDirection.x);
            rotationSpeed *= Mathf.Abs(moveDirection.x);

            state = State.Bouncing;
            animator.Play("Play");

            if (ballWant)
            {
                status.SatisfyWant(Status.HappinessWant.Ball);
                return;
            }
            
            if (carrotWant)
            {
                status.SatisfyWant(Status.HappinessWant.CarrotOnAStick);
                return;
            }

            if (tamagotchiWant)
            {
                status.SatisfyWant(Status.HappinessWant.Tamagotchi);
                return;
            }

        }
    }

    private void BounceAway()
    {
        transform.position += (Vector3)(moveDirection * bounceSpeed * Time.deltaTime);
        
        transform.Rotate(0f, 0f, rotationDirection * rotationSpeed * Time.deltaTime);

        Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position);

        if (viewport.y > 1.1f ||
            viewport.x < -0.1f ||
            viewport.x > 1.1f)
        {
            Destroy(gameObject);
        }
    }
}