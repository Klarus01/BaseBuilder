using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickup : MonoBehaviour
{
    public float PickUpRadius = 2f;
    public Item item;

    private SphereCollider collider;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = PickUpRadius;
    }

    private void OnTriggerStay(Collider other)
    {
        var inventory = other.transform.GetComponent<InventoryHolder>();

        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(item, 1))
        {
            Destroy(this.gameObject);
        }
    }
}
