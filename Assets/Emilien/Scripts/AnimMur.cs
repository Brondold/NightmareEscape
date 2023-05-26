using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMur : MonoBehaviour
{

    public Animator animator;
    public Animator porteAnim;
    public bool isColliding;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = true;
            animator.SetBool("Collision", isColliding);
            porteAnim.SetBool("Collision", isColliding);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
