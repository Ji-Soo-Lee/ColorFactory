using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public int selected = 0;//현재 선택된 색
    public bool playable = false;//현재 플레이어가 답을 할 수 있는지
    public Material[] color;//게임에서 사용하는 색
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

    // Update is called once per frame
    void Update()
    {
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
            {//블록들의 색을 제거한다.
                bool result = block.GetComponent<Block>().verdict();
                if (result == false)
                {//답이 틀린 경우
                    wrong = true;
                }
            }
            Debug.Log(!wrong);//답이 맞았는지 틀렸는지 표시
            reset_problem();
        }
    }
    void reset_problem()
    {//문제에 나온 모든 블록과 팔레트를 치운다.
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in allBlocks)
        {//블록들의 색을 제거한다.
            Destroy(block);
        }
        GameObject[] allPalettes = GameObject.FindGameObjectsWithTag("Palette");
        foreach (GameObject palette in allPalettes)
        {//팔레트를 제거한다.
            Destroy(palette);
        }
        this.start = false; this.elapsed = 0.0f;//새 문제를 낼 준비를 한다.
    }
}
