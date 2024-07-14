using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    protected float timerDuration;       // Duration of Timer
    protected float timer;               // Current Timer value
    protected bool isTimerRunning;       // Timer status

    protected Action timerHandler;       // Timer handler

    // Configure Timer
    // Input : duration(sec), handler(method)
    public void SetupTimer(float duration, Action handler)
    {
        timerDuration = duration;
        timerHandler = handler;
        ResetTimer();
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
        }
    }

    // Activate Timer
    public virtual void StartTimer()
    {
        isTimerRunning = true;
    }

    // Pause Timer
    public virtual void PauseTimer()
    {
        isTimerRunning = false;
    }

    // Reset Timer
    public virtual void ResetTimer()
    {
        timer = timerDuration;
        isTimerRunning = false;
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
}