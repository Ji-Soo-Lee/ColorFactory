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
    [SerializeField] private List<Sprite> buttonBackgrounds = new List<Sprite>();
    private List<Color> buttonColors = new List<Color>();
    private Color targetColor;
    private Color currentColor;
    [SerializeField] private float colorTransitionDuration = 1.0f; // Duration in seconds over which color change occurs
    [SerializeField] private float robotColorTransitionDuration = 5.0f;
    private int buttonColorLength;
    private int itv;
    private float colorThreshold;
    private int currentColorIndex;
    private int currentClickNum;
    private int rewardIdx;
    private Color color;
    // private Coroutine colorChangeCoroutine;

    void Awake()
    {   
        clickNum = 0;

        for (int i = 0; i < worldColors.Count; i++) {
            color = new Color(worldColors[i].r, worldColors[i].g, worldColors[i].b, worldColors[i].a / maxCycle);
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

        backgroundSprite.sprite = buttonBackgrounds[0];
    }

    void OnClickButton()
    {
        clickNum++;
        currentClickNum++;
        
        itv = (int) Mathf.Floor(clickNum / colorThreshold);
        targetColor = Color.Lerp(buttonColors[itv % buttonColorLength], buttonColors[(itv + 1) % buttonColorLength], ((float) (clickNum % colorThreshold)) / colorThreshold);
        
        StopCoroutine(ChangeColor(colorTransitionDuration));
        StartCoroutine(ChangeColor(colorTransitionDuration));

        if (currentClickNum >= maxClicks) {
            // Debug.Log(clickNum);
            currentClickNum -= maxClicks;
            GetReward();
        }

        // Continuously update the color to smoothly transition to the target color
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime / colorTransitionDuration);
        buttonSprite.color = currentColor;
    }

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

        currentColor = targetColor;
        buttonSprite.color = currentColor;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundSprite.sprite = buttonBackgrounds[1];
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        backgroundSprite.sprite = buttonBackgrounds[0];
    }

public void RobotClick(int n)
{
    clickButton.interactable = false;
    
    StartCoroutine(SimulateMultipleClicks(n));
}

    private IEnumerator SimulateMultipleClicks(int n)
    {

        float stepColorTransitionDuration = robotColorTransitionDuration / n;
        for (int i = 0; i < n; i++)
        {
            clickNum++;
            currentClickNum++;

            itv = (int)Mathf.Floor(clickNum / colorThreshold);
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

    private void GetReward()
    {
        // TODO : need to fix
        do {
            rewardIdx = Random.Range(0, buttonColorLength);
        } while(buttonColors[rewardIdx].a == worldColors[rewardIdx].a);

        Debug.Log(rewardIdx);
        
        color = new Color(buttonColors[rewardIdx].r, buttonColors[rewardIdx].g, buttonColors[rewardIdx].b, buttonColors[rewardIdx].a + worldColors[rewardIdx].a / maxCycle);
        buttonColors[rewardIdx] = color;
        backgrounds[rewardIdx].color = color;

        if (Enumerable.SequenceEqual(buttonColors, worldColors)) {
            Debug.Log("You win!");
            clickButton.interactable = false;
            for (int i = 0; i < robots.Count; i++) {
                robots[i].robotButton.interactable = false;
            }
        }
    }
}

