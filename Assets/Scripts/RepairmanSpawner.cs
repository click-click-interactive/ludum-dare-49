using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Utils;

public class RepairmanSpawner : MonoBehaviour
{

    public GameObject repairmanPrefab;
    public float spawnRadius;

    public Bool isCryostasisActive;

    public bool isWorking = true;

    public Material disabledMaterial;

    private Material _originalMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _originalMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnRepairman()
    {
        if (!isCryostasisActive.value && isWorking)
        {
            var position = transform.position + Random.insideUnitSphere * spawnRadius;
            Instantiate(repairmanPrefab, position, new Quaternion());    
        }
        
    }

    public void OnAction(Skill selectedSkill)
    {
        isWorking = false;
        GetComponent<MeshRenderer>().material = disabledMaterial;
        Invoke(nameof(RestartDoor), selectedSkill.SkillDuration);
    }

    private void RestartDoor()
    {
        isWorking = true;
        GetComponent<MeshRenderer>().material = _originalMaterial;
    }

}
