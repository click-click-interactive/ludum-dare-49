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
    public Material offMaterial;
    public Material unusedMaterial;
    public Material usedMaterial;
    public GameObject lightBulb;
    public GameObject overloadZoneTooltip;
    public float explosionRadius = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        nearbyRepairmen = new List<GameObject>();
        UpdateLightBulb();
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
        UpdateLightBulb();
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
        UpdateLightBulb();
    }

    public void OnAction(Skill selectedSkill)
    {
        isWorking = false;
        KillAllWorkersInRadius();
        UpdateLightBulb();
        Invoke(nameof(RestartStation), selectedSkill.SkillDuration);
    }

    private void RestartStation()
    {
        isWorking = true;
        UpdateLightBulb();
    }

    private void UpdateLightBulb()
    {
        if (!isWorking)
        {
            lightBulb.GetComponent<MeshRenderer>().material = offMaterial;
        }
        else if (nearbyRepairmen.Count > 0)
        {
            lightBulb.GetComponent<MeshRenderer>().material = usedMaterial;    
        }
        else
        {
            lightBulb.GetComponent<MeshRenderer>().material = unusedMaterial;    
        }
    }

    public void KillAllWorkersInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = colliders.Length - 1; i >= 0; i--)
        {
            Collider col = colliders[i];
            if (nearbyRepairmen.Contains(col.gameObject))
            {
                nearbyRepairmen.Remove(col.gameObject);
            }

            if (col.gameObject.CompareTag("Repairman"))
            {
                Destroy(col.gameObject);    
            }
        }
    }
    
    private void ShowTooltip()
    {
        overloadZoneTooltip.SetActive(true);
    }

    private void HideTooltip()
    {
        overloadZoneTooltip.SetActive(false);
    }
}