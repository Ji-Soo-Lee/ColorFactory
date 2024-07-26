using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMiniGM : PausableMonoBehaviour
{
    protected override void BeforePauseUpdate()
    {
        UnityEngine.Debug.Log("DummyMiniGM Before Pause Update");
    }

    protected override void AfterPauseUpdate()
    {
        Pause();
        UnityEngine.Debug.Log("DummyMiniGM After Pause Update");
    }
}
