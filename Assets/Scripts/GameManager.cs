using ScriptableObjects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Float unstability;

    public GameplayConfig gameplayConfig;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (unstability.value <= gameplayConfig.maxUnstability && unstability.value >= gameplayConfig.minUnstability)
        {
            unstability.value += gameplayConfig.unstabilityPerSecond * Time.fixedDeltaTime;
        }
        if (unstability.value >= gameplayConfig.maxUnstability)
        {
            onVictory();
        }

        if (unstability.value <= gameplayConfig.minUnstability)
        {
            onDefeat();
        }
        
    }


    void onVictory()
    {
        Debug.Log("KA-BOOM");
        
    }

    void onDefeat()
    {
        Debug.Log("CRISIS AVERTED, YOU LOST");
    }
}
