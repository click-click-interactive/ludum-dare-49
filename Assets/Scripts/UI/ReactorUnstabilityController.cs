using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Scrollbar))]
    public class ReactorUnstabilityController : MonoBehaviour
    {
        public Float unstability;
        public GameplayConfig gameplayConfig;
        private Scrollbar _scrollbar;

        private void Start()
        {
            _scrollbar = GetComponent<Scrollbar>();
        }

        private void Update()
        {
            _scrollbar.size = unstability.value / gameplayConfig.maxUnstability;
        }
    }
}