using UnityEngine;
using UnityGPShockwaves;


//<author>Michael Jones</author>
public class ShockWaveHandler : MonoBehaviour
{
    public Shockwave shockWave;

    void Awake()
    {
        if (!enabled) return;

        if (!shockWave)
        {
            Debug.LogError("Missing Shockwave object!");
            enabled = false;
            return;
        }
    }

    void FixedUpdate()
    {
        if (!enabled) return;
        shockWave.FixedUpdate();
    }

    void OnRenderObject()
    {
        if (!enabled) return;
        shockWave.OnRenderObject();
    }

    void OnDestroy()
    {
        shockWave.Reset();
    }
}
