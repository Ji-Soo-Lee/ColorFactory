using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    BrainGameManager game;
    public Material blank;//���(��ĭ)
    int answer = 0, selected = 0;
    void Start()
    {
        this.game = BrainGameManager.game;
    }
    public IEnumerator assign_color(int x, Material m)
    {//����� �� �����ֱ�. 3�� �� ���� �����.
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = m;
        this.answer = x;
        yield return new WaitForSeconds(3.0f);
        hide_color();
    }
    void hide_color()
    {//�� �����
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = this.blank;
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
