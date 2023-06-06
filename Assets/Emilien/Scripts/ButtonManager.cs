using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject crochet;

    public void DesactivateCrochet()
    {
        crochet.SetActive(false);
    }

    public void ActivateCrochet()
    {
        crochet.SetActive(true);
    }
}
