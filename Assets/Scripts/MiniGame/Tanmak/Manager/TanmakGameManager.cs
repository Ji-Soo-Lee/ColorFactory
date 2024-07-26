using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakGameManager : PausableMonoBehaviour
{
    public TimerManager timerManager;
    public ScoreManager scoreManager;
    public ScoreDataManager scoreDataManager;
    public TanmakUIManager TUIManager;
    public DummySceneController sceneController;

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
        timerManager.PauseStopwatch();
    }

    public override void Resume()
    {
        base.Resume();
        timerManager.StartStopwatch();
    }

    public void EndGame()
    {
        UnityEngine.Debug.Log("End Game");
        Pause();
        // End Logic Needed
        // Save & Show Score
        scoreDataManager.finalMiniGameScore = scoreManager.GetScore();
        // Small Menu?
        TUIManager.ToggleGameOverPanel();
    }

    void Start()
    {
        // Setup Stopwatch
        timerManager.ResetStopwatch();
        timerManager.SetupStopwatchTik(1, () => ModifyScore(5));
        timerManager.StartStopwatch();
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
        TUIManager.SetTimerText(timerManager.GetStopwatchValue());
    }
}
