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
    public Button robotButton;
    public int clickAmount { get; private set; } = 0;
    public int MAX_CLICK { get; private set; } = 100;

    public void OnClickButton()
    {
        robotManager.RobotClick(clickAmount);

        SetClickAmount(0);
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
