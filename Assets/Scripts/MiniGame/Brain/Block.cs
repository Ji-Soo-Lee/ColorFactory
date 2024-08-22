using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    BrainGameManager game;
    SpriteRenderer sprite;
    Color answer;
    bool clicked = false;
    void Start()
    {
        this.game = BrainGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
    }
    public IEnumerator assign_color(Color color)
    {
        this.answer = color;
        yield return new WaitForSeconds(0.3f);
        this.sprite.color = color;
        yield return new WaitForSeconds(3.0f);
        this.sprite.color = Color.gray;
        game.stageTimer.StartTimer();
        game.playable = true;
    }
    void OnMouseDown()
    {
        if(game.playable && !clicked)
        {
            this.clicked = true;
            this.sprite.color = this.game.now_color;
            this.game.verdict(this.sprite.color == this.answer);
        }
    }
    // public bool verdict()
    // {
    //     return (this.answer == this.sprite.color);
    // }
}
