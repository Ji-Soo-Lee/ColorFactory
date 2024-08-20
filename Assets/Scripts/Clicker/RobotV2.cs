using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// TODO : need to fix
public class RobotV2 : MonoBehaviour
{
    public RobotManager robotManager;
    // public int clickAmount;
    public Button robotButton;
    public ClickerUIManager clickerUIManager;
    [field: SerializeField] public int clickAmount { get; private set; } = 0;
    public int MAX_ROBOT_BATTERY = 50;
    // [field: SerializeField] public int clickAmount { get; private set; } = 0;
    [field: SerializeField] public int MAX_CLICK { get; private set; } = 100;

    public void ResetBattery()
    {
        SetRobotBattery(0);
    }

    public void AddRobotBattery(int inc)
    {
        int sum = clickAmount + inc;
        if (inc < 0 || sum < clickAmount) return;

        sum = Mathf.Clamp(sum, 0, MAX_ROBOT_BATTERY);

        SetRobotBattery(sum);
    }

    public void SetRobotBattery(int gauge)
    {
        clickAmount = Mathf.Clamp(gauge, 0, MAX_ROBOT_BATTERY);
        Debug.Log("clickAmount : " + clickAmount);

        clickerUIManager.SetRobotBatterySprite(clickAmount);
    }

    public void OnClickButton()
    {
        Debug.Log("Click Button");
        Debug.Log(clickAmount);
        
        robotManager.RobotClick(clickAmount);

        SetClickAmount(clickAmount);
    }

    public void AddClickAmount(int inc)
    {
        int sum = clickAmount + inc;
        if (inc < 0 || sum < clickAmount) return;

        SetClickAmount(sum);
    }

    public void SetClickAmount(int amount)
    {
        clickAmount = Mathf.Clamp(amount, 0, MAX_CLICK);
    }

    public void SetMaxClick(int maxClick)
    {
        if (maxClick < MAX_CLICK) return;
        
        MAX_CLICK = maxClick;
    }
}
