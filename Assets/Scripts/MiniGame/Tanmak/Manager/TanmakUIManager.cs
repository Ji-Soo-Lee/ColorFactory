using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TanmakUIManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text ScoreText;
    public Image colorChangeImage;

    public void SetTimerText(float time)
    {
        timerText.text = "Time " + ((int)time).ToString();
    }

    public void SetScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void SetColorChangeImageColor(Color color)
    {
        colorChangeImage.color = color;
    }
}
