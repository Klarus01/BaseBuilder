using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public bool isInventoryOpen = false;
    public float moveSpeed = 7.0f;
    public float rotationSpeed = 3.0f;

    public float interactionRange = 1.0f;
    public TMP_Text interactText;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = moveSpeed * Time.deltaTime * new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement);

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(0, mouseX, 0);

        IsInteractableObjectInRange();

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInteractText(bool showText)
    {
        interactText.gameObject.SetActive(showText);
    }

    private void IsInteractableObjectInRange()
    {
        bool isAnyInteractableObject = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<InteractableObject>(out _))
            {
                isAnyInteractableObject = true;
            }
        }

        ToggleInteractText(isAnyInteractableObject);
    }

    private void TryInteract()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange)
        .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
        .ToArray();

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<InteractableObject>(out var interactableObject))
            {
                interactableObject.Interact();
                return;
            }/*
            else if (collider.TryGetComponent<Chest>(out var chest))
            {
                InventoryManager.Singleton.isChestOpen = true;
                chest.ToggleChest();
                return;
            }*/
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}