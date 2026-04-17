using UnityEngine;

public class CraftingController : MonoBehaviour
{
    public HotbarController hotbarController;
    public ItemDictionary itemDictionary;
    public Transform playerTransform;

    public int firstItemID = 1;
    public int secondItemID = 2;
    public int resultItemID = 3;

    public Vector2 spawnOffset = new Vector2(1.5f, 0f);

    public void AssembleFromHotbar()
    {
        if (hotbarController == null)
        {
            Debug.LogError("hotbarController is NULL");
            return;
        }

        if (itemDictionary == null)
        {
            Debug.LogError("itemDictionary is NULL");
            return;
        }

        if (hotbarController.hotbarPanel == null)
        {
            Debug.LogError("hotbarPanel is NULL");
            return;
        }

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTransform = playerObj.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogError("playerTransform is NULL");
            return;
        }

        Transform firstSlot = null;
        Transform secondSlot = null;

        foreach (Transform slotTransform in hotbarController.hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem == null)
                continue;

            Item item = slot.currentItem.GetComponent<Item>();
            if (item == null)
                continue;

            if (item.ID == firstItemID && firstSlot == null)
                firstSlot = slotTransform;
            else if (item.ID == secondItemID && secondSlot == null)
                secondSlot = slotTransform;
        }

        if (firstSlot == null || secondSlot == null)
        {
            Debug.LogError("Required items not found in hotbar");
            return;
        }

        Slot firstHotbarSlot = firstSlot.GetComponent<Slot>();
        Slot secondHotbarSlot = secondSlot.GetComponent<Slot>();

        if (firstHotbarSlot == null || secondHotbarSlot == null)
        {
            Debug.LogError("Could not get Slot component from found slots");
            return;
        }

        if (firstHotbarSlot.currentItem != null)
            Destroy(firstHotbarSlot.currentItem);

        if (secondHotbarSlot.currentItem != null)
            Destroy(secondHotbarSlot.currentItem);

        firstHotbarSlot.currentItem = null;
        secondHotbarSlot.currentItem = null;

        GameObject resultPrefab = itemDictionary.GetItemPrefab(resultItemID);

        if (resultPrefab == null)
        {
            Debug.LogError("Result prefab is NULL for ID: " + resultItemID);
            return;
        }

        Vector3 spawnPosition = playerTransform.position + (Vector3)spawnOffset;
        Instantiate(resultPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Assembled successfully and spawned in world");
    }
}