using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public int selected = 0;//현재 선택된 색
    public bool playable = false;//현재 플레이어가 답을 할 수 있는지
    public Material[] color;//게임에서 사용하는 색
    public GameObject scoreboard;
    int score = 0;
    float elapsed = 0.0f;
    bool start = false;
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
        Debug.Log("게임 준비");
    }
    void Update()
    {//타이머
        this.elapsed += Time.deltaTime;
        if(this.start==false && this.elapsed>=2.0f)
        {
            this.start = true;
            GameObject.Find("ProblemGenerator").GetComponent<ProblemGenerator>().make_problem();
        }
    }
    public void judge_answer()
    {//사용자가 입력한 답 평가하기
        if(this.playable==true)
        {
            this.playable = false;
            bool wrong = false;
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject block in allBlocks)
            {//각 블록별로 색이 바르게 칠해졌는지 확인한다.
                bool result = block.GetComponent<Block>().verdict();
                if (result == false)
                {//답이 틀린 경우
                    wrong = true;
                }
            }
            Debug.Log(!wrong);//답이 맞았는지 틀렸는지 표시
            this.score += (wrong ? 0 : 1);//정답일 경우 점수 부여
            this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
            reset_problem();
        }
    }
    void reset_problem()
    {//문제에 나온 모든 블록과 팔레트를 치운다.
        GameObject[] block = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] palette = GameObject.FindGameObjectsWithTag("Palette");
        GameObject[] obj = block.Concat(palette).ToArray();
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
        this.start = false; this.elapsed = 0.0f;//새 문제를 낼 준비를 한다.
    }
}
