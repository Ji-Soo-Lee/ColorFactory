using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MixPause : PausableMonoBehaviour
{
    // public Button btn1;
    public PaletteResult paletteResult;
    // public void PauseButtonOnClick()
    // {
    //     if (CheckPause())
    //     {
    //         Resume();
    //         paletteResult.isMixable = true;
    //     }
    //     else
    //     {
    //         Pause();
    //         paletteResult.isMixable = false;
    //     }
    // }
    // protected void Start()
    // {
    //     btn1.onClick.AddListener(() =>
    //     {
    //         PauseButtonOnClick();
    //     });
    // }

    public override void Pause()
    {
        base.Pause();
        paletteResult.isMixable = true;
    }

    public override void Resume()
    {
        base.Resume();
        paletteResult.isMixable = true;
    }
}
