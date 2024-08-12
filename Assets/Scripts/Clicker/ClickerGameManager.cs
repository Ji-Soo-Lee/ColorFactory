using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGameManager : MonoBehaviour
{
    public ClickerUIManager clickerUIManager;
    public RobotManager robotManager;

    public int clickNum;
    public int currentClickNum;
    public int maxClicks { get; private set; } = 24;
    public int maxCycle { get; private set; } = 4;

    public List<Color> worldColors;   // World Color
    public List<Color> buttonColors { get; private set; } = new List<Color>();  // Available Colors for Button

    private int buttonColorLength;
    private float colorThreshold;

    public void IncrementClickCount()
    {
        clickNum++;
        currentClickNum++;

        if (currentClickNum >= maxClicks)
        {
            currentClickNum -= maxClicks;
            GetReward();
        }
    }

    void Start()
    {
        clickNum = 0;

        InitColors();

        clickerUIManager.InitColors();

        // Load Data
        ClickerData data = ClickerDataManager.LoadData();
        if (data != null)
        {
            LoadGameData(data);
        }
    }

    void OnDisable()
    {
        // Save Data
        ClickerData data = ClickerDataManager.CreateClickerData(clickNum, currentClickNum, clickerUIManager.currentColor, buttonColors);
        ClickerDataManager.SaveData(data);
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

    private void LoadGameData(ClickerData data)
    {
        clickNum = data.clickNum;
        currentClickNum = data.currentClickNum;
        buttonColors = data.buttonColors;

        // Apply on Sprites
        clickerUIManager.SetButtonColor(data.currentColor);
        for (int i = 0; i < buttonColors.Count; i++)
        {
            Color c = buttonColors[i];
            clickerUIManager.backgrounds[i].color = new Color(c.r, c.g, c.b, c.a);
        }
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