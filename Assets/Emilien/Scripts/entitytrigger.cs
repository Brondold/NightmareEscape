using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entitytrigger : MonoBehaviour
{

    public GameObject entity;

    private void OnTriggerEnter(Collider other)
    {
        entity.SetActive(true);
    }
}
