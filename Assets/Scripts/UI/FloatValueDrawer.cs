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
        public bool roundValue;
        private TMP_Text _text;
        private string _originalText;

        private void Start()
        {
            _text = GetComponent<TMP_Text>();
            _originalText = _text.text;
        }

        private void Update()
        {
            try
            {
                _text.SetText(_originalText, roundValue ? Mathf.Round(variable.value) : variable.value);
                // _text.text = roundValue ? Mathf.RoundToInt(variable.value).ToString() : variable.value.ToString(CultureInfo.InvariantCulture);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("No float variable assigned to this text (" + name + ")", e);
                throw;
            }
            
        }
    }
}
