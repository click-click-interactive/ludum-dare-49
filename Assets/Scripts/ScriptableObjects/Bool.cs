using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Bool Value", menuName = "Variables/Bool")]
    public class Bool : ScriptableObject, ISerializationCallbackReceiver
    {
        public bool initialValue;
        [NonSerialized]
        public bool value;

        public void OnAfterDeserialize()
        {
            value = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}