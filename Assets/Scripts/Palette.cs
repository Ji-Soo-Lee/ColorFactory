using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    GameManager game;
    int color;//�� �ȷ�Ʈ�� ����ϴ� ��
    void Start()
    {
        this.game = GameManager.game;
    }
    public void assign_color(int x,Material m)
    {
        this.color = x;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = m;
    }
    void OnMouseDown()
    {
        game.selected = this.color;
    }
}
