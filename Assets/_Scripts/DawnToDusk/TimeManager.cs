using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnTenMinuteChanged;
    public static Action OnHourChanged;
    public static Action OnDayChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }

    private float minuteToRealTime = 0.1f;
    private float timer;
    private int tenMinutesCount;

    private void Start()
    {
        Minute = 0;
        Hour = 23;
        Day = 1;
        timer = minuteToRealTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Minute++;
            tenMinutesCount++;
            OnMinuteChanged?.Invoke();
            if (tenMinutesCount >= 10)
            {
                OnTenMinuteChanged?.Invoke();
                tenMinutesCount = 0;
                if (Minute >= 60)
                {
                    Hour++;
                    Minute = 0;
                    OnHourChanged?.Invoke();
                    if (Hour >= 24)
                    {
                        Day++;
                        Hour = 0;
                        OnDayChanged?.Invoke();
                    }
                }
            }
            timer = minuteToRealTime;
        }
    }

 
}
