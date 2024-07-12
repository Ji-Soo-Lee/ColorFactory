using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    GameManager game;
    int answer = 0, selected = 0;
    void Start()
    {
        this.game = GameManager.game;
    }
    public void assign_color(int x, Material m, bool apply)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = m;
        if(apply==true)
        {
            this.answer = x;
        }
    }
    void OnMouseDown()
    {
        if(game.playable==true)
        {//�÷��̾ ���ڸ� Ŭ���ϴ� ���
            this.selected = game.selected;
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material = game.color[this.selected];
        }
    }
    public bool verdict()
    {//�� ��Ͽ� ���� �°� �ߴ��� Ȯ���ϱ�
        return (this.answer == this.selected);
    }
}
