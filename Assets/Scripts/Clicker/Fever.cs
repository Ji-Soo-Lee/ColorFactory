using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : MonoBehaviour
{
    public ClickerGameManager clickerGM;
    public ClickerUIManager clickerUIManager;

    private TimerManager feverTimer;
    
    public bool isFeverTime { get; private set; } = false;
    public bool isFeverTimerSet { get; private set; } = false;
    public int feverGauge { get; private set; } = 0;

    private int feverInput = 0;
    private int MAX_FEVER_GAUGE = 5;

    private void Start()
    {
        ResetFeverTimer();
    }

    public void AddFeverGauge(int inc)
    {
        int sum = feverGauge + inc;
        if (inc < 0 || sum < feverGauge) return;

        sum = Mathf.Clamp(sum, 0, MAX_FEVER_GAUGE);

        SetFeverGauge(sum);
    }

    public void SetFeverGauge(int gauge)
    {
        feverGauge = Mathf.Clamp(gauge, 0, MAX_FEVER_GAUGE);

        clickerUIManager.SetFeverGaugeSprite(feverGauge);
    }

    public void ResetFeverTimer()
    {
        isFeverTime = false;
        isFeverTimerSet = false;
        clickerGM.SetFeverWeight(1);

        // Create Fever Timer
        if (feverTimer == null)
        {
            GameObject FeverTimerObject = new GameObject("Fever Timer Object");
            FeverTimerObject.transform.parent = transform;
            FeverTimerObject.AddComponent<TimerManager>();
            feverTimer = FeverTimerObject.GetComponent<TimerManager>();
            if (feverTimer == null)
            {
                UnityEngine.Debug.Log("Create Fever Timer Error");
            }
        }

        feverTimer.ResetTimer();
    }

    public void PauseFeverTime()
    {
        if (!isFeverTime || feverTimer == null || !isFeverTimerSet) return;
        isFeverTime = false;
        clickerGM.SetFeverWeight(1);
        feverTimer.PauseTimer();
    }

    public void ResumeFeverTime()
    {
        if (isFeverTime || feverTimer == null || !isFeverTimerSet) return;
        isFeverTime = true;
        clickerGM.SetFeverWeight(feverInput);
        feverTimer.StartTimer();
    }

    public void StartFeverTime()
    {
        if (isFeverTime) return;
        ResetFeverTimer();

        isFeverTime = true;
        feverInput = feverGauge;
        SetFeverGauge(0);

        clickerGM.SetFeverWeight(feverInput);

        isFeverTimerSet = true;

        feverTimer.SetupTimer(10.0f, ResetFeverTimer);
    }
}
