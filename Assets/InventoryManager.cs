using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Singleton;
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemPrefab;

    private int selectedSlot = -1;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 6)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    private void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            InventorySlots[selectedSlot].Deselect();
        }

        InventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                if (itemInSlot.item == item && itemInSlot.count < itemInSlot.maxCount && itemInSlot.item.isStackable)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    return;
                }
            }
        }

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void CheckIfDragFromAnotherInventory(InventoryItem inventoryItem, Transform slotTransform)
    {
        if (transform.childCount == 0)
        {
            foreach (InventorySlot slot in InventorySlots)
            {
                if (slot.transform == inventoryItem.parentAfterDrag)
                {
                    if (ChestManager.Singleton.chestInventory.transform == slotTransform.parent.parent)
                    {
                        ChestManager.Singleton.AddItemToChest(inventoryItem.item);
                    }
                }
            }

            if (ChestManager.Singleton.chestInventory.transform == inventoryItem.parentAfterDrag.parent.parent)
            {
                if (inventoryItem.parentAfterDrag.parent != slotTransform.parent)
                {
                    ChestManager.Singleton.RemoveItemFromChest(inventoryItem.item);
                }
            }
        }
    }
}