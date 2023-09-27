using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private Item item;
    [SerializeField] private int stackSize;

    public Item Item => item;
    public int StackSize => stackSize;

    public InventorySlot(Item source, int amount)
    {
        item = source;
        stackSize = amount;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        item = null;
        stackSize = -1;
    }

    public void AssignItem(InventorySlot invSlot)
    {
        if (item == invSlot.item)
        {
            AddToStack(invSlot.stackSize);
        }
        else
        {
            item = invSlot.item;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }

    public void UpdateInventorySlot(Item data, int amount)
    {
        item = data;
        stackSize = amount;
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = Item.maxStrackSize - stackSize;
        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= item.maxStrackSize) return true;
        return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public bool SplitStack(out InventorySlot splitStack)
    {
        if (stackSize <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(item, halfStack);
        return true;
    }
}