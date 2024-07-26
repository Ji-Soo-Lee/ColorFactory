using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StopwatchManager : MonoBehaviour
{
    protected float stopwatchTime;       // Current Stopwatch Time Value
    protected bool isStopwatchRunning;   // Stopwatch Status
    protected float stopwatchTik;          // Timer Tik for callback
    protected float stopwatchTikStride;    // Stride for Tik

    protected Action stopwatchTikHandler;// Stopwatch Tik handler

    // Configure Stopwatch Tik
    // Input : stride(sec), handler(method)
    public void SetupStopwatchTik(float stride, Action handler)
    {
        stopwatchTik = 0;
        stopwatchTikStride = stride;
        stopwatchTikHandler = handler;
    }

    protected virtual void Update()
    {
        if (isStopwatchRunning)
        {
            stopwatchTime += Time.deltaTime;

            // Activate Stopwatch Tik Logic : invoke handler for every tik
            if ((stopwatchTime - stopwatchTik) >= stopwatchTikStride)
            {
                stopwatchTik = stopwatchTime;
                stopwatchTikHandler?.Invoke();
            }
        }
    }

    // Activate Stopwatch
    public virtual void StartStopwatch()
    {
        isStopwatchRunning = true;
    }

    // Pause Stopwatch
    public virtual void PauseStopwatch()
    {
        isStopwatchRunning = false;
    }

    // Reset Stopwatch
    public virtual void ResetStopwatch()
    {
        stopwatchTime = 0;
        isStopwatchRunning = false;
        stopwatchTik = 0;
        stopwatchTikStride = 0;
    }

    public float GetStopwatchValue()
    {
        return stopwatchTime;
    }
}