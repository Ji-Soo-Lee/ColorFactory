using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickerGameManager : MonoBehaviour
{
    public ClickerUIManager clickerUIManager;
    public GameObject resetButton;
    public ClickEffect clickEffect;
    public RobotManager robotManager;
    public Fever feverManager;
    public DummySceneController dummySceneController;

    public int clickNum { get; private set; } = 0;
    public int currentClickNum { get; private set; } = 0;
    public int maxClicks { get; private set; } = 24;
    public int maxCycle { get; private set; } = 4;

    public List<float> targetAlphas;   // target Alpahs
    public List<float> currentAlphas;  // current Alphas
    public List<Color> buttonColors { get; private set; } = new List<Color>();  // Available Colors for Button
    [SerializeField] public List<Color> worldColors;    // World Colors

    public float colorThreshold;
    
    private int buttonColorLength;
    private int feverWeight = 1;

    [DllImport("__Internal")]
    public static extern void Vibrate(int _n);

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

    public void IncrementClickCount(float duration = 1.0f)
    {
        int inc = (int)(1 + 0.1 * ((feverWeight << 1) + feverWeight));
        
        // Vibrate
        # if UNITY_ANDROID && !UNITY_EDITOR
            Vibration.Vibrate(30);
        # elif UNITY_IOS && !UNITY_EDITOR
            Vibrate(1519);
        # endif

        // Click Effect
        Color targetColor = clickerUIManager.CalculateTargetColor();
        clickerUIManager.UpdateButtonColor(duration);
        clickEffect.SpawnAndGrowEffect(targetColor);
        
        clickNum += inc;
        currentClickNum += inc;

        if (currentClickNum >= maxClicks)
        {
            currentClickNum -= maxClicks;
            GetReward();
        }
    }

    public IEnumerator SimulateMultipleClicks(int clickNum, float duration = 5.0f)
    {
        // Spinlock if preceding coroutine exists
        // Cannot guarantee the order if blocked
        // while (currentClickCoroutine != null) yield return currentClickCoroutine;
        // isClickCoroutineRunning = true;

        float stepColorTransitionDuration = duration / clickNum;

        clickerUIManager.mainButton.SetInteractive(false);

        for (int i = 0; i < clickNum; i++)
        {
            IncrementClickCount(stepColorTransitionDuration);

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
        targetAlphas.Clear();
        currentAlphas.Clear();
        buttonColors.Clear();

        for (int i = 0; i < worldColors.Count; i++)
        {
            targetAlphas.Add(clickerUIManager.backgrounds[i].color.a);
            currentAlphas.Add(clickerUIManager.backgrounds[i].color.a / maxCycle);
        }

        // Distribute World Colors
        for (int i = 0; i < targetAlphas.Count; i++)
        {
            Color color = new Color(worldColors[i].r, worldColors[i].g, worldColors[i].b, currentAlphas[i]);
            buttonColors.Add(color);

            clickerUIManager.backgrounds[i].color = new Color(1.0f, 1.0f, 1.0f, currentAlphas[i]);
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

    public void ResetGameData()
    {
        InitColors();
        clickerUIManager.InitColors();

        clickNum = 0;
        currentClickNum = 0;

        // Save Data
        ClickerData data = ClickerDataManager.CreateClickerData(clickNum, currentClickNum,
            feverManager.feverGauge, clickerUIManager.currentColor,
            buttonColors, robotManager.GetClickAmounts(), robotManager.GetMaxClicks());
        ClickerDataManager.SaveData(data);

        resetButton.SetActive(false);

        robotManager.SetAllRobotsInteractable(true);
        clickerUIManager.mainButton.gameObject.SetActive(true);
        clickerUIManager.backgroundButtonSprite.gameObject.SetActive(true);
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
            if (buttonColors[i].a != targetAlphas[i])
            {
                validIndices.Add(i);
            }
        }

        if (validIndices.Count > 0)
        {
            // Get Random Reward
            rewardIdx = validIndices[Random.Range(0, validIndices.Count)];

            UnityEngine.Debug.Log(rewardIdx);

            currentAlphas[rewardIdx] = buttonColors[rewardIdx].a + targetAlphas[rewardIdx] / maxCycle;
            if (currentAlphas[rewardIdx] > 1.0f)
            {
                currentAlphas[rewardIdx] = 1.0f;
            }
            
            // Update button 
            buttonColors[rewardIdx] = new Color(buttonColors[rewardIdx].r, buttonColors[rewardIdx].g, buttonColors[rewardIdx].b, currentAlphas[rewardIdx]);;
            
            // Update background
            // Color backgroundColor = clickerUIManager.backgrounds[rewardIdx].color;
            // backgroundColor.a = currentAlphas[rewardIdx];
            clickerUIManager.backgrounds[rewardIdx].color = new Color(1.0f, 1.0f, 1.0f, currentAlphas[rewardIdx]);
        }

        if (Enumerable.SequenceEqual(targetAlphas, currentAlphas))
        {
            // End Logic
            UnityEngine.Debug.Log("You win!");
            robotManager.SetAllRobotsInteractable(false);
            clickerUIManager.mainButton.gameObject.SetActive(false);
            clickerUIManager.backgroundButtonSprite.gameObject.SetActive(false);

            resetButton.SetActive(true);
        }
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(clickEffect.growDuration);
    }
}
