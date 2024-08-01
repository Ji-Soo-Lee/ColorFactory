using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainProblemGenerator : MonoBehaviour
{
    BrainGameManager game;
    public GameObject blockPrefab;
    public GameObject[] palette;
    int row = 2;//행의 수
    int column = 2;//열의 수
    int kind = 3;//색의 수
    Color[] colors = new Color[3] { Color.red, Color.green, Color.blue };

    void Start()
    {
        this.game = BrainGameManager.game;
        game.new_problem += InitiateProblem;
        game.stop_problem += stop_problem;
        for (int i=0; i<this.kind; i++)
        {
            Button button = this.palette[i].GetComponent<Button>();
            Color color = this.colors[i];
            ColorBlock cb = button.colors;
            cb.normalColor = color; cb.highlightedColor = color; cb.selectedColor = color;
            button.colors = cb;
            button.onClick.AddListener(() => change_color(color));
        }
    }
    void change_color(Color x)
    {//버튼에 넣는 함수. 현재 색 바꾸기.
        this.game.now_color = x;
    }
    public void InitiateProblem()
    {//새 문제를 만들 준비하기
        StartCoroutine(ProblemCycle());
    }
    IEnumerator ProblemCycle()
    {//한 문제를 만드는 사이클
        yield return new WaitForSeconds(2.0f);
        prepare_problem();
        yield return new WaitForSeconds(4.0f);
        game.playable = true;
    }
    void prepare_problem()
    {//블록 준비하기
        for (int i = 0; i < this.row; i++)
        {//블록들을 설치한다.
            for (int j = 0; j < this.column; j++)
            {
                Vector3 position = new Vector3((-0.25f * (this.column - 1)) + j * 0.5f, (0.25f * (this.row - 1)) - i * 0.5f, 0);
                GameObject block = Instantiate(this.blockPrefab, position, Quaternion.identity);
                int x = Random.Range(0, this.kind);
                StartCoroutine(block.GetComponent<Block>().assign_color(this.colors[x]));
            }
        }
    }
    void difficulty_increase()
    {//난이도 올리기
        if(this.row==this.column)
        {
            this.column += 1;
        }
        else
        {
            this.row += 1;
        }
    }
    public void judge_answer()
    {//응답 채점하기
        if(game.playable)
        {
            game.playable = false;
            int cnt = 0;
            bool wrong = false;
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {
                Block block = x.GetComponent<Block>();
                bool correct = block.verdict();
                if (correct)
                {//맞은 것 하나당 1점
                    cnt += 1;
                }
                else
                {//틀린 상자가 있으면 표시
                    wrong = true;
                }
                Destroy(x);
            }
            game.apply_result(cnt);
            if(!wrong)
            {
                difficulty_increase();
            }
        }
    }
    void stop_problem()
    {//현 문제 중지. 기존 문제를 버리고 다시 문제를 낼 때 사용한다.
        StopAllCoroutines();
    }
}
