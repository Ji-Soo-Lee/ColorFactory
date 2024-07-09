using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public int clickNum;
    public Button clickButton;
    public Image buttonSprite;
    [SerializeField] private int maxClicks = 24;
    [SerializeField] private int maxCycle = 4;
    private List<Vector3> worldColors = new List<Vector3>();
    private List<Vector3> buttonColors = new List<Vector3>();
    private Vector3 targetHSV;
    private Vector3 currentHSV;
    private float colorTransitionDuration = 1.0f; // Duration in seconds over which color change occurs
    private int buttonColorLength;
    private float colorThreshold;
    private int itv;
    private int currentClickNum;
    private int rewardIdx;

    void Awake()
    {   
        clickNum = 0;

        worldColors.Add(new Vector3((float) 0/360, 0.6f, 0.93f));
        worldColors.Add(new Vector3((float) 90/360, 0.6f, 0.91f));
        worldColors.Add(new Vector3((float) 180/360, 0.6f, 0.95f));
        worldColors.Add(new Vector3((float) 270/360, 0.6f, 0.95f));

        for (int i = 0; i < worldColors.Count; i++) {
            buttonColors.Add(new Vector3(worldColors[i].x, worldColors[i].y / maxCycle, worldColors[i].z));
        }

        currentHSV = buttonColors[0];
        targetHSV = currentHSV;
        buttonSprite = clickButton.GetComponent<Image>();

        buttonColorLength = buttonColors.Count;
        colorThreshold = maxClicks / buttonColorLength;

        buttonSprite.color = Color.HSVToRGB(currentHSV.x, currentHSV.y, currentHSV.z);
        clickButton.onClick.AddListener(OnClickButton);
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
            } while(buttonColors[rewardIdx].y == worldColors[rewardIdx].y);

            Debug.Log(rewardIdx);
            
            buttonColors[rewardIdx] = new Vector3(buttonColors[rewardIdx].x, buttonColors[rewardIdx].y + worldColors[rewardIdx].y / maxCycle, buttonColors[rewardIdx].z);

            if (Enumerable.SequenceEqual(buttonColors, worldColors)) {
                Debug.Log("You win!");
                clickButton.interactable = false;
            }
        }
        
        itv = (int) Mathf.Floor(clickNum / colorThreshold);
        targetHSV = Vector3.Slerp(buttonColors[itv % buttonColorLength], buttonColors[(itv + 1) % buttonColorLength], ((float) (clickNum % colorThreshold)) / colorThreshold);
        
        StopCoroutine("ChangeColor"); // Stop the current color transition coroutine if running
        StartCoroutine("ChangeColor"); // Start the color transition coroutine

        // Continuously update the color to smoothly transition to the target color
        currentHSV = Vector3.Slerp(currentHSV, targetHSV, Time.deltaTime / colorTransitionDuration);
        buttonSprite.color = Color.HSVToRGB(currentHSV.x, currentHSV.y, currentHSV.z);
    }

    IEnumerator ChangeColor()
    {
        float elapsedTime = 0;

        while (elapsedTime < colorTransitionDuration)
        {
            currentHSV = Vector3.Slerp(currentHSV, targetHSV, elapsedTime / colorTransitionDuration);
            buttonSprite.color = Color.HSVToRGB(currentHSV.x, currentHSV.y, currentHSV.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
            
        currentHSV = targetHSV; // Ensure the final color is set
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
        targetHSV = Vector3.Slerp(buttonColors[itv % buttonColorLength], buttonColors[(itv + 1) % buttonColorLength], ((float) (clickNum % colorThreshold)) / colorThreshold);

        StopCoroutine("ChangeColor"); // Stop the current color transition coroutine if running
        StartCoroutine("ChangeColor"); // Start the color transition coroutine
    }
}

