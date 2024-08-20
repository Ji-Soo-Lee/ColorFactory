using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUIManager : MonoBehaviour
{
    public ClickerGameManager clickerGM;

    public MainButton mainButton;
    public Image mainButtonSprite;
    public Image backgroundButtonSprite;
    public Image feverGaugeImage;

    public List<Image> backgrounds = new List<Image>();
    public List<Sprite> feverGaugeSprites;

    public Color currentColor { get; private set; }
    private Coroutine currentButtonColorCoroutine;

    public void InitColors()
    {
        if (clickerGM.buttonColors.Count > 0)
        {
            currentColor = clickerGM.buttonColors[0];
            mainButtonSprite.color = currentColor;
        }
    }

    public void SetFeverGaugeSprite(int feverGauge)
    {
        if (feverGauge < 0 || feverGauge >= feverGaugeSprites.Count) return;
        feverGaugeImage.sprite = feverGaugeSprites[feverGauge];
    }

    public void SetButtonColor(Color color)
    {
        currentColor = color;
        mainButtonSprite.color = color;
    }

    public void UpdateButtonColor(float duration = 1.0f)
    {
        Color targetColor = clickerGM.CalculateTargetColor();

        if (currentButtonColorCoroutine != null) StopCoroutine(currentButtonColorCoroutine);
        currentButtonColorCoroutine = StartCoroutine(GraduallyChangeColor(mainButtonSprite, targetColor, duration));
    }

    public IEnumerator GraduallyChangeColor(Image img, Color targetColor, float transitionDuration)
    {
        float elapsedTime = 0;
        Color startColor = img.color;

        while (elapsedTime < transitionDuration)
        {
            currentColor = Color.Lerp(startColor, targetColor, elapsedTime / transitionDuration);
            img.color = currentColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure Target Color
        currentColor = targetColor;
        img.color = currentColor;
    }
}