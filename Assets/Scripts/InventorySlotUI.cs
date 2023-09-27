using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TMP_Text itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay parentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClock);

        parentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.Item != null)
        {
            itemSprite.sprite = slot.Item.sprite;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1)
            {
                itemCount.text = slot.StackSize.ToString();
            }
            else
            {
                itemCount.SetText(string.Empty);
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null)
        {
            UpdateUISlot(assignedInventorySlot);
        }
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.SetText(string.Empty);
    }

    public void OnUISlotClock()
    {
        parentDisplay?.SlotClicked(this);
    }
}
