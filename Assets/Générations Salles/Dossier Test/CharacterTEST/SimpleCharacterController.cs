using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 300f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private bool isJumping;
    public Timer timer;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FIN"))
        {
            // Arrêter le timer via le script Timer attaché au joueur
            timer.StopTimer();
        }
    }

    private void Update()
    {
        // Déplacement horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection = transform.forward * (horizontalInput * moveSpeed);

        // Saut
        if (characterController.isGrounded)
        {
            isJumping = false;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
                isJumping = true;
            }
        }

        // Gravité
        moveDirection.y -= gravity * Time.deltaTime;

        // Rotation
        float rotationInput = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.up * (rotationInput * rotationSpeed * Time.deltaTime));

        // Déplacement vertical
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
