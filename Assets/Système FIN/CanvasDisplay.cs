using UnityEngine;

public class CanvasDisplay : MonoBehaviour
{
    public GameObject image;
    private bool isTouchingFin = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FIN"))
        {
            isTouchingFin = true;
            image.gameObject.SetActive(true);
            Debug.Log("J'entre");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FIN"))
        {
            isTouchingFin = false;
            image.gameObject.SetActive(false);
            Debug.Log("Je sors");
        }
    }
}