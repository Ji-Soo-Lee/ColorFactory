using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanmak
{
    [System.Serializable]
    public class TanmakGameData
    {
        public int highScore;
    }

    public class TanmakGameManager : StageManager
    {
        public TimerManager invincibleTimer;
        public StopwatchManager stopwatchManager;
        public TanmakUIManager TUIManager;

        public GameObject World;
        public GameObject Map;

        public Color[] colors;
        public static int colorSize = 3;
        public Dictionary<string, Color> bulletColorMapping;

        public int highScore = 0;

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

            int curScore = scoreManager.GetScore();

            TUIManager.SetScoreText(curScore);

            // if (curScore > highScore)
            // {
            //     highScore = curScore;
            //     TUIManager.SetActiveRecordText(true);
            // }
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

        public override void EndGameWithTime()
        {
            UnityEngine.Debug.Log("End Game");
            Pause();
            // End Logic Needed
            base.EndGameWithTime();

            SaveGameData();

            commonPopupUIManager.SetResultTextWithTime((int)stopwatchManager.GetStopwatchValue(), scoreManager.GetScore(), true);
        }

        protected override void Start()
        {
            base.Start();

            LoadGameData();

            Resume();

            // Setup Stopwatch
            stopwatchManager.ResetStopwatch();
            stopwatchManager.SetupStopwatchTik(1, () => ModifyScore((int)stopwatchManager.GetStopwatchValue()/7 + 1));
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

            bulletColorMapping = new Dictionary<string, Color>()
            {
                { "BulletPrefab1", colors[0] },
                { "BulletPrefab2", colors[1] },
                { "BulletPrefab3", colors[2] },
            };
        }

        void LoadGameData()
        {
            TanmakGameData data = DataManager.LoadJSON<TanmakGameData>("tanmak_save_data");
            if (data != null)
            {
                highScore = data.highScore;
            }
        }

        void SaveGameData()
        {
            TanmakGameData data = new TanmakGameData();
            data.highScore = Mathf.Max(highScore, scoreManager.GetScore());

            DataManager.SaveJSON<TanmakGameData>(data, "tanmak_save_data");
        }

        protected override void AfterPauseUpdate()
        {
            // Set Timer UI
            TUIManager.SetTimerText(stopwatchManager.GetStopwatchValue());
        }
    }
}