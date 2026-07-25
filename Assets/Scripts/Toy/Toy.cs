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
            currentDraggable = Instantiate(draggablePrefab);

            ToyManager.ToySelected(this);
        }
    }

    public void DespawnDraggable()
    {
        if (currentDraggable != null)
        {
            Destroy(currentDraggable.gameObject);
        }
    }


}
