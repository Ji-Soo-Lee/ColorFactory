using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MixStageManager : MonoBehaviour
{
    public int totalStages = 5;             // The number of Stages
    public float[] stageTimeLimits = new float[5] { 10f, 10f, 20f, 20f, 30f };         // Time limit for each stage
    public int[] stageScores = new int[5] { 100, 200, 300, 400, 500 };               // Current score for each stage

    private int currentStage = 0;           // Index of current stage
    protected TimerManager timerManager;      // Timer Manager
    protected ScoreManager scoreManager;      // Score Manager
    private bool isStageActive = false;     // Status of stage (active or not)

    private AnswerToken answerToken;
    private PaletteResult paletteResult;
    public Button btn1;
    public Color targetColor, resultColor;
    public TMP_Text timeText, scoreText;

    protected virtual void Start()
    {
        if (stageTimeLimits.Length != totalStages || stageScores.Length != totalStages)
        {
            Debug.LogError("stageTimeLimits / stageScores array size error.");
            return;
        }

        InitializeTimerManager();
        InitializeScoreManager();

        answerToken = GameObject.Find("TargetImage").GetComponent<AnswerToken>();
        paletteResult = GameObject.Find("ResultImage").GetComponent<PaletteResult>();
        btn1 = GameObject.Find("SubmissionButton").GetComponent<Button>();
        btn1.onClick.AddListener(() =>
        {
            targetColor = GameObject.Find("TargetImage").GetComponent<Image>().color;
            resultColor = GameObject.Find("ResultImage").GetComponent<Image>().color;
            if (ColorUtils.CompareColor(targetColor, resultColor))
            {
                scoreManager.AddScore(stageScores[currentStage]);
                EndStage();
                Debug.Log("Correct");
            }
            else
            {
                Debug.Log("Wrong");
            }
        });

        StartStage(0);  // Start first stage
    }

    protected virtual void InitializeScoreManager()
    {
        scoreManager = gameObject.AddComponent<ScoreManager>();
    }

    protected virtual void InitializeTimerManager()
    {
        timerManager = gameObject.AddComponent<TimerManager>();
    }

    protected virtual void Update()
    {
        // Check if a stage is currently active

        if (isStageActive)
        {
            timeText.text = timerManager.GetTimerValue().ToString();
        }

        // General update logic if needed
    }

    protected virtual void StartStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= totalStages)
        {
            Debug.LogError("Stage index error.");
            return;
        }

        currentStage = stageIndex;

        // Timer Setup
        float stageTimeLimit = stageTimeLimits[currentStage];
        timerManager.SetupTimer(stageTimeLimit, EndStage);

        isStageActive = true;

        // Initialize
        InitializeGameElements();

        timerManager.StartTimer();

        Debug.Log("Stage " + (currentStage + 1) + "started.");
    }

    protected virtual void EndStage()
    {
        isStageActive = false;
        timerManager.PauseTimer();

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

    protected virtual void InitializeGameElements()
    {
        // Initialize logic (ex: Player location initialize, Color initialize etc.)
        paletteResult.SetPaletteColor(currentStage);
        paletteResult.SetOnClickAction(paletteResult.mixType);
        answerToken.SetTargetColor(currentStage);
        paletteResult.ClickReset();
    }

    protected virtual void CalculateFinalScore()
    {
        // Score Calculation logic (ex: Remaining time, Color sync rate etc.)
        float remainingTime = timerManager.GetTimerValue();
        scoreManager.OnStageClear(remainingTime);
    }

    protected virtual void EndGame()
    {
        Debug.Log("Game completed.");
        // Game termination logic (ex: Show result page, Move to main menu etc.)
    }
}
