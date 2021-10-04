using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class SkillPanelController : MonoBehaviour
{
    public GameplayConfig gameplayConfig;
    public TMP_Text overloadButtonText;
    public TMP_Text doorJamButtonText;
    public TMP_Text cryostasisButtonText;

    private void Update()
    {
        overloadButtonText.SetText("{0} unstability", gameplayConfig.overloadCost);
        doorJamButtonText.SetText("{0} unstability", gameplayConfig.doorJamCost);
        cryostasisButtonText.SetText("{0} unstability", gameplayConfig.cryostasisCost);
    }
}