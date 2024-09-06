using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
   public TextMeshProUGUI timerText; // Süreyi gösteren UI Text
    public TextMeshProUGUI recordText; // Rekoru gösteren UI Text

    private float startTime;
    private bool isTiming;

    private void Start()
    {
        ResetRecord();
        StartTimer();
        DisplayRecordTime();
    }

    void Update()
    {
        if (isTiming)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            float seconds = t % 60;

            int secondsInt = (int)seconds;
            int milliseconds = (int)((seconds - secondsInt) * 1000);

            string secondsFormatted = secondsInt.ToString() + "." + milliseconds.ToString("D3");

            timerText.text = minutes + ":" + secondsFormatted;
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isTiming = true;
    }

    public void StopTimer()
    {
        isTiming = false;
        float finalTime = Time.time - startTime;
        CheckAndSetRecord(finalTime);
        DisplayRecordTime();
    }

    private void CheckAndSetRecord(float finalTime)
    {
        float recordTime = PlayerPrefs.GetFloat("RecordTime", float.MaxValue); // cihazdaki kayıtlı rekoru alır, eğer kayıtlı rekor yoksa en yüksek float değeri kayıtlı değer olur.
        if (finalTime < recordTime)
        {
            PlayerPrefs.SetFloat("RecordTime", finalTime);
            Debug.Log("New Record: " + finalTime);
        }
    }

    private void DisplayRecordTime()
    {
        float recordTime = PlayerPrefs.GetFloat("RecordTime", float.MaxValue);

        if (recordTime == float.MaxValue)
        {
            recordText.text = "No Record";
        }
        else
        {
            string minutes = ((int)recordTime / 60).ToString();
            float seconds = recordTime % 60;

            int secondsInt = (int)seconds;
            int milliseconds = (int)((seconds - secondsInt) * 1000);

            string recordFormatted = minutes + ":" + secondsInt.ToString() + "." + milliseconds.ToString("D3");

            recordText.text = recordFormatted;
        }
    }

    public void ResetRecord()
    {
        PlayerPrefs.DeleteKey("RecordTime");
        DisplayRecordTime();
    }
}
