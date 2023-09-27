using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotClicked(InventorySlotUI clickedSlot)
    {
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        //Slot has an item, but the mouse does not have : split or take
        if (clickedSlot.AssignedInventorySlot.Item != null && mouseInventoryItem.AssignedInventorySlot.Item == null)
        {
            if (isShiftPressed && clickedSlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedSlot.UpdateUISlot();
                return;
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedSlot.AssignedInventorySlot);
                clickedSlot.ClearSlot();
                return;
            }
        }

        //Slot does not have an item, but the mouse does : place item
        if (clickedSlot.AssignedInventorySlot.Item == null && mouseInventoryItem.AssignedInventorySlot.Item != null)
        {
            clickedSlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedSlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        //Slot and mouse have an item
        if (clickedSlot.AssignedInventorySlot.Item != null && mouseInventoryItem.AssignedInventorySlot.Item != null)
        {
            bool isSameItem = clickedSlot.AssignedInventorySlot.Item == mouseInventoryItem.AssignedInventorySlot.Item;

            //Same item and slot have room for all mouse stack size
            if (isSameItem && clickedSlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedSlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedSlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }
            //Same item and slot does not have room for all mouse stack size
            else if (isSameItem && !clickedSlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1)
                {
                    //one of the stacks is full: swap them in place
                    SwapSlots(clickedSlot);
                }
                else
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedSlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedSlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.Item, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            //Not the same item: swap them
            else if (!isSameItem)
            {
                SwapSlots(clickedSlot);
                return;
            }
        }
    }

    private void SwapSlots(InventorySlotUI clickedSlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.Item, mouseInventoryItem.AssignedInventorySlot.StackSize);

        mouseInventoryItem.ClearSlot();
        mouseInventoryItem.UpdateMouseSlot(clickedSlot.AssignedInventorySlot);

        clickedSlot.ClearSlot();
        clickedSlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedSlot.UpdateUISlot();
    }
}
