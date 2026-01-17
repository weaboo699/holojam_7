using UnityEngine;
using TMPro;
using System;
public class ClockManager : MonoBehaviour
{
    static public int CurrentHour;
    static public float CurrentSec;
    private float totalTime;

    [SerializeField]private TMP_Text currentTime;
    [SerializeField]private int startTime = 11;
    private float TimeInADay = 86400f;
    [SerializeField] private int timeScale = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        totalTime = startTime * 3600f;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime * (timeScale * 60f);
        totalTime %= TimeInADay;
        CurrentSec = totalTime;
        UpdateUI();
    }
    void UpdateUI()
    {
        int Hours = Mathf.FloorToInt(totalTime/3600f);
        int Min = Mathf.FloorToInt((totalTime - Hours * 3600f)/60f);
        String AmPm = Hours < 12 ? "Am":"Pm";
        CurrentHour = Hours;
        Hours %= 12;
        if (Hours == 0){
            Hours = 12;
        }
        string ClockString = String.Format("{0:00}:{1:00} {2}", Hours,Min,AmPm);
        currentTime.text = ClockString;
    }
}
