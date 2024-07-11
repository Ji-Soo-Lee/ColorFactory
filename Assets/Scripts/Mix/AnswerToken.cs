using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerToken : MonoBehaviour
{
    public Image targetImage,btn1Image,btn2Image,btn3Image;
    GameObject obj1;
    public int btn1Target, btn2Target, btn3Target;
    public void SetTargetColor(int stage)
    {
        int currentMixType = obj1.GetComponent<PaletteResult>().mixType;
        Color color1, color2, color3;
        btn1Target = Random.Range(1,6);
        btn2Target = Random.Range(1,6);
        btn3Target = Random.Range(1,6);
        if (stage < 2) //Determin which value will be 0
        {
            int tmp = Random.Range(0,3);
            if (tmp == 0) {btn1Target = 0;}
            else if (tmp == 1) { btn2Target = 0;}
            else { btn3Target = 0; }
        }
        if (currentMixType == 0) //Additive Mixing
        {
            color1 = btn1Image.color * (btn1Target / 5f);
            color2 = btn2Image.color * (btn2Target / 5f);
            color3 = btn3Image.color * (btn3Target / 5f);
            targetImage.color = ColorUtils.MixLights(color1, ColorUtils.MixLights(color2, color3)); 
        }
        else //Subtractive Mixing
        {
            color1 = Color.white - (Color.white - btn1Image.color) * btn1Target / 5f;
            color2 = Color.white - (Color.white - btn2Image.color) * btn2Target / 5f;
            color3 = Color.white - (Color.white - btn3Image.color) * btn3Target / 5f;
            targetImage.color = ColorUtils.MixColors(color1, ColorUtils.MixColors(color2, color3));
        }
    }
    void Start()
    {
        targetImage = GetComponent<Image>();
        obj1 = GameObject.Find("ResultImage");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
