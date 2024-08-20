using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickEffect : MonoBehaviour
{
    public GameObject pongPrefab;
    public GameObject canvas;
    public float growDuration = 1.0f;
    public Vector3 maxScale = new Vector3 (3f, 3f, 3f);
    public float startScale = 0.1f;
    [SerializeField] private Vector3 center;

    public void SpawnAndGrowEffect(Color color)
    {
        GameObject effect = Instantiate(pongPrefab, center, Quaternion.identity);
        effect.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.3f);
        effect.transform.SetParent(canvas.transform, false);
        effect.transform.SetSiblingIndex(1);

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
