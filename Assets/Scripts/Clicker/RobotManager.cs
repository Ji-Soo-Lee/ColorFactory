using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotManager : MonoBehaviour
{
    public ClickerUIManager clickerUIManager;
    public ClickerGameManager clickerGM;

    public float robotColorTransitionDuration = 5.0f;

    public List<RobotV2> robots = new List<RobotV2>();

    public void SetAllRobotsInteractable(bool isInteractable)
    {
        foreach (RobotV2 robot in robots)
        {
            robot.robotButton.interactable = isInteractable;
        }
    }

    public List<int> GetClickAmounts()
    {
        List<int> clickAmounts = new List<int>();

        foreach (RobotV2 robot in robots)
        {
            clickAmounts.Add(robot.clickAmount);
        }

        return clickAmounts;
    }

    public List<int> GetMaxClicks()
    {
        List<int> maxClicks = new List<int>();

        foreach (RobotV2 robot in robots)
        {
            maxClicks.Add(robot.MAX_CLICK);
        }

        return maxClicks;
    }

    public void SetClickAmounts(List<int> amounts)
    {
        int minLen = Mathf.Min(robots.Count, amounts.Count);

        for (int i = 0; i < minLen; i++)
        {
            robots[i].SetClickAmount(amounts[i]);
        }
    }

    public void SetMaxClicks(List<int> maxClicks)
    {
        int minLen = Mathf.Min(robots.Count, maxClicks.Count);

        for (int i = 0; i < minLen; i++)
        {
            robots[i].SetMaxClick(maxClicks[i]);
        }
    }

    public void RobotClick(int clickNum)
    {
        StartCoroutine(clickerGM.SimulateMultipleClicks(clickNum, robotColorTransitionDuration));
    }

    public void ConsumeAllRobotClicks()
    {
        foreach (RobotV2 robot in robots)
        {
            robot.OnClickButton();
        }
    }
}