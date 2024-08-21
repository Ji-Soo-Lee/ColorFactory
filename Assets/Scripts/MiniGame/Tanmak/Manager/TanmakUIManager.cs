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
    public GameObject RecordTextObject;

    public void SetTimerText(float time)
    {
        timerText.text = "TIME" + ((int)time).ToString();
    }

    public void SetScoreText(int score)
    {
        // string text = "";
        
        // while(score >= 1000)
        // {
        //     text = "," + (score % 1000).ToString("D3") + text;
        //     score /= 1000;
        // }
        // text = score.ToString() + text;

        ScoreText.text = score.ToString("N0");
    }

    public void SetColorChangeImageColor(Color color)
    {
        colorChangeImage.color = color;
    }

    public void SetActiveRecordText(bool isActive)
    {
        RecordTextObject.SetActive(isActive);
    }
}
