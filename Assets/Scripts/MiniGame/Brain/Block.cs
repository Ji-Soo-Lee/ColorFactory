using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    BrainGameManager game;
    SpriteRenderer sprite;
    Color answer;
    void Start()
    {
        this.game = BrainGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
    }
    public IEnumerator assign_color(Color color)
    {//블록의 색 보여주기. 답을 미리 정한 다음 1초 후 색을 보여주고, 3초 후 색을 숨긴다.
        this.answer = color;
        yield return new WaitForSeconds(1.0f);
        this.sprite.color = color;
        yield return new WaitForSeconds(3.0f);
        this.sprite.color = Color.white;
    }
    void OnMouseDown()
    {
        if(game.playable)
        {//플레이어가 상자를 클릭하는 경우
            this.sprite.color = this.game.now_color;
        }
    }
    public bool verdict()
    {//이 블록에 답을 바르게 했는지 확인하기
        return (this.answer == this.sprite.color);
    }
}
