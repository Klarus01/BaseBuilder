using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Outline outline;
    public Item item;

    private void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 4f;
        outline.enabled = false;
    }

    private void OnMouseEnter()
    {
        ToggleOutline(true);
    }

    private void OnMouseExit()
    {
        ToggleOutline(false);
    }

    public void Interact()
    {
        //InventoryManager.Singleton.AddItem(item);
        Debug.Log("Collected: " + gameObject.name);
        Destroy(gameObject);
    }

    public void ToggleOutline(bool interactable)
    {
        outline.enabled = interactable;
    }
}