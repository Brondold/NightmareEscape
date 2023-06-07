using UnityEngine;

[CreateAssetMenu(fileName = "ShaderControl", menuName = "ScriptableObjects/Shader Control")]
public class ShaderControl1 : ScriptableObject
{
    public float startIntensity = 0f;
    public float intensityMultiplier = 1f;

    private float currentIntensity;

    public void Initialize()
    {
        currentIntensity = startIntensity;
    }

    public void UpdateIntensity(float deltaTime)
    {
        currentIntensity += deltaTime * intensityMultiplier;
    }

    public float GetIntensity()
    {
        return currentIntensity;
    }
}
