using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float elapsedTime = 0f;
    private bool isTimerActive = false;

    public TextMeshProUGUI timerText;
    public GameObject Ecranfin;

    public float ElapsedTime
    {
        get { return elapsedTime; }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();

            // Vérifier si le timer atteint les 10 secondes
            if (elapsedTime >= 10f)
            {
                Ecranfin.SetActive(true);
                Debug.Log("J'ai passé 10 secondes");
            }
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        string timerString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        timerText.text = timerString;
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isTimerActive = true;
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}
