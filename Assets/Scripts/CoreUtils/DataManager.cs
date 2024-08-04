using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;

public class DataManager
{
    private static string default_path = Application.persistentDataPath; // OR dataPath

    public static T LoadJSON<T>(string fileName)
    {
        return LoadJSON<T>(fileName, default_path);
    }

    public static T LoadJSON<T>(string fileName, string path)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            UnityEngine.Debug.LogWarning("LoadJSON: fileName is null or empty");
            return default(T);
        }

        if (fileName.Substring(fileName.Length - 5) != ".json")
        {
            fileName = fileName + ".json";
        }

        string filePath = path + '/' + fileName;
        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.LogWarning($"LoadJSON: File not found at {filePath}");
            return default(T);
        }

        try
        {
            string loadJson = File.ReadAllText(Path.Combine(path, fileName));
            T data = JsonUtility.FromJson<T>(loadJson);

            if (data == null)
            {
                return default(T);
            }

            // UnityEngine.Debug.Log("Successfully Loaded JSON at : " + filePath);

            return data;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"LoadJSON: Exception occurred - {e.Message}");
            return default(T);
        }
    }

    public static void SaveJSON<T>(T data, string fileName)
    {
        SaveJSON<T>(data, fileName, default_path);
    }

    public static void SaveJSON<T>(T data, string fileName, string path)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            UnityEngine.Debug.LogWarning("SaveJson: fileName is null or empty");
            return;
        }

        if (fileName.Substring(fileName.Length - 5) != ".json")
        {
            fileName = fileName + ".json";
        }

        string filePath = path + '/' + fileName;
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            // UnityEngine.Debug.Log("Successfully Saved JSON at : " + filePath);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"SaveJson: Exception occurred - {e.Message}");
        }
    }
}
