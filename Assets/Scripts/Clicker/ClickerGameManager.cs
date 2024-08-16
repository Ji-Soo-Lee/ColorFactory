using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGameManager : MonoBehaviour
{
    public ClickerUIManager clickerUIManager;
    public RobotManager robotManager;
    public Fever feverManager;
    public DummySceneController dummySceneController;

    public int clickNum { get; private set; } = 0;
    public int currentClickNum { get; private set; } = 0;
    public int maxClicks { get; private set; } = 24;
    public int maxCycle { get; private set; } = 4;

    public List<Color> worldColors;   // World Color
    public List<Color> buttonColors { get; private set; } = new List<Color>();  // Available Colors for Button

    private int buttonColorLength;
    private float colorThreshold;
    private int feverWeight = 1;

    void Start()
    {
        InitColors();

        clickerUIManager.InitColors();

        // Load Data
        ClickerData data = ClickerDataManager.LoadData();
        if (data != null)
        {
            LoadGameData(data);
        }

        // Check MiniGame Result
        ApplyMiniGameResult();
    }

    void OnDisable()
    {
        SaveGameData();
    }

    public void SetFeverWeight(int weight)
    {
        if (feverWeight < 1) return;
        feverWeight = weight;
    }

    public void IncrementClickCount()
    {
        int inc = (int)(1 + 0.1 * ((feverWeight << 1) + feverWeight));
        clickNum += inc;
        currentClickNum += inc;

        if (currentClickNum >= maxClicks)
        {
            currentClickNum -= maxClicks;
            GetReward();
        }
    }

    public IEnumerator SimulateMultipleClicks(int clickNum, float duration)
    {
        // Spinlock if preceding coroutine exists
        // Cannot guarantee the order if blocked
        // while (currentClickCoroutine != null) yield return currentClickCoroutine;
        // isClickCoroutineRunning = true;

        float stepColorTransitionDuration = duration / clickNum;

        clickerUIManager.mainButton.SetInteractive(false);

        for (int i = 0; i < clickNum; i++)
        {
            IncrementClickCount();

            clickerUIManager.UpdateButtonColor(duration);

            if (i != clickNum - 1)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }

        clickerUIManager.mainButton.SetInteractive(true);

        // isClickCoroutineRunning = false;
    }

    private void InitColors()
    {
        // Distribute World Colors
        for (int i = 0; i < worldColors.Count; i++)
        {
            Color color = new Color(worldColors[i].r, worldColors[i].g, worldColors[i].b, worldColors[i].a / maxCycle);
            buttonColors.Add(color);

            clickerUIManager.backgrounds[i].color = color;
        }

        // Set Constant
        buttonColorLength = buttonColors.Count;
        colorThreshold = maxClicks / buttonColorLength;
    }

    private void ApplyMiniGameResult()
    {
        ScoreDataManager scoreDataManager = GameObject.FindObjectOfType<ScoreDataManager>();
        if (scoreDataManager != null)
        {
            int score = scoreDataManager.finalMiniGameScore;
            string sceneName = scoreDataManager.resultSceneName;

            int robotIdx = 0;
            float weight = 1.0f;
            
            // Set Weight & Robot with Scene Name
            if (dummySceneController.sceneNames.Contains(sceneName))
            {
                weight = 0.5f;
                robotIdx = dummySceneController.sceneNames.FindIndex(x => x.Equals(sceneName)) - 1;
            }

            // Weight Score
            int weightedScore = (int)(score * weight);

            // Add Robot Clicks
            robotManager.robots[robotIdx].AddClickAmount(weightedScore);

            // Add Fever Gauge
            if (scoreDataManager.isMiniGameClear)
            {
                feverManager.AddFeverGauge(1);
            }
        }
    }

    private void SaveGameData()
    {
        // Save Data
        ClickerData data = ClickerDataManager.CreateClickerData(clickNum, currentClickNum,
            feverManager.feverGauge, clickerUIManager.currentColor,
            buttonColors, robotManager.GetClickAmounts(), robotManager.GetMaxClicks());
        ClickerDataManager.SaveData(data);
    }

    private void LoadGameData(ClickerData data)
    {
        clickNum = data.clickNum;
        currentClickNum = data.currentClickNum;
        buttonColors = data.buttonColors;

        feverManager.SetFeverGauge(data.feverGauge);

        // Apply on Sprites
        clickerUIManager.SetButtonColor(data.currentColor);
        for (int i = 0; i < buttonColors.Count; i++)
        {
            Color c = buttonColors[i];
            clickerUIManager.backgrounds[i].color = new Color(c.r, c.g, c.b, c.a);
        }

        // Apply on Robots
        robotManager.SetClickAmounts(data.robotClickAmounts);
        robotManager.SetMaxClicks(data.robotMaxClicks);
    }

    private void GetReward()
    {
        int rewardIdx = -1;
        List<int> validIndices = new List<int>();

        // Get Valid Indices
        for (int i = 0; i < buttonColorLength; i++)
        {
            if (buttonColors[i].a != worldColors[i].a)
            {
                validIndices.Add(i);
            }
        }

        if (validIndices.Count > 0)
        {
            // Get Random Reward
            rewardIdx = validIndices[Random.Range(0, validIndices.Count)];

            UnityEngine.Debug.Log(rewardIdx);

            Color color = new Color(buttonColors[rewardIdx].r, buttonColors[rewardIdx].g, buttonColors[rewardIdx].b, buttonColors[rewardIdx].a + worldColors[rewardIdx].a / maxCycle);
            buttonColors[rewardIdx] = color;
            clickerUIManager.backgrounds[rewardIdx].color = color;
        }

        if (Enumerable.SequenceEqual(buttonColors, worldColors))
        {
            // End Logic
            UnityEngine.Debug.Log("You win!");
            robotManager.SetAllRobotsInteractable(false);
        }
    }
}