using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public float[] angles = { 45f, 90f, 135f }; // Angles des raycasts en degrés
    public float raycastDistance = 5f; // Distance du raycast
    public GameObject canvas;

    void Update()
    {
        foreach (float angle in angles)
        {
            // Convertit l'angle en radians
            float angleRadians = angle * Mathf.Deg2Rad;

            // Calcule la direction du raycast en fonction de l'angle et de la rotation de l'objet
            Vector3 raycastDirection = Quaternion.Euler(0f, angle, 0f) * transform.forward;

            // Crée un raycast à partir de la position et de la direction calculée
            Ray ray = new Ray(transform.position, raycastDirection);

            // Effectue le raycast
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // Affiche un message dans la console de débogage
                    canvas.SetActive(true);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        foreach (float angle in angles)
        {
            // Convertit l'angle en radians
            float angleRadians = angle * Mathf.Deg2Rad;

            // Calcule la direction du raycast en fonction de l'angle et de la rotation de l'objet
            Vector3 raycastDirection = Quaternion.Euler(0f, angle, 0f) * transform.forward;

            // Crée un raycast à partir de la position et de la direction calculée
            Ray ray = new Ray(transform.position, raycastDirection);

            // Dessine le raycast avec des Gizmos
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * raycastDistance);
        }
    }
}
