using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreDataManager : MonoBehaviour
{
    public static ScoreDataManager Inst;
    // [HideInInspector]
    public int finalMiniGameScore;
    // [HideInInspector]
    public string resultSceneName;

    public void SaveResult(int score)
    {
        finalMiniGameScore = score;
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
        }
    }
}
