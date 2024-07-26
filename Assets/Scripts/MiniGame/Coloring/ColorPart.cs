using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPart : MonoBehaviour
{
    ColoringGameManager game;
    SpriteRenderer sprite;
    void Start()
    {
        this.game = ColoringGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
    }
    void OnMouseDown()
    {
        this.sprite.color = this.game.now_color;
    }
}
