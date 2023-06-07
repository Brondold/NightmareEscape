using UnityEngine;
using UnityEngine.UI;

public class DisplayModeController : MonoBehaviour
{
    public Toggle fullscreenToggle;

    public void OnDisplayModeChanged()
    {
        // R�cup�rer l'�tat de la case � cocher
        bool isFullscreen = fullscreenToggle.isOn;

        // Changer le mode d'affichage
        Screen.fullScreen = isFullscreen;
    }
}
