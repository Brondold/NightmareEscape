using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public Timer timer;
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        int score = CalculateScore();
        UpdateScoreText(score);
    }

    private int CalculateScore()
    {
        int timerInteger = timer.TimerToInteger();
        int maxScore = 180000; // Valeur maximale du score

        // Calculer le score en soustrayant le temps �coul� � la valeur maximale du score
        int score = maxScore - timerInteger;

        // S'assurer que le score ne devienne pas n�gatif
        score = Mathf.Max(score, 0);

        return score;
    }


    private void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}
