using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject menu;

    private RectTransform buttonRectTransform;
    private RectTransform menuRectTransform;

    [SerializeField]
    private float offset = 0f;

    [SerializeField]
    private float duration = 1.0f;

    private Vector3 startButtonPosition;
    private Vector3 startMenuPosition;
    private Vector3 endButtonPosition;
    private Vector3 endMenuPosition;

    private Coroutine coroutine = null;
    
    public bool isActive = false;

    private void Start()
    {
        buttonRectTransform = button.gameObject.GetComponent<RectTransform>();
        menuRectTransform = menu.gameObject.GetComponent<RectTransform>();

        var totalHeight = buttonRectTransform.rect.height + menuRectTransform.rect.height + offset;

        startButtonPosition = buttonRectTransform.position;
        startMenuPosition = menuRectTransform.position;

        endButtonPosition = new Vector3(startButtonPosition.x, totalHeight, startButtonPosition.z);
        endMenuPosition = endButtonPosition - new Vector3(0, buttonRectTransform.rect.height / 2 + menuRectTransform.rect.height/2 + offset, 0); 

        PopupMenuManager.popupMenus.Add(this);
        button.onClick.AddListener(ToggleMenu);
        button.onClick.AddListener(BroadcastMenuOpened);
    }

    private void BroadcastMenuOpened()
    {
        PopupMenuManager.MenuOpened(this);
    }

    public void ToggleMenu()
    {
        isActive = !isActive;
        
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        if (isActive)
        {
            coroutine = StartCoroutine(menuUp());
        }
        else
        {
            coroutine = StartCoroutine(menuDown());
        }
    }

    private IEnumerator menuUp()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            buttonRectTransform.position = Vector3.Lerp(buttonRectTransform.position, endButtonPosition, t);
            menuRectTransform.position = Vector3.Lerp(menuRectTransform.position, endMenuPosition, t);

            timeElapsed += Time.deltaTime;
            yield return null;    
        }

        yield break;
    }

    private IEnumerator menuDown()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            buttonRectTransform.position = Vector3.Lerp(buttonRectTransform.position, startButtonPosition, t);
            menuRectTransform.position = Vector3.Lerp(menuRectTransform.position, startMenuPosition, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}