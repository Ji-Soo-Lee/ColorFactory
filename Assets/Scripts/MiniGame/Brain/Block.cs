using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    BrainGameManager game;
    Button button;
    Image sprite;
    Color answer;
    bool clicked = false;
    void Start()
    {
        this.game = BrainGameManager.game;
        button = GetComponent<Button>();
        sprite = button.GetComponent<Image>();
        button.onClick.AddListener(BlockClick);
    }
    public IEnumerator assign_color(Color color)
    {
        this.answer = color;
        yield return new WaitForSeconds(0.3f);
        sprite.color = color;
        yield return new WaitForSeconds(3.0f);
        sprite.color = Color.white;
        game.stageTimer.StartTimer();
    }

    public void BlockClick()
    {
        if(game.playable && !clicked)
        {
            this.clicked = true;
            sprite.color = this.game.now_color;
            this.game.verdict(this.sprite.color == this.answer);
        }
    }
    public void DeactivateButton()
    {
        button.interactable = false;
    }
}
