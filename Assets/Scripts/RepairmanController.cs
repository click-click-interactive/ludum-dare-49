using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class RepairmanController : MonoBehaviour
{
    public float speed = 3.0f;
    
    [CanBeNull] private GameObject targetRepairStation;
    private bool isNearStation;
    private Rigidbody2D _rigidbody2D;

    private const string RepairStationsTag = "RepairStation";

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        var random = new Random();
        var repairStations = GameObject.FindGameObjectsWithTag(RepairStationsTag);

        if (repairStations.Length > 0)
        {
            var selected = random.Next(repairStations.Length);
            targetRepairStation = repairStations[selected];

            StartCoroutine(GoToStation());
        }
        else
        {
            Debug.LogWarning("No repair station found");
        }
    }

    private IEnumerator GoToStation()
    {
        var timeElapsed = 0.0f;
        
        while (!isNearStation)
        {
            _rigidbody2D.MovePosition(Vector2.Lerp(transform.position, targetRepairStation.transform.position, timeElapsed / speed));
            timeElapsed = Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnDetected()
    {
        Debug.Log("NPC arrived to repair station");
        isNearStation = true;
    }

    /* private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RepairStation") && !isNearStation)
        {
            Debug.Log("NPC arrived to repair station");
            isNearStation = true;
        }
    }*/

    private void OnDrawGizmos()
    {
        var position = transform.position;
        var zAxis = transform.rotation.eulerAngles.z;

        var direction = new Vector2(-Mathf.Sin(zAxis * Mathf.Deg2Rad),  Mathf.Cos(zAxis * Mathf.Deg2Rad));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(position , position + (Vector3) direction);
        
        Debug.Log(direction);
    }
}