using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMecs : MonoBehaviour
{
    public CharacterController controller;

    public float walkSpeed = 15f;
    public float runSpeed = 23f;
    public float gravity = -19.62f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    float speed;
    Vector3 velocity;
    bool isGrounded;
    bool midAirJump;
    float runTimer;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (z > 0)
        {
            runTimer += Time.deltaTime;
            if (runTimer > 4)
                speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
            runTimer = 0;
        }

        Vector3 move = transform.right * x * walkSpeed + transform.forward * z * speed;
        controller.Move(move * Time.deltaTime);
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);    //your jumping code
                midAirJump = true;
            }
            else if (midAirJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);    //your jumping code
                midAirJump = false;
            }
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
