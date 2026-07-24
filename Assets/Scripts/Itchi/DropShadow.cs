using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{
    public Vector2 ShadowOffset = new Vector2(-0.1f, -0.1f);
    public Material ShadowMaterial;

    GameObject shadowGameobject;

    void Start()
    {

        //create a new gameobject to be used as drop shadow
        shadowGameobject = new GameObject("Shadow");
        shadowGameobject.transform.parent = transform;
        
        shadowGameobject.transform.localPosition = ShadowOffset;
        shadowGameobject.transform.localRotation = Quaternion.identity;

        SpriteRenderer render = GetComponent <SpriteRenderer>();
        SpriteRenderer shadowSpriteRendererShadow = shadowGameobject.AddComponent<SpriteRenderer>();   
        shadowSpriteRendererShadow.sprite = render.sprite;
        shadowSpriteRendererShadow.material = ShadowMaterial;
        
        shadowSpriteRendererShadow.sortingLayerName = "Shadow";
        shadowSpriteRendererShadow.sortingOrder = render.sortingOrder - 1;
    }

    void LateUpdate()
    {
        shadowGameobject.transform.localPosition = ShadowOffset;
    }
}
