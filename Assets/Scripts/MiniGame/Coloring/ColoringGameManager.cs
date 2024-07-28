using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ColoringGameManager game;
    public Color now_color;
    public bool playable = false;
    const int TOTAL = 3;//�� ���� ��
    int current = 0;//���� ����

    public event Action new_problem;//ProblemGenerator

    void Awake()
    {//���� �Ŵ����� ���� �̱������� �����ϱ�.
        if (game == null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void judge_answer()
    {//��� ä���ϱ�
        if (this.playable == true)
        {
            this.playable = false;
            this.current += 1;
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {//��� �׸� �������� player �±װ� �پ� �ִ�.
                ColorPart part = x.GetComponent<ColorPart>();
                //Debug.Log(part.verdict());
                Destroy(x);
            }
            GameObject.Find("Frame").GetComponent<SpriteRenderer>().sprite = null;
            if (this.current < TOTAL)
            {
                new_problem();
            }
            else
            {
                Debug.Log("������ �������ϴ�.");
            }
        }//ä���� ���� ���Ŀ��� �������� ġ���.
    }
}
