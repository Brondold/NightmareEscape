using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathmur : MonoBehaviour
{
    private bool first = false;
    private bool second = false;
    public GameObject canvas;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("mur1"))
        {
            first = true;
            Debug.Log("collision mur1");
        }

        if (collision.gameObject.CompareTag("mur2"))
        {
            second = true;
            Debug.Log("collision mur2");
        }

        if (first && second)
        {
            canvas.SetActive(true); 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("mur1"))
        {
            first = false;
        }

        if (collision.gameObject.CompareTag("mur2"))
        {
            second = false;
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
