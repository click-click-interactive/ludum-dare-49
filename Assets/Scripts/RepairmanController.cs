using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class RepairmanController : MonoBehaviour
{
    private GameObject _targetRepairStation;
    private bool _isNearStation;
    private NavMeshAgent _navMeshAgent;

    private const string RepairStationsTag = "RepairStation";

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        var random = new Random();
        var repairStations = GameObject.FindGameObjectsWithTag(RepairStationsTag);

        if (repairStations.Length > 0)
        {
            var selected = random.Next(repairStations.Length);
            _targetRepairStation = repairStations[selected];

            _navMeshAgent.destination = _targetRepairStation.transform.position;
        }
        else
        {
            Debug.LogWarning("No repair station found");
        }
    }

    private void Update()
    {
        if (_isNearStation)
        {
            _navMeshAgent.destination = transform.position;
        }
        else
        {
            _navMeshAgent.destination = _targetRepairStation.transform.position;
        }
    }

    public void OnEnterStation()
    {
        Debug.Log("NPC arrived to repair station");
        _isNearStation = true;
    }

    public void OnLeaveStation()
    {
        Debug.Log("Repairman leaves repair station");
        _isNearStation = false;
    }
}