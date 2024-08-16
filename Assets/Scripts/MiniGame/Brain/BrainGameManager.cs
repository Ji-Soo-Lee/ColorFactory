using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BrainGameManager : StageManager
{
    public static BrainGameManager game;
    public bool playable = false;//현재 플레이어가 답을 할 수 있는지
    bool pause = false;
    public Color now_color;

    public GameObject scoreboard;
    public GameObject pausepanel;
    public GameObject questionboard;
    public GameObject submitButton;

    public event Action new_problem;//ProblemGenerator과 느슨히 연결됨.
    public event Action stop_problem;

    const int TOTAL = 10;//총 문제 수
    int current = 1;
    int score = 0;//점수
    int bonus = 0;//최대 기억 수(보너스 점수)
    protected override void Awake()
    {//게임 매니저를 전역 싱글톤으로 설정하기.
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
    {//점수 반영하기
        this.current += 1;
        this.score += correct;//맞은 것 하나당 1점
        this.bonus = (this.bonus < correct ? correct : this.bonus);//최대 기억 수는 수에 따라 보너스 점수를 준다.
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.current <= TOTAL)
        {//다음 문제 내기
            this.questionboard.GetComponent<TextMeshProUGUI>().text = "Q " + this.current + "/" + TOTAL;
            new_problem();
        }
        else
        {//게임 종료
            // StartCoroutine(Gameover());
            EndGame();
        }
    }
    public void toggle_player(bool toggle)
    {//플레이어의 입력 차례인지 체크하기
        this.playable = toggle;
        this.submitButton.SetActive(toggle);
    }
    public void toggle_pause()
    {//게임 일시정지
        this.pause = !(this.pause);
        this.pausepanel.SetActive(this.pause);
        toggle_player(false);
        stop_problem();//게임을 일시정지한 경우 문제 출제를 중지한다.
        if(this.pause==false)
        {//게임을 다시 켠 경우 문제를 다시 낸다.
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {
                Destroy(x);
            }
            new_problem();
        }
    }
    IEnumerator Gameover()
    {//게임 종료. 기록을 저장하고 다음 화면에서 결과를 보여줌.
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
