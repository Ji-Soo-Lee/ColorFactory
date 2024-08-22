using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    BrainGameManager game;
    SpriteRenderer sprite;
    Color answer;
    bool clicked = false;//�� ��Ͽ� ���� �ߴ���
    void Start()
    {
        this.game = BrainGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
    }
    public IEnumerator assign_color(Color color)
    {//����� �� �����ֱ�. ���� �̸� ���� ���� 1�� �� ���� �����ְ�, 3�� �� ���� �����.
        this.answer = color;
        yield return new WaitForSeconds(1.0f);
        this.sprite.color = color;
        yield return new WaitForSeconds(3.0f);
        this.sprite.color = Color.gray;
    }
    void OnMouseDown()
    {
        if(game.playable && !clicked)
        {//�÷��̾ ���ڸ� Ŭ���ϴ� ���
            this.clicked = true;
            this.sprite.color = this.game.now_color;
            this.game.verdict(this.sprite.color == this.answer);
        }
    }
    public bool verdict()
    {//�� ��Ͽ� ���� �ٸ��� �ߴ��� Ȯ���ϱ�
        return (this.answer == this.sprite.color);
    }
}
