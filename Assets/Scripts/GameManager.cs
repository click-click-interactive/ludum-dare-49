using System;
using System.Collections;
using System.Collections.Generic;
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

    public GameObject skillPanel;

    public GameObject unstabilityBar;

    private RepairmanSpawner[] repairmanSpawnerArray;

    public int spawnWaveSizeEasy = 3;
    public int spawnWaveSizeMedium = 6;
    public int spawnWaveSizeHard = 12;

    public float spawnWaveIntervalSeconds = 3f;

    private string state = "idle";

    private IEnumerator spawnRoutine;

    private Boolean isSpawnCoroutineActive;
    
    public DifficultyValue currentDifficulty;

    private InputManager _inputManager;

    public Skill skill = null;

    public Bool isCryostasisActive;
    public Texture2D overloadSkillCursor;

    public Texture2D doorJamSkillCursor;

    public SoundController soundController;

    public AnimationController animationController;

    public CameraShake cameraShakeBehavior;

    [Header("Difficulty levels")]
    public List<RepairStationController> mediumRepairStations;
    public List<RepairStationController> hardRepairStations;

    private bool _hasReachedMedium;
    private bool _hasReachedHard;
    // Start is called before the first frame update
    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        skillPanel.SetActive(false);
        unstabilityBar.SetActive(false);
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
            currentDifficulty.value = Difficulty.Basic;
        }
        else if (Predicate.Between(unstability.value, gameplayConfig.easyThreshold, gameplayConfig.mediumThreshold))
        {
            currentDifficulty.value = Difficulty.Easy;
        }
        else if (Predicate.Between(unstability.value, gameplayConfig.mediumThreshold, gameplayConfig.hardThreshold))
        {
            currentDifficulty.value = Difficulty.Medium;

            if (!_hasReachedMedium)
            {
                foreach (var station in mediumRepairStations)
                {
                    station.isWorking = true;
                }
            }
        }
        else if (Predicate.Between(unstability.value, gameplayConfig.hardThreshold, gameplayConfig.maxUnstability))
        {
            currentDifficulty.value = Difficulty.Hard;
            
            if (!_hasReachedHard)
            {
                foreach (var station in hardRepairStations)
                {
                    station.isWorking = true;
                }
            }
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
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        state = "idle";
        _inputManager.ToggleActionMap(InputType.UI);
        isSpawnCoroutineActive = false;
        skillPanel.SetActive(false);
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
        unstabilityBar.SetActive(true);
        skillPanel.SetActive(true);
    }

    public void OnOverload(GameObject target)
    {
        unstability.value -= gameplayConfig.overloadCost;
        cameraShakeBehavior.ShakeCamera();
        animationController.CastOverload(target.transform.position);
        soundController.OnOverload();
    }

    public void OnDoorJam(GameObject target)
    {
        unstability.value -= gameplayConfig.doorJamCost;
    }

    public void OnOverloadClick()
    {
        if (unstability.value >= gameplayConfig.overloadCost)
        {
            Cursor.SetCursor(overloadSkillCursor, Vector2.zero, CursorMode.Auto);
            Debug.Log("Electrical Overload selected");
            skill = new Skill("Overload", "RepairStation", gameplayConfig.overloadCost, gameplayConfig.overloadDuration);
        }
    }

    public void OnDoorJamClick()
    {
        if (unstability.value >= gameplayConfig.doorJamCost)
        {
            Cursor.SetCursor(doorJamSkillCursor, Vector2.zero, CursorMode.Auto);
            Debug.Log("Door Jam selected");
            skill = new Skill("DoorJam", "Spawner", gameplayConfig.doorJamCost, gameplayConfig.doorJamDuration);    
        }
    }

    public void OnCryostasisClick()
    {
        if (unstability.value >= gameplayConfig.cryostasisCost && !isCryostasisActive.value)
        {
            Debug.Log("Cryostasis activated");
            unstability.value -= gameplayConfig.cryostasisCost;
            isCryostasisActive.value = true;
            soundController.OnCryostasis();
            
            Invoke(nameof(disableCryostasis), gameplayConfig.cryostasisDuration);
            skill = null;
        }
        else
        {
            Debug.Log("Could not cast cryostasis");
        }
        
    }

    IEnumerator spawnWave()
    {
        while (state == "play")
        {
            
            foreach (RepairmanSpawner repairmanSpawner in repairmanSpawnerArray)
            {
                int waveSize = spawnWaveSizeEasy;
                if (currentDifficulty.value == Difficulty.Medium)
                {
                    waveSize = spawnWaveSizeMedium;
                } else if (currentDifficulty.value == Difficulty.Hard)
                {
                    waveSize = spawnWaveSizeHard;
                }
                for (int i = 0; i < waveSize; i++)
                {
                    repairmanSpawner.SpawnRepairman();
                }
            }
            yield return new WaitForSeconds(spawnWaveIntervalSeconds);
        }
    }

    private void disableCryostasis()
    {
        Debug.Log("Cryostasis ended");
        isCryostasisActive.value = false;
    }
}
