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
        Debug.Log(this.x + " �� ������ Ŭ���߽��ϴ�.");
        this.sprite.color = Color.red;
    }
}
