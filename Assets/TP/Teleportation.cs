using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform teleportTarget; // L'endroit TP1
    public Transform teleportTarget1;
    public Transform teleportTarget2;
    public float ScoreTP = 0;

    private CharacterController characterController;
    public GameObject controller;
    private bool isTeleporting;

    private void Start()
    {
        //characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportationSurface")) // Vérifie si le personnage entre en contact avec la surface de téléportation
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
                //characterController.enabled = false;
                controller.SetActive(false);
                controller.transform.position = teleportTarget.position; // Téléporte le personnage à l'endroit TP1
                controller.SetActive(true);
                //characterController.enabled = true;
            }
        }

        if (ScoreTP == 2)
        {
            if (isTeleporting)
            {
                isTeleporting = false;
                controller.SetActive(false);
                controller.transform.position = teleportTarget1.position; // Téléporte le personnage à l'endroit TP2
                controller.SetActive(true);
            }
        }

        if (ScoreTP == 3)
        {
            if (isTeleporting)
            {
                isTeleporting = false;
                controller.SetActive(false);
                controller.transform.position = teleportTarget2.position; // Téléporte le personnage à l'endroit TP2
                controller.SetActive(true);
            }
        }
    }
}
