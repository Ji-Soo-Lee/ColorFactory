using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;

public class PaletteResult : MonoBehaviour
{
    public Button btn1, btn2, btn3, btnReset;
    public Image resultImage, btn1Image, btn2Image, btn3Image;
    public int AClick = 0, BClick = 0, CClick = 0, mixType = 1;
    private Color colorA, colorB, colorC;

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

    void Start()
    {
        resultImage = GetComponent<Image>();

        SetOnClickAction(mixType);

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
            AClick = 0;
            BClick = 0;
            CClick = 0;
            ActionOnClick?.Invoke();
        });
    }
}
