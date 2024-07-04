using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    protected int currentScore;      // Current Score

    protected virtual void Start()
    {
        // Initialize Current Score
        Initialize();
    }

    // Initialize Score
    public virtual void Initialize()
    {
        currentScore = 0;
    }

    // Increase Score
    public virtual void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
    }

    // Decrease Score
    public virtual void SubtractScore(int scoreToSubtract)
    {
        currentScore -= scoreToSubtract;
        if (currentScore < 0)
        {
            currentScore = 0;  // Score should be non-negative
        }
    }

    // Increase Score by In-game Elements
    public virtual void CalculateScoreFromGameElements()
    {
        // Score increasing logic
        // ex : pop single line -> increase score
    }

    // Increase Extra Score on Stage Clear
    public virtual void OnStageClear(float remainingTime)
    {
        // Score increasing logic after stage clear
        // ex : increase score in porportion to remaining time
        int scoreToAdd = Mathf.FloorToInt(remainingTime) * 10;
        AddScore(scoreToAdd);
    }

    // Get Score in String
    public string GetScoreAsString()
    {
        return currentScore.ToString();
    }
}
