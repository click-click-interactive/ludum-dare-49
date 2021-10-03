using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Float unstability;

    public GameplayConfig gameplayConfig;

    public GameObject startPanel;

    public GameObject victoryPanel;

    public GameObject defeatPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        unstability.value = unstability.initialValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateUnstability();
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
        Debug.Log("KA-BOOM");
        victoryPanel.SetActive(true);
    }
    
    void OnDefeat()
    {
        Debug.Log("CRISIS AVERTED, YOU LOST");
        defeatPanel.SetActive(true);
    }
    
    public void OnRestartClick()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
