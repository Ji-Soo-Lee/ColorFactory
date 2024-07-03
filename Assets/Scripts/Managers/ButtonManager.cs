using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public int clickNum;
    public List<Color> buttonColors = new List<Color>();
    private Color targetColor;
    private Color currentColor;
    private float colorTransitionDuration = 0.1f; // Duration in seconds over which color change occurs
    private float positionTransitionDuration = 0.1f; // Duration for each phase of the position change
    private float zDistance = 0.5f; // Distance to move backward along the z-axis
    private Vector3 originalPosition;
    private bool isAnimating = false;

    // Start is called before the first frame update
    void Start()
    {   
        clickNum = 0;
        currentColor = buttonColors[clickNum];
        targetColor = currentColor;
        gameObject.GetComponent<Renderer>().material.color = currentColor;
        
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAnimating)
        // if (Input.GetMouseButtonDown(0))
        {  
            clickNum++;

            targetColor = buttonColors[clickNum % buttonColors.Count];
            
            StopCoroutine("ChangeColor"); // Stop the current color transition coroutine if running
            StartCoroutine("ChangeColor"); // Start the color transition coroutine
            
            StopCoroutine(MoveButton());
            transform.position = originalPosition;
            StartCoroutine(MoveButton()); // Start the position transition coroutine
        }

        // Continuously update the color to smoothly transition to the target color
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime / colorTransitionDuration);
        gameObject.GetComponent<Renderer>().material.color = currentColor;
    }

    IEnumerator ChangeColor()
    {
        float elapsedTime = 0;

        while (elapsedTime < colorTransitionDuration)
        {
            currentColor = Color.Lerp(currentColor, targetColor, elapsedTime / colorTransitionDuration);
            gameObject.GetComponent<Renderer>().material.color = currentColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
            
        currentColor = targetColor; // Ensure the final color is set
    }

    IEnumerator MoveButton()
    {
        isAnimating = true;
        float elapsedTime = 0;
        Vector3 targetPosition = originalPosition + new Vector3(0, 0, zDistance);

        // Move backward
        while (elapsedTime < positionTransitionDuration)
        {
            transform.position = Vector3.Slerp(originalPosition, targetPosition, elapsedTime / positionTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is set
        transform.position = targetPosition;

        elapsedTime = 0;

        // Move forward
        while (elapsedTime < positionTransitionDuration)
        {
            transform.position = Vector3.Slerp(targetPosition, originalPosition, elapsedTime / positionTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is set
        transform.position = originalPosition;

        isAnimating = false;
    }

    public void Click(int n)
    {
        clickNum += n;
        targetColor = buttonColors[clickNum % buttonColors.Count];
            
        StopCoroutine("ChangeColor"); // Stop the current color transition coroutine if running
        StartCoroutine("ChangeColor"); // Start the color transition coroutine
    }
}

