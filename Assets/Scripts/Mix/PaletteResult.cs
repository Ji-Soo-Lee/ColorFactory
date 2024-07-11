using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PaletteResult : MonoBehaviour
{
    public Button btn1, btn2, btn3, btnReset;
    public Image resultImage,btn1Image,btn2Image,btn3Image;
    public int AClick=0, BClick=0, CClick=0, mixType=1;
    private Color colorA,colorB,colorC;
    void Start()
    {
        resultImage = GetComponent<Image>();
        colorA = btn1Image.color;
        colorB = btn2Image.color;
        colorC = btn3Image.color;
        btn1.onClick.AddListener(() => { if (AClick < 5) { AClick++; } });
        btn2.onClick.AddListener(() => { if (BClick < 5) { BClick++; } });
        btn3.onClick.AddListener(() => { if (CClick < 5) { CClick++; } });
        btnReset.onClick.AddListener(() => { AClick=0;BClick = 0;CClick = 0; });
    }

    void Update()
    {
       
        if (mixType == 0) //Additive Mixing
        {
            colorA = btn1Image.color * (AClick / 5f);
            colorB = btn2Image.color * (BClick / 5f);
            colorC = btn3Image.color * (CClick / 5f);
            resultImage.color = ColorUtils.MixLights(colorA, ColorUtils.MixLights(colorB, colorC));
        }
        else //Subtractive Mixing
        {
            colorA = Color.white - (Color.white - btn1Image.color) * AClick / 5f;
            colorB = Color.white - (Color.white - btn2Image.color) * BClick / 5f;
            colorC = Color.white - (Color.white - btn3Image.color) * CClick / 5f;
            resultImage.color = ColorUtils.MixColors(colorA, ColorUtils.MixColors(colorB, colorC));
        }
    }
}
