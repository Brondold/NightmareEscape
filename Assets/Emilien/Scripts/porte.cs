using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class porte : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float downSpeed = 1f;
    public bool sol = true;
    public bool ready = false;

    private Rigidbody rb;

    public bool isMoving = false;
    public float heightLimit = 5f;

    public bool plafond = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("sol"))
        {
            sol = false;
            Debug.Log("au sol");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("sol"))
        {
            sol = true;
            Debug.Log("en l'air");
        }
    }


    private void Update()
    {
        if (ready)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isMoving = true;
                rb.isKinematic = true;
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                isMoving = false;
                rb.isKinematic = false;
                plafond = true;
            }

            if (isMoving)
            {
                if (transform.position.y < heightLimit)
                {
                    transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                    plafond = true;
                }
                else
                {
                    isMoving = false; // Stop mouvement si limite de hauteur atteinte
                    plafond = false;
                }
            }
                else
                {
                    if (sol && plafond)
                    {
                        transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
                    }
                }

        }
        else
        {
            isMoving = false;
            rb.isKinematic = false;
            if (sol)
            {

                transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
            }
        }
        
    }
}
