using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    ProblemGenerator generator;
    public int selected = 0;//���� ���õ� ��
    public bool playable = false;//���� �÷��̾ ���� �� �� �ִ���
    public Material[] color;//���ӿ��� ����ϴ� ��
    public GameObject scoreboard;
    const int TOTAL = 10;//�� ���� ��
    int current = 0;
    int score = 0;//����
    int bonus = 0;//�ִ� ��� ��(���ʽ� ����)
    void Awake()
    {//���� �Ŵ����� ���� �̱������� �����ϱ�.
        if(game==null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        this.generator = GameObject.Find("ProblemGenerator").GetComponent<ProblemGenerator>();
    }
    public void apply_result(int correct)
    {//���� �ݿ��ϱ�
        this.current += 1;
        this.score += correct;//���� �� �ϳ��� 1��
        this.bonus = (this.bonus < correct ? correct : this.bonus);//�ִ� ��� ���� ���� ���� ���ʽ� ������ �ش�.
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.current < TOTAL)
        {//���� ���� ����
            this.generator.InitiateProblem();
        }
        else
        {
            Debug.Log("������ �������ϴ�.");
        }
    }
}
