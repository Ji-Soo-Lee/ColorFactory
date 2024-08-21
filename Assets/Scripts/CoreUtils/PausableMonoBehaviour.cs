using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PausableMonoBehaviour : MonoBehaviour
{
    private bool isPause;
    public Action pauseHandler;
    public Action resumeHandler;

    public void AddPauseHandler(Action handler)
    {
        pauseHandler += handler;
    }

    public void AddResumeHandler(Action handler)
    {
        resumeHandler += handler;
    }

    public virtual void Pause()
    {
        Time.timeScale = 0; // Stops FixedUpdate
        isPause = true; // Stops After Pause Update

        pauseHandler?.Invoke();
    }

    public virtual void Resume()
    {
        Time.timeScale = 1f;
        isPause = false;

        resumeHandler?.Invoke();
    }

    protected virtual void OnDisable()
    {
        Resume();
    }

    public bool CheckPause()
    {
        return isPause;
    }

    // Update with pause mechanism
    // Use override if you don't like it.
    // You can use pause yourself like below in other cases
    // ex) if (CheckPause()) return;
    protected virtual void Update()
    {
        BeforePauseUpdate();
        if (isPause) return;
        AfterPauseUpdate();
    }

    protected virtual void BeforePauseUpdate() { } // doesn't pause
    protected virtual void AfterPauseUpdate() { } // can be paused
}
