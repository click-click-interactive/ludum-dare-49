using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairmanSpawner : MonoBehaviour
{

    public GameObject repairmanPrefab;
    public float spawnRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnRepairman()
    {
        var position = transform.position + Random.insideUnitSphere * spawnRadius;
        Instantiate(repairmanPrefab, position, new Quaternion());
    }

}
