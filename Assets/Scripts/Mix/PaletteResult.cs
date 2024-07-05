using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PaletteResult : MonoBehaviour
{
    public GameObject ob1, ob2, ob3;
    public Image image;
    public int AClicked;
    public int BClicked;
    public int CClicked;
    void Start()
    {
        AClicked = ob1.GetComponent<PaletteButtonClick>().ButtonClickedTimes;
        BClicked = ob2.GetComponent<PaletteButtonClick>().ButtonClickedTimes;
        CClicked = ob3.GetComponent<PaletteButtonClick>().ButtonClickedTimes;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        AClicked = ob1.GetComponent<PaletteButtonClick>().ButtonClickedTimes;
        BClicked = ob2.GetComponent<PaletteButtonClick>().ButtonClickedTimes;
        CClicked = ob3.GetComponent<PaletteButtonClick>().ButtonClickedTimes;
        Color colorR = new Color(255f*AClicked / (255 * 4), 0f, 0f);
        Color colorG = new Color(0f, 255f*BClicked / (255 * 4), 0f);
        Color colorB = new Color(0f, 0f, 255f*CClicked / (255 * 4));
        Color tempColor = ColorUtils.MixLights(colorR,ColorUtils.MixLights(colorG,colorB));
        image.color = tempColor;
    }
}
