using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (originalSlot == null)
        {
            Debug.LogError("Original parent has no Slot component!");
        }

        if (dropSlot != null)
        {
                if (dropSlot.currentItem != null)
                {
                    dropSlot.currentItem.transform.SetParent(originalParent.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }

                else
                {
                    originalSlot.currentItem = null;
                }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }    
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);    
            }
            
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slot originalSlot)
{
    originalSlot.currentItem = null;

    //Find player
    Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    if (playerTransform == null)
    {
        Debug.LogError("Missing 'Player' tag");
        return;
    }

    //Random drop position
    Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
    Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

    //Instantiate drop item
    Instantiate(gameObject, dropPosition, Quaternion.identity);

    //Destroy the UI one
    Destroy(gameObject);
}

}
