using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJINteraction : MonoBehaviour
{
    public string dialogueText = "Bonjour, comment vas-tu ?";
    public bool isInRange = false;
    public KeyCode interactKey = KeyCode.E;
    public GUIStyle dialogueStyle;
    public porte porteready;
    public CircleMovement porteManReady;

    void OnGUI()
    {
        if (isInRange)
        {
            GUI.Box(new Rect(10, Screen.height - 100, Screen.width - 20, 90), dialogueText, dialogueStyle);

            /*if (Input.GetKeyDown(interactKey))
            {
                // Lancer une action lors de l'interaction
                Debug.Log("Interaction avec le PNJ !");
            }*/
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            porteready.ready = true;
            porteManReady.readyManiv = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            porteready.ready = false;
            porteManReady.readyManiv = false;
            porteready.plafond = true;
        }
    }
}