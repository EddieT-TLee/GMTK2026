using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> PointerDown;
    public event Action<PointerEventData> BeginDrag;
    public event Action<PointerEventData> Drag;
    public event Action<PointerEventData> EndDrag;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Camera.main != null)
        {
            float screenZ = Camera.main.WorldToScreenPoint(transform.position).z;

            Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, screenZ);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            transform.position = worldPos;
        }

        PointerDown?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Camera.main != null)
        {
            float screenZ = Camera.main.WorldToScreenPoint(transform.position).z;

            Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, screenZ);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            transform.position = worldPos;
        }

        Drag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag?.Invoke(eventData);
    }
}
