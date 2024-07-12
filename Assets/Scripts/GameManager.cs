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
    public void judge_answer()
    {//����ڰ� �Է��� �� ���ϱ�
        if(this.playable==true)
        {
            this.current += 1; this.playable = false;
            int cnt = 0;
            bool wrong = false;
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject block in allBlocks)
            {//�� ��Ϻ��� ���� �ٸ��� ĥ�������� Ȯ���Ѵ�.
                bool result = block.GetComponent<Block>().verdict();
                if (result == true)
                {//���� �� �ϳ��� 1��
                    this.score += 1; cnt += 1;
                }
                if(result==false)
                {//Ʋ�� ���ڰ� ������ ǥ��
                    wrong = true;
                }
            }
            this.bonus = (this.bonus < cnt ? cnt : this.bonus);//�ִ� ��� ���� �����Ѵ�.
            this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
            if (wrong == false)
            {//��� ������ ��� ���̵��� �ø���.
                this.generator.difficulty_increase();
            }
            if(this.current<TOTAL)
            {//���� ���� ����
                this.generator.reset_problem();
            }
            else
            {
                Debug.Log("������ �������ϴ�.");
            }
        }
    }
}
