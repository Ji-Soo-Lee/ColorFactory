using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DummySceneController : MonoBehaviour
{
    [HideInInspector] public static List<string> sceneNames = new List<string>()
        {"ClickerTestScene",
        "TanmakTestScene",
        "Brain",
        "Coloring",
        "MixFinal",
        "BombTap",
        "dgim608"
        };

    private static string nextSceneName;
    private float minimumDuration = 2f;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "LoadingScene"
            && nextSceneName != null && sceneNames.Contains(nextSceneName))
        {
            StartCoroutine(LoadSceneProcess(nextSceneName));
        }
    }

    public static void LoadScene(int sceneID)
    {
        nextSceneName = SceneManager.GetSceneByBuildIndex(sceneID).name;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadSceneProcess(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        UnityEngine.Debug.Log("Loading Scene : " + sceneName);

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.unscaledDeltaTime;
            if (op.progress >= 0.9f && timer >= minimumDuration)
            {
                op.allowSceneActivation = true;
                yield break;
            }

            // Loading Bar?
            //float timer = 0f;
            //if(op.progress < 0.9f)
            //{
            //    ProgressBar.fillAmount = op.progress;
            //}
            //else
            //{
            //    timer += Time.unscaledDeltaTime;
            //    progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
            //    if(progressBar.fillAmount >= 1f)
            //    {
            //        op.allowSceneActivation = true;
            //        yield break;
            //    }
            //}
        }
    }
}
