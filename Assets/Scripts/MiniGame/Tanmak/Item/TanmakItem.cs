using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakItem : MonoBehaviour
{
    TanmakGameManager tanmakGM;
    TimerManager timer;
    public string itemName = "";

    protected virtual void Start()
    {
        tanmakGM = GameObject.FindObjectOfType<TanmakGameManager>();
        timer = gameObject.AddComponent<TimerManager>();

        if (tanmakGM != null)
        {
            timer.SetupTimer(15f, () => Destroy(gameObject));
            tanmakGM.AddPauseHandler(timer.PauseTimer);
            tanmakGM.AddResumeHandler(timer.StartTimer);
        }

        timer.StartTimer();
    }
}
