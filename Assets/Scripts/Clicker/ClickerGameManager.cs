using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;

public class ClickerGameManager : MonoBehaviour
{
    public ClickerUIManager clickerUIManager;
    public GameObject resetButton;
    public ClickEffect clickEffect;
    public RewardEffect rewardEffect;
    public RobotManager robotManager;
    public Fever feverManager;
    public DummySceneController dummySceneController;

    [field: SerializeField] public int clickNum { get; private set; } = 0;
    public int currentClickNum { get; private set; } = 0;
    public int maxClicks { get; private set; } = 24;
    public int maxCycle { get; private set; } = 4;

    public List<float> targetAlphas;   // target Alpahs
    public List<float> currentAlphas;  // current Alphas
    public List<Color> buttonColors { get; private set; } = new List<Color>();  // Available Colors for Button
    [SerializeField] public List<Color> worldColors;    // World Colors

    public float colorThreshold { get; private set; }

    private int buttonColorLength;
    private int feverWeight = 1;
    private bool isCoroutineClicking = false;
    private Coroutine clickCoroutine = null;

    [DllImport("__Internal")]
    public static extern void Vibrate(int _n);

    void Awake()
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

    void OnApplicationQuit()
    {
        SaveGameData();
    }

    //void OnApplicationFocus(bool paused)
    //{
    //    SaveGameData();
    //}

    void OnApplicationPause(bool paused)
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
        Color targetColor = CalculateTargetColor();
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

    public void InitClickCoroutine()
    {
        isCoroutineClicking = false;
        if (clickCoroutine != null)
        {
            StopCoroutine(clickCoroutine);
        }
        clickCoroutine = null;
    }

    public void StartMultipleClicks(int clickNum, float duration = 5.0f, Action handler = null)
    {
        if (!isCoroutineClicking)
        {
            isCoroutineClicking = true;
            Action action = () =>
            {
                InitClickCoroutine();
                if (handler != null)
                {
                    handler();
                }
            };
            clickCoroutine = StartCoroutine(
                SimulateMultipleClicks(clickNum, duration, action)
            );
        }
        else
        {
            UnityEngine.Debug.LogWarning("Clicking Coroutine Already Running!");
        }
    }

    public Color CalculateTargetColor()
    {
        if (buttonColors.Count < 1)
        {
            UnityEngine.Debug.LogWarning("Button Colors Empty");
            return clickerUIManager.currentColor;
        }

        int curIdx = (int)Mathf.Floor(clickNum / colorThreshold);
        Color targetColor = Color.Lerp(
            buttonColors[curIdx % buttonColors.Count],
            buttonColors[(curIdx + 1) % buttonColors.Count],
            ((float)(clickNum % colorThreshold)) / colorThreshold
        );

        return targetColor;
    }

    private void InitColors()
    {
        targetAlphas.Clear();
        currentAlphas.Clear();
        buttonColors.Clear();

        // For OOB Safety
        if (worldColors.Count != clickerUIManager.backgrounds.Count)
        {
            UnityEngine.Debug.LogWarning("World Colors and Background Colors Mismatch!");
            return;
        }
        else if (worldColors.Count == 0)
        {
            UnityEngine.Debug.LogWarning("World Colors Empty!");
            return;
        }

        // Set Alphas
        for (int i = 0; i < clickerUIManager.backgrounds.Count; i++)
        {
            targetAlphas.Add(clickerUIManager.backgrounds[i].color.a);
            currentAlphas.Add(clickerUIManager.backgrounds[i].color.a / maxCycle);
        }

        // Distribute World Colors
        for (int i = 0; i < currentAlphas.Count; i++)
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
        ClickerData data = new ClickerData(
            new ClickerStateData(clickNum, currentClickNum, feverManager.feverGauge),
            new ClickerColorData(clickerUIManager.currentColor, buttonColors),
            new ClickerRobotData(robotManager.GetClickAmounts(), robotManager.GetMaxClicks()),
            null);
        ClickerDataManager.SaveData(data);
    }

    public void ResetGameData()
    {
        // Reset
        InitColors();
        clickerUIManager.InitColors();

        clickNum = 0;
        currentClickNum = 0;

        // Save Data
        SaveGameData();

        // resetButton.SetActive(false);

        // Activate Buttons
        robotManager.SetAllRobotsInteractable(true);
        clickerUIManager.mainButton.gameObject.SetActive(true);
        clickerUIManager.backgroundButtonSprite.gameObject.SetActive(true);
    }

    private void LoadGameData(ClickerData data)
    {
        // Apply on States
        clickNum = data.stateData.clickNum;
        currentClickNum = data.stateData.currentClickNum;
        buttonColors = data.colorData.buttonColors;

        feverManager.SetFeverGauge(data.stateData.feverGauge);

        // Apply on Sprites
        clickerUIManager.SetButtonColor(data.colorData.currentColor);
        for (int i = 0; i < buttonColors.Count; i++)
        {
            Color c = buttonColors[i];
            clickerUIManager.backgrounds[i].color = new Color(c.r, c.g, c.b, c.a);
        }

        // Apply on Robots
        robotManager.SetClickAmounts(data.robotData.robotClickAmounts);
        robotManager.SetMaxClicks(data.robotData.robotMaxClicks);
    }

    private void GetReward()
    {
        int rewardIdx = -1;
        List<int> validIndices = new List<int>();

        if (buttonColors.Count != targetAlphas.Count)
        {
            UnityEngine.Debug.LogWarning("Button Colors and Target Alphas Length Mismatch");
            return;
        }

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
            rewardIdx = validIndices[UnityEngine.Random.Range(0, validIndices.Count)];

            UnityEngine.Debug.Log(rewardIdx);

            currentAlphas[rewardIdx] = buttonColors[rewardIdx].a + targetAlphas[rewardIdx] / maxCycle;
            if (currentAlphas[rewardIdx] > 1.0f)
            {
                currentAlphas[rewardIdx] = 1.0f;
            }
            
            // Update button 
            buttonColors[rewardIdx] = new Color(buttonColors[rewardIdx].r, buttonColors[rewardIdx].g, buttonColors[rewardIdx].b, currentAlphas[rewardIdx]);;
            
            // Reward Effect
            rewardEffect.DisplayRewardEffect(buttonColors[rewardIdx]);

            // Update background
            // Color backgroundColor = clickerUIManager.backgrounds[rewardIdx].color;
            // backgroundColor.a = currentAlphas[rewardIdx];
            clickerUIManager.backgrounds[rewardIdx].color = new Color(1.0f, 1.0f, 1.0f, currentAlphas[rewardIdx]);
        }

        if (Enumerable.SequenceEqual(targetAlphas, currentAlphas))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // End Logic
        UnityEngine.Debug.Log("You win!");

        InitClickCoroutine();

        robotManager.SetAllRobotsInteractable(false);
        clickerUIManager.mainButton.gameObject.SetActive(false);
        clickerUIManager.backgroundButtonSprite.gameObject.SetActive(false);

        resetButton.SetActive(true);
    }

    private IEnumerator SimulateMultipleClicks(int clickNum, float duration = 5.0f, Action handler = null)
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

        if (handler != null)
        {
            handler();
        }

        // isClickCoroutineRunning = false;
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(clickEffect.growDuration);
    }
}
