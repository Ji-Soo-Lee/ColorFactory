using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPart : MonoBehaviour
{
    ColoringGameManager game;
    SpriteRenderer sprite;
    Color answer;//이 조각의 정답
    void Start()
    {
        this.game = ColoringGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
    }
    public IEnumerator SetAnswer(Color color)
    {//이 조각의 정답에 해당하는 색.
        this.answer = color;
        yield return null;
        this.sprite.color = color;
        yield return new WaitForSeconds(5.0f);
        this.sprite.color = Color.clear;
    }//색을 보여준 후 5초 후 숨기기.
    void OnMouseDown()
    {//조각을 클릭하면 해당 색으로 바뀐다.
        if(this.game.playable)
        {
            this.sprite.color = this.game.now_color;
        }
    }
    public bool verdict()
    {//이 조각에 답을 바르게 했는지 확인하기
        return (this.sprite.color == this.answer);
    }
}
