using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUIManager : MonoBehaviour
{
    public ClickerGameManager clickerGM;
    public RobotManager robotManager;
    public RobotV2 robot;   

    public MainButton mainButton;
    public Image mainButtonSprite;
    public Image backgroundButtonSprite;
    public Image feverGaugeImage;
    public Image robotBatterySprite;

    public GameObject WorldPanel;
    public GameObject RobotPanel;
    public GameObject MiniGamePanel;

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

    public void DeactivatePanels()
    {
        WorldPanel.SetActive(false);
        RobotPanel.SetActive(false);
        MiniGamePanel.SetActive(false);
    }

    public void ToggleWorldPanel()
    {
        if (WorldPanel.activeSelf)
        {
            DeactivatePanels();
        }
        else
        {
            DeactivatePanels();
            WorldPanel.SetActive(true);
        }
    }

    public void ToggleRobotPanel()
    {
        if (RobotPanel.activeSelf)
        {
            DeactivatePanels();
        }
        else
        {
            DeactivatePanels();
            RobotPanel.SetActive(true);
        }
    }

    public void ToggleMiniGamePanel()
    {
        if (MiniGamePanel.activeSelf)
        {
            DeactivatePanels();
        }
        else
        {
            DeactivatePanels();
            MiniGamePanel.SetActive(true);
        }
    }

    public void SetFeverGaugeSprite(int feverGauge)
    {
        if (feverGauge < 0 || feverGauge >= feverGaugeSprites.Count) return;
        feverGaugeImage.sprite = feverGaugeSprites[feverGauge];
    }

    public void SetRobotBatterySprite(int robotBattery)
    {
        Debug.Log("SetRobotBatterySprite : " + robotBattery);
        if (robotBattery < 0) return;
        robotBatterySprite.fillAmount = (float)robotBattery / robot.MAX_ROBOT_BATTERY;
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