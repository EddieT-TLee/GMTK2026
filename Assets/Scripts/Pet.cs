using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public enum Status
{
    Idle,
    Hungry,
    Unclean,
    Bored,
}

public class Pet : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    [Header("Amount of Time in seconds before changing status to something else")] [SerializeField]
    private float StatusChangeRate;

    private float ChangeStatusTimer;
    private Status status;
    private Vector3 wanderTarget;
    private float WalkTimer;
    private float WalkTimeInterval;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        status = Status.Idle;
        PickTargetPosition();
        WalkTimeInterval = Random.Range(1, 5);
    }

    void LateUpdate()
    {
        // Choosing a random position to wanter to
        transform.position = Vector3.MoveTowards(transform.position, wanderTarget, speed * Time.deltaTime);

        if (WalkTimer > WalkTimeInterval)
        {
            WalkTimeInterval = Random.Range(1, 5);
            PickTargetPosition();
            WalkTimer = 0;
        }

        WalkTimer += Time.deltaTime;
        
        if (Keyboard.current.spaceKey.isPressed) // Debug stuff for now
        {
            status = Status.Idle;
            spriteRenderer.color = Color.white;
        }
        
        // Changing status stuff
        if (status == Status.Idle) ChangeStatusTimer += Time.deltaTime;

        if (status == Status.Idle && ChangeStatusTimer > StatusChangeRate) // Change The status if Idle
        {
            int statusChoosen = Random.Range(1, Enum.GetNames(typeof(Status)).Length);
            status = (Status)statusChoosen;
            ChangeMood(); // Expensive but Just gonna leave it like this for now. Could change to coroutine
            ChangeStatusTimer = 0;
            Debug.Log(status);
        }
    }

    // Will change later to show a thought bubble or something
    // Changes the mood that Tamagotchi with show off once they choose 
    private void ChangeMood()
    {
        switch (status)
        {
            case Status.Idle:
                spriteRenderer.color = Color.white;
                break;
            case Status.Hungry:
                spriteRenderer.color = Color.green;
                break;
            case Status.Unclean:
                spriteRenderer.color = Color.saddleBrown;
                break;
            case Status.Bored:
                spriteRenderer.color = Color.orange;
                break;
        }
    }

    private void PickTargetPosition()
    {
        float randomX = Random.Range(-0.9f, 0.9f);

        // Change the positon to a world position
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;

        float worldX = cam.transform.position.x + randomX * halfWidth;

        wanderTarget = new Vector3(worldX, transform.position.y, transform.position.z);
        Debug.Log(wanderTarget.x);
        Debug.Log(randomX);
    }
}