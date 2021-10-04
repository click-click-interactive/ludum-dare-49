using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject overloadParticles;

    public void CastOverload(Vector3 position)
    {
        Instantiate(overloadParticles, position, new Quaternion());
    }
}