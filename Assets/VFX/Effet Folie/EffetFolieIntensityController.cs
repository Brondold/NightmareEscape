using UnityEngine;

public class EffetFolieIntensityController : MonoBehaviour
{
    public float totalTime = 0f;
    public float intensityMultiplier = 1f;
    public Material effetFolieMaterial;

    private void Update()
    {
        totalTime += Time.deltaTime;
        float intensity = totalTime / 180f; // Adapter la plage du timer (ici, 3 minutes) � la plage souhait�e pour l'intensit� (0 � 1)
        intensity *= intensityMultiplier; // Appliquer un multiplicateur d'intensit� si n�cessaire
        effetFolieMaterial.SetFloat("FullScreenIntensity", intensity);
    }
}
