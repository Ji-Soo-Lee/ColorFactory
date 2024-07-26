using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPart : MonoBehaviour
{
    public int x;
    SpriteRenderer sprite;
    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
    }
    void OnMouseDown()
    {
        Debug.Log(this.x + " 번 영역을 클릭했습니다.");
        this.sprite.color = Color.red;
    }
}
