using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class BrainGameManager : StageManager
{
    public static BrainGameManager game;
    public GameObject wrongPopup;
    public bool playable = false;
    bool pause = false;
    public Color now_color;

    public GameObject scoreboard;
    public GameObject pausepanel;
    public GameObject questionboard;

    public event Action new_problem;
    public event Action stop_problem;
    public event Action difficulty_increase;
    const int TOTAL = 10;

    public int remain;
    int score = 0;
    int add = 0;
    int bonus = 0;
    bool wrong = false;
    public GameObject timer;
    public float timeLimit = 5.0f;

    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void Vibrate(long _n);
    # endif

    protected override void Awake()
    {
        base.Awake();

        if(game == null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentStage = 0;
        stageTimeLimits = new float[10] { 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f };
        stageScores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        isStageActive = false;
    }
    protected override void Start()
    {
        base.Start();

        InitializeStageTimer();
        InitializeScoreManager();

        StartStage(0);
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = "0";
    }
    protected override void Update() {
        if (isStageActive)
        {
            this.timer.GetComponent<TextMeshProUGUI>().text = (Mathf.Ceil(stageTimer.GetTimerValue()*10)/10).ToString("F1");
        }
    }

    protected override void StartStage(int stageIndex)
    {
        currentStage = stageIndex;

        float stageTimeLimit = 5.0f;
        stageTimer.SetupTimer(stageTimeLimit, EndStage);

        isStageActive = true;

        questionboard.GetComponent<TextMeshProUGUI>().text = "STAGE" + (currentStage + 1).ToString();

        this.score = 0;
        new_problem();
    }

    protected override void EndStage()
    {
        isStageActive = false;
        stageTimer.PauseTimer();
        stageTimer.PauseTimer();
    
        stop_problem();
        CalculateFinalScore();
        scoreboard.GetComponent<TextMeshProUGUI>().text = scoreManager.GetScoreAsString();

        if (currentStage < totalStages - 1)
        {
            if (!this.wrong)
            {
                difficulty_increase();
            }
            this.wrong = false;
            StartStage(currentStage + 1);
        }
        else
        {
            EndGame();
        }
    }

    public void verdict(bool correct)
    {
        this.remain -= 1;
        if(correct)
        {
            # if UNITY_ANDROID && !UNITY_EDITOR
                Vibration.Vibrate(30);
            # elif UNITY_IOS && !UNITY_EDITOR
                Vibrate(1519);
            # endif
            
            this.score += 1;
        }
        else
        {
            # if UNITY_ANDROID && !UNITY_EDITOR
                Vibration.Vibrate(60);
            # elif UNITY_IOS && !UNITY_EDITOR
                Vibrate(1521);
            # endif

            wrongPopup.SetActive(true);
            stageTimer.PauseTimer();
            Invoke("HideMessage", 0.5f);
            StartCoroutine(delayedEndStage(0.6f));
            this.wrong = true;
        }
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.remain<=0)
        {
            EndStage();
        }
    }

    public void toggle_player(bool toggle)
    {
        this.playable = toggle;
    }
    public void toggle_pause()
    {
        this.pause = !(this.pause);
        this.pausepanel.SetActive(this.pause);
        toggle_player(false);
        stop_problem();
        if(this.pause==false)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {
                Destroy(x);
            }
            new_problem();
        }
    }

    protected override void CalculateFinalScore()
    {
        // Score Calculation logic (ex: Remaining time, Color sync rate etc.)
        float remainingTime = stageTimer.GetTimerValue();
        OnStageClear(remainingTime);
    }

    public void OnStageClear(float remainingTime)
    {
        // Score increasing logic after stage clear
        // ex : increase score in porportion to remaining time
        int scoreToAdd = Mathf.FloorToInt(remainingTime) * 3;
        scoreManager.AddScore(scoreToAdd);
    }

    
    protected void HideMessage()
    {
        wrongPopup.SetActive(false);
    }
    IEnumerator delayedEndStage(float time)
    {
        yield return new WaitForSeconds(time);
        EndStage();
    }
}
