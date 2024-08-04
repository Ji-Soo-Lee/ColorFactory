using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DummySaveData
{
    public List<int> testDataList = new List<int>();

    public int dummyHP;
}

public class DummyDataSaver : MonoBehaviour
{
    void Start()
    {
        // Create Save Data
        DummySaveData dummySaveData = new DummySaveData();
        for (int i = 0; i < 10; i++)
        {
            dummySaveData.testDataList.Add(i);
        }
        dummySaveData.dummyHP = 15;

        // Save
        DataManager.SaveJSON<DummySaveData>(dummySaveData, "dummy_save_data");

        // Load
        DummySaveData savedData = DataManager.LoadJSON<DummySaveData>("dummy_save_data");

        // Check
        for (int i = 0; i < 10; i++)
        {
            UnityEngine.Debug.Log(savedData.testDataList[i]);
        }
        UnityEngine.Debug.Log("Saved HP : " + savedData.dummyHP.ToString());
    }
}
