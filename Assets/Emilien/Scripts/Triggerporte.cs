using UnityEngine;
using Invector.vCharacterController; // Assurez-vous d'avoir import� et ajout� le namespace du Character Controller Invector

public class Triggerporte : MonoBehaviour
{
    public Transform teleportDestination; // Destination de t�l�portation

    private vThirdPersonController playerController;
    private Vector3 originalVelocity;

    private void Start()
    {
        playerController = GetComponent<vThirdPersonController>(); // Obtenez le contr�leur de personnage � partir du m�me GameObject que ce script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Bloquer les d�placements du joueur et r�initialiser la vitesse
            playerController.lockMovement = true;
            playerController.lockRotation = true;
            playerController.input = Vector3.zero;

            // Sauvegarder la vitesse originale


            // T�l�porter le joueur � la destination sp�cifi�e
            other.transform.position = teleportDestination.position;

            // R�tablir la libert� de mouvement apr�s un court d�lai
            Invoke(nameof(RestoreMovement), 0.1f);
        }
    }

    private void RestoreMovement()
    {
        // R�tablir la libert� de mouvement du joueur
        playerController.lockMovement = false;
        playerController.lockRotation = false;

        // R�tablir la vitesse originale

    }
}
