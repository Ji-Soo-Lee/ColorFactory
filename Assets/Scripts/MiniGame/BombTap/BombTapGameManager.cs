using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTapGameManager : MonoBehaviour
{
    public static BombTapGameManager game;
    public bool playable = false;

    public event Action<int> initiate;//Core와 연결

    Color target = Color.white;//현재의 목표 색
    int remain = 0;//남은 목표 폭탄 수

    int bomb = 4;//폭탄 배치 수(갈수록 늘어남)
    float limit = 5.0f;//제한 시간(갈수록 짧아짐)
    float elapsed = 0.0f;

    int score = 0;

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
        this.elapsed += Time.deltaTime;
    }
    IEnumerator new_problem(float time)
    {//새 문제 내기
        this.playable = false;
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {//기존에 있던 폭탄들은 모두 지운다.
            Destroy(x);
        }
        yield return new WaitForSeconds(time);
        this.initiate(this.bomb);
    }
    public void set_goal(Color color,int num)
    {//현재 목표 색과 목표 폭탄의 개수를 저장한다.
        this.target = color;
        this.remain = num;
    }
    public void judge(Color color)
    {
        if(this.target==color)
        {//맞는 색을 누른 경우
            this.score += 1; this.remain -= 1;
            if(this.remain<=0)
            {
                Debug.Log("맞았습니다!!");
                StartCoroutine(new_problem(0.6f));
            }
            else
            {
                this.playable = true;
            }
        }
        else
        {
            Debug.Log("틀렸습니다");
            StartCoroutine(new_problem(0.6f));
        }
    }
    
}
