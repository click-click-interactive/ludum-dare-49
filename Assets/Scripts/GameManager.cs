using System;
using System.Collections;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;

[RequireComponent(typeof(PlayerInput))]
public class GameManager : MonoBehaviour
{
    public Float unstability;

    public GameplayConfig gameplayConfig;

    public GameObject startPanel;

    public GameObject victoryPanel;

    public GameObject defeatPanel;

    private RepairmanSpawner[] repairmanSpawnerArray;

    public int spawnWaveSize = 3;

    public float spawnWaveIntervalSeconds = 3f;

    private string state = "idle";

    private IEnumerator spawnRoutine;

    private Boolean isSpawnCoroutineActive;
    
    public Difficulty difficulty;

    private InputManager _inputManager;

    public Bool isCryostasisActive;
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        startPanel.SetActive(true);
        unstability.value = unstability.initialValue;

        repairmanSpawnerArray = GetComponentsInChildren<RepairmanSpawner>();
        isSpawnCoroutineActive = false;
        isCryostasisActive.value = false;
        spawnRoutine = spawnWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == "play")
        {
            if (!isSpawnCoroutineActive)
            {
                StartCoroutine(spawnRoutine);
                isSpawnCoroutineActive = true;
            }

            if (!isCryostasisActive.value)
            {
                UpdateUnstability();    
            }
            
            UpdateDifficulty();
        }
    }

    private void UpdateDifficulty()
    {
        if (Predicate.Between(unstability.value, gameplayConfig.minUnstability, gameplayConfig.easyThreshold))
        {
            difficulty = Difficulty.Basic;
        }
        else if (Predicate.Between(unstability.value, gameplayConfig.easyThreshold, gameplayConfig.mediumThreshold))
        {
            difficulty = Difficulty.Easy;
        }
        else if (Predicate.Between(unstability.value, gameplayConfig.mediumThreshold, gameplayConfig.hardThreshold))
        {
            difficulty = Difficulty.Medium;
        }
        else if (Predicate.Between(unstability.value, gameplayConfig.hardThreshold, gameplayConfig.maxUnstability))
        {
            difficulty = Difficulty.Hard;
        }
    }

    private void UpdateUnstability()
    {
        if (unstability.value <= gameplayConfig.maxUnstability && unstability.value >= gameplayConfig.minUnstability)
        {
            unstability.value += gameplayConfig.unstabilityPerSecond * Time.fixedDeltaTime;
        }
        if (unstability.value >= gameplayConfig.maxUnstability)
        {
            OnVictory();
        }
        if (unstability.value <= gameplayConfig.minUnstability)
        {
            OnDefeat();
        }
    }

    void OnVictory()
    {
        PauseGame();
        
        victoryPanel.SetActive(true);
    }
    
    void OnDefeat()
    {
        PauseGame();
        defeatPanel.SetActive(true);
    }

    private void PauseGame()
    {
        state = "idle";
        _inputManager.ToggleActionMap(InputType.UI);
        isSpawnCoroutineActive = false;
        StopCoroutine(spawnRoutine);
    }
    
    public void OnRestartClick()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void OnPlayClick()
    {
        startPanel.SetActive(false);
        state = "play";
        _inputManager.ToggleActionMap(InputType.Player);
    }

    IEnumerator spawnWave()
    {
        while (state == "play")
        {
            
            foreach (RepairmanSpawner repairmanSpawner in repairmanSpawnerArray)
            {
                for (int i = 0; i < spawnWaveSize; i++)
                {
                    repairmanSpawner.spawnRepairman();
                }
            }
            yield return new WaitForSeconds(spawnWaveIntervalSeconds);
        }
    }

    public void TryCastCryostasis()
    {
        if (unstability.value >= gameplayConfig.cryostasisCost && !isCryostasisActive.value)
        {
            Debug.Log("Cryostasis activated");
            unstability.value -= gameplayConfig.cryostasisCost;
            isCryostasisActive.value = true;
            
            Invoke(nameof(disableCryostasis), gameplayConfig.cryostasisDuration);
        }
        else
        {
            Debug.Log("Could not cast cryostasis");
        }
        
    }

    private void disableCryostasis()
    {
        Debug.Log("Cryostasis ended");
        isCryostasisActive.value = false;
    }
}
