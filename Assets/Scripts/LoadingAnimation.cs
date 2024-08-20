using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour
{
    public Image loadingImage;

    private float timer = 0f;
    private float duration = 5f;
    
    void Update()
    {
        timer = (timer + Time.unscaledDeltaTime) % duration;
        loadingImage.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 360f, timer / duration));
    }
}
