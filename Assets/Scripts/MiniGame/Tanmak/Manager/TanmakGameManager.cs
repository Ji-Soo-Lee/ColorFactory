using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakGameManager : StageManager
{
    public TimerManager invincibleTimer;
    public StopwatchManager stopwatchManager;
    public TanmakUIManager TUIManager;

    public Color[] colors;
    public static int colorSize = 3;

    // Modify Tanmak Mini Game Score
    public void ModifyScore(int score)
    {
        if (CheckPause()) return;

        if (score >= 0)
        {
            scoreManager.AddScore(score);
        }
        else
        {
            scoreManager.SubtractScore(-score);
        }
        TUIManager.SetScoreText(scoreManager.GetScore());
    }

    public override void Pause()
    {
        base.Pause();
        stopwatchManager.PauseStopwatch();
    }

    public override void Resume()
    {
        base.Resume();
        stopwatchManager.StartStopwatch();
    }

    public override void EndGame()
    {
        UnityEngine.Debug.Log("End Game");
        Pause();
        // End Logic Needed
        base.EndGame();
    }

    protected override void Start()
    {
        base.Start();
        // Setup Stopwatch
        stopwatchManager.ResetStopwatch();
        stopwatchManager.SetupStopwatchTik(1, () => ModifyScore(5));
        stopwatchManager.StartStopwatch();
    }

    void OnEnable()
    {
        // set initial random game colors
        colors = new Color[colorSize];

        for (int i = 0; i < colorSize; i++)
        {
            colors[i] = ColorUtils.GetRandomColor();
        }
    }

    protected override void AfterPauseUpdate()
    {
        // Set Timer UI
        TUIManager.SetTimerText(stopwatchManager.GetStopwatchValue());
    }
}
