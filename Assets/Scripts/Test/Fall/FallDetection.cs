using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    public float minHeightToDie = 10f; // Hauteur minimale pour d�clencher la mort
    public LayerMask groundLayer; // Couche du sol
    public Transform groundCheck; // Point de v�rification pour d�tecter le sol (par exemple, un objet vide situ� pr�s des pieds du personnage)

    private bool isFalling = false;
    private float fallStartHeight = 0f;
    private bool isDead = false;

    private void Update()
    {
        // V�rifier si l'objet est en contact avec le sol
        bool isTouchingGround = Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer);

        if (isTouchingGround && isFalling && !isDead)
        {
            // Calculer la hauteur de la chute
            float fallHeight = fallStartHeight - hit.point.y;

            // V�rifier si la hauteur de la chute est sup�rieure � la hauteur minimale pour mourir
            if (fallHeight > minHeightToDie)
            {
                Die();
            }
        }

        // Si l'objet est en l'air et n'est pas en train de tomber, enregistrer la hauteur de d�part de la chute
        if (!isTouchingGround && !isFalling)
        {
            isFalling = true;
            fallStartHeight = transform.position.y;
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Vous �tes mort !");
        // Ici, vous pouvez ajouter du code pour afficher un message � l'�cran ou effectuer d'autres actions lorsque le personnage meurt
    }
}