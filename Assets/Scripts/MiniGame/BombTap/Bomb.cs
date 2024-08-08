using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    BombTapGameManager game;
    SpriteRenderer sprite;
    void Start()
    {
        this.game = BombTapGameManager.game;
    }
    public void assign_color(Color color)
    {//��ź�� �� �����ϱ�
        this.sprite = GetComponent<SpriteRenderer>();
        this.sprite.color = color;
    }
    void OnMouseDown()
    {//��ź Ŭ���ϱ�
        if(this.game.playable)
        {
            this.game.playable = false;
            this.game.judge(this.sprite.color);
            Destroy(gameObject);
        }
    }
}
