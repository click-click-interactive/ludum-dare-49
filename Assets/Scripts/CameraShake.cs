using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
	
    // How long the object should shake for.
    public float shakeDuration;
	
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 _originalPos;

    IEnumerator Shake()
    {
        var remainingTime = shakeDuration;
        var mainCamera = Camera.main;

        if (mainCamera)
        {
            _originalPos = mainCamera.transform.localPosition;
    
            while (remainingTime >= 0f)
            {
                mainCamera.transform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;
                
                remainingTime -= Time.deltaTime * decreaseFactor;
    
                yield return null;
            }
        }
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }
}
