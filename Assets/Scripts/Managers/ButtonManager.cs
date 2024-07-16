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
    [SerializeField] private int maxClicks = 24;
    [SerializeField] private int maxCycle = 4;
    [SerializeField] private List<Color> worldColors = new List<Color>();
    [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();
    private List<Color> buttonColors = new List<Color>();
    private Color targetColor;
    private Color currentColor;
    [SerializeField] private float colorTransitionDuration = 1.0f; // Duration in seconds over which color change occurs
    private int buttonColorLength;
    private float colorThreshold;
    private int itv;
    private int currentClickNum;
    private int rewardIdx;

    void Awake()
    {   
        clickNum = 0;

        for (int i = 0; i < worldColors.Count; i++) {
            buttonColors.Add(new Color(worldColors[i].r, worldColors[i].g, worldColors[i].b, worldColors[i].a / maxCycle));
        }

        currentColor = buttonColors[0];
        targetColor = currentColor;
        buttonSprite = clickButton.GetComponent<Image>();

        buttonColorLength = buttonColors.Count;
        colorThreshold = maxClicks / buttonColorLength;

        buttonSprite.color = currentColor;
        clickButton.onClick.AddListener(OnClickButton);

        backgroundSprite.sprite = backgrounds[0];
    }

    void OnClickButton()
    {
        clickNum++;
        currentClickNum++;

        if (currentClickNum >= maxClicks) {
            // Debug.Log(clickNum);
            currentClickNum -= maxClicks;

            // TODO : need to fix
            do {
                rewardIdx = Random.Range(0, buttonColorLength);
            } while(buttonColors[rewardIdx].a == worldColors[rewardIdx].a);

            Debug.Log(rewardIdx);
            
            buttonColors[rewardIdx] = new Color(buttonColors[rewardIdx].r, buttonColors[rewardIdx].g, buttonColors[rewardIdx].b, buttonColors[rewardIdx].a + worldColors[rewardIdx].a / maxCycle);

            if (Enumerable.SequenceEqual(buttonColors, worldColors)) {
                Debug.Log("You win!");
                clickButton.interactable = false;
            }
        }
        
        itv = (int) Mathf.Floor(clickNum / colorThreshold);
        targetColor = Color.Lerp(buttonColors[itv % buttonColorLength], buttonColors[(itv + 1) % buttonColorLength], ((float) (clickNum % colorThreshold)) / colorThreshold);
        
        StopCoroutine("ChangeColor"); // Stop the current color transition coroutine if running
        StartCoroutine("ChangeColor"); // Start the color transition coroutine

        // Continuously update the color to smoothly transition to the target color
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime / colorTransitionDuration);
        buttonSprite.color = currentColor;
    }

    IEnumerator ChangeColor()
    {
        float elapsedTime = 0;

        while (elapsedTime < colorTransitionDuration)
        {
            currentColor = Color.Lerp(currentColor, targetColor, elapsedTime / colorTransitionDuration);
            buttonSprite.color = currentColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
            
        currentColor = targetColor; // Ensure the final color is set
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundSprite.sprite = backgrounds[1];
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        backgroundSprite.sprite = backgrounds[0];
    }

    // TODO : need to fix
    public void Click(int n)
    {
        clickNum += n;
        currentClickNum += n;

        if (currentClickNum >= maxClicks) {
            Debug.Log(clickNum);
            currentClickNum -= maxClicks;
        }
        
        itv = (int) Mathf.Floor(clickNum / colorThreshold);
        targetColor = Color.Lerp(buttonColors[itv % buttonColorLength], buttonColors[(itv + 1) % buttonColorLength], ((float) (clickNum % colorThreshold)) / colorThreshold);

        StopCoroutine("ChangeColor"); // Stop the current color transition coroutine if running
        StartCoroutine("ChangeColor"); // Start the color transition coroutine
    }
}

