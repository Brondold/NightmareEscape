using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{
    private float totalTime = 0f;
    private float startTime = 0f;
    private bool isTimerActive = false;

    public TextMeshProUGUI timerText;
    public GameObject Ecranfin;


    void Update()
    {
        if (isTimerActive)
        {
            totalTime += Time.deltaTime;
            UpdateTimerText();

            // Vérifier si le timer atteint les 3 minutes (180 secondes)
            if (totalTime >= 10f)
            {
                //ShowImage(); // Afficher l'image lorsque le timer atteint 3 minutes
                Ecranfin.SetActive(true);
                Debug.Log("J'ai passé 10secondes");
            }

            // Vérifier si le timer atteint ou dépasse 3 minutes (180 secondes)
            if (totalTime >= 10f)
            {
                StopTimer(); // Arrêter le timer lorsque 3 minutes sont atteintes
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FIN"))
        {
            StopTimer();
            Debug.Log("Je fonctionne");
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(totalTime / 60f);
        int seconds = Mathf.FloorToInt(totalTime % 60f);
        int milliseconds = Mathf.FloorToInt((totalTime * 1000f) % 1000f);

        string timerString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        timerText.text = timerString;
    }


    public void StartTimer()
    {
        startTime = Time.time;
        isTimerActive = true;
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    public void ResetTimer()
    {
        totalTime = 0f;
        startTime = 0f;
    }
}