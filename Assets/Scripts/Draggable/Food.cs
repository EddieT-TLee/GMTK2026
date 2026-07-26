using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Food : MonoBehaviour
{
    [SerializeField] private Draggable food;

    [SerializeField] private bool apple = false;
    [SerializeField] private bool milk = false;
    [SerializeField] private bool chickenNoodleSoup = false;
    [SerializeField] private bool pills = false;

    public static event Action<Status.HungerWant> foodAte;
    private Rigidbody2D rb;
    private Status status;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;    

        status = GameObject.FindGameObjectWithTag("Itchi").GetComponent<Status>();
    }

    private void OnEnable()
    {
        food.PointerUp += FoodDropped;
        food.PointerDown += FoodPickedUp;
    }

    private void OnDisable()
    {
        food.PointerUp -= FoodDropped;        
        food.PointerDown -= FoodPickedUp;
    }

    private void FoodPickedUp(PointerEventData eventData)
    {
       rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FoodDropped(PointerEventData eventData)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Itchi")
        {
            Destroy(gameObject);
            Status.HungerWant food = Status.HungerWant.Satisfied;

            if (apple)
            {
                food = Status.HungerWant.Apple;
            } else if (milk)
            {
                food = Status.HungerWant.Milk;
            }
            if (chickenNoodleSoup)
            {
                food = Status.HungerWant.ChickenNoodleSoup;
            }
            if (pills)
            {
                food = Status.HungerWant.Pills;
            }

            foodAte?.Invoke(food);
        }
    }
}


