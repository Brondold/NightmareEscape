using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    public CapsuleCollider ledgeCollider;
    public LayerMask ledgeLayer;
    public float grabRange = 1.5f;
    public float climbSpeed = 2f;
    public float grabCooldown = 1f;

    public float m_speed = 5f;

    private bool isGrabbing;
    private float lastGrabTime;
    private Rigidbody rb;
    private Vector3 originalPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isGrabbing)
        {

            if (Input.GetMouseButtonUp(0))
            {
                // Lâcher le rebord
                isGrabbing = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                lastGrabTime = Time.time;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Logic pour monter le personnage sur le rebord
                // Par exemple, vous pouvez déplacer le personnage vers le haut avec une certaine vitesse (climbSpeed)
                // Vous pouvez utiliser rb.MovePosition() pour déplacer le Rigidbody du personnage

                Vector3 m_input = new Vector3(Input.GetAxis("Horizontal"), 20, Input.GetAxis("Vertical"));

                rb.MovePosition(transform.position + m_input * Time.deltaTime * m_speed);
            }

            return;
        }

        if (Time.time - lastGrabTime < grabCooldown)
        {
            // Attente de la fin du cooldown avant de pouvoir s'accrocher à nouveau
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Si le personnage est déjà en train de grimper, ne pas agripper un autre rebord
            if (isGrabbing)
                return;

            // Vérifier si le personnage est dans la zone d'un rebord (layer "Ledge") en utilisant des collisions de déclencheurs
            Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange, ledgeLayer);
            if (colliders.Length > 0)
            {
                // Agripper le rebord
                isGrabbing = true;
                originalPosition = transform.position;

                // Freeze la position et la rotation du personnage
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            if (Input.GetMouseButton(0) && Time.time - lastGrabTime >= grabCooldown)
            {
                isGrabbing = true;
                originalPosition = transform.position;

                // Freeze la position du personnage
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ledge") )
        {
            // Lâcher le rebord lorsque le personnage quitte sa zone
            isGrabbing = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            lastGrabTime = Time.time;
        }
    }
}