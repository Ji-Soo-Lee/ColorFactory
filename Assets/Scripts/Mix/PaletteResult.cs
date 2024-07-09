using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PaletteResult : MonoBehaviour
{
    public Button btn1, btn2, btn3, btnreset;
    public Image resultimage,btn1image,btn2image,btn3image;
    public int AClicked=0, BClicked=0, CClicked=0, MixType=1;
    public Color btncolorA, btncolorB, btncolorC, colorA,colorB,colorC;
    void Start()
    {
        resultimage = GetComponent<Image>();
        btncolorA = btn1image.color;
        btncolorB = btn2image.color;
        btncolorC = btn3image.color;
        colorA = btncolorA;
        colorB = btncolorB;
        colorC = btncolorC;
        btn1.onClick.AddListener(() => { if (AClicked < 4) { AClicked++; } });
        btn2.onClick.AddListener(() => { if (BClicked < 4) { BClicked++; } });
        btn3.onClick.AddListener(() => { if (CClicked < 4) { CClicked++; } });
        btnreset.onClick.AddListener(() => { AClicked=0;BClicked = 0;CClicked = 0; });
    }

    void Update()
    {
       
        if (MixType == 0) //Additive Mixing
        {
            colorA = btncolorA * (AClicked / 4f);
            colorB = btncolorB * (BClicked / 4f);
            colorC = btncolorC * (CClicked / 4f);
            resultimage.color = ColorUtils.MixLights(colorA, ColorUtils.MixLights(colorB, colorC));
        }
        else //Subtractive Mixing
        {
            colorA = Color.white - (Color.white - btncolorA) * AClicked / 4f;
            colorB = Color.white - (Color.white - btncolorB) * BClicked / 4f;
            colorC = Color.white - (Color.white - btncolorC) * CClicked / 4f;
            resultimage.color = ColorUtils.MixColors(colorA, ColorUtils.MixColors(colorB, colorC));
        }
    }
}
