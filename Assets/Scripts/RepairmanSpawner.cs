using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class RepairmanSpawner : MonoBehaviour
{

    public GameObject repairmanPrefab;
    public float spawnRadius;

    public Bool isCryostasisActive;

    public bool isWorking = true;

    public Material disabledMaterial;
    private Material _originalMaterial;

    public GameObject leftDoorHinge;
    public GameObject rightDoorHinge;
    public GameObject spawnLocation;

    private List<GameObject> workersInLobby = new List<GameObject>();

    public GameObject doorJamTooltip;

    [Header("Wave Spawn")]
    [Tooltip("Delay in seconds before a wave spawn occurs again. Prevents spam if difficulty changes a lot in a short amount of time")]
    public float waveSpawnCooldown = 2f;
    [Tooltip("Delay between each spawn in a wave, in seconds")]
    public float waveSpawnDelay = 0.1f;
    public DifficultyValue currentDifficulty;
    private Difficulty _previousDifficulty = Difficulty.Basic;
    private bool _waveSpawnLocked;
    private Animator _leftHingeAnimator;
    private Animator _rightHingeAnimator;
    

    [Header("Game Config")] public GameplayConfig gameplayConfig;

    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    // Start is called before the first frame update
    void Start()
    {
        _leftHingeAnimator = leftDoorHinge.GetComponent<Animator>();
        _rightHingeAnimator = rightDoorHinge.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ComputeWaveSpawn();
    }

    private void ComputeWaveSpawn()
    {
        if (currentDifficulty.value != _previousDifficulty)
        {
            _previousDifficulty = currentDifficulty.value;

            if (!_waveSpawnLocked)
            {
                // wave spawn logic
                StartCoroutine(LockWaveSpawn());
                StartCoroutine(WaveSpawn(currentDifficulty.value));
            }
        }
    }

    private IEnumerator WaveSpawn(Difficulty difficulty)
    {
        var spawnQuantity = difficulty switch
        {
            Difficulty.Basic => 0,
            Difficulty.Easy => gameplayConfig.waveSpawnQuatityEasy,
            Difficulty.Medium => gameplayConfig.waveSpawnQuatityMedium,
            Difficulty.Hard => gameplayConfig.waveSpawnQuatityHard,
            _ => 0
        };
        
        for (var i = 0; i < spawnQuantity; i++)
        {
            SpawnRepairman();
            yield return new WaitForSeconds(waveSpawnDelay);
        }
    }

    private IEnumerator LockWaveSpawn()
    {
        _waveSpawnLocked = true;

        yield return new WaitForSeconds(waveSpawnCooldown);

        _waveSpawnLocked = false;
    }

    public void SpawnRepairman()
    {
        if (!isCryostasisActive.value)
        {
            var position = spawnLocation.transform.position;
            var randomHorizontalPoint = Random.insideUnitCircle * spawnRadius;
            var spawnPosition = new Vector3(position.x + randomHorizontalPoint.x, position.y, position.z + randomHorizontalPoint.y);
            Instantiate(repairmanPrefab, spawnPosition, new Quaternion());    
        }
    }

    public void OnAction(Skill selectedSkill)
    {
        Debug.Log("Door received action");
        isWorking = false;
        _leftHingeAnimator.SetBool(IsOpen, false);
        _rightHingeAnimator.SetBool(IsOpen, false);
        Invoke(nameof(RestartDoor), selectedSkill.SkillDuration);
    }

    private void RestartDoor()
    {
        isWorking = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Repairman"))
        {
            if (!workersInLobby.Contains(other.gameObject))
            {
                workersInLobby.Add(other.gameObject);
            }

            if (isWorking)
            {
                if (_leftHingeAnimator.GetBool(IsOpen) != true)
                {
                    _leftHingeAnimator.SetBool(IsOpen, true);
                }
                if (_rightHingeAnimator.GetBool(IsOpen) != true)
                {
                    _rightHingeAnimator.SetBool(IsOpen, true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Repairman"))
        {
            workersInLobby.Remove(other.gameObject);
        }

        if (workersInLobby.Count == 0)
        {
            _leftHingeAnimator.SetBool(IsOpen, false);
            _rightHingeAnimator.SetBool(IsOpen, false);
        }
    }

    private void ShowTooltip()
    {
        doorJamTooltip.SetActive(true);
    }

    private void HideTooltip()
    {
        doorJamTooltip.SetActive(false);
    }
}
