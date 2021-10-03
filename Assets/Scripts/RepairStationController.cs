using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class RepairStationController : MonoBehaviour
{
    private List<GameObject> nearbyRepairmen;
    public Float unstability;
    public GameplayConfig gameplayConfig;

    // Start is called before the first frame update
    void Start()
    {
        nearbyRepairmen = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (unstability.value <= gameplayConfig.maxUnstability && unstability.value >= gameplayConfig.minUnstability)
        {
            unstability.value -= (gameplayConfig.workerFixScore * nearbyRepairmen.Count) * Time.fixedDeltaTime;    
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Repairman"))
        {
            if (!nearbyRepairmen.Contains(other.gameObject))
            {
                var repairman = other.GetComponent<RepairmanController>();
                
                if (repairman != null)
                {
                    repairman.OnEnterStation();
                }
                else
                {
                    Debug.LogWarning("Object doesn't have a RepairmanController script attached !");
                }
                
                nearbyRepairmen.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Repairman"))
        {
            if (nearbyRepairmen.Contains(other.gameObject))
            {
                var repairman = other.GetComponent<RepairmanController>();

                if (repairman)
                {
                    repairman.OnLeaveStation();
                }
                
                if (!nearbyRepairmen.Remove(other.gameObject))
                {
                    Debug.LogWarning("Failed to remove object from repairmen Set");
                }
            }
        }
    }

    public void OnClick()
    {
        Die();
    }

    public void OnOverload()
    {
        Die();
    }

    public void Die()
    {
        for (int i = nearbyRepairmen.Count -1; i >= 0; i--)
        {
            GameObject repairmanToDestroy = nearbyRepairmen[i];
            nearbyRepairmen.Remove(nearbyRepairmen[i]);
            Destroy(repairmanToDestroy);
        }
        Destroy(gameObject);
    }
}