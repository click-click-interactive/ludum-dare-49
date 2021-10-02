using System;
using System.Globalization;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class FloatValueDrawer : MonoBehaviour
    {
        public Float variable;
        private TMP_Text _text;

        private void Start()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            try
            {
                _text.text = variable.value.ToString(CultureInfo.InvariantCulture);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("No float variable assigned to this text (" + name + ")", e);
                throw;
            }
            
        }
    }
}
