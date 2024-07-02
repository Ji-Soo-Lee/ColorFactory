using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public int clickNum;
    public List<Color> buttonColors = new List<Color>();
    private Color targetColor;
    private Color currentColor;
    public float transitionDuration = 1.0f; // Duration in seconds over which color change occurs
    public GameObject ringPrefab;  // Ring 프리팹에 대한 참조
    public float growDuration = 1.0f;  // 성장하는 데 걸리는 시간
    public Vector3 maxScale = new Vector3(5f, 5f, 5f);  // 최대 크기
    public float startScale = 0.1f;  // 시작 크기

    // Start is called before the first frame update
    void Start()
    {   
        clickNum = 0;
        currentColor = buttonColors[clickNum];
        targetColor = currentColor;
        gameObject.GetComponent<Renderer>().material.color = currentColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {  
            clickNum++;
            targetColor = buttonColors[clickNum % buttonColors.Count];
            StopCoroutine("ChangeColor"); // Stop the current coroutine if running
            StartCoroutine("ChangeColor"); // Start the color transition coroutine

            StopCoroutine("ShakeButton"); // Stop the current coroutine if running
            StartCoroutine("ShakeButton"); // Start the shake coroutine

            SpawnAndGrowRing(targetColor);
        }

        // Continuously update the color to smoothly transition to the target color
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime / transitionDuration);
        gameObject.GetComponent<Renderer>().material.color = currentColor;
    }

    void SpawnAndGrowRing(Color color)
    {
        // GameObject ringH = Instantiate(ringPrefab, transform.position, Quaternion.Euler(90, 0, 0));
        // GameObject ringV = Instantiate(ringPrefab, transform.position, Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), 0));
        GameObject ringH = Instantiate(ringPrefab, transform.position, UnityEngine.Random.rotation);
        GameObject ringV = Instantiate(ringPrefab, transform.position, UnityEngine.Random.rotation);
        ringH.GetComponent<SpriteRenderer>().material.color = color;
        ringV.GetComponent<SpriteRenderer>().material.color = color;
        ringH.transform.localScale = Vector3.one * startScale;  // 시작 크기 설정
        ringV.transform.localScale = Vector3.one * startScale;  // 시작 크기 설정
        StartCoroutine(ScaleOverTime(ringH));
        StartCoroutine(ScaleOverTime(ringV));
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

    IEnumerator ChangeColor()
    {
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            currentColor = Color.Lerp(currentColor, targetColor, elapsedTime / transitionDuration);
            gameObject.GetComponent<Renderer>().material.color = currentColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentColor = targetColor; // Ensure the final color is set
    }
    
    public float shakeDuration = 0.3f; // Time the object will shake
    public float shakeMagnitude = 0.1f; // The magnitude of the shake

    IEnumerator ShakeButton()
    {
        float elapsedTime = 0;
        transform.position = Vector3.zero; // Ensure the object is in the starting position

        while (elapsedTime < shakeDuration)
        {
            transform.position = Random.insideUnitSphere * shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = Vector3.zero; // Ensure the object is reset
    }
}
