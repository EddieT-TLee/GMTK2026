using UnityEngine;

public class WantDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer thoughtBubble;
    [SerializeField] private SpriteRenderer want;

    public void Show()
    {
        thoughtBubble.enabled = true;
        want.enabled = true;
    }

    public void Hide()
    {
        thoughtBubble.enabled = false;
        want.enabled = false;
    }

    public void ChangeSprite(Sprite newSprite)
    {
        want.sprite = newSprite;
    }
}
