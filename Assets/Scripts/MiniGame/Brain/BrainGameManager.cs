using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BrainGameManager : MonoBehaviour
{
    public static BrainGameManager game;
    BrainProblemGenerator generator;
    public bool playable = false;//���� �÷��̾ ���� �� �� �ִ���
    public Color now_color;
    public GameObject[] palette;
    public GameObject scoreboard;

    public event Action new_problem;//ProblemGenerator�� ������ �����.

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
    public void apply_result(int correct)
    {//���� �ݿ��ϱ�
        this.current += 1;
        this.score += correct;//���� �� �ϳ��� 1��
        this.bonus = (this.bonus < correct ? correct : this.bonus);//�ִ� ��� ���� ���� ���� ���ʽ� ������ �ش�.
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.current < TOTAL)
        {//���� ���� ����
            new_problem();
        }
        else
        {//���� ����
            StartCoroutine(Gameover());
        }
    }
    IEnumerator Gameover()
    {//���� ����. ����� �����ϰ� ���� ȭ�鿡�� ����� ������.
        PlayerPrefs.SetInt("score", this.score);
        PlayerPrefs.SetInt("bonus", this.bonus);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("BrainResult");
    }
}
