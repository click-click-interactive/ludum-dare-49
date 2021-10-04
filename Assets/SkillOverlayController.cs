using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class SkillOverlayController : MonoBehaviour
{
    // public float fadeTime = 5f;
    public List<Image> imagesToFade;

    public Bool isCryostasisActive;
    public GameplayConfig gameplayConfig;

    private bool _fadeActive;
    private Color _color;
    // Start is called before the first frame update
    void Start()
    {
        _color = imagesToFade[0].color;
        _color.a = 0f;

        foreach (var image in imagesToFade)
        {
            image.color = _color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCryostasisActive.value && !_fadeActive)
        {
            _fadeActive = true;
            StartCoroutine(FadeOverlay(gameplayConfig.cryostasisDuration));
        }
    }

    private IEnumerator FadeOverlay(float fadeTime)
    {
        _color.a = 1f;

        var timeLeft = fadeTime;
        while (timeLeft >= 0f)
        {
            timeLeft -= Time.deltaTime;
            _color.a = timeLeft / fadeTime;
            foreach (var image in imagesToFade)
            {
                image.color = _color;
            }
            yield return null;
        }

        _color.a = 0;
        _fadeActive = false;
    }
}
