using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Draggable))]
public class Brush : MonoBehaviour
{
    [Header("Brush Target")]
    [SerializeField] private string brushZoneTag = "Itchi";

    [Header("Brush Settings")]
    [SerializeField] private float requiredBrushTime = 3f;
    [SerializeField] private float minStrokeDistance = 0.1f;
    [SerializeField] private float reverseDotThreshold = -0.2f;
    [SerializeField] private float brushingWeight = 50f;

    private Draggable draggable;
    private bool isDragging;
    private bool isInZone;
    private bool brushCompleted;

    private Vector3 lastPosition;
    private Vector3 lastDirection;
    private float brushedTime;
    

    private void Awake()
    {
        draggable = GetComponent<Draggable>();
    }

    private void OnEnable()
    {
        draggable.BeginDrag += HandleBeginDrag;
        draggable.Drag += HandleDrag;
        draggable.EndDrag += HandleEndDrag;
    }

    private void OnDisable()
    {
        draggable.BeginDrag -= HandleBeginDrag;
        draggable.Drag -= HandleDrag;
        draggable.EndDrag -= HandleEndDrag;
    }

    private void HandleBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        lastPosition = transform.position;
        lastDirection = Vector3.zero;
    }

    private void HandleDrag(PointerEventData eventData)
    {
        if (brushCompleted || !isDragging || !isInZone) return;

        Vector3 delta = transform.position - lastPosition;
        if (delta.magnitude < minStrokeDistance) return;

        Vector3 direction = delta.normalized;

        // Counts how many times brush changes directions
        if (lastDirection != Vector3.zero && Vector3.Dot(direction, lastDirection) < reverseDotThreshold)
        {
            brushedTime += brushingWeight * Time.deltaTime;
            Debug.Log($"Brushing... {brushedTime:F2} / {requiredBrushTime}s");

            if (brushedTime >= requiredBrushTime)
            {
                Debug.Log("Brush Works");   
                ToyManager.ClearCurrentDraggable(draggable);
                Destroy(gameObject);
                brushCompleted = true;
            }
        }

        lastDirection = direction;
        lastPosition = transform.position;
    }

    private void HandleEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(brushZoneTag)) isInZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(brushZoneTag)) isInZone = false;
    }
}
