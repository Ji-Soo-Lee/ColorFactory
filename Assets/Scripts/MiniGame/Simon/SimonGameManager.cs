using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonGameManager : MonoBehaviour
{
    public static SimonGameManager game;
    public bool playable = false;

    public GameObject[] buttons;
    Bulb bulb;

    Color[] kind = new Color[4] { Color.red, Color.yellow, Color.green, Color.blue };
    Color[] answer = new Color[100];
    int level = 1;//현재 레벨. 레벨의 수 만큼의 색을 외워야 한다.
    int current = 0;

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
        this.bulb = GameObject.Find("Bulb").GetComponent<Bulb>();
        for (int i = 0; i < this.buttons.Length; i++)
        {//각 버튼에 함수 넣기
            Button button = this.buttons[i].GetComponent<Button>();
            Color color = this.kind[i];
            button.onClick.AddListener(() => check_color(color));
        }
        toggle_player(false);
        StartCoroutine(show_problem(1.0f));
    }
    IEnumerator show_problem(float time)
    {//문제를 보여준다.
        yield return new WaitForSeconds(time);
        for (int i = 0; i < this.level; i++)
        {
            Color now;
            if (i == (this.level - 1))
            {//새로운 색 고르고 저장하기
                now = this.kind[Random.Range(0, 4)];
                this.answer[i] = now;
            }
            else
            {//이전에 지나갔던 색 보여주기
                now = this.answer[i];
            }
            StartCoroutine(this.bulb.blink(now));
            yield return new WaitForSeconds(0.7f);
        }
        toggle_player(true);
        this.current = 0;
    }
    void toggle_player(bool toggle)
    {//플레이어의 입력 차례인지 체크하기
        this.playable = toggle;
        for(int i=0; i<this.buttons.Length; i++)
        {
            Button button = this.buttons[i].GetComponent<Button>();
            button.interactable = toggle;
        }
    }
    void check_color(Color color)
    {//순서에 맞는 색을 눌렀는지 확인하기
        if(this.playable)
        {
            StopAllCoroutines();
            StartCoroutine(this.bulb.blink(color));
            if (this.answer[this.current] == color)
            {//순서에 맞는 색을 누른 경우
                this.current += 1;
                if (this.current >= this.level)
                {//현재 레벨에서 모두 맞은 경우
                    toggle_player(false);
                    this.current = 0; this.level += 1;
                    if (this.level <= 100)
                    {//다음 레벨로 넘어가기
                        StartCoroutine(show_problem(1.0f));
                    }
                }
            }
            else
            {//순서에 맞지 않는 색을 누를 경우 즉시 게임을 종료한다.
                toggle_player(false);
                Debug.Log(this.level + " 에서 실패했습니다");
            }
        }
    }
}
