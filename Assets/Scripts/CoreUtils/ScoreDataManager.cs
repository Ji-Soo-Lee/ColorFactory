using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreDataManager : MonoBehaviour
{
    public static ScoreDataManager Inst;
    public int finalMiniGameScore { get; private set; } = 0;
    public bool isMiniGameClear { get; private set; } = false;
    public string resultSceneName { get; private set; }

    public void SaveResult(int score, bool isClear)
    {
        finalMiniGameScore = score;
        isMiniGameClear = isClear;
        resultSceneName = SceneManager.GetActiveScene().name;
    }

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Jisoo")
        {
            finalMiniGameScore = 0;
            isMiniGameClear = false;
        }
    }
}
