using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;
    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>();

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(Item item, int amount)
    {
        if (ContainsItem(item, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                if (slot.RoomLeftInStack(amount))
                {
                    slot.AddToStack(amount);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(item, amount);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public bool ContainsItem(Item item, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(i => i.Item == item).ToList();
        return invSlot == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.Item == null);
        return freeSlot == null ? false : true;
    }
}