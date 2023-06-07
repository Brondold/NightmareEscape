using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCamera : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    private void OnTriggerExit(Collider other)
    {
        camera1.SetActive(true);
        camera2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        camera2.SetActive(true);
        camera1.SetActive(false);
    }
}
