using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    protected float timerDuration;       // Duration of Timer
    protected float timer;               // Current Timer value
    protected bool isTimerRunning;       // Timer status
    protected int timerTik;              // Timer Tik for callback
    protected int timerTikStride;        // Stride for Tik

    protected float stopwatchTime;       // Current Stopwatch Time Value
    protected bool isStopwatchRunning;   // Stopwatch Status
    protected int stopwatchTik;          // Timer Tik for callback
    protected int stopwatchTikStride;    // Stride for Tik

    protected Action timerHandler;       // Timer handler
    protected Action timerTikHandler;    // Timer Tik handler
    protected Action stopwatchTikHandler;// Stopwatch Tik handler

    // Configure Timer
    // Input : duration(sec), handler(method)
    public void SetupTimer(float duration, Action handler)
    {
        timerDuration = duration;
        timerHandler = handler;
        ResetTimer();
    }

    // Configure Timer Tik
    // Input : stride(sec), handler(method)
    public void SetupTimerTik(int stride, Action handler)
    {
        timerTikStride = stride;
        timerTik = (int)timer;
        timerTikHandler = handler;
    }

    // Configure Stopwatch Tik
    // Input : stride(sec), handler(method)
    public void SetupStopwatchTik(int stride, Action handler)
    {
        stopwatchTik = 0;
        stopwatchTikStride = stride;
        stopwatchTikHandler = handler;
    }

    protected virtual void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                TimerExpired();
            }

            // Activate Timer Tik Logic : invoke handler for every tik
            if ((timerTik - (int)timer) >= timerTikStride)
            {
                timerTik = (int)timer;
                timerTikHandler?.Invoke();
            }
        }

        if (isStopwatchRunning)
        {
            stopwatchTime += Time.deltaTime;

            // Activate Stopwatch Tik Logic : invoke handler for every tik
            if (((int)stopwatchTime - stopwatchTik) >= stopwatchTikStride)
            {
                stopwatchTik = (int)stopwatchTime;
                stopwatchTikHandler?.Invoke();
            }
        }
    }

    // Activate Timer
    public virtual void StartTimer()
    {
        isTimerRunning = true;
    }

    // Activate Stopwatch
    public virtual void StartStopwatch()
    {
        isStopwatchRunning = true;
    }

    // Pause Timer
    public virtual void PauseTimer()
    {
        isTimerRunning = false;
    }

    // Pause Stopwatch
    public virtual void PauseStopwatch()
    {
        isStopwatchRunning = false;
    }

    // Reset Timer
    public virtual void ResetTimer()
    {
        timer = timerDuration;
        isTimerRunning = false;
        timerTik = 0;
        timerTikStride = 0;
    }

    // Reset Stopwatch
    public virtual void ResetStopwatch()
    {
        stopwatchTime = 0;
        isStopwatchRunning = false;
        stopwatchTik = 0;
        stopwatchTikStride = 0;
    }

    protected virtual void TimerExpired()
    {
        isTimerRunning = false;
        timerHandler?.Invoke();
    }

    public float GetTimerValue()
    {
        return timer;
    }

    public float GetStopwatchValue()
    {
        return stopwatchTime;
    }
}