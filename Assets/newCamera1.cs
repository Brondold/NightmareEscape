using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCamera1 : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;

    private void OnTriggerEnter(Collider other)
    {
        camera1.SetActive(true);
        camera2.SetActive(false);
    }

}