using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PaletteResult : MonoBehaviour
{
    public Button btn1, btn2, btn3, btnreset;
    public Image resultimage;
    public int AClicked=0, BClicked=0, CClicked=0;
    void Start()
    {
        resultimage = GetComponent<Image>();
        btn1.onClick.AddListener(() => { if (AClicked < 5) { AClicked++; } });
        btn2.onClick.AddListener(() => { if (BClicked < 5) { BClicked++; } });
        btn3.onClick.AddListener(() => { if (CClicked < 5) { CClicked++; } });
        btnreset.onClick.AddListener(() => { AClicked=0;BClicked = 0;CClicked = 0; });
    }

    void Update()
    {
        Color colorR = new Color(255f*AClicked / (255 * 5), 0f, 0f);
        Color colorG = new Color(0f, 255f*BClicked / (255 * 5), 0f);
        Color colorB = new Color(0f, 0f, 255f*CClicked / (255 * 5));
        Color tempColor = ColorUtils.MixLights(colorR,ColorUtils.MixLights(colorG,colorB));
        resultimage.color = tempColor;
    }
}
