using UnityEngine;
using Invector.vCharacterController; // Assurez-vous d'avoir importé et ajouté le namespace du Character Controller Invector

public class Triggerporte : MonoBehaviour
{
    public Transform teleportDestination; // Destination de téléportation

    private vThirdPersonController playerController;
    private Vector3 originalVelocity;

    private void Start()
    {
        playerController = GetComponent<vThirdPersonController>(); // Obtenez le contrôleur de personnage à partir du même GameObject que ce script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Bloquer les déplacements du joueur et réinitialiser la vitesse
            playerController.lockMovement = true;
            playerController.lockRotation = true;
            playerController.input = Vector3.zero;

            // Sauvegarder la vitesse originale


            // Téléporter le joueur à la destination spécifiée
            other.transform.position = teleportDestination.position;

            // Rétablir la liberté de mouvement après un court délai
            Invoke(nameof(RestoreMovement), 0.1f);
        }
    }

    private void RestoreMovement()
    {
        // Rétablir la liberté de mouvement du joueur
        playerController.lockMovement = false;
        playerController.lockRotation = false;

        // Rétablir la vitesse originale

    }
}
