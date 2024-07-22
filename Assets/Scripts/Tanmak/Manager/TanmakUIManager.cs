using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TanmakUIManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text ScoreText;

    public void SetTimerText(float time)
    {
        timerText.text = "  Time : " + time.ToString();
    }

    public void SetScoreText(int score)
    {
        ScoreText.text = "  Score : " + score.ToString();
    }
}
