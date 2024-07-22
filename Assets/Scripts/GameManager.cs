using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    ProblemGenerator generator;
    public int selected = 0;//현재 선택된 색
    public bool playable = false;//현재 플레이어가 답을 할 수 있는지
    public Material[] color;//게임에서 사용하는 색
    public GameObject scoreboard;
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
    void Start()
    {
        this.generator = GameObject.Find("ProblemGenerator").GetComponent<ProblemGenerator>();
    }
    public void apply_result(int correct)
    {//점수 반영하기
        this.current += 1;
        this.score += correct;//맞은 것 하나당 1점
        this.bonus = (this.bonus < correct ? correct : this.bonus);//최대 기억 수는 수에 따라 보너스 점수를 준다.
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        if (this.current < TOTAL)
        {//다음 문제 내기
            this.generator.InitiateProblem();
        }
        else
        {//게임 종료
            StartCoroutine(Gameover());
        }
    }
    IEnumerator Gameover()
    {//한 문제를 만드는 사이클
        PlayerPrefs.SetInt("score", this.score);
        PlayerPrefs.SetInt("bonus", this.bonus);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("BrainResult");
    }
}
