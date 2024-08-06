using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClickerData
{
    public int clickNum;
}

public class ClickerDataManager : MonoBehaviour
{
    public ButtonManager buttonManager;

    public void SaveData()
    {
        ClickerData data = new ClickerData();
        data.clickNum = buttonManager.clickNum;

        DataManager.SaveJSON<ClickerData>(data, "clicker_save_data");
    }

    public void LoadData()
    {
        ClickerData data = DataManager.LoadJSON<ClickerData>("clicker_save_data");
        if (data != null)
        {
            buttonManager.clickNum = data.clickNum;
            // currentClickNum, color
        }
    }

    void Awake()
    {
        LoadData();
    }
}
