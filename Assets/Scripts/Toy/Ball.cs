using UnityEngine;
using UnityEngine.EventSystems;

public class Ball : MonoBehaviour
{
    [SerializeField] private Draggable draggable;
    private Transform itchiHead;
    private Status status;

    [Header("Movement")]
    [SerializeField] private float launchSpeed = 8f;
    [SerializeField] private float bounceSpeed = 6f;
    [SerializeField] private float bounceAngle = 45f;
    [SerializeField] private float rotationSpeed = 1080f;

    private enum State
    {
        Idle,
        Launching,
        Bouncing
    }

    private State state = State.Idle;
    private Vector2 moveDirection;
    private float rotationDirection;

    private void OnEnable()
    {
        draggable.PointerUp += LaunchBall;
    }
    private void OnDisable()
    {
        draggable.PointerUp += LaunchBall;
    }

    private void Start()
    {
        itchiHead = GameObject.FindGameObjectWithTag("ItchiHead").GetComponent<Transform>();
        status = GameObject.FindGameObjectWithTag("Itchi").GetComponent<Status>();
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
    
    private void LaunchBall(PointerEventData eventData)
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
            status.SatisfyWant(Status.HappinessWant.Ball);
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