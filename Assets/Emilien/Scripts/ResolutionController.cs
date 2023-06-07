using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    public void OnResolutionChanged()
    {
        // R�cup�rer la r�solution s�lectionn�e dans le menu d�roulant
        string selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;

        // Diviser la cha�ne de r�solution en largeur et hauteur
        string[] resolutionValues = selectedResolution.Split('x');
        int width = int.Parse(resolutionValues[0]);
        int height = int.Parse(resolutionValues[1]);

        // Changer la r�solution de l'�cran
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}

