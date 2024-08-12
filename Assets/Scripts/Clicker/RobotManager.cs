using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotManager : MonoBehaviour
{
    public ClickerUIManager clickerUIManager;
    public ClickerGameManager clickerGM;

    public float robotColorTransitionDuration = 5.0f;

    public List<Robot> robots = new List<Robot>();

    private bool isClickCoroutineRunning;

    public void SetAllRobotsInteractable(bool isInteractable)
    {
        foreach (Robot robot in robots)
        {
            robot.robotButton.interactable = isInteractable;
        }
    }

    public void RobotClick(int clickNum)
    {
        StartCoroutine(SimulateMultipleClicks(clickNum));
    }


    private IEnumerator SimulateMultipleClicks(int clickNum)
    {
        // Spinlock if preceding coroutine exists
        // Cannot guarantee the order if blocked
        // while (currentClickCoroutine != null) yield return currentClickCoroutine;
        // isClickCoroutineRunning = true;

        float stepColorTransitionDuration = robotColorTransitionDuration / clickNum;

        clickerUIManager.mainButton.SetInteractive(false);

        for (int i = 0; i < clickNum; i++)
        {
            clickerGM.IncrementClickCount();

            clickerUIManager.UpdateButtonColor(robotColorTransitionDuration);

            if (i != clickNum - 1)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }

        clickerUIManager.mainButton.SetInteractive(true);

        // isClickCoroutineRunning = false;
    }
}