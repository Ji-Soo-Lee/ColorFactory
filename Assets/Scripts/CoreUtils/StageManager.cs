using System;
using UnityEngine;


public class StageManager : MonoBehaviour
{
    public int totalStages = 5;             // The number of Stages
    public float[] stageTimeLimits;         // Time limit for each stage
    public int[] stageScores;               // Current score for each stage

    private int currentStage = 0;           // Index of current stage
    protected TimerManager timerManager;      // Timer Manager
    protected ScoreManager scoreManager;      // Score Manager
    private bool isStageActive = false;     // Status of stage (active or not)

    protected virtual void Start()
    {
        if (stageTimeLimits.Length != totalStages || stageScores.Length != totalStages)
        {
            Debug.LogError("stageTimeLimits / stageScores array size error.");
            return;
        }

        InitializeTimerManager();
        InitializeScoreManager();

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
            // Stage-specific update logic if needed
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
        
        scoreManager.Initialize();
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
