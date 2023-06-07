using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject canvas;

    public void DesactivateCanvas()
    {
        canvas.SetActive(false);
    }

}
