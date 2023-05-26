using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform teleportTarget; // L'endroit TP1
    public Transform teleportTarget1;
    public float ScoreTP = 0;

    private CharacterController characterController;
    private bool isTeleporting;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportationSurface")) // V�rifie si le personnage entre en contact avec la surface de t�l�portation
        {
            isTeleporting = true;
            ScoreTP++;
        }
    }

    private void Update()
    {
        if (ScoreTP == 1)
        {
            if (isTeleporting)
            {
                isTeleporting = false;
                characterController.enabled = false;
                characterController.transform.position = teleportTarget.position; // T�l�porte le personnage � l'endroit TP1
                characterController.enabled = true;
            }
        }

        if (ScoreTP == 2)
        {
            if (isTeleporting)
            {
                isTeleporting = false;
                characterController.enabled = false;
                characterController.transform.position = teleportTarget1.position; // T�l�porte le personnage � l'endroit TP2
                characterController.enabled = true;
            }
        }
    }
}
