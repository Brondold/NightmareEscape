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

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 slideDirection;
    Vector3 slideStartPosition;
    bool isSliding;

    Rigidbody rb;

    [Header("UI")]
    public TextMeshProUGUI speedText; // Référence au composant TextMeshProUGUI pour afficher la vitesse
    public TextMeshProUGUI staminaText; // Référence au composant TextMeshProUGUI pour afficher la stamina

    bool isCrouching;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

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

        // when to jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && !isCrouching)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // when to slide
        if (Input.GetKeyDown(slideKey) && Input.GetKey(sprintKey) && grounded && currentStamina >= 20f && !isCrouching)
        {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey))
        {
            EndSlide();
        }

        // when to crouch
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

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // calculate target speed based on sprinting and crouching
        float targetSpeed = moveSpeed;
        if (Input.GetKey(sprintKey) && currentStamina > 0 && !isCrouching)
        {
            targetSpeed *= sprintMultiplier;
            currentStamina -= Time.deltaTime * staminaDepletionRate;
        }

        // smoothly interpolate current speed towards target speed
        float currentSpeed = Mathf.Lerp(rb.velocity.magnitude, targetSpeed, Time.deltaTime * 15f);

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
            // apply the movement force
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
            rb.MovePosition(rb.position + slideDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void EndSlide()
    {
        if (!isSliding) return;

        isSliding = false;
        rb.useGravity = true;
        transform.localScale = new Vector3(1f, 1f, 1f);
        currentStamina -= 20f;
    }

    private void StartCrouch()
    {
        if (isCrouching) return;

        isCrouching = true;
        rb.velocity = Vector3.zero;
        transform.localScale = new Vector3(1f, 0.5f, 1f);

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.height *= 0.5f;
        capsuleCollider.center *= 0.5f;
    }

    private void EndCrouch()
    {
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
        transform.localScale = new Vector3(1f, 1f, 1f);
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