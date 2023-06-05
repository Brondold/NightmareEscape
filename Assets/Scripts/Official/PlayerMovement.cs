using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public MovementState state;

    public enum MovementState
    {
        Freeze,
        Unlimited
    }

    public bool freeze;
    public bool unlimited;

    public bool restricted;

    [Header("Movement")]
    public float moveSpeed;
    public float sprintMultiplier;
    public float slideDistance;
    public float slideHeight;

    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Stamina")]
    public float maxStamina;
    public float staminaDepletionRate;
    public float staminaRechargeRate;
    private float currentStamina;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode slideKey = KeyCode.LeftControl;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    [SerializeField] float fallTresholdVelocity = 5f;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 slideDirection;
    Vector3 slideStartPosition;
    bool isSliding;
    bool isCrouching;

    Rigidbody rb;

    [Header("UI")]
    public TextMeshProUGUI speedText; // Référence au composant TextMeshProUGUI pour afficher la vitesse
    public TextMeshProUGUI staminaText; // Référence au composant TextMeshProUGUI pour afficher la stamina
    public GameObject mortText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        currentStamina = maxStamina;

    }

    private void Update()
    {
        bool previousGrounded = grounded;
        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if(!previousGrounded && grounded)
        {
            Debug.Log("Fall Damage" + (rb.velocity.y < -fallTresholdVelocity));

            if (rb.velocity.y < -fallTresholdVelocity)
            {
                float damage = Mathf.Abs(rb.velocity.y + fallTresholdVelocity);
                Debug.Log("Damage Dealt :" + damage);
                if(damage >=4)
                {
                    mortText.SetActive(true);
                }
            }

        }
        MyInput();
        SpeedControl();
        Stamina();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Mettre à jour le texte de la vitesse dans l'UI
        speedText.text = rb.velocity.magnitude.ToString("F2");
        // Mettre à jour le texte de la stamina dans l'UI
        staminaText.text = currentStamina.ToString("F0");
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (isSliding)
        {
            Slide();
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && !isCrouching)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Slide
        if (Input.GetKeyDown(slideKey) && Input.GetKey(sprintKey) && grounded && currentStamina >= 20f && !isCrouching)
        {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey))
        {
            EndSlide();
        }

        //Crouch
        if (Input.GetKey(crouchKey) && grounded && !isSliding && !Input.GetKey(jumpKey) && !Input.GetKey(sprintKey))
        {
            StartCrouch();
        }
        else
        {
            EndCrouch();
        }
    }

    private void MovePlayer()
    {
        if (restricted) return;

        //Calcul Direction Mouvement
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Calcul de la vitesse du personnage lors de la Course
        float targetSpeed = moveSpeed;

        if (Input.GetKey(sprintKey) && currentStamina > 0 && !isCrouching)
        {
            targetSpeed *= sprintMultiplier;
            currentStamina -= Time.deltaTime * staminaDepletionRate;
            Debug.Log("Sprint");
        }

        //Transition Vitesse Smooth
        float currentSpeed = Mathf.Lerp(rb.velocity.magnitude, targetSpeed, Time.deltaTime * 15f);

        //Verification Stamina
        if (currentStamina <= 0)
        {
            currentSpeed = moveSpeed;

            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }
        else
        {
            //Applique Force Mouvement
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

        // Mettre à jour le texte de la vitesse dans l'UI
        speedText.text = targetSpeed.ToString("F2");
    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Stamina()
    {
        if (Input.GetKey(sprintKey) && currentStamina > 0)
        {
            currentStamina -= Time.deltaTime * staminaDepletionRate;
        }
        else
        {
            currentStamina += Time.deltaTime * staminaRechargeRate;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StartSlide()
    {
        if (isSliding) return;

        isSliding = true;
        slideStartPosition = transform.position;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        // Adjust the position to align with the ground
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, Mathf.Infinity, whatIsGround))
        {
            transform.position = groundHit.point + Vector3.up * slideHeight;
        }

        transform.localScale = new Vector3(1f, slideHeight, 1f);
        slideDirection = moveDirection.normalized;
    }

    private void Slide()
    {
        float distance = Vector3.Distance(slideStartPosition, transform.position);

        if (distance >= slideDistance)
        {
            EndSlide();
        }
        else
        {
            Vector3 slideMovement = slideDirection * moveSpeed * Time.deltaTime;
            RaycastHit groundHit;

            // Cast a ray from the current position to the desired slide position
            if (Physics.Raycast(transform.position, slideMovement, out groundHit, slideMovement.magnitude, whatIsGround))
            {
                // Adjust the slide movement to stop at the point of impact
                slideMovement = groundHit.point - transform.position;
            }

            // Move the character using the adjusted slide movement
            rb.MovePosition(rb.position + slideMovement);
        }
    }

    private void EndSlide()
    {
        if (!isSliding) return;

        isSliding = false;
        rb.useGravity = true;
        transform.localScale = new Vector3(.7f, .7f, .7f);
        currentStamina -= 20f;

        bool obstacleAboveHead = CheckObstacleAboveHead(); // Vérifie s'il y a un obstacle au-dessus de la tête

        // Effectuer un raycast vers le bas pour détecter le sol
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, whatIsGround))
        {
            // Ajuster la position du personnage en fonction de la distance au sol
            float distanceToGround = hit.distance;
            Vector3 newPosition = transform.position - new Vector3(0f, distanceToGround - playerHeight * 0.5f, 0f);
            transform.position = newPosition;

            // Vérifier si le personnage est toujours en contact avec le sol
            bool groundedAfterSlide = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

            // Si un obstacle est détecté au-dessus de la tête ou si le personnage n'est plus en contact avec le sol, passer en position "crouch"
            if (obstacleAboveHead || !groundedAfterSlide)
            {
                StartCrouch();
            }
            else
            {
                EndCrouch(); // Si aucun obstacle n'est détecté au-dessus de la tête, arrêter le crouch
            }
        }
    }

    private void StartCrouch()
    {
        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();

        if (isCrouching) return;

        isCrouching = true;

        rb.velocity = Vector3.zero;
        //transform.localScale = new Vector3(1f, 0.5f, 1f);

        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, Mathf.Infinity, whatIsGround))
        {
            transform.position = groundHit.point + Vector3.up * slideHeight;
        }

        playerCollider.height = .45f;
        playerCollider.radius = .22f;
        playerCollider.center = new Vector3(0f, -0.8f, -0.005f);

        //transform.localScale = new Vector3(1f, slideHeight, 1f);

    }

    private void EndCrouch()
    {
        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();

        if (!isCrouching) return;

        Vector3 standingPosition = transform.position + new Vector3(0f, playerHeight * 0.25f, 0f);
        RaycastHit hit;
        float standingHeight = playerHeight * 2f; // Hauteur de raycast pour vérifier les collisions au-dessus du personnage

        // Effectuer un raycast vers le haut pour vérifier les collisions
        if (Physics.Raycast(standingPosition, Vector3.up, out hit, standingHeight))
        {
            // Si une collision est détectée, empêcher le personnage de se relever
            return;
        }

        // Si aucune collision n'est détectée, le personnage peut se relever
        transform.position = standingPosition;

        isCrouching = false;
        playerCollider.height = .9f;
        playerCollider.radius = .22f;
        playerCollider.center = new Vector3(0f, -0.556f, -0.005f);

        transform.localScale = new Vector3(.7f, .7f, .7f);
    }

    private bool CheckHeadCollision()
    {
        Vector3 headPosition = transform.position + new Vector3(0f, playerHeight * 0.5f, 0f);
        float headCheckDistance = playerHeight; // Distance de vérification de collision au-dessus de la tête

        RaycastHit hit;
        if (Physics.Raycast(headPosition, Vector3.up, out hit, headCheckDistance))
        {
            // Une collision a été détectée au-dessus de la tête
            return true;
        }

        // Aucune collision détectée au-dessus de la tête
        return false;
    }

    private bool CheckObstacleAboveHead()
    {
        Vector3 headPosition = transform.position + new Vector3(0f, playerHeight * 0.5f, 0f);
        float headCheckDistance = playerHeight * 0.5f; // Distance de vérification de collision au-dessus de la tête

        RaycastHit hit;
        if (Physics.SphereCast(headPosition, headCheckDistance, Vector3.up, out hit, 0f, whatIsGround))
        {
            // Une collision a été détectée au-dessus de la tête
            return true;
        }

        // Aucune collision détectée au-dessus de la tête
        return false;
    }



    private void StateHandler()
    {
        if (freeze)
        {
            state = MovementState.Freeze;
            rb.velocity = Vector3.zero;
        }
        else if (unlimited)
        {
            state = MovementState.Unlimited;
            moveSpeed = 999f;
            return;
        }
    }
}