using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private Transform selection;

    void Update()
    {
        if (selection != null)
        {
            if (selection.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject))
            {
                interactableObject.ToggleOutline(false);
                selection = null;
            }
        }


        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            selection = hit.transform;
            if (selection.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject))
            {
                interactableObject.ToggleOutline(true);
            }
        }
    }
}
