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
    public bool playable = false;//현재 플레이어가 답을 할 수 있는지
    public Color now_color;
    public GameObject[] palette;
    public GameObject scoreboard;

    public event Action new_problem;//ProblemGenerator과 느슨히 연결됨.

    const int TOTAL = 10;//총 문제 수
    int current = 0;
    int score = 0;//점수
    int bonus = 0;//최대 기억 수(보너스 점수)
    void Awake()
    {//게임 매니저를 전역 싱글톤으로 설정하기.
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
    {//점수 반영하기
        this.current += 1;
        this.score += correct;//맞은 것 하나당 1점
        this.bonus = (this.bonus < correct ? correct : this.bonus);//최대 기억 수는 수에 따라 보너스 점수를 준다.
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.current < TOTAL)
        {//다음 문제 내기
            new_problem();
        }
        else
        {//게임 종료
            StartCoroutine(Gameover());
        }
    }
    IEnumerator Gameover()
    {//게임 종료. 기록을 저장하고 다음 화면에서 결과를 보여줌.
        PlayerPrefs.SetInt("score", this.score);
        PlayerPrefs.SetInt("bonus", this.bonus);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("BrainResult");
    }
}
