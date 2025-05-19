using System.Collections;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private float remainingTime;
    public TMP_Text timerText;

    public void StartGame(float serverTime)
    {
        remainingTime = serverTime;
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            int displayTime = Mathf.FloorToInt(remainingTime);
            timerText.text = displayTime.ToString();

            yield return null;
        }
    }
}