using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    private Camera cam;
    private Vector3 mouseOffset;

    public event Action<PointerEventData> PointerUp;
    public event Action<PointerEventData> PointerDown;
    public event Action<PointerEventData> BeginDrag;
    public event Action<PointerEventData> Drag;
    public event Action<PointerEventData> EndDrag;
    

    private void Awake()
    {
        cam = Camera.main;
    }

    private Vector3 GetMouseWorldPosition(Vector2 screenPosition)
    {
        float screenZ = cam.WorldToScreenPoint(transform.position).z;
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, screenZ));
        world.z = transform.position.z;

        return world;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUp?.Invoke(eventData);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        mouseOffset = transform.position - GetMouseWorldPosition(eventData.position);

        PointerDown?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPosition(eventData.position) + mouseOffset;

        Drag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag?.Invoke(eventData);
    }

}
