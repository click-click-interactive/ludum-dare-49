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

    [Header("Wave Spawn")]
    [Tooltip("Delay in seconds before a wave spawn occurs again. Prevents spam if difficulty changes a lot in a short amount of time")]
    public float waveSpawnCooldown = 2f;
    [Tooltip("Delay between each spawn in a wave, in seconds")]
    public float waveSpawnDelay = 0.1f;
    public DifficultyValue currentDifficulty;
    private Difficulty _previousDifficulty = Difficulty.Basic;
    private bool _waveSpawnLocked;

    [Header("Game Config")] public GameplayConfig gameplayConfig;
    // Start is called before the first frame update
    void Start()
    {
        _originalMaterial = GetComponent<MeshRenderer>().material;
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
                Debug.Log("Spawn wave for " + nameof(currentDifficulty.value) + " difficulty");
                // wave spawn logic
                StartCoroutine(WaveSpawn(currentDifficulty.value));
                StartCoroutine(LockWaveSpawn());
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
        
        Debug.Log("Spawn " + spawnQuantity + " minions in wave");

        for (var i = 0; i < spawnQuantity; i++)
        {
            Debug.Log("Spawn n°" + i);
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
        if (!isCryostasisActive.value && isWorking)
        {
            var position = transform.position;
            var randomHorizontalPoint = Random.insideUnitCircle * spawnRadius;
            var spawnPosition = new Vector3(position.x + randomHorizontalPoint.x, position.y, position.z + randomHorizontalPoint.y);
            Instantiate(repairmanPrefab, spawnPosition, new Quaternion());    
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
