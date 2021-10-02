using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void Click(InputAction.CallbackContext context)
    {
        if (context.phase.Equals(InputActionPhase.Performed))
        {   
            Mouse mouse = Mouse.current;
            Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            if (hit.collider != null) {
                if (hit.collider.gameObject.CompareTag("RepairStation"))
                {
                    Debug.Log ("CLICKED " + hit.collider.gameObject.name);

                    RepairStationController repairStationController =
                        hit.collider.gameObject.GetComponent<RepairStationController>();
                    if (repairStationController != null)
                    {
                        repairStationController.Die();
                    }
                }
            }
        }
    }
}
