using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class MixStageManager : StageManager
{
    private AnswerToken answerToken;
    private PaletteResult paletteResult;
    public Button btn1;
    public Color targetColor, resultColor;
    public TMP_Text timeText, scoreText, stageText;
    public GameObject ob1;

    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void Vibrate(long _n);
    # endif

    protected override void Awake()
    {
        base.Awake();

        totalStages = 5;
        stageTimeLimits = new float[5] { 10f, 10f, 20f, 20f, 30f };
        stageScores = new int[5] { 1, 2, 3, 4, 5 };

        currentStage = 0;
        isStageActive = false;

        Debug.Log("Total Score : " + scoreManager.GetScoreAsString());
    }

    protected override void Start()
    {
        if (stageTimeLimits.Length != totalStages || stageScores.Length != totalStages)
        {
            Debug.LogError("stageTimeLimits / stageScores array size error.");
            return;
        }

        InitializeStageTimer();
        InitializeScoreManager();

        answerToken = GameObject.Find("TargetImage").GetComponent<AnswerToken>();
        paletteResult = GameObject.Find("ResultImage").GetComponent<PaletteResult>();
        btn1 = GameObject.Find("SubmissionButton").GetComponent<Button>();
        //submit button
        btn1.onClick.AddListener(() =>
        {
            targetColor = GameObject.Find("TargetImage").GetComponent<Image>().color;
            resultColor = GameObject.Find("ResultImage").GetComponent<Image>().color;
            if (ColorUtils.CompareColor(targetColor, resultColor))
            {
                // Vibrate
                # if UNITY_ANDROID && !UNITY_EDITOR
                    Vibration.Vibrate(30);
                # elif UNITY_IOS && !UNITY_EDITOR
                    Vibrate(1519);
                # endif
                scoreManager.AddScore(stageScores[currentStage]);
                Debug.Log("Add Score: " + stageScores[currentStage]);
                EndStage();
                Debug.Log("Correct");
            }
            else
            {
                // Vibrate
                # if UNITY_ANDROID && !UNITY_EDITOR
                    Vibration.Vibrate(60);
                # elif UNITY_IOS && !UNITY_EDITOR
                    Vibrate(1521);
                # endif
                Debug.Log("Wrong");
                ob1.SetActive(true);
                Invoke("HideMessage", 1.5f);
            }
        });

        StartStage(0);  // Start first stage
        scoreText.text = "0";
    }

    protected override void Update()
    {
        // Check if a stage is currently active

        if (isStageActive)
        {
            timeText.text = (Mathf.Ceil(stageTimer.GetTimerValue()*10)/10).ToString("F1");
        }

        // General update logic if needed
    }

    protected override void StartStage(int stageIndex)
    {
        currentStage = stageIndex;

        // Timer Setup
        float stageTimeLimit = stageTimeLimits[currentStage];
        stageTimer.SetupTimer(stageTimeLimit, EndStage);

        isStageActive = true;
        paletteResult.isMixable = true;

        // Initialize
        InitializeGameElements();

        stageTimer.StartTimer();
        stageText.text = "STAGE"+(currentStage+1).ToString();

        Debug.Log("STAGE" + (currentStage + 1) + "started.");
    }

    protected override void EndStage()
    {
        isStageActive = false;
        paletteResult.isMixable = false;
        stageTimer.PauseTimer();
        stageTimer.PauseTimer();

        // Score Calculation
        CalculateFinalScore();
        Debug.Log("Stage " + (currentStage + 1) + "ended.");
        Debug.Log("Total Score: " + scoreManager.GetScoreAsString());
        scoreText.text = scoreManager.GetScoreAsString();

        if (currentStage < totalStages - 1)
        {
            // Go to next stage
            StartStage(currentStage + 1);
        }
        else
        {
            // Terminate game
            EndGame();
        }
    }

    protected override void CalculateFinalScore()
    {
        // Score Calculation logic (ex: Remaining time, Color sync rate etc.)
        Debug.Log(stageTimer.GetTimerValue());
        float remainingTime = stageTimer.GetTimerValue();
        Debug.Log("Scaled Time: " + remainingTime);
        OnStageClear(remainingTime);
    }

    public void OnStageClear(float remainingTime)
    {
        // Score increasing logic after stage clear
        // ex : increase score in porportion to remaining time
        int scoreToAdd = Mathf.FloorToInt(remainingTime) * 5;
        scoreManager.AddScore(scoreToAdd);
    }

    protected override void InitializeGameElements()
    {
        // Initialize logic (ex: Player location initialize, Color initialize etc.)
        paletteResult.SetPaletteColor(currentStage);
        paletteResult.SetOnClickAction(paletteResult.mixType);
        answerToken.SetTargetColor(currentStage);
        paletteResult.ClickReset();
    }

    protected void HideMessage()
    {
        ob1.SetActive(false);
    }
}
