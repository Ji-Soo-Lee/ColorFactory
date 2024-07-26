using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummySceneController : MonoBehaviour
{
    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
