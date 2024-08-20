using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int clickNum;
    public Button clickButton;
    public Image buttonSprite;
    public Image backgroundSprite;
    public List<Robot> robots = new List<Robot>();
    public List<Image> backgrounds = new List<Image>();
    [SerializeField] private int maxClicks = 24;
    [SerializeField] private int maxCycle = 4;
    [SerializeField] private List<Color> worldColors = new List<Color>();
    // [SerializeField] private List<Sprite> buttonBackgrounds = new List<Sprite>();
    private List<Color> buttonColors = new List<Color>();
    private Color targetColor;
    private Color currentColor;
    [SerializeField] private float colorTransitionDuration = 1.0f; // Duration in seconds over which color change occurs
    [SerializeField] private float robotColorTransitionDuration = 5.0f;
    private int buttonColorLength;
    private float colorThreshold;
    private int currentClickNum;
    private Vector3 originalScale;
    // private Coroutine colorChangeCoroutine;

    void Awake()
    {   
        clickNum = 0;

        for (int i = 0; i < worldColors.Count; i++) {
            Color color = new Color(worldColors[i].r, worldColors[i].g, worldColors[i].b, worldColors[i].a / maxCycle);
            buttonColors.Add(color);
            backgrounds[i].color = color;
        }

        currentColor = buttonColors[0];
        targetColor = currentColor;
        buttonSprite = clickButton.GetComponent<Image>();

        buttonColorLength = buttonColors.Count;
        colorThreshold = maxClicks / buttonColorLength;

        buttonSprite.color = currentColor;
        clickButton.onClick.AddListener(OnClickButton);

        // backgroundSprite.sprite = buttonBackgrounds[0];

        originalScale = clickButton.transform.localScale;

        // Load Data
        ClickerData data = ClickerDataManager.LoadData();
        if (data != null)
        {
            //clickNum = data.clickNum;
            //currentClickNum = data.currentClickNum;
            //currentColor = data.currentColor;
            //buttonColors = data.buttonColors;

            // Apply on Sprites
            buttonSprite.color = currentColor;
            for (int i = 0;i < buttonColors.Count; i++)
            {
                Color c = buttonColors[i];
                backgrounds[i].color = new Color(c.r, c.g, c.b, c.a);
            }
        }
    }

    void OnClickButton()
    {
        clickNum++;
        currentClickNum++;
        
        int itv = (int) Mathf.Floor(clickNum / colorThreshold);
        targetColor = Color.Lerp(buttonColors[itv % buttonColorLength], buttonColors[(itv + 1) % buttonColorLength], ((float) (clickNum % colorThreshold)) / colorThreshold);
        
        StopCoroutine(ChangeColor(colorTransitionDuration));
        StartCoroutine(ChangeColor(colorTransitionDuration));

        if (currentClickNum >= maxClicks) {
            // Debug.Log(clickNum);
            currentClickNum -= maxClicks;
            GetReward();
        }

        // Continuously update the color to smoothly transition to the target color
        //currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime / colorTransitionDuration);
        //buttonSprite.color = currentColor;
    }

    // Slowly Change Color During transitionDuration
    IEnumerator ChangeColor(float transitionDuration)
    {
        Debug.Log("DEBUG");

        float elapsedTime = 0;
        Color startColor = buttonSprite.color;

        while (elapsedTime < transitionDuration)
        {
            currentColor = Color.Lerp(startColor, targetColor, elapsedTime / transitionDuration);
            buttonSprite.color = currentColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure Final Color
        currentColor = targetColor;
        buttonSprite.color = currentColor;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        clickButton.transform.localScale = originalScale * 0.9f;
        backgroundSprite.transform.localScale = originalScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clickButton.transform.localScale = originalScale;
        backgroundSprite.transform.localScale = originalScale;
    }

    public void RobotClick(int n)
    {
        clickButton.interactable = false;
        
        StartCoroutine(SimulateMultipleClicks(n));
    }

    // Add Clicks for Robots
    private IEnumerator SimulateMultipleClicks(int n)
    {

        float stepColorTransitionDuration = robotColorTransitionDuration / n;
        for (int i = 0; i < n; i++)
        {
            clickNum++;
            currentClickNum++;

            int itv = (int)Mathf.Floor(clickNum / colorThreshold);
            targetColor = Color.Lerp(buttonColors[itv % buttonColorLength], buttonColors[(itv + 1) % buttonColorLength], ((float)(clickNum % colorThreshold)) / colorThreshold);

            yield return StartCoroutine(ChangeColor(stepColorTransitionDuration)); // Adjust duration for faster transitions

            if (currentClickNum >= maxClicks)
            {
                currentClickNum -= maxClicks;
                GetReward();
            }
            
            if (i != n - 1) {
                yield return new WaitForSeconds(0.05f); // Small delay between clicks
            }
        }

        clickButton.interactable = true;
    }

    // IEnumerator ChangeColor(float transitionDuration)
    // {
    //     float elapsedTime = 0;
    //     Color startColor = buttonSprite.color;

    //     while (elapsedTime < transitionDuration)
    //     {
    //         currentColor = Color.Lerp(startColor, targetColor, elapsedTime / transitionDuration);
    //         buttonSprite.color = currentColor;
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     currentColor = targetColor;
    //     buttonSprite.color = currentColor;
    // }

    // Get Color Reward
    private void GetReward()
    {
        // TODO : need to fix
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

            Debug.Log(rewardIdx);

            Color color = new Color(buttonColors[rewardIdx].r, buttonColors[rewardIdx].g, buttonColors[rewardIdx].b, buttonColors[rewardIdx].a + worldColors[rewardIdx].a / maxCycle);
            buttonColors[rewardIdx] = color;
            backgrounds[rewardIdx].color = color;
        }

        if (Enumerable.SequenceEqual(buttonColors, worldColors)) {
            // End Logic
            Debug.Log("You win!");
            clickButton.interactable = false;
            for (int i = 0; i < robots.Count; i++) {
                robots[i].robotButton.interactable = false;
            }
        }
    }

    void OnDisable()
    {
        // Save Data
        //ClickerData data = ClickerDataManager.CreateClickerData(clickNum, currentClickNum, currentColor, buttonColors);
        //ClickerDataManager.SaveData(data);
    }
}

