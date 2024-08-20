using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ClickerData
{
    public ClickerStateData stateData;
    public ClickerColorData colorData;
    public ClickerRobotData robotData;
    public List<ClickerWorldData> worldData;

    public ClickerData(ClickerStateData stateData, ClickerColorData colorData, ClickerRobotData robotData, List<ClickerWorldData> worldData)
    {
        this.stateData = stateData;
        this.colorData = colorData;
        this.robotData = robotData;
        this.worldData = worldData;
    }
}

[System.Serializable]
public class ClickerStateData
{
    public int clickNum;
    public int currentClickNum;
    public int feverGauge;

    public ClickerStateData(int clickNum, int currentClickNum, int feverGauge)
    {
        this.clickNum = clickNum;
        this.currentClickNum = currentClickNum;
        this.feverGauge = feverGauge;
    }
}

[System.Serializable]
public class ClickerColorData
{
    public Color currentColor;
    public List<Color> buttonColors;

    public ClickerColorData(Color currentColor, List<Color> buttonColors)
    {
        this.currentColor = currentColor;
        this.buttonColors = buttonColors;
    }
}

[System.Serializable]
public class ClickerRobotData
{
    public List<int> robotClickAmounts;
    public List<int> robotMaxClicks;

    public ClickerRobotData(List<int> robotClickAmounts, List<int> robotMaxClicks)
    {
        this.robotClickAmounts = robotClickAmounts;
        this.robotMaxClicks = robotMaxClicks;
    }
}

[System.Serializable]
public class ClickerWorldData
{
    public List<Color> worldColors;
    public List<Image> backgrounds;

    public ClickerWorldData(List<Color> worldColors, List<Image> backgrounds)
    {
        this.worldColors = worldColors;
        this.backgrounds = backgrounds;
    }
}

public class ClickerDataManager
{
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
