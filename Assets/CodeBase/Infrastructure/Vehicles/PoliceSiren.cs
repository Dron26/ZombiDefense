using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSiren : MonoBehaviour
{
    public List<Light> lights; // Array of lights to be animated
    public float strobeDuration ; // Duration of each strobe
    public int strobeCount ; // Number of times each light will strobe
    private  WaitForSeconds _waitForSeconds; 
    private bool isRunning = true; // Flag to control the strobe effect

    private void Start()
    {
        _waitForSeconds = new(strobeDuration);
        // Enable lights
        foreach (Light light in lights)
        {
            light.enabled = true;
        }

        // Start coroutine for strobing lights
        StartCoroutine(StrobeLights());
    }

    private void OnDestroy()
    {
        // Disable lights before destroying the object
        foreach (Light light in lights)
        {
            light.enabled = false;
        }

        // Stop the strobe coroutine
        isRunning = false;
    }

    private IEnumerator StrobeLights()
    {
        yield return new WaitForSeconds(strobeDuration * strobeCount);
        while (isRunning)
        {
            bool enable = true;
            // Strobe each light individually
            for(int i=0; i < lights.Count; i++)
            {
                StartCoroutine(StrobeLight(lights[i], enable));
                enable = !enable;
            }

            yield return new WaitForSeconds(strobeDuration * strobeCount);
        }
    }

    private IEnumerator StrobeLight(Light light,bool enable)
    {
        bool enabled = enable;
        for (int i = 0; i < strobeCount; i++)
        {
            light.enabled = enabled;
            yield return _waitForSeconds;

            light.enabled = !enabled;
            yield return _waitForSeconds;
        }
    }
}