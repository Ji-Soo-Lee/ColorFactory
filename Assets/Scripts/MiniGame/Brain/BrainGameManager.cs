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
    public GameObject submitButton;

    public event Action new_problem;//ProblemGenerator�� ������ �����.
    public event Action stop_problem;

    const int TOTAL = 10;//�� ���� ��
    int current = 1;
    int score = 0;//����
    int bonus = 0;//�ִ� ��� ��(���ʽ� ����)
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
    public void apply_result(int correct)
    {//���� �ݿ��ϱ�
        this.current += 1;
        this.score += correct;//���� �� �ϳ��� 1��
        this.bonus = (this.bonus < correct ? correct : this.bonus);//�ִ� ��� ���� ���� ���� ���ʽ� ������ �ش�.
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.current <= TOTAL)
        {//���� ���� ����
            this.questionboard.GetComponent<TextMeshProUGUI>().text = "Q " + this.current + "/" + TOTAL;
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
        this.submitButton.SetActive(toggle);
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
