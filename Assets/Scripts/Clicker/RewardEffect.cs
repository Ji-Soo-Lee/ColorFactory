using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardEffect : MonoBehaviour
{
    public GameObject rewardPrefab;
    public GameObject canvas;
    public float disappearDuration = 0.8f;
    private float endAlpha = 0.1f;
    [SerializeField] private Vector3 center;

    public void DisplayRewardEffect(Color color)
    {
        GameObject effect = Instantiate(rewardPrefab);
        effect.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.8f);
        effect.transform.SetParent(canvas.transform, false);
        
        StartCoroutine(DisappearOverTime(effect));
    }

    IEnumerator DisappearOverTime(GameObject obj)
    {
        Debug.Log("ShrinkOverTime");

        float currentTime = 0;
        Color originalColor = obj.GetComponent<Image>().color;

        while (currentTime < disappearDuration)
        {
            currentTime += Time.deltaTime;
            float alpah = Mathf.Lerp(originalColor.a, endAlpha, currentTime / disappearDuration);
            obj.GetComponent<Image>().color = new Color (originalColor.r, originalColor.g, originalColor.b, alpah);
            yield return null;
        }

        Destroy(obj);  // Remove object after it reaches maximum size
    }
}
