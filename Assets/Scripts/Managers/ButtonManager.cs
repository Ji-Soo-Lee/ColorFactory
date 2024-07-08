using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public int clickNum;
    public Button clickButton;
    public Image buttonSprite;
    public List<Color> buttonColors = new List<Color>();
    private Color targetColor;
    private Color currentColor;
    private float colorTransitionDuration = 0.1f; // Duration in seconds over which color change occurs
    private int maxClicks = 4;

    // Start is called before the first frame update
    void Start()
    {   
        clickNum = 0;
        currentColor = buttonColors[clickNum];
        targetColor = currentColor;
        buttonSprite = clickButton.GetComponent<Image>();

        buttonSprite.color = currentColor;
        clickButton.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        clickNum++;
        targetColor = buttonColors[clickNum % maxClicks];
        
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

    public void Click(int n)
    {
        clickNum += n;
        targetColor = buttonColors[clickNum % maxClicks];
            
        StopCoroutine("ChangeColor"); // Stop the current color transition coroutine if running
        StartCoroutine("ChangeColor"); // Start the color transition coroutine
    }
}

