using UnityEngine;
using UnityEngine.UI;

public class Toy : MonoBehaviour
{
    [SerializeField] private Draggable draggablePrefab;
    [SerializeField] private PopupMenu popupMenu;
    [SerializeField] private Button spawnButton;
    
    private Draggable currentDraggable;

    private void Awake()
    {
        ToyManager.toys.Add(this);
        
    }

    private void OnEnable()
    {
        spawnButton.onClick.AddListener(SpawnDraggable);
        spawnButton.onClick.AddListener(popupMenu.ToggleMenu);
    }

    private void OnDisable()
    {
        spawnButton.onClick.RemoveListener(SpawnDraggable);
        spawnButton.onClick.RemoveListener(popupMenu.ToggleMenu);
    }

    public void SpawnDraggable()
    {
        if (currentDraggable == null)
        {
            currentDraggable = Instantiate(draggablePrefab, new Vector3(-3, 3, 0) , Quaternion.identity);
            currentDraggable.GetComponent<SpriteRenderer>().sortingOrder = 99; // LAzy way to render in front of everything
            
            ToyManager.SetCurrentDraggable(currentDraggable);
            
            ToyManager.ToySelected(this);
        }
    }
 
    public void DespawnDraggable()
    {
        if (currentDraggable != null)
        {
            ToyManager.ClearCurrentDraggable(currentDraggable);
            Destroy(currentDraggable.gameObject);
            currentDraggable = null;
        }
    }
 
}
