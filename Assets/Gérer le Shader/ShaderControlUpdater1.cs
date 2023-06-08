using UnityEngine;

public class ShaderControlUpdater1 : MonoBehaviour
{
    public Timer timer;
    public ShaderControl shaderControl;
    public Material material;

    private void Start()
    {
        shaderControl.Initialize();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        shaderControl.UpdateIntensity(deltaTime);
        float intensity = shaderControl.GetIntensity();
        material.SetFloat("_FullScreenIntensity", intensity); // Utilisez le nom de propriété correct avec le préfixe "_"
    }
}
