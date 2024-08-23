using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class BrainGameManager : StageManager
{
    public static BrainGameManager game;
    // public GameObject problemGenerator;
    public GameObject wrongPopup;
    public bool playable = false;
    public bool isCleared = false;
    bool pause = false;
    public Color now_color;

    public GameObject scoreboard;
    public GameObject pausepanel;
    public GameObject questionboard;

    public event Action new_problem;
    public event Action stop_problem;
    public event Action pause_problem;
    public event Action difficulty_increase;
    const int TOTAL = 10;

    public int remain;
    int score = 0;
    int add = 0;
    int bonus = 0;
    bool wrong = false;
    public GameObject timer;
    public float timeLimit = 5.0f;
    public float stageTimeLimit;
    public Button buttonA;
    public Button buttonB;
    public Button buttonC; 

    public List<Button> buttons;
    public List<GameObject> rings;

    public GameObject startPopup;

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
        // stageTimeLimits = new float[10] { 4f, 4f, 5f, 5f, 5f, 7f, 7f, 7f, 10f, 10f };
        stageTimeLimit = 3.0f;
        // stageScores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        buttons = new List<Button>() { buttonA, buttonB, buttonC };

        for (int i = 0; i < buttons.Count; i++) {
            Button button = buttons[i];
            button.onClick.AddListener(() => PickColor(button));
            button.interactable = false;
        }

        isCleared = false;

        isStageActive = false;
        playable = false;
        DeactivateButton();
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

        // float stageTimeLimit = stageTimeLimits[currentStage];
        stageTimer.SetupTimer(stageTimeLimit, EndStage);

        isStageActive = true;

        questionboard.GetComponent<TextMeshProUGUI>().text = "STAGE" + (currentStage + 1).ToString();

        this.score = 0;
        new_problem();
    }

    protected override void EndStage()
    {
        isStageActive = false;
        playable = false;
        DeactivateButton();

        stageTimer.PauseTimer();
        stageTimer.PauseTimer();
    
        stop_problem();

        if (isCleared)
        {
            Debug.Log("Stage Cleared");
            CalculateFinalScore();
            isCleared = false;
        }
        else
        {
            this.wrong = true;
        }
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
            // stop_problem();
            {
                StopAllCoroutines();
                GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject x in obj)
                {
                    x.GetComponent<Button>().interactable = false;
                }
            }
            stageTimer.PauseTimer();
            Invoke("HideMessage", 0.5f);
            StartCoroutine(delayedEndStage(0.6f));
            this.wrong = true;
        }
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.remain<=0)
        {
            isCleared = true;
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
        int scoreToAdd = Mathf.FloorToInt(remainingTime) * 5;
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
    public void PickColor(Button button)
    {
        # if UNITY_ANDROID && !UNITY_EDITOR
            Vibration.Vibrate(30);
        # elif UNITY_IOS && !UNITY_EDITOR
            Vibrate(1519);
        # endif

        Color color = button.colors.normalColor;
        now_color = color;

        int idx = buttons.IndexOf(button);
        RingHandler(idx);
    }

    public void RingHandler(int idx)
    {
        for (int i = 0; i < rings.Count; i++)
            {
                if (i == idx)
                {
                    rings[i].SetActive(true);
                }
                else
                {
                    rings[i].SetActive(false);
                }
            }
    }
    

    public void ActivateButton()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void DeactivateButton()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
            rings[i].SetActive(false);
        }
    }
}
