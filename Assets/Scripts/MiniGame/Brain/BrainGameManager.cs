using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BrainGameManager : StageManager
{
    public static BrainGameManager game;
    public bool playable = false;//���� �÷��̾ ���� �� �� �ִ���
    bool pause = false;
    public Color now_color;

    public GameObject scoreboard;
    public GameObject pausepanel;
    public GameObject questionboard;

    public event Action new_problem;//ProblemGenerator�� ������ �����.
    public event Action stop_problem;
    public event Action difficulty_increase;
    const int TOTAL = 10;//�� ���� ��

    public int remain;
    int current = 1;
    int score = 0;//����
    int add = 0;
    int bonus = 0;//�ִ� ��� ��(���ʽ� ����)
    bool wrong = false;
    protected override void Awake()
    {//���� �Ŵ����� ���� �̱������� �����ϱ�.
        base.Awake();

        if(game==null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected override void Start()
    {
        base.Start();
        new_problem();
    }
    public void verdict(bool correct)
    {//Ŭ�� ����
        this.remain -= 1;
        if(correct)
        {//����
            this.score += 1; this.add += 1;
        }
        else
        {//����
            this.wrong = true;
        }
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.remain<=0)
        {
            apply_result();
        }
    }
    public void apply_result()
    {//���� �ݿ��ϱ�
        toggle_player(false);
        stop_problem();
        this.current += 1;
        this.bonus = (this.bonus < this.add ? this.add : this.bonus);//�ִ� ��� ���� ���� ���� ���ʽ� ������ �ش�.
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.current <= TOTAL)
        {//���� ���� ����
            if(!this.wrong)
            {//���� �������� ��� ������ ��� ���̵� �ø���
                difficulty_increase();
            }
            this.add = 0; this.wrong = false;
            this.questionboard.GetComponent<TextMeshProUGUI>().text = "STAGE" + this.current;
            new_problem();
        }
        else
        {//���� ����
            // StartCoroutine(Gameover());
            EndGame();
        }
    }
    public void toggle_player(bool toggle)
    {//�÷��̾��� �Է� �������� üũ�ϱ�
        this.playable = toggle;
    }
    public void toggle_pause()
    {//���� �Ͻ�����
        this.pause = !(this.pause);
        this.pausepanel.SetActive(this.pause);
        toggle_player(false);
        stop_problem();//������ �Ͻ������� ��� ���� ������ �����Ѵ�.
        if(this.pause==false)
        {//������ �ٽ� �� ��� ������ �ٽ� ����.
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {
                Destroy(x);
            }
            new_problem();
        }
    }
    IEnumerator Gameover()
    {//���� ����. ����� �����ϰ� ���� ȭ�鿡�� ����� ������.
        PlayerPrefs.SetInt("score", this.score);
        PlayerPrefs.SetInt("bonus", this.bonus);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("BrainResult");
    }

    public override void EndGame()
    {
        base.EndGame();
        commonPopupUIManager.SetResultText(0, this.score, true);
    }
}
