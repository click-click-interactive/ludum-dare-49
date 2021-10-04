using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Gameplay Config", menuName = "Config/Gameplay Config")]
    public class GameplayConfig : ScriptableObject
    {
        public int minUnstability;
        public int maxUnstability = 100;
        public float unstabilityPerSecond;
        public float workerFixScore;
        public float easyThreshold = 25;
        public float mediumThreshold = 50;
        public float hardThreshold = 75;
        public float cryostasisCost = 30;
        public float cryostasisDuration = 5.0f;
        public float overloadCost = 5;
        public float overloadDuration = 5.0f;
        public float doorJamCost = 10;
        public float doorJamDuration = 5.0f;
        public int waveSpawnQuatityEasy = 4;
        public int waveSpawnQuatityMedium = 8;
        public int waveSpawnQuatityHard = 16;
    }
}
