using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommonPopupUIManager : MonoBehaviour
{
    public GameObject ResultPanelObject;
    public GameObject ExitPanelObject;
    public GameObject PausePanelObject;

    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text colorMixText;


    void Start()
    {
        ResultPanelObject.SetActive(false);
        ExitPanelObject.SetActive(false);
        PausePanelObject.SetActive(false);
    }

    public void SetActiveResultPanel(bool isActive)
    {
        ResultPanelObject.SetActive(isActive);
    }

    public void SetActiveExitPanel(bool isActive)
    {
        ExitPanelObject.SetActive(isActive);
    }

    public void SetActivePausePanel(bool isActive)
    {
        PausePanelObject.SetActive(isActive);
    }

    public void SetResultText(int time, int score, int colorMix)
    {
        if (ResultPanelObject.activeSelf)
        {
            timeText.text = time.ToString();
            scoreText.text = score.ToString();
            colorMixText.text = "+" + colorMix.ToString() + " COLORMIX";
        }
    }
}
