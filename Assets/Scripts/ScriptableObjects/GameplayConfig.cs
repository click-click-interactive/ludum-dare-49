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
    }
}
