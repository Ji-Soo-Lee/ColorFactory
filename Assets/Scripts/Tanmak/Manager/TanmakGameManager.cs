using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakGameManager : MonoBehaviour
{
    public TimerManager timerManager;
    public ScoreManager scoreManager;
    public TanmakUIManager TUIManager;

    public Color[] colors;
    public static int colorSize = 3;

    public void ModifyScore(int score)
    {
        if (score >= 0)
        {
            scoreManager.AddScore(score);
        }
        else
        {
            scoreManager.SubtractScore(-score);
        }
        TUIManager.SetScoreText(int.Parse(scoreManager.GetScoreAsString()));
    }

    void Start()
    {
        timerManager.SetupTimer(100.0f, null);
        timerManager.StartTimer();
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

    void Update()
    {
        TUIManager.SetTimerText(timerManager.GetTimerValue());
    }
}
