using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClickerData
{
    public int clickNum;
    public int currentClickNum;
    public int feverGauge;
    public Color currentColor;
    public List<Color> buttonColors;
}

public class ClickerDataManager
{
    public static ClickerData CreateClickerData(int clickNum, int currentClickNum, int feverGauge, Color currentColor, List<Color> buttonColors)
    {
        ClickerData clickerData = new ClickerData();
        clickerData.clickNum = clickNum;
        clickerData.currentClickNum = currentClickNum;
        clickerData.feverGauge = feverGauge;
        clickerData.currentColor = currentColor;
        clickerData.buttonColors = buttonColors;

        return clickerData;
    }

    public static void SaveData(ClickerData data)
    {
        DataManager.SaveJSON<ClickerData>(data, "clicker_save_data");
    }

    public static ClickerData LoadData()
    {
        ClickerData data = DataManager.LoadJSON<ClickerData>("clicker_save_data");
        return data;
    }
}
