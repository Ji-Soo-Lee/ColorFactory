using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameManager : MonoBehaviour
{
    private bool isPause;

    public virtual void Pause()
    {
        Time.timeScale = 0; // Stops FixedUpdate
        isPause = true; // Stops After Pause Update
    }

    public virtual void Resume()
    {
        Time.timeScale = 1f;
        isPause = false;
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

    // Below methods must be implemented
    protected abstract void BeforePauseUpdate(); // doesn't pause
    protected abstract void AfterPauseUpdate(); // can be paused
}
