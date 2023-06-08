using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class morttrigger : MonoBehaviour
{

    public GameObject player;
    public GameObject canvas;

    private void OnTriggerEnter(Collider other)
    {
        player.SetActive(false);
        canvas.SetActive(true);
    }
}
