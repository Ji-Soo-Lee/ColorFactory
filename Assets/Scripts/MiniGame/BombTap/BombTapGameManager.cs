using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombTapGameManager : MonoBehaviour
{
    public static BombTapGameManager game;
    public bool playable = false;

    public event Action<int> initiate;//Core�� ����

    Color target = Color.white;//������ ��ǥ ��
    int remain = 0;//���� ��ǥ ��ź ��

    int bomb = 4;//��ź ��ġ ��(������ �þ)
    float limit = 5.0f;//���� �ð�(������ ª����)
    float clock;//���� �ð�

    int score = 0;//���� ����
    int life = 3;//���� ����

    public GameObject scoreboard;
    public GameObject timer;

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
    void Start()
    {//3�� ��ٸ��� ���� ����
        StartCoroutine(new_problem(3.0f));
    }

    // Update is called once per frame
    void Update()
    {
        this.clock -= Time.deltaTime;
        if(this.playable)
        {
            this.timer.GetComponent<TextMeshProUGUI>().text = this.clock.ToString("F2");
        }
        if ((this.clock <= 0.0f) && this.playable)
        {//�ð��� �ʰ��Ǿ��µ� ��ź�� �����ִ� ���
            Debug.Log("�ð� �ʰ�");
            this.timer.GetComponent<TextMeshProUGUI>().text = "0.00";
            this.life -= 1;
            StartCoroutine(new_problem(0.6f));
        }
    }
    IEnumerator new_problem(float time)
    {//�� ���� ����
        this.playable = false;
        if (this.life > 0)
        {//������ �������� ��� �� ������ ����.
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {//������ �ִ� ��ź���� ��� �����.
                Destroy(x);
            }
            yield return new WaitForSeconds(time);
            this.initiate(this.bomb);
            this.clock = this.limit;
        }
        else
        {//������ ��� �Ҿ��� ��� ������ ������.
            Debug.Log("������ �������ϴ�.");
        }
    }
    public void set_goal(Color color,int num)
    {//���� ��ǥ ���� ��ǥ ��ź�� ������ �����Ѵ�.
        this.target = color;
        this.remain = num;
    }
    public void judge(Color color)
    {//��ǥ�� ���� �ٸ��� Ŭ���ߴ��� Ȯ���ϱ�
        if(this.target==color)
        {//���� �´� ���
            this.score += 1; this.remain -= 1;
            this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
            if (this.remain<=0)
            {
                StartCoroutine(new_problem(0.6f));
            }
            else
            {
                this.playable = true;
            }
        }
        else
        {//���� Ʋ�� ���
            this.life -= 1;
            StartCoroutine(new_problem(0.6f));
        }
    }
}
