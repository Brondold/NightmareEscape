using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOff : MonoBehaviour
{

    public GameObject light1;
    public GameObject light2;
    public GameObject light3;

    private void OnTriggerEnter(Collider other)
    {
        light1.SetActive(false);
        light2.SetActive(false);
        light3.SetActive(false);

    }
}
