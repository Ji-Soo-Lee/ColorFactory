using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    protected float timerDuration;       // Duration of Timer
    [SerializeField] protected float timer;               // Current Timer value
    protected bool isTimerRunning;       // Timer status
    protected float timerTik;              // Timer Tik for callback
    protected float timerTikStride;        // Stride for Tik

    protected Action timerHandler;       // Timer handler
    protected Action timerTikHandler;    // Timer Tik handler

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
    public void SetupTimerTik(float stride, Action handler)
    {
        timerTikStride = stride;
        timerTik = timer;
        timerTikHandler = handler;
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
            if ((timerTik - timer) >= timerTikStride)
            {
                timerTik = timer;
                timerTikHandler?.Invoke();
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
        timerTik = 0;
        timerTikStride = 0;
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

    public bool IsTimerExpired()
    {
        return timer <= 0f;
    }
}
