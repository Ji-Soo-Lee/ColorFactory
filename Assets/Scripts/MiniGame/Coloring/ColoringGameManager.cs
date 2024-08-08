using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColoringGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ColoringGameManager game;
    public Color now_color;
    public bool playable = false;
    const int TOTAL = 3;//총 문제 수
    int current = 1;//현재 문제
    int hint_used = 0;//총 사용한 힌트 수
    float elapsed = 0.0f;
    public event Action new_problem;//ProblemGenerator

    int score = 5;

    AudioSource[] sound;
    public GameObject questionboard;
    public GameObject DummyEndGamePannel;

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
    {
        this.sound = GetComponents<AudioSource>();
    }
    public void StartGame()
    {//problem generator의 준비가 끝나면 게임 시작하기
        new_problem();
    }
    public void toggle_player(bool toggle)
    {
        this.playable = toggle;
    }
    public void hint()
    {//힌트 사용하기
        if (!this.playable)
        {
            this.hint_used += 1;
            StartCoroutine(hint_procedure());
        }
    }
    IEnumerator hint_procedure()
    {//잠깐 전체 답을 3초간 봤다가 사라진다.
        toggle_player(false);
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {
            ColorPart part = x.GetComponent<ColorPart>();
            if (part != null)
            {
                StartCoroutine(part.hint());
            }
        }
        yield return new WaitForSeconds(3.0f);
        toggle_player(true);
    }
    public void judge_answer()
    {//답안 채점하기
        if (this.playable == true)
        {
            bool wrong = false;
            StopAllCoroutines();
            toggle_player(false);
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {//모든 그림 조각들, 팔레트는 player 태그가 붙어 있다.
                ColorPart part = x.GetComponent<ColorPart>();
                if(part!=null)
                {//그림 조각 확인하기
                    bool result = part.verdict();
                    if(!result)
                    {
                        StartCoroutine(part.blink());
                        wrong = true;
                    }
                }
            }
            if(!wrong)
            {//모두 맞는 경우
                this.current += 1;
                this.sound[0].Play();
                foreach (GameObject x in obj)
                {//기존의 문제를 치운다.
                    Destroy(x);
                }
                GameObject.Find("Frame").GetComponent<SpriteRenderer>().sprite = null;
                if (this.current < TOTAL)
                {//새 문제 내기
                    this.questionboard.GetComponent<TextMeshProUGUI>().text = "Q " + this.current + "/" + TOTAL;
                    new_problem();
                }
                else
                {//게임 종료하기
                    EndGame();
                }
            }
            else
            {//틀린 답이 있는 경우
                this.sound[1].Play();
                toggle_player(true);
            }
        }//채점이 끝난 이후에는 프레임을 치운다.
    }
    void Update()
    {//시간 세기. 문제를 푸는 중일때만 센다.
        if(this.playable)
        {
            this.elapsed += Time.deltaTime;
        }
    }
    public void EndGame()
    {
        ScoreDataManager.Inst.SaveResult(score);
        DummyEndGamePannel.SetActive(true);
    }
}
