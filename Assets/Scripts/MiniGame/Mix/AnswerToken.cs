using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerToken : MonoBehaviour
{
    public Image targetImage,btn1Image,btn2Image,btn3Image;
    public int btn1Target, btn2Target, btn3Target, currentMixType;
    public PaletteResult paletteResult;
    private float clickThreshold = 5;
    private Color color1, color2, color3;
    public void SetTargetColor(int _stage)
    {
        currentMixType = paletteResult.mixType;
        btn1Target = Random.Range(1, 6);
        btn2Target = Random.Range(1, 6);
        btn3Target = Random.Range(1, 6);
        if (_stage < 4) //Determin which value will be 0
        {
            int tmp = Random.Range(0, 3);
            if (tmp == 0) { btn1Target = 0; }
            else if (tmp == 1) { btn2Target = 0; }
            else { btn3Target = 0; }
        }
        else
        {
            while (btn1Target + btn2Target + btn3Target > 9)
            {
                btn1Target = Random.Range(1, 6);
                btn2Target = Random.Range(1, 6);
                btn3Target = Random.Range(1, 6);
            }
        }
        if (currentMixType == 0) //Additive Mixing
        {
            color1 = btn1Image.color * (btn1Target / clickThreshold);
            color2 = btn2Image.color * (btn2Target / clickThreshold);
            color3 = btn3Image.color * (btn3Target / clickThreshold);
            targetImage.color = ColorUtils.MixLights(color1, ColorUtils.MixLights(color2, color3));
        }
        else //Subtractive Mixing
        {
            color1 = Color.white - (Color.white - btn1Image.color) * btn1Target / clickThreshold;
            color2 = Color.white - (Color.white - btn2Image.color) * btn2Target / clickThreshold;
            color3 = Color.white - (Color.white - btn3Image.color) * btn3Target / clickThreshold;
            targetImage.color = ColorUtils.MixColors(color1, ColorUtils.MixColors(color2, color3));
        }
    }
    void Start()
    {
        targetImage = GetComponent<Image>();
        paletteResult = GameObject.Find("ResultImage").GetComponent<PaletteResult>();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
