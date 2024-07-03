using System;
using UnityEngine;


public class StageManager : MonoBehaviour
{
    public int totalStages = 5;             // The number of Stages
    public float[] stageTimeLimits;         // Time limit for each stage
    public int[] stageScores;               // Current score for each stage

    private int currentStage = 0;           // Index of current stage
    private float stageStartTime;           // Start time for stage
    private bool isStageActive = false;     // Status of stage (active or not)

    protected virtual void Start()
    {
        if (stageTimeLimits.Length != totalStages || stageScores.Length != totalStages)
        {
            Debug.LogError("stageTimeLimits / stageScores array size error.");
            return;
        }

        StartStage(0);  // Start first stage
    }

    protected virtual void Update()
    {
        if (isStageActive)
        {
            // Update Stage Timer
            float elapsedTime = Time.time - stageStartTime;
            if (elapsedTime >= stageTimeLimits[currentStage])
            {
                EndStage();
            }
        }
    }

    protected virtual void StartStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= totalStages)
        {
            Debug.LogError("Stage index error.");
            return;
        }

        currentStage = stageIndex;
        stageStartTime = Time.time;
        isStageActive = true;

        // initialize
        InitializeGameElements();

        Debug.Log("Stage " + (currentStage + 1) + "started.");
    }

    protected virtual void EndStage()
    {
        isStageActive = false;

        // Score Calculation
        int score = CalculateScore();
        Debug.Log("Stage " + (currentStage + 1) + "ended. Score: " + score);

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

    protected virtual int CalculateScore()
    {
        // Score Calculation logic (ex: Remaining time, Color sync rate etc.)
        return stageScores[currentStage];
    }

    protected virtual void EndGame()
    {
        Debug.Log("Game completed.");
        // Game termination logic (ex: Show result page, Move to main menu etc.)
    }
}
