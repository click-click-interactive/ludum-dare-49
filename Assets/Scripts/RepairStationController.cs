using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Utils;

public class RepairStationController : MonoBehaviour
{
    private List<GameObject> nearbyRepairmen;
    public Float unstability;
    public GameplayConfig gameplayConfig;
    public Bool isCryostasisActive;
    public bool isWorking = true;
    public Material disabledMaterial;
    private Material _originalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        nearbyRepairmen = new List<GameObject>();
        _originalMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isCryostasisActive.value && isWorking)
        {
            if (unstability.value <= gameplayConfig.maxUnstability && unstability.value >= gameplayConfig.minUnstability)
            {
                unstability.value -= (gameplayConfig.workerFixScore * nearbyRepairmen.Count) * Time.fixedDeltaTime;    
            }     
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

    public void OnAction(Skill selectedSkill)
    {
        isWorking = false;
        KillAllWorkers();
        GetComponent<MeshRenderer>().material = disabledMaterial;
        Invoke(nameof(RestartStation), selectedSkill.SkillDuration);
    }

    private void RestartStation()
    {
        isWorking = true;
        GetComponent<MeshRenderer>().material = _originalMaterial;
    }

    public void KillAllWorkers()
    {
        for (int i = nearbyRepairmen.Count -1; i >= 0; i--)
        {
            GameObject repairmanToDestroy = nearbyRepairmen[i];
            nearbyRepairmen.Remove(nearbyRepairmen[i]);
            Destroy(repairmanToDestroy);
        }
    }
}