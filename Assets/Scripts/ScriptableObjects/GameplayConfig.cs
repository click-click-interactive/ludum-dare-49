using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Gameplay Config", menuName = "Config/Gameplay Config")]
    public class GameplayConfig : ScriptableObject
    {
        public int minUnstability;
        public int maxUnstability = 100;
        public float unstabilityPerSecond;
        public int workerFixScore;
        public float easyThreshold = 25;
        public float mediumThreshold = 50;
        public float hardThreshold = 75;
        public float cryostasisCost = 30;
        public float cryostasisDuration = 5.0f;
    }
}
