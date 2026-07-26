using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Food : MonoBehaviour
{
    [SerializeField] private Draggable food;

    public static event Action foodAte;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;    
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
            foodAte?.Invoke();
        }
    }
}


