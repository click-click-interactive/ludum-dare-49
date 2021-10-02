using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Float Value", menuName = "Variables/Float")]
    public class Float : ScriptableObject, ISerializationCallbackReceiver
    {
        public float initialValue;
        [NonSerialized]
        public float value;

        public void OnAfterDeserialize()
        {
            value = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}