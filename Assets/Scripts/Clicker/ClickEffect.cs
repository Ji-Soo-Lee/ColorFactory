using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public GameObject pongPrefab;
    public GameObject canvas;
    public float growDuration = 1.0f;
    public Vector3 maxScale = new Vector3 (5f, 5f, 5f);
    public float startScale = 0.1f;

    public void SpawnAndGrowEffect()
    {
        GameObject effect = Instantiate(pongPrefab, new Vector3(30f, 240f, 0f), Quaternion.identity);
        effect.transform.SetParent(canvas.transform, false);

        StartCoroutine(ScaleOverTime(effect));
    }

    IEnumerator ScaleOverTime(GameObject obj)
    {
        float currentTime = 0;
        Vector3 originalScale = obj.transform.localScale;

        while (currentTime < growDuration)
        {
            currentTime += Time.deltaTime;
            float scale = Mathf.Lerp(startScale, maxScale.x, currentTime / growDuration);
            obj.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        Destroy(obj);  // Remove object after it reaches maximum size
    }
}
