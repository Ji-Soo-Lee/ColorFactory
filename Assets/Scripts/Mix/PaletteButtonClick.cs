using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteButtonClick : MonoBehaviour
{
    public int ButtonClickedTimes = 0;
    public void OnClickButton()
    {
        ButtonClickedTimes = (ButtonClickedTimes + 1) % 5;
    }
}
