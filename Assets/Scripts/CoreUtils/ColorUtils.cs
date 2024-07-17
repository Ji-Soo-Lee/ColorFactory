using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtils
{
    // Get Random Color (Without Candidate)
    public static Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    
    public static Color GetRandomColorinList(List<Color> colorList)
    {
        if (colorList.Count == 0)
        {
            return GetRandomColor();
        }
        else
        {
            return colorList[Random.Range(0, colorList.Count)];
        }
    }

    // Mix Two Colors (Additive Mixing)
    public static Color MixLights(Color color1, Color color2)
    {
        float r = Mathf.Clamp01(color1.r + color2.r);
        float g = Mathf.Clamp01(color1.g + color2.g);
        float b = Mathf.Clamp01(color1.b + color2.b);

        return new Color(r, g, b);
    }

    public static void RGBtoCMYK(Color color, out float c, out float m, out float y, out float k)
    {
        float r = color.r;
        float g = color.g;
        float b = color.b;

        float cyan = 1f - r;
        float magenta = 1f - g;
        float yellow = 1f - b;

        float black = Mathf.Min(cyan, Mathf.Min(magenta, yellow));

        if (black < 1f)
        {
            c = Mathf.Clamp01((cyan - black) / (1f - black));
            m = Mathf.Clamp01((magenta - black) / (1f - black));
            y = Mathf.Clamp01((yellow - black) / (1f - black));
            k = black;
        }
        else
        {
            c = 0f;
            m = 0f;
            y = 0f;
            k = 1f;
        }
    }

    public static Color CMYKtoRGB(float c, float m, float y, float k)
    {
        float r = Mathf.Clamp01(1f - Mathf.Min(1f, c * (1f - k) + k));
        float g = Mathf.Clamp01(1f - Mathf.Min(1f, m * (1f - k) + k));
        float b = Mathf.Clamp01(1f - Mathf.Min(1f, y * (1f - k) + k));

        return new Color(r, g, b);
    }

    // Mix Two Colors (Subtractive Mixing)
    public static Color MixColors(Color color1, Color color2)
    {
        float c1 = 0f, m1 = 0f, y1 = 0f, k1 = 0f, c2 = 0f, m2 = 0f, y2 = 0f, k2 = 0f;
        RGBtoCMYK(color1, out c1, out m1, out y1, out k1);
        RGBtoCMYK(color2, out c2, out m2, out y2, out k2);
        
        float c = Mathf.Clamp01(c1 + c2);
        float m = Mathf.Clamp01(m1 + m2);
        float y = Mathf.Clamp01(y1 + y2);
        float k = Mathf.Clamp01(k1 + k2);

        return CMYKtoRGB(c, m, y, k);
    }

    // Todo : Return Color List w.Gradation

    public bool CompareColor(Color color1, Color color2)
    {
        float errRange = 0.00001f;
        return Mathf.Abs(color1.r - color2.r) < errRange &&
            Mathf.Abs(color1.g - color2.g) < errRange &&
            Mathf.Abs(color1.b - color2.b) < errRange;
    }
}
