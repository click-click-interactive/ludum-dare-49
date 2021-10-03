using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class RepairmanController : MonoBehaviour
{
    
    public int totalHitPoints = 5;
    public Material hurtMaterial;
    
    private GameObject _targetRepairStation;
    private bool _isNearStation;
    private NavMeshAgent _navMeshAgent;

    private const string RepairStationsTag = "RepairStation";
    private static Random random = new Random();
    private int _hitPoints;
    private Material _originalMaterial;

    private void Start()
    {
        _hitPoints = totalHitPoints;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _originalMaterial = gameObject.GetComponent<MeshRenderer>().material;
        FindTargetRepairStation();
    }

    private void FindTargetRepairStation()
    {
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
        if (_targetRepairStation == null)
        {
            FindTargetRepairStation();
        }
        if (_isNearStation)
        {
            _navMeshAgent.destination = transform.position;
        }
        else
        {
            _navMeshAgent.destination = _targetRepairStation.transform.position;
        }
    }

    public void OnDetected()
    {
        Debug.Log("NPC arrived to repair station");
        _isNearStation = true;
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

    public void OnClick()
    {
        _hitPoints--;
        Debug.Log("Ouch ! " + _hitPoints + "/" + totalHitPoints + " hp");
        gameObject.GetComponent<MeshRenderer>().material = hurtMaterial;
        Invoke(nameof(RestoreMaterial), 0.1f);
        if (_hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void RestoreMaterial()
    {
        gameObject.GetComponent<MeshRenderer>().material = _originalMaterial;
    }
}