using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPart : MonoBehaviour
{
    ColoringGameManager game;
    SpriteRenderer sprite;
    Color answer;//�� ������ ����
    void Start()
    {
        this.game = ColoringGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
    }
    public IEnumerator SetAnswer(Color color)
    {//�� ������ ���信 �ش��ϴ� ��.
        this.answer = color;
        yield return null;
        this.sprite.color = color;
        yield return new WaitForSeconds(5.0f);
        this.sprite.color = Color.clear;
    }//���� ������ �� 5�� �� �����.
    void OnMouseDown()
    {//������ Ŭ���ϸ� �ش� ������ �ٲ��.
        if(this.game.playable)
        {
            this.sprite.color = this.game.now_color;
        }
    }
    public bool verdict()
    {//�� ������ ���� �ٸ��� �ߴ��� Ȯ���ϱ�
        return (this.sprite.color == this.answer);
    }
}
