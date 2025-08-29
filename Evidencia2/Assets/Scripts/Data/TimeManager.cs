using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static Action OnPatternChange;
    private float patternDuration = 10f;
    private float timer;

    private void Start()
    {
        timer = patternDuration;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            OnPatternChange?.Invoke();
            timer = patternDuration;
        }
    }
}
