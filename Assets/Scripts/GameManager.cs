using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public void judge_answer()
    {//사용자가 입력한 답 평가하기
        if(this.playable==true)
        {
            this.current += 1; this.playable = false;
            int cnt = 0;
            bool wrong = false;
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject block in allBlocks)
            {//각 블록별로 색이 바르게 칠해졌는지 확인한다.
                bool result = block.GetComponent<Block>().verdict();
                if (result == true)
                {//맞은 것 하나당 1점
                    this.score += 1; cnt += 1;
                }
                if(result==false)
                {//틀린 상자가 있으면 표시
                    wrong = true;
                }
            }
            this.bonus = (this.bonus < cnt ? cnt : this.bonus);//최대 기억 수를 저장한다.
            this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
            if (wrong == false)
            {//모두 정답일 경우 난이도를 올린다.
                this.generator.difficulty_increase();
            }
            if(this.current<TOTAL)
            {//다음 문제 내기
                this.generator.reset_problem();
            }
            else
            {
                Debug.Log("게임이 끝났습니다.");
            }
        }
    }
}
