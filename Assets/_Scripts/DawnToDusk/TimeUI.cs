using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public Text timeText;
    public Text dayText;

    private void OnEnable()
    {
        //TimeManager.OnMinuteChanged += UpdateTime;
        TimeManager.OnTenMinuteChanged += UpdateTime;
        TimeManager.OnHourChanged += UpdateTime;
        TimeManager.OnDayChanged += UpdateTime;
    }
    private void OnDisable()
    {
        //TimeManager.OnMinuteChanged -= UpdateTime;
        TimeManager.OnTenMinuteChanged -= UpdateTime;
        TimeManager.OnHourChanged -= UpdateTime;
        TimeManager.OnDayChanged -= UpdateTime;
    }
    private void Start()
    {
        UpdateTime();
    }
    private void UpdateTime()
    {
        timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}";
        dayText.text = $"Day {TimeManager.Day}";
    }
}
