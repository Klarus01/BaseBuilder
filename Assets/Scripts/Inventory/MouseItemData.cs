using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    public Image itemSprite;
    public TMP_Text itemCount;
    public InventorySlot AssignedInventorySlot;

    private void Awake()
    {
        itemSprite.color = Color.clear;
        itemCount.SetText(string.Empty);
    }

    private void Update()
    {
        if (AssignedInventorySlot.Item != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        itemCount.SetText(string.Empty);
        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        itemSprite.sprite = invSlot.Item.sprite;
        itemCount.SetText(invSlot.StackSize.ToString());
        itemSprite.color = Color.white;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, result);
        return result.Count > 0;
    }
}