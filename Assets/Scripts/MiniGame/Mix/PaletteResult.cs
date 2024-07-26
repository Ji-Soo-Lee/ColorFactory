using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;

public class PaletteResult : MonoBehaviour
{
    public Button btn1, btn2, btn3, btnReset, btnSubmission;
    public Image resultImage, btn1Image, btn2Image, btn3Image, targetImage;
    public int AClick = 0, BClick = 0, CClick = 0, mixType = 1;
    private Color colorA, colorB, colorC;
    private AnswerToken answerToken;
    private Action ActionOnClick;

    private float clickThreshold = 5;

    public void AdditiveMixing()
    {
        colorA = btn1Image.color * (AClick / clickThreshold);
        colorB = btn2Image.color * (BClick / clickThreshold);
        colorC = btn3Image.color * (CClick / clickThreshold);
        resultImage.color = ColorUtils.MixLights(colorA, ColorUtils.MixLights(colorB, colorC));
    }

    public void SubtractiveMixing()
    {
        colorA = Color.white - (Color.white - btn1Image.color) * AClick / clickThreshold;
        colorB = Color.white - (Color.white - btn2Image.color) * BClick / clickThreshold;
        colorC = Color.white - (Color.white - btn3Image.color) * CClick / clickThreshold;
        resultImage.color = ColorUtils.MixColors(colorA, ColorUtils.MixColors(colorB, colorC));
    }

    public void SetOnClickAction(int _mixType)
    {
        if (_mixType == 0)
        {
            ActionOnClick = AdditiveMixing;
        }
        else
        {
            ActionOnClick = SubtractiveMixing;
        }
    }

    public void SetPaletteColor(int _stage)
    {
        if (_stage == 0)
        {
            btn1Image.color = new Color(0,1,1);
            btn2Image.color = new Color(1,0,1);
            btn3Image.color = new Color(1,1,0);
            mixType = 1;
        }
        else if (_stage == 1)
        {
            btn1Image.color = Color.red;
            btn2Image.color = Color.green;
            btn3Image.color = Color.blue;
            mixType = 0;
        }
        else if( _stage == 2)
        {
            btn1Image.color = new Color(1, UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            btn2Image.color = new Color(UnityEngine.Random.Range(0f,1f), 1f, 1f-btn1Image.color.b);
            btn3Image.color = new Color(1f - btn2Image.color.r, 1f - btn1Image.color.g, 1f);
            mixType = 1;
        }
        else if(_stage  == 3)
        {
            btn1Image.color = new Color(0, UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            btn2Image.color = new Color(UnityEngine.Random.Range(0f, 1f), 0, 1f - btn1Image.color.b);
            btn3Image.color = new Color(1f - btn2Image.color.r, 1f - btn1Image.color.g, 0);
            mixType = 0;
        }
        else
        {
            btn1Image.color = ColorUtils.GetRandomColor();
            btn2Image.color = new Color(UnityEngine.Random.Range(0f, 1f- btn1Image.color.r), UnityEngine.Random.Range(0f, 1f - btn1Image.color.g), UnityEngine.Random.Range(0f, 1f - btn1Image.color.b));
            btn3Image.color = new Color(1,1,1) - btn1Image.color - btn2Image.color;
            mixType = 1;
        }
    }

    public void ClickReset()
    {
        AClick = 0;
        BClick = 0;
        CClick = 0;
        ActionOnClick?.Invoke();
    }

    void Start()
    {
        answerToken = GameObject.Find("TargetImage").GetComponent<AnswerToken>();
        resultImage = GetComponent<Image>();

        btn1.onClick.AddListener(() => {
            if (AClick < clickThreshold)
            {
                AClick++;
                ActionOnClick?.Invoke();
            }
        });

        btn2.onClick.AddListener(() => {
            if (BClick < clickThreshold)
            {
                BClick++;
                ActionOnClick?.Invoke();
            }
        });

        btn3.onClick.AddListener(() => {
            if (CClick < clickThreshold)
            {
                CClick++;
                ActionOnClick?.Invoke();
            }
        });

        btnReset.onClick.AddListener(() => {
            ClickReset();
        });
    }
}
