using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class TanmakItemSpawner : MonoBehaviour
{
    TanmakGameManager tanmakGM;
    StopwatchManager stopwatch;

    public GameObject barrierPrefab;

    void Start()
    {
        tanmakGM = GameObject.FindObjectOfType<TanmakGameManager>();
        stopwatch = gameObject.AddComponent<StopwatchManager>();

        if (tanmakGM != null)
        {
            stopwatch.SetupStopwatchTik(10f, () => SpawnItem(barrierPrefab));
            tanmakGM.AddPauseHandler(stopwatch.PauseStopwatch);
            tanmakGM.AddResumeHandler(stopwatch.StartStopwatch);
        }

        stopwatch.StartStopwatch();
    }

    void SpawnItem(GameObject item)
    {
        GameObject obj = Instantiate(item);
        obj.transform.SetParent(tanmakGM.World.transform, false);

        Vector3 mapSize = tanmakGM.Map.transform.localScale;
        obj.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(-mapSize.x, mapSize.x) / 2,
            UnityEngine.Random.Range(-mapSize.y, mapSize.y) / 2,
            0
        );
    }
}
