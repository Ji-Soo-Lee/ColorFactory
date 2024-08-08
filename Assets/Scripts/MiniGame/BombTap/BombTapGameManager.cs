using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombTapGameManager : MonoBehaviour
{
    public static BombTapGameManager game;
    public bool playable = false;

    public event Action<int> initiate;//Core와 연결

    Color target = Color.white;//현재의 목표 색
    int remain = 0;//남은 목표 폭탄 수

    int bomb = 4;//폭탄 배치 수(갈수록 늘어남)
    float limit = 5.0f;//제한 시간(갈수록 짧아짐)
    float clock;//남은 시간

    int score = 0;//현재 점수
    int life = 3;//남은 생명

    public GameObject scoreboard;
    public GameObject timer;

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
    void Start()
    {//3초 기다리고 게임 시작
        StartCoroutine(new_problem(3.0f));
    }

    // Update is called once per frame
    void Update()
    {
        this.clock -= Time.deltaTime;
        if(this.playable)
        {
            this.timer.GetComponent<TextMeshProUGUI>().text = this.clock.ToString("F2");
        }
        if ((this.clock <= 0.0f) && this.playable)
        {//시간이 초과되었는데 폭탄이 남아있는 경우
            Debug.Log("시간 초과");
            this.timer.GetComponent<TextMeshProUGUI>().text = "0.00";
            this.life -= 1;
            StartCoroutine(new_problem(0.6f));
        }
    }
    IEnumerator new_problem(float time)
    {//새 문제 내기
        this.playable = false;
        if (this.life > 0)
        {//생명이 남아있을 경우 새 문제를 낸다.
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {//기존에 있던 폭탄들은 모두 지운다.
                Destroy(x);
            }
            yield return new WaitForSeconds(time);
            this.initiate(this.bomb);
            this.clock = this.limit;
        }
        else
        {//생명을 모두 잃었을 경우 게임이 끝난다.
            Debug.Log("게임이 끝났습니다.");
        }
    }
    public void set_goal(Color color,int num)
    {//현재 목표 색과 목표 폭탄의 개수를 저장한다.
        this.target = color;
        this.remain = num;
    }
    public void judge(Color color)
    {//목표한 색을 바르게 클릭했는지 확인하기
        if(this.target==color)
        {//답이 맞는 경우
            this.score += 1; this.remain -= 1;
            this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
            if (this.remain<=0)
            {
                StartCoroutine(new_problem(0.6f));
            }
            else
            {
                this.playable = true;
            }
        }
        else
        {//답이 틀린 경우
            this.life -= 1;
            StartCoroutine(new_problem(0.6f));
        }
    }
}
