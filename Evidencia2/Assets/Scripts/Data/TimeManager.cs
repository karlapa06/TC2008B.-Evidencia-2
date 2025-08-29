using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static Action OnPatternChange;
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private float minuteToRealTime = 0.5f;
    private float timer;

    private void Start()
    {
        Minute = 0;
        Hour = 0;
        timer = minuteToRealTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Minute++;
            if (Minute % 10 == 0) OnPatternChange?.Invoke(); // Cambia patrón cada 10s

            if (Minute >= 60)
            {
                Hour++;
                Minute = 0;
            }
            timer = minuteToRealTime;
        }
    }
}
