using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    BrainGameManager game;
    public Material blank;//흰색(빈칸)
    int answer = 0, selected = 0;
    void Start()
    {
        this.game = BrainGameManager.game;
    }
    public IEnumerator assign_color(int x, Material m)
    {//블록의 색 보여주기. 3초 후 색을 숨긴다.
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = m;
        this.answer = x;
        yield return new WaitForSeconds(3.0f);
        hide_color();
    }
    void hide_color()
    {//색 숨기기
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = this.blank;
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
