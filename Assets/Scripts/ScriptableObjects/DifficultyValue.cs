using System;
using UnityEngine;
using Utils;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Difficulty Value", menuName = "Variables/Difficulty")]
    public class DifficultyValue : ScriptableObject, ISerializationCallbackReceiver
    {
        public Difficulty initialValue;
        [NonSerialized]
        public Difficulty value;

        public void OnAfterDeserialize()
        {
            value = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}