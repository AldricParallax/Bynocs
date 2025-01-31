using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    private float startTime;
    private float timerDuration = -1f;
    private bool isRunning = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start the timer
    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    // Reset the timer
    public void ResetTimer()
    {
        startTime = 0f;
        isRunning = false;
    }

    // Get current elapsed time in minutes, seconds, and milliseconds (two decimals)
    public string GetCurrentTime()
    {
        if (!isRunning) return "00:00.00";

        float elapsedTime = Time.time - startTime;
        return FormatTime(elapsedTime);
    }

    // Set a timer duration and stop when it reaches the limit
    public void SetTimerDuration(float duration)
    {
        timerDuration = duration;
        StartTimer();
    }

    private void Update()
    {
        if (isRunning && timerDuration > 0 && Time.time - startTime >= timerDuration)
        {
            isRunning = false;
        }
    }

    // Calculate response time difference
    public string CalculateResponseTime(float spawnTime, float responseTime)
    {
        float difference = responseTime - spawnTime;
        return FormatTime(difference);
    }

    // Format time in mm:ss:ms format
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time * 100) % 100; // Two decimal places

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}

