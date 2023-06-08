using UnityEngine;

public class DeletePref : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        // Autres actions de démarrage du jeu...
    }
}
