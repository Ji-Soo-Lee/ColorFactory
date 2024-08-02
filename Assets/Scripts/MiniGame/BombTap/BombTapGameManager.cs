using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTapGameManager : MonoBehaviour
{
    public static BombTapGameManager game;
    public bool playable = false;

    public event Action<int> initiate;//Core�� ����

    Color target = Color.white;//������ ��ǥ ��
    int remain = 0;//���� ��ǥ ��ź ��

    int bomb = 4;//��ź ��ġ ��(������ �þ)
    float limit = 5.0f;//���� �ð�(������ ª����)
    float elapsed = 0.0f;

    int score = 0;

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
        this.elapsed += Time.deltaTime;
    }
    IEnumerator new_problem(float time)
    {//�� ���� ����
        this.playable = false;
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {//������ �ִ� ��ź���� ��� �����.
            Destroy(x);
        }
        yield return new WaitForSeconds(time);
        this.initiate(this.bomb);
    }
    public void set_goal(Color color,int num)
    {//���� ��ǥ ���� ��ǥ ��ź�� ������ �����Ѵ�.
        this.target = color;
        this.remain = num;
    }
    public void judge(Color color)
    {
        if(this.target==color)
        {//�´� ���� ���� ���
            this.score += 1; this.remain -= 1;
            if(this.remain<=0)
            {
                Debug.Log("�¾ҽ��ϴ�!!");
                StartCoroutine(new_problem(0.6f));
            }
            else
            {
                this.playable = true;
            }
        }
        else
        {
            Debug.Log("Ʋ�Ƚ��ϴ�");
            StartCoroutine(new_problem(0.6f));
        }
    }
    
}
