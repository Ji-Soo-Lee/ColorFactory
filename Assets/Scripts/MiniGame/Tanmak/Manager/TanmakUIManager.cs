using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tanmak
{
    public class TanmakUIManager : MonoBehaviour
    {
        public TMP_Text timerText;
        public TMP_Text ScoreText;
        public GameObject ResultPanel;

        public void SetTimerText(float time)
        {
            timerText.text = "Time " + ((int)time).ToString();
        }

        public void SetScoreText(int score)
        {
            ScoreText.text = score.ToString();
        }
    }
}