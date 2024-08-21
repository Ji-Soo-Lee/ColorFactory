using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;
using TMPro;
using System.Runtime.InteropServices;

public class PaletteResult : MonoBehaviour
{
    public Button btn1, btn2, btn3, btnPaletteMod, btnSubmission;
    public Image resultImage, btn1Image, btn2Image, btn3Image, targetImage, btnPlusImage, btnMinusImage;
    public int AClick = 0, BClick = 0, CClick = 0, mixType = 1;
    private int paletteType = 0;
    private Color colorA, colorB, colorC;
    private AnswerToken answerToken;
    private Action ActionOnClick;
    public TMP_Text textBtn1, textBtn2, textBtn3, textPlus, textMinus;
    public bool isMixable = false;

    private float clickThreshold = 5;

    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void Vibrate(long _n);
    # endif

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
        else
        {
            btn1Image.color = new Color(0, UnityEngine.Random.Range(0.2f, 0.8f), UnityEngine.Random.Range(0.2f, 0.8f));
            btn2Image.color = new Color(UnityEngine.Random.Range(0.2f, 0.8f), 0, 1f - btn1Image.color.b);
            btn3Image.color = new Color(1f - btn2Image.color.r, 1f - btn1Image.color.g, 0);
            mixType = 0;
            if (_stage % 2 == 0) { mixType = 1; }
        }
    }

    public void ClickReset()
    {
        AClick = 0;
        BClick = 0;
        CClick = 0;
        textBtn1.text = "";
        textBtn2.text = "";
        textBtn3.text = "";
        ActionOnClick?.Invoke();
    }

    void Start()
    {
        answerToken = GameObject.Find("TargetImage").GetComponent<AnswerToken>();
        resultImage = GetComponent<Image>();

        btn1.onClick.AddListener(() => {
            if (paletteType == 0)
            {
                if (AClick < clickThreshold && isMixable)
                {
                    // Vibrate
                    # if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(30);
                    # elif UNITY_IOS && !UNITY_EDITOR
                        Vibrate(1519);
                    # endif
                    AClick++;
                }
            }
            else
            {
                if(AClick > 0 && isMixable ) 
                {
                    // Vibrate
                    # if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(30);
                    # elif UNITY_IOS && !UNITY_EDITOR
                        Vibrate(1519);
                    # endif
                    AClick--;
                }
            }
            textBtn1.text = AClick.ToString();
            if (AClick == 0) { textBtn1.text = ""; }
            ActionOnClick?.Invoke();
        });

        btn2.onClick.AddListener(() => {
            if (paletteType == 0)
            {
                if (BClick < clickThreshold && isMixable)
                {
                    // Vibrate
                    # if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(30);
                    # elif UNITY_IOS && !UNITY_EDITOR
                        Vibrate(1519);
                    # endif
                    BClick++;
                }
            }
            else
            {
                if (BClick > 0 && isMixable )
                {
                    // Vibrate
                    # if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(30);
                    # elif UNITY_IOS && !UNITY_EDITOR
                        Vibrate(1519);
                    # endif
                    BClick--;
                }
            }
            textBtn2.text = BClick.ToString();
            if (BClick == 0) { textBtn2.text = ""; }
            ActionOnClick?.Invoke();
        });

        btn3.onClick.AddListener(() => {
            if (paletteType == 0)
            {
                if (CClick < clickThreshold && isMixable)
                {
                    // Vibrate
                    # if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(30);
                    # elif UNITY_IOS && !UNITY_EDITOR
                        Vibrate(1519);
                    # endif
                    CClick++;
                }
            }
            else
            {
                if (CClick > 0 && isMixable)
                {
                    // Vibrate
                    # if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(30);
                    # elif UNITY_IOS && !UNITY_EDITOR
                        Vibrate(1519);
                    # endif
                    CClick--;
                }
            }
            textBtn3.text = CClick.ToString();
            if (CClick == 0) { textBtn3.text = ""; }
            ActionOnClick?.Invoke();
        });

        btnPaletteMod.onClick.AddListener(() => {
            if (isMixable)
            {
                paletteType = (paletteType + 1) % 2;
                if (paletteType == 0)
                {
                    btnPlusImage.color = new Color(1, 1, 1, 1);
                    btnMinusImage.color = new Color(1, 1, 1, 0);
                    textPlus.color = new Color(0, 0, 0, 1);
                    textMinus.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    btnPlusImage.color = new Color(1, 1, 1, 0);
                    btnMinusImage.color = new Color(1, 1, 1, 1);
                    textPlus.color = new Color(1, 1, 1, 1);
                    textMinus.color = new Color(0, 0, 0, 1);
                }
            }
        });
    }
}
