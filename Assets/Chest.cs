using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isActivated = false;

    public InventorySlot[] chestInventorySlots;
    public GameObject inventoryItemPrefab;
    public List<Item> items;

    public void ToggleChest()
    {
        isActivated = !isActivated;
        ChestManager.Singleton.ToggleChestInventory(isActivated);

        if (isActivated)
        {
            Cursor.lockState = CursorLockMode.Confined;
            SpawnItemInChest();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            ClearChestInventory();
        }
    }

    public void ClearChestInventory()
    {
        for (int i = 0; i < chestInventorySlots.Length; i++)
        {
            InventorySlot slot = chestInventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
            }
        }
    }

    public void SpawnItemInChest()
    {
        for (int i = 0; i < items.Count; i++)
        {
            InventorySlot slot = chestInventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null && items[i] != null)
            {
                SpawnNewItem(items[i], slot);
            }
        }
    }

    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void MoveItemToInventory(Item item)
    {
        items.Remove(item);
    }

    public void MoveItemToChest(Item item)
    {
        items.Add(item);
    }
}