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
        StartCoroutine(clickerGM.SimulateMultipleClicks(clickNum, robotColorTransitionDuration));
    }
}