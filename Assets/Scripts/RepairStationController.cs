using System.Collections.Generic;
using UnityEngine;

public class RepairStationController : MonoBehaviour
{
    private List<GameObject> nearbyRepairmen;

    // Start is called before the first frame update
    void Start()
    {
        nearbyRepairmen = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            if (!nearbyRepairmen.Contains(other.gameObject))
            {
                var repairman = other.GetComponent<RepairmanController>();
                
                if (repairman != null)
                {
                    repairman.OnDetected();
                }
                else
                {
                    Debug.LogWarning("Object doesn't have a RepairmanController script attached !");
                }
                
                nearbyRepairmen.Add(other.gameObject);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    { 
        if (other.gameObject.CompareTag("NPC"))
        {
            if (nearbyRepairmen.Contains(other.gameObject))
            {
                if (!nearbyRepairmen.Remove(other.gameObject))
                {
                    Debug.LogWarning("Failed to remove object from repairmen Set");
                }
            }
        }
    }


    public void Die()
    {
        for (int i = nearbyRepairmen.Count -1; i >= 0; i--)
        {
            GameObject repairmanToDestroy = nearbyRepairmen[i];
            nearbyRepairmen.Remove(nearbyRepairmen[i]);
            Destroy(repairmanToDestroy);
        }
        Destroy(this.gameObject);
    }
}