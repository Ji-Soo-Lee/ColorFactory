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
    protected int timerClk;              // Timer Clock for callback
    protected int timerClkStride;        // Stride for Clk

    protected Action timerHandler;       // Timer handler
    protected Action timerClkHandler;

    // Configure Timer
    // Input : duration(sec), handler(method)
    public void SetupTimer(float duration, Action handler)
    {
        timerDuration = duration;
        timerHandler = handler;
        ResetTimer();
    }

    public void SetupClk(int stride, Action handler)
    {
        timerClkStride = stride;
        timerClk = (int)timer;
        timerClkHandler = handler;
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

            if ((timerClk - (int)timer) >= timerClkStride)
            {
                timerClk = (int)timer;
                timerClkHandler?.Invoke();
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
        timerClk = 0;
        timerClkStride = 0;
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