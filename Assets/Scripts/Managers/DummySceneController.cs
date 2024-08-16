using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummySceneController : MonoBehaviour
{
    [HideInInspector] public List<string> sceneNames = new List<string>()
        {"ClickerTestScene",
        "TanmakTestScene",
        "Brain",
        "Coloring",
        "Hoo",
        "dgim608",
        "Simon"};

    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
