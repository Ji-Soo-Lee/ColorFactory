using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ColoringGameManager game;
    public Color now_color;
    public bool playable = false;
    const int TOTAL = 3;//총 문제 수
    int current = 0;//현재 문제

    public event Action new_problem;//ProblemGenerator

    void Awake()
    {//게임 매니저를 전역 싱글톤으로 설정하기.
        if (game == null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void judge_answer()
    {//답안 채점하기
        if (this.playable == true)
        {
            this.playable = false;
            this.current += 1;
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {//모든 그림 조각들은 player 태그가 붙어 있다.
                ColorPart part = x.GetComponent<ColorPart>();
                //Debug.Log(part.verdict());
                Destroy(x);
            }
            GameObject.Find("Frame").GetComponent<SpriteRenderer>().sprite = null;
            if (this.current < TOTAL)
            {
                new_problem();
            }
            else
            {
                Debug.Log("게임이 끝났습니다.");
            }
        }//채점이 끝난 이후에는 프레임을 치운다.
    }
}
