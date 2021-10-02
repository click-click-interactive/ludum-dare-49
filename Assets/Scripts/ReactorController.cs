using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class ReactorController : MonoBehaviour
{
    public Float unstability;
    public string npcTagFilter = "NPC";
    public GameplayConfig gameplayConfig;

    private bool _isBeingFixed;
    private List<GameObject> detectedNpcs = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateUnstability();
    }

    private void UpdateUnstability()
    {
        _isBeingFixed = detectedNpcs.Count > 0;

        if (!_isBeingFixed)
        {
            unstability.value += gameplayConfig.unstabilityPerSecond * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(npcTagFilter)) return;

        detectedNpcs.Add(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(npcTagFilter)) return;
        
        unstability.value -= gameplayConfig.workerFixScore * Time.fixedDeltaTime;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(npcTagFilter)) return;

        detectedNpcs.Remove(other.gameObject);
    }
}
