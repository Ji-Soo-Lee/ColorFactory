using UnityEngine;
using UnityEngine.EventSystems;

public class TanmakJoystick : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool isJoystickMoving;
    [HideInInspector]
    public Vector3 dir;

    RectTransform lever;
    RectTransform rectTransform;
    float leverRange;

    void Start()
    {
        isJoystickMoving = false;
        lever = transform.GetChild(0).GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        leverRange = rectTransform.rect.width / rectTransform.localScale.x;
    }

    void SetDirection(PointerEventData eventData)
    {
        var inputDir = (eventData.position - rectTransform.anchoredPosition) / rectTransform.localScale.x;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        dir = clampedDir / leverRange;
        lever.anchoredPosition = clampedDir;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isJoystickMoving = true;
            SetDirection(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isJoystickMoving = false;
            SetDirection(eventData);
            lever.anchoredPosition = new Vector2(0.0f, 0.0f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isJoystickMoving = true;
        SetDirection(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDirection(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isJoystickMoving = false;
        SetDirection(eventData);
        lever.anchoredPosition = new Vector2(0.0f, 0.0f);
    }
}
