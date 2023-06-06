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

    public CapsuleCollider crouch;
    public CapsuleCollider idle;

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

    [Header("HeadCheck")]
    public Transform headCheck; // R�f�rence � l'objet vide plac� au-dessus de la t�te du personnage
    public float detectionRadius = 0.5f; // Rayon de d�tection
    public bool isObjectDetected = false;


    [Header("Animation")]
    public Animator animator;

    [Header("UI")]
    public TextMeshProUGUI speedText; // R�f�rence au composant TextMeshProUGUI pour afficher la vitesse
    public TextMeshProUGUI staminaText; // R�f�rence au composant TextMeshProUGUI pour afficher la stamina
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

        //Fall Damage & detection for death from Fall Damage
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

        //HeadCheckCollision with SphereRadius
        Collider[] colliders = Physics.OverlapSphere(headCheck.position, detectionRadius);

        isObjectDetected = false;

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject && collider.gameObject.tag != "Player")
            {
                Debug.Log("Objet : " + collider.gameObject.name);
                isObjectDetected = true;
            }
        }

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

        if (Input.GetKeyUp(slideKey) && !isObjectDetected)
        {
            EndSlide();
        }
        if (Input.GetKeyUp(slideKey) && isObjectDetected)
        {
            StartCrouch();
        }


        //Crouch
        if (Input.GetKeyDown(crouchKey) && grounded && !isSliding && !Input.GetKey(jumpKey) && !Input.GetKey(sprintKey))
        {
            StartCrouch();
            Debug.Log("Crouch");

            animator.SetBool("crouchIdle", true);
        }
        if (Input.GetKeyUp(crouchKey) && grounded && !isSliding && !Input.GetKey(jumpKey) && !Input.GetKey(sprintKey) && !isObjectDetected)
        {
            EndCrouch();
            Debug.Log("StopCrouch");
            animator.SetBool("crouchIdle", false);
        }

        //MyInput();
        SpeedControl();
        Stamina();

        // handle drag
        //if (grounded)
          //.  rb.drag = groundDrag;
        //.else
        //    rb.drag = 0;

        // Mettre � jour le texte de la vitesse dans l'UI
        speedText.text = rb.velocity.magnitude.ToString("F2");
        // Mettre � jour le texte de la stamina dans l'UI
        staminaText.text = currentStamina.ToString("F0");
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if(Input.GetKey(crouchKey) == false && !isObjectDetected)
        {
            EndCrouch();
        }

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

        if (Input.GetKeyUp(slideKey) && !isObjectDetected)
        {
            EndSlide();
        }
        if(Input.GetKeyUp(slideKey) && isObjectDetected)
        {
            StartCrouch();
        }

        //Crouch
        if (Input.GetKeyDown(crouchKey) && grounded && !isSliding && !Input.GetKey(jumpKey) && !Input.GetKey(sprintKey))
        {
            StartCrouch();
            Debug.Log("Crouch");

            animator.SetBool("crouchIdle", true);
        }
        if(Input.GetKeyUp(crouchKey) && grounded &&!isSliding && !Input.GetKey(jumpKey) && !Input.GetKey(sprintKey) && !isObjectDetected)
        {
            EndCrouch();
            Debug.Log("StopCrouch");
            animator.SetBool("crouchIdle", false);
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

        // Mettre � jour le texte de la vitesse dans l'UI
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
        //RaycastHit groundHit;

        idle.enabled = false;
        crouch.enabled = true;

        //transform.localScale = new Vector3(1f, slideHeight, 1f);
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
        currentStamina -= 20f;

        // Effectuer un raycast vers le bas pour d�tecter le sol
        RaycastHit hit;

        if (isObjectDetected)
        {
            StartCrouch();
        }
        else
        {
            idle.enabled = true;
            crouch.enabled = false;
        }      
    }

    private void StartCrouch()
    {

        if (isCrouching) return;

        isCrouching = true;

        rb.velocity = Vector3.zero;       

        idle.enabled = false;
        crouch.enabled = true;

    }

    private void EndCrouch()
    {

        if (!isCrouching) return;

        isCrouching = false;

        idle.enabled = true;
        crouch.enabled = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(headCheck.position, detectionRadius);
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