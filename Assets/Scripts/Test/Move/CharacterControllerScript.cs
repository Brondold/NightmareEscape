using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float speed = 5f;              // Vitesse de d�placement du personnage
    public float rotationSpeed = 10f;     // Vitesse de rotation du personnage

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // D�placement du personnage en fonction des touches de d�placement (WASD, fl�ches)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        // Application de la gravit� au personnage
        moveDirection.y -= 9.8f * Time.deltaTime;

        // D�placement du personnage
        controller.Move(moveDirection * Time.deltaTime);
    }
}
