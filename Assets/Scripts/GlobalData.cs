using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Inst;
    public ColorPallete colorPallete { get; private set; }

    void Awake()
    {
        // Set Singleton
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // Temporarily Add Color Manually
        // AddColorPallete(new Color(0.1f, 0.1f, 0.1f));
        // AddColorPallete(new Color(0.1f, 0.2f, 0.3f));
        // SaveColorPallete();

        LoadColorPallete();
    }

    // Add Additional Color to Color List
    public void AddColorPallete(Color color)
    {
        if (colorPallete == null)
        {
            colorPallete = new ColorPallete();
        }

        if (colorPallete.colors == null)
        {
            colorPallete.colors = new List<Color>();
        }

        colorPallete.colors.Add(color);
    }

    // Save Current Color Pallete
    public void SaveColorPallete()
    {
        ColorUtils.SaveColorPallete(colorPallete);
    }

    // Load Color Pallete From JSON
    public void LoadColorPallete()
    {
        colorPallete = ColorUtils.LoadColorPallete();
    }
}
