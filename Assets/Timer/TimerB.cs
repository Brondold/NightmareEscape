using UnityEngine;
using UnityEngine.UI;

public class TimerB : MonoBehaviour
{
    public Timer timer;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        timer.StartTimer();
    }
}