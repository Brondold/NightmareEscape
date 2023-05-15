using System.Collections;
using UnityEngine;

public class BlinkingLightOpposed : MonoBehaviour
{
    public Light lightObject;
    public float blinkInterval = 0.5f;
    public float blinkIntervalOn = 0.5f;

    public float minIntensity = 0.0f;
    public float maxIntensity = 1.0f;
    public float transitionDuration = 0.5f;

    private IEnumerator blinkCoroutine;

    private void Start()
    {
        blinkCoroutine = Blink();
        StartCoroutine(blinkCoroutine);
    }

    private IEnumerator Blink()
    {
        while (true)
        {

            float t = 0.0f;
            float currentIntensity = lightObject.intensity;

            while (t < 1.0f)
            {
                t += Time.deltaTime / transitionDuration;
                lightObject.intensity = Mathf.Lerp(currentIntensity, minIntensity, t);
                yield return null;
            }

            yield return new WaitForSeconds(blinkInterval);

            t = 0.0f;
            currentIntensity = lightObject.intensity;

            while (t < 1.0f)
            {
                t += Time.deltaTime / transitionDuration;
                lightObject.intensity = Mathf.Lerp(currentIntensity, maxIntensity, t);
                yield return null;
            }

            yield return new WaitForSeconds(blinkIntervalOn);
        }
    }
}
