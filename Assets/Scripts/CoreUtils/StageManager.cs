using System;
using UnityEngine;


public class StageManager : PausableMonoBehaviour
{
    public int totalStages = 5;             // The number of Stages
    public float[] stageTimeLimits;         // Time limit for each stage
    public int[] stageScores;               // Current score for each stage

    public int currentStage = 0;           // Index of current stage
    protected bool isStageActive = false;     // Status of stage (active or not)

    public GameObject ResultPannel;
    [HideInInspector] public ScoreManager scoreManager;
    [HideInInspector] public ScoreDataManager scoreDataManager;
    [HideInInspector] public TimerManager stageTimer;
    [HideInInspector] public StopwatchManager stageStopwatch;

    protected virtual void Awake()
    {
        InitializeStageTimer();
        InitializeStageStopwatch();
        InitializeScoreManager();
        InitializeScoreDataManager();
    }

    protected virtual void Start()
    {
        if (stageTimeLimits == null || stageScores == null ||
            stageTimeLimits.Length != totalStages || stageScores.Length != totalStages)
        {
            Debug.LogWarning("stageTimeLimits / stageScores array size error.");
            return;
        }

        StartStage(0);  // Start first stage
    }

    protected virtual void InitializeScoreManager()
    {
        if (gameObject.GetComponent<ScoreManager>() == null)
        {
            gameObject.AddComponent<ScoreManager>();
        }
        scoreManager = gameObject.GetComponent<ScoreManager>();
    }

    protected virtual void InitializeStageTimer()
    {
        if (gameObject.GetComponent<TimerManager>() == null)
        {
            gameObject.AddComponent<TimerManager>();
        }
        stageTimer = gameObject.GetComponent<TimerManager>();
    }

    protected virtual void InitializeStageStopwatch()
    {
        if (gameObject.GetComponent<StopwatchManager>() == null)
        {
            gameObject.AddComponent<StopwatchManager>();
        }
        stageStopwatch = gameObject.GetComponent<StopwatchManager>();
    }

    protected virtual void InitializeScoreDataManager()
    {
        GameObject obj = new GameObject("Score Data Manager Object");
        obj.AddComponent<ScoreDataManager>();
        scoreDataManager = obj.GetComponent<ScoreDataManager>();
    }

    protected override void Update()
    {
        base.Update();

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
        stageTimer.SetupTimer(stageTimeLimit, EndStage);

        isStageActive = true;

        // Initialize
        InitializeGameElements();
        
        scoreManager.Initialize();
        stageTimer.StartTimer();

        Debug.Log("Stage " + (currentStage + 1) + "started.");
    }

    protected virtual void EndStage()
    {
        isStageActive = false;
        stageTimer.PauseTimer();

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
        float remainingTime = stageTimer.GetTimerValue();
        scoreManager.OnStageClear(remainingTime);
    }

    public virtual void EndGame()
    {
        Debug.Log("Game completed.");
        // Game termination logic (ex: Show result page, Move to main menu etc.)

        // Save Score & Activate End Game Panel
        ScoreDataManager.Inst.SaveResult(scoreManager.GetScore());
        ResultPannel.SetActive(true);
    }
}
