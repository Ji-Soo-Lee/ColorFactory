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
        {//플레이어가 상자를 클릭하는 경우
            this.selected = game.selected;
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material = game.color[this.selected];
        }
    }
    public bool verdict()
    {//이 블록에 답을 맞게 했는지 확인하기
        return (this.answer == this.selected);
    }
}
