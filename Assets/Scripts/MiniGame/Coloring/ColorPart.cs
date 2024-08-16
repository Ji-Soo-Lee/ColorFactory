using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPart : MonoBehaviour
{
    ColoringGameManager game;
    SpriteRenderer sprite;
    Color answer;//�� ������ ����
    Color chosen;//�� ������ � ���� ĥ�ߴ���
    void Start()
    {
        this.game = ColoringGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
        this.sprite.color = Color.clear;
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
            this.chosen = this.game.now_color;
        }
    }
    public bool verdict()
    {//�� ������ ���� �ٸ��� �ߴ��� Ȯ���ϱ�
        bool result = (this.sprite.color == this.answer);
        return result;
    }
    public IEnumerator hint()
    {//�� ������ ��Ʈ �ֱ�
        this.sprite.color = this.answer;
        yield return new WaitForSeconds(3.0f);
        this.sprite.color = this.chosen;
    }
    public IEnumerator blink()
    {//�� ���� �����̱�
        this.sprite.color = Color.clear;
        yield return new WaitForSeconds(0.2f);
        this.sprite.color = this.chosen;
        yield return new WaitForSeconds(0.2f);
        this.sprite.color = Color.clear;
        yield return new WaitForSeconds(0.2f);
        this.sprite.color = this.chosen;
        yield return new WaitForSeconds(0.2f);
    }
}
