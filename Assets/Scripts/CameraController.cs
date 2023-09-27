using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 3.0f;
    private float rotationX = 0.0f;

    public float interactionDistance = 3f;
    public LayerMask interactionLayer;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.parent.Rotate(0, mouseX, 0);

        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -60, 60);

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.TryGetComponent<InteractableObject>(out var interactableObject))
            {
                interactableObject.Interact();
            }
        }
    }
}