using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ClickerGameManager clickerGM;
    [SerializeField] private ClickerUIManager clickerUIManager;
    private Vector3 originalScale;
    private Button selfbutton;
    public float duration = 1.0f;

    void Awake()
    {
        selfbutton = GetComponent<Button>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * 0.8f;
        clickerUIManager.backgroundButtonSprite.transform.localScale = originalScale * 0.8f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        clickerUIManager.backgroundButtonSprite.transform.localScale = originalScale;
    }

    public void OnClickButton()
    {
        clickerGM.IncrementClickCount(duration);
    }

    public void SetInteractive(bool isInteractive)
    {
        selfbutton.interactable = isInteractive;
    }
}